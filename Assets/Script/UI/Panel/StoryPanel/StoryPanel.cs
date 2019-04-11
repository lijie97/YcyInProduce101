using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryPanel : BasePanel
{

    public Image characterIcon, bg;
    public Text dialogText, characterName;
    public GameObject optionDialogObj,dialogObj;
    public Button clickArea;
    public List<Button> optionsBtn = new List<Button>();
    private int curStoryID;
    private StoryParam storyParam;
    public override void Init()
    {
        base.Init();
    }

    public void SetDataById(int id)
    {
        curStoryID = id;
        StoryParam param = new StoryParam(id);
        storyParam = param;
        if (!string.IsNullOrEmpty(param.dialogText))
        {
            dialogText.text = param.dialogText;
        }
        characterName.text = param.characterName;
        characterIcon.sprite = AtlasManager.Instance.GetSprite(AtlasType.UIAtlas, param.characterIconName);


        bg.sprite = AtlasManager.Instance.GetSprite(AtlasType.UIAtlas, param.bgName);

        if(param.dialogType == DialogType.SelectDialog)
        {
            for (int i = 0; i < param.selectResultNextIDParams.Count; i++)
            {
                optionsBtn[i].transform.Find("Text").GetComponent<Text>().text = string.Format("{0} - {1}", i + 1, param.selectResultNextIDParams[i].selectDialogText);
                optionsBtn[i].gameObject.SetActive(true);
            }
        }
        optionDialogObj.SetActive(param.dialogType == DialogType.SelectDialog);
        dialogObj.SetActive(param.dialogType != DialogType.SelectDialog);
        clickArea.gameObject.SetActive(param.dialogType == DialogType.NormalDialog);
        if (IsInvoking("DelayCallEvent"))
        {
            CancelInvoke("DelayCallEvent");
        }
        Invoke("DelayCallEvent", 0.1f);
    }

    private void DelayCallEvent()
    {
        string strEvent = StoryData.Instance.GetStartEvent(curStoryID);
        if (!string.IsNullOrEmpty(strEvent))
        {
            StoryEvent.Instance.gameObject.SendMessage(strEvent);
        }
    }

    public void ShowStoryById(int id)
    {
        SetDataById(id);
        UIManager.Instance.ShowPanel(PanelType.StoryPanel);
    }


    public void ClickNext()
    {
        Debug.Log("next");
        string strClick = storyParam.clickEvent;
        int nextId = storyParam.nextID;
        if (nextId != 0)
        {
            SetDataById(nextId);
            if (!string.IsNullOrEmpty(strClick))
            {
                StoryEvent.Instance.gameObject.SendMessage(strClick);
            }
        }
        else
        {
            gameObject.SetActive(false);
            if (!string.IsNullOrEmpty(strClick))
            {
                StoryEvent.Instance.gameObject.SendMessage(strClick);
            }
        }
    }

    public void ClickNext(int nextID)
    {
        Debug.Log("next");
        string strClick = storyParam.clickEvent;
        if (nextID != 0)
        {
            SetDataById(nextID);
            if (!string.IsNullOrEmpty(strClick))
            {
                StoryEvent.Instance.gameObject.SendMessage(strClick);
            }
        }
        else
        {
            gameObject.SetActive(false);
            if (!string.IsNullOrEmpty(strClick))
            {
                StoryEvent.Instance.gameObject.SendMessage(strClick);
            }
        }
    }


    public void OptionsBtnClick(int selectID)
    {
        ClickNext(storyParam.selectResultNextIDParams[selectID - 1].nextID);
    }
}
