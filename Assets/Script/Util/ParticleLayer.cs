using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class ParticleLayer : MonoBehaviour {

	public int OrderInLayer = 0;
	public bool ApplyNow = false;
	public bool ApplyAllChild = false;

	void Awake()
	{
        return;
#if UNITY_EDITOR
		if(Application.isPlaying)
#endif
            ApplySortOrder(transform, OrderInLayer);
	}

#if UNITY_EDITOR
	void Update()
	{
		if(ApplyNow)
		{
			ApplyNow = false;
            ApplySortOrder(transform, OrderInLayer);
		}

		if (ApplyAllChild) {
			ApplyAllChild = false;
			ApplyAllChildSortOrder (transform);
		}
	}
#endif

    private void ApplySortOrder(Transform tran, int layer)
	{
		if(tran.GetComponent<Renderer>() != null)
		{
            tran.GetComponent<Renderer>().sortingOrder = layer;
		}
	}

    public void ApplyAllChildSortOrder(Transform tran, int layer = -1)
	{
        if (layer < 0)
            layer = OrderInLayer;
        if (tran == null)
            tran = transform;
        
		int count = tran.childCount;
		for (int i = 0; i < count; i++) {
            ApplySortOrder (tran.GetChild(i), layer);
			ApplyAllChildSortOrder (tran.GetChild(i));
		}
	}

}
