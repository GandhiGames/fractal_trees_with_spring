using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
    public class DemoController : MonoBehaviour
    {
        public MovingTreeBuilder tree;

        public enum ForceType
        {
            Directed,
            Push,
            Pull
        }
        public ForceType forceType = ForceType.Push;

        private Dictionary<ForceType, Action> m_ForceActions = new Dictionary<ForceType, Action>();

        // Use this for initialization
        void Start()
        {
            m_ForceActions.Add(ForceType.Directed, ApplyDirectedForce);
            m_ForceActions.Add(ForceType.Push, ApplyPushForce);
            m_ForceActions.Add(ForceType.Pull, ApplyPullForce);
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                m_ForceActions[forceType]();
            }
        }

        private void ApplyDirectedForce()
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tree.ApplyDirectedForce(Vector2.down, pos, 5f);
        }

        private void ApplyPushForce()
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tree.ApplyPushForce(1f, pos, 20f);
        }

        private void ApplyPullForce()
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tree.ApplyPullForce(0.4f, pos, 5f);
        }
    }
}