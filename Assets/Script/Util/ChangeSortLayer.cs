using UnityEngine;
using System.Collections;

/// <summary>
/// 用于改变一个模块中的所有精灵文本层数.
/// </summary>
[ExecuteInEditMode]
public class ChangeSortLayer : MonoBehaviour
{

    public int AddSortOrder = 0;
    public bool ApplyNow = false;
	
    #if UNITY_EDITOR
    void Update()
    {
        if (ApplyNow)
        {
            ApplyNow = false;
            ChangeSort(this.transform);
        }
    }
    #endif
	
    void ChangeSort(Transform tran)
    {
//		if(tran.GetComponent<tk2dBaseSprite>() != null)
//			tran.GetComponent<tk2dBaseSprite>().SortingOrder += AddSortOrder;
//			
//		if(tran.GetComponent<tk2dTextMesh>() != null)
//			tran.GetComponent<tk2dTextMesh>().SortingOrder += AddSortOrder;
//			
//		if(tran.GetComponent<TextLayer>() != null)
//		{
//			tran.GetComponent<TextLayer>().orderInLayer += AddSortOrder;
//			tran.GetComponent<TextLayer>().ApplySortingOrder();
//		}
//		
//		if(tran.GetComponent<EasyFontTextMesh>() != null)
//		{
//			tran.GetComponent<EasyFontTextMesh>().OrderInLayer += AddSortOrder;
//		}
//		
        for (int i = 0; i < tran.childCount; i++)
        {
            ChangeSort(tran.GetChild(i));
        }
    }
}
