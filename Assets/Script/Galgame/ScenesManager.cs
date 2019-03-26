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
    private Image nextBgImg,nextBgImgLocader;
    private int alphaTunel;
    GameObject nextBgObj;
    // Use this for initialization
    void Start () {
        index = 0;
        //Debug.Log("start");
        GameObject bgObj = GameObject.Find("Canvas/Background");
        GameObject btnObj = GameObject.Find("Canvas/Button");
        GameObject textObj = GameObject.Find("Canvas/Diag-Box/Text");
        //GameObject peopleObj = GameObject.Find("Canvas/People");
        nextBgObj = GameObject.Find("Canvas/NextBackground");
        Button btn = btnObj.GetComponent<Button>();
        nextBgImg = nextBgObj.GetComponent<Image>();
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
            nextBgImgLocader = Resources.Load(Res_BGPath + nextBgPicName, typeof(Image)) as Image;
            nextBgObj.GetComponent<Image>() = nextBgImgLocader;

            alphaTunel = 0;
        }
        
    }

    void Update() { 
        if (afs > 0) afs--;
        
        if (afs % 5 == 0) {
            lenPrinted++;
            if (lenPrinted<=stringlen) textOnBoard.text = nextString.Substring(0,lenPrinted);
        }
        alphaTunel += 5;
        if (alphaTunel <= 255) nextBgImg.color = new Color(255, 255, 255, alphaTunel);
        nextBgObj.GetComponent<Image>().color = new Color(255, 255, 255, alphaTunel);

    }
}
