using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//[CreateAssetMenu(menuName = "Create BuildSetting ")]
public class BuildSetting : ScriptableObject
{

    private static BuildSetting _instance = null;

    public static BuildSetting Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<BuildSetting>("Config/BuildSetting");
            }
            return _instance;
        }
    }

    [Header("是否显示广告")]
    public bool IsShowAds = false;

    [Header("是否有新手教程")]
    public bool IsHaveGuide = false;

    [Header("免费评测裸包")]
    public bool isFree = false;
}

