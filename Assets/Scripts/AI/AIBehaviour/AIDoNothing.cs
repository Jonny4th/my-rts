using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDoNothing : AIBehaviour
{
    private float returnWeight = 0.5f;

    public override void Execute()
    {
        //Do nothing.
    }

    public override float GetWeight()
    {
        return returnWeight;
    }
}
