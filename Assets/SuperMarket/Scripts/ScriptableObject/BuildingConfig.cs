using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(menuName = "Config/BuildingConfig", fileName = "BuildingConfig")]
    public class BuildingConfig : ScriptableObject
    {
        public string buildingName;
        public float buildMoney;
        public GameObject buildingPrefab;
    }

}
