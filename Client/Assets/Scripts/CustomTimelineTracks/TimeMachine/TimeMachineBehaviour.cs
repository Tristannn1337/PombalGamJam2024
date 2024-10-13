using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TimeMachineBehaviour : PlayableBehaviour
{
	public TimeMachineAction action;
	public Condition condition;
	public string markerToJumpTo, markerLabel;
	public float timeToJumpTo;
    public IntroPuppet IntroPuppet;

	[HideInInspector]
	public bool clipExecuted = false; //the user shouldn't author this, the Mixer does

	public bool ConditionMet()
	{
		switch(condition)
		{
			case Condition.Always:
				return true;
				
			case Condition.KeyIsPressed:
				if(IntroPuppet != null)
				{
					return !IntroPuppet.KeyPressed;
				}
				else
				{
					return false;
				}

			case Condition.Never:
			default:
				return false;
		}
	}

	public enum TimeMachineAction
	{
		Marker,
		JumpToTime,
		JumpToMarker,
		//Pause,
	}

	public enum Condition
	{
		Always,
		Never,
		KeyIsPressed,
	}
}
