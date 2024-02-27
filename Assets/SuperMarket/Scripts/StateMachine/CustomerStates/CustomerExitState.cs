using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerExitState : CustomerInteractionState
{
    CustomerInteractionContext m_context;
    public CustomerExitState(CustomerInteractionContext context, CustomerStateMachine.CustomerInteractionState estate) : base(context, estate)
    {
        m_context = context;
    }

    public override void EnterState()
    {
        m_context.Controller.MoveToward(GameManager.instance.GetDespawnPos());
    }

    public override void ExitState()
    {
    }

    public override CustomerStateMachine.CustomerInteractionState GetNextState()
    {
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
        if (m_context.Controller.ReachDestinationCheck())
        {
            m_context.Controller.Despawn();
            GameManager.instance.totalCustomers--;
        }
    }
}
