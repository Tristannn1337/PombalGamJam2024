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
    public BBParameter<Transform> Target;
    public BBParameter<float> MovementSpeed;
    public BBParameter<float> PathUpdateTime;
    protected override string info => $"Move to {(Target.value != null ? Target.value : "Target")}";

    float updateTimer;
    protected override void OnExecute()
    {
        agent.Pathfinding.CalculatePath(false, Target.value.position);
    }

    protected override void OnUpdate()
    {
        updateTimer += Time.deltaTime;
        if (updateTimer >= PathUpdateTime.value)
        {
            agent.Pathfinding.CalculatePath(false, Target.value.position);
            updateTimer = 0f;
        }


        if (!agent.Pathfinding.PathExists()) return;

        Vector2? nextWaypoint = agent.Pathfinding.GetNextWaypoint();
        if (agent.Pathfinding.ReachedDestination(Target.value.position))
        {
            EndAction(true);
        }
        if (nextWaypoint == null) return;

        agent.transform.position = Vector2.MoveTowards(agent.transform.position, (Vector2)nextWaypoint, MovementSpeed.value * Time.deltaTime);
        Vector2 moveDirection = (Vector2)nextWaypoint - (Vector2)agent.transform.position;
        agent.transform.right = Vector2.MoveTowards(agent.transform.right, moveDirection, 3f * Time.deltaTime);


    }
}
