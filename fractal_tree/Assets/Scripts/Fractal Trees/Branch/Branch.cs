using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FractalTree
{

    public interface Branch 
    {
        Vector2 startPos { get; }
        Vector2 endPos { get; }

        bool hasBranched { get; set; }
        Transform transform { get; }

        void Setup(Branch owner, Vector2 end,
            float thickness, Color color);

        void Setup(Branch owner, Vector2 end,
                float thickness, Color color, bool autoMass);

        void Setup(Vector2 start, Vector2 end,
             float thickness, Color color);

        void Setup(Vector2 start, Vector2 end,
            float width, Color color, bool autoMass);


        T DoBranching<T>(float angle) where T : Branch;
    }

    public interface MovingBranch : Branch
    {
        PointMass startPoint { get; }
        PointMass endPoint { get; }
    }

}