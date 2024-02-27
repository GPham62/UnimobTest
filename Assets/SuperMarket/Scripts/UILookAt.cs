using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAt : MonoBehaviour
{
    [SerializeField] private Transform m_lookAt;

    private void Start()
    {
        if (m_lookAt == null)
        {
            m_lookAt = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        if (m_lookAt == null) return;
        transform.LookAt(2 * transform.position - m_lookAt.position);
    }
}
