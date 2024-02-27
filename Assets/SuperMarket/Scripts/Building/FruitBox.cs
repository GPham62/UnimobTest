using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBox : MonoBehaviour
{
    [SerializeField] List<Transform> fruitPlaceTransList;
    [SerializeField] List<GameObject> fruits;
    public Vector3 initScale;
    public int placeIndex;
    private void OnEnable()
    {
        placeIndex = 0;
        fruits = new List<GameObject>();
    }

    public Transform GetFruitPlace()
    {
        return fruitPlaceTransList[placeIndex++];
    }

    public void AddToFruitList(GameObject fruit) => fruits.Add(fruit);

    public void DespawnFruits()
    {
        foreach (GameObject fruit in fruits)
            ObjectPoolManager.DespawnObject(fruit);
    }
}
