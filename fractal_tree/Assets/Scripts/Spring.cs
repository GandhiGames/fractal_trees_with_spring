using UnityEngine;

public class Spring : MonoBehaviour
{
    public PointMass start;
    public PointMass end;
    public float targetLength;
    public float stiffNess;
    public float damping;

    public void Setup(PointMass start, PointMass end, 
        float stiffness, float damping)
    {
        this.start = start;
        this.end = end;
        this.stiffNess = stiffness;
        this.damping = damping;
        targetLength = Vector2.Distance(start.position, end.position) * 0.95f;
    }

    public void DoUpdate()
    {
        var x = start.position - end.position;

        float length = x.magnitude;
        // these springs can only pull, not push
        if (length <= targetLength)
            return;

        x = (x / length) * (length - targetLength);
        var dv = end.velocity - start.velocity;
        var force = stiffNess * x - dv * damping;

        start.ApplyForce(-force);
        end.ApplyForce(force);
    }

    
}