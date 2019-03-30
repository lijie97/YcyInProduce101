using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DialogItem : MonoBehaviour {
    public TextMeshProUGUI timeText, dialogText;
    public FadeInTextWordByWord fadeInText;
    public void Init(BehaviorType behaviorType)
    {
        fadeInText.Init();
        timeText.text = DataManager.Instance.GetTimeTypeStr(PlayerData.Instance.playerData.playerInfo.curTimeType);
        string dialog = BehaviorDialogData.Instance.GetDialog(behaviorType);
        dialogText.text = dialog;
    }


    public void ShowFadeText()
    {
        fadeInText.Fade();
    }
}
