using UnityEngine;
using System.Collections;
using System.IO;

public class BundleEncryption
{

    public static byte[] Encryption(string filePath)
    {
        var fileBytes = File.ReadAllBytes(filePath);
        for (int i = 0; i < fileBytes.Length; ++i)
        {
            fileBytes[i] = (byte)~fileBytes[i];
        }
        return fileBytes;
    }

    public static void Decryption(byte[] bytes)
    {
        for (int i = 0; i < bytes.Length; ++i)
        {
            bytes[i] = (byte)~bytes[i];
        }
    }
}
