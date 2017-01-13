using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FractalTree.Demo
{
	/// <summary>
	/// For demo purposes. Listens for changes in UI and updates L-Tree variables.
	/// </summary>
	public class DemoLTreeUI : MonoBehaviour
	{
		public TreeBuilder treeBuilder;

		public Toggle autoWidthToggle;

		public InputField widthInput;
		public InputField genInput;
		public InputField lengthInput;
		public InputField angleInput;
		public InputField axiomInput;
		public InputField[] ruleInputs;

		public Button generate;

		void Start ()
		{
			autoWidthToggle.isOn = treeBuilder.lTreeAutoWidth;
			widthInput.text = treeBuilder.lTreeWidth.ToString ();
			genInput.text = treeBuilder.lTreeGrowthCount.ToString ();
			lengthInput.text = treeBuilder.lTreeBranchLength.ToString ();
			angleInput.text = treeBuilder.lTreeAngle.ToString ();
			axiomInput.text = treeBuilder.lTreeAxiom;

			for (int i = 0; i < treeBuilder.lTreeRules.Length; i++) {
				ruleInputs [i].text = RuleToString (treeBuilder.lTreeRules [i]);
			}

			if (treeBuilder.lTreeRules.Length < ruleInputs.Length) {

				LRule[] tempRules = new LRule[ruleInputs.Length];

				for (int i = 0; i < tempRules.Length; i++) {
					
					if (i < treeBuilder.lTreeRules.Length) {
						tempRules [i] = treeBuilder.lTreeRules [i];
					} else {
						tempRules[i] = new LRule('\0', string.Empty);
					}


				}

				treeBuilder.lTreeRules = tempRules;
			}

			AddListeners ();

		}

		private void AddListeners ()
		{
			autoWidthToggle.onValueChanged.AddListener (OnAutoWidthChanged);
			widthInput.onEndEdit.AddListener (OnWidthChanged);
			genInput.onEndEdit.AddListener (OnGenChanged);
			lengthInput.onEndEdit.AddListener (OnLengthChanged);
			angleInput.onEndEdit.AddListener (OnAngleChanged);
			axiomInput.onEndEdit.AddListener (OnAxiomChanged);
			ruleInputs [0].onEndEdit.AddListener (OnRule1Changed);
			ruleInputs [1].onEndEdit.AddListener (OnRule2Changed);
			ruleInputs [2].onEndEdit.AddListener (OnRule3Changed);
			ruleInputs [3].onEndEdit.AddListener (OnRule4Changed);

			generate.onClick.AddListener (Generate);
		}

		private void Generate ()
		{
			treeBuilder.Remove ();
			treeBuilder.Build ();
		}

		private void OnAutoWidthChanged (bool value)
		{
			treeBuilder.lTreeAutoWidth = value;
		}

		private void OnWidthChanged (string value)
		{
			float changed;

			if (float.TryParse (value, out changed)) {
				treeBuilder.lTreeWidth = changed;
			}
		}

		private void OnGenChanged (string value)
		{
			int changed;

			if (int.TryParse (value, out changed)) {
				treeBuilder.lTreeGrowthCount = changed;
			}
		}

		private void OnLengthChanged (string value)
		{
			float changed;

			if (float.TryParse (value, out changed)) {
				treeBuilder.lTreeBranchLength = changed;
			}
		}

		private void OnAngleChanged (string value)
		{
			int changed;

			if (int.TryParse (value, out changed)) {
				treeBuilder.lTreeAngle = changed;
			}
		}

		private void OnAxiomChanged (string value)
		{
			treeBuilder.lTreeAxiom = value;
		}

		private void OnRule1Changed (string value)
		{
			var rule = StringToRule (value);

			if (rule != null) {
				treeBuilder.lTreeRules [0] = StringToRule (value);
			}
		}

		private void OnRule2Changed (string value)
		{
			var rule = StringToRule (value);

			if (rule != null) {
				treeBuilder.lTreeRules [1] = StringToRule (value);
			}
		}

		private void OnRule3Changed (string value)
		{
			var rule = StringToRule (value);

			if (rule != null) {
				treeBuilder.lTreeRules [2] = StringToRule (value);
			}
		}

		private void OnRule4Changed (string value)
		{
			var rule = StringToRule (value);

			if (rule != null) {
				treeBuilder.lTreeRules [3] = StringToRule (value);
			}
		}

		private string RuleToString (LRule rule)
		{
			return string.Format ("{0}={1}", rule.from, rule.to);
		}

		private LRule StringToRule (string rule)
		{
			string[] formatedRule = rule.Split ('=');

			if (formatedRule.Length == 2) {
				return new LRule (formatedRule [0] [0], formatedRule [1]);
			}

			return null;
		}
	}
}