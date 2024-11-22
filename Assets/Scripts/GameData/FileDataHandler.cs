using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler {
    //once again, all of this is from the tutorial "How to Make a Save & Load System in Unity", so I need to refer back to that if confused
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "bsection";

    public FileDataHandler (string dataDirPath, string dataFileName, bool useEncryption) {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load () {
        //different OSs have different path separators so use Path.Combine
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath)) {
            try {
                //load serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //decrypt optionally
                if (useEncryption) {
                    dataToLoad = EncryptDecryptXOR(dataToLoad);
                }

                //deserialize from json back to to GameData object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e) {
                Debug.Log("Error occurred while trying to load data from file: "+fullPath+"\n"+e);
            }
        }
        return loadedData;
    }

    public void Save (GameData data) {
        //different OSs have different path separators so use Path.Combine
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try {
            //create directory path
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            
            //serialize game data into json string
            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption) {
                dataToStore = EncryptDecryptXOR(dataToStore);
            }

            //write data to the file system, using closes the connection after we're done writing to it.
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e) {
            Debug.Log("Error occurred while trying to save data to file: "+fullPath+"\n"+e);
        }
    }

    //XOR encryption
    private string EncryptDecryptXOR (string data) {
        string modifiedData = "";
        for (int i = 0; i <data.Length; i++) {
            modifiedData += (char) (data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    //Caesar cipher encryption post release?

}
