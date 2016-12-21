using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	/// <summary>
	/// Spawns a fractal true using the L-system: http://www.allenpike.com/modeling-plants-with-l-systems/
	/// </summary>
	public class LTree : Tree
	{
		private int m_Steps;
		private LRule[] m_Rules;
		private string m_Seed;
		private string m_Sentence;
		private GameObject m_BranchPrefab;
		private float m_Length;
		private float m_Angle;
		private Transform m_Owner;
		private Color[] m_Colors;
		private float[] m_Widths;
		private bool m_AutoWidth;
		private bool m_AutoMass;

		/// <summary>
		/// Initializes a new instance of the <see cref="FractalTree.LTree"/> class.
		/// </summary>
		/// <param name="branchPrefab">Branch prefab.</param>
		/// <param name="steps">Steps.</param>
		/// <param name="axiom">Axiom.</param>
		/// <param name="rules">Rules.</param>
		/// <param name="initialLength">Initial length.</param>
		/// <param name="angle">Angle.</param>
		/// <param name="owner">Owner.</param>
		/// <param name="colors">Colors.</param>
		/// <param name="width">Width.</param>
		/// <param name="autoWidth">If set to <c>true</c> auto width.</param>
		/// <param name="autoMass">If set to <c>true</c> auto mass.</param>
		public LTree (GameObject branchPrefab, 
		              int steps,
		              string axiom,
		              LRule[] rules,
		              float branchLength,
		              float angle, Transform owner, Color[] colors, 
		              float width,
		              bool autoWidth,
		              bool autoMass)
		{
			m_BranchPrefab = branchPrefab;
			m_Steps = steps;
			m_Seed = axiom;
			m_Rules = rules;
			m_Sentence = m_Seed;
			m_Length = branchLength;
			m_Angle = angle;

			m_Owner = owner;

			if (colors.Length == 0) {
				Debug.LogWarning ("No colors set for tree/shape. Defaulting to white");
				m_Colors = new Color[]{ Color.white };
			} else {
				m_Colors = colors;
			}

			m_AutoWidth = autoWidth;
			m_AutoMass = autoMass;

			m_Widths = new float[m_Colors.Length];

			float widthOffset = 1f / (m_Widths.Length * 1.2f);

			for (int i = 0; i < m_Widths.Length; i++) {
				m_Widths [i] = width - width * (i * widthOffset);
			}
		}

		/// <summary>
		/// Generates the tree.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public List<T> Generate<T> () where T : Branch
		{
			for (int i = 0; i < m_Steps; i++) {
				CalculateNextStep ();

			}

			return CreateTree<T> ();
		}

		private void CalculateNextStep ()
		{
			string nextSentence = "";

			for (int i = 0; i < m_Sentence.Length; i++) {
				var currentChar = m_Sentence [i];

				bool matched = false;

				for (int j = 0; j < m_Rules.Length; j++) {
					
					if (m_Rules [j].from == '\0') {
						continue;
					}

					if (currentChar.Equals (m_Rules [j].from)) {
						nextSentence += m_Rules [j].to;
						matched = true;
						break;
					}
				}

				if (!matched) {
					nextSentence += currentChar;
				}
			}

			m_Sentence = nextSentence;
		}

		private List<T> CreateTree<T> () where T : Branch
		{
			Vector2 current = m_Owner.transform.position;

			float rotation = 0f;

			var storedStates = new Stack<LMovementState<T>> ();

			var branchesCreated = new List<T> ();

			T previousBranch = default(T);

			Color color = m_Colors [0];

			float width = m_Widths [0];

			for (int i = 0; i < m_Sentence.Length; i++) {
				var currentChar = m_Sentence [i];

				if (currentChar.Equals ('F')) {
					var branch = ((GameObject)MonoBehaviour.Instantiate (m_BranchPrefab))
						.GetComponent<T> ();

					Vector2 next = current
					               + (Vector2.up * m_Length).Rotate (rotation);

					if (previousBranch == null) {
						previousBranch = branch;
						branch.Setup (current, next, width, color, m_AutoMass);
					} else {
						branch.Setup (previousBranch, next, width, color, m_AutoMass);
					}

					previousBranch = branch;

					branch.transform.SetParent (m_Owner);
					branchesCreated.Add (branch);
					current = next;
				} else if (currentChar.Equals ('G')) {
					current += (Vector2.up * m_Length).Rotate (rotation);
				} else if (currentChar.Equals ('+')) {
					rotation -= m_Angle;
				} else if (currentChar.Equals ('-')) {
					rotation += m_Angle;
				} else if (currentChar.Equals ('[')) {
					storedStates.Push (new LMovementState<T> (current, 
						rotation, previousBranch, color, width));
				} else if (currentChar.Equals (']')) {
					var storedState = storedStates.Pop ();
					current = storedState.position;
					rotation = storedState.rotation;
					previousBranch = storedState.previousBranch;
					color = storedState.color;
					width = storedState.thickness;
				} else if (currentChar.Equals ('C')) {
					if (i < m_Sentence.Length - 1) {
						char next = m_Sentence [i + 1];

						int nextColorIndex = -1;

						if (int.TryParse ("" + next, out nextColorIndex)) {
							if (nextColorIndex > m_Colors.Length - 1) {
								Debug.Log ("Incorrect number of colours for rule set");
							} else if (nextColorIndex >= 0) {
								color = m_Colors [nextColorIndex];

								if (m_AutoWidth) {
									width = m_Widths [nextColorIndex];
								}
							}
                       
						}
					}
				} else if (currentChar.Equals ('!')) {
					m_Angle *= -1f;
				} else if (currentChar.Equals ('|')) {
					rotation = 180f;
				}

			}

			return branchesCreated;
		}

		private class LMovementState<T> where T : Branch
		{
			public Vector2 position { get; private set; }

			public float rotation { get; private set; }

			public T previousBranch { get; private set; }

			public Color color { get; private set; }

			public float thickness { get; private set; }

			public LMovementState (Vector2 position, 
			                       float rotation, T previousBranch, 
			                       Color color, float thickness)
			{
				this.position = position;
				this.rotation = rotation;
				this.previousBranch = previousBranch;
				this.color = color;
				this.thickness = thickness;
			}
		}
			
	}

	[System.Serializable]
	public class LRule
	{
		public char from;
		public string to;

		public LRule (char from, string to)
		{
			this.from = from;
			this.to = to;
		}
	}


}
