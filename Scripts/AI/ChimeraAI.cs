using Godot;
using System;

public class ChimeraAI : BaseAI
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	[Export]
	bool showWeaponOnAttackMode = false;
	Sprite weaponSprite;
	public override void _Ready()
	{
		onReady();
		if (human.atkMode && showWeaponOnAttackMode)
		{
			weaponSprite = human.GetNode<Sprite>("Sprite/Weapon");
			weaponSprite.Visible = true;
		}
		else
		{
			if (weaponSprite != null) weaponSprite.Visible = false;
		}
	}
	public override void onBeat(int beat)
	{
		if(beat == 64)
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
			int moveBeat = beat % 8;
			switch (moveBeat)
			{
				case 0:
					//Teleport(ToBackofEnemy());
					human.selectedSkill = 0;
					TryAttack();
					break;
				case 1:
					TryAttack();
					break;
				case 2:
					human.Move(ToEnemy());
					break;
				case 3:
					//human.selectedSkill = 0;
					human.Move(ToEnemy());
					break;
				case 4:
					TryAttack();
					break;
				case 5:
					human.Move(AwayFromEnemy());
					break;
				case 6:
					human.RegenInspiration();
					break;
				case 7:
					human.RegenInspiration();
					break;

			}


		}
		
	}
	
}
