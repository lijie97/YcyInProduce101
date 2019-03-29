using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : MonoSingletonBase<AtlasManager>
{

	public SpriteAtlas uiAtlas;
	public SpriteAtlas bigAltas;


	public override void Init ()
	{
		uiAtlas = Resources.Load<SpriteAtlas> ("Atlas/UIAtlas");
		bigAltas = Resources.Load<SpriteAtlas> ("Atlas/BigUIAtlas");
	}

	public Sprite GetSprite (AtlasType atlasType, string spriteName)
	{
		switch (atlasType) {
		case AtlasType.UIAtlas:
			Sprite sprite = uiAtlas.GetSprite (spriteName);
			if (sprite != null) {
				return sprite;
			} else {
				sprite = bigAltas.GetSprite (spriteName);

				if (sprite != null) {
					return sprite;
				} else {
					Debug.Log (string.Format ("图集中没有 {0} 这个图片", spriteName));
					return null;
				}
			}
		default:
			return null;
		}
	}
}

public enum AtlasType
{
	UIAtlas,
}
