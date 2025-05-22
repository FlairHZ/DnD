using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Photon.Pun.Simple.Pooling;

namespace DnD.Extensions
{
    [Serializable]
    public class CharacterStatModifiersAdditionalData
    {
        public int str, dex, con, lnt, wis, chr, maxPoints, freePoints;

        public CharacterStatModifiersAdditionalData()
        {
            str = 5;
            dex = 5;
            con = 5;
            lnt = 5;
            wis = 5;
            chr = 5;
            maxPoints = 30;
            freePoints = 0;
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
            __instance.GetAdditionalData().str = 5;
            __instance.GetAdditionalData().dex = 5;
            __instance.GetAdditionalData().con = 5;
            __instance.GetAdditionalData().lnt = 5;
            __instance.GetAdditionalData().wis = 5;
            __instance.GetAdditionalData().chr = 5;
            __instance.GetAdditionalData().maxPoints = 30;
            __instance.GetAdditionalData().freePoints = 0;
        }
    }
}