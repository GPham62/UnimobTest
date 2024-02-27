using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerWaitForTransactionState : CustomerInteractionState
{
    CustomerInteractionContext m_context;
    public CustomerWaitForTransactionState(CustomerInteractionContext context, CustomerStateMachine.CustomerInteractionState estate) : base(context, estate)
    {
        m_context = context;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override CustomerStateMachine.CustomerInteractionState GetNextState()
    {
        if (m_context.Controller.isHappy)
        {
            m_context.Controller.ToogleTooltipCashier(false);
            m_context.Controller.ToogleTooltipHappy(true);
            return CustomerStateMachine.CustomerInteractionState.Exit;
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
