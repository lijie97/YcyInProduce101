using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;

public class PlayerDataEditor : EditorWindow
{

    public const string MenuPath = EditorUtil.MenuRoot + "用户数据/";
    private static PlayerDataEditor window;

	[MenuItem(MenuPath + "编辑用户数据")]
    private static void EditorPlayerData()
    {
        window = (PlayerDataEditor)EditorWindow.GetWindow(typeof(PlayerDataEditor), true, "用户数据编辑");
    }

    [MenuItem(MenuPath + "删除用户数据")]
    private static void DeletePlayerData()
    {
        FileTool.DelectFile("PlayerData");
        Debug.Log("delete data");
        AssetDatabase.Refresh();
    }
    [MenuItem(MenuPath + "删除", true)]
    private static bool ValidateDeletePlayerData()
    {
        return FileTool.IsFileExists("PlayerData");
    }

    private int coinCount;
    private int jewelCount;
    void OnEnable()
    {
		PlayerData.Instance.InitData("PlayerData", true, false);

        coinCount = PlayerData.Instance.Coin;
        jewelCount = PlayerData.Instance.Jewel;
    }

	private Vector2 scrollVector;
    void OnGUI()
    {
		scrollVector = EditorGUILayout.BeginScrollView (scrollVector);
        coinCount = EditorGUILayout.IntField("金币-数量", coinCount, GUILayout.Height(20));
        jewelCount = EditorGUILayout.IntField("钻石-数量", jewelCount, GUILayout.Height(20));

        EditorGUILayout.Space();
       
        if (GUILayout.Button("保存数据"))
        {
            PlayerData.Instance.Coin = coinCount;
            PlayerData.Instance.Jewel = jewelCount;
            PlayerData.Instance.SaveData();
        }
		EditorGUILayout.EndScrollView ();
    }

 
}