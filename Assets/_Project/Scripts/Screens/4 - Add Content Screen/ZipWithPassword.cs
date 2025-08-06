using UnityEngine;
using System.IO;
using Ionic.Zip;
using System;

public class ZipWithPassword : MonoBehaviour
{

    public void CreateEncryptedZip(string inputFilePath, string outputZipPath, string password)
    {
        if (!File.Exists(inputFilePath))
        {
            Debug.LogError("EPUB file not found: " + inputFilePath);
            return;
        }

        using (ZipFile zip = new ZipFile())
        {
            zip.Encryption = EncryptionAlgorithm.WinZipAes256;

            // Add the file and assign password encryption to it explicitly
            ZipEntry entry = zip.AddFile(inputFilePath, "");
            entry.Password = password;
            entry.Encryption = EncryptionAlgorithm.WinZipAes256;

            zip.Save(outputZipPath);
        }

        Debug.Log("Encrypted zip created at: " + outputZipPath);
    }

    public void CreateEncryptedZipFolder(string inputFolderPath, string outputZipPath, string password)
    {
        if (!Directory.Exists(inputFolderPath))
        {
            Debug.LogError("EPUB folder not found: " + inputFolderPath);
            return;
        }

        using (ZipFile zip = new ZipFile(System.Text.Encoding.UTF8))
        {
            zip.Encryption = EncryptionAlgorithm.WinZipAes256;
            zip.Password = password;

            // Add the folder and assign password encryption to it explicitly
            zip.AddDirectory(inputFolderPath);

            zip.Save(outputZipPath);
        }

        Debug.Log("Encrypted zip created at: " + outputZipPath);
    }

    public void DecompressEncryptedZip(string zipFilePath, string outputFolderPath, string password)
    {
        if (!File.Exists(zipFilePath))
        {
            Debug.LogError("Encrypted zip file not found: " + zipFilePath);
            return;
        }

        using (ZipFile zip = ZipFile.Read(zipFilePath))
        {
            zip.Password = password;
            zip.ExtractAll(outputFolderPath, ExtractExistingFileAction.OverwriteSilently);
        }

        Debug.Log("Decompressed files to: " + outputFolderPath);
    }


}
