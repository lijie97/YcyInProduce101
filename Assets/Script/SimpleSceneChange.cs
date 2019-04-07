using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SimpleSceneChange : MonoBehaviour
{
    public Button button;//Please set your button name as " 'Name of Scene' + 'Button'", such as "HomeButton","DeckButton".
    public string NextSceneName = "";
    //If you dont want script automatically auto get your button name, please input your ButtonName in here.
    //If you want ,please let ButtonName == "" .

    void OnSceneChangeClick()
    {
        SceneManager.LoadScene(GetNextSceneName());
    }

    string GetNextSceneName()
    {
        return NextSceneName;
    }

    void Start()
    {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(OnSceneChangeClick);
    }
}
