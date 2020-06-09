// This program is used to generate new patterns by the user. 
// The design is based off of default designs. 
// Author : Manish Anand
using System.Collections.Generic;
using UnityEngine;


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
        ClearDesign();
        design.designmode = true;
        design.currentTask = currentTask;
        design.startTask();
    }

        public void SaveDesign()
    {
        design.saveDesign = true;

    }
        public void ClearDesign()
    {
        design.clearframe();
    }
}