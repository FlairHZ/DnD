using DnD.Extensions;
using ModdingUtils.MonoBehaviours;
using ModsPlus;
using Photon.Pun;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Color = UnityEngine.Color;

namespace DnD.Monobehaviours
{
    class XPBarMono : MonoBehaviourPun
    {
        private Player player;
        private Gun gun;
        private CharacterStatModifiers stats;
        private CustomHealthBar XPBar;
        private GameObject tabText;

        private void Start()
        {
            if (!photonView.IsMine) return;
            player = GetComponent<Player>();
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            stats = GetComponentInParent<CharacterStatModifiers>();
            CreateBar();
            stats.DealtDamageAction += DealtDamage;
            UpdateBar();
        }
        private void Update()
        {
            if (!photonView.IsMine) return;
            if (Input.GetKeyDown(KeyCode.Tab) && tabText == null)
            {
                tabText = new GameObject("StatText");
                TextMeshPro textMesh = tabText.AddComponent<TextMeshPro>();
                textMesh.fontSize = 12;
                textMesh.color = Color.green;
                textMesh.alignment = TextAlignmentOptions.Center;

                var statsData = player.data.stats.GetAdditionalData();
                textMesh.text = $"LVL: {statsData.level} STR: {statsData.str} DEX: {statsData.dex} CON: {statsData.con}\nINT: {statsData.lnt} WIS: {statsData.wis} CHR: {statsData.chr}";

                tabText.transform.SetParent(transform);
                tabText.transform.localPosition = new Vector3(0, 2f, 0);
                tabText.transform.localRotation = Quaternion.identity;
            }
            else if (Input.GetKeyUp(KeyCode.Tab) && tabText != null)
            {
                Destroy(tabText);
                tabText = null;
            }
        }
        public void DealtDamage(Vector2 damage, bool selfDamage)
        {
            if (!selfDamage && player != null)
            {
                player.data.stats.GetAdditionalData().xp += (int)damage.magnitude;
                if (player.data.stats.GetAdditionalData().xp >= player.data.stats.GetAdditionalData().level * 100)
                {
                    player.data.stats.GetAdditionalData().xp -= player.data.stats.GetAdditionalData().level * 100;
                    player.data.stats.GetAdditionalData().level++;
                    NotifyLevelUp();
                }
                UpdateBar();
            }
        }

        private void NotifyLevelUp()
        {
            photonView.RPC("RPCA_SyncNewStats", RpcTarget.AllBuffered);

        }

        private void CreateBar()
        {
            if (player == null) return;

            var parent = player.GetComponentInChildren<PlayerWobblePosition>().transform;

            GameObject obj = new GameObject("XP Bar");
            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.up * 0.25f;
            obj.transform.localScale = Vector3.one;

            XPBar = obj.AddComponent<CustomHealthBar>();
        }

        public void UpdateBar()
        {
            if (XPBar == null || player == null) return;
            int xp = player.data.stats.GetAdditionalData().xp;
            int level = player.data.stats.GetAdditionalData().level;
            float barHealth = 100f * ((float)xp / (level * 100));
            photonView.RPC("RPCA_SyncXPBar", RpcTarget.AllBuffered, barHealth);
        }

        [PunRPC]
        private void RPCA_SyncXPBar(float health, float r, float g, float b)
        {
            if (XPBar == null) return;

            XPBar.CurrentHealth = health;
        }

        [PunRPC]
        private void RPCA_SyncNewStats()
        {
            System.Random rng = new System.Random(UnityEngine.Random.Range(0, 999999));
            switch (player.data.stats.GetAdditionalData().playerClass)
            {
                case "Wizard":
                    player.data.stats.GetAdditionalData().lnt++;
                    player.data.stats.GetAdditionalData().wis++;
                    player.data.stats.GetAdditionalData().magDmg += 0.1f;
                    player.data.stats.GetAdditionalData().xpRate += 0.05f;
                    break;
                case "Ranger":
                    player.data.stats.GetAdditionalData().str++;
                    player.data.stats.GetAdditionalData().dex++;
                    gun.damage += 0.1f;
                    player.data.stats.movementSpeed += 0.05f;
                    break;
                case "Barbarian":
                    player.data.stats.GetAdditionalData().str++;
                    player.data.stats.GetAdditionalData().con++;
                    gun.damage += 0.1f;
                    player.data.stats.health += 0.1f;
                    break;
            }

            var modifiers = new List<Action>
            {
                () => { gun.damage += 0.1f;
                    player.data.stats.GetAdditionalData().str++;
                },
                () => { player.data.stats.movementSpeed += 0.05f;
                    player.data.stats.GetAdditionalData().dex++;
                },
                () => { player.data.maxHealth += 0.1f;
                    player.data.stats.GetAdditionalData().con++;
                },
                () => { player.data.stats.GetAdditionalData().magDmg += 0.1f;
                    player.data.stats.GetAdditionalData().lnt++;
                },
                () => { player.data.stats.GetAdditionalData().xpRate += 0.05f;
                    player.data.stats.GetAdditionalData().wis++;
                },
                () => { player.data.stats.gravity -= 0.1f;
                    player.data.stats.GetAdditionalData().chr++;
                },
            };

            modifiers = modifiers.OrderBy(x => rng.Next()).ToList();
            for (int i = 0; i < 3; i++)
                modifiers[i]();
        }
    }
}
