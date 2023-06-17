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
		Connect("area_entered", this, "onPlayerEntered");
		
	}
	private void onPlayerEntered(Node body)
	{
		if (!(body is Humanoid plr && plr.isPlayer)) return;
		if (parameter == "") return;
		switch (action)
		{
			case Action.TriggerDialog:
				DialogicSharp.Start(parameter);
				break;
			case Action.ChangeScene:
				GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
				gv.fromScene = GetParent().Name;

				if(parameter == "RUN")
				{
					gv.loadPos = true;
					gv.LoadGameData("FightRun");
					
					break;
				}


				SceneTransition st = GetTree().Root.GetNode<SceneTransition>("SceneTransition");
				st.ChangeScene(parameter, parameter2, parameter3);
				break;
			default: break;
		}
	}


}
