using UnityEngine;
using System.Collections;

namespace FractalTree
{
	/// <summary>
	/// Added to the start and end of movable branches. Used to add spring force to a branch.
	/// </summary>
	public class PointMass
	{
		/// <summary>
		/// THe current position of the branch point.
		/// </summary>
		/// <value>The position.</value>
		public Vector2 position { get; set; }

		/// <summary>
		/// Gets the velocity.
		/// </summary>
		/// <value>The velocity.</value>
		public Vector2 velocity { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="FractalTree.PointMass"/> has had a force applied.
		/// </summary>
		/// <value><c>true</c> if force applied; otherwise, <c>false</c>.</value>
		public bool forceApplied { get; private set; }

		/// <summary>
		/// The minimun magnitude required to move the point.
		/// </summary>
		private static readonly float MINIMUM_MAG = 0.001f;

		private float m_InverseMass;
		private Vector2 m_Acceleration;
		private float m_Damping = 0.98f;
		private Vector2 m_TargetPos;
		private float m_BounceBackForce;

		/// <summary>
		/// Initializes a new instance of the <see cref="FractalTree.PointMass"/> class.
		/// </summary>
		/// <param name="position">Initial position.</param>
		/// <param name="invMass">Inverse mass, lower numbers result in more force required to move the point.</param>
		/// <param name="bounceBackForce">Bounce back force. The force applied when moving the spring back to its initial position. </param>
		public PointMass (Vector2 position, 
		                   float invMass, float bounceBackForce)
		{
			this.position = position;
			m_TargetPos = position;
			m_InverseMass = invMass;
			m_BounceBackForce = bounceBackForce;
		}

		/// <summary>
		/// Applies a force.
		/// </summary>
		/// <param name="force">Force.</param>
		public void ApplyForce (Vector2 force)
		{
			m_Acceleration += force * m_InverseMass;
		}

		/// <summary>
		/// Increases the damping factor. This dampens the velocity each step.
		/// </summary>
		/// <param name="factor">Factor.</param>
		public void IncreaseDamping (float factor)
		{
			m_Damping *= factor;
		}

		/// <summary>
		/// Updates position based on current force and distance from initial position.
		/// </summary>
		public void DoUpdate ()
		{
			m_Acceleration += (m_TargetPos - position) * m_BounceBackForce * m_InverseMass;

			velocity += m_Acceleration;
			position += velocity;
			m_Acceleration = Vector2.zero;

			if (velocity.sqrMagnitude < MINIMUM_MAG * MINIMUM_MAG) {
				velocity = Vector2.zero;
			} else {
				velocity *= m_Damping;
			}

			m_Damping = 0.98f;

			forceApplied = velocity != Vector2.zero;
		}
	}
}