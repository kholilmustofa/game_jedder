using UnityEngine;
using UnityEditor;
using System.IO;

public class SpecialPaperStitcher : EditorWindow
{
    [MenuItem("Tools/Stitch Special Paper")]
    public static void Stitch()
    {
        string sourcePath = "Assets/Tiny Swords (Free Pack)/UI Elements/UI Elements/Papers/SpecialPaper.png";
        string targetPath = "Assets/Tiny Swords (Free Pack)/UI Elements/UI Elements/Papers/SpecialPaper_Stitched.png";

        TextureImporter importer = AssetImporter.GetAtPath(sourcePath) as TextureImporter;
        if (importer == null)
        {
            Debug.LogError("Tidak dapat menemukan SpecialPaper.png di: " + sourcePath);
            EditorUtility.DisplayDialog("Error", "Tidak dapat menemukan SpecialPaper.png.", "OK");
            return;
        }

        bool wasReadable = importer.isReadable;
        TextureImporterCompression oldCompression = importer.textureCompression;

        if (!wasReadable || oldCompression != TextureImporterCompression.Uncompressed)
        {
            importer.isReadable = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        Texture2D sourceTex = AssetDatabase.LoadAssetAtPath<Texture2D>(sourcePath);
        if (sourceTex == null) return;

        int targetWidth = 178;
        int targetHeight = 156;
        Texture2D targetTex = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);

        Color[] clearColors = new Color[targetWidth * targetHeight];
        for (int i = 0; i < clearColors.Length; i++) clearColors[i] = Color.clear;
        targetTex.SetPixels(clearColors);

        int[] widths = { 56, 66, 56 };
        int[] sourceX = { 9, 127, 255 };
        int[] targetX = { 0, 56, 122 };

        int[] heights = { 45, 66, 45 };
        int[] sourceY = { 20, 127, 255 };
        int[] targetY = { 0, 45, 111 };

        for (int col = 0; col < 3; col++)
        {
            for (int row = 0; row < 3; row++)
            {
                int w = widths[col];
                int h = heights[row];

                Color[] pixels = sourceTex.GetPixels(sourceX[col], sourceY[row], w, h);
                targetTex.SetPixels(targetX[col], targetY[row], w, h, pixels);
            }
        }

        targetTex.Apply();
        byte[] pngData = targetTex.EncodeToPNG();
        File.WriteAllBytes(targetPath, pngData);
        AssetDatabase.Refresh();

        if (!wasReadable || oldCompression != TextureImporterCompression.Uncompressed)
        {
            importer.isReadable = wasReadable;
            importer.textureCompression = oldCompression;
            importer.SaveAndReimport();
        }

        TextureImporter targetImporter = AssetImporter.GetAtPath(targetPath) as TextureImporter;
        if (targetImporter != null)
        {
            targetImporter.textureType = TextureImporterType.Sprite;
            targetImporter.spriteImportMode = SpriteImportMode.Single;
            
            TextureImporterSettings settings = new TextureImporterSettings();
            targetImporter.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            targetImporter.SetTextureSettings(settings);

            targetImporter.mipmapEnabled = false;
            targetImporter.spriteBorder = new Vector4(56, 45, 56, 45);
            targetImporter.filterMode = FilterMode.Point;
            targetImporter.wrapMode = TextureWrapMode.Clamp;
            targetImporter.textureCompression = TextureImporterCompression.Uncompressed;
            targetImporter.SaveAndReimport();
        }

        Debug.Log("Sukses menggabungkan SpecialPaper!");
        EditorUtility.DisplayDialog("Sukses!", "SpecialPaper berhasil dijahit tanpa celah!", "Mantap!");
    }

    [MenuItem("Tools/Stitch BigBar Base")]
    public static void StitchBigBarBase()
    {
        string sourcePath = "Assets/Tiny Swords (Free Pack)/UI Elements/UI Elements/Bars/BigBar_Base.png";
        string targetPath = "Assets/Tiny Swords (Free Pack)/UI Elements/UI Elements/Bars/BigBar_Base_Stitched.png";

        TextureImporter importer = AssetImporter.GetAtPath(sourcePath) as TextureImporter;
        if (importer == null)
        {
            Debug.LogError("Tidak dapat menemukan BigBar_Base.png di: " + sourcePath);
            EditorUtility.DisplayDialog("Error", "Tidak dapat menemukan BigBar_Base.png.", "OK");
            return;
        }

        bool wasReadable = importer.isReadable;
        TextureImporterCompression oldCompression = importer.textureCompression;

        if (!wasReadable || oldCompression != TextureImporterCompression.Uncompressed)
        {
            importer.isReadable = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        Texture2D sourceTex = AssetDatabase.LoadAssetAtPath<Texture2D>(sourcePath);
        if (sourceTex == null) return;

        // Dimensi potongan BigBar_Base (dijahit tanpa celah transparan):
        // Left Cap: x=40, y=3, w=24, h=53 (kolom x=39 dan x=64 adalah transparan, kita abaikan)
        // Middle: x=128, y=3, w=64, h=53 (kolom x=127 dan x=192 adalah transparan, kita abaikan)
        // Right Cap: x=256, y=3, w=24, h=53 (kolom x=255 dan x=280 adalah transparan, kita abaikan)
        // Total Stitched Width = 24 + 64 + 24 = 112, Height = 53
        int targetWidth = 112;
        int targetHeight = 53;
        Texture2D targetTex = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, false);

        // Bersihkan warna dasar
        Color[] clearColors = new Color[targetWidth * targetHeight];
        for (int i = 0; i < clearColors.Length; i++) clearColors[i] = Color.clear;
        targetTex.SetPixels(clearColors);

        // Salin Left Cap (x=40, y=3, w=24, h=53)
        Color[] leftPixels = sourceTex.GetPixels(40, 3, 24, 53);
        targetTex.SetPixels(0, 0, 24, 53, leftPixels);

        // Salin Middle (x=128, y=3, w=64, h=53)
        Color[] midPixels = sourceTex.GetPixels(128, 3, 64, 53);
        targetTex.SetPixels(24, 0, 64, 53, midPixels);

        // Salin Right Cap (x=256, y=3, w=24, h=53)
        Color[] rightPixels = sourceTex.GetPixels(256, 3, 24, 53);
        targetTex.SetPixels(88, 0, 24, 53, rightPixels); // targetX = 24 + 64 = 88

        targetTex.Apply();
        byte[] pngData = targetTex.EncodeToPNG();
        File.WriteAllBytes(targetPath, pngData);
        AssetDatabase.Refresh();

        if (!wasReadable || oldCompression != TextureImporterCompression.Uncompressed)
        {
            importer.isReadable = wasReadable;
            importer.textureCompression = oldCompression;
            importer.SaveAndReimport();
        }

        // Konfigurasi otomatis sebagai Sprite 9-Slice dengan Border: Left=24, Right=24
        TextureImporter targetImporter = AssetImporter.GetAtPath(targetPath) as TextureImporter;
        if (targetImporter != null)
        {
            targetImporter.textureType = TextureImporterType.Sprite;
            targetImporter.spriteImportMode = SpriteImportMode.Single;

            TextureImporterSettings settings = new TextureImporterSettings();
            targetImporter.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            targetImporter.SetTextureSettings(settings);

            targetImporter.mipmapEnabled = false;
            targetImporter.spriteBorder = new Vector4(24, 0, 24, 0); // Kiri=24, Kanan=24, Atas-Bawah=0
            targetImporter.filterMode = FilterMode.Point;
            targetImporter.wrapMode = TextureWrapMode.Clamp;
            targetImporter.textureCompression = TextureImporterCompression.Uncompressed;
            targetImporter.SaveAndReimport();
        }

        Debug.Log("Sukses menggabungkan BigBar_Base!");
        EditorUtility.DisplayDialog("Sukses!", "BigBar_Base berhasil dijahit rapat tanpa celah dan diset sebagai Sprite!", "Mantap!");
    }
}
