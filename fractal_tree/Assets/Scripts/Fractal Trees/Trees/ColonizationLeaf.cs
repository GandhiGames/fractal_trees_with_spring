using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	public class ColonizationLeaf : MonoBehaviour
	{
		public bool hasBeenReached { get; set; }
		public Vector2 position { get { return transform.position; } }
	}
}