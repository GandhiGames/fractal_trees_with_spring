using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	public class ColonizationLeafGenerator : MonoBehaviour
	{
		public Rect bounds;
		public int numToCreate = 100;

		// Use this for initialization
		void Awake ()
		{
			for (int i = 0; i < numToCreate; i++) {
				GameObject leaf = new GameObject ("Leaf " + (i + 1));
				leaf.transform.position = new Vector2 (
					Random.Range (bounds.xMin, bounds.xMax),
						Random.Range (bounds.yMin, bounds.yMax));
				leaf.AddComponent<ColonizationLeaf> ();
				leaf.transform.SetParent (transform);
					
			}
		}

	}
}