﻿// Public class to specify and generate a set of way points from
// coordinate positions and task type (A or B). Contains the rendering 
// function as well.
// Author : Manish Anand
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Targets
{
    public List<Vector3> targetPositions;
    public int currentTask=0;
    public GameObject targetPrefab;
    // public List<GameObject> targets;
    public Canvas canvas;
    List<string> letters  = new List<string> { "A", "B", "C", "D", "E", "F", "G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z" };
    public List<GameObject> targets = new List<GameObject>();
    public List<string> targetsSequence = new List<string>();
    public void render()
    {   
        int count = targetPositions.Count;
        int start = 0;
        if(currentTask == 1 || currentTask == 3)
        {
            display(start,count/2,false);
                start = count/2;
            display(start,count,true);
        }
        else
        {
            display(start,count,false);
        }

        for(int i =1; i <= count; i++)
        {
            targetsSequence.Add(i.ToString());
            if(currentTask == 1 || currentTask == 3)
            {
                targetsSequence.Add(letters[i-1]);
            }
        }
    }

    private void display(int start, int count, bool prLetters)
    {
        // targets = new List<GameObject>();
        for (int i = start; i < count; i++)
        {
            GameObject newTarget = GameObject.Instantiate(targetPrefab);
            Text targetText = newTarget.transform.Find("Text").GetComponent<Text>();
            
            //Target Text
            if (prLetters)  {targetText.text = letters[i-start];}  // prints letters
            else    {targetText.text = (i + 1).ToString();}          //prints numbers
     
            //Target Position
            Vector3 newTargetPos = Vector3.zero;
            newTargetPos = targetPositions[i];
            newTargetPos.z = Camera.main.nearClipPlane;
            newTarget.transform.position = newTargetPos;
            newTarget.transform.SetParent( canvas.transform);
            targets.Add(newTarget);
        } 
    }
}
