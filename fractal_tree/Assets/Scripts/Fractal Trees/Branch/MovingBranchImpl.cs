using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	[RequireComponent (typeof(SpriteRenderer), typeof(Spring))]
	public class MovingBranchImpl : MonoBehaviour, MovingBranch
	{
		public static float LengthDegradation = 0.67f;

		public PointMass startPoint { get { return m_Spring.start; } }

		public PointMass endPoint { get { return m_Spring.end; } }

		public Vector2 startPos { get { return startPoint.position; } }

		public Vector2 endPos { get { return endPoint.position; } }

		public Vector2 colonizationDir { get; set; }

		public int colonizationLeafCount { get; set; }

		public Color color {
			set {
				m_Color = value;
				m_ColorUpdateRequired = true;
			}
		}



		public bool hasBranched { get; set; }

		private static readonly float SPRITE_SIZE = 100f / 100f;
		// pixels of line sprite / pixels per units.

		private SpriteRenderer m_Renderer;
		private Spring m_Spring;
		private Color m_Color;
		private bool m_ColorUpdateRequired = false;
		private Branch m_Owner;
		private float m_Width;

		void Awake ()
		{
			m_Renderer = GetComponent<SpriteRenderer> ();
			m_Spring = GetComponent<Spring> ();
		}

		public void Setup (Branch owner, Vector2 end,
		                        float thickness, Color color)
		{
			m_Owner = owner;

			Setup (owner.endPos, end, thickness, color);
		}

		public void Setup (Branch owner, Vector2 end,
		                        float thickness, Color color, bool autoMass)
		{
			m_Owner = owner;

			Setup (owner.endPos, end, thickness, color, autoMass);
		}

		public void Setup (Vector2 start, Vector2 end,
		                        float width, Color color)
		{
			Setup (start, end, width, color, false);
		}

		public void Setup (Vector2 start, Vector2 end,
		                        float width, Color color, bool autoMass)
		{
			this.m_Width = width;
			this.m_Color = color;

			const float stiffness = 0.1f;
			const float damping = 0.96f;
			const float stationary = 0f;
			float moveable = autoMass ? 1f * (width) : 0.1f;
			const float bounceBackForce = 0.5f;

			m_Spring.Setup (new PointMass (start, stationary, bounceBackForce),
				new PointMass (end, moveable, bounceBackForce), stiffness, damping);

			colonizationDir = end - start;
			colonizationLeafCount = 0;

			UpdateSprite ();
			UpdateColor ();
		}

		public T DoBranching<T> (float angle) where T : Branch
		{
			var newBranch = ((GameObject)Instantiate (gameObject)).GetComponent<T> ();

			var dir = (endPoint.position - startPoint.position) * LengthDegradation;
			var dirRot = dir.Rotate (angle);
			var newEnd = endPoint.position + dirRot;

			newBranch.Setup (this, newEnd, this.m_Width, this.m_Color);

			return newBranch;
		}

		public void DoReset()
		{
			colonizationDir = endPos - startPos;
			colonizationLeafCount = 0;
		}

		void Update ()
		{
			bool updateReq = false;

			if (m_Owner != null) {
				if (startPoint.position != m_Owner.endPos) {
					startPoint.position = m_Owner.endPos;
					updateReq = true;
				}

			}


			startPoint.DoUpdate ();
			endPoint.DoUpdate ();

 
			if (updateReq || startPoint.forceApplied || endPoint.forceApplied) {
				m_Spring.DoUpdate ();
				UpdateSprite ();
			}

			if (m_ColorUpdateRequired) {
				UpdateColor ();
				m_ColorUpdateRequired = false;
			}


		}

		private void UpdateSprite ()
		{
			var heading = endPoint.position - startPoint.position;
			var distance = heading.magnitude;
			var direction = heading / distance;

			var centerPos = new Vector2 (
				                         startPoint.position.x + endPoint.position.x,
				                         startPoint.position.y + endPoint.position.y) * 0.5f;

			m_Renderer.transform.position = centerPos;

			// angle
			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
			m_Renderer.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

			//length
			m_Renderer.transform.localScale = new Vector3 (distance / SPRITE_SIZE + 0.0041f, m_Width, m_Renderer.transform.localScale.z);

		}

		private void UpdateColor ()
		{
			m_Renderer.color = m_Color;
		}
	}
}