﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicGame_UIManager : MonoBehaviour {

    private static MusicGame_UIManager Instance;

    protected GuitarGameplay guitarGameplay;
    public static MusicGame_UIManager getInstance()
    {
        if (Instance == null)
        {
            Instance = new MusicGame_UIManager();
        }
        return Instance;
    }


    //创建歌曲播放列表
    private void Awake()
    {
        creatSongList();
    }

    //分值显示文本
    public Text ScoreText;             //分值
    public Image hp;                   //血量
    public float _cutHpValue=0.05f;    //掉血量
    public Text HitTips;               //击打提示
    public GameObject SongList;        //歌曲列表
    public GameObject GameOver;        //游戏失败界面
    public GameObject SettlementBG;    //游戏完成界面
    //public GameObject SetGrade;        //游戏难度设置界面
    //public Image gradeStatus;          //游戏难度等级显示UI；
    //protected int curSongID;           //当前播放歌曲的ID；
    public bool gameFinisher=true;       //游戏完成还是失败
    // Use this for initialization
    void Start()
    {
        guitarGameplay = GetComponent<GuitarGameplay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnGUI()
    {
        ScoreText.text = ((int)guitarGameplay.GetScore()).ToString();
        //HitTips.text = guitarGameplay.GetCombo().ToString();
    }



    //创建歌曲列表
    public void creatSongList()
    {
        //Draw all songs in the playlist
        SongData[] playlist = GetComponent<GuitarGameplay>().GetPlaylist();
        for (int i = 0; i < playlist.Length; i++)
        {
            string buttonLabel = playlist[i].Band + " - " + playlist[i].Name;
            GameObject playList = Instantiate(Resources.Load<GameObject>("UIPerfebs/songList"));
            playList.transform.parent = SongList.transform.Find("Viewport").Find("Content").transform;
            playList.name = i.ToString();
            playList.transform.GetChild(0).GetComponent<Text>().text = playlist[i].Name;
            playList.GetComponent<Button>().onClick.AddListener(delegate () {
                int index = int.Parse(playList.name);
                GetComponent<GuitarGameplay>().StartPlaying(index);              
                SongList.SetActive(false);                
                //gradeStatus.fillAmount = 0.33f;             
            });
        }
    }


    //掉血
    public void reduceHp()
    {     
        if (hp.fillAmount > 0)
        {
            hp.fillAmount = hp.fillAmount - _cutHpValue;
            if (hp.fillAmount <=0)
            {
                //Game Over
                gameFinisher = false;
                GameOver.SetActive(true);
                guitarGameplay.StopPlaying();
            }
        }
        else
        {
            //Game Over
            gameFinisher = false;
            GameOver.SetActive(true);
            guitarGameplay.StopPlaying();
        }
    }



    //游戏通过后，结算页面
    public void FinishGame(string Score,string perfect,string great,string good,string miss,string combo,float Proportion)
    {
        //区间判定详情
        SettlementBG.SetActive(true);
        SettlementBG.transform.Find("Details").Find("Perfect").GetChild(0).GetComponent<Text>().text = perfect;
        SettlementBG.transform.Find("Details").Find("Great").GetChild(0).GetComponent<Text>().text = great;
        SettlementBG.transform.Find("Details").Find("Good").GetChild(0).GetComponent<Text>().text = good;
        SettlementBG.transform.Find("Details").Find("Miss").GetChild(0).GetComponent<Text>().text = miss;
        SettlementBG.transform.Find("Details").Find("Combo").GetChild(0).GetComponent<Text>().text = combo;

        //评级
        if (Proportion>=0.9 && Proportion<=1)
        {
            SettlementBG.transform.Find("Grade").GetChild(0).GetComponent<Text>().text = "A";
        }
        else if (Proportion > 0.75 && Proportion < 0.9)
        {
            SettlementBG.transform.Find("Grade").GetChild(0).GetComponent<Text>().text = "B";
        }
        else if (Proportion > 0.5 && Proportion < 0.75)
        {
            SettlementBG.transform.Find("Grade").GetChild(0).GetComponent<Text>().text = "C";
        }
        else 
        {
            SettlementBG.transform.Find("Grade").GetChild(0).GetComponent<Text>().text = "D";
        }

        //分值
        SettlementBG.transform.Find("Score").GetChild(0).GetComponent<Text>().text = Score;
        SettlementBG.transform.Find("MaxScore").GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("MaxScore");
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("MaxScore")))
        {
            if (float.Parse(Score)> float.Parse(PlayerPrefs.GetString("MaxScore")) )
            {
                SettlementBG.transform.Find("NewRecord").GetComponent<Text>().enabled =true;
                SettlementBG.transform.Find("NewRecord").GetComponent<Text>().text = "新纪录";
                PlayerPrefs.SetString("MaxScore", Score);
            }   
        }
        else
        {

            PlayerPrefs.SetString("MaxScore", Score);
        }


        if (gameFinisher==false)
        {
            SettlementBG.transform.Find("Grade").GetChild(0).GetComponent<Text>().text = "失败";
            SettlementBG.transform.Find("NewRecord").GetComponent<Text>().enabled = true;
            SettlementBG.transform.Find("NewRecord").GetComponent<Text>().text = "游戏挑战失败";
        }

    }
        
           

    //返回主菜单
    public void goBackMainMenu()
    {
        SceneManager.LoadScene("");
    }


    //再来一局
    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    //打开难易等级设置界面
    //public void openGradeSettingUI()
    //{
    //    guitarGameplay.StopPlaying();
    //    SetGrade.SetActive(true);
    //}

    //游戏难度设定
    //public void GradeSeting(int type)
    //{      
    //    switch (type)
    //    {
    //        //简单  初步大致定为总音符的十分之一MISS数。
    //        case 1:                         //大
    //            _cutHpValue =(float) 1 / (guitarGameplay.GetNoteObjectsCount() / 1);     //yi分之一
    //            SetGrade.SetActive(false);
    //            gradeStatus.fillAmount = 0.33f;
    //            break;

    //            //中等
    //        case 2:
    //            _cutHpValue = (float)1 / (guitarGameplay.GetNoteObjectsCount() / 10);    //十分之一
    //            SetGrade.SetActive(false);
    //            gradeStatus.fillAmount = 0.66f;
    //            break;

    //            //困哪
    //        case 3:
    //            _cutHpValue = (float)1 / (guitarGameplay.GetNoteObjectsCount() / 20);    //二十分之一
    //            SetGrade.SetActive(false);
    //            gradeStatus.fillAmount = 1 ;
    //            break;
    //    }
    //}



    //创建连击提示
    public IEnumerator DisplayText(string text)
    {     
        HitTips.text = text;
        HitTips.gameObject.SetActive(true);      
        yield return new WaitForSeconds(2);
        if (float.Parse(guitarGameplay.GetCombo().ToString())==0)
        {
            HitTips.gameObject.SetActive(false);
        }     
    }


}
