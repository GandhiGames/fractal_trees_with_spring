using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Spring))]
    public class Branch : MonoBehaviour
    {
        public static float LengthDegradation = 0.67f;

        public Vector2 start { get { return m_PointMass[0].position; } }
        public Vector2 end { get { return m_PointMass[1].position; } }
        public float thickness;
        public Color color;
        public Branch owner;
        public bool hasBranched { get; set; }

        private SpriteRenderer m_Renderer;

        private Vector2 m_PreviousStart;
        private Vector2 m_PreviousEnd;

        private Spring m_Spring;
        private PointMass[] m_PointMass;

        void Awake()
        {
            m_Renderer = GetComponent<SpriteRenderer>();
            m_Spring = GetComponent<Spring>();
        }

        public void Setup(Branch owner, Vector2 end, float thickness, Color color)
        {
            this.owner = owner;
            Setup(owner.start, end, thickness, color);
        }

        public void Setup(Vector2 start, Vector2 end, float thickness, Color color)
        {
            this.thickness = thickness;
            this.color = color;

            m_PointMass = new PointMass[2];
            m_PointMass[0] = new PointMass(start, 1f);
            m_PointMass[1] = new PointMass(end, 1f);

            m_Spring.Setup(m_PointMass[0], m_PointMass[1], 0.002f, 0.02f);

            UpdateSprite();
            UpdateColor();
        }

        public Branch DoBranching(float angle)
        {
            var newBranch = ((GameObject)Instantiate(gameObject)).GetComponent<Branch>();

            var dir = (end - start) * LengthDegradation;
            var dirRot = dir.Rotate(angle);
            var newEnd = end + dirRot;

            newBranch.Setup(this, newEnd, this.thickness, this.color);

            return newBranch;
        }

        public void DoUpdate()
        {
            if (owner != null)
            {
                m_PointMass[0].position = owner.end;
            }

            m_Spring.DoUpdate();

            foreach(var point in m_PointMass)
            {
                point.Update();
            }

            // position not updated this step.
            if (m_PreviousStart != start || m_PreviousEnd != end) 
            {
                UpdateSprite();
            }


            UpdateColor();

        
        }

        private void UpdateSprite()
        {
            m_PreviousStart = start;
            m_PreviousEnd = end;

            var heading = end - start;
            var distance = heading.magnitude;
            var direction = heading / distance;

            //Debug.Log("Start: " + start);
            Vector3 centerPos = new Vector3(start.x + end.x, start.y + end.y) / 2;
            m_Renderer.transform.position = centerPos;

            // angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            m_Renderer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //length
            var objectWidthSize = 10f / 5f; // 10 = pixels of line sprite, 5f = pixels per units of line sprite.
            m_Renderer.transform.localScale = new Vector3(distance / objectWidthSize + 0.0041f, thickness, m_Renderer.transform.localScale.z);
 
        }

        private void UpdateColor()
        {
            m_Renderer.color = color;
        }

        void Update()
        {
            //end += new Vector2(Random.Range(-.01f, .01f), Random.Range(-.01f, .01f));
        }
    }
}