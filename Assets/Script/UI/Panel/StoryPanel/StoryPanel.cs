using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryPanel : BasePanel
{

    public Image characterIcon, bg;
    public Text dialogText, characterName;

    private int curStoryID;
    public override void Init()
    {
        base.Init();
    }

    public void SetDataById(int id)
    {
        curStoryID = id;

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
        string strClick = StoryData.Instance.GetClickAction(curStoryID);
        int nextId = StoryData.Instance.GetNextID(curStoryID);
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
}
