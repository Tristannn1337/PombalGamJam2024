using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Category("✫ Fish")]
public class ObstacleConditionTask : ConditionTask<Human>
{
    public BBParameter<LayerMask> obstacleLayer = default;
    public BBParameter<float> distance = 0f;
    protected override string info
    {
        get { return $"Doesn't have obstacle in player direction"; }
    }

    protected override bool OnCheck()
    {
        if (agent.FishTransform == null) return false;

        Vector2 raycastDirection = (agent.FishTransform.position - agent.transform.position).normalized;
        var raycastHit2D = Physics2D.Raycast(agent.transform.position, raycastDirection, distance.value, obstacleLayer.value);

        return raycastHit2D.transform != null;
    }
}
