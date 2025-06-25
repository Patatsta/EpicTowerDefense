using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEditor;

public class ReplaceTextWithTextMeshPro : MonoBehaviour
{
    [MenuItem("Tools/Replace All Text with TextMeshPro")]
    public static void ReplaceAllText()
    {
        // Find all Text components in the project
        Text[] texts = Resources.FindObjectsOfTypeAll<Text>();

        // Iterate through each Text component
        foreach (Text text in texts)
        {
            // Get the parent GameObject
            GameObject go = text.gameObject;

            // Get the RectTransform
            RectTransform rectTransform = text.GetComponent<RectTransform>();

            // Get the Text component's properties
            string textValue = text.text;
            Font font = text.font;
            Color textColor = text.color;
            int fontSize = text.fontSize;
            FontStyle fontStyle = text.fontStyle;
            TextAnchor alignment = text.alignment;
            bool supportRichText = text.supportRichText;

            // Destroy the Text component
            DestroyImmediate(text);

            // Add a TextMeshProUGUI component
            TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();

            // Set TextMeshProUGUI properties
            tmp.text = textValue;
            if (font != null)
            {
                // Assuming you have a TMP font asset with the same name
                TMP_FontAsset tmpFont = Resources.Load<TMP_FontAsset>(font.name);
                if (tmpFont != null)
                {
                    tmp.font = tmpFont;
                }
                else
                {
                    // If no matching TMP font, use the default
                    tmp.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
                }
            }
            tmp.color = textColor;
            tmp.fontSize = fontSize;
            tmp.fontStyle = (FontStyles)fontStyle;
            tmp.alignment = (TextAlignmentOptions)alignment;
            tmp.richText = supportRichText;

            // Handle RectTransform
            if (rectTransform != null)
            {
                // Copy RectTransform values
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
