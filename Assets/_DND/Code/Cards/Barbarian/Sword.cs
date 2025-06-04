using UnityEngine;
using ModsPlus;
using UnboundLib;
using DnD.SwordScripts;
using System.Linq;
using System.Collections.Generic;
using ClassesManagerReborn.Util;
using Photon.Pun;
using System.Collections;
using UnboundLib.GameModes;
using UnboundLib.Networking;

// Credit: https://github.com/Autuwumn/SwordsManClass/blob/main/Code/Cards/swordmancard.cs

namespace DnD.Cards
{
    public class Sword : CustomEffectCard<SwordCard>
    {
        internal static CardInfo Card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>();
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Sword",
            Description = "Temp",
            ModName = "DnD",
            Art = null,
            Rarity = CardInfo.Rarity.Uncommon,
            Theme = CardThemeColor.CardThemeColorType.DestructiveRed,
            Stats = null,
            OwnerOnly = true
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }
    }
}

namespace DnD.SwordScripts
{
    public class SwordCard : CardEffect
    {
        private SwordHandler sword;
        protected override void Start()
        {
            var swo = PhotonNetwork.Instantiate("DnD_Sword", player.data.hand.position, Quaternion.identity);
            this.ExecuteAfterFrames(20, () =>
            {
                NetworkingManager.RPC(typeof(SwordCard), nameof(RPC_SwordWork), swo.name, player.playerID);
            });
            sword = swo.GetComponent<SwordHandler>();
            FixSword();
        }

        [UnboundRPC]
        public static void RPC_SwordWork(string swordName, int playerid)
        {
            var objects = UnityEngine.GameObject.FindGameObjectsWithTag("Bullet");
            List<GameObject> swords = new List<GameObject>();
            foreach(var obj in objects)
            {
                if(obj.name == swordName)
                {
                    swords.Add(obj);
                }
            }
            foreach(var swo in swords)
            {
                if(swo.GetComponent<SwordHandler>() == null)
                {
                    swo.AddComponent<SwordHandler>().owner = PlayerManager.instance.players.Where((p) => p.playerID == playerid).ToArray()[0];
                }
            }
        }
        private void FixSword()
        {
            var poli = sword.gameObject.GetComponentInChildren<PolygonCollider2D>();
            foreach (var colli in player.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(poli, colli);
            }
        }
        public override IEnumerator OnPointStart(IGameModeHandler gameModeHandler)
        {
            FixSword();
            yield break;
        }
        public override void OnRevive()
        {
            FixSword();
            base.OnRevive();
        }
        protected override void OnDestroy()
        {
            PhotonNetwork.Destroy(sword.gameObject);
            base.OnDestroy();
        }
    }
    public class SwordHandler : MonoBehaviour
    {
        public Player owner;
        private Rigidbody2D rigid;
        private DamageBox damagebox;

        public void Start()
        {
            rigid = gameObject.GetComponent<Rigidbody2D>();
            damagebox = gameObject.GetComponentInChildren<DamageBox>();
        }
        public void Update()
        {
            var poli = gameObject.GetComponentInChildren<PolygonCollider2D>();
            foreach (var colli in owner.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(poli, colli);
            }
            var size = 0.6f;
            var stun = false;
            var knock = false;
            rigid.transform.localScale = new Vector3(size, size, size);
            rigid.position = owner.data.hand.position;
            rigid.transform.up = -owner.data.aimDirection;
            damagebox.damage = owner.data.weaponHandler.gun.damage * 55f;
            damagebox.cd = owner.data.weaponHandler.gun.attackSpeed*2;
            if (!knock) damagebox.force = 0;
            if (!stun) damagebox.setFlyingFor = 0;
            if (knock) damagebox.force = 5000;
            if (stun) damagebox.setFlyingFor = 1;
            if(owner.data.health <= 0)
            {
                rigid.position = new Vector3(1000, 1000, 0);
            }
        }
    }
}