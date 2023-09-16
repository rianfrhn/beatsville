using Godot;
using System;

public class BeatsvilleMap : MapScene
{
	public override void _Ready()
	{
		base._Ready();
		Humanoid cutter = BV.WM.CharacterEnter(References.Char.Cutter, new Vector2(1256,312), false);
		cutter.dialogue = "CutterFights";
		if(DialogicSharp.GetVariable("FoughtCutter") == "1")
		{
			BV.WM.CharacterTeleport("Player", new Vector2(1240, 312), true);
			BV.WM.CharacterTeleport("Cutter", new Vector2(1256, 312), true);
			Node dialogue = DialogicSharp.Start("/VillageEpilogue1/Village2Cutter/SparkyQuestionsAzka");
			AddChild(dialogue);
		}
		onUpdate();
		
	}
	public override void onUpdate()
	{
		if(DialogicSharp.GetVariable("VillagersIntroduction") == "1")
		{
			Humanoid sparky = BV.WM.CharacterEnter(References.Char.Sparky, new Vector2(760, 248), true);
			Humanoid khel = BV.WM.CharacterEnter(References.Char.Khel, new Vector2(728, 248), false);
			Humanoid marione = BV.WM.CharacterEnter(References.Char.Marione, new Vector2(344, 136), false);
			Humanoid cutter = BV.WM.CharacterEnter(References.Char.Cutter, new Vector2(1256, 312), true);
			Humanoid adelle = BV.WM.CharacterEnter(References.Char.Adelle, new Vector2(920, 120), false);
			Humanoid rai = BV.WM.CharacterEnter(References.Char.Rai, new Vector2(616, 121), true);
			Humanoid lazarus = BV.WM.CharacterEnter(References.Char.Lazarus, new Vector2(488, 312), false);


			sparky.dialogue = "IntroSparky";
			khel.dialogue = "IntroKhel";
			marione.dialogue = "IntroMarione";
			adelle.dialogue = "IntroAdelle";
			cutter.dialogue = "IntroCutter";
			rai.dialogue = "IntroRai";
			lazarus.dialogue = "IntroLazarus";


		}
	}
}
