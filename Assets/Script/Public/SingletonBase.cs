using UnityEngine;
using System.Collections;

/// <summary>
/// <para>纯粹的单例模式</para>
/// <para>可以用于读取游戏文件等等的情况.</para>
/// <para>同时可参考： <see cref="MonoSingletonBase"/> 这个类</para>
/// <para>By 浩深.</para>
/// </summary>
public abstract class SingletonBase<T> where T : new()
{
	private static T instance;
	
	public static T Instance
	{
		get
		{
			if(instance == null)
				instance = new T();
				
			return instance;
		}
	}

    public virtual void Init(){
    }
}
