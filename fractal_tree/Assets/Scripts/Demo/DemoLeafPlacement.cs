using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	public class DemoLeafPlacement : MonoBehaviour
	{
		public GameObject leafPrefab;

		// Update is called once per frame
		void Update ()
		{
			if (Input.GetMouseButtonUp (0)) {
				var pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

				pos.z = 0f;

				var leaf = (GameObject)Instantiate (leafPrefab, pos, Quaternion.identity);

				leaf.transform.SetParent (transform);
			}
		}

		public void Clear()
		{
			for (int i = transform.childCount - 1; i >= 0; i--) {
				Destroy (transform.GetChild (i).gameObject);
			}
		}
	}
}