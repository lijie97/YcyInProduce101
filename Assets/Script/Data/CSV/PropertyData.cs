using UnityEngine;
using System.Collections;
using System;

public class PropertyData : ICSVData<PropertyData> {

	public PropertyData()
	{
		InitData("PropertyData");
	}
	
	public void RefreshData()
	{
		InitData("PropertyData");
	}
	
	public string GetName(int Id)
	{
		return GetProperty("Name", Id);
	}
	
    public int GetMaxValue(int id)
    {
        return GetPropertyToInt("MaxValue", id);
    }

    public PropertyType GetPropertyType(int id)
    {
        string str = GetProperty("PropertyType", id);
        return (PropertyType)Enum.Parse(typeof(PropertyType), str);
    }
}