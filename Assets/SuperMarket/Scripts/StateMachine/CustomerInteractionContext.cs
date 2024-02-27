using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteractionContext
{
    private CustomerController m_controller;
    public TomatoTable tomatoTable;
    public TomatoTable.TomatoTableQueueInfo queueInfo;
    public CustomerInteractionContext(CustomerController controller)
    {
        m_controller = controller;
    }

    public CustomerController Controller => m_controller;
}
