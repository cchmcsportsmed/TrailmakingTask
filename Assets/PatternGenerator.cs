using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternGenerator : MonoBehaviour
{
    public int NumPoints=0;

    public void setNumPoints(string num)
    {
        NumPoints = int.Parse(num);
        print(NumPoints);
    }

    public void GenerateGrid(string type)
    {
        switch(type)
        {
            case "A":
            {
                
                break;
            }
            

            case "B":
            {
            
                break;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
