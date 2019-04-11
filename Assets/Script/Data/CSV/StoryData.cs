
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoryData : ICSVData<StoryData>
{

    public StoryData()
    {
        try
        {

            InitData("StoryData");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message + e.StackTrace);
        }
    }


    public string GetBgName(int id)
    {
        string bgName = GetProperty("BgName", id);
        return bgName;
    }

    public string GetDialogText(int id)
    {
        return GetProperty("DialogText", id);
    }
    public string GetCharacterName(int id)
    {
        return GetProperty("CharacterName", id);
    }

    public string GetCharacterIconName(int id)
    {
        return GetProperty("CharacterIconName", id);
    }

    public int GetAwardPropCount(int id)
    {
        return GetPropertyToInt("AwardPropCount", id);
    }


    public DialogType GetDialogType(int id)
    {
        string str = GetProperty("DialogType", id);
        DialogType dialogType = (DialogType)System.Enum.Parse(typeof(DialogType), str);
        return dialogType;
    }

    public string GetStartEvent(int id)
    {
        return GetProperty("StartEvent", id);
    }

    public string GetClickAction(int id)
    {
        return GetProperty("ClickAction", id);
    }

    public int GetNextID(int id)
    {
        return GetPropertyToInt("NextID", id);
    }

    public List<SelectResultNextIDParam> GetSelectResultNextIDList(int id)
    {
        string str = GetProperty("SelectResultNextID",id);
        string[] selectStr = str.Split('+');
        List<SelectResultNextIDParam> selectResultNextIDParams = new List<SelectResultNextIDParam>();
        string[] selectDialogs = GetSelectDialogTextArray(id);
        for (int i = 0; i < selectStr.Length; i++)
        {
            string[] paramStr =  selectStr[i].Split('|');
            SelectResultNextIDParam param = new SelectResultNextIDParam(int.Parse(paramStr[0]),int.Parse(paramStr[1]),selectDialogs[i]);
            selectResultNextIDParams.Add(param);
        }
        return selectResultNextIDParams;
    }

   
    public string[] GetSelectDialogTextArray(int id)
    {
        string str = GetProperty("SelectDialogText", id);
        string[] selectDialogStr = str.Split('|');
        return selectDialogStr;
    }

    public CharacterIconDir GetCharacterIconDir(int id)
    {
        CharacterIconDir dir = (CharacterIconDir)Enum.Parse(typeof(CharacterIconDir), GetProperty("CharacterIconDir", id));
        return dir;
    }

}


public class StoryParam
{
    public int storyID;
    public string dialogText;
    public string startEvent;
    public string clickEvent;
    public DialogType dialogType;
    public List<SelectResultNextIDParam> selectResultNextIDParams;
    public string bgName;
    public int nextID;
    public string characterName;
    public string characterIconName;
    public CharacterIconDir characterIconDir;
    public StoryParam(int storyID)
    {
        this.storyID = storyID;
        this.dialogText = StoryData.Instance.GetDialogText(storyID);
        this.startEvent = StoryData.Instance.GetStartEvent(storyID);
        this.clickEvent = StoryData.Instance.GetClickAction(storyID);
        this.dialogType = StoryData.Instance.GetDialogType(storyID);
        this.selectResultNextIDParams = StoryData.Instance.GetSelectResultNextIDList(storyID);
        this.bgName = StoryData.Instance.GetBgName(storyID);
        this.nextID = StoryData.Instance.GetNextID(storyID);
        this.characterName = StoryData.Instance.GetCharacterName(storyID);
        this.characterIconName = StoryData.Instance.GetCharacterIconName(storyID);
        this.characterIconDir = StoryData.Instance.GetCharacterIconDir(storyID);
    }
}

public enum DialogType
{
    NormalDialog,
    SelectDialog,
}


public enum CharacterIconDir
{
    Left,
    Right,
    Mid
}

public class SelectResultNextIDParam
{
    public int selectID;
    public int nextID;
    public string selectDialogText;
    public SelectResultNextIDParam(int selectID,int nextID,string selectDialogText)
    {
        this.selectID = selectID;
        this.nextID = nextID;
        this.selectDialogText = selectDialogText;
    }
}