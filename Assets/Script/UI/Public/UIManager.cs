using System;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;

public class UIManager : MonoSingletonBase<UIManager>
{
    //public GameObject LoadingPanel;
    public PanelType curUIPanelType, curStackPeekUIPanelType;
    public Transform[] layerList;

    public Dictionary<PanelType, BasePanel> panelDict = new Dictionary<PanelType, BasePanel>();
    //保存所有实例化面板的游戏物体身上的BasePanel组件
    private Dictionary<PanelType, PanelLayerType> panelLayerDict = new Dictionary<PanelType, PanelLayerType>();
    //层级
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();

    public int loadingPanelIndex = 0, loadingPanelCount = 0;
    private BasePanel infoPanel;

    private void Start()
    {
        Init();
        ShowPanel(PanelType.DevelopPanel);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.L)){
        //    GuidePanel.Instance.ShowGuideById(201);

        //}

    }
    

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerData.Instance.SaveData();
            SettingData.Instance.SaveData();
        }
    }

    private void OnApplicationQuit()
    {
        PlayerData.Instance.SaveData();
        SettingData.Instance.SaveData();

    }

    public override void Init()
    {
        StartCoroutine(IELoadPanel());       
    }


    public void ShowPanel(PanelType panelType)
    {

        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        BasePanel prePanel = GetPanel(panelType);
        //判断一下栈里面是否有页面
        if (panelStack.Count > 0 && prePanel.selfPanelLayerType != PanelLayerType.PopPanel)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.Hide();
        }

        BasePanel panel = GetPanel(panelType);
        panelStack.Push(panel);
        curUIPanelType = panel.selfPanelType;
        panel.Show();
    }

    public void HidePanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0)
            return;

        //关闭栈顶页面的显示
        BasePanel topPanel = panelStack.Pop();
        topPanel.Hide();

        if (panelStack.Count <= 0)
            return;
        BasePanel topPanel2 = panelStack.Peek();
        topPanel2.OnResume();
        curUIPanelType = topPanel2.selfPanelType;

    }

    /// <summary>
    /// 根据面板类型 得到实例化的面板
    /// </summary>
    /// <returns></returns>
    private BasePanel GetPanel(PanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<PanelType, BasePanel>();
        }

        BasePanel panel;
        panelDict.TryGetValue(panelType, out panel);

        if (panel == null)
        {
            string name = panelType.ToString();
            GameObject instPanel = ResourcesManager.Instance.LoadUIPanel(name);
            panelDict.Add(panelType, instPanel.GetComponent<BasePanel>());
            return instPanel.GetComponent<BasePanel>();
        }
        else
        {
            return panel;
        }

    }

    private void LoadAndInitPanel(UIPanelStruct uiPanelStruct)
    {
        GameObject gameObj = ResourcesManager.Instance.LoadUIPanel(uiPanelStruct.panelType.ToString());
        for (int i = 0; i < layerList.Length; i++)
        {
            if (layerList[i].name.Equals(uiPanelStruct.layerType.ToString()))
            {
                gameObj.transform.SetParent(layerList[i]);
                //ChangeAllLayer(gameObj.transform, layerList[i].gameObject.layer);
            }
        }

        RectTransform rectTransform = gameObj.GetComponent<RectTransform>();
        rectTransform.offsetMax = Vector3.zero;
        rectTransform.offsetMin = Vector3.zero;
        gameObj.transform.localPosition = Vector3.zero;
        gameObj.transform.rotation = Quaternion.identity;
        gameObj.transform.localScale = Vector3.one;
        BasePanel panelScript = gameObj.GetComponent<BasePanel>();
        panelScript.selfPanelType = uiPanelStruct.panelType;
        panelScript.selfPanelLayerType = uiPanelStruct.layerType;
       
        panelScript.Init();
        panelDict.Add(uiPanelStruct.panelType, panelScript);
        panelLayerDict.Add(uiPanelStruct.panelType, uiPanelStruct.layerType);
        gameObj.SetActive(false);
    }


    IEnumerator IELoadPanel()
    {
        loadingPanelIndex = 0;
        loadingPanelCount = UIPanelData.Instance.MainSceneUIList.Count;

        for (int i = 0; i < UIPanelData.Instance.MainSceneUIList.Count; i++)
        {
            LoadAndInitPanel(UIPanelData.Instance.MainSceneUIList[i]);
            yield return 0;
            ++loadingPanelIndex;
        }
		LoadingFinish ();
    }


    /// <summary>
    /// 控制infoPanel的显示
    /// </summary>
    /// <returns><c>true</c>, if info panel state was gotten, <c>false</c> otherwise.</returns>
    /// <param name="panelType">Panel type.</param>
    public bool GetInfoPanelState(PanelType panelType)
    {
        switch (panelType)
        {
            case PanelType.MainUIPanel:
                return true;
        }
        return false;
    }


	private void LoadingFinish()
	{
	}


   

}

