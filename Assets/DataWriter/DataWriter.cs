using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
#if WINDOWS_UWP
using Windows.Storage;
using System.Threading.Tasks;
using System;
using System.Text;
#else
using System.Text;
#endif


public class DataWriter : MonoBehaviour
{
    public string subIDFile = "SubID.csv";
    public int trialID = 0;
    private string timeStamp;
    private string fileName;
    public bool writing = false;
    public string subID;
    public StringBuilder csv = new StringBuilder();
    public void createFile()
    {
        csv = new StringBuilder();
        //setFileName();
    }
    public void setFileName()
    {
        fileName=Application.persistentDataPath+ "\\Subject_" + subID.ToString() + "_Trial_" + trialID.ToString() + ".dat";
    }
#if WINDOWS_UWP
    StorageFolder rootFolder;
    StorageFile csvFile;
    public async Task createFile()
    {
        csvFile = await rootFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
        print(csvFile.Path);
    }
    public void addToCSV(string data)
    {
        csv.Append(data);
        writing = false;
    }
    public async Task writeToFile(string data)
    {

        await FileIO.AppendTextAsync(csvFile, data);
        writing = false;
    }
    public async Task saveData()
    {
        await FileIO.AppendTextAsync(csvFile, csv.ToString());
        writing = false;
    }
    
    // Use this for initialization
    void Awake () 
    {
        rootFolder = ApplicationData.Current.LocalFolder;
        print("Root Folder Set To:" + rootFolder.DisplayName);
    }
    public void Start(){
    }
#else
    public void addToCSV(string data)
    {
        csv.Append(data);
        writing = false;
    }
    public void saveData()
    {
        print("saving data");
        FileStream fileStream = File.Open(fileName, FileMode.OpenOrCreate);
        data taskData = new data();
        taskData.saveData = csv.ToString();
        BinaryFormatter serializer = new BinaryFormatter();
        serializer.Serialize(fileStream, taskData);
        fileStream.Close();
    }
    [Serializable]
    public class data
    {
        public string saveData;
    }
#endif
}
