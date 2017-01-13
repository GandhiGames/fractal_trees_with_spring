using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FractalTree
{
	[CustomEditor(typeof(MovingTreeBuilder)), CanEditMultipleObjects]
	public class MovingTreeBuilderEditor : TreeBuilderEditor
	{
		public override void OnInspectorGUI()
		{
			DrawEditor ();

		}
	}
}
