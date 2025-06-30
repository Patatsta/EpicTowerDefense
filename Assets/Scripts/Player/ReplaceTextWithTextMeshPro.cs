#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ReplaceTextWithTextMeshPro : MonoBehaviour
{
    [MenuItem("Tools/Replace All Text with TextMeshPro")]
    public static void ReplaceAllText()
    {
        Text[] texts = Resources.FindObjectsOfTypeAll<Text>();

        foreach (Text text in texts)
        {
            GameObject go = text.gameObject;
            RectTransform rectTransform = text.GetComponent<RectTransform>();

            string textValue = text.text;
            Font font = text.font;
            Color textColor = text.color;
            int fontSize = text.fontSize;
            FontStyle fontStyle = text.fontStyle;
            TextAnchor alignment = text.alignment;
            bool supportRichText = text.supportRichText;

            DestroyImmediate(text);

            TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = textValue;

            if (font != null)
            {
                TMP_FontAsset tmpFont = Resources.Load<TMP_FontAsset>(font.name);
                if (tmpFont != null)
                    tmp.font = tmpFont;
                else
                    tmp.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            }

            tmp.color = textColor;
            tmp.fontSize = fontSize;
            tmp.fontStyle = (FontStyles)fontStyle;
            tmp.alignment = (TextAlignmentOptions)alignment;
            tmp.richText = supportRichText;

            if (rectTransform != null)
            {
                tmp.rectTransform.anchorMin = rectTransform.anchorMin;
                tmp.rectTransform.anchorMax = rectTransform.anchorMax;
                tmp.rectTransform.anchoredPosition = rectTransform.anchoredPosition;
                tmp.rectTransform.sizeDelta = rectTransform.sizeDelta;
                tmp.rectTransform.pivot = rectTransform.pivot;
            }
        }

        Debug.Log("Replaced all Text components with TextMeshProUGUI.");
    }
}
#endif

