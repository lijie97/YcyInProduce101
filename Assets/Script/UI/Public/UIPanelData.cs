using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="UIData/Create UIPanelData ")]
public class UIPanelData : ScriptableObject
{
    private static UIPanelData _instance = null;

    public static UIPanelData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<UIPanelData>("Config/UIPanelData");
            }
            return _instance;
        }
    }

    public void RefreshData()
    {
        _instance = Resources.Load<UIPanelData>("Config/UIPanelData");
    }

    public Dictionary<object, bool> _editorMainSceneUIListItemStates = new Dictionary<object, bool>();
    [Header("主界面的UI")]
    public List<UIPanelStruct> MainSceneUIList = new List<UIPanelStruct>();


    public Dictionary<object, bool> _editorGameSceneUIListItemStates = new Dictionary<object, bool>();
    [Header("游戏界面的UI")]
    public List<UIPanelStruct> GameSceneUIList = new List<UIPanelStruct>();


    public PanelLayerType GetPanelLayerType(PanelType panelType)
    {
        for (int i = 0; i < MainSceneUIList.Count; i++)
        {
            if (MainSceneUIList[i].panelType == panelType)
            {
                return MainSceneUIList[i].layerType;
            }
        }

        for (int i = 0; i < GameSceneUIList.Count; i++)
        {
            if (GameSceneUIList[i].panelType == panelType)
            {
                return GameSceneUIList[i].layerType;
            }
        }
        return PanelLayerType.None;
    }
}

[System.Serializable]
public class UIPanelStruct
{
    public PanelType panelType;
    public PanelLayerType layerType;
}