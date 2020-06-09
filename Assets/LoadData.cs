using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadData : MonoBehaviour
{
    private string path = "Assets/Data.json";
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
