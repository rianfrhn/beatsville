using Godot;
using System;

public class SKalAI : BaseAI
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	bool belowThresh = false;
	public override void _Ready()
	{
		onReady();
	}
	public override void onBeat(int beat)
	{
		if(beat == 6)
		{
			phase = 1;
		}
		if (human.GetInspirationCount() == 0)
		{
			regenning = true;
		}
		if (regenning)
		{
			if (!RegenPhase()) regenning = false;
			else return;
		}
		if (canMove(beat))
		{
			switch (phase)
			{
				case 1:
					Phase1(beat);
					return;
				default: return;
			}

		}
	}
	public void Phase0(int beat)
	{

	}
	public void Phase1(int beat)
	{
		
		if (canMove(beat))
		{
			int moveBeat = beat % 6;
			switch (moveBeat)
			{
				case 0:
					human.Move(ToEnemy());
					break;
				case 1:
					human.selectedSkill = 0;
					TryAttack();
					break;
				case 2:
					human.Move(ToEnemy());
					break;
				case 3:
					human.Move(ToEnemy());
					break;
				case 4:
					TryAttack();
					break;
				case 5:
					human.RegenInspiration();
					break;

			}


		}
		if(human.GetHealth() < 100 && !belowThresh)
		{
			belowThresh = true;
			BV.ST.ChangeScene("res://Scenes/Map/VillageInside/HouseAdelle.tscn", "", "ShadowFightWon");
			if (DialogicSharp.GetVariable("FoughtShadow") != "2")
				DialogicSharp.SetVariable("FoughtShadow", "1");
		}
		if(enemy.GetHealth() < 200 && !belowThresh)
        {
			belowThresh = true;
			BV.ST.ChangeScene("res://Scenes/Map/VillageInside/HouseAdelle.tscn", "", "ShadowFightLost");
			if (DialogicSharp.GetVariable("FoughtShadow") != "2")
				DialogicSharp.SetVariable("FoughtShadow", "1");
		}
		
	}
	
}
