using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButtonArea : Image
{

    Collider2D collider2d;

    void OnEnable()
    {
        if (this.collider2d == null)
            this.collider2d = GetComponent<Collider2D>();
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return this.collider2d.OverlapPoint(screenPoint);
    }
}
