using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class managerData : MonoBehaviour {
    Button classBtn, practiceBtn, talkBtn, restBtn;
    // Use this for initialization
    void Start() {
        classBtn = GameObject.Find("Canvas/Buttons/ClassButton").GetComponent<Button>();
        practiceBtn = GameObject.Find("Canvas/Buttons/PracticeButton").GetComponent<Button>();
        talkBtn = GameObject.Find("Canvas/Buttons/TalkButton").GetComponent<Button>();
        restBtn = GameObject.Find("Canvas/Buttons/ClassButton").GetComponent<Button>();

    }
    //
    void practiceBtnClick() {
        PlayerData.Instance.playerData.playerInfo.countDownTime -= 1;

    }


    // Update is called once per frame
    void Update () {
		
	}
}
