using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CustomEditor(typeof(UIPanelData))]
public class UIPanelDataEditor : Editor
{

    public bool expandUIList = true, expandGameSceneList = true;

    public override void OnInspectorGUI()
    {
        UIPanelData script = (UIPanelData)target;
        EditorGUI.indentLevel = 0;
        PGEditorUtils.LookLikeControls();

        EditorGUILayout.Space();
        this.expandUIList = PGEditorUtils.SerializedObjFoldOutList<UIPanelStruct>
            (
                "UI场景的界面列表",
                script.MainSceneUIList,
                this.expandUIList,
                ref script._editorMainSceneUIListItemStates,
                true
            );
        EditorGUILayout.Space();

        this.expandGameSceneList = PGEditorUtils.SerializedObjFoldOutList<UIPanelStruct>
            (
                "游戏场景的界面列表",
                script.GameSceneUIList,
                this.expandGameSceneList,
                ref script._editorGameSceneUIListItemStates,
                true
            );

        if (GUI.changed)
            EditorUtility.SetDirty(target);

    }
}
