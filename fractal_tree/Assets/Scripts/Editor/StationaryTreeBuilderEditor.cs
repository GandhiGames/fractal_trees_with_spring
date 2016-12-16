using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace FractalTree
{
	[CustomEditor(typeof(StationaryTreeBuilder))]
	public class StationaryTreeBuilderEditor : TreeBuilderEditor
	{
		public override void OnInspectorGUI()
		{
			DrawEditor ();

		}
	}
}