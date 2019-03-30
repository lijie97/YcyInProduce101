using UnityEngine;
using System.Collections;
using System;

public class BehaviorData : ICSVData<BehaviorData>
{

    public BehaviorData()
    {
        InitData("BehaviorData");
    }

    public void RefreshData()
    {
        InitData("BehaviorData");
    }

    public string GetName(int Id)
    {
        return GetProperty("Name", Id);
    }

    /// <summary>
    /// 获得心情改变值
    /// </summary>
    /// <returns>The change mood.</returns>
    /// <param name="id">Identifier.</param>
    private PropertyChangeParam GetChangeMood(int id)
    {
        string str = GetProperty("ChangeMoodValue", id);
        ChangeType changeType = GetChangeType(str[0]);
        PropertyType propertyType = PropertyType.Mood;
        int changeValue = int.Parse(str.Substring(1));
        return new PropertyChangeParam(changeType, propertyType, changeValue);
    }

    /// <summary>
    /// 获得精力改变值
    /// </summary>
    /// <returns>The change vigor.</returns>
    /// <param name="id">Identifier.</param>
    private PropertyChangeParam GetChangeVigor(int id)
    {
        string str = GetProperty("ChangeVigorValue", id);
        ChangeType changeType = GetChangeType(str[0]);
        PropertyType propertyType = PropertyType.Vigor;
        int changeValue = int.Parse(str.Substring(1));
        return new PropertyChangeParam(changeType, propertyType, changeValue);
    }

    /// <summary>
    /// 话题度改变值
    /// </summary>
    /// <returns>The change topic.</returns>
    /// <param name="id">Identifier.</param>
    private PropertyChangeParam GetChangeTopic(int id)
    {
        string str = GetProperty("ChangeTopicValue", id);
        ChangeType changeType = GetChangeType(str[0]);
        PropertyType propertyType = PropertyType.TopicValue;
        int changeValue = int.Parse(str.Substring(1));
        return new PropertyChangeParam(changeType, propertyType, changeValue);
    }

    /// <summary>
    /// 获得才艺点改变值
    /// </summary>
    /// <returns>The change talent.</returns>
    /// <param name="id">Identifier.</param>
    private PropertyChangeParam GetChangeTalent(int id)
    {
        string str = GetProperty("ChangeTalentValue", id);
        ChangeType changeType = GetChangeType(str[0]);
        PropertyType propertyType = PropertyType.Talent;
        int changeValue = int.Parse(str.Substring(1));
        return new PropertyChangeParam(changeType, propertyType, changeValue);
    }

    /// <summary>
    /// 获得公演歌曲熟练度改变值
    /// </summary>
    /// <returns>The song proficiency.</returns>
    /// <param name="id">Identifier.</param>
    private PropertyChangeParam GetSongProficiency(int id)
    {
        string str = GetProperty("ChangeSongProficiencyValue", id);
        ChangeType changeType = GetChangeType(str[0]);
        PropertyType propertyType = PropertyType.SongProficiency;
        int changeValue = int.Parse(str.Substring(1));
        return new PropertyChangeParam(changeType, propertyType, changeValue);
    }

    /// <summary>
    /// 获得该行为的所有改变值
    /// </summary>
    /// <returns>The property change parameters.</returns>
    /// <param name="id">Identifier.</param>
    public PropertyChangeParam[] GetPropertyChangeParams(int id)
    {
        PropertyChangeParam[] propertyChangeParams = new PropertyChangeParam[5] { GetChangeMood(id), GetChangeVigor(id), GetChangeTopic(id), GetChangeTalent(id), GetSongProficiency(id) };
        return propertyChangeParams;
        
    }

    public PropertyChangeParam[] GetPropertyChangeParams(BehaviorType behaviorType)
    {
        int id = (int)behaviorType;
        PropertyChangeParam[] propertyChangeParams = new PropertyChangeParam[5] { GetChangeMood(id), GetChangeVigor(id), GetChangeTopic(id), GetChangeTalent(id), GetSongProficiency(id) };
        return propertyChangeParams;

    }

    public BehaviorType GetBehaviorType(int id)
    {
        string str = GetProperty("BehaviorType", id);
        return (BehaviorType)Enum.Parse(typeof(BehaviorType), str);
    }

    private ChangeType GetChangeType(char c)
    {
        if(c == '+')
        {
            return ChangeType.Add;
        }
        else
        {
            return ChangeType.Reduce;
        }
    }
}

public class PropertyChangeParam
{
    public ChangeType changeType;
    public PropertyType propertyType;
    public int changeValue;
    public PropertyChangeParam(ChangeType changeType, PropertyType propertyType, int changeValue)
    {
        this.changeType = changeType;
        this.propertyType = propertyType;
        this.changeValue = changeValue;
    }
}

public enum ChangeType
{
    Add,
    Reduce
}