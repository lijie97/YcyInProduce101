using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlatformSetting.Instance.Init();
        SceneManager.LoadScene("Game");
    }

}
