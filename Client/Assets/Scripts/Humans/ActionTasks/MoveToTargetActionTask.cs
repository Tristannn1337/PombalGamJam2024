using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Name("Move To Target")]
[Category("✫ Fish")]
[Description("Moves the Agent towards a target using Pathfinding")]
public class MoveToTargetActionTask : ActionTask<Human>
{
    protected override string info => $"Move to {(agent.Target != null ? agent.Target : "Target")}";

    protected override void OnUpdate()
    {
        if (!agent.Pathfinding.PathExists()) return;

        Vector2? nextWaypoint = agent.Pathfinding.GetNextWaypoint();
        if(nextWaypoint == null) return;

        agent.transform.position = Vector2.MoveTowards(agent.transform.position, (Vector2)nextWaypoint, agent.MovementSpeed * Time.deltaTime);

        if (agent.Pathfinding.ReachedDestination(agent.Target.position))
        {
            EndAction(true);
        }
    }
}
