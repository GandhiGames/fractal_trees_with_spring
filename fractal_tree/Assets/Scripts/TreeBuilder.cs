using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
    public class TreeBuilder : MonoBehaviour
    {
        public int growthCount = 3;
        public float lengthDegradation = 0.67f;
        public float angle = 45f;

        public GameObject branchPrefab;

        private List<Branch> m_Branches = new List<Branch>();

        private float[] m_Angles;

        void Start()
        {
            m_Angles = new float[2];
            m_Angles[0] = angle;
            m_Angles[1] = -angle;

            Branch.LengthDegradation = lengthDegradation;

            var branch = ((GameObject)Instantiate(branchPrefab)).GetComponent<Branch>();

            var x = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width * 0.5f, 0));
            var y = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width * 0.5f, Screen.height * 0.2f));
            branch.Setup(x, y, 0.01f, Color.white);

            m_Branches.Add(branch);

            branch.transform.SetParent(transform);

            for(int i = 0; i < growthCount; i++)
            {
                for(int j = m_Branches.Count - 1; j >= 0; j--)
                {
                    if(!m_Branches[j].hasBranched)
                    {
                        foreach(var ang in m_Angles)
                        { 
                            var newBranch = m_Branches[j].DoBranching(ang);
                            newBranch.transform.SetParent(transform);
                            m_Branches.Add(newBranch);
                        }

                        m_Branches[j].hasBranched = true;
                    }
                }
            }
        }

        void Update()
        {
			if (Input.GetMouseButtonUp (0)) {
				foreach (var branch in m_Branches) {
					branch.endPoint.ApplyForce(10 * Vector2.down * 1f / (10 + Vector2.Distance(Input.mousePosition, branch.startPoint.position)));
				}
			}
			
            foreach(var branch in m_Branches)
            {
            	branch.DoUpdate();
            }


			foreach(var branch in m_Branches)
			{
				branch.startPoint.DoUpdate();
				branch.endPoint.DoUpdate();
			}
        }

    }
}