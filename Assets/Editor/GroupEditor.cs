using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GroupEditor {

	public const string MenuPath = EditorUtil.MenuRoot + "物体打组/";

	private static Vector3 Vector3Max = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
	private static Vector3 Vector3Min = new Vector3(float.MinValue, float.MinValue, float.MinValue);
	[MenuItem(MenuPath +  "打组 %g")]
	public static void GroupGO()
	{
		Object[] objs = Selection.objects;
		List<Transform> list = new List<Transform> ();
		foreach (GameObject obj1 in objs) {
			bool add = true;
			foreach (GameObject obj2 in objs) {
				if (obj1.transform.parent == obj2.transform) {
					add = false; 
					break;
				}
			}
			if (add == true)
				list.Add (obj1.transform);
		}
		GameObject group = new GameObject ();
		group.name = "Group";
		group.transform.position = Vector3.zero;
		group.transform.eulerAngles = Vector3.zero;
		group.transform.localScale = Vector3.one;

		foreach (Transform tf in list)
			tf.parent = group.transform;
	}

}