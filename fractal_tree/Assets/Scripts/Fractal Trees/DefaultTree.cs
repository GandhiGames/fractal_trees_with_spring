using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
    public class DefaultTree : Tree
    {
        private int m_GrowthCount;
        private float m_InitialLength;
        private float m_Angle;
        private GameObject m_BranchPrefab;
        private Transform m_Owner;
        private float[] m_Angles;
        private bool m_IsStatic;
        private float m_Thickness;

        public DefaultTree(int growth, float initialLength, 
            float lengthDegredation, float angle, float thickness,
            GameObject branchPrefab, Transform owner)
        {
    
            Branch.LengthDegradation = lengthDegredation;

            m_GrowthCount = growth;
            m_InitialLength = initialLength;
            m_Angle = angle;
            m_BranchPrefab = branchPrefab;
            m_Owner = owner;
            m_Thickness = thickness;

            m_Angles = new float[2];
            m_Angles[0] = m_Angle;
            m_Angles[1] = -m_Angle;
        }

        public List<Branch> Generate()
        {
            var branches = new List<Branch>();

            var branch = ((GameObject)MonoBehaviour.Instantiate(m_BranchPrefab)).GetComponent<Branch>();

            Vector2 start = m_Owner.transform.position;
            Vector2 end = (Vector2) m_Owner.transform.position + (Vector2.up * m_InitialLength);
            branch.Setup(start, end, m_Thickness, Color.white);
            branches.Add(branch);

            branch.transform.SetParent(m_Owner);

            for (int i = 0; i < m_GrowthCount; i++)
            {
                for (int j = branches.Count - 1; j >= 0; j--)
                {
                    if (!branches[j].hasBranched)
                    {
                        foreach (var ang in m_Angles)
                        {
                            var newBranch = branches[j].DoBranching(ang);
                            newBranch.transform.SetParent(m_Owner);
                            branches.Add(newBranch);
                        }

                        branches[j].hasBranched = true;
                    }
                }
            }

            return branches;
        }
    }
}