using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

[Category("✫ Fish")]
public class DistanceToPlayerConditionTask : ConditionTask<Human>
{
    public BBParameter<float> distance = 0f;
    public CompareMethod comparison = CompareMethod.EqualTo;
    protected override string info
    {
        get { return $"Distance to Player is {OperationTools.GetCompareString(comparison)} {distance.value}"; }
    }

    protected override bool OnCheck()
    {
        if (agent.Fish == null) return false;

        var d = Vector3.Distance(agent.transform.position, agent.Fish.transform.position);
        return OperationTools.Compare((float)d, (float)distance.value, comparison, 0f);
    }
}