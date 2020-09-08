using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public string boardPath;
    public string locationPath;
    public string colorPath;

    private void Awake()
    {
        boardPath = Application.dataPath + "/File/boards.txt";
        locationPath = Application.dataPath + "/File/locations.txt";
        colorPath = Application.dataPath + "/File/colors.txt";
    }

    public T loadData<T>(string s)
    {
        BinaryFormatter bf = new BinaryFormatter();
        Byte[] by = Convert.FromBase64String(s);
        MemoryStream sr = new MemoryStream(by);

        return (T)bf.Deserialize(sr);
    }

    public List<T> loadAllData<T>(string path)
    {
        List<T> locations = new List<T>();
        using (StreamReader reader = new StreamReader(File.Open(path, FileMode.Open)))
        {
            string dat = reader.ReadLine();
            while (dat != "")
            {
                locations.Add(loadData<T>(dat));
                dat = reader.ReadLine();
            }
        }
        return locations;
    }

    public string saveData<T>(T board)
    {
        string s = "";

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, board);

        s = Convert.ToBase64String(ms.ToArray());
        return s;
    }

    public void saveAllData<T>(List<T> data, string path)
    {
        string[] boardStrings = new string[data.Capacity];
        int i = 0;
        foreach (T dat in data)
        {
            boardStrings[i++] = saveData<T>(dat);
        }

        saveDataToFile(boardStrings, path);
    }

    private void saveDataToFile(string[] data, string path)
    {
        using (StreamWriter writer = new StreamWriter(File.Open(path, FileMode.Truncate)))
        {
            foreach (string s in data)
                writer.WriteLine(s);
        }
    }
}
