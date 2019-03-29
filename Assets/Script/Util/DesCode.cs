using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System;

/// <summary>
/// DES code. DES 加密
/// by 湛昌
/// 
/// </summary>
public class DesCode
{

	// static string password="huibang0";
	public static string PassWord {
		get {
			string password = PlayerPrefs.GetString ("KeyString", "");
			if (string.IsNullOrEmpty (password)) {
				Debug.Log ("key in null.");
				password = SystemInfo.deviceUniqueIdentifier.Substring (0, 8);
				#if UNITY_IOS || UNITY_IPHONE
				password = "youmeng!";
				#endif
				PlayerPrefs.SetString ("KeyString", password);
			}
			
			return password;
		}
	}
	
	//默认密钥向量
	private static byte[] Keys = { 0xEF, 0xAB, 0x56, 0x78, 0x90, 0x34, 0xCD, 0x12 };

	/// <summary>
	/// DES加密字符串
	/// </summary>
	/// <param name="encryptString">待加密的字符串</param>
	/// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
	public static string EncryptDES (string encryptString, string password = "huibang0")
	{
		string encryptKey = password;
		try {
			byte[] rgbKey = Encoding.UTF8.GetBytes (encryptKey.Substring (0, 8));
			byte[] rgbIV = Keys;
			byte[] inputByteArray = Encoding.UTF8.GetBytes (encryptString);
			DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider ();
			MemoryStream mStream = new MemoryStream ();
			CryptoStream cStream = new CryptoStream (mStream, dCSP.CreateEncryptor (rgbKey, rgbIV), CryptoStreamMode.Write);
			cStream.Write (inputByteArray, 0, inputByteArray.Length);
			cStream.FlushFinalBlock ();
			return Convert.ToBase64String (mStream.ToArray ());
		} catch {
			return null;
		}
	}

	/// <summary>
	/// DES解密字符串
	/// </summary>
	/// <param name="decryptString">待解密的字符串</param>
	/// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
	/// <returns>解密成功返回解密后的字符串，失败返null</returns>
	public static string DecryptDES (string decryptString, string password = "huibang0")
	{
		string decryptKey = password;
		try {
			byte[] rgbKey = Encoding.UTF8.GetBytes (decryptKey.Substring (0, 8));
			byte[] rgbIV = Keys;
			byte[] inputByteArray = Convert.FromBase64String (decryptString);
			DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider ();
			MemoryStream mStream = new MemoryStream ();
			CryptoStream cStream = new CryptoStream (mStream, DCSP.CreateDecryptor (rgbKey, rgbIV), CryptoStreamMode.Write);
			cStream.Write (inputByteArray, 0, inputByteArray.Length);
			cStream.FlushFinalBlock ();
			return Encoding.UTF8.GetString (mStream.ToArray ());
		} catch {
			return null;
		}
	}

	
	public static void DeleteDesKey (string key)
	{
		if (null == key || key.Length == 0) {
			Debug.LogError ("null key");
			return;
		}
		string des_key = EncryptDES (key);
		PlayerPrefs.DeleteKey (des_key);
	}

	#region  替换 PlayerPrefs, 数据加密存储

	public static void SetInt (string key, int val)
	{
		if (null == key || key.Length == 0) {
			Debug.LogError ("null key");
			return;
		}
		string des_key = EncryptDES (key);
		string des_val = EncryptDES (val.ToString ());
		
		PlayerPrefs.SetString (des_key, des_val);
		
	}

	public static int GetInt (string key)
	{
		if (null == key || key.Length == 0) {
			Debug.LogError ("null key");
			return 0;
		}
		
		try {
			string des_key = EncryptDES (key);
			string des_val = PlayerPrefs.GetString (des_key);
			
			if (des_val == "" || des_val.Length == 0) {
				return 0;
			}
			
			string str_val = DecryptDES (des_val);
			
			int val = int.Parse (str_val);
			
			return val;
				
		} catch {
			Debug.LogError ("error change int");
		}
		
		return 0;
	}

	public static void SetLong (string key, long val)
	{
		if (null == key || key.Length == 0) {
			Debug.LogError ("null key");
			return;
		}
		string des_key = EncryptDES (key);
		string des_val = EncryptDES (val.ToString ());
		
		PlayerPrefs.SetString (des_key, des_val);
		
	}

	public static long GetLong (string key)
	{
		if (null == key || key.Length == 0) {
			Debug.LogError ("null key");
			return 0;
		}
		
		try {
			string des_key = EncryptDES (key);
			string des_val = PlayerPrefs.GetString (des_key);
			
			if (des_val == "" || des_val.Length == 0) {
				return 0;
			}
			
			string str_val = DecryptDES (des_val);
			
			long val = long.Parse (str_val);
			
			return val;
				
		} catch {
			Debug.LogError ("error change long");
		}
		
		return 0;
	}

	
	public static void SetString (string key, string str)
	{
		if (null == key || key.Length == 0 || null == str) {
			Debug.LogError ("null key or null str");
			return;
		}
		string des_key = EncryptDES (key);
		string des_str = EncryptDES (str);
		PlayerPrefs.SetString (des_key, des_str);
	}

	public static string GetString (string key)
	{
		if (null == key || key.Length == 0) {
			Debug.LogError ("null key");
			return "";
		}
		try {
			string des_key = EncryptDES (key);
			string des_val = PlayerPrefs.GetString (des_key);
			
			if (des_val.Length == 0) {
				return "";
			}
			
			string val = DecryptDES (des_val);
			return val;
		} catch {
			
			Debug.LogError ("error read string");
			return null;
		}	
	}

	#endregion

	
	
	#region  Des 算法加密解密存储  输入的 val 是内存加密过的数据

	/// <summary>
	/// Sets the DES int.  
	/// </summary>
	/// <param name='key'>
	/// Key.
	/// </param>
	/// <param name='val'>
	/// Value. 加密过的val
	/// </param>
	public static void SetDesInt (string key, int val)
	{
		if (null == key || key.Length == 0) {
			Debug.LogError ("null key");
			return;
		}
		
		val = IntToView (val);
		string des_key = EncryptDES (key);
		string des_val = EncryptDES (val.ToString ());
		
		PlayerPrefs.SetString (des_key, des_val);
	}

	public static int GetDesInt (string key)
	{
		if (null == key || key.Length == 0) {
			Debug.LogError ("null key");
			return 0;
		}
		
		try {
			string des_key = EncryptDES (key);
			string des_val = PlayerPrefs.GetString (des_key);
			
			if (des_val == "" || des_val.Length == 0) {
				return  DesCode.IntToMomey (0);
			}
			
			string str_val = DecryptDES (des_val);
			
			int val = int.Parse (str_val);
			
			val = IntToMomey (val);
			
			return val;
				
		} catch {
			Debug.LogError ("error change int");
		}
		
		return 0;
	}

	public static void SetDesFloat (string key, float val)
	{
		if (null == key || key.Length == 0) {
			Debug.LogError ("null key");
			return;
		}
		val = FloatToView (val);
		string des_key = EncryptDES (key);
		string des_val = EncryptDES (val.ToString ());
		
		PlayerPrefs.SetString (des_key, des_val);
		
	}

	public static float GetDesFloat (string key)
	{
		if (null == key || key.Length == 0) {
			Debug.LogError ("null key");
			return 0;
		}
		
		try {
			string des_key = EncryptDES (key);
			string des_val = PlayerPrefs.GetString (des_key);
		
			if (des_val == "" || des_val.Length == 0) {
				return   DesCode.FloatToMoney (0);
			}
		  
			string str_val = DecryptDES (des_val);
			
			float val = float.Parse (str_val);
			
			val = FloatToMoney (val);	
			return val;
			
		} catch {
			Debug.LogError ("error change int");
		}
		
		return 0f;
	}

	
	
	
	#endregion

	
	#region  数字转换 加密

	public static int IntAdd (int view, int momey)
	{
		int v = IntToView (momey);
		v += view;
		
		return IntToMomey (v);
		
	}

	
	
	public static bool IntEqual (int view, int momey)
	{
		int v = IntToView (momey);
		
		if (v == view) {
			return true;
		} else {
			return false;
		}
	}

	public static int IntToView (int val)
	{
		return (val + GetOffset ());
	}

	public static int IntToMomey (int val)
	{
		return (val - GetOffset ());
	}

	
	public static bool FloatEqual (float view, float money)
	{
		float v = FloatToView (money);
		if (v == view) {
			return true;
		} else {
			return false;
		}
	}

	
	public static float FloatAdd (int view, int momey)
	{
		float v = FloatToView (momey);
		v += view;
		
		return FloatToMoney (v);
	}

	public static float FloatToView (float val)
	{
		return val + GetOffset ();
		
	}

	public static float FloatToMoney (float val)
	{
		return val - GetOffset ();
	}

	static int offset = 0;

	private static int GetOffset ()
	{
		if (offset != 0) {
			return offset;
		}
		
		//read from 
		offset = DesCode.GetInt ("OFFSET");
		
		if (0 == offset) {  // 如果没有 则生成随机数
			do {
				offset = (int)(UnityEngine.Random.value * 1000);
			} while(offset == 0);
			DesCode.SetInt ("OFFSET", offset);
		}
		return offset;
	}

	#endregion

	
	#region  内存验证

	//内存数据加密验证
	public static string golden_des = "";
	public static string money_des = "";
	public static string mine_des = "";

	public  enum des_type
	{
Gold,
Moeny,
Mine}

	;

	public static des_type VerifyType = des_type.Gold;

	public static bool IsVerify (int number, des_type type)
	{
		string ver_str = null;
		
		switch (type) {
		case des_type.Gold:
			ver_str = golden_des;
			break;
		case des_type.Moeny:
			ver_str = money_des;
			break;
		case des_type.Mine:
			ver_str = mine_des;
			break;
		}
		;
		
		try {
			string str = DesCode.DecryptDES (ver_str);
			int t = int.Parse (str);
				
			if (number == t) {
				return true;
			} else {
				return false;
			}
				
		} catch {
			return false;	
		}
		
		return false;
	}

	#endregion
	
}
