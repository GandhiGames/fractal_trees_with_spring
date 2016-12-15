using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	/// <summary>
	/// Builds a stationary tree.
	/// </summary>
    public class StationaryTreeBuilder : TreeBuilder
    {       
		private List<Branch> m_Branches = new List<Branch>();

		/// <summary>
		/// A list of all branches associated with the tree.
		/// </summary>
		/// <value>The branches.</value>
		public List<Branch> branches { get { return m_Branches; } }

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
			m_Branches = DoBuild<Branch> ();
		}
			
    }
}