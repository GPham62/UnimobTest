using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();
    private static GameObject m_objectPoolEmptyHolder;


    private void Awake()
    {
        m_objectPoolEmptyHolder = new GameObject("PooledObjects");
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, Transform parent = null, Vector3? scale = null)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation, parent != null ? parent : m_objectPoolEmptyHolder.transform);
            if (scale != null)
                spawnableObj.transform.localScale = (Vector3)scale;
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            if (scale != null)
                spawnableObj.transform.localScale = (Vector3)scale;
            spawnableObj.transform.SetParent(parent != null ? parent : m_objectPoolEmptyHolder.transform);
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }

    public static void DespawnObject(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7); //Remove (Clone) name
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release object that's not pooled: " + obj.name);
            Destroy(obj);
        }
        else
        {
            obj.SetActive(false);
            obj.transform.SetParent(m_objectPoolEmptyHolder.transform);
            pool.InactiveObjects.Add(obj);
        }
    }
}
