using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopPanel : BasePanel {

    public Transform propertyItemContainer;
    private List<PropertyItem> propertyItems = new List<PropertyItem>();
    public override void Init()
    {
        base.Init();
        List<string> idList  = PropertyData.Instance.GetIDList();
        for (int i = 0; i < idList.Count; i++)
        {
            PropertyItem item = ResourcesManager.Instance.LoadUIItem("PropertyItem").GetComponent<PropertyItem>();
            item.transform.SetParent(propertyItemContainer);
            item.transform.localScale = Vector3.one;
            item.Init(int.Parse(idList[i]));
            propertyItems.Add(item);
        }
    }

    #region Click
    public void Go2ClassBtnClick()
    {
        Debug.Log("上课");
    }

    public void RehearsalBtnClick()
    {
        Debug.Log("排练");

    }
    public void InteractBtnClick()
    {
        Debug.Log("互动");

    }
    public void RestBtnClick()
    {
        Debug.Log("休息");

    }
    #endregion
}


public enum PropertyType
{
    Mood=1,
    Vigor,
    TopicValue
}