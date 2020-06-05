using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadData : MonoBehaviour
{
    //    [SerializeField]
    //    private TextAsset datafile; 

    public string path;
    public string filename = "Data.json";
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
    void Start()
    {
        // path = Application.persistentDataPath + "/" + filename;
        path = "/home/quest/Drive/CCHMC/Codes/TrailMaking Task/TrailmakingTask/Assets/" + filename;
        print(path);
        // pracTask_A.count=controller.targetPos_PA.Count;
        // pracTask_B.count=controller.targetPos_PB.Count;
        // Task_A.count=controller.targetPos.Count;
        // Task_B.count=controller.targetPosTaskB.Count;
        
        pracTask_A.task_name="Practice Task A";
        pracTask_B.task_name="Practice Task B";
        Task_A.task_name="Task A";
        Task_A.task_name="Task A";

        pracTask_A.positions=controller.targetPos_PA;
        pracTask_B.positions=controller.targetPos_PB;
        Task_A.positions=controller.targetPos;
        Task_B.positions=controller.targetPosTaskB;

        // taskData.pracTask_A=pracTask_A;
        
        
        

        taskData.taskDataList.Add(pracTask_A);
        taskData.taskDataList.Add(pracTask_B);
        taskData.taskDataList.Add(Task_A);
        taskData.taskDataList.Add(Task_B);

        // for(int i=0;i<pracTask_A.positions.Count;i++)
        // {
        //     print(pracTask_A.positions[i]);
        // }
        SaveData();

        // ReadData();
    }


    void SaveData()
    {
        string contents = JsonUtility.ToJson(taskData, true);
        System.IO.File.WriteAllText(path , contents);

    }

    void ReadData()
    {
        // string contents = System.IO.File.ReadAllText(path);
        // taskData = JsonUtility.FromJson<TaskData>(contents);
        // print(taskData.count);

    }
    
}
