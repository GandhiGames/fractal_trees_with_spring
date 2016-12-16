using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	public class DemoTreeCreator : MonoBehaviour
	{
		public TreeBuilder[] treeBuilders;
		public ColonizationLeafGenerator leafGenerator;

		private int m_TreeIndex = -1;

		void Start()
		{
			foreach (var tree in treeBuilders) {
				leafGenerator.Generate ();
				tree.Build ();
				tree.gameObject.SetActive (false);
			}
		}

		void Update()
		{
			if (Input.GetKeyUp (KeyCode.Return)) {

				if (m_TreeIndex >= 0) {
					treeBuilders [m_TreeIndex].gameObject.SetActive (false);
				}

				m_TreeIndex = (m_TreeIndex + 1) % treeBuilders.Length;



				treeBuilders [m_TreeIndex].gameObject.SetActive (true);
			}
		}
	}
}