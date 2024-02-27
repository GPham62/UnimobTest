using DG.Tweening;
using Model;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : FruitPickup
{
    [FoldoutGroup("Skins")]
    [SerializeField] private List<Material> m_mats;
    [FoldoutGroup("Components")]
    [SerializeField] private SkinnedMeshRenderer m_meshRenderer;
    [FoldoutGroup("Components")]
    [SerializeField] private Animator m_animator;
    [FoldoutGroup("Components")]
    [SerializeField] private Rigidbody m_rigidbody;
    [FoldoutGroup("Components")]
    [SerializeField] private CapsuleCollider m_collider;
    [FoldoutGroup("Components")]
    [SerializeField] private NavMeshAgent m_agent;
    [FoldoutGroup("Tooltip")]
    [SerializeField] private GameObject m_tooltipTomato;

    [FoldoutGroup("Tooltip")]
    [SerializeField] private TextMeshProUGUI m_tooltipTomatoText;

    [FoldoutGroup("Tooltip")]
    [SerializeField] private GameObject m_tooltipCashier;

    [FoldoutGroup("Tooltip")]
    [SerializeField] private GameObject m_tooltipHappy;

    public FruitBox fruitBox;

    public bool isHappy;

    private int isMoveAnim = Animator.StringToHash("IsMove");
    private int isEmptyAnim = Animator.StringToHash("IsEmpty");
    private int isCarryMoveAnim = Animator.StringToHash("IsCarryMove");

    private void OnEnable()
    {
        ToogleTooltipTomato(false);
        ToogleTooltipCashier(false);
        ToogleTooltipHappy(false);
        RandomSkin();
        isHappy = false;
        fruitBox = null;
    }

    public void RandomSkin()
    {
        m_meshRenderer.material = m_mats[UnityEngine.Random.Range(0, m_mats.Count)];
    }

    public void ToogleTooltipTomato(bool isEnable)
    {
        m_tooltipTomato.SetActive(isEnable);
    }

    public void SetRequiredTomato(int required)
    {
        m_maxFruitHold = required;
        UpdateTomatoText(0, m_maxFruitHold);
    }

    public void UpdateTomatoText(int amount, int required)
    {
        m_tooltipTomatoText.SetText(amount + " / " + required);
    }

    public void ToogleTooltipCashier(bool isEnable)
    {
        m_tooltipCashier.SetActive(isEnable);
    }

    public void ToogleTooltipHappy(bool isEnable)
    {
        m_tooltipHappy.SetActive(isEnable);
    }

    public void MoveToward(Vector3 target)
    {
        m_animator.SetBool(isMoveAnim, true);
        m_animator.SetBool(isCarryMoveAnim, true);
        target.y = 0;
        m_agent.SetDestination(target);
    }
    public bool ReachDestinationCheck()
    {
        return (m_agent.remainingDistance < 0.1f);
    }

    public void LookAt(Vector3 destination)
    {
        transform.DOLookAt(destination, 0.2f, AxisConstraint.Y);
    }

    public void BackToIdle()
    {
        m_animator.SetBool(isMoveAnim, false);
        m_animator.SetBool(isCarryMoveAnim, false);
    }

    public override void AddToFruitList(GameObject fruit)
    {
        base.AddToFruitList(fruit);
        if (m_animator.GetBool(isEmptyAnim))
        {
            m_animator.SetBool(isEmptyAnim, false);
        }

        if (FullTomatoCheck())
            m_tooltipTomato.SetActive(false);
        else
        {
            UpdateTomatoText(GetTotalFruitHold(), m_maxFruitHold);
        }
    }

    public override void RemoveFromFruitList(GameObject fruit)
    {
        base.RemoveFromFruitList(fruit);
        if (GetTotalFruitHold() < 1 && !m_animator.GetBool(isEmptyAnim))
        {
            m_animator.SetBool(isEmptyAnim, true);
        }
    }

    public void Despawn()
    {
        fruitBox.DespawnFruits();
        ObjectPoolManager.DespawnObject(fruitBox.gameObject);
        ObjectPoolManager.DespawnObject(gameObject);
    }
}
