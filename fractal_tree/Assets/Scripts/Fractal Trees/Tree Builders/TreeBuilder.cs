using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	public class TreeBuilder : MonoBehaviour
	{

		public enum TreeType
		{
			Default,
			LTree,
			Colonization
		}

		public TreeType treeType = TreeType.Default;
		public GameObject branchPrefab;

		[Header ("Default Tree")]
		public int growthCount = 3;
		public float initialLength = 1f;
		public float lengthDegradation = 0.67f;
		public float angle = 45f;
		public float thickness = 0.4f;

		[Header ("L Tree")]
		public bool lTreeAutoWidth = true;
		public bool lTreeMassBasedOnWidth = true;
		public float lTreeWidth = 0.03f;
		public int steps = 4;
		public string axiom = "FX";
		public LRule[] rules = new LRule[] {
			// F = forward
			// + = rotate right
			// - = rotate left
			// [ = save state
			// ] = restore state

			new LRule ('F', "C0FF-[C1-F+F]+[C2+F-F]"),
			new LRule ('X', "C0FF+[C1+F]+[C3-F]")
		};
		public float lTreeBranchLength = 0.2f;
		public float lTreeAngle = 25f;
		public Color[] lTreeColours;

		[Header("Colonization")]
		public Transform colonizationLeafParent;
		public float colonizationInitialLength = 1f;
		public float colonizationWidth = 0.4f;
		public float colonizationMinDistance = 1f;
		public float colonizationMaxDistance = 10f;

		protected Tree CreateTree ()
		{
			Tree tree = null;

			switch (treeType) {
   

			case TreeType.Default:
				tree = new DefaultTree (growthCount, initialLength,
					lengthDegradation, angle, thickness,
					branchPrefab, transform);
				break;
			case TreeType.LTree:
				tree = new LTree (branchPrefab, steps,
					axiom, rules, lTreeBranchLength,
					lTreeAngle, transform, lTreeColours, lTreeWidth, 
					lTreeAutoWidth, lTreeMassBasedOnWidth);
				break;
			case TreeType.Colonization:
				tree = new ColonizationTree (
					new List<ColonizationLeaf>(
						colonizationLeafParent.GetComponentsInChildren<ColonizationLeaf>()
					), 
					transform, colonizationInitialLength, branchPrefab, 
					colonizationWidth, colonizationMinDistance, colonizationMaxDistance);
				break;
			}

			return tree;
		}
	}

    
}