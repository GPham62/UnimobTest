using Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] protected FruitConfig m_config;

        public float GetFruitPrice() => m_config.fruitPrice;
    }
}
