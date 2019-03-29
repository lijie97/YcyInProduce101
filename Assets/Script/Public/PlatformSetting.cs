using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlatformType
{
	YiDong = 0,
	DianXin = 1,
	LianTong = 2
};
public enum ChannelType
{
	SDK = 0,		//使用SDK的退出接口
	Default = 1		//使用游戏自带的退出接口
};
public enum PayVersionType
{
	ShenHe = 0,
	LiBaoBai = 1,
	LiBaoHei = 2,
	IOS = 3
};

public class PlatformSetting : MonoSingletonBase<PlatformSetting>
{

    public static string channelID = "unity";
    public static string sign = "unity";
	public static string imsi = "0";

	public static bool isOpenUpdateAssets = false;
    public static string hotfixAssetServer = "";

	public static int orderPayId=0;//游戏补单的PayId

    public override  void Init()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "PlatformSetting";

		#if UNITY_EDITOR
		EditorSetup();
		#endif
    }


	private void EditorSetup()
	{
		
	}

    #region  Unity设置

    private bool showMoreGame;
    private bool showAboutInfo;


	public void SetIMSI(string strID)
	{
		Debug.Log("SetIMSI:" + strID);
		imsi = strID;
	}

    public void SetChannelID(string strID)
    {
        Debug.Log("SetChannelID:" + strID);
        channelID = strID;
    }

    /// <summary>
    /// 设置android 的签名指纹
    /// </summary>
    /// <param name="str">String.</param>
    public void SetSign(string str)
    {
        Debug.Log("Not use any more. SetSign:" + str);
        //sign = str;
    }

    public void SetShowMoreGame(string result)
    {
        bool flag = bool.Parse(result);
        print("ShowMoreGame: " + flag.ToString());
       // BuildSetting.Instance.IsShowSettingMoreGame = flag;
    }

    public void SetShowAboutInfo(string result)
    {
        bool flag = bool.Parse(result);
        print("ShowAboutInfo: " + flag.ToString());
       // BuildSetting.Instance.IsShowSettingInfo = flag;
    }
		
	public void SetIsExchangeCodeShow(string result)
	{
		bool flag = bool.Parse(result);
		print("SetIsExchangeCodeShow: " + flag.ToString());
		//BuildSetting.Instance.IsExchangeCodeShow = flag;
	}
    

    #endregion


    #region  Android SDK 接口

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("ExitGame");

        #if UNITY_EDITOR
        return;
        #endif

        #if UNITY_ANDROID
        AndroidJavaClass androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject androidJO = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
        androidJO.Call("OnExitGame");
        #endif
    }

    /// <summary>
    /// 更多游戏
    /// </summary>
    public void MoreGame()
    {
		Debug.Log("MoreGame");
        #if UNITY_EDITOR
        return;
        #endif

        #if UNITY_ANDROID
        AndroidJavaClass androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject androidJO = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
        androidJO.Call("OnMoreGame");
        #endif
    }

    /// <summary>
    /// 关于信息
    /// </summary>
    public void AboutInfo()
    {
		Debug.Log("AboutInfo");
        #if UNITY_EDITOR
        return;
        #endif

        #if UNITY_ANDROID
        AndroidJavaClass androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject androidJO = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
        androidJO.Call("OnAboutInfo");
        #endif
    }

	/// <summary>
	/// 关闭礼包触发的事件
	/// </summary>
	public void PackageClose()
	{
		Debug.Log("PackageClose");
		#if UNITY_EDITOR
		return;
		#endif

		#if UNITY_ANDROID
		AndroidJavaClass androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject androidJO = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
		androidJO.Call("PackageClose");
		#endif
	}

    #endregion


   

    #region 计费

	/// <summary>
	/// 游戏补单处理
	/// </summary>
	/// <param name="payIdStr">Pay identifier string.</param>
	public void SetOrderPayId(string payIdStr)
	{
		Debug.Log ("SetOrderPayId "+payIdStr);
		int payId = int.Parse (payIdStr);
		orderPayId = payId;
	}
		
    /// <summary>
    /// 广告直接通过
    /// </summary>
    /// <param name="result">Result.</param>
    public void SetIsAdFreeReturn(string result)
    {
        Debug.Log("IsAdFreeReturn:" + result);
        bool flag = bool.Parse(result);
       // BuildSetting.Instance.IsAdVersion = flag;
    }

    /// <summary>
    /// 商店二次确认界面
    /// </summary>
    /// <param name="result">Result.</param>
    public void SetIsShowShopSecondSure(string result)
    {
        Debug.Log("IsShowShopSecondSure:" + result);
        bool flag = bool.Parse(result);
       // BuildSetting.Instance.IsShowShopSecondSure = flag;
    }
		
    /// <summary>
    /// 是否使用SDK 的支付回调
    /// </summary>
    /// <param name="result">Result.</param>
    public void SetIsUseSDKPayCallback(string result)
    {
        Debug.Log("isUseSDKPayCallback:" + result);
        bool flag = bool.Parse(result);
        //AndroidSDKManage.Instance.isUseSDKPayCallback = flag;
    }


	/// <summary>
	/// 设置支付方式
	///  other(第三方支付) fourNet(运营商支付)
	/// </summary>
	/// <param name="typeStr">Type string.</param>
	public void SetPayType(string typeStr)
	{
		Debug.Log ("SetPayType:"+typeStr);
		//PayBuild.PayBuildPayManage.Instance.curPayType = typeStr;
	}

    #endregion

	#region 计费版本

	/// <summary>
	/// 修改礼包的计费版本
	/// </summary>
	/// <param name="versionIndex">Version index.</param>
	public void SetPayVersionType(string versionIndex)
	{
//		Debug.Log ("SetPayVersionType:"+versionIndex);
//		int index = int.Parse (versionIndex);
//		PayVersionType payVersionType = (PayVersionType)index;
//		BuildSetting.Instance.payVersionType = payVersionType;
//		PayJsonData.Instance.InitData (payVersionType);
//		Debug.Log ("OK SetPayVersionType "+ BuildSetting.Instance.payVersionType.ToString());
//		if (payVersionType == PayVersionType.LiBaoBai) {
//			BuildSetting.Instance.IsOperatorVersion = false; //修改礼包价格版本
//		}if (payVersionType == PayVersionType.LiBaoHei) {
//			BuildSetting.Instance.IsOperatorVersion = true; //修改礼包价格版本
//		}
	}

	public void SetPlatformType(string result)
	{
		PlatformType itemType = (PlatformType)int.Parse(result);
		print ("PlatformType: " + itemType.ToString ());
		//PayJsonData.Instance.payJsonData.curPlatformType = itemType;
	}

	public void SetChannelType(string result)
	{
		ChannelType itemType = (ChannelType)int.Parse(result);
		print ("ChannelType: " + itemType.ToString ());
//		PayJsonData.Instance.payJsonData.curChannelType = itemType;
	}

	public void SetPackageAState(string result)
	{
		bool state = bool.Parse(result);
		print ("SetPackageAState: " + state.ToString ());
//		PayJsonData.Instance.payJsonData.packageA = state;
	}

	public void SetPackageBState(string result)
	{
		bool state = bool.Parse(result);
		print ("SetPackageBState: " + state.ToString ());
//		PayJsonData.Instance.payJsonData.packageB = state;
	}
	public void SetPackageCState(string result)
	{
		bool state = bool.Parse(result);
		print ("SetPackageCState: " + state.ToString ());
//		PayJsonData.Instance.payJsonData.packageC = state;
	}
	public void SetPackageDState(string result)
	{
		bool state = bool.Parse(result);
		print ("SetPackageDState: " + state.ToString ());
//		PayJsonData.Instance.payJsonData.packageD = state;
	}
	#endregion
}