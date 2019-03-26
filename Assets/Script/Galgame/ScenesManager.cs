using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScenesManager : MonoBehaviour {
    int index, afs,stringlen,lenPrinted;
    int maxAFS = 220;
    int maxScenes;
    public GameObject bgmObj;
    private string Res_BGPath = "Background/";
    string[] fileData;
    GameObject bgObj;
    private string nextString;
    private Text textOnBoard;
    private string nextPeoplePicName;
    private string nextBgPicName;
    private Sprite nextBgSprite;
    // Use this for initialization
    void Start () {
        index = 0;
        //Debug.Log("start");
        GameObject bgObj = GameObject.Find("Canvas/Background");
        GameObject btnObj = GameObject.Find("Canvas/Button");
        GameObject textObj = GameObject.Find("Canvas/Diag-Box/Text");
        GameObject peopleObj = GameObject.Find("Canvas/People");
        Button btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(onClick);
        textOnBoard = textObj.GetComponent<Text>();
        readCSV();
        maxScenes = csvController.GetInstance().getSizeY();
        afs = maxAFS;
        nextString = csvController.GetInstance().getString(index, 3);
        stringlen = nextString.Length;
        textOnBoard.text = "";
        lenPrinted = 0;
    }
    void readCSV() {
        csvController.GetInstance().loadFile();
    }

    void onClick() {
        if (afs > 0) afs = 0;
        else{
            index += 1;
            afs = maxAFS;
            nextString = csvController.GetInstance().getString(index, 3);
            nextPeoplePicName = csvController.GetInstance().getString(index, 2);
            nextBgPicName = csvController.GetInstance().getString(index, 2);
            stringlen = nextString.Length;
            lenPrinted = 0;
            nextBgSprite = Resources.Load(Res_BGPath + nextBgPicName, typeof(Sprite)) as Sprite;

        }
        
    }

    void Update() { 
        if (afs > 0) afs--;
        
        if (maxAFS - afs > lenPrinted * (maxAFS/stringlen)) {
            lenPrinted++;
            if (lenPrinted<=stringlen) textOnBoard.text = nextString.Substring(0,lenPrinted);
        }
    }
}
