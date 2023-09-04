using Godot;
using System;

public class TriggerNode : Interactable
{
	enum Action { ChangeScene, TriggerDialog}
	[Export]
	public bool onClick = false;
	[Export]
	Action action;
	[Export(PropertyHint.File, "*.tscn")]
	string sceneDir = "";
	[Export(PropertyHint.File, "*.tres")]
	string musicDir = "";

	public override void _Ready()
	{
		Connect("area_entered", this, "onPlayerEntered");
		
	}
	private void onPlayerEntered(Node body)
	{
		if (!Visible) return;
		if (onClick) return;
		if (!(body is Humanoid plr && plr.isPlayer)) return;
		switch (action)
		{
			case Action.TriggerDialog:
				Node dialogueNode = DialogicSharp.Start(dialogue);
				BV.ST.AddChild(dialogueNode);
				break;
			case Action.ChangeScene:
				if (sceneDir == "") return;
				BV.GH.fromScene = GetParent().Name;

				if(sceneDir == "RUN")
				{
					BV.GH.loadPos = true;
					BV.GH.LoadGameData("FightRun");
					
					break;
				}
				BV.ST.ChangeScene(sceneDir, musicDir, dialogue);
				break;
			default: break;
		}
	}
	public override void OpenDialogue(Node2D castSource)
	{
		if (!Visible) return;
		if (!onClick) return;
		switch (action)
		{
			case Action.TriggerDialog:
				Node dialogueNode = DialogicSharp.Start(dialogue);
				BV.ST.AddChild(dialogueNode);
				break;
			case Action.ChangeScene:
				if (sceneDir == "") return;
				BV.GH.fromScene = GetParent().Name;

				if (sceneDir == "RUN")
				{
					BV.GH.loadPos = true;
					BV.GH.LoadGameData("FightRun");

					break;
				}

				BV.ST.ChangeScene(sceneDir, musicDir, dialogue);
				break;
			default: break;
		}

	}


}
