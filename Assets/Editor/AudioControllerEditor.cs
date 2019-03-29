using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// AudioToolkit音效编辑器
/// By LeoHui
/// </summary>
public class AudioControllerEditor : Editor {

	const string MenuPath = EditorUtil.MenuRoot + "音效/";

	private static AudioController audioController;

	private static List<AudioStruct> audioStructList = new List<AudioStruct> ();
	private static List<AudioClip> audioClipList = new List<AudioClip> ();
	private static List<string> audioClipPathList = new List<string> ();

	[MenuItem(MenuPath + "更新音效")]
	public static void RefreshAudio()
	{
		InitData ();
		GameObject audioContainer = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/Resources/Audio/AudioContainer.prefab");
        GameObject newobj = PrefabUtility.InstantiatePrefab(audioContainer) as GameObject;
        audioController = newobj.GetComponent<AudioController> ();

		//删除多余的Category
		RemoveInvalidCategory ();

		//更新音效
		RefreshValidAudio ();

        PrefabUtility.ReplacePrefab(newobj, PrefabUtility.GetPrefabParent(newobj), ReplacePrefabOptions.Default);
        DestroyImmediate(newobj, true);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh ();
	}

	[MenuItem(MenuPath + "删除无效音效(没有使用的音效)")]
	public static void InvalidAudio()
	{
		InitData ();
		List<string> audioClipNameList = new List<string> ();
		for (int i = 0; i < audioClipList.Count; ++i) {
			audioClipNameList.Add (audioClipList [i].name);
		}
		for (int i = 0; i < audioStructList.Count; ++i) {
			if (audioClipNameList.Contains (audioStructList [i].subItem)) {
				audioClipNameList.Remove (audioStructList [i].subItem);
				continue;
			}
		}
		for (int i = 0; i < audioClipNameList.Count; ++i) {
			for (int j = 0; j < audioClipPathList.Count; ++j) {
				if (audioClipPathList [j].Contains (audioClipNameList [i])) {
					AssetDatabase.DeleteAsset (audioClipPathList [i]);
					break;
				}
			}
		}
		AssetDatabase.Refresh ();
	}

	[MenuItem(MenuPath + "缺失音效(使用不存在的音效)")]
	public static void MissingAudio()
	{
		InitData ();
		for (int i = 0; i < audioStructList.Count; ++i) {
			if (audioStructList [i].audioClip == null) {
				Debug.Log (audioStructList [i].subItem);
			}
		}
	}

	private static void InitData()
	{
		AudioData.Instance.RefreshData ();
		audioStructList.Clear ();
		audioClipList.Clear ();
		audioClipPathList.Clear ();

		//初始化数据表格
		int rowCount = AudioData.Instance.GetDataRow ();
		for (int i = 1; i <= rowCount; ++i) {
			AudioStruct audioStruct = new AudioStruct ();
			audioStruct.id = i;
			audioStruct.name = AudioData.Instance.GetName (i);
			audioStruct.category = AudioData.Instance.GetCategory (i);
			audioStruct.item = AudioData.Instance.GetItem (i);
			audioStruct.subItem = AudioData.Instance.GetSubItem (i);
			audioStruct.loopMode = AudioData.Instance.GetLoopMode (i);
			audioStruct.audioClip = null;
			audioStructList.Add (audioStruct);
		}

		//初始化音效文件
		string[] pathArr = { "Assets/Audio" };
		string[] audioArr = AssetDatabase.FindAssets ("t:AudioClip", pathArr);
		if (audioArr == null || audioArr.Length <= 0) {
			Debug.Log("没有音效文件!");
			return;
		}
		for (int i = 0; i < audioArr.Length; ++i) {
			string pathName = AssetDatabase.GUIDToAssetPath (audioArr [i]);
			AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip> (pathName);
			audioClipList.Add (audioClip);
			audioClipPathList.Add (pathName);
		}

		//将AudioClip关联到AudioStruct
		for (int i = 0; i < audioStructList.Count; ++i) {
			for (int j = 0; j < audioClipList.Count; ++j) {
				if (audioStructList [i].subItem == audioClipList [j].name) {
					AudioStruct audioStruct = audioStructList [i];
					audioStruct.audioClip = audioClipList [j];
					audioStructList [i] = audioStruct;
					break;
				}
			}
		}
	}

	/// <summary>
	/// 更新有效的音效
	/// </summary>
	private static void RefreshValidAudio()
	{
		List<string> categoryList = new List<string> ();
		for (int i = 0; i < audioController.AudioCategories.Length; ++i) {
			categoryList.Add (audioController.AudioCategories [i].Name);
		}
		//删除所有的AudioItem
		RemoveAllAudioItem ();
		for (int i = 0; i < audioStructList.Count; ++i) {
			//如果不存在Category，添加新的Category
			if (!categoryList.Contains (audioStructList [i].category)) {
				AddCategory (audioStructList [i].category);
				categoryList.Add (audioStructList [i].category);
			}

			//添加AudioItem
			AudioCategory audioCategory = GetAudioCategoryByName (audioStructList [i].category);
			AddAudioItem (audioCategory, audioStructList [i].audioClip, audioStructList [i].item, audioStructList [i].loopMode);
		}
	}

	private static AudioCategory GetAudioCategoryByName (string name)
	{
		for (int i = 0; i < audioController.AudioCategories.Length; ++i) {
			if (audioController.AudioCategories [i].Name == name)
				return audioController.AudioCategories [i];
		}
		return null;
	}

	/// <summary>
	/// 删除多余的Category
	/// </summary>
	private static void RemoveInvalidCategory()
	{
		List<string> categoryList = new List<string> ();
		for (int i = 0; i < audioController.AudioCategories.Length; ++i) {
			categoryList.Add (audioController.AudioCategories [i].Name);
		}
		for (int i = 0; i < audioStructList.Count; ++i) {
			if (categoryList.Contains (audioStructList [i].category)) {
				categoryList.Remove (audioStructList [i].category);
				continue;
			}
		}
		for (int i = 0; i < categoryList.Count; ++i) {
			RemoveCategory (categoryList [i]);
		}
	}

	private static void RemoveAllAudioItem()
	{
		for (int i = 0; i < audioController.AudioCategories.Length; ++i) {
			audioController.AudioCategories [i].AudioItems = null;
		}
	}

	private static void AddCategory(string categoryName)
	{
		int oldCategoryCount = audioController.AudioCategories != null ? audioController.AudioCategories.Length : 0;
		var oldArray = audioController.AudioCategories;
		audioController.AudioCategories = new AudioCategory[ oldCategoryCount + 1 ];

		if ( oldCategoryCount > 0 )
		{
			oldArray.CopyTo( audioController.AudioCategories, 0 );
		}

		var newCategory = new AudioCategory( audioController );
		newCategory.Name = categoryName;

		audioController.AudioCategories[ oldCategoryCount ] = newCategory;
	}

	private static void RemoveCategory( string categoryName )
	{
		int i, index = -1;
		int oldCategoryCount;

		if ( audioController.AudioCategories != null )
		{
			oldCategoryCount = audioController.AudioCategories.Length;
		}
		else
			oldCategoryCount = 0;

		for ( i = 0; i < oldCategoryCount; i++ )
		{
			if ( audioController.AudioCategories[ i ].Name == categoryName )
			{
				index = i;
				break;
			}
		}

		if ( index == -1 )
		{
			Debug.LogError( "AudioCategory does not exist: " + categoryName );
			return;
		}

		var newArray = new AudioCategory[ audioController.AudioCategories.Length - 1 ];
		for ( i = 0; i < index; i++ )
		{
			newArray[ i ] = audioController.AudioCategories[ i ];
		}
		for ( i = index + 1; i < audioController.AudioCategories.Length; i++ )
		{
			newArray[ i - 1 ] = audioController.AudioCategories[ i ];
		}
		audioController.AudioCategories = newArray;
	}

	private static void AddAudioItem(AudioCategory category, AudioItem audioItem)
	{
		int oldCount = category.AudioItems != null ? category.AudioItems.Length : 0;
		var oldArray = category.AudioItems;
		category.AudioItems = new AudioItem[ oldCount + 1 ];

		if ( oldCount > 0 )
		{
			oldArray.CopyTo( category.AudioItems, 0 );
		}

		category.AudioItems[ oldCount ] = audioItem;
	}

	private static AudioItem AddAudioItem(AudioCategory category, AudioClip audioClip, string audioID, bool loopMode)
	{
		var audioItem = new AudioItem();
		audioItem.Name = audioID;
		audioItem.subItems = new AudioSubItem[ 1 ];
		if (loopMode)
			audioItem.Loop = AudioItem.LoopMode.LoopSubitem;
		else
			audioItem.Loop = AudioItem.LoopMode.DoNotLoop;

		var audioSubItem = new AudioSubItem();
		audioSubItem.Clip = audioClip;
		audioItem.subItems[ 0 ] = audioSubItem;

		AddAudioItem( category, audioItem );
		return audioItem;
	}
}
public struct AudioStruct {
	public int id;
	public string name;
	public string category;
	public string item;
	public string subItem;
	public bool loopMode;
	public AudioClip audioClip;
}