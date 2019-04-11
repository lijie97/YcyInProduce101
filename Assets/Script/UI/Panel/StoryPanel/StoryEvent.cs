using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class StoryEvent : MonoSingletonBase<StoryEvent>
{

    public override void Init()
    {
        gameObject.name = "StoryEvent";
    }


}
