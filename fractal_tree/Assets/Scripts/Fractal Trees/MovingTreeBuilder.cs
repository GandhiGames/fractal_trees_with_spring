﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
    public interface Tree
    {
        List<T> Generate<T>() where T : Branch;
    }

    public class MovingTreeBuilder : TreeBuilder
    {

        private List<MovingBranch> m_Branches = new List<MovingBranch>();

        void Start()
        {

            Tree tree = CreateTree();

            if (tree != null)
            {
                m_Branches.AddRange(tree.Generate<MovingBranch>());
            }
        }


        public void ApplyDirectedForce(Vector2 force, Vector2 position, float radius)
        {
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

