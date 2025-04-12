using System.IO;
using UnityEngine;

public static class ImageConverter
{
    // Convert Texture2D to byte array
    public static byte[] TextureToByteArray(Texture2D texture)
    {
        return texture.EncodeToPNG();
    }

    // Save image from Texture2D
    public static void SaveTextureToFile(Texture2D texture, string filePath)
    {
        byte[] imageBytes = texture.EncodeToPNG();
        File.WriteAllBytes(filePath, imageBytes);
    }
}