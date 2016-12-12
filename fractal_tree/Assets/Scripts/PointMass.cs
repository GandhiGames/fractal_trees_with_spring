using UnityEngine;
using System.Collections;

public class PointMass
{
    public Vector2 position;
    public Vector2 velocity;
    public float inverseMass;

    private Vector2 m_Acceleration;
    private float m_Damping = 0.98f;
    private Vector2 m_TargetPos;

    public PointMass(Vector2 position, float invMass)
    {
        this.position = position;
        m_TargetPos = position;
        inverseMass = invMass;
    }

    public void ApplyForce(Vector2 force)
    {
        m_Acceleration += force * inverseMass;
    }

    public void IncreaseDamping(float factor)
    {
        m_Damping *= factor;
    }

    public void DoUpdate()
    {
        m_Acceleration += (m_TargetPos - position) * .5f * inverseMass;

       if (m_Acceleration != Vector2.zero)
        {
            velocity += m_Acceleration;
            position += velocity;
            m_Acceleration = Vector2.zero;

            if (velocity.sqrMagnitude < 0.001f * 0.001f)
            {
                velocity = Vector2.zero;

            }

            velocity *= m_Damping;
            m_Damping = 0.98f;
        }
    }
}
