using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FractalTree.Demo
{
	[RequireComponent(typeof(Text))]
	public class DemoTreeData : MonoBehaviour
	{
		public DemoTreeCreator treeCreator;

		private Text m_Text;
		private TreeBuilder m_Current;

		void Awake()
		{
			m_Text = GetComponent<Text> ();
		}
			
		void Update()
		{
			if (treeCreator.activeTree == null || treeCreator.activeTree == m_Current) {
				return;
			}

			m_Current = treeCreator.activeTree;

			Clear ();

			AppendLine ("Type: " + m_Current.treeType.ToString ());
			AppendLine("Moving: " + (!treeCreator.showingStationary ? "yes" : "no"));

			switch (m_Current.treeType) {
			case TreeBuilder.TreeType.Branching:
				AppendLine ("Generation: " + m_Current.defaultGrowthCount);
				AppendLine ("Initial Length: " + m_Current.defaultInitialLength);
				AppendLine ("Length Degredation: " + m_Current.defaultLengthReductionOnNewGeneration);
				AppendLine ("Angle: " + m_Current.defaultAngle);
				AppendLine ("Width: " + m_Current.defaultWidth);
				break;
			case TreeBuilder.TreeType.LTree:
				AppendLine ("Auto Width: " + (m_Current.lTreeAutoWidth ? "yes" : "no"));
				if (!treeCreator.showingStationary) {
					AppendLine ("Mass Based on Width: " + (m_Current.lTreeAutoWidth ? "yes" : "no"));
				}
				AppendLine ("Initial Width: " + m_Current.lTreeWidth);
				AppendLine ("Initial Length: " + m_Current.lTreeBranchLength);
				AppendLine ("Generations: " + m_Current.lTreeGrowthCount);
				AppendLine ("Angle: " + m_Current.lTreeAngle);
				AppendLine ("Axiom: " + m_Current.lTreeAxiom);

				foreach (var rule in m_Current.lTreeRules) {
					AppendLine ("Rule: " + rule.from + "=" + rule.to);
				}
				break;
			case TreeBuilder.TreeType.Colonization:
				AppendLine ("Initial Length: " + m_Current.colonizationInitialLength);
				AppendLine ("Width: " + m_Current.colonizationWidth);
				AppendLine ("Min Distance to Leafs: " + m_Current.colonizationMinDistance);
				AppendLine ("Max Distance to Leafs: " + m_Current.colonizationMaxDistance);
				break;
			}


		}

		private void AppendLine(object obj)
		{
			m_Text.text += obj.ToString() + '\n';
		}

		private void Clear()
		{
			m_Text.text = "";
		}
	}
}