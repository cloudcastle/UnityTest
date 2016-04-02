using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class FileManager
{
    public static T LoadFromFile<T>(string filename) where T : class {
        filename = Application.persistentDataPath + "/" + filename;
        T result = null;
        FileStream fs = null;
        try {
            var bf = new BinaryFormatter();
            fs = new FileStream(filename, FileMode.Open);
            result = bf.Deserialize(fs) as T;
        } catch (Exception e) {
            Debug.LogException(e);
            Debug.Log(e.StackTrace);
        } finally {
            if (fs != null) fs.Close();
        }
        return result;
    }

    public static void SaveToFile<T>(T data, string filename) {
        filename = Application.persistentDataPath + "/" + filename;
        FileStream fs = null;
        try {
            var bf = new BinaryFormatter();
            fs = new FileStream(filename, FileMode.Create);
            bf.Serialize(fs, data);
        } catch (Exception e) {
            Debug.LogException(e);
            Debug.Log(e.StackTrace);
        } finally {
            if (fs != null) fs.Close();
        }
    }
}
