using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPickup : MonoBehaviour
{
    [FoldoutGroup("Tomato Handling Infos")]
    [SerializeField] private GameObject m_tomatoHolder;
    [FoldoutGroup("Tomato Handling Infos")]
    [SerializeField] private float m_tomatoPickupTime = 0.15f;
    [FoldoutGroup("Tomato Handling Infos")]
    [SerializeField] protected int m_maxFruitHold = 5;
    [FoldoutGroup("Tomato Handling Infos")]
    [SerializeField] private List<GameObject> m_fruitList;

    void Awake()
    {
        m_fruitList = new List<GameObject>();
    }
    public Transform GetFruitHolder() => m_tomatoHolder.transform;

    public Vector3 GetLastFruitLocalPosition()
    {
        if (GetTotalFruitHold() < 1)
        {
            return Vector3.zero;
        }
        else
        {
            Vector3 lastFoodPos = m_fruitList[0].transform.localPosition;
            return new Vector3(lastFoodPos.x, lastFoodPos.y + 0.17f, lastFoodPos.z);
        }
    }

    public int GetTotalFruitHold() => m_fruitList.Count;

    public int GetMaximumFruitHold() => m_maxFruitHold;

    public bool FullTomatoCheck() => GetTotalFruitHold() >= GetMaximumFruitHold();

    public float GetPickupSpeed() => m_tomatoPickupTime;

    public virtual void AddToFruitList(GameObject fruit)
    {
        m_fruitList.Insert(0, fruit);
    }

    public virtual void RemoveFromFruitList(GameObject fruit)
    {
        m_fruitList.Remove(fruit);
    }

    public GameObject GetLastFruit() => m_fruitList[0];

    public void ClearHolder()
    {
        foreach (Transform child in m_tomatoHolder.transform)
            ObjectPoolManager.DespawnObject(child.gameObject);
    }

}
