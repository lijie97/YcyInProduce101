using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DialogItem : MonoBehaviour {
    public TextMeshProUGUI timeText, dialogText;
    public FadeInTextWordByWord fadeInText;
    public void Init(BehaviorType behaviorType)
    {
        TimeType timeType;
        if (PlayerData.Instance.playerData.playerInfo.curTimeType == TimeType.Morning)
        {
            timeType = TimeType.LateAtNight;
        }
        else
        {
            timeType = PlayerData.Instance.playerData.playerInfo.curTimeType - 1;
        }

        timeText.text =  DataManager.Instance.GetTimeTypeStr(timeType);
        string dialog = BehaviorDialogData.Instance.GetDialog(behaviorType);
        dialogText.text = dialog;
        transform.SetAsFirstSibling();
        fadeInText.Init();

    }


    public void ShowFadeText()
    {
        fadeInText.Fade();
    }
}
