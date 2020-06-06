using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternGenerator : MonoBehaviour
{
    public int NumPoints=0;
    public Canvas canvas;
    public GameObject gamedesigner;
    public GameObject targetPrefab;
    public List<GameObject> targets;
    public LoadData dataLoader;

    public TrailmakingController design;

    public dataStore[] dataS = new dataStore[6];

    public void setNumPoints(string num)
    {
        NumPoints = int.Parse(num);
        design.NumPoints = NumPoints;
    }

    public void GenerateGrid(int currentTask)
    {

        design.designmode = true;
        design.startTask();


        
        // // dataStore[] dataS = new dataStore[6];
        // dataS = dataLoader.ReadData();
        // Targets TaskTargets = new Targets();
        // TaskTargets.currentTask = currentTask;
        // List<Vector3> pattern = dataS[currentTask].positions;
        
        // int i =NumPoints;
        // pattern.RemoveRange(NumPoints, pattern.Count - NumPoints);
        
        // TaskTargets.targetPositions = pattern;
        // TaskTargets.canvas = canvas;
        // TaskTargets.targetPrefab = targetPrefab;

        // TaskTargets.render();
        // targets = TaskTargets.targets;

 

    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
