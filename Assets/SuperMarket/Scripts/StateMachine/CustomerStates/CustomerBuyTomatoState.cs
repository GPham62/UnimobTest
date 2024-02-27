using Controller;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Model.TomatoTable;

public class CustomerBuyTomatoState : CustomerInteractionState
{
    CustomerInteractionContext m_context;
    public CustomerBuyTomatoState(CustomerInteractionContext context, CustomerStateMachine.CustomerInteractionState estate) : base (context, estate)
    {
        m_context = context;
    }

    private Vector3 m_targetPos;
    private TomatoTableQueueInfo m_tomatoTableQueueInfo;

    public override void EnterState()
    {
        SetupTomatoToBuy();
        LookForEmptyTomatoQueue();
        ReservePlaceInTomatoTable();
        MoveTowardReservedPlace();
    }

    private void SetupTomatoToBuy()
    {
        m_context.Controller.SetRequiredTomato(UnityEngine.Random.Range(2, 5));
        m_context.Controller.ToogleTooltipTomato(true);
        m_context.tomatoTable = null;
    }

    private void LookForEmptyTomatoQueue()
    {
        TomatoTable TomatoTable = GameManager.instance.GetTomatoTable();
        if (TomatoTable == null)
        {
            Debug.LogWarning("No available table");
        }
        else
        {
            m_context.tomatoTable = TomatoTable;
            TomatoTableQueueInfo tomatoTableQueueInfo = TomatoTable.GetAvailableQueueInfo();
            if (tomatoTableQueueInfo == null)
            {
                Debug.LogWarning("Table should have available queue slot");
            }
            else
            {
                m_tomatoTableQueueInfo = tomatoTableQueueInfo;
            }
        }
    }

    private void ReservePlaceInTomatoTable()
    {
        m_tomatoTableQueueInfo.isEmpty = false;
        m_context.tomatoTable.FullCustomerCheck();
    }

    private void MoveTowardReservedPlace()
    {
        m_targetPos = m_tomatoTableQueueInfo.queuePos.position;
        m_context.Controller.MoveToward(m_targetPos);
    }

    public override void ExitState()
    {
        m_context.Controller.LookAt(m_context.tomatoTable.transform.position);
        m_context.tomatoTable.AssignCustomerToQueueInfo(m_tomatoTableQueueInfo, m_context.Controller.gameObject);
        m_context.queueInfo = m_tomatoTableQueueInfo;
        if (!m_context.tomatoTable.isDistributingFruits)
            m_context.tomatoTable.isDistributingFruits = true;
    }

    public override CustomerStateMachine.CustomerInteractionState GetNextState()
    {
        if (m_context.Controller.ReachDestinationCheck())
        {
            m_context.Controller.BackToIdle();
            return CustomerStateMachine.CustomerInteractionState.WaitForTomato;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
    }

    public override void OnTriggerExit(Collider other)
    {

    }

    public override void OnTriggerStay(Collider other)
    {
    }

    public override void UpdateState()
    {
    }
}
