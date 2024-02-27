using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class CustomerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject m_customerPrefab;
        [SerializeField] private Transform m_spawnTrans;
        [SerializeField] private Transform m_despawnTrans;
        [HorizontalGroup] [SerializeField] private float m_spawnIntervalMin;
        [HorizontalGroup] [SerializeField] private float m_spawnIntervalMax;
        [SerializeField] private int m_maxCustomers = 4;
        [SerializeField] private bool m_isRunning;
        private float m_spawnInterval;
        private float m_cd = 0;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(GameManager.instance.delayStartCustomerSpawnTime);
            m_isRunning = true;
            ResetTimer();
        }

        private void Update()
        {
            if (!m_isRunning) return;
            if (m_cd > m_spawnInterval)
            {
                ResetTimer();
                SpawnCustomer();
            }
            m_cd += Time.deltaTime;
        }

        private void ResetTimer()
        {
            m_cd = 0;
            m_spawnInterval = Random.Range(m_spawnIntervalMin, m_spawnIntervalMax);
        }

        public void TooglePause() => m_isRunning = !m_isRunning;

        public void SpawnCustomer()
        {
            if (GameManager.instance.totalCustomers >= m_maxCustomers) return;
            GameObject customer = ObjectPoolManager.SpawnObject(m_customerPrefab, m_spawnTrans.position, Quaternion.identity);
            GameManager.instance.totalCustomers++;
        }
    }

}
