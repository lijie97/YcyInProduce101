using UnityEngine;
using System.Collections;

public class AudioData : ICSVData<AudioData> {

	public AudioData()
	{
		InitData("AudioData");
	}
	
	public void RefreshData()
	{
		InitData("AudioData");
	}
	
	public string GetName(int Id)
	{
		return GetProperty("Name", Id);
	}
	
	public string GetCategory(int Id)
	{
		return GetProperty("Category", Id);
	}
	
	public string GetItem(int Id)
	{
		return GetProperty ("Item", Id);
	}

	public string GetSubItem(int Id)
	{
		return GetProperty ("SubItem", Id);
	}

	public bool GetLoopMode(int Id)
	{
		return bool.Parse (GetProperty ("LoopMode", Id));
	}
}