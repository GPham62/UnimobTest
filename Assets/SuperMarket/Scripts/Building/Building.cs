using Controller;
using DG.Tweening;
using Scriptable;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Building : MonoBehaviour
    {
        [FoldoutGroup("Building")]
        [SerializeField] protected DOTweenAnimation m_animActive;
        [FoldoutGroup("Building")]
        [SerializeField] protected BuildingConfig m_config;

        public virtual void OnInteractWithPlayer(PlayerController player)
        {
            if (m_animActive)
                m_animActive.DORestart();
        }

        public virtual void OnPlayerExit()
        {
            if (m_animActive)
                m_animActive.DOPlayBackwards();
        }
    }
}