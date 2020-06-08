using System.Collections;
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
    public GameObject GamePanel;
    public float taskTime;
    bool taskStarted;
    bool canStart = true;
    public LayerMask raycastMask;
    StringBuilder data;
    public List<GameObject> targets;
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
    List<string> letters  = new List<string> { "A", "B", "C", "D", "E", "F", "G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z" };
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
    
    public void startTask()
    {   
        dataS = dataLoader.dataS;
    
        taskStarted = false;
        taskTime = 0;
        canStart = true;
        data = new StringBuilder();
        hitTargets = new List<GameObject>();
        mouseObj.GetComponent<TrailRenderer>().Clear();
        taskInitialized = true;

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
        TaskTargets.panel = GamePanel;
        TaskTargets.targetPrefab = targetPrefab;
        TaskTargets.render();
        targets = TaskTargets.targets;

        writer.createFile();
        //writer.setFileName(fileName);
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
    
    public void clearframe()
    {
         while (targets.Count > 0)
        {
            GameObject target = targets[0];
            targets.RemoveAt(0);
            Destroy(target);
        }
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
        print(ix);
        print("saved");
        yield return gui.showOverlay(3, "New task design saved");
        dataS[ix].positions.Clear();
        foreach(GameObject target in targets)
        {
            dataS[ix].positions.Add(target.transform.position);
        }
        // targets.Clear();
        clearframe();
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
                    mouseObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                    taskTime += Time.deltaTime;
                    // writer.addToCSV(taskTime.ToString() + "," + mouseObj.transform.position.x.ToString() + "," + mouseObj.transform.position.y.ToString() + "\n");
                    writer.addToCSV(taskTime.ToString() + "," + Input.mousePosition.x + "," + Input.mousePosition.y.ToString() + "\n"); // save cursor coordiantes in current frame to the output
                    string output = "";
                    for (int i = 0; i < 3; i++)
                    {
                        output += mouseObj.transform.position[i].ToString("f4");
                        if (i < 2) output += ",";
                    }
                    // print(output);
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
                                hitTargets.Add(hit.collider.gameObject);
                                if (targetText != null)
                                {
                                    int numTargets = 1;
                                    numTargets = targets.Count;
                                    if (hitTargets.Count == numTargets)
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

                    startposX=mousePos.x - desObject.transform.position.x;
                    startposy=mousePos.y - desObject.transform.position.y;
                }
            }
            else
            {   
                    if (Input.GetMouseButton(0))
                    {
                        mousePos.z = zPos;
                        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                        desObject.transform.position = new Vector3(mousePos.x - startposX, mousePos.y - startposy, zPos + Camera.main.transform.position.z);                    
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
