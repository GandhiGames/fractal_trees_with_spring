using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FractalTree.Demo
{
	/// <summary>
	/// For demo purposes. Listens for changes in UI and updates Space Colonization variables.
	/// </summary>
	public class DemoColonizationTreeUI : MonoBehaviour
	{
		public TreeBuilder treeBuilder;
		public DemoLeafPlacement leafPlacement;

		public InputField lengthInput;
		public InputField widthInput;
		public InputField minDistanceToLeafInput;
		public InputField maxDistanceToLeafInput;

		public Button clearButton;
		public Button generateButton;

		// Use this for initialization
		void Start ()
		{
			LoadDefaultValues ();
			AddEventListeners ();
		}

		private void LoadDefaultValues ()
		{
			lengthInput.text = treeBuilder.colonizationInitialLength.ToString ();
			widthInput.text = treeBuilder.colonizationWidth.ToString ();
			minDistanceToLeafInput.text = treeBuilder.colonizationMinDistance.ToString ();
			maxDistanceToLeafInput.text = treeBuilder.colonizationMaxDistance.ToString ();
		}

		private void AddEventListeners ()
		{
			lengthInput.onEndEdit.AddListener (OnLengthChanged);
			widthInput.onEndEdit.AddListener (OnWidthChanged);
			minDistanceToLeafInput.onEndEdit.AddListener (OnMinDistChanged);
			maxDistanceToLeafInput.onEndEdit.AddListener (OnMaxDistChanged);
			clearButton.onClick.AddListener (Clear);
			generateButton.onClick.AddListener (Generate);
		}

		private void OnLengthChanged (string value)
		{
			float changed;

			if (float.TryParse (value, out changed)) {
				treeBuilder.colonizationInitialLength = changed;
			}
		}

		private void OnWidthChanged (string value)
		{
			float changed;

			if (float.TryParse (value, out changed)) {
				treeBuilder.colonizationWidth = changed;
			}
		}

		private void OnMinDistChanged (string value)
		{
			float changed;

			if (float.TryParse (value, out changed)) {
				treeBuilder.colonizationMinDistance = changed;
			}
		}

		private void OnMaxDistChanged (string value)
		{
			float changed;

			if (float.TryParse (value, out changed)) {
				treeBuilder.colonizationMaxDistance = changed;
			}
		}
	
		private void Clear()
		{
			treeBuilder.Remove ();
			leafPlacement.Clear ();
		}

		private void Generate()
		{
			treeBuilder.Remove ();
			treeBuilder.Build ();
		}
	}
}
