using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
	/// <summary>
	/// Extension methods used by the Fractal Tree generator.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Rotate the specified vector by degrees.
		/// </summary>
		/// <param name="v">V.</param>
		/// <param name="degrees">Degrees.</param>
		public static Vector2 Rotate (this Vector2 v, float degrees)
		{
			float radians = degrees * Mathf.Deg2Rad;
			float sin = Mathf.Sin (radians);
			float cos = Mathf.Cos (radians);

			float tx = v.x;
			float ty = v.y;

			return new Vector2 (cos * tx - sin * ty, sin * tx + cos * ty);
		}
	}
}