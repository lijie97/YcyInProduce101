using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScenesManager : MonoBehaviour {
    int index, afs,stringlen,lenPrinted, maxScenes;
    int maxAFS = 220;
    public GameObject bgmObj;
    //private string Res_BGPath = "Background/";
    string[] fileData;
    GameObject bgObj;
    private string nextString, nextBgPicName, nextPsPicName;
    private Text textOnBoard, textPeopleName;
    private Image nextBgImg,nextBgImgLocader,bgImg,psImg,nextPsImg;
    private bool visible;
    GameObject nextBgObj, nextPsObj, PsObj;
    // Use this for initialization
    void Start () {
        index = 1;
        //Debug.Log("start");
        PsObj = GameObject.Find("Canvas/Person");
        GameObject bgObj = GameObject.Find("Canvas/Background");
        //GameObject btnObj = GameObject.Find("Canvas/Button");
        //GameObject textObj = GameObject.Find("Canvas/Diag-Box/Text");
        //GameObject nextPsObj = GameObject.Find("Canvas/NestPerson");
        //GameObject textPeopleNameObj = GameObject.Find("Canvas/Diag-Box/NameCard/Text");
        //GameObject peopleObj = GameObject.Find("Canvas/People");
        nextBgObj = GameObject.Find("Canvas/NextBackground");
        nextPsObj = GameObject.Find("Canvas/NextPerson");
        nextPsImg = GameObject.Find("Canvas/NestPerson").GetComponent<Image>();
        Button btn = GameObject.Find("Canvas/Button").GetComponent<Button>();
        textPeopleName = GameObject.Find("Canvas/Diag-Box/NameCard/Text").GetComponent<Text>();
        nextBgImg = nextBgObj.GetComponent<Image>();
        bgImg = bgObj.GetComponent<Image>();
        btn.onClick.AddListener(onClick);
        textOnBoard = GameObject.Find("Canvas/Diag-Box/Text").GetComponent<Text>();
        csvController.GetInstance().loadFile(); //readCSV();
        //########从csv读取第一帧的信息########
        maxScenes = csvController.GetInstance().getSizeY();
        //afs = maxAFS;
        nextString = csvController.GetInstance().getString(index, 3);
        //Debug.Log();
        textPeopleName.text = csvController.GetInstance().getString(index, 2);
        nextPsPicName = csvController.GetInstance().getString(index, 1);
        nextBgPicName = csvController.GetInstance().getString(index, 4);
        //Debug.Log(nextBgPicName);
        nextBgImg.sprite = Resources.Load("Background/" + nextBgPicName, typeof(Sprite)) as Sprite;
        nextPsImg.sprite = Resources.Load("img_girls/" + nextPsPicName, typeof(Sprite)) as Sprite;
        stringlen = nextString.Length;
        textOnBoard.text = "";
        lenPrinted = 0;

    }

    void onClick() {
        if (afs > 0) afs = 0;
        else{
            index = csvController.GetInstance().getInt(index, 6);
            afs = maxAFS;
            nextString = csvController.GetInstance().getString(index, 3);
            nextPsPicName = csvController.GetInstance().getString(index, 1);
            stringlen = nextString.Length;
            lenPrinted = 0;
            textPeopleName.text = csvController.GetInstance().getString(index, 2);
            if (nextBgPicName!=csvController.GetInstance().getString(index, 4)) { 
                nextBgPicName = csvController.GetInstance().getString(index, 4);
                nextBgImg.sprite = Resources.Load("Background/" + nextBgPicName, typeof(Sprite)) as Sprite;
                nextPsImg.sprite = Resources.Load("img_girls/" + nextPsPicName, typeof(Sprite)) as Sprite;
                nextBgObj.GetComponent<Appear>().appearing = true;
                nextPsObj.GetComponent<Appear>().appearing = true;
                nextPsObj.GetComponent<Appear>().disappearing = true;
                //BgObj.GetComponent<Appear>().appearing = false;

            }

        }
        
    }

    void Update() {
        if (afs == 0)
        {
            bgImg = nextBgImg;
            psImg = nextPsImg;
            PsObj.GetComponent<Appear>().fillAlphaTunel();

        }
        if (afs > 0) afs--;
        if (afs % 5 == 0) {
            lenPrinted++;
            if (lenPrinted<=stringlen) textOnBoard.text = nextString.Substring(0,lenPrinted);
        }
        

    }
}
