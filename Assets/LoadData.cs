﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadData : MonoBehaviour
{
    //    [SerializeField]
    //    private TextAsset datafile; 

    public string path;
    public string filename = "Data2.json";
    // Start is called before the first frame update
    public TrailmakingController controller;
    TaskData taskData = new TaskData();
    // List<dataStore> taskData = new List<dataStore>(); 
    dataStore pracTask_A = new dataStore();
    dataStore pracTask_B = new dataStore();
    dataStore Task_A = new dataStore();
    dataStore Task_B= new dataStore();
    dataStore userTask_A = new dataStore();
    dataStore userTask_B = new dataStore();
    public dataStore[] dataS = new dataStore[6];
    void Start()
    {
        // path = Application.persistentDataPath + "/" + filename;
        path = "/home/quest/Drive/CCHMC/Codes/TrailMaking Task/TrailmakingTask/Assets/" + filename;
        // SaveData();
        // initialDataGeneration();
        ReadData();
    }

    public void initialDataGeneration()
    {
        pracTask_A.task_name="Practice Task A";
        pracTask_B.task_name="Practice Task B";
        Task_A.task_name="Task A";
        Task_B.task_name="Task B";
        userTask_A.task_name="User Task A";
        userTask_B.task_name="User Task B";

        pracTask_A.positions=controller.targetPos_PA;
        pracTask_B.positions=controller.targetPos_PB;
        Task_A.positions=controller.targetPosTaskA;
        Task_B.positions=controller.targetPosTaskB;
        userTask_A.positions=Task_A.positions;
        userTask_B.positions=Task_B.positions;

        taskData.Items.Add(pracTask_A);
        taskData.Items.Add(pracTask_B);
        taskData.Items.Add(Task_A);
        taskData.Items.Add(Task_B);
        taskData.Items.Add(userTask_A);
        taskData.Items.Add(userTask_B);
        SaveData();
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
