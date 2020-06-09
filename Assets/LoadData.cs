using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadData : MonoBehaviour
{
    // public TrailmakingController controller;
    private string path = "Assets/Data.json";
    TaskData taskData = new TaskData();
    dataStore pracTask_A = new dataStore();
    dataStore pracTask_B = new dataStore();
    dataStore Task_A = new dataStore();
    dataStore Task_B= new dataStore();
    dataStore userTask_A = new dataStore();
    dataStore userTask_B = new dataStore();
    public dataStore[] dataS = new dataStore[6];
    void Start()
    {
        ReadData();
    }
    
    public void SaveData()
    {
        string contents = JsonHelper.ToJson(dataS, true);
        System.IO.File.WriteAllText(path , contents);
    }

    public void ReadData()
    {   
        string contents = System.IO.File.ReadAllText(path);
        dataS = JsonHelper.FromJson<dataStore>(contents);        
    }
    
}
