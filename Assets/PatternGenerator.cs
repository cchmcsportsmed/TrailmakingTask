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
        ClearDesign();
        design.designmode = true;
        design.currentTask = currentTask;
        design.startTask();
    }

        public void SaveDesign()
    {

    }
        public void ClearDesign()
    {
        design.clearframe();
    }
}