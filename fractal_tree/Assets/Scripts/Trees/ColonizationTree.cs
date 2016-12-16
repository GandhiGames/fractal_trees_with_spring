using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	/// <summary>
	/// Spawns a fractal tree using space colonization: http://algorithmicbotany.org/papers/colonization.egwnp2007.pdf
	/// </summary>
	public class ColonizationTree : Tree
	{
		private static readonly int TRUNK_MAX_ATTEMPTS = 200;
		private static readonly int GROWTH_MAX_ATTEMPTS = 200;
		
		private List<ColonizationLeaf> m_Leaves;
		private Transform m_Owner;
		private float m_InitialLength;
		private GameObject m_BranchPrefab;
		private float m_Width;
		private float m_MinDistance;
		private float m_MaxDistance;
		private Color m_BranchColor;

		/// <summary>
		/// Initializes a new instance of the <see cref="FractalTree.ColonizationTree"/> class.
		/// </summary>
		/// <param name="leaves">Leaves.</param>
		/// <param name="owner">Owner.</param>
		/// <param name="initialLength">Initial length.</param>
		/// <param name="branchPrefab">Branch prefab.</param>
		/// <param name="width">Width.</param>
		/// <param name="minDistance">Minimum distance.</param>
		/// <param name="maxDistance">Max distance.</param>
		public ColonizationTree (List<ColonizationLeaf> leaves, 
		                         Transform owner, float initialLength, 
		                         GameObject branchPrefab, 
			float width, float minDistance, float maxDistance, Color branchColor)
		{
			m_Leaves = leaves;
			m_Owner = owner;
			m_InitialLength = initialLength;
			m_BranchPrefab = branchPrefab;
			m_Width = width;
			m_MinDistance = minDistance;
			m_MaxDistance = maxDistance;
			m_BranchColor = branchColor;

		}

		/// <summary>
		/// Generates a tree using space colonization.
		/// </summary>
		/// <typeparam name="T">Branch type.</typeparam>
		public List<T> Generate<T> () where T : Branch
		{
			var branches = new List<T> ();

			var root = CreateRoot<T> ();
			branches.Add (root);

			branches.AddRange (CreateTrunk (root));
		
			int growthAttempts = 0;

			while (growthAttempts++ <= GROWTH_MAX_ATTEMPTS) {
				bool stillGrowing = Grow (ref branches);

				if (!stillGrowing) {
					break;
				}
			}

			return branches;
		}

		private T CreateRoot<T> () where T : Branch
		{
			Vector2 start = m_Owner.transform.position;
			Vector2 end = (Vector2)m_Owner.transform.position + (Vector2.up * m_InitialLength);

			return CreateBranch<T> (start, end);
		}

		private T CreateBranch<T> (T previous, Vector2 end) where T : Branch
		{
			var branch = ((GameObject)MonoBehaviour.Instantiate (m_BranchPrefab)).GetComponent<T> ();
			branch.Setup (previous, end, m_Width, m_BranchColor);
			branch.transform.SetParent (m_Owner);
			return branch;
		}

		private T CreateBranch<T> (Vector2 start, Vector2 end) where T : Branch
		{
			var branch = ((GameObject)MonoBehaviour.Instantiate (m_BranchPrefab)).GetComponent<T> ();
			branch.Setup (start, end, m_Width, m_BranchColor);
			branch.transform.SetParent (m_Owner);
			return branch;
		}

		private List<T> CreateTrunk<T> (T root) where T : Branch
		{
			var branches = new List<T> ();

			T current = root;

			bool found = false;

			int attempts = 0;

			while (!found && (attempts++ < TRUNK_MAX_ATTEMPTS)) {
				foreach (var leaf in m_Leaves) {
					var dist = Vector2.Distance (current.endPos, leaf.position);

					if (dist >= m_MinDistance && dist <= m_MaxDistance) {
						found = true;
					}
				}

				if (!found) {
					var dir = (current.endPos - current.startPos).normalized;

					current = CreateBranch (current, current.endPos + (dir * m_InitialLength));

					branches.Add (current);
				}
			}

			return branches;
		}

		private bool Grow<T> (ref List<T> branches) where T : Branch
		{
			bool growing = false;

			if (m_Leaves.Count == 0) {
				return growing;
			}
	
			foreach (var leaf in m_Leaves) {

				T closest = default(T);
				float closestDist = float.MaxValue;

				foreach (var branch in branches) {
					var dist = Vector2.Distance (leaf.position, branch.endPos);

					if (dist < m_MinDistance) { // too close
						leaf.hasBeenReached = true;
						closest = default(T);
						break;
					}

					if (dist <= m_MaxDistance && dist < closestDist) {
						closest = branch;
						closestDist = dist;
					}
				}

				if (closest != null) {

					var dir = (leaf.position - closest.endPos).normalized;

					closest.colonizationDir += dir;

					closest.colonizationLeafCount++;
				}
			}


			for (int i = m_Leaves.Count - 1; i >= 0; i--) {
				if (m_Leaves [i].hasBeenReached) {
					GameObject.Destroy (m_Leaves [i].gameObject);
					m_Leaves.RemoveAt (i);
				}
			}

			for (int i = branches.Count - 1; i >= 0; i--) {

				var branch = branches [i];

				if (branch.colonizationLeafCount > 0) {
					branch.colonizationDir /= (branch.colonizationLeafCount + 1);

					branches.Add (CreateBranch (branch, 
						branch.endPos + branch.colonizationDir));

					growing = true;

				}

				branch.DoColonizationReset ();
			}

			return growing;

		}
	}
}