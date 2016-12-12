using UnityEngine;
using System.Collections;

public class PointMass
{
    public Vector2 position { get; set; }
    public Vector2 velocity { get; private set; }
    public bool forceApplied { get; private set; }

    private static readonly float MINIMUM_MAG = 0.001f;

    private float m_InverseMass;
    private Vector2 m_Acceleration;
    private float m_Damping = 0.98f;
    private Vector2 m_TargetPos;
    private float m_BounceBackForce;
  

    public PointMass(Vector2 position, 
        float invMass, float bounceBackForce)
    {
        this.position = position;
        m_TargetPos = position;
        m_InverseMass = invMass;
        m_BounceBackForce = bounceBackForce;
    }

    public void ApplyForce(Vector2 force)
    {
        m_Acceleration += force * m_InverseMass;
    }

    public void IncreaseDamping(float factor)
    {
        m_Damping *= factor;
    }

    public void DoUpdate()
    {
        m_Acceleration += (m_TargetPos - position) * m_BounceBackForce * m_InverseMass;

        velocity += m_Acceleration;
        position += velocity;
        m_Acceleration = Vector2.zero;

        if (velocity.sqrMagnitude < MINIMUM_MAG * MINIMUM_MAG)
        {
            velocity = Vector2.zero;
        }
        else
        {
            velocity *= m_Damping;
        }

        m_Damping = 0.98f;

        forceApplied = velocity != Vector2.zero;
    }
}
