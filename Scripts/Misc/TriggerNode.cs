using Godot;
using System;

public class TriggerNode : Area2D
{
	enum Action { ChangeScene, TriggerDialog}

	[Export]
	Action action;
	[Export(PropertyHint.File, "*.tscn")]
	string sceneDir = "";
	[Export(PropertyHint.File, "*.tres")]
	string musicDir = "";
	[Export]
	string dialogue = "";

	public override void _Ready()
	{
		Connect("area_entered", this, "onPlayerEntered");
		
	}
	private void onPlayerEntered(Node body)
	{
		if (!Visible) return;
		if (!(body is Humanoid plr && plr.isPlayer)) return;
		switch (action)
		{
			case Action.TriggerDialog:
				Node dialogueNode = DialogicSharp.Start(dialogue);
				BV.ST.AddChild(dialogueNode);
				break;
			case Action.ChangeScene:
				if (sceneDir == "") return;
				GlobalHandler gv = GetTree().Root.GetNode<GlobalHandler>("GlobalHandler");
				gv.fromScene = GetParent().Name;

				if(sceneDir == "RUN")
				{
					gv.loadPos = true;
					gv.LoadGameData("FightRun");
					
					break;
				}


				SceneTransition st = GetTree().Root.GetNode<SceneTransition>("SceneTransition");
				st.ChangeScene(sceneDir, musicDir, dialogue);
				break;
			default: break;
		}
	}


}
