using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;
using ChooseStatsUI;
using System.Collections;
using UnboundLib;
using UnboundLib.GameModes;


[BepInDependency("com.willis.rounds.unbound")]
[BepInDependency("pykess.rounds.plugins.moddingutils")]
[BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
[BepInPlugin("com.Flair.Mod.DnD", "Dungeons And Dragons", "0.1.0")]
[BepInProcess("Rounds.exe")]
public class DungeonsAndDragons : BaseUnityPlugin
{
    internal static string modInitials = "DnD";
    internal static AssetBundle assets;
    public static DungeonsAndDragons instance { get; private set; }
    void Awake()
    {
        assets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("cards", typeof(DungeonsAndDragons).Assembly);
        assets.LoadAsset<GameObject>("ModCards").GetComponent<CardHolder>().RegisterCards();

        new Harmony("com.Flair.Mod.DnD").PatchAll();
    }
    void Start()
    {
        GameModeManager.AddHook(GameModeHooks.HookPickStart, this.PickStart);
    }

    private IEnumerator PickStart(IGameModeHandler gameModeHandler)
    {
        ChooseStats gameStatusUpdate = new GameObject().AddComponent<ChooseStats>();
        yield break;
    }
}