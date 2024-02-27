using Controller;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Cashier : Building
    {
        [FoldoutGroup("Cashier Info")]
        [SerializeField] private List<Transform> m_customerTransList;
        [SerializeField] private int m_moneyInCashier;
        [FoldoutGroup("Money Grid")]
        [SerializeField] private Transform m_moneyGridHolder;
        [FoldoutGroup("Money Grid")]
        [SerializeField] private GameObject m_gridItemPrefab;
        [FoldoutGroup("Money Grid")]
        [SerializeField] private int m_totalItemInRow = 16;
        [FoldoutGroup("Money Grid")]
        [SerializeField] private GameObject m_moneyPrefab;
        [FoldoutGroup("Money Grid")]
        [SerializeField] private List<GameObject> m_moneyList;
        [FoldoutGroup("Box")]
        [SerializeField] private GameObject m_boxPrefab;
        [FoldoutGroup("Box")]
        [SerializeField] private Transform m_boxSpawnTrans;
        private List<CustomerController> m_customers;
        private bool m_isHandlingCustomer, m_isCollectingMoney;
        private void OnEnable()
        {
            m_customers = new List<CustomerController>();
            m_isHandlingCustomer = false;
            m_moneyInCashier = 0;
        }

        public Vector3 GetEmptyPosition() => m_customerTransList[m_customers.Count - 1].position;

        public void AddCustomer(CustomerController customer)
        {
            m_customers.Add(customer);
        }

        public CustomerController GetNextCustomer() => m_customers[0];

        private Coroutine m_collectMoneyRoutine;
        public override void OnInteractWithPlayer(PlayerController player)
        {
            base.OnInteractWithPlayer(player);
            m_collectMoneyRoutine = StartCoroutine(CollectMoney(player));
            HandleCustomers();
        }

        private IEnumerator CollectMoney(PlayerController player)
        {
            while (m_moneyInCashier > 0)
            {
                GameObject money = m_moneyList[0];
                m_moneyInCashier--;
                money.transform.DOMove(player.transform.position, 0.3f).OnComplete(() =>
                {
                    ObjectPoolManager.DespawnObject(money);
                    GameManager.instance.onCollectMoney.Raise(1);
                });
                m_moneyList.RemoveAt(0);
                yield return new WaitForSeconds(0.15f);
            }
        }

        private void HandleCustomers()
        {
            if (m_isHandlingCustomer) return;
            if (m_customers.Count < 1) return;
            m_isHandlingCustomer = true;
            //process transaction
            CustomerController customer = GetNextCustomer();
            //show box
            GameObject newBox = ObjectPoolManager.SpawnObject(m_boxPrefab, m_boxSpawnTrans.position, Quaternion.identity, null, Vector3.zero);
            FruitBox fruitBoxComp = newBox.GetComponent<FruitBox>();
            customer.fruitBox = fruitBoxComp;
            newBox.transform.DOScale(fruitBoxComp.initScale, 0.3f).OnComplete(() =>
            {
                int fruitCount = 0;
                float fruitPrice = customer.GetLastFruit().GetComponent<Fruit>().GetFruitPrice();
                //put tomatoes in box
                Sequence sequence = DOTween.Sequence();
                while (customer.GetTotalFruitHold() > 0)
                {
                    fruitCount++;
                    GameObject lastTomato = customer.GetLastFruit();
                    customer.RemoveFromFruitList(lastTomato);
                    fruitBoxComp.AddToFruitList(lastTomato);
                    Transform targetTrans = fruitBoxComp.GetFruitPlace();
                    lastTomato.transform.SetParent(targetTrans);
                    sequence.Append(lastTomato.transform.DOMove(targetTrans.position, 0.3f));
                }
                //pay money
                int totalMoneyInCashier = m_moneyInCashier + fruitCount * (int)fruitPrice;
                if (totalMoneyInCashier > m_moneyGridHolder.childCount)
                {
                    //spawn enough grid to contain money
                    for (int i = m_moneyGridHolder.childCount; i < totalMoneyInCashier + m_totalItemInRow - (totalMoneyInCashier % 16); i++)
                    {
                        Instantiate(m_gridItemPrefab, m_moneyGridHolder);
                    }
                }

                //put box in customer hand
                sequence.AppendCallback(() => {
                    newBox.transform.SetParent(customer.GetFruitHolder());
                    newBox.transform.DOLocalMove(Vector3.zero, 0.3f);
                });

                for (int i = m_moneyInCashier; i < totalMoneyInCashier; i++)
                {
                    GameObject newMoney = ObjectPoolManager.SpawnObject(m_moneyPrefab, customer.transform.position,Quaternion.Euler(-90, 0, 0));
                    m_moneyList.Add(newMoney);
                    newMoney.transform.SetParent(m_moneyGridHolder.GetChild(i));
                    sequence.Append(newMoney.transform.DOLocalMove(Vector3.zero, 0.3f));
                }
                m_moneyInCashier = totalMoneyInCashier;
                sequence.OnComplete(() => {
                    customer.isHappy = true;
                });

                //process next customer
                ProcessNextCustomer();
                m_isHandlingCustomer = false;
            });
        }

        public void ProcessNextCustomer()
        {
            m_customers.RemoveAt(0);
            if (m_customers.Count > 0)
            {
                for (int i = 0; i < m_customers.Count; i++)
                {
                    m_customers[i].MoveToward(m_customerTransList[i].position);
                }
            }
        }
        public override void OnPlayerExit()
        {
            base.OnPlayerExit();
            if (m_collectMoneyRoutine != null)
                StopCoroutine(m_collectMoneyRoutine);
        }
    }
}