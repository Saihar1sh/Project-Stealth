using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolManager 
{
    private static Dictionary<int,List<PoolableObject>> pools = new Dictionary<int, List<PoolableObject>>();
    
    public static T GetPoolableObject<T>(T poolablePrefab, Vector3 position, Quaternion rotation) where T : PoolableObject
    {
        int objHash = poolablePrefab.name.GetHashCode();
        PoolableObject usablePoolableObject;
        if(!pools.ContainsKey(objHash))
        {
            pools.Add(objHash,new List<PoolableObject>());
        }

        pools.TryGetValue(objHash, out var poolList);
        var poolObj = poolList.Find(_poolObj => _poolObj.isInUse == false);
        if (poolObj != null)
        {
            usablePoolableObject = CreatePoolableObject<T>(poolablePrefab,position,rotation);
            usablePoolableObject.isInUse = true;
        }
        else
        {
            usablePoolableObject = poolObj;
        }

        return usablePoolableObject as T;
    }
    public static void PoolObjects<T>(T poolablePrefab) where T : PoolableObject
    {
        int objHash = poolablePrefab.name.GetHashCode();
        pools.TryGetValue(objHash, out var poolList);
        poolList.ForEach(obj=>PoolObject(obj));
    }

    private static void PoolObject(PoolableObject poolable)
    {
        if(!poolable.InitialInit)
        {
            poolable.Init();
        }

        poolable.gameObject.SetActive(false);
    }

    private static PoolableObject CreatePoolableObject<T>(T poolablePrefab,Vector3 position, Quaternion rotation) where T : PoolableObject
    {
        return GameObject.Instantiate<T>(poolablePrefab);
    }
}
