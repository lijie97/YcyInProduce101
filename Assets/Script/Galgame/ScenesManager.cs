using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ScenesManager : MonoBehaviour
{
    int afs, stringlen, lenPrinted, maxScenes,option;
    int maxAFS = 220;
    public GameObject bgmObj;
    //private string Res_BGPath = "Background/";
    string nowString, nowBgPicName, nowPsPicName;
    Text textOnBoard, textPsName,textOnBtnOpt1, textOnBtnOpt2, textOnBtnOpt3;
    Image nowBgImg, nowBgImgLocader, nowPsImg;
    bool visible,acc;
    Button btn,btnOpt1, btnOpt2, btnOpt3;
    GameObject nowBgObj, nowPsObj,nameCardObj,diagBoxObj;

    void Start()
    {
        PlayerData.Instance.playerData.playerInfo.curStoryIndex = 1;

        //Debug.Log("start");
        nameCardObj = GameObject.Find("Canvas/DiagBox/NameCard");
        diagBoxObj = GameObject.Find("Canvas/DiagBox");

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
        changePersonPos();
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
        if (csvController.GetInstance().getInt(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 9) == 1)
        {
            activeOptions(true);            
            //更改三个按钮文本
            textOnBtnOpt1.text = csvController.GetInstance().getString(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 10);
            textOnBtnOpt2.text = csvController.GetInstance().getString(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 12);
            textOnBtnOpt3.text = csvController.GetInstance().getString(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 14);
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
        visible = csvController.GetInstance().getInt(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 6)==1;
        if (!visible)
        {
            if (nameCardObj)
                nameCardObj.GetComponent<Appear>().disappear();
            if (diagBoxObj)
                diagBoxObj.GetComponent<Appear>().disappear();
            //GameObject.Find("Canvas/Diag-Box").GetComponent<Appear>().disappear();
        }
        else {
            if (nameCardObj)
                nameCardObj.GetComponent<Appear>().appear();
            if (diagBoxObj)
                diagBoxObj.GetComponent<Appear>().appear();
        }
    }
    void getPsName()
    {
        if (textPsName) textPsName.text = csvController.GetInstance().getString(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 2);
    }
    void changePersonPos() {
        if (csvController.GetInstance().getInt(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 5) == 1)
        {
            Debug.Log("1");
            nowPsObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(1400f, nowPsObj.GetComponent<RectTransform>().anchoredPosition.y);
        }
        else
            nowPsObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(600f, nowPsObj.GetComponent<RectTransform>().anchoredPosition.y);

    }
    void getPic()
    {
       
        if (nowPsPicName != csvController.GetInstance().getString(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 1)) {  
            nowPsPicName = csvController.GetInstance().getString(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 1);
            if (nowPsPicName.Length != 0)
            {
                nowPsImg.sprite = Resources.Load("透明", typeof(Sprite)) as Sprite;
                nowPsObj.GetComponent<Appear>().ResetAlphaTunel();
                nowPsImg.sprite = Resources.Load("img_girls/" + nowPsPicName, typeof(Sprite)) as Sprite;
                
            }
        else {
                nowPsImg.sprite = Resources.Load("透明", typeof(Sprite)) as Sprite;
                
            }
        }
        if (nowBgPicName != csvController.GetInstance().getString(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 4))
        {
            nowBgPicName = csvController.GetInstance().getString(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 4);
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
        nowString = csvController.GetInstance().getString(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 3);
        stringlen = nowString.Length;
        lenPrinted = 0;
    }
    int autoSlide() { 
        return csvController.GetInstance().getInt(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 8)==0?int.MaxValue: csvController.GetInstance().getInt(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 7);
    }
    void changeScene() {
        
        if (csvController.GetInstance().getString(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 19).Length!= 0)
        {
            PlayerData.Instance.playerData.playerInfo.curStoryIndex++;
            SceneManager.LoadScene(csvController.GetInstance().getString(PlayerData.Instance.playerData.playerInfo.curStoryIndex - 1, 19));
        }
    }

    void onClick()
    {
        if (lenPrinted < stringlen) { 
            //lenPrinted++;
            acc = true;
        }
        else {
            //Debug.Log(csvController.GetInstance().getInt(index, 7));
            if (option == 0)
                PlayerData.Instance.playerData.playerInfo.curStoryIndex = csvController.GetInstance().getInt(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 7);
            else {
                PlayerData.Instance.playerData.playerInfo.curStoryIndex = csvController.GetInstance().getInt(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 9+2*option);
            }
            
            changeScene();
            getPsName();
            getText();
            getPic();
            getOptions();
            getVisibility();
            changePersonPos();
            afs = 0;
            acc = false;
            option = 0;
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A) && csvController.GetInstance().getInt(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 9) == 1)
        {
            onClickOpt1();
        }
        if (Input.GetKeyDown(KeyCode.S) && csvController.GetInstance().getInt(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 9) == 1)
        {
            onClickOpt2();
        }
        if (Input.GetKeyDown(KeyCode.D) && csvController.GetInstance().getInt(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 9) == 1)
        {
            onClickOpt3();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && csvController.GetInstance().getInt(PlayerData.Instance.playerData.playerInfo.curStoryIndex, 9) == 0)
        {
            onClick();
        }
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
