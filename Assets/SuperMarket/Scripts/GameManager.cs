using DG.Tweening;
using Model;
using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Controller
{
    public class GameManager : SingletonMono<GameManager>
    {
        public int totalCustomers;
        public float delayStartCustomerSpawnTime;
        public List<TomatoTable> m_tomatoTables = new List<TomatoTable>();
        public List<Cashier> m_cashiers = new List<Cashier>();
        [SerializeField] private FloatReference m_totalMoney = default(FloatReference);
        public FloatGameEvent onCollectMoney;
        public FloatGameEvent onSpendMoney;
        [SerializeField] private TextMeshProUGUI m_moneyText;
        [SerializeField] private Transform m_despawnTrans;
        [SerializeField] private NavMeshSurface m_surface;

        public void Start()
        {
            onCollectMoney.AddListener((float amount) => IncreaseMoney(amount));
            onSpendMoney.AddListener((float amount) => DecreaseMoney(amount));
            UpdateMoneyText();
        }

        private void OnDestroy()
        {
            onCollectMoney.RemoveListener((float amount) => IncreaseMoney(amount));
            onSpendMoney.RemoveListener((float amount) => DecreaseMoney(amount));
        }

        private void IncreaseMoney(float amount)
        {
            m_totalMoney.Value += amount;
            UpdateMoneyText();
        }

        private void DecreaseMoney(float amount)
        {
            if (m_totalMoney.Value < amount) return;
            m_totalMoney.Value -= amount;
            UpdateMoneyText();
        }
        private void UpdateMoneyText()
        {
            float currentMoney = float.Parse(m_moneyText.text);
            DOVirtual.Float(currentMoney, m_totalMoney.Value, 2f, onVirtualUpdate: (float m) => {
                currentMoney = m;
                m_moneyText.text = ((int)currentMoney).ToString();
            });
        }

        public TomatoTable GetTomatoTable()
        {
            var tableList = m_tomatoTables.Where(c => c.IsTableFullCustomers() == false).ToList();
            return tableList[Random.Range(0, tableList.Count)];
        }

        public Cashier GetCashier()
        {
            return m_cashiers[0];
        }

        public void AddToTomatoTableList(TomatoTable tmtCase)
        {
            m_tomatoTables.Add(tmtCase);
        }

        public void AddCashierToList(Cashier cashier)
        {
            m_cashiers.Add(cashier);
        }

        public Vector3 GetDespawnPos() => m_despawnTrans.position;

        public void Rebake() => m_surface.BuildNavMesh();
    }
}

