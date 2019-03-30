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

    void OnEnable()
    {
		PlayerData.Instance.InitData("PlayerData", true, false);

    }

	private Vector2 scrollVector;
    void OnGUI()
    {
		scrollVector = EditorGUILayout.BeginScrollView (scrollVector);

        EditorGUILayout.Space();
       
        if (GUILayout.Button("保存数据"))
        {
            PlayerData.Instance.SaveData();
        }
		EditorGUILayout.EndScrollView ();
    }

 
}