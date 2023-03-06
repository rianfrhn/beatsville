using Godot;
using System;

public class TriggerNode : Area2D
{
	enum Action { ChangeScene, TriggerDialog}

	[Export]
	Action action;
	[Export]
	string parameter = "";
	[Export]
	string parameter2 = "";
	[Export]
	string parameter3 = "";

	public override void _Ready()
	{
		Connect("body_entered", this, "onPlayerEntered");
		
	}
	private void onPlayerEntered(Node body)
	{
		if (parameter == "") return;
		switch (action)
		{
			case Action.TriggerDialog:
				DialogicSharp.Start(parameter);
				break;
			case Action.ChangeScene:
				GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
				gv.fromScene = GetParent().Name;

				if(parameter == "HELD")
				{
					parameter = DialogicSharp.GetVariable("HeldScene");
					gv.loadPos = true;
				}
				if(parameter2 == "HELD")
				{
					parameter2 = DialogicSharp.GetVariable("HeldMusic");
				}


				SceneTransition st = GetTree().Root.GetNode<SceneTransition>("SceneTransition");
				st.ChangeScene(parameter, parameter2, parameter3);
				break;
			default: break;
		}
	}


}
