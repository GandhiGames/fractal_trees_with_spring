using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree.Demo
{
	public class DemoTreeCreator : MonoBehaviour
	{
		/// <summary>
		/// A list of stationary and moving tree pairs with helper methods to switch between them.
		/// </summary>
		public TreesToDemo[] treeBuilders;

		/// <summary>
		/// A leaf generator used for space colonization trees.
		/// </summary>
		public ColonizationLeafGenerator leafGenerator;

		/// <summary>
		/// Showing a static or moving version of the tree.
		/// </summary>
		public bool showingStationary = true;

		public int startIndex = 0;

		/// <summary>
		/// Gets the active tree or null if there is none.
		/// </summary>
		/// <value>The active tree.</value>
		public TreeBuilder activeTree {
			get {
				return treeBuilders [m_TreeIndex].active;
			}
		}

		private int m_TreeIndex = 0;
	

		void Start()
		{
			m_TreeIndex = startIndex;
			
			for(int i = 0 ; i < treeBuilders.Length; i++)
			{
				var demo = treeBuilders [i];

				if (demo.preload) {
					BuildTree (demo);
					demo.built = true;
				}
			}

			if (!treeBuilders [m_TreeIndex].built) {
				BuildTree (treeBuilders [m_TreeIndex]);
				treeBuilders [m_TreeIndex].built = true;
			}

			treeBuilders [m_TreeIndex].Show (showingStationary);
		}

		public void ShowNextTree()
		{
			treeBuilders [m_TreeIndex].Hide ();


			m_TreeIndex = (m_TreeIndex + 1) % treeBuilders.Length;

			if (!treeBuilders [m_TreeIndex].built) {
				BuildTree (treeBuilders [m_TreeIndex]);
				treeBuilders [m_TreeIndex].built = true;
			}

			if (!showingStationary && treeBuilders [m_TreeIndex].movingTree == null) {
				showingStationary = true;
			}

			treeBuilders [m_TreeIndex].Show (showingStationary);
		}

		public bool SwitchTreeState()
		{
			if (!(showingStationary && treeBuilders [m_TreeIndex].movingTree == null)) {
				showingStationary = !showingStationary;
				treeBuilders [m_TreeIndex].Show (showingStationary);

				return true;
			}

			return false;
		}

		private void BuildTree(TreesToDemo demo)
		{
			if (leafGenerator != null && demo.stationaryTree.treeType == TreeBuilder.TreeType.Colonization) {
				leafGenerator.Generate ();
			}

			demo.BuildStationary ();

			if (leafGenerator != null && demo.movingTree != null && demo.movingTree.treeType == TreeBuilder.TreeType.Colonization) {
				leafGenerator.Generate ();
			}

			demo.BuildMoving ();
		}
	}

	/// <summary>
	/// Trees to demo. Stationary and Moving tree builder pairs.
	/// </summary>
	[System.Serializable]
	public class TreesToDemo
	{
		public bool preload;

		public bool built;
		
		/// <summary>
		/// The stationary tree.
		/// </summary>
		public TreeBuilder stationaryTree;

		/// <summary>
		/// The moving tree.
		/// </summary>
		public TreeBuilder movingTree;

		/// <summary>
		/// Gets the active tree builder.
		/// </summary>
		/// <value>The active.</value>
		public TreeBuilder active { get; private set; }

		/// <summary>
		/// Builds the stationary tree and then disables game object.
		/// </summary>
		public void BuildStationary()
		{
			stationaryTree.Build ();
			stationaryTree.gameObject.SetActive (false);
		}

		/// <summary>
		/// Builds the moving tree and then disables game object.
		/// </summary>
		public void BuildMoving()
		{
			if (movingTree != null) {
				movingTree.Build ();
				movingTree.gameObject.SetActive (false);
			}
		}

		/// <summary>
		/// Enables either the stationary or moving tree.
		/// </summary>
		/// <param name="showStationary">If set to <c>true</c> show stationary else show moving tree.</param>
		public void Show(bool showStationary)
		{	
			if (showStationary) {
				ShowStationary ();
			} else {
				ShowMoving ();
			}
		}

		/// <summary>
		/// Disables both trees.
		/// </summary>
		public void Hide()
		{
			stationaryTree.gameObject.SetActive (false);

			if (movingTree != null) {
				movingTree.gameObject.SetActive (false);
			}
		}

		private void ShowStationary()
		{
			if (movingTree != null) {
				movingTree.gameObject.SetActive (false);
			}

			stationaryTree.gameObject.SetActive (true);

			active = stationaryTree;
		}

		private void ShowMoving()
		{
			stationaryTree.gameObject.SetActive (false);

			if (movingTree != null) {
				movingTree.gameObject.SetActive (true);
			}

			active = movingTree;
		}
	}
}