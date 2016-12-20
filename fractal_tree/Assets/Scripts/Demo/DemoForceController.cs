using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree.Demo
{
	/// <summary>
	/// Applies forces to the currently active tree in the demo scene.
	/// </summary>
    public class DemoForceController : MonoBehaviour
    {
		public DemoTreeCreator treeCreator;
		public float radius = 10;
		public float pushForce = 2f;
		public float pullForce = 0.1f;

		[Range(0f, 1f)]
		public float directedForce = 0.5f;


        void Update()
        {
			if (treeCreator.showingStationary || treeCreator.activeTree == null) {
				return;
			}

			var targetTree = treeCreator.activeTree;

			if (!(targetTree is MovingTreeBuilder)) {
				return;
			}

			var target = (MovingTreeBuilder)targetTree;

			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if (Input.GetMouseButton (0)) {
				ApplyPushForce (pos, target);
			} else if (Input.GetMouseButton (1)) {
				ApplyPullForce (pos, target);
			}

			var move = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

			if(move != Vector2.zero)
			{
				ApplyDirectedForce (move, target);
        	}

		}

		private void ApplyDirectedForce(Vector2 dir, MovingTreeBuilder treeBuilder)
        {
			treeBuilder.ApplyDirectedForce(dir * directedForce, Vector2.zero, 200f);
        }

		private void ApplyPushForce(Vector2 pos, MovingTreeBuilder treeBuilder)
        {
			treeBuilder.ApplyPushForce(pushForce, pos, radius);
        }

		private void ApplyPullForce(Vector2 pos, MovingTreeBuilder treeBuilder)
        {
			treeBuilder.ApplyPullForce(pullForce, pos, radius);
        }
    }
}