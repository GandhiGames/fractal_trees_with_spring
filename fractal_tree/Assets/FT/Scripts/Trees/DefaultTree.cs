using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	/// <summary>
	/// Spawns a fractal tree.
	/// </summary>
    public class DefaultTree : Tree
    {
        private int m_GrowthCount;
        private float m_InitialLength;
        private float m_Angle;
        private GameObject m_BranchPrefab;
        private Transform m_Owner;
        private float[] m_Angles;
        private float m_Thickness;
		private Color m_BranchColour;

		/// <summary>
		/// Initializes a new instance of the <see cref="FractalTree.DefaultTree"/> class.
		/// </summary>
		/// <param name="growth">Growth.</param>
		/// <param name="initialLength">Initial length.</param>
		/// <param name="lengthDegredation">Length degredation.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="thickness">Thickness.</param>
		/// <param name="branchPrefab">Branch prefab.</param>
		/// <param name="owner">Owner.</param>
        public DefaultTree(int growth, float initialLength,
            float lengthDegredation, float angle, float thickness,
			GameObject branchPrefab, Color branchColor, Transform owner)
        {
            StationaryBranch.LengthDegradation = lengthDegredation;
            MovingBranchImpl.LengthDegradation = lengthDegredation;

            m_GrowthCount = growth;
            m_InitialLength = initialLength;
            m_Angle = angle;
            m_BranchPrefab = branchPrefab;
            m_Owner = owner;
            m_Thickness = thickness;
			m_BranchColour = branchColor;

            m_Angles = new float[2];
            m_Angles[0] = m_Angle;
            m_Angles[1] = -m_Angle;
        }

		/// <summary>
		/// Generates a fractal tree.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
        public List<T> Generate<T>() where T : Branch
        {
            var branches = new List<T>();

            var branch = ((GameObject)MonoBehaviour.Instantiate(m_BranchPrefab)).GetComponent<T>();

            Vector2 start = m_Owner.transform.position;
            Vector2 end = (Vector2)m_Owner.transform.position + (Vector2.up * m_InitialLength);
			branch.Setup(start, end, m_Thickness, m_BranchColour);
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
                            var newBranch = branches[j].DoBranching<T>(ang);
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