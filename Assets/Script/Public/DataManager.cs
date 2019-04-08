﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoSingletonBase<DataManager>
{

    public override void Init()
    {

    }

    public string GetTimeTypeStr(TimeType timeType)
    {
        switch (timeType)
        {
            case TimeType.Morning:
                return "上           午";
            case TimeType.Afternoon:
                return "下           午";
            case TimeType.Night:
                return "晚           上";
            case TimeType.LateAtNight:
                return "深           夜";
            default:
                return "";
        }
    }

    /// <summary>
    /// 获得才艺点分数加成
    /// </summary>
    /// <returns>The talent addition.</returns>
    /// <param name="talentPoint">Talent point.</param>
    public float GetTalentAddition(int talentPoint)
    {
        //todo 才艺点分数加成
        return 1;
    }

}
