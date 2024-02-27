using Controller;
using DG.Tweening;
using ScriptableObjectArchitecture;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Model
{
    public class SellingSite : Building
    {
        [SerializeField] private TextMeshProUGUI m_moneyText;
        [SerializeField] private FloatGameEvent m_onMoneySpend;
        [SerializeField] private FloatReference m_playerMoney;
        [SerializeField] private float m_spendSpeed = 0.3f;
        [SerializeField] private GameObject m_moneyPrefab;
        [SerializeField] private Transform m_spawnPos;

        private float m_moneyRequired;
        private void Start()
        {
            m_moneyRequired = m_config.buildMoney;
            m_moneyText.text = m_moneyRequired.ToString();
        }

        private Coroutine m_spendMoneyCoroutine;
        public override void OnInteractWithPlayer(PlayerController player)
        {
            base.OnInteractWithPlayer(player);
            if (m_playerMoney.Value < 1) return;
            m_spendMoneyCoroutine = StartCoroutine(SpendMoney(player.gameObject.transform));
        }

        public override void OnPlayerExit()
        {
            base.OnPlayerExit();
            if (m_spendMoneyCoroutine != null)
                StopCoroutine(m_spendMoneyCoroutine);
        }

        private IEnumerator SpendMoney(Transform playerTrans)
        {
            while (m_moneyRequired > 0)
            {
                m_onMoneySpend.Raise(1);
                m_moneyRequired--;
                m_moneyText.text = m_moneyRequired.ToString();
                GameObject newMoney = ObjectPoolManager.SpawnObject(m_moneyPrefab, playerTrans.position, Quaternion.identity);
                newMoney.transform.DOMove(m_moneyText.transform.position, 0.3f).OnComplete(() => ObjectPoolManager.DespawnObject(newMoney));
                yield return new WaitForSeconds(m_spendSpeed);
            }
            BuildSite();
            ObjectPoolManager.DespawnObject(gameObject);
        }

        private void BuildSite()
        {
            Building newBuilding = ObjectPoolManager.SpawnObject(m_config.buildingPrefab, m_spawnPos.position, Quaternion.identity).GetComponent<Building>();
            GameManager.instance.Rebake();
            if (newBuilding is TomatoTable)
                GameManager.instance.AddToTomatoTableList((TomatoTable)newBuilding);
        }
    }

}
