using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevelopPanel : BasePanel
{

    public Transform propertyItemContainer,dialogItemContainer;
    private List<PropertyItem> propertyItems = new List<PropertyItem>();
    public TextMeshProUGUI timeText, countDownText, talentText, songProficiencyText;
    private Fade canvasFade;
    private SpawnPool uiItemPool;
    public override void Init()
    {
        base.Init();
        canvasFade = GameObject.Find("Canvas/FadeCanvas").GetComponent<Fade>();
        uiItemPool = PoolManager.Pools["UIItemPool"];
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
        //daysText.text = string.Format("第<color=green>{0}</color>天", PlayerData.Instance.playerData.playerInfo.curDays);
        timeText.text = DataManager.Instance.GetTimeTypeStr(PlayerData.Instance.playerData.playerInfo.curTimeType);
        countDownText.text = string.Format("{0}", PlayerData.Instance.playerData.playerInfo.countDownTime);
        int talentPoint = PlayerData.Instance.playerData.playerInfo.talentPoint;
        talentText.text = talentPoint.ToString();
        songProficiencyText.text = string.Format("{0}%", PlayerData.Instance.playerData.playerInfo.songProficiency);
    }

    private void ShowFadeCavasAndSetValue(BehaviorType behaviorType)
    {
        canvasFade.tips.gameObject.SetActive(true);
        canvasFade.FadeIn(1f, () =>
        {
            canvasFade.tips.gameObject.SetActive(false);
            canvasFade.FadeOut(1f, () =>
            {
                PropertyChangeParam[] changeParams = BehaviorData.Instance.GetPropertyChangeParams(behaviorType);
                SetPlayerPropertyChange(changeParams);
                RefreshInfo();
                DialogItem dialogItem = CreateDialogItem(behaviorType);
                dialogItem.ShowFadeText();
            });
            PlayerData.Instance.SetNextTimePoint();
            switch (behaviorType)
            {
                case BehaviorType.Go2Class:
                    SceneManager.LoadScene("Stories");
                    break;
            }
        });
    }

    private DialogItem CreateDialogItem(BehaviorType behaviorType)
    {
        DialogItem item = uiItemPool.Spawn("DialogItem").GetComponent<DialogItem>();
        item.transform.SetParent(dialogItemContainer);
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
        item.transform.localScale = Vector3.one;
        item.Init(behaviorType);
        item.gameObject.SetActive(true);
        return item;
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


