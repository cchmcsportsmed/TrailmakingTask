using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataDeserializer : MonoBehaviour
{

    public string filePath;
    string output;
    // Use this for initialization
    void Start()
    {
        string[] fileArray = Directory.GetFiles(filePath);
        print (fileArray.Length);
        foreach (String file in fileArray)
        {
            if (file.Contains(".dat"))
            {
                FileStream fileStream = File.Open(file, FileMode.OpenOrCreate);
                BinaryFormatter serializer = new BinaryFormatter();
                DataWriter.data taskData = (DataWriter.data)serializer.Deserialize(fileStream);
                fileStream.Close();
                createCSV(file, taskData);
            }
        }
        
        //fileStream.Close();      
    }
    void createCSV(String file,DataWriter.data taskData)
    {
        String fileName = file.Replace(".dat",".csv");
        int counter = 0;
        while (File.Exists(fileName))
        {
            counter++;
            fileName = fileName.Replace(".csv", "-" + counter.ToString() + ".csv");
        }
        File.WriteAllText(fileName, taskData.saveData);
    }
}
