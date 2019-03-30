using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BehaviorDialogData : ICSVData<BehaviorDialogData>
{

    public BehaviorDialogData()
    {
        InitData("BehaviorDialogData");
    }

    public void RefreshData()
    {
        InitData("BehaviorDialogData");
    }

    public string GetName(int Id)
    {
        return GetProperty("Name", Id);
    }

    public string GetDialog(BehaviorType behaviorType)
    {
        int id = (int)behaviorType;
        int count = GetBehaviourDialogCount(behaviorType);
        int randomValue = UnityEngine.Random.Range(1, count+1);
        int realID = id * 100 + randomValue;
        string str = Regex.Unescape(GetProperty("Dialog", realID));
        return str;
    }

    private int GetBehaviourDialogCount(BehaviorType behaviorType)
    {
        int value = (int)behaviorType;
        int count = 0;
        List<string> allIdList = GetIDList();
        for (int i = 0; i < allIdList.Count; i++)
        {
            if (allIdList[i].StartsWith(value.ToString()))
            {
                count++;
            }
        }
        return count;
    }
}