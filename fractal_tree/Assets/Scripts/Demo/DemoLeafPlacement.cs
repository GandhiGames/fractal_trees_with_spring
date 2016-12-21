using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	/// <summary>
	/// For demo purposes. Spawns a leaf object (used for space colonization tree algorithm) at mouse position on left-click.
	/// </summary>
	public class DemoLeafPlacement : MonoBehaviour
	{
		public GameObject leafPrefab;

		void Update ()
		{
			if (Input.GetMouseButtonUp (0)) {
				var pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

				if (pos.x < 5) {
					pos.z = 0f;

					var leaf = (GameObject)Instantiate (leafPrefab, pos, Quaternion.identity);

					leaf.transform.SetParent (transform);
				}

			
			}
		}

		/// <summary>
		/// Remove all child leaves.
		/// </summary>
		public void Clear()
		{
			for (int i = transform.childCount - 1; i >= 0; i--) {
				Destroy (transform.GetChild (i).gameObject);
			}
		}
	}
}