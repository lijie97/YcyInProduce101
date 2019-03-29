using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabItem : MonoBehaviour
{


    private GameObject selectIcon;
    private GameObject unSelectIcon;

    private int tabIndex;
    private BasePanel currentPanel;



    public void SetTabIndex(int index, BasePanel panel)
    {
        tabIndex = index;
        currentPanel = panel;
        if (transform.Find("SelectIcon") != null && transform.Find("UnSelectIcon") != null)
        {
            selectIcon = transform.Find("SelectIcon").gameObject;
            unSelectIcon = transform.Find("UnSelectIcon").gameObject;
        }
    }

    public void SetTabState(bool tabState)
    {
        if (selectIcon != null && unSelectIcon != null)
        {
            selectIcon.SetActive(tabState);
            unSelectIcon.SetActive(!tabState); 
        }
    }

    private void TabOnClick()
    {
        currentPanel.ChangeTab(tabIndex);

    }

}