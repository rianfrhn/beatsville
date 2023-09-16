using Godot;
using System;

public class CutterAI : BaseAI
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	bool belowThresh = false;
	public override void _Ready()
	{

		phase = 0;
		onReady();
	}
	public override void onBeat(int beat)
	{
		if(beat == 0)
        {
			phase = 0;
        }
		if(beat == 32)
		{
			phase = 1;
		}
		if(beat == 96)
        {
			phase = 2;
        }

		if (beat == 160)
		{
			phase = 3;
		}
		if (beat == 224)
		{
			phase = 4;
		}
		if (beat == 256)
		{
			phase = 5;
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
				case 0:
					Phase0(beat);
					break;
				case 1:
					Phase1(beat);
					break;
				case 2:
					Phase2(beat);
					return;
				case 3:
					Phase3(beat);
					return;
				case 4:
					Phase4(beat);
					break;
				case 5:
					Phase5(beat);
					break;
				default: break;
			}

		}
	}
	public void Phase0(int beat)
	{
		if (!canMove(beat)) return;
		int moveBeat = beat % 4;
		switch (moveBeat)
        {
			case 0:
				human.Move(ToEnemy()); break;
			case 1:
				human.Move(ToEnemy()); break;
			case 2:
				human.Move(AwayFromEnemy()); break;
			case 3:
				human.Move(AwayFromEnemy()); break;
		}
	}
	public void Phase1(int beat)
	{
		
		if (canMove(beat))
		{
			int moveBeat = beat % 8;
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
					human.selectedSkill = 0;
					TryAttack();
					break;
				case 4:
					human.Move(ToEnemy());
					break;
				case 5:
					human.selectedSkill = 0;
					TryAttack();
					break;
				case 6:
					human.RegenInspiration();
					break;
				case 7:
					human.selectedSkill = 0;
					TryAttack();
					break;

			}


		}

		
		if(human.GetHealth() < human.GetMaxHealth() * 0.15f && !belowThresh)
		{
			belowThresh = true;
			BV.ST.ChangeScene("res://Scenes/Map/BeatsvilleMap.tscn", "", "/VillageEpilogue1/Village2Cutter/SparkyInterrupts");
			if (DialogicSharp.GetVariable("FoughtCutter") != "2")
				DialogicSharp.SetVariable("FoughtCutter", "1");
		}
		if(enemy.GetHealth() < enemy.GetMaxHealth() * 0.15f && !belowThresh)
        {
			belowThresh = true;
			BV.ST.ChangeScene("res://Scenes/Map/BeatsvilleMap.tscn", "", "/VillageEpilogue1/Village2Cutter/SparkyInterrupts");
			if (DialogicSharp.GetVariable("FoughtCutter") != "2")
				DialogicSharp.SetVariable("FoughtCutter", "1");
		}
		
	}
	public void Phase2(int beat)
	{
		Phase1(beat);
	}
	public void Phase3(int beat)
    {
		if (!canMove(beat)) return;
		int moveBeat = beat % 16;
		switch (moveBeat)
		{
			case 0:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 1:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 2:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 3:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 4:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 5:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 6:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 7:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 8:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 9:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 10:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 11:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 12:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 13:
				human.RegenInspiration();
				break;
			case 14:
				human.RegenInspiration();
				break;
			case 15:
				human.Move(ToEnemy());
				break;
		}
	}
	public void Phase4(int beat)
	{
		Phase1(beat);
	}
	public void Phase5(int beat)
	{
		if (!canMove(beat)) return;
		int moveBeat = beat % 8;
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
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 3:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 4:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 5:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 6:
				human.selectedSkill = 0;
				TryAttack();
				break;
			case 7:
				human.RegenInspiration();
				break;
		}
	}
}
