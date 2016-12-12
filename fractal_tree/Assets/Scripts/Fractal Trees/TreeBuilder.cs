using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
    public interface Tree
    {
        List<Branch> Generate();
    }

    public class TreeBuilder : MonoBehaviour
    {
        public bool isStatic = false;

        public enum TreeType
        {
            Default,
            LTree
        }
        public TreeType treeType = TreeType.Default;
        public GameObject branchPrefab;

        [Header("Default Tree")]
        public int growthCount = 3;
        public float initialLength = 1f;
        public float lengthDegradation = 0.67f;
        public float angle = 45f;
        public float thickness = 0.4f;

        [Header("L Tree")]
        public int steps = 4;
        public string axiom = "F";
        public LRule[] rules = new LRule[]{
            // F = forward
            // + = rotate right
            // - = rotate left
            // [ = save state
            // ] = restore state

            new LRule('F', "FF+[+F-F-F]-[-F+F+F]")
        };
        public float lTreeBranchLength = 0.2f;
        public float lTreeAngle = 25f;

        private List<Branch> m_Branches = new List<Branch>();

        void Start()
        {
            // m_Branches.AddRange().Generate());

            Tree tree = null;

            switch (treeType)
            {
                case TreeType.Default:
                   tree = new DefaultTree(growthCount, initialLength,
                                lengthDegradation, angle, thickness,
                                branchPrefab, transform);
                    break;
                case TreeType.LTree:
                    tree = new LTree(branchPrefab, steps,
                                axiom, rules, lTreeBranchLength, lTreeAngle, transform, isStatic);
                    break;
            }

           
            m_Branches.AddRange(tree.Generate());
        }


        public void ApplyDirectedForce(Vector2 force, Vector2 position, float radius)
        {
            if(isStatic)
            {
                return;
            }

            foreach (var branch in m_Branches)
            {
                float distance = Vector2.Distance(position, branch.endPoint.position);

                if (distance < radius)
                {
                    branch.endPoint.ApplyForce(force / (distance * 4f));
                }
            }
        }

        public void ApplyPushForce(float force, Vector2 position, float radius)
        {
            if (isStatic)
            {
                return;
            }

            foreach (var branch in m_Branches)
            {
                float distance = Vector2.Distance(position, branch.endPoint.position);

                if (distance < radius)
                {
                    Vector2 dir = (branch.endPoint.position - position).normalized;
                    branch.endPoint.ApplyForce((force * dir) / (distance * 4f));
                    branch.endPoint.IncreaseDamping(0.5f);
                }
            }
        }

        public void ApplyPullForce(float force, Vector2 position, float radius)
        {
            if (isStatic)
            {
                return;
            }

            foreach (var branch in m_Branches)
            {
                float distance = Vector2.Distance(position, branch.endPoint.position);

                if (distance < radius)
                {
                    branch.endPoint.ApplyForce(force * (position - branch.endPoint.position) / (distance * 4f));
                    branch.endPoint.IncreaseDamping(0.6f);
                }
            }
        }

    }
}

