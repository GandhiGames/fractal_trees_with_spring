using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
    public class DemoForceController : MonoBehaviour
    {
        public MovingTreeBuilder tree;
		public float radius = 10;
		public float force = 2f;

        void Update()
        {
			if (Input.GetMouseButton (0)) {
				ApplyPushForce ();
			} else if (Input.GetMouseButton (1)) {
				ApplyPullForce ();
			}

			var move = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

			if(move != Vector2.zero)
			{
				ApplyDirectedForce (move);
        	}
		}

		private void ApplyDirectedForce(Vector2 dir)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			tree.ApplyDirectedForce(dir, pos, 200f);
        }

        private void ApplyPushForce()
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			tree.ApplyPushForce(force, pos, radius);
        }

        private void ApplyPullForce()
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			tree.ApplyPullForce(0.4f, pos, radius);
        }
    }
}