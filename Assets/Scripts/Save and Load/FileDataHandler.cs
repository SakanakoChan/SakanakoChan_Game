using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro.EditorUtilities;
using System;

//Save location:
//C:\Users\megum\AppData\LocalLow\DefaultCompany\BrandNewRPGGame_2nd

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool encryptData = false;
    private string codeWord = "Zakozako~";

    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _encryptData)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        encryptData = _encryptData;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            //create file directory to store the save file
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //parse the savedata to json, true means the json file will be formatted and easier to read
            string dataToStore = JsonUtility.ToJson(_data, true);

            if (encryptData)
            {
                dataToStore = EncryptAndDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error: Failed to save data file: \n{fullPath}\n {e.Message}");
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (encryptData)
                {
                    dataToLoad = EncryptAndDecrypt(dataToLoad);
                }

                //read json from the save file to gamedata
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.Log($"Failed to load game data from:\n{fullPath}\n{e.Message}");
            }
        }

        return loadData;
    }

    public void DeleteSave()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    private string EncryptAndDecrypt(string _data)
    {
        string result = "";

        for (int i = 0; i < _data.Length; i++)
        {
            // ^ means XOR, ���
            result += (char)(_data[i] ^ codeWord[i % codeWord.Length]);
        }

        return result;
    }

}
