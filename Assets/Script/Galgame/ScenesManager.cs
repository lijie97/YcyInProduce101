using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScenesManager : MonoBehaviour {
    int index, afs;
    int maxAFS = 300;
    public Text textObj;
    public GameObject bgmObj;
    public GameObject btnObj;
    public Image backgroundObj;
    public Image peopleObj;
    string[] fileData;
    private string newText;
    // Use this for initialization
    void Start () {
        index = 0;
        Debug.Log("start");
        //GameObject btnObj = GameObject.Find("Canvas/Button");
        Button btn = (Button)btnObj.GetComponent<Button>();
        btn.onClick.AddListener(onClick);
        readCSV();
    }
    void readCSV() {
        string filePath = Application.streamingAssetsPath;
        Debug.Log(filePath);
        csvController.GetInstance().loadFile(filePath, "novel.csv ");
        fileData = File.ReadAllLines(filePath);
     
    }
    void onClick() {
        //Debug.Log("On click");
        index += 1;
        if (afs > 0) afs = 0;
        else afs = maxAFS;
        newText = csvController.GetInstance().getString(index, 2);
        Debug.Log(newText);
    }
	void Update () {
        
        if (afs > 0) afs--;
	}
}
