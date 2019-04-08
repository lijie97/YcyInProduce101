using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropertyItem : MonoBehaviour {
    public Image slider,bg,icon;
    private int curPropertyID;
    public Sprite moodSprite, vigorSprite, moodIconSprite, vigorIconSprite;
    public void Init(int propertyID)
    {
        this.curPropertyID = propertyID;
        //propertyName.text = PropertyData.Instance.GetName(propertyID);
        //propertyDesc.text = PropertyData.Instance.
        PropertyParam param = PlayerData.Instance.GetPropertyParam(propertyID);
        int maxValue = PropertyData.Instance.GetMaxValue(propertyID);
        slider.fillAmount = param.curValue * 1f / maxValue;
        bg.sprite = param.propertyType == PropertyType.Mood ? moodSprite : vigorSprite;
        icon.sprite = param.propertyType == PropertyType.Mood ? moodIconSprite : vigorIconSprite;
        icon.SetNativeSize();
    }

    public void RefreshInfo()
    {
        //propertyName.text = PropertyData.Instance.GetName(curPropertyID);
        //propertyDesc.text = PropertyData.Instance.
        PropertyParam param = PlayerData.Instance.GetPropertyParam(curPropertyID);
        int maxValue = PropertyData.Instance.GetMaxValue(curPropertyID);

        slider.fillAmount = param.curValue * 1f / maxValue;
    }
}
