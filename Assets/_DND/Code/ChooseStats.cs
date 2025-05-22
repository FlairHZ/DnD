using UnboundLib;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HarmonyLib; 
using System.Linq;
using System;
using UnboundLib.Utils.UI;
using UI;
using DnD.Extensions;
using System.Collections.Generic;
using System.Data;
using ModdingUtils.AIMinion.Extensions;

namespace ChooseStatsUI
{
    public class ChooseStats : MonoBehaviour
    {
        public static TextMeshProUGUI currentText, strength, dexterity, constitution, intelligence, wisdom, charisma, points = null;
        static Dictionary<int, float> previousLeftInput = new Dictionary<int, float>();
        static Dictionary<int, float> previousRightInput = new Dictionary<int, float>();
        static Dictionary<int, float> previousUpInput = new Dictionary<int, float>();
        static Dictionary<int, float> previousDownInput = new Dictionary<int, float>();
        public static int pickerID = -1;
        public static int currentStat = 1;
        private static float lastChangeTime = 0f;
        private static float changeCooldown = 0.15f;
        public static GameObject dummyCard = null;

        private void Start()
        {
            strength = new GameObject().AddComponent<TextMeshProUGUI>();
            strength.gameObject.AddComponent<DestroyOnUnparent>();
            strength.transform.SetParent(gameObject.transform, false);
            strength.gameObject.transform.localScale = new Vector3(.4f, .4f, .4f);
            strength.gameObject.AddComponent<Canvas>().sortingLayerName = "MostFront";
            strength.fontSize = 16;
            strength.text = "";
            strength.color = Color.red;
            strength.rectTransform.anchoredPosition = Vector2.zero;
            strength.alignment = TextAlignmentOptions.Center;
            RectTransform rect = strength.rectTransform;
            float screenHeightPercentage = 0.007f;
            float screenWidthPercentage = 0.01f;
            float movementInPixelsH = Screen.height * screenHeightPercentage;
            float movementInPixelsW = Screen.width * screenWidthPercentage;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(-movementInPixelsW, movementInPixelsH);

            dexterity = new GameObject().AddComponent<TextMeshProUGUI>();
            dexterity.gameObject.AddComponent<DestroyOnUnparent>();
            dexterity.transform.SetParent(gameObject.transform, false);
            dexterity.gameObject.transform.localScale = new Vector3(.4f, .4f, .4f);
            dexterity.gameObject.AddComponent<Canvas>().sortingLayerName = "MostFront";
            dexterity.fontSize = 16;
            dexterity.text = "";
            dexterity.color = Color.green;
            dexterity.rectTransform.anchoredPosition = Vector2.zero;
            dexterity.alignment = TextAlignmentOptions.Center;
            rect = dexterity.rectTransform;
            screenHeightPercentage = 0.007f;
            screenWidthPercentage = 0.006f;
            movementInPixelsH = Screen.height * screenHeightPercentage;
            movementInPixelsW = Screen.width * screenWidthPercentage;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(-movementInPixelsW, movementInPixelsH);

            constitution = new GameObject().AddComponent<TextMeshProUGUI>();
            constitution.gameObject.AddComponent<DestroyOnUnparent>();
            constitution.transform.SetParent(gameObject.transform, false);
            constitution.gameObject.transform.localScale = new Vector3(.4f, .4f, .4f);
            constitution.gameObject.AddComponent<Canvas>().sortingLayerName = "MostFront";
            constitution.fontSize = 16;
            constitution.text = "";
            constitution.color = Color.yellow;
            constitution.rectTransform.anchoredPosition = Vector2.zero;
            constitution.alignment = TextAlignmentOptions.Center;
            rect = constitution.rectTransform;
            screenHeightPercentage = 0.007f;
            screenWidthPercentage = 0.002f;
            movementInPixelsH = Screen.height * screenHeightPercentage;
            movementInPixelsW = Screen.width * screenWidthPercentage;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(-movementInPixelsW, movementInPixelsH);

            intelligence = new GameObject().AddComponent<TextMeshProUGUI>();
            intelligence.gameObject.AddComponent<DestroyOnUnparent>();
            intelligence.transform.SetParent(gameObject.transform, false);
            intelligence.gameObject.transform.localScale = new Vector3(.4f, .4f, .4f);
            intelligence.gameObject.AddComponent<Canvas>().sortingLayerName = "MostFront";
            intelligence.fontSize = 16;
            intelligence.text = "";
            intelligence.color = Color.cyan;
            intelligence.rectTransform.anchoredPosition = Vector2.zero;
            intelligence.alignment = TextAlignmentOptions.Center;
            rect = intelligence.rectTransform;
            screenHeightPercentage = 0.007f;
            screenWidthPercentage = -0.002f;
            movementInPixelsH = Screen.height * screenHeightPercentage;
            movementInPixelsW = Screen.width * screenWidthPercentage;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(-movementInPixelsW, movementInPixelsH);

            wisdom = new GameObject().AddComponent<TextMeshProUGUI>();
            wisdom.gameObject.AddComponent<DestroyOnUnparent>();
            wisdom.transform.SetParent(gameObject.transform, false);
            wisdom.gameObject.transform.localScale = new Vector3(.4f, .4f, .4f);
            wisdom.gameObject.AddComponent<Canvas>().sortingLayerName = "MostFront";
            wisdom.fontSize = 16;
            wisdom.text = "";
            wisdom.color = Color.white;
            wisdom.rectTransform.anchoredPosition = Vector2.zero;
            wisdom.alignment = TextAlignmentOptions.Center;
            rect = wisdom.rectTransform;
            screenHeightPercentage = 0.007f;
            screenWidthPercentage = -0.006f;
            movementInPixelsH = Screen.height * screenHeightPercentage;
            movementInPixelsW = Screen.width * screenWidthPercentage;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(-movementInPixelsW, movementInPixelsH);

            charisma = new GameObject().AddComponent<TextMeshProUGUI>();
            charisma.gameObject.AddComponent<DestroyOnUnparent>();
            charisma.transform.SetParent(gameObject.transform, false);
            charisma.gameObject.transform.localScale = new Vector3(.4f, .4f, .4f);
            charisma.gameObject.AddComponent<Canvas>().sortingLayerName = "MostFront";
            charisma.fontSize = 16;
            charisma.text = "";
            charisma.color = Color.magenta;
            charisma.rectTransform.anchoredPosition = Vector2.zero;
            charisma.alignment = TextAlignmentOptions.Center;
            rect = charisma.rectTransform;
            screenHeightPercentage = 0.007f;
            screenWidthPercentage = -0.01f;
            movementInPixelsH = Screen.height * screenHeightPercentage;
            movementInPixelsW = Screen.width * screenWidthPercentage;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(-movementInPixelsW, movementInPixelsH);

            points = new GameObject().AddComponent<TextMeshProUGUI>();
            points.gameObject.AddComponent<DestroyOnUnparent>();
            points.transform.SetParent(gameObject.transform, false);
            points.gameObject.transform.localScale = new Vector3(.4f, .4f, .4f);
            points.gameObject.AddComponent<Canvas>().sortingLayerName = "MostFront";
            points.fontSize = 3;
            points.text = "";
            points.color = Color.white;
            points.rectTransform.anchoredPosition = Vector2.zero;
            points.alignment = TextAlignmentOptions.Center;
            rect = points.rectTransform;
            screenHeightPercentage = 0.011f;
            screenWidthPercentage = 0f;
            movementInPixelsH = Screen.height * screenHeightPercentage;
            movementInPixelsW = Screen.width * screenWidthPercentage;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(-movementInPixelsW, movementInPixelsH);
        }

        [Serializable]
        [HarmonyPatch(typeof(CardChoice), "StartPick")]
        class CardChoicePatchStartPick
        {
            private static void Prefix(int pickerIDToSet)
            {
                currentText = UIHelper.AddText("Strength: +0% Physical Damage", 1, Color.red, TextAlignmentOptions.Center, new Vector2(0.5f, 0.5f), 0.012f, new Vector2(400, 60));
                currentStat = 1;

                pickerID = pickerIDToSet;
                Player player = PlayerManager.instance.players[pickerID];

                strength.text = player.data.stats.GetAdditionalData().str.ToString();
                dexterity.text = player.data.stats.GetAdditionalData().dex.ToString();
                constitution.text = player.data.stats.GetAdditionalData().con.ToString();
                intelligence.text = player.data.stats.GetAdditionalData().lnt.ToString();
                wisdom.text = player.data.stats.GetAdditionalData().wis.ToString();
                charisma.text = player.data.stats.GetAdditionalData().chr.ToString();
                points.text = "Available points: " + player.data.stats.GetAdditionalData().freePoints.ToString();

                // Removes all the cards that could spawn
                Transform[] test = (Transform[])CardChoice.instance.GetFieldValue("children");
                dummyCard = test[0].gameObject;
                foreach (Transform child in test.Skip(1))
                {
                    UnityEngine.GameObject.Destroy(child.gameObject);
                }
            }
        }

        public class DestroyOnUnparent : MonoBehaviour
        {
            void LateUpdate()
            {
                if (this.gameObject.transform.parent == null)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        [Serializable]
        [HarmonyPatch(typeof(CardChoice), "DoPlayerSelect")]
        class ChangeBar
        {
            private static void Prefix()
            {
                var actions = PlayerManager.instance.GetActionsFromPlayer(pickerID);
                if (actions == null) return;

                float left = actions[0].Left.Value;
                float right = actions[0].Right.Value;
                float up = actions[0].Up.Value; 
                float down = actions[0].Down.Value;
                float space = actions[0].Jump.Value;

                // Initialize tracking if not yet done
                if (!previousLeftInput.ContainsKey(pickerID))
                {
                    previousLeftInput[pickerID] = 0f;
                    previousRightInput[pickerID] = 0f;
                    previousUpInput[pickerID] = 0f;
                    previousDownInput[pickerID] = 0f;
                }

                if (Time.time - lastChangeTime < changeCooldown)
                {
                    return;
                }

                // Update stored values
                previousLeftInput[pickerID] = left;
                previousRightInput[pickerID] = right;

                if (previousRightInput[pickerID] >= 0.7f && currentStat < 7)
                {
                    currentStat++;
                    updateText();
                    lastChangeTime = Time.time;
                    UnityEngine.Debug.Log(dummyCard);
                }
                else if (previousLeftInput[pickerID] >= 0.7f && currentStat > 1)
                {
                    currentStat--;
                    updateText();
                    lastChangeTime = Time.time;
                }
                else if (up >= 0.7f)
                {
                    updateStat(true);
                    lastChangeTime = Time.time;
                }
                else if (down >= 0.7f)
                {
                    updateStat(false);
                    lastChangeTime = Time.time;
                }
                else if (space >= 0.7 && currentStat == 7)
                {
                    UnityEngine.Debug.Log(dummyCard);
                    Transform[] children = (Transform[])CardChoice.instance.GetFieldValue("children");

                    foreach (Transform child in children)
                    {
                        UnityEngine.Debug.Log(child);
                        if (child == null) continue;

                        CardInfo cardInfo = child.GetComponentInChildren<CardInfo>();
                        UnityEngine.Debug.Log(cardInfo);

                        if (cardInfo != null)
                        {
                            // This is a valid pickable card
                            CardChoice.instance.Pick(child.gameObject, false);
                            break;
                        }
                    }
                    lastChangeTime = Time.time;
                }
            }
        }
        public static void updateText()
        {
            Player player = PlayerManager.instance.players[pickerID];
            if (currentStat < 0 || currentStat > 7)
            {
                return;
            }

            UIHelper.ClearText(ref currentText);

            if (currentStat == 1)
            {
                currentText = UIHelper.AddText("Strength: " + (10 * (player.data.stats.GetAdditionalData().str - 5)).ToString() + "% Physical Damage", 1, Color.red, TextAlignmentOptions.Center, new Vector2(0.5f, 0.5f), 0.012f, new Vector2(400, 60));
            }
            else if (currentStat == 2)
            {
                currentText = UIHelper.AddText("Dexterity: " + (5 * (player.data.stats.GetAdditionalData().dex - 5)).ToString() + "% Movement Speed", 1, Color.green, TextAlignmentOptions.Center, new Vector2(0.5f, 0.5f), 0.012f, new Vector2(400, 60));
            }
            else if (currentStat == 3)
            {
                currentText = UIHelper.AddText("Constitution: " + (10 * (player.data.stats.GetAdditionalData().con - 5)).ToString() + "% Health", 1, Color.yellow, TextAlignmentOptions.Center, new Vector2(0.5f, 0.5f), 0.012f, new Vector2(400, 60));
            }
            else if (currentStat == 4)
            {
                currentText = UIHelper.AddText("Intelligence: " + (10 * (player.data.stats.GetAdditionalData().lnt - 5)).ToString() + "% Magic Damage", 1, Color.cyan, TextAlignmentOptions.Center, new Vector2(0.5f, 0.5f), 0.012f, new Vector2(400, 60));
            }
            else if (currentStat == 5)
            {
                currentText = UIHelper.AddText("Wisdom: " + (5 * (player.data.stats.GetAdditionalData().wis - 5)).ToString() + "% XP Gain", 1, Color.white, TextAlignmentOptions.Center, new Vector2(0.5f, 0.5f), 0.012f, new Vector2(400, 60));
            }
            else if (currentStat == 6)
            {
                currentText = UIHelper.AddText("Charisma: " + (5 * (player.data.stats.GetAdditionalData().chr - 5)).ToString() + "% ????", 1, Color.magenta, TextAlignmentOptions.Center, new Vector2(0.5f, 0.5f), 0.012f, new Vector2(400, 60));
            }
            else
            {
                currentText = UIHelper.AddText("Press Space To Finish", 1, Color.magenta, TextAlignmentOptions.Center, new Vector2(0.5f, 0.5f), 0.012f, new Vector2(400, 60));
            }
        }

        public static void updateStat(bool Up)
        {
            Player player = PlayerManager.instance.players[pickerID];
            if (currentStat < 1 || currentStat > 6)
            {
                return;
            }
            else if (player.data.stats.GetAdditionalData().freePoints == 0 && Up)
            {
                return;
            }

            var stats = player.data.stats.GetAdditionalData();

            // Map stat indices to getter/setter actions and text objects
            var statMap = new Dictionary<int, (Func<int> getter, Action<int> setter, TextMeshProUGUI text)>
            {
                { 1, (() => stats.str, val => stats.str = val, strength) },
                { 2, (() => stats.dex, val => stats.dex = val, dexterity) },
                { 3, (() => stats.con, val => stats.con = val, constitution) },
                { 4, (() => stats.lnt, val => stats.lnt = val, intelligence) },
                { 5, (() => stats.wis, val => stats.wis = val, wisdom) },
                { 6, (() => stats.chr, val => stats.chr = val, charisma) },
            };

            var (getStat, setStat, textField) = statMap[currentStat];
            int value = getStat();

            if (Up && value < 20)
            {
                value++;
                player.data.stats.GetAdditionalData().freePoints--;
            }
            else if (!Up && value > 1)
            {
                player.data.stats.GetAdditionalData().freePoints++;
                value--;
            }
            else
            {
                return;
            }

            setStat(value);
            textField.text = value.ToString();
            points.text = "Available points: " + player.data.stats.GetAdditionalData().freePoints.ToString();
            updateText();
        }
    }
}