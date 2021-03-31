using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveHelper 
{
 
    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.kaz";
        FileStream stream = new FileStream(path, FileMode.Create);
        DataHelper data = new DataHelper();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static DataHelper LoadData()
    {
        string path =  Application.persistentDataPath + 
            "/data.kaz";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            DataHelper data = formatter.Deserialize(stream) as DataHelper;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }

    }

}
