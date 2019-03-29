using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;


/// <summary>
///资源加载 
/// </summary>
public class ResourcesManager : MonoSingletonBase<ResourcesManager>
{
    private SpawnPool animalPool;
    private SpawnPool uiItemPool;
    public override void Init()
    {
        this.animalPool = PoolManager.Pools["AnimalPool"];
        this.uiItemPool = PoolManager.Pools["UIItemPool"];
    }


    public GameObject LoadUIPanel(string name)
    {
        string path = "UI/Panel/" + name;

        return Load(path, name);
    }

    public GameObject LoadUIItem(string name)
    {
        string path = "UI/Item/" + name;

        return Load(path, name);
    }

    public GameObject Load(string path, string name)
    {
        GameObject prefab = Resources.Load<GameObject>(path) as GameObject;
        if (prefab == null)
        {
            Debug.LogWarning(path + " load fail");
            return null;
        }
        GameObject prefabClone = Instantiate(prefab);
        prefabClone.name = name;
        prefab = null;
        return prefabClone;
    }

    public Material LoadMaterial(string path)
    {
        Material mat = Resources.Load<Material>(path) as Material;
        if (mat == null)
        {
            Debug.LogWarning(path + " load fail");
            return null;
        }
        return mat;
    }
}
