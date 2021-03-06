﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TrailmakingController : MonoBehaviour
{
    public GameObject targetPrefab;
    public List<float> xRange;
    public List<float> yRange;
    public GameObject mouseObj;
    public Canvas canvas;
    public float taskTime;
    bool taskStarted;
    bool canStart = true;
    public LayerMask raycastMask;
    StringBuilder data;
    public List<GameObject> targets;
    private List<string> targetsSequence;
    public List<Vector3> targetPosTaskA;
    public List<Vector3> targetPosTaskB;
    public List<Vector3> targetPos_PA;
    public List<Vector3> targetPos_PB;
    public DataWriter writer;
    public string fileName;
    public List<string> header;
    public int currentTask;
    public int taskBNumbers;
    bool taskInitialized = false;
    public GUIController gui;
    public List<GameObject> hitTargets;
    private GameObject wrongHit;
    // Use this for initialization
    public LoadData dataLoader;
    public dataStore[] dataS = new dataStore[6];
    public bool designmode = false;
    private GameObject desObject;
    private bool isHeld = false;
    public int NumPoints;
    float zPos;
    float startposX;
    float startposy;
    public bool saveDesign = false;
    public int ctr;
    
    public void initialize()
    {
        dataS = dataLoader.dataS; // contains the Practise data [0, 1], default task data [2, 3] and the user task data  respectively
        ctr=0;
        taskStarted = false;
        taskTime = 0;
        canStart = true;
        data = new StringBuilder();
        hitTargets = new List<GameObject>();
        clearTrail();
        taskInitialized = true;
    }
    public void startTask()
    {   
        initialize();
        // Render targets 
        Targets TaskTargets = new Targets();
        TaskTargets.currentTask = currentTask;
        
        List<Vector3> pattern = new List<Vector3>(dataS[currentTask].positions); // create a new list.
        // pattern = dataS[currentTask].positions;
        TaskTargets.targetPositions = pattern;

        if (designmode)
        {
            int i =NumPoints;
            pattern.RemoveRange(NumPoints, pattern.Count - NumPoints);
            taskInitialized = false;
        }

        TaskTargets.canvas = canvas;
        TaskTargets.targetPrefab = targetPrefab;
        TaskTargets.render();
        targets = TaskTargets.targets;
        targetsSequence = TaskTargets.targetsSequence;

        writer.createFile();
        foreach (string item in header)
        {
            writer.addToCSV(item);
            if (header.IndexOf(item) != header.Count - 1)
            {
                writer.addToCSV(",");
            }
            else
            {
                writer.addToCSV("\n");
            }
        }
    }
    // Clear target points and trail from the screen
    public void clearframe()
    {
         while (targets.Count > 0)
        {
            GameObject target = targets[0];
            targets.RemoveAt(0);
            Destroy(target);
        }
        clearTrail();
    }
    // Call functiont to remove the trail from the screen
    public void clearTrail()
    {
        mouseObj.GetComponent<TrailRenderer>().Clear();
    }
    public IEnumerator endTask()
    {
        yield return gui.showOverlay(3, "Task Completed");
        taskStarted = false;
        writer.saveData();
        taskInitialized = false;
        clearframe();
        gui.showGUI();
    }
    public IEnumerator saveTask()
    {
        int ix = 2+currentTask;
        dataS[ix].positions.Clear();
        foreach(GameObject target in targets)
        {
            dataS[ix].positions.Add(target.transform.position);
        }
        // targets.Clear();
        clearframe();
        yield return gui.showOverlay(3, "New task design saved");
        dataLoader.SaveData();
    }
 
 // Update is called once per frame
    void Update()
    {
        //play mode
        if (taskInitialized) 
        {
            if (Input.GetMouseButton(0))
            {
                if (!taskStarted && canStart)
                {
                    taskStarted = true;
                    canStart = false;
                }
                else if (taskStarted)
                {
                    // generate a trail that follows the mouse when the left button is clicked
                    mouseObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                    taskTime += Time.deltaTime;
                    Vector3 savePos = Camera.main.WorldToScreenPoint(mouseObj.transform.position); // Task coordinates in the screen coordinates.
                    writer.addToCSV(taskTime.ToString() + "," + savePos.x.ToString("f0") + "," + savePos.y.ToString("f0") + "\n"); // save cursor coordiantes in current frame to the output
                    string output = "";
                    for (int i = 0; i < 3; i++)
                    {
                        output += mouseObj.transform.position[i].ToString("f4");
                        if (i < 2) output += ",";
                    }
                    
                    // print(output);
                    // Detect collision of mouse with targets
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, raycastMask))
                    {
                        data.Append(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)) + ",");
                        if (hit.collider.gameObject != null)
                        {
                            Text targetText = hit.collider.gameObject.transform.Find("Text").GetComponent<Text>();
                            if (!hitTargets.Contains(hit.collider.gameObject))
                            {
                                if(wrongHit != null)
                                {
                                    var spr = wrongHit.gameObject.GetComponent<SpriteRenderer>(); 
                                    spr.color = new Color(255 ,255 ,255); // change the color of the wrong hit back to white
                                    wrongHit = null; //reset the wrong hit
                                }
                                 var sprite= hit.collider.gameObject.GetComponent<SpriteRenderer>();
                                if (targetText.text == targetsSequence[ctr])
                                {   
                                    sprite.color = new Color(0 ,255 ,0); // Change color of correctly hit target to green
                                    hitTargets.Add(hit.collider.gameObject); // add target to list only if hit in correct sequence
                                    ctr++;
                                    
                                }
                                else
                                {   
                                    sprite.color = new Color(255 ,0 ,0); // Change color of wrongly hit target to red
                                    wrongHit = hit.collider.gameObject;
                                }
                                
                                
                                if (targetText != null)
                                {
                                    if (hitTargets.Count == targets.Count)
                                    {
                                        StartCoroutine(endTask());
                                    }
                                }
                                
                            }
                        }
                    }
                }
            }
        }
        // design level
        if (designmode)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            // if no object is being held
            if(!isHeld)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, raycastMask) && Input.GetMouseButton(0))
                {   
                    // print("Hit Target");
                    desObject = hit.collider.gameObject; 
                    isHeld = true;
                    zPos = desObject.transform.position.z - Camera.main.transform.position.z;
                    mousePos.z = zPos ;
                    mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                    startposX=mousePos.x - desObject.transform.position.x; // To prevent snapping of grabebd object
                    startposy=mousePos.y - desObject.transform.position.y;
                }
            }
            else
            {   
                    if (Input.GetMouseButton(0))
                    {
                        mousePos.z = zPos;
                        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                        desObject.transform.position = new Vector3(mousePos.x - startposX, mousePos.y - startposy, zPos + Camera.main.transform.position.z);     // drag target around               
                    }
                    else    {isHeld = false;} 
            }
            if (saveDesign == true)
            {   
                saveDesign = false;
                StartCoroutine(saveTask());
            }
        }
    }
}
