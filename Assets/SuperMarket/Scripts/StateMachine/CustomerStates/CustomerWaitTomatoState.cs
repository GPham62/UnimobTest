using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerWaitTomatoState : CustomerInteractionState
{
    CustomerInteractionContext m_context;
    public CustomerWaitTomatoState(CustomerInteractionContext context, CustomerStateMachine.CustomerInteractionState estate) : base(context, estate)
    {
        m_context = context;
    }
    public override void EnterState()
    {
    }

    public override void ExitState()
    {
        m_context.tomatoTable.RemoveCustomerFromQueueInfo(m_context.queueInfo, m_context.Controller.gameObject);
        m_context.queueInfo = null;
    }

    public override CustomerStateMachine.CustomerInteractionState GetNextState()
    {
        if (m_context.Controller.FullTomatoCheck())
        {
            return CustomerStateMachine.CustomerInteractionState.JoinCashierQueue;
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
