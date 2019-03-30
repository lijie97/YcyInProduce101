using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropertyItem : MonoBehaviour {
    public TextMeshProUGUI propertyName, propertyDesc;
    public Image slider;
    private int curPropertyID;

    public void Init(int propertyID)
    {
        this.curPropertyID = propertyID;
        propertyName.text = PropertyData.Instance.GetName(propertyID);
        //propertyDesc.text = PropertyData.Instance.
        PropertyParam param = PlayerData.Instance.GetPropertyParam(propertyID);
        int maxValue = PropertyData.Instance.GetMaxValue(propertyID);

        slider.fillAmount = param.curValue * 1f / maxValue;
    }
}
