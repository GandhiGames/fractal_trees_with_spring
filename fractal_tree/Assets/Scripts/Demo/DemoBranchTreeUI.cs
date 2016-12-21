using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FractalTree.Demo
{
	/// <summary>
	/// For demo purposes. Listens for changes in UI and updates Branch variables.
	/// </summary>
	public class DemoBranchTreeUI : MonoBehaviour
	{
		public TreeBuilder treeBuilder;

		public InputField genInput;
		public InputField lengthInput;
		public InputField multiplierInput;
		public InputField angleInput;
		public InputField widthInput;

		void Start ()
		{
			genInput.text = treeBuilder.defaultGrowthCount.ToString();
			lengthInput.text = treeBuilder.defaultInitialLength.ToString();
			multiplierInput.text = treeBuilder.defaultLengthReductionOnNewGeneration.ToString ();
			angleInput.text = treeBuilder.defaultAngle.ToString();
			widthInput.text = treeBuilder.defaultWidth.ToString();
		}

		public void OnGenerationChange (string value)
		{
			int changed;

			if (int.TryParse (value, out changed)) {
				treeBuilder.defaultGrowthCount = changed;
			}
		}

		public void OnLengthChanged (string value)
		{
			float changed;

			if (float.TryParse (value, out changed)) {
				treeBuilder.defaultInitialLength = changed;
			}
		}

		public void OnMultiplierChanged(string value)
		{
			float changed;

			if (float.TryParse (value, out changed)) {
				treeBuilder.defaultLengthReductionOnNewGeneration = changed;
			}
		}

		public void OnAngleChanged (string value)
		{
			int changed;

			if (int.TryParse (value, out changed)) {
				treeBuilder.defaultAngle = changed;
			}
		}

		public void OnWidthChanged (string value)
		{
			float changed;

			if (float.TryParse (value, out changed)) {
				treeBuilder.defaultWidth = changed;
			}
		}

		public void Generate ()
		{
			treeBuilder.Remove ();
			treeBuilder.Build ();
		}
	}
}