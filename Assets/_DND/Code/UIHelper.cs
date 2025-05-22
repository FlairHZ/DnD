using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnboundLib;
using UnityEngine;
using UnityEngine.UI;

// Credit to Willis for a good portion of this code (aka setting up the canvas)
namespace UI
{
    public static class UIHelper
    {
        public static TextMeshProUGUI AddText(string content, int fontSize, Color color, TextAlignmentOptions alignment, Vector2 pivot, float offset, Vector2 size)
        {
            TextMeshProUGUI textGO = new GameObject().AddComponent<TextMeshProUGUI>();
            textGO.transform.SetParent(textGO.transform, false);
            textGO.gameObject.AddComponent<Canvas>().sortingLayerName = "MostFront";

            var text = textGO.GetComponent<TextMeshProUGUI>();
            text.text = content;
            text.fontSize = fontSize;
            text.color = color;
            text.alignment = alignment;

            RectTransform rect = text.GetComponent<RectTransform>();
            float screenHeightPercentage = offset;  
            float screenHeight = Screen.height;    
            float movementInPixels = screenHeight * screenHeightPercentage;
            rect.anchorMin = pivot;
            rect.anchorMax = pivot;
            rect.pivot = pivot;
            rect.anchoredPosition = new Vector2(0, movementInPixels);
            rect.sizeDelta = size;

            return textGO;
        }

        public static void AddRectangle(Color color, Vector2 pivot, Vector2 position, Vector2 size)
        {
            GameObject rectGO = new GameObject("UIRectangle", typeof(RectTransform), typeof(Image));

            Image img = rectGO.GetComponent<Image>();
            img.color = color;

            RectTransform rect = rectGO.GetComponent<RectTransform>();
            rect.anchorMin = pivot;
            rect.anchorMax = pivot;
            rect.pivot = pivot;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        public static void ClearText(ref TextMeshProUGUI text)
        {
            text.text = "";
            text = null;
        }
    }
}
