using Godot;
using System;

public class QuestGoal : Node
{
	[Export]
	public string goalID;
	[Signal]
	public delegate void Completed(string GoalID);
	[Signal]
	public delegate void Progressed(string GoalID);

	public int progress;
	public bool activated;
	private string connectedSignalName;
	/// <summary>
	/// Initialize the quest goal with bindings
	/// </summary>
	/// <param name="signalBind">What signal (action) from the GlobalVariable will it connect to the Progress() method</param>
	public void Initialize(string signalBind)
	{
		GD.Print("Binded Quest Goal " + goalID + " to signal " + signalBind);
		BV.GH.Connect(signalBind, this, "Progress");
		connectedSignalName = signalBind;
		activated = true;
	}
	public void Deactivate()
	{
		if (activated == false) return;
		activated = false;
		BV.GH.Disconnect(connectedSignalName, this, "Progress");
	}
	public virtual void Progress(string name, int ammount)
	{
		GD.PushWarning("Progress Referenced to base class: QuestGoal. Tru to inherit annd override this method");
		EmitSignal("Progressed", goalID);
	}
	public virtual void Finish()
	{
		GD.Print("Quest with id " + goalID + " finished");
		EmitSignal("Completed", goalID);
		Deactivate();
	}
	public virtual void Activate()
	{
		GD.PushWarning("Activate Referenced to base class: QuestGoal. Tru to inherit annd override this method");
	}
}
