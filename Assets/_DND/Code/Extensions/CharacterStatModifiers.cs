using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Photon.Pun.Simple;
using Photon.Pun.Simple.Pooling;

namespace DnD.Extensions
{
    [Serializable]
    public class CharacterStatModifiersAdditionalData
    {
        public int str, dex, con, lnt, wis, chr, xp, level;
        public float magDmg, xpRate;
        public string playerClass;

        public CharacterStatModifiersAdditionalData()
        {
            str = 1;
            dex = 1;
            con = 1;
            lnt = 1;
            wis = 1;
            chr = 1;
            xp = 0;
            level = 1;
            magDmg = 1f;
            xpRate = 1f;
            playerClass = "";
        }
    }
    public static class CharacterStatModifiersExtension
    {
        public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData> data =
            new ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData>();

        public static CharacterStatModifiersAdditionalData GetAdditionalData(this CharacterStatModifiers statModifiers)
        {
            return data.GetOrCreateValue(statModifiers);
        }

        public static void AddData(this CharacterStatModifiers statModifiers, CharacterStatModifiersAdditionalData value)
        {
            try
            {
                data.Add(statModifiers, value);
            }
            catch (Exception) { }
        }
    }

    [HarmonyPatch(typeof(CharacterStatModifiers), "ResetStats")]
    class CharacterStatModifiersPatchResetStats
    {
        private static void Prefix(CharacterStatModifiers __instance)
        {
            __instance.GetAdditionalData().str = 0;
            __instance.GetAdditionalData().dex = 0;
            __instance.GetAdditionalData().con = 0;
            __instance.GetAdditionalData().lnt = 0;
            __instance.GetAdditionalData().wis = 0;
            __instance.GetAdditionalData().chr = 0;
            __instance.GetAdditionalData().xp = 0;
            __instance.GetAdditionalData().level = 1;
            __instance.GetAdditionalData().magDmg = 1f;
            __instance.GetAdditionalData().xpRate = 1f;
            __instance.GetAdditionalData().playerClass = "";
        }
    }
}