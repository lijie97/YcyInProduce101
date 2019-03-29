using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{

    protected Vector3 minRectUILayout, maxRectUILayout;

    [HideInInspector]
    public PanelType selfPanelType;
    [HideInInspector]
    public PanelLayerType selfPanelLayerType;


    public virtual void Init()
    {

    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnResume()
    {
        gameObject.SetActive(true);
    }

    public virtual void ChangeTab(int tabIndex)
    {

    }

}
