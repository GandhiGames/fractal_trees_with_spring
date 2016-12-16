using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	/// <summary>
	/// Spawns a set number of leaves within a bounds. Used by space colonization.
	/// </summary>
	public class ColonizationLeafGenerator : MonoBehaviour
	{
		/// <summary>
		/// The bounds within which to spawn the leafs.
		/// </summary>
		public Rect bounds;

		/// <summary>
		/// The number of leaves to spawn.
		/// </summary>
		public int numToCreate = 100;

		void Awake ()
		{
			Generate ();
		}

		public void Generate()
		{
			Remove ();

			for (int i = 0; i < numToCreate; i++) {
				GameObject leaf = new GameObject ("Leaf " + (i + 1));
				leaf.transform.position = new Vector2 (
					Random.Range (bounds.xMin, bounds.xMax),
					Random.Range (bounds.yMin, bounds.yMax));
				leaf.AddComponent<ColonizationLeaf> ();
				leaf.transform.SetParent (transform);

			}
		}

		public void Remove()
		{
			for (int i = transform.childCount - 1; i >= 0; i--) {
				Destroy(transform.GetChild(i).gameObject);
			}
		}

	}
}