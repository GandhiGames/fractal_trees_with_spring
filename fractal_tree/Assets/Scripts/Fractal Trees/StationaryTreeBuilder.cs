using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{
    public class StationaryTreeBuilder : TreeBuilder
    {
      

        private List<Branch> m_Branches = new List<Branch>();

        void Start()
        {
            // m_Branches.AddRange().Generate());

            Tree tree = CreateTree();



            if (tree != null)
            {
                m_Branches.AddRange(tree.Generate<Branch>());
            }
        }
    }
}