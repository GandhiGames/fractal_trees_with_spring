using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	/// <summary>
	/// All trees should have a method of generating themselves.
	/// </summary>
	public interface Tree
	{
		List<T> Generate<T>() where T : Branch;
	}

	/// <summary>
	/// The base tree builder class. Provides paramaters for default, L, and colonization tree generation.
	/// </summary>
	public abstract class TreeBuilder : MonoBehaviour
	{
		/// <summary>
		/// If true, builds tree on start.
		/// </summary>
		public bool buildOnStart = true;
		
		/// <summary>
		/// Tree type.
		/// </summary>
		public enum TreeType
		{
			Branching,
			LTree,
			Colonization
		}

		/// <summary>
		/// The tree type to generate.
		/// </summary>
		public TreeType treeType = TreeType.Branching;

		/// <summary>
		/// The branch prefab. If tree to generate is moving then prefab should have MovingBranch script attached.
		/// </summary>
		public GameObject branchPrefab;

		[Header ("Default Tree")]

		/// <summary>
		/// The branch colour for the default tree.
		/// </summary>
		public Color defaultBranchColour = Color.white;

		/// <summary>
		/// The number of tree generations.
		/// </summary>
		public int defaultGrowthCount = 8;

		/// <summary>
		/// The default length of the initial branches for the default tree generation.
		/// </summary>
		public float defaultInitialLength = 5f;

		/// <summary>
		/// The length degradation for the default tree. Branches are reduced in size by this factor.
		/// </summary>
		public float defaultLengthReductionOnNewGeneration = 0.67f;

		/// <summary>
		/// The angle for default tree branching.
		/// </summary>
		public float defaultAngle = 45f;

		/// <summary>
		/// The width of the branches for the default tree generator.
		/// </summary>
		public float defaultWidth = 0.04f;

		[Header ("L Tree")]
		/// <summary>
		/// When true, the width of the branches will be set automatically based on the colours.
		/// </summary>
		public bool lTreeAutoWidth = true;

		/// <summary>
		/// When true, the mass of the branches will be set automatically based on colours. Used only when generating a moving tree.
		/// </summary>
		public bool lTreeMassBasedOnWidth = true;

		/// <summary>
		/// The max branch width for L trees.
		/// </summary>
		public float lTreeWidth = 0.03f;

		/// <summary>
		/// The number of L tree generations.
		/// </summary>
		public int lTreeGrowthCount = 5;

		/// <summary>
		/// The l tree axiom. The initial seed used to generate a L tree.
		/// </summary>
		public string lTreeAxiom = "FX";

		/// <summary>
		/// The rules applied to the axoim.
		/// </summary>
		public LRule[] lTreeRules = new LRule[] {
			// F = forward
			// + = rotate right
			// - = rotate left
			// [ = save state
			// ] = restore state

			new LRule ('F', "C0FF-[C1-F+F]+[C2+F-F]"),
			new LRule ('X', "C0FF+[C1+F]+[C3-F]")
		};

		/// <summary>
		/// The length of the l tree branch.
		/// </summary>
		public float lTreeBranchLength = 0.17f;

		/// <summary>
		/// The angles used to branch an L tree.
		/// </summary>
		public float lTreeAngle = 25f;

		/// <summary>
		/// The L tree colours.
		/// </summary>
		public Color[] lTreeColours;

		[Header ("Colonization")]
		/// <summary>
		/// The parent of the game object that holds the colonization leaves.
		/// </summary>
		public Transform colonizationLeafParent;

		public Color colonizationBranchColor = Color.white;

		/// <summary>
		/// The initial length for a colonization tree trunk.
		/// </summary>
		public float colonizationInitialLength = 1f;

		/// <summary>
		/// The width of the colonization tree branches.
		/// </summary>
		public float colonizationWidth = 0.04f;

		/// <summary>
		/// The minimum distance between the branch and a colonization leaf for it to be registered.
		/// </summary>
		public float colonizationMinDistance = 1f;

		/// <summary>
		/// The maximum distance between the branch and a colonization leaf for it to be registered.
		/// </summary>
		public float colonizationMaxDistance = 10f;

		public abstract void Build();
		public abstract void Remove();

		/// <summary>
		/// Build this instance of the tree.
		/// </summary>
		protected List<T> DoBuild<T> () where T : Branch
		{
			var branches = new List<T>();
			
			Tree tree = CreateTree();

			if (tree != null)
			{
				branches = tree.Generate<T>();
			}

			return branches;
		}

		/// <summary>
		/// Creates a tree based on treeType.
		/// </summary>
		/// <returns>The tree.</returns>
		protected Tree CreateTree ()
		{
			Tree tree = null;

			switch (treeType) {
   

			case TreeType.Branching:
				tree = new DefaultTree (defaultGrowthCount, defaultInitialLength,
					defaultLengthReductionOnNewGeneration, defaultAngle, defaultWidth,
					branchPrefab, defaultBranchColour, transform);
				break;
			case TreeType.LTree:
				tree = new LTree (branchPrefab, lTreeGrowthCount,
					lTreeAxiom, lTreeRules, lTreeBranchLength,
					lTreeAngle, transform, lTreeColours, lTreeWidth, 
					lTreeAutoWidth, lTreeMassBasedOnWidth);
				break;
			case TreeType.Colonization:
				tree = new ColonizationTree (
					new List<ColonizationLeaf> (
						colonizationLeafParent.GetComponentsInChildren<ColonizationLeaf> ()
					), 
					transform, colonizationInitialLength, branchPrefab, 
					colonizationWidth, colonizationMinDistance, colonizationMaxDistance, colonizationBranchColor);
				break;
			}

			return tree;
		}
	}

    
}