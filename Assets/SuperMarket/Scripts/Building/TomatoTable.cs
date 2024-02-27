using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Linq;

namespace Model
{
    public class TomatoTable : Building
    {
        [FoldoutGroup("Tomato Settings")]
        [SerializeField] private List<Transform> m_tomatoHolder;

        [FoldoutGroup("Tomato Settings")]
        [SerializeField] private List<GameObject> m_fruitList = new List<GameObject>();

        [FoldoutGroup("Work Settings")]
        [SerializeField] private bool m_isFullCustomer;

        [FoldoutGroup("Work Settings")]
        [SerializeField] private List<TomatoTableQueueInfo> m_tomatoTableQueueInfos;

        [Serializable]
        public class TomatoTableQueueInfo
        {
            public Transform queuePos;
            public GameObject customerInQueue;
            public bool isEmpty;

        }

        [FoldoutGroup("Work Settings")]
        public bool isDistributingFruits;

        [FoldoutGroup("Work Settings")]
        [SerializeField] private float m_distributeSpeed;

        private float m_distributeCD;

        private Coroutine m_placeFruitCoroutine;

        public override void OnInteractWithPlayer(PlayerController player)
        {
            base.OnInteractWithPlayer(player);
            m_placeFruitCoroutine = StartCoroutine(PlaceFruitOnTable(player));
        }

        private void Update()
        {
            if (!isDistributingFruits) return;
            DistributeFruits();
        }

        private void DistributeFruits()
        {
            if (m_fruitList.Count < 1) return;
            if (m_distributeCD > m_distributeSpeed)
            {
                m_distributeCD = 0;
                var infos = m_tomatoTableQueueInfos.Where(info => info.customerInQueue != null).ToList();
                if (infos.Count > 0)
                {
                    FruitPickup customer = infos[UnityEngine.Random.Range(0, infos.Count)].customerInQueue.GetComponent<FruitPickup>();
                    GameObject tomato = m_fruitList[m_fruitList.Count - 1];
                    m_fruitList.Remove(tomato);
                    tomato.transform.parent = customer.GetFruitHolder();
                    tomato.transform.DOLocalMove(customer.GetLastFruitLocalPosition(), 0.3f);
                    tomato.transform.SetAsFirstSibling();
                    customer.AddToFruitList(tomato);
                }
                else
                {
                    isDistributingFruits = false;
                }
            }
            m_distributeCD += Time.deltaTime;
        }

        private IEnumerator PlaceFruitOnTable(PlayerController player)
        {
            while (!IsTableFull() && player.GetTotalFruitHold() > 0)
            {
                AddTomatoToTable(player);
                yield return new WaitForSeconds(player.GetPickupSpeed());
            }
        }

        public override void OnPlayerExit()
        {
            base.OnPlayerExit();
            if (m_placeFruitCoroutine != null)
                StopCoroutine(m_placeFruitCoroutine);
        }

        public bool IsTableFull() => m_tomatoHolder.Count <= m_fruitList.Count;

        public bool IsTableFullCustomers() => m_isFullCustomer;

        private void AddTomatoToTable(PlayerController player)
        {
            GameObject playerLastFruit = player.GetLastFruit();
            player.RemoveFromFruitList(playerLastFruit);
            AddToFruitList(playerLastFruit);
            Vector3 targetPos = m_tomatoHolder[m_fruitList.Count - 1].position;
            playerLastFruit.transform.DOMove(targetPos, 0.3f);
            playerLastFruit.transform.SetParent(m_tomatoHolder[m_fruitList.Count - 1]);
        }
        public void AddToFruitList(GameObject fruit)
        {
            m_fruitList.Add(fruit);
        }

        public void RemoveFromFruitList(GameObject fruit)
        {
            if (m_fruitList.Contains(fruit))
            {
                m_fruitList.Remove(fruit);
            }
        }

        public TomatoTableQueueInfo GetAvailableQueueInfo()
        {
            return m_tomatoTableQueueInfos.Find(info => info.isEmpty);
        }

        public void AssignCustomerToQueueInfo(TomatoTableQueueInfo queueInfo, GameObject customer)
        {
            queueInfo.customerInQueue = customer;
        }

        public void FullCustomerCheck()
        {
            m_isFullCustomer = m_tomatoTableQueueInfos.Any() && m_tomatoTableQueueInfos.All(info => !info.isEmpty);
        }

        public void RemoveCustomerFromQueueInfo(TomatoTableQueueInfo queueInfo, GameObject customer)
        {
            queueInfo.customerInQueue = null;
            queueInfo.isEmpty = true;
            if (m_isFullCustomer)
                m_isFullCustomer = false;
        }
    }
}
