using Controller;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerJoinCashierQueueState : CustomerInteractionState
{
    CustomerInteractionContext m_context;
    public CustomerJoinCashierQueueState(CustomerInteractionContext context, CustomerStateMachine.CustomerInteractionState estate) : base(context, estate)
    {
        m_context = context;
    }

    private Cashier m_tempCashier;

    public override void EnterState()
    {
        m_context.Controller.ToogleTooltipCashier(true);
        //find cashier
        m_tempCashier = GameManager.instance.GetCashier();
        //reserve place cashier
        m_tempCashier.AddCustomer(m_context.Controller);
        //move to queue pos
        m_context.Controller.MoveToward(m_tempCashier.GetEmptyPosition());
        //wait for player to process transaction
    }

    public override void ExitState()
    {
    }

    public override CustomerStateMachine.CustomerInteractionState GetNextState()
    {
        if (m_context.Controller.ReachDestinationCheck())
        {
            if (m_tempCashier.GetNextCustomer() == m_context.Controller)
            {
                m_context.Controller.LookAt(m_tempCashier.transform.position);
            }
            else
            {
                m_context.Controller.LookAt(m_tempCashier.GetNextCustomer().transform.position);
            }
            m_context.Controller.BackToIdle();
            return CustomerStateMachine.CustomerInteractionState.WaitForTransaction;
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
