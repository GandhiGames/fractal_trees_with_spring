using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{

	/// <summary>
	/// Builds a moving tree and provides methods of applying forces to generated trees.
	/// </summary>
    public class MovingTreeBuilder : TreeBuilder
    {
		/// <summary>
		/// All forces applies to this tree are multiplied by this value. Use this to create trees that are more sensitive to forces than others.
		/// </summary>
		public float forceDistanceMulti = 4f;

		private List<MovingBranch> m_Branches = new List<MovingBranch>();

		/// <summary>
		/// A list of all branches associated with the tree.
		/// </summary>
		/// <value>The branches.</value>
		public List<MovingBranch> branches { get { return m_Branches; } }

        void Start()
        {
			if (buildOnStart) {
				Build ();
			}
        }

		/// <summary>
		/// Build this instance.
		/// </summary>
		public override void Build ()
		{
			m_Branches = DoBuild<MovingBranch> ();
		}

		/// <summary>
		/// Deletes all child branches and clears branch list.
		/// </summary>
		public override void Remove ()
		{
			for (int i = m_Branches.Count - 1; i >= 0; i--) {
				Destroy (m_Branches [i].transform.gameObject);
			}

			m_Branches.Clear ();
		}

		/// <summary>
		/// Applies a directed force to all branches within range.
		/// </summary>
		/// <param name="force">Force.</param>
		/// <param name="position">Position.</param>
		/// <param name="radius">Radius.</param>
        public void ApplyDirectedForce(Vector2 force, Vector2 position, float radius)
        {
            foreach (var branch in m_Branches)
            {
				float distance = Vector2.Distance(position, branch.endPoint.position);

				if (distance < radius)
                {
					branch.endPoint.ApplyForce(force);
                }
            }
        }

		/// <summary>
		/// Applies a push force to all branches within range.
		/// </summary>
		/// <param name="force">Force.</param>
		/// <param name="position">Position.</param>
		/// <param name="radius">Radius.</param>
        public void ApplyPushForce(float force, Vector2 position, float radius)
        {
			
            foreach (var branch in m_Branches)
            {
				float distance = Vector2.Distance(position, branch.endPoint.position);

				if (distance < radius)
                {
					Vector2 heading = branch.endPoint.position - position;

					branch.endPoint.ApplyForce((force * heading) / (distance * forceDistanceMulti));
                    branch.endPoint.IncreaseDamping(0.6f);
                }
            }
        }

		/// <summary>
		/// Applies a pull force to all branches in range.
		/// </summary>
		/// <param name="force">Force.</param>
		/// <param name="position">Position.</param>
		/// <param name="radius">Radius.</param>
        public void ApplyPullForce(float force, Vector2 position, float radius)
        {
            foreach (var branch in m_Branches)
            {
                float distance = Vector2.Distance(position, branch.endPoint.position);

                if (distance < radius)
                {
					Vector2 heading = position - branch.endPoint.position;

					branch.endPoint.ApplyForce((force * heading) / (distance * forceDistanceMulti));
                    branch.endPoint.IncreaseDamping(0.6f);
                }
            }
        }

    }
}

