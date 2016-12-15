using UnityEngine;

namespace FractalTree
{
	/// <summary>
	/// Connects two point masses and apllies a pull force to ensure points stay within a target length.
	/// </summary>
	public class Spring : MonoBehaviour
	{
		/// <summary>
		/// The start point mass.
		/// </summary>
		public PointMass start;

		/// <summary>
		/// The end point mass.
		/// </summary>
		public PointMass end;

		private float m_TargetLength;
		private float m_StiffNess;
		private float m_Damping;

		/// <summary>
		/// Setup the specified start, end, stiffness and damping. 
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="stiffness">Stiffness.</param>
		/// <param name="damping">Damping.</param>
		public void Setup (PointMass start, PointMass end, 
		                    float stiffness, float damping)
		{
			this.start = start;
			this.end = end;
			this.m_StiffNess = stiffness;
			this.m_Damping = damping;
			m_TargetLength = Vector2.Distance (start.position, end.position) * 0.95f;
		}

		/// <summary>
		/// Applies force to start and point based on distance between points.
		/// </summary>
		public void DoUpdate ()
		{
			var x = start.position - end.position;

			float length = x.magnitude;

			x = (x / length) * (length - m_TargetLength);
			var dv = end.velocity - start.velocity;
			var force = m_StiffNess * x - dv * m_Damping;

			start.ApplyForce (-force);
			end.ApplyForce (force);
		}
	}
}