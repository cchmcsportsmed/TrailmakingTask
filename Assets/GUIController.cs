using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

    public string subjectID = "";
    public GameObject canvas;
    public TrailmakingController task;
    public GameObject overlay;
    public Text overlayText;
    // Use this for initialization
    public IEnumerator showOverlay(float duration, string text)
    {
        overlayText.text = text;
        overlay.SetActive(true);
        yield return new WaitForSeconds(duration);
        overlay.SetActive(false);
    }
    public void startTask(int taskID)
    {
        if (subjectID != "")
        {
            task.currentTask = taskID;
           
            task.writer.trialID+=1;
            canvas.SetActive(false);
            task.writer.setFileName();
            task.startTask();
        }
        else { StartCoroutine(showOverlay(2, "Enter Subject ID First!")); }
    }
    public void setSubjectID(string newID)
    {
        subjectID = newID;
        task.writer.subID = subjectID;
        
    }
    public void showGUI()
    {
        canvas.SetActive(true);
    }
    

}
