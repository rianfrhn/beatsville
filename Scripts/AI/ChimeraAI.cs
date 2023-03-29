using Godot;
using System;

public class ChimeraAI : BaseAI
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    Sprite weaponSprite;
    public override void _Ready()
    {
        onReady();
        if (human.atkMode)
        {
            weaponSprite = human.GetNode<Sprite>("Sprite/Weapon");
            weaponSprite.Visible = true;
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
                    Teleport(ToBackofEnemy());
                    break;
                case 1:
                    human.selectedSkill = 1;
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
                    human.Move(AwayFromEnemy());
                    break;
                case 5:
                    human.RegenInspiration();
                    break;
                case 6:
                    human.Move(AwayFromEnemy());
                    break;
                case 7:
                    human.Move(AwayFromEnemy());
                    break;

            }


        }
        
    }
    
}
