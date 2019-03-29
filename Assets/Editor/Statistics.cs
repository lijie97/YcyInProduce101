using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Statistics : Editor
{
    public const string MenuPath = EditorUtil.MenuRoot + "统计/";


    [MenuItem(MenuPath +"统计FBX顶点数")]
    public static void StatiFBX()
    {
        string startPath = Application.dataPath + "/Model/";
        GetAllFile(startPath);
    }

    private static void GetAllFile(string path)
    {
        DirectoryInfo dinfo = new DirectoryInfo(path);

        DirectoryInfo[] childDir = dinfo.GetDirectories();
        for (int i = 0; i < childDir.Length; ++i)
        {
            GetAllFile(childDir[i].FullName);
        }

        FileInfo[] childFile = dinfo.GetFiles();
        for (int j = 0; j < childFile.Length; ++j)
        {

            if (childFile[j].Extension == ".fbx" || childFile[j].Extension == ".FBX")
            {
                string mPath = childFile[j].FullName;
                int index = mPath.IndexOf("Assets");
                mPath = mPath.Substring(index);
                ShowFBXVertex(mPath);
            }            
        }

    }

    private static void ShowFBXVertex(string path)
    {
        //Debug.Log(path);
        Transform prefabTran = AssetDatabase.LoadAssetAtPath<Transform>(path);

        if (prefabTran == null)
            return;

        Transform t = Instantiate<Transform>(prefabTran);

        if (t == null)
            return;

        MeshFilter[] mfs = t.GetComponentsInChildren<MeshFilter>();

	if (mfs == null || mfs.Length<1)
            return;
	
		MeshFilter curMf=mfs[0];

		for(int i=0;i<mfs.Length;++i)
		{
			if(mfs[i].gameObject.name.Contains("yingzi"))
				continue;
			curMf = mfs[i];
			break;
		}

		Debug.Log(path + " " + curMf.sharedMesh.vertexCount );
        DestroyImmediate(t.gameObject,true);
    }
   
}
