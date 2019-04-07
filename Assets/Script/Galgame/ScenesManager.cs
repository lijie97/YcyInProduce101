using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ScenesManager : MonoBehaviour
{
    int index, afs, stringlen, lenPrinted, maxScenes,option;
    int maxAFS = 220;
    public GameObject bgmObj;
    //private string Res_BGPath = "Background/";
    string nowString, nowBgPicName, nowPsPicName;
    Text textOnBoard, textPsName,textOnBtnOpt1, textOnBtnOpt2, textOnBtnOpt3;
    Image nowBgImg, nowBgImgLocader, nowPsImg;
    bool visible,acc;
    public Button btn,btnOpt1, btnOpt2, btnOpt3;
    GameObject nowBgObj, nowPsObj;

    void Start()
    {
        index = 1;
        //Debug.Log("start");
      
        nowBgObj = GameObject.Find("Canvas/NowBackground");
        nowPsObj = GameObject.Find("Canvas/NowPerson");
        
        nowPsImg = nowPsObj.GetComponent<Image>();
        nowBgImg = nowBgObj.GetComponent<Image>();
        
        btn = GameObject.Find("Canvas/Button").GetComponent<Button>();
        btn.onClick.AddListener(onClick);

        GameObject obj = GameObject.Find("Canvas/OptionsButton1");
        if (obj) btnOpt1 = obj.GetComponent<Button>();
        obj = GameObject.Find("Canvas/OptionsButton2");
        if (obj) btnOpt2 = obj.GetComponent<Button>();
        //btnOpt2 = obj.GetComponent<Button>();
        obj = GameObject.Find("Canvas/OptionsButton3");
        if (obj) btnOpt3 = obj.GetComponent<Button>();
        //btnOpt3 = obj.GetComponent<Button>();

        btnOpt1.onClick.AddListener(onClickOpt1);
        btnOpt2.onClick.AddListener(onClickOpt2);
        btnOpt3.onClick.AddListener(onClickOpt3);
        

        textOnBtnOpt1 = GameObject.Find("Canvas/OptionsButton1/Text").GetComponent<Text>();
        textOnBtnOpt2 = GameObject.Find("Canvas/OptionsButton2/Text").GetComponent<Text>();
        textOnBtnOpt3 = GameObject.Find("Canvas/OptionsButton3/Text").GetComponent<Text>();
        activeOptions(false);
        textPsName = GameObject.Find("Canvas/DiagBox/NameCard/Text").GetComponent<Text>();
        textOnBoard = GameObject.Find("Canvas/DiagBox/Text").GetComponent<Text>();
        //Debug.Log(textOnBoard.text);

        csvController.GetInstance().loadFile(); //readCSV();
        //########从csv读取第一帧的信息########
        maxScenes = csvController.GetInstance().getSizeY();
        changeScene();
        getPsName();
        getPic();
        getOptions();
        getText();
        getVisibility();
        afs = 0;
        option = 0;
        nowBgObj.GetComponent<Appear>().appear();
        nowPsObj.GetComponent<Appear>().appear();

    }
    void activeOptions(bool conf)
    {
        //激活三个按钮
        btnOpt1.gameObject.SetActive(conf);
        btnOpt2.gameObject.SetActive(conf);
        btnOpt3.gameObject.SetActive(conf);
        //disactive 翻页按钮
        btn.gameObject.SetActive(!conf);
    }
    void getOptions() {
        if (csvController.GetInstance().getInt(index, 9) == 1)
        {
            activeOptions(true);            
            //更改三个按钮文本
            textOnBtnOpt1.text = csvController.GetInstance().getString(index, 10);
            textOnBtnOpt2.text = csvController.GetInstance().getString(index, 12);
            textOnBtnOpt3.text = csvController.GetInstance().getString(index, 14);
        }
    }

    void onClickOpt1() {
        option = 1;
        activeOptions(false);
        onClick();
    }
    void onClickOpt2()
    {
        option = 2;
        activeOptions(false);
        onClick();
    }
    void onClickOpt3()
    {
        option = 3;
        activeOptions(false);
        onClick();
    }
    void getVisibility()
    {
        visible = csvController.GetInstance().getInt(index, 6)==1;
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
        if (textPsName) textPsName.text = csvController.GetInstance().getString(index, 2);
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
        return csvController.GetInstance().getInt(index, 8)==0?int.MaxValue: csvController.GetInstance().getInt(index, 7);
    }
    void changeScene() {
        if (csvController.GetInstance().getString(index, 19).Length!= 0)
            SceneManager.LoadScene(csvController.GetInstance().getString(index, 19));
    }

    void onClick()
    {
        if (lenPrinted < stringlen) { 
            lenPrinted++;
            acc = true;
        }
        else {
            if (option == 0)
                index = csvController.GetInstance().getInt(index, 7);
            else { 
                index = csvController.GetInstance().getInt(index,9+2*option);
            }
            changeScene();
            getPsName();
            getText();
            getPic();
            getOptions();
            getVisibility();
            
            afs = 0;
            acc = false;
            option = 0;
        }
    }

    void Update()
    {
        if (afs >= autoSlide())
            onClick();
        afs ++;
        if (afs % 5 == 0 || acc) {
            if (textOnBoard)
                if (lenPrinted <= stringlen)
                    textOnBoard.text = nowString.Substring(0, lenPrinted);
            lenPrinted++;
        }
    }
}
