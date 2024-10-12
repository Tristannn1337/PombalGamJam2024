using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

[Name("Update Path")]
[Category("✫ Fish")]
[Description("Updates the path from A* Pathfinding")]
public class UpdatePathActionTask : ActionTask<Human>
{
    public BBParameter<Transform> Target;
    public BBParameter<float> PathUpdateTime = 0.5f;

    float updateTimer = 0f;

    protected override string info => $"Updates the Path every {PathUpdateTime.value} seconds";
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
    }

}
