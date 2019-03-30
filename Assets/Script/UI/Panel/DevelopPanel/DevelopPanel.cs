using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevelopPanel : BasePanel
{

    public Transform propertyItemContainer;
    private List<PropertyItem> propertyItems = new List<PropertyItem>();
    public TextMeshProUGUI daysText, timeText, countDownText, talentText, songProficiencyText;
    private Fade canvasFade;
    public override void Init()
    {
        base.Init();
        canvasFade = GameObject.Find("Canvas/FadeCanvas").GetComponent<Fade>();
        List<string> idList = PropertyData.Instance.GetIDList();
        for (int i = 0; i < idList.Count; i++)
        {
            PropertyItem item = ResourcesManager.Instance.LoadUIItem("PropertyItem").GetComponent<PropertyItem>();
            item.transform.SetParent(propertyItemContainer);
            item.transform.localScale = Vector3.one;
            item.Init(int.Parse(idList[i]));
            propertyItems.Add(item);
        }
    }

    public override void Show()
    {
        base.Show();
        RefreshInfo();
    }
    public void SetPlayerPropertyChange(PropertyChangeParam[] changeParams)
    {
        for (int i = 0; i < changeParams.Length; i++)
        {
            PlayerData.Instance.SetPlayerPropertyValue(changeParams[i]);
        }
    }

    private void RefreshInfo()
    {
        daysText.text = string.Format("第<color=green>{0}</color>天", PlayerData.Instance.playerData.playerInfo.curDays);
        timeText.text = DataManager.Instance.GetTimeTypeStr(PlayerData.Instance.playerData.playerInfo.curTimeType);
        countDownText.text = string.Format("距离公演还有<color=green>{0}</color>天", PlayerData.Instance.playerData.playerInfo.countDownTime);
        int talentPoint = PlayerData.Instance.playerData.playerInfo.talentPoint;
        talentText.text = string.Format("才艺点：<color=green>{0}</color>（分数加成{1}）", talentPoint, DataManager.Instance.GetTalentAddition(talentPoint));
        songProficiencyText.text = string.Format("公演曲熟练度: <color=green>{0}</color>%", PlayerData.Instance.playerData.playerInfo.songProficiency);
    }

    private void ShowFadeCavasAndSetValue(BehaviorType behaviorType)
    {
        canvasFade.tips.gameObject.SetActive(true);
        canvasFade.FadeIn(0.1f, () =>
        {
            canvasFade.tips.gameObject.SetActive(false);
            canvasFade.FadeOut(0.1f, () =>
            {
                PropertyChangeParam[] changeParams = BehaviorData.Instance.GetPropertyChangeParams(behaviorType);
                SetPlayerPropertyChange(changeParams);
                RefreshInfo();
            });
            PlayerData.Instance.SetNextTimePoint();
        });
    }

    #region Click
    public void Go2ClassBtnClick()
    {
        Debug.Log("上课"); 
        canvasFade.SetTips("上课中...");
        ShowFadeCavasAndSetValue(BehaviorType.Go2Class);

    }

    public void RehearsalBtnClick()
    {
        Debug.Log("排练");
        canvasFade.SetTips("排练中...");
        ShowFadeCavasAndSetValue(BehaviorType.Rehearsal);
    }

    public void InteractBtnClick()
    {
        Debug.Log("互动");
        canvasFade.SetTips("互动中...");
        ShowFadeCavasAndSetValue(BehaviorType.Interact);

    }
    public void RestBtnClick()
    {
        Debug.Log("休息");
        canvasFade.SetTips("休息中...");
        ShowFadeCavasAndSetValue(BehaviorType.Rest);
    }
    #endregion
}


