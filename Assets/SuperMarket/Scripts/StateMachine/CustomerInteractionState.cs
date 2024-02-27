using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomerInteractionState : BaseState<CustomerStateMachine.CustomerInteractionState>
{
    protected CustomerInteractionContext Context;
    public CustomerInteractionState(CustomerInteractionContext context, CustomerStateMachine.CustomerInteractionState stateKey) : base(stateKey)
    {
        Context = context;
    }
}
