#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class SetMaxTextureSize : EditorWindow
{
    [MenuItem("Tools/Set All Textures Max Size to 128")]
    public static void SetTexturesTo128()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture");

        int changed = 0;
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null && importer.maxTextureSize != 128)
            {
                importer.maxTextureSize = 128;
                importer.SaveAndReimport();
                changed++;
            }
        }

        Debug.Log($"Set max size to 128 for {changed} textures.");
    }
}
#endif
