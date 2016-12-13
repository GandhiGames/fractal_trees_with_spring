using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
    public class StationaryBranch : MonoBehaviour, Branch
    {
        public static float LengthDegradation = 0.67f;

        public Vector2 startPos { get; private set; }
        public Vector2 endPos { get; private set; }
        public bool hasBranched { get; set; }
        public Color color
        {
            set
            {
                UpdateColor(value);
            }
        }

        private static readonly float SPRITE_SIZE = 100f / 100f; // pixels of line sprite / pixels per units.
        private float m_Thickness;
        private SpriteRenderer m_Renderer;

        void Awake()
        {
            m_Renderer = GetComponent<SpriteRenderer>();
        }

        public void Setup(Branch owner, Vector2 end,
           float thickness, Color color)
        {
            Setup(owner.endPos, end, thickness, color, false);
        }

        public void Setup(Branch owner, Vector2 end,
                float thickness, Color color, bool autoMass)
        {
            Setup(owner.endPos, end, thickness, color, autoMass);
        }

        public void Setup(Vector2 start, Vector2 end,
            float thickness, Color color)
        {
            Setup(start, end, thickness, color, false);
        }

        public void Setup(Vector2 start, Vector2 end,
            float thickness, Color color, bool autoMass)
        {
            this.m_Thickness = thickness;
            startPos = start;
            endPos = end;

            UpdateSprite();
            UpdateColor(color);
        }

        public T DoBranching<T>(float angle) where T : Branch
        {
            var newBranch = ((GameObject)Instantiate(gameObject)).GetComponent<T>();

            var dir = (endPos - startPos) * LengthDegradation;
            var dirRot = dir.Rotate(angle);
            var newEnd = endPos + dirRot;

            newBranch.Setup(this, newEnd, this.m_Thickness, m_Renderer.color);

            return newBranch;
        }


        private void UpdateSprite()
        {
            var heading = endPos - startPos;
            var distance = heading.magnitude;
            var direction = heading / distance;

            var centerPos = new Vector2(
                startPos.x + endPos.x,
                startPos.y + endPos.y) * 0.5f;

            m_Renderer.transform.position = centerPos;

            // angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            m_Renderer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //length
            m_Renderer.transform.localScale = new Vector3(distance / SPRITE_SIZE + 0.0041f, m_Thickness, m_Renderer.transform.localScale.z);

        }

        private void UpdateColor(Color color)
        {
            m_Renderer.color = color;
        }
    }
}