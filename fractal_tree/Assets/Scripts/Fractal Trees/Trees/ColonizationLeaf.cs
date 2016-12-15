using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	/// <summary>
	/// Attach to leaf objects for space colonization. The branches move towards the leaves.
	/// </summary>
	public class ColonizationLeaf : MonoBehaviour
	{
		/// <summary>
		/// Within the minimum distance of a branch. To be removed from the simulation.

		/// </summary>
		/// <value><c>true</c> if has been reached; otherwise, <c>false</c>.</value>
		public bool hasBeenReached { get; set; }

		/// <summary>
		/// Gets the position of the leaf.
		/// </summary>
		/// <value>The position.</value>
		public Vector2 position { get { return transform.position; } }
	}
}