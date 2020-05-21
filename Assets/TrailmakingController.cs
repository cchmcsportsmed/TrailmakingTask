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
    public float taskTime;
    bool taskStarted;
    bool canStart = true;
    public LayerMask raycastMask;
    StringBuilder data;
    public List<GameObject> targets;
    public List<Vector3> targetPos;
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
    public void startTask()
    {
        taskStarted = false;
        taskTime = 0;
        canStart = true;
        data = new StringBuilder();
        hitTargets = new List<GameObject>();
        mouseObj.GetComponent<TrailRenderer>().Clear();
        if (currentTask ==0)
        {
            targets = new List<GameObject>();
            for (int i = 0; i < targetPos.Count; i++)
            {
                GameObject newTarget = Instantiate(targetPrefab);
                Text targetText = newTarget.transform.Find("Text").GetComponent<Text>();
                //Target Text
                targetText.text = (i + 1).ToString();
                //Target Position
                Vector3 newTargetPos = Vector3.zero;
                newTargetPos = targetPos[i];
                newTargetPos.z = Camera.main.nearClipPlane;
                newTarget.transform.position = newTargetPos;
                newTarget.transform.parent = canvas.transform;
                targets.Add(newTarget);
            }
        }
        else if (currentTask ==1)
        {
            for (int i = 0; i < taskBNumbers; i++)
            {
                GameObject newTarget = Instantiate(targetPrefab);
                Text targetText = newTarget.transform.Find("Text").GetComponent<Text>();
                //Target Text
                targetText.text = (i + 1).ToString();
                //Target Position
                Vector3 newTargetPos = Vector3.zero;
                newTargetPos = targetPosTaskB[i];
                newTargetPos.z = Camera.main.nearClipPlane;
                newTarget.transform.position = newTargetPos;
                newTarget.transform.parent = canvas.transform;
                targets.Add(newTarget);
            }
            for (int i = 0; i < targetPosTaskB.Count - taskBNumbers; i++)
            {
                GameObject newTarget = Instantiate(targetPrefab);
                Text targetText = newTarget.transform.Find("Text").GetComponent<Text>();
                //Target Text
                targetText.text = letters[i];
                //Target Position
                Vector3 newTargetPos = Vector3.zero;
                newTargetPos = targetPosTaskB[taskBNumbers+i];
                newTargetPos.z = Camera.main.nearClipPlane;
                newTarget.transform.position = newTargetPos;
                newTarget.transform.parent = canvas.transform;
                targets.Add(newTarget);
            }
        }
        else if (currentTask == 2)
            {
                targets = new List<GameObject>();
                for (int i = 0; i < targetPos_PA.Count; i++)
                {
                    GameObject newTarget = Instantiate(targetPrefab);
                    Text targetText = newTarget.transform.Find("Text").GetComponent<Text>();
                    //Target Text
                    targetText.text = (i + 1).ToString();
                    //Target Position
                    Vector3 newTargetPos = Vector3.zero;
                    newTargetPos = targetPos_PA[i];
                    newTargetPos.z = Camera.main.nearClipPlane;
                    newTarget.transform.position = newTargetPos;
                    newTarget.transform.parent = canvas.transform;
                    targets.Add(newTarget);
                }
            }
        else if (currentTask == 3)
        {
            for (int i = 0; i < (taskBNumbers/3); i++)
            {
                GameObject newTarget = Instantiate(targetPrefab);
                Text targetText = newTarget.transform.Find("Text").GetComponent<Text>();
                //Target Text
                targetText.text = (i + 1).ToString();
                //Target Position
                Vector3 newTargetPos = Vector3.zero;
                newTargetPos = targetPos_PB[i];
                newTargetPos.z = Camera.main.nearClipPlane;
                newTarget.transform.position = newTargetPos;
                newTarget.transform.parent = canvas.transform;
                targets.Add(newTarget);
            }
            for (int i = 0; i < targetPos_PB.Count - (taskBNumbers / 3); i++)
            {
                GameObject newTarget = Instantiate(targetPrefab);
                Text targetText = newTarget.transform.Find("Text").GetComponent<Text>();
                //Target Text
                targetText.text = letters[i];
                //Target Position
                Vector3 newTargetPos = Vector3.zero;
                newTargetPos = targetPos_PB[(taskBNumbers / 3) + i];
                newTargetPos.z = Camera.main.nearClipPlane;
                newTarget.transform.position = newTargetPos;
                newTarget.transform.parent = canvas.transform;
                targets.Add(newTarget);
            }
        }
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
        taskInitialized = true;
        for (int i=0;i<targets.Count;i++)
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(targets[i].transform.position);
            string output = "Target "+(i+1).ToString()+ ":";
            for (int x = 0; x< 3; x++)
            {
                output += targetPos[x].ToString("f4");
                if (x < 2) output += ",";
            }
            print(output);
        }
    }
    public IEnumerator endTask()
    {
        yield return gui.showOverlay(3, "Task Completed");
        taskStarted = false;
        writer.saveData();
        taskInitialized = false;
        while (targets.Count > 0)
        {
            GameObject target = targets[0];
            targets.RemoveAt(0);
            Destroy(target);
        }
        mouseObj.GetComponent<TrailRenderer>().Clear();
        gui.showGUI();
    }
    // Update is called once per frame
    void Update()
    {if (taskInitialized) {
            if (Input.GetMouseButton(0))
            {
                if (!taskStarted && canStart)
                {
                    //print("Go!");
                    taskStarted = true;
                    canStart = false;
                }
                else if (taskStarted)
                {
                    mouseObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                    taskTime += Time.deltaTime;
                    writer.addToCSV(taskTime.ToString() + "," + mouseObj.transform.position.x.ToString() + "," + mouseObj.transform.position.y.ToString() + "\n");
                    string output = "";
                    for (int i = 0; i < 3; i++)
                    {
                        output += mouseObj.transform.position[i].ToString("f4");
                        if (i < 2) output += ",";
                    }
                    print(output);
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, raycastMask))
                    {
                        data.Append(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)) + ",");
                        //print("Hit a target.");
                        if (hit.collider.gameObject != null)
                        {
                            Text targetText = hit.collider.gameObject.transform.Find("Text").GetComponent<Text>();
                            if (!hitTargets.Contains(hit.collider.gameObject))
                            {
                                hitTargets.Add(hit.collider.gameObject);
                                if (targetText != null)
                                {
                                    int numTargets;
                                    if (currentTask == 0) { numTargets = targetPos.Count; }
                                    else if (currentTask == 1) { numTargets = targetPosTaskB.Count; }
                                    else if (currentTask == 2) { numTargets = targetPos_PA.Count; }
                                    else if (currentTask == 3) { numTargets = targetPos_PB.Count; }
                                    else { numTargets = 1; }
                                    //print(targetText.text);
                                    if (hitTargets.Count == numTargets)
                                    {
                                        //print("Hit final target.");
                                        StartCoroutine(endTask());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
