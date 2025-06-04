using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;
using DnDConfig;
using System.Collections;
using UnboundLib;
using UnboundLib.GameModes;
using DnD.Cards;
using UnboundLib.Cards;
using static UnityEngine.ParticleSystem;
using Jotunn.Utils;
using Photon.Pun;


[BepInDependency("com.willis.rounds.unbound")]
[BepInDependency("pykess.rounds.plugins.moddingutils")]
[BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
[BepInPlugin("com.Flair.Mod.DnD", "Dungeons And Dragons", "0.1.0")]
[BepInProcess("Rounds.exe")]
public class DungeonsAndDragons : BaseUnityPlugin
{
    internal static string modInitials = "DnD";
    public static DungeonsAndDragons instance { get; private set; }

    internal static AssetBundle assets;
    
    void Awake()
    {

        new Harmony("com.Flair.Mod.DnD").PatchAll();
    }
    void Start()
    {
        DungeonsAndDragons.assets = AssetUtils.LoadAssetBundleFromResources("assets", typeof(DungeonsAndDragons).Assembly);

        PhotonNetwork.PrefabPool.RegisterPrefab("DnD_Sword", assets.LoadAsset<GameObject>("Sword"));

        CustomCard.BuildCard<Barbarian>((card) => Barbarian.Card = card);
        CustomCard.BuildCard<Sword>((card) => Sword.Card = card);

        CustomCard.BuildCard<Ranger>((card) => Ranger.Card = card);

        CustomCard.BuildCard<Wizard>((card) => Wizard.Card = card);
    }
}