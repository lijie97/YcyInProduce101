using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Appear : MonoBehaviour {
    public bool appearing;
    public bool disappearing;
    Image image;
    byte alphaTunnel = 0xFF;
    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
	}
    public void setAlphaTunel(byte value) {
        alphaTunnel = value;
    }
    public void fillAlphaTunel() {
        alphaTunnel = 255;
    }
    public void ResetAlphaTunel()
    {
        alphaTunnel = 0;
    }
    // Update is called once per frame
    void Update () {

        if (appearing && alphaTunnel<255) alphaTunnel += 5;
        if (disappearing && alphaTunnel > 0) alphaTunnel -= 5;
        if (alphaTunnel%10==0)
            image.color = new Color32(255,255,255, alphaTunnel);
	}

}
