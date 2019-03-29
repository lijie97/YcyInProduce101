using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BatchSetLODEditor : Editor
{

    const string MenuPath = EditorUtil.MenuRoot + "LOD/";
    private static Transform[] lodTemplateGameObjs;
    private static LODGroup lodGroupTemplate;
    [MenuItem(MenuPath + "储存Lod模板")]
    public static void SaveLODGroupTemplate()
    {
        Transform[] transforms = Selection.transforms;
        if (transforms.Length == 0)
        {
            Debug.LogError("请选中Lod模板");
            return;
        }
        else
        {
            lodGroupTemplate = transforms[0].GetComponent<LODGroup>();
            lodTemplateGameObjs = new Transform[transforms[0].childCount];
            for (int i = 0; i < transforms[0].childCount; i++)
            {
                lodTemplateGameObjs[i] = transforms[0].GetChild(i);
            }
            Debug.Log("保存模板成功");
        }
    }

    [MenuItem(MenuPath + "储存Lod长度模板")]
    public static void SaveLODGroupHeightTemplate()
    {
        Transform[] transforms = Selection.transforms;
        if (transforms.Length == 0)
        {
            Debug.LogError("请选中Lod模板");
            return;
        }
        else
        {
            lodGroupTemplate = transforms[0].GetComponent<LODGroup>();
        }
        Debug.Log("保存模板成功");
    }

    [MenuItem(MenuPath + "批量设置LOD")]
    public static void BatchSetLOD()
    {
        Transform[] transforms = Selection.transforms;
        if (transforms.Length == 0)
        {
            Debug.LogError("请选中需要添加LOD组件的物体");
            return;
        }
        for (int i = 0; i < transforms.Length; i++)
        {
            Debug.Log("子物体数量+" + transforms[i].childCount + " " + transforms[i].name);
            //清空所有子物体
            if (transforms[i].childCount != 0)
            {
                for (int o = transforms[i].childCount - 1; o >= 0; o--)
                {
                    Debug.Log("删除" + transforms[i].GetChild(o).gameObject);
                    Undo.DestroyObjectImmediate(transforms[i].GetChild(o).gameObject);
                }
            }

            LOD[] newLods = new LOD[lodTemplateGameObjs.Length];
            LOD[] templateLods = lodGroupTemplate.GetLODs();
            Debug.Log("模板gameobj数量+" + lodTemplateGameObjs.Length);
            for (int k = 0; k < lodTemplateGameObjs.Length; k++)
            {
                GameObject gameObject = GameObject.Instantiate(lodTemplateGameObjs[k].gameObject);
                //Debug.Log(gameObject.name);
                gameObject.transform.SetParent(transforms[i]);
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localEulerAngles = Vector3.zero;
                gameObject.transform.localScale = Vector3.one;
                Renderer[] renderers = new Renderer[1];
                renderers[0] = gameObject.GetComponent<Renderer>();
                //复制模板lod数据到新的LOD数组
                newLods[k] = new LOD(templateLods[k].screenRelativeTransitionHeight, renderers);
            }
            //newLods[newLods.Length - 1] = new LOD();
            LODGroup lODGroup = transforms[i].gameObject.GetComponent<LODGroup>();
            if (lODGroup == null)
            {
                lODGroup = transforms[i].gameObject.AddComponent<LODGroup>();
                //Debug.Log("添加lod组件");
            }
            lODGroup.SetLODs(newLods);
            //UnityEditorInternal.ComponentUtility.CopyComponent(lodGroupTemplate);
            //UnityEditorInternal.ComponentUtility.PasteComponentAsNew(transforms[i].gameObject);
        }


        Debug.Log("设置成功");
    }

    [MenuItem(MenuPath + "批量设置LOD距离")]
    public static void BatchSetLODHeight()
    {
        Transform[] transforms = Selection.transforms;
        if (transforms.Length == 0)
        {
            Debug.LogError("请选中需要添加LOD组件的物体");
            return;
        }
        for (int i = 0; i < transforms.Length; i++)
        {           
            LODGroup lODGroup = transforms[i].gameObject.GetComponent<LODGroup>();
            if (lODGroup == null)
            {
                lODGroup = transforms[i].gameObject.AddComponent<LODGroup>();
            }
            LOD[] lODs = lODGroup.GetLODs();
            LOD[] templateLods = lodGroupTemplate.GetLODs();
            for (int k = 0; k < templateLods.Length; k++)
            {
                lODs[k].screenRelativeTransitionHeight = templateLods[k].screenRelativeTransitionHeight;
            }
            List<LOD> lodList = lODs.ToList();
            if (lodList.Count < 3)
            {
                lODGroup.SetLODs(lODs);
                continue;
            }
            else
            {
                lodList.RemoveAt(2);
                lODs = lodList.ToArray();
                lODGroup.SetLODs(lODs);
            }
        }


        Debug.Log("设置成功");
    }

}
