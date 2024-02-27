using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStateMachine : StateManager<CustomerStateMachine.CustomerInteractionState>
{
    public enum CustomerInteractionState
    {
        BuyTomato,
        WaitForTomato,
        JoinCashierQueue,
        WaitForTransaction,
        Exit
    }

    private CustomerInteractionContext m_context;
    [SerializeField] private CustomerController m_customerController;

    private void Awake()
    {
        m_context = new CustomerInteractionContext(m_customerController);
        InitStates();
    }

    private void OnEnable()
    {
        CurrentState = States[CustomerInteractionState.BuyTomato];
        CurrentState.EnterState();
    }

    private void InitStates()
    {
        States.Add(CustomerInteractionState.BuyTomato, new CustomerBuyTomatoState(m_context, CustomerInteractionState.BuyTomato));
        States.Add(CustomerInteractionState.WaitForTomato, new CustomerWaitTomatoState(m_context, CustomerInteractionState.WaitForTomato));
        States.Add(CustomerInteractionState.JoinCashierQueue, new CustomerJoinCashierQueueState(m_context, CustomerInteractionState.JoinCashierQueue));
        States.Add(CustomerInteractionState.WaitForTransaction, new CustomerWaitForTransactionState(m_context, CustomerInteractionState.WaitForTransaction));
        States.Add(CustomerInteractionState.Exit, new CustomerExitState(m_context, CustomerInteractionState.Exit));
        CurrentState = States[CustomerInteractionState.BuyTomato];
    }
}
