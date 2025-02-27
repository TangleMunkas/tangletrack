using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SaveJsonHandler
{
    private static readonly string EncryptionKey = "qDzCF-u1;@]Z>2GJ"; // 16 karakteres titkosítási kulcs | qDzCF-u1;@]Z>2GJ |

    // Listák mentése
    public static void SaveList(List<VehicleData> data, string fileName, string path = "", bool doEncrypt = true)
    {
        SaveVehicleDataWrapper wrapper = new SaveVehicleDataWrapper
        {
            saveVehicleDataList = data
        };

        string filePath;
        if (path == "")
        {
            filePath = Path.Combine(Application.persistentDataPath, fileName);
        }
        else
        {
            filePath = Path.Combine(path, fileName);
        }

        string json = JsonUtility.ToJson(wrapper, true);

        if (doEncrypt)
        {
            byte[] encryptedData = Encrypt(json);
            File.WriteAllBytes(filePath, encryptedData);
        }
        else
        {
            File.WriteAllText(filePath, json);
        }
        Debug.Log($"List saved to: {filePath}");
    }

    // Listák betöltése
    public static List<VehicleData> GetList(string fileName, string path = "", bool doDecrypt = true)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (path == "")
        {
            filePath = Path.Combine(Application.persistentDataPath, fileName);
        }
        else
        {
            filePath = Path.Combine(path, fileName);
        }

        if (File.Exists(filePath))
        {
            try
            {
                if (doDecrypt)
                {
                    byte[] encryptedData = File.ReadAllBytes(filePath);
                    string json = Decrypt(encryptedData);
                    SaveVehicleDataWrapper wrapper = JsonUtility.FromJson<SaveVehicleDataWrapper>(json);
                    return wrapper.saveVehicleDataList;
                }
                else
                {
                    string json = File.ReadAllText(filePath);
                    SaveVehicleDataWrapper wrapper = JsonUtility.FromJson<SaveVehicleDataWrapper>(json);
                    return wrapper.saveVehicleDataList;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Visszafejtési hiba: {ex.Message}");
                return new List<VehicleData>();
            }
        }
        else
        {
            Debug.LogWarning($"A fájl nem található! | {filePath} |");
            return new List<VehicleData>();
        }
    }

    private static byte[] Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aes.GenerateIV();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(aes.IV, 0, aes.IV.Length); // IV hozzáadása a fájl elejére
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(cryptoStream))
                    {
                        writer.Write(plainText);
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }

    private static string Decrypt(byte[] cipherData)
    {
        using (Aes aes = Aes.Create())
        {
            if (cipherData.Length < aes.BlockSize / 8)
            {
                throw new ArgumentException("A fájl túl kicsi az IV tárolásához.");
            }

            using (MemoryStream memoryStream = new MemoryStream(cipherData))
            {
                byte[] iv = new byte[aes.BlockSize / 8];
                int bytesRead = memoryStream.Read(iv, 0, iv.Length);

                if (bytesRead != iv.Length)
                {
                    throw new ArgumentException("Az IV olvasása sikertelen. A fájl sérült lehet.");
                }

                aes.IV = iv;
                aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);

                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cryptoStream))
                    {
                        try
                        {
                            return reader.ReadToEnd();
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"Dekódolási hiba: {e.Message}");
                            throw;
                        }
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class SaveVehicleDataWrapper
    {
        public List<VehicleData> saveVehicleDataList;
    }
}
