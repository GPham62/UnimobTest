using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(menuName = "Config/FruitConfig", fileName = "FruitConfig")]
    public class FruitConfig : ScriptableObject
    {
        public string fruitName;
        public float fruitPrice;
    }
}

