using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace FractalTree
{
	/// <summary>
	/// Custom editor for tree builder class. Hides variables not in use based on TreeType.
	/// </summary>
	[CustomEditor (typeof(TreeBuilder))]
	public abstract class TreeBuilderEditor : Editor
	{
		private TreeBuilder m_TreeBuilder;

		void Awake()
		{
			m_TreeBuilder = (TreeBuilder)target;
		}



		protected void DrawEditor()
		{
			m_TreeBuilder.buildOnStart = EditorGUILayout.Toggle ("Build On Start", m_TreeBuilder.buildOnStart);

			m_TreeBuilder.branchPrefab = (GameObject)EditorGUILayout.ObjectField(
				"Branch Prefab", m_TreeBuilder.branchPrefab, typeof(GameObject), false);

			m_TreeBuilder.treeType = (TreeBuilder.TreeType) EditorGUILayout.EnumPopup ("Tree Type", m_TreeBuilder.treeType);

			switch (m_TreeBuilder.treeType) {

			case TreeBuilder.TreeType.Branching:
				ShowDefaultTreeOptions ();
				break;
			case TreeBuilder.TreeType.LTree:
				ShowLTreeOptions ();
				break;
			case TreeBuilder.TreeType.Colonization:
				ShowColonizationOptions ();
				break;
			default :
				break;
			}

			if (GUI.changed) {
				
				EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
			}
		}

		private void ShowDefaultTreeOptions()
		{
			m_TreeBuilder.defaultBranchColour = EditorGUILayout.ColorField ("Branch Color", m_TreeBuilder.defaultBranchColour);
			m_TreeBuilder.defaultGrowthCount = EditorGUILayout.IntField("Generations", m_TreeBuilder.defaultGrowthCount);
			m_TreeBuilder.defaultInitialLength = EditorGUILayout.FloatField ("Initial Length", m_TreeBuilder.defaultInitialLength);
			m_TreeBuilder.defaultLengthReductionOnNewGeneration = EditorGUILayout.FloatField ("Length Multiplier On New Generation", m_TreeBuilder.defaultLengthReductionOnNewGeneration);
			m_TreeBuilder.defaultAngle = EditorGUILayout.FloatField ("Angle", m_TreeBuilder.defaultAngle);
			m_TreeBuilder.defaultWidth = EditorGUILayout.FloatField ("Branch Width", m_TreeBuilder.defaultWidth);
		}

		private void ShowLTreeOptions()
		{
			EditorGUI.BeginChangeCheck();

			m_TreeBuilder.lTreeAutoWidth = EditorGUILayout.Toggle ("Auto Width", m_TreeBuilder.lTreeAutoWidth);
			m_TreeBuilder.lTreeMassBasedOnWidth = EditorGUILayout.Toggle ("Mass Based on Width", m_TreeBuilder.lTreeMassBasedOnWidth);
			m_TreeBuilder.lTreeWidth = EditorGUILayout.FloatField ("Branch Initial Width", m_TreeBuilder.lTreeWidth);
			m_TreeBuilder.lTreeGrowthCount = EditorGUILayout.IntField("Generations", m_TreeBuilder.lTreeGrowthCount);
			m_TreeBuilder.lTreeAxiom = EditorGUILayout.TextField ("Axiom", m_TreeBuilder.lTreeAxiom);

			SerializedProperty rules = serializedObject.FindProperty ("lTreeRules");
			EditorGUILayout.PropertyField(rules, true);

			m_TreeBuilder.lTreeBranchLength = EditorGUILayout.FloatField ("Branch Length", m_TreeBuilder.lTreeBranchLength);
			m_TreeBuilder.lTreeAngle = EditorGUILayout.FloatField ("Angle", m_TreeBuilder.lTreeAngle);

			SerializedProperty treeColors = serializedObject.FindProperty ("lTreeColours");
			EditorGUILayout.PropertyField(treeColors, true);

			if (EditorGUI.EndChangeCheck ()) {
				serializedObject.ApplyModifiedProperties ();
			}
		}

		private void ShowColonizationOptions()
		{
			EditorGUI.BeginChangeCheck();

			m_TreeBuilder.colonizationLeafParent = (Transform)EditorGUILayout.ObjectField(
				"Leaf Parent", m_TreeBuilder.colonizationLeafParent, typeof(Transform), true);
			m_TreeBuilder.colonizationBranchColor = EditorGUILayout.ColorField ("Branch Color", m_TreeBuilder.colonizationBranchColor);
			m_TreeBuilder.colonizationInitialLength = EditorGUILayout.FloatField ("Branch Initial Length", m_TreeBuilder.colonizationInitialLength);
			m_TreeBuilder.colonizationWidth = EditorGUILayout.FloatField ("Branch Width", m_TreeBuilder.colonizationWidth);
			m_TreeBuilder.colonizationMinDistance = EditorGUILayout.FloatField ("Min Distance To Leaf", m_TreeBuilder.colonizationMinDistance);
			m_TreeBuilder.colonizationMaxDistance = EditorGUILayout.FloatField ("Max Distance To Leaf", m_TreeBuilder.colonizationMaxDistance);

			if (EditorGUI.EndChangeCheck ()) {
				serializedObject.ApplyModifiedProperties ();
			}
		}
	}
}