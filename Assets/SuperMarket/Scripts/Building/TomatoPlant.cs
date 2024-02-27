using Controller;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class TomatoPlant : Building
    {
        [SerializeField] private DOTweenAnimation m_animWorking;
        [SerializeField] private int m_spawnMaximumAmount = 3;
        [SerializeField] private int m_spawnTotal = 0;
        [SerializeField] private float m_spawnInterval = 1f;
        [SerializeField] private float m_cd;
        [SerializeField] private List<Transform> m_tomatoSpawnLocations;
        [SerializeField] private GameObject m_tomatoPrefab;

        private void Start()
        {
            m_cd = 0f;
        }

        private void Update()
        {
            if (m_spawnTotal >= m_spawnMaximumAmount)
            {
                m_animWorking.DORestart();
            }
            else
            {
                SpawnTomatoes();
            }
        }

        private void SpawnTomatoes()
        {
            m_animWorking.DOPlay();
            if (m_cd < m_spawnInterval)
            {
                m_cd += Time.deltaTime;
            }
            else
            {
                m_cd = 0f;
                GameObject tomatoe = ObjectPoolManager.SpawnObject(m_tomatoPrefab, m_tomatoSpawnLocations[m_spawnTotal].transform.position, Quaternion.identity, m_tomatoSpawnLocations[m_spawnTotal], Vector3.zero);
                tomatoe.transform.DOScale(1f, 0.3f).SetAutoKill(true);
                m_spawnTotal++;
            }
        }

        public void Harvest(PlayerController player)
        {
            if (m_spawnTotal < 1) return;
            GameObject tomato = m_tomatoSpawnLocations[m_spawnTotal - 1].GetChild(0).gameObject;
            Vector3 fruitPos = player.GetLastFruitLocalPosition();
            player.AddToFruitList(tomato);
            tomato.transform.SetParent(player.GetFruitHolder());
            tomato.transform.DOLocalMove(fruitPos, 0.3f);
            tomato.transform.SetAsFirstSibling();
            m_spawnTotal--;
        }

        private Coroutine m_harvestRoutine;

        public override void OnInteractWithPlayer(PlayerController player)
        {
            base.OnInteractWithPlayer(player);
            m_harvestRoutine = StartCoroutine(HarvestFruit(player));
        }

        public override void OnPlayerExit()
        {
            base.OnPlayerExit();
            if (m_harvestRoutine != null)
                StopCoroutine(m_harvestRoutine);
        }

        private IEnumerator HarvestFruit(PlayerController player)
        {
            while (!player.FullTomatoCheck())
            {
                Harvest(player);
                yield return new WaitForSeconds(player.GetPickupSpeed());
            }
        }
    }

}