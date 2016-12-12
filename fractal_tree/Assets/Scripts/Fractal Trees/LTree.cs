using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
    public class LTree : Tree
    {
        private int m_Steps;
        private LRule[] m_Rules;
        private string m_Seed;
        private string m_Sentence;
        private GameObject m_BranchPrefab;
        private float m_Length;
        private float m_Angle;
        private Transform m_Owner;
        private bool m_IsStatic;

        public LTree(GameObject branchPrefab, 
            int steps,
            string axiom,
            LRule[] rules,
            float initialLength,
            float angle, Transform owner, bool isStatic)
        {
            m_BranchPrefab = branchPrefab;
            m_Steps = steps;
            m_Seed = axiom;
            m_Rules = rules;
            m_Sentence = m_Seed;
            m_Length = initialLength;
            m_Angle = angle;

            m_Owner = owner;
            m_IsStatic = isStatic;
        }

        public List<Branch> Generate()
        {
            var branches = new List<Branch>();

            for (int i = 0; i < m_Steps; i++)
            {
                CalculateNextStep();
 
            }

            return CreateTree();
        }

        private void CalculateNextStep()
        {
            string nextSentence = "";

            for (int i = 0; i < m_Sentence.Length; i++)
            {
                var currentChar = m_Sentence[i];

                bool matched = false;

                for (int j = 0; j < m_Rules.Length; j++)
                {
                    if (currentChar.Equals(m_Rules[j].from))
                    {
                        nextSentence += m_Rules[j].to;
                        matched = true;
                        break;
                    }
                }

                if(!matched)
                {
                    nextSentence += currentChar;
                }
            }

            m_Sentence = nextSentence;
        }

        private List<Branch> CreateTree()
        {
            Vector2 current = m_Owner.transform.position;

            float rotation = 0f;

            var storedStates = new Stack<LMovementState>();

            var branchesCreated = new List<Branch>();

            Branch previousBranch = null;

            for (int i = 0; i < m_Sentence.Length; i++)
            {
                var currentChar = m_Sentence[i];

                if(currentChar.Equals('F'))
                {
                    var branch = ((GameObject)MonoBehaviour.Instantiate(m_BranchPrefab)).GetComponent<Branch>();

                    Vector2 next = current
                        + (Vector2.up * m_Length).Rotate(rotation);

                    if(previousBranch == null)
                    {
                        previousBranch = branch;
                        branch.Setup(current, next, 0.01f, Color.white);
                    }
                    else
                    {
                        branch.Setup(previousBranch, next, 0.01f, Color.white);
                    }

                    previousBranch = branch;

                    branch.transform.SetParent(m_Owner);
                    branchesCreated.Add(branch);

                    current = next;
                }
                else if (currentChar.Equals('+'))
                {
                    rotation -= m_Angle;
                }
                else if (currentChar.Equals('-'))
                {
                    rotation += m_Angle;
                }
                else if (currentChar.Equals('['))
                {
                    storedStates.Push(new LMovementState(current, 
                        rotation, previousBranch));
                }
                else if(currentChar.Equals(']'))
                {
                    var storedState = storedStates.Pop();
                    current = storedState.position;
                    rotation = storedState.rotation;
                    previousBranch = storedState.previousBranch;
                }
            }

            return branchesCreated;
        }

        private class LMovementState
        {
            public Vector2 position { get; private set; }
            public float rotation { get; private set; }
            public Branch previousBranch { get; private set; }

            public LMovementState(Vector2 position, 
                float rotation, Branch previousBranch)
            {
                this.position = position;
                this.rotation = rotation;
                this.previousBranch = previousBranch;
            }
        }
    }

    [System.Serializable]
    public class LRule
    {
        public char from;
        public string to;

        public LRule(char from, string to)
        {
            this.from = from;
            this.to = to;
        }
    }


}
