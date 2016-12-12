using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Spring))]
    public class Branch : MonoBehaviour
    {
        public static float LengthDegradation = 0.67f;
		public PointMass startPoint { get { return m_Spring.start; } }
		public PointMass endPoint { get { return m_Spring.end; } }

        public float thickness;
        public Color color
        {
            set
            {
                m_Color = value;
                m_ColorUpdateRequired = true;
            }
        }

        public Branch owner;
        
		public bool hasBranched { get; set; }

        private static readonly float SPRITE_SIZE = 100f / 100f; // 10 = pixels of line sprite, 5f = pixels per units of line sprite.

        private SpriteRenderer m_Renderer;
        private Spring m_Spring;
        private Color m_Color;
        private bool m_ColorUpdateRequired = false;

        void Awake()
        {
            m_Renderer = GetComponent<SpriteRenderer>();
            m_Spring = GetComponent<Spring>();
        }

        public void Setup(Branch owner, Vector2 end, 
            float thickness, Color color)
        {
            this.owner = owner;
           
            Setup(owner.endPoint.position, end, thickness, color);
        }

        public void Setup(Vector2 start, Vector2 end, 
            float thickness, Color color)
        {
            this.thickness = thickness;
            this.m_Color = color;

            const float stiffness = 0.1f;
            const float damping = 0.96f;
            const float stationary = 0f;
            const float moveable = 0.1f;
            const float bounceBackForce = 0.5f;

			m_Spring.Setup(new PointMass(start, stationary, bounceBackForce), 
				new PointMass(end, moveable, bounceBackForce), stiffness, damping);

            UpdateSprite();
            UpdateColor();
        }

        public Branch DoBranching(float angle)
        {
            var newBranch = ((GameObject)Instantiate(gameObject)).GetComponent<Branch>();

			var dir = (endPoint.position - startPoint.position) * LengthDegradation;
            var dirRot = dir.Rotate(angle);
			var newEnd = endPoint.position + dirRot;

            newBranch.Setup(this, newEnd, this.thickness, this.m_Color);

            return newBranch;
        }

        void Update()
        {
        
            if (owner != null)
            {
				startPoint.position = owner.endPoint.position;
                
            }

            startPoint.DoUpdate();
            endPoint.DoUpdate();

            if(startPoint.forceApplied || endPoint.forceApplied)
            {
                m_Spring.DoUpdate();
                UpdateSprite();
            }

            if (m_ColorUpdateRequired)
            {
                UpdateColor();
                m_ColorUpdateRequired = false;
            }

        
        }

        private void UpdateSprite()
        {
			var heading = endPoint.position - startPoint.position;
            var distance = heading.magnitude;
            var direction = heading / distance;

            var centerPos = new Vector2(
				startPoint.position.x + endPoint.position.x, 
				startPoint.position.y + endPoint.position.y) * 0.5f;

            m_Renderer.transform.position = centerPos;

            // angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            m_Renderer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //length
            m_Renderer.transform.localScale = new Vector3(distance / SPRITE_SIZE + 0.0041f, thickness, m_Renderer.transform.localScale.z);
 
        }

        private void UpdateColor()
        {
            m_Renderer.color = m_Color;
        }

    }
}