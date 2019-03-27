using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScenesManager : MonoBehaviour
{
    int index, afs, stringlen, lenPrinted, maxScenes;
    int maxAFS = 220;
    public GameObject bgmObj;
    //private string Res_BGPath = "Background/";
    private string nowString, nowBgPicName, nowPsPicName;
    private Text textOnBoard, textPsName;
    private Image nowBgImg, nowBgImgLocader, nowPsImg;

    private bool visible,acc;
    GameObject nowBgObj, nowPsObj;
    // Use this for initialization
    /*IEnumerator loadPsImg()
    {
        AsyncOperation loadRequest = Resources.LoadAsync("img_girls/" + nowPsPicName, typeof(Sprite));
        yield return loadRequest;
        nowPsImg.sprite = loadRequest.bytes as Sprite;
    }*/

    //StartCoroutine(loadBackground());
    void Start()
    {
        index = 1;
        //Debug.Log("start");
       
        nowBgObj = GameObject.Find("Canvas/NowBackground");
        nowPsObj = GameObject.Find("Canvas/NowPerson");
        
        nowPsImg = nowPsObj.GetComponent<Image>();
        nowBgImg = nowBgObj.GetComponent<Image>();
        
        Button btn = GameObject.Find("Canvas/Button").GetComponent<Button>();
        textPsName = GameObject.Find("Canvas/DiagBox/NameCard/Text").GetComponent<Text>();
        btn.onClick.AddListener(onClick);
        textOnBoard = GameObject.Find("Canvas/DiagBox/Text").GetComponent<Text>();
        csvController.GetInstance().loadFile(); //readCSV();
        //########从csv读取第一帧的信息########
        maxScenes = csvController.GetInstance().getSizeY();
        getPsName();
        getPic();
        getText();
        getVisibility();
        afs = 0;
        nowBgObj.GetComponent<Appear>().appear();
        nowPsObj.GetComponent<Appear>().appear();
    }
    void getVisibility()
    {
        visible = csvController.GetInstance().getInt(index, 5)==1;
        //Debug.Log(111111111);
        if (!visible)
        {
            GameObject.Find("Canvas/DiagBox/NameCard").GetComponent<Appear>().disappear();
            GameObject.Find("Canvas/DiagBox").GetComponent<Appear>().disappear();
            //GameObject.Find("Canvas/Diag-Box").GetComponent<Appear>().disappear();
        }
        else {
            GameObject.Find("Canvas/DiagBox/NameCard").GetComponent<Appear>().appear();
            GameObject.Find("Canvas/DiagBox").GetComponent<Appear>().appear();
        }
    }
    void getPsName()
    {
        textPsName.text = csvController.GetInstance().getString(index, 2);
    }

    void getPic()
    {
       
        if (nowPsPicName != csvController.GetInstance().getString(index, 1)) {  
            nowPsPicName = csvController.GetInstance().getString(index, 1);
            if (nowPsPicName.Length != 0)
            {
                
                nowPsImg.sprite = Resources.Load("img_girls/" + nowPsPicName, typeof(Sprite)) as Sprite;
                nowPsObj.GetComponent<Appear>().ResetAlphaTunel();
            }
        else {
                nowPsImg.sprite = Resources.Load("透明", typeof(Sprite)) as Sprite;
                
            }
        }
        if (nowBgPicName != csvController.GetInstance().getString(index, 4))
        {
            nowBgPicName = csvController.GetInstance().getString(index, 4);
            if (nowBgPicName.Length != 0)
            {
               
                nowBgImg.sprite = Resources.Load("Background/" + nowBgPicName, typeof(Sprite)) as Sprite;
                nowBgObj.GetComponent<Appear>().ResetAlphaTunel();
            }
            else nowPsImg.sprite = Resources.Load("透明", typeof(Sprite)) as Sprite;
        }
    }
    void getText()
    {
        nowString = csvController.GetInstance().getString(index, 3);
        stringlen = nowString.Length;
        lenPrinted = 0;
    }
    int autoSlide() { 
        return csvController.GetInstance().getInt(index, 7)==0?int.MaxValue: csvController.GetInstance().getInt(index, 7);
    }
    void onClick()
    {
        if (lenPrinted < stringlen) { 
            lenPrinted++;
            acc = true;
        }
        else { 
            index = csvController.GetInstance().getInt(index, 6);
            getPsName();
            getText();
            getPic();
            getVisibility();
            afs = 0;
            acc = false;
        }



    }

    void Update()
    {
        if (afs >= autoSlide())
            onClick();
        afs ++;
        if (afs % 5 == 0 || acc) {
            if (lenPrinted <= stringlen) textOnBoard.text = nowString.Substring(0, lenPrinted);
            lenPrinted++;

        }

    }
}
