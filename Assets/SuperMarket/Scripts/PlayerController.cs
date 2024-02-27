using DG.Tweening;
using Model;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class PlayerController : FruitPickup
    {
        [FoldoutGroup("Control Infos")]
        [SerializeField] private Joystick m_joystick;
        [FoldoutGroup("Control Infos")]
        [SerializeField] private float m_movespeed;
        [FoldoutGroup("Control Infos")]
        [SerializeField] private Animator m_animator;
        [FoldoutGroup("Control Infos")]
        [SerializeField] private Rigidbody m_rigidboydy;

        [FoldoutGroup("Tomato Handling Infos")]
        [SerializeField] private GameObject m_maxFruitSign;

        private float m_interpolation = 10f;
        private float m_currentV = 0;
        private float m_currentH = 0;
        private Vector3 m_currentDirection = Vector3.zero;
        private int isMoveAnim = Animator.StringToHash("IsMove");
        private int isEmptyAnim = Animator.StringToHash("IsEmpty");
        private int isCarryMoveAnim = Animator.StringToHash("IsCarryMove");

        private void Awake()
        {
            if (!m_animator)
                gameObject.GetComponent<Animator>();
            if (!m_rigidboydy)
                gameObject.GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            float v = m_joystick.Vertical;
            float h = m_joystick.Horizontal;

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);
            Transform camera = Camera.main.transform;
            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;
            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

                transform.rotation = Quaternion.LookRotation(m_currentDirection);
                transform.position += m_currentDirection * m_movespeed * Time.deltaTime;
            }

            m_animator.SetBool(isMoveAnim, direction.magnitude > 0.5f ? true : false);

            m_animator.SetBool(isCarryMoveAnim, direction.magnitude > 0.5f ? true : false);
        }

       
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Building")
            {
                other.GetComponent<Building>().OnInteractWithPlayer(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Building")
            {
                other.GetComponent<Building>().OnPlayerExit();
            }
        }

        public override void AddToFruitList(GameObject fruit)
        {
            base.AddToFruitList(fruit);
            if (m_animator.GetBool(isEmptyAnim))
            {
                m_animator.SetBool(isEmptyAnim, false);
            }
            if (FullTomatoCheck())
            {
                if (!m_maxFruitSign.activeSelf)
                    m_maxFruitSign.SetActive(true);
            }
        }

        public override void RemoveFromFruitList(GameObject fruit)
        {
            base.RemoveFromFruitList(fruit);
            if (GetTotalFruitHold() < 1 && !m_animator.GetBool(isEmptyAnim))
            {
                m_animator.SetBool(isEmptyAnim, true);
            }
            if (m_maxFruitSign.activeSelf)
                m_maxFruitSign.SetActive(false);
        }
    }
}