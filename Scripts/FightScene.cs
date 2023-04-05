using Godot;
using System;

public class FightScene : Node2D
{

	/// <summary>
	/// Initialize the fight song with its bpm etc. To make resource, add resource in project, attach songdata as script.
	/// </summary>
	public Resource songdata;

	/// <summary>
	/// Bossfight condition. If true, player cant run.
	/// </summary>
	public bool IsBossfight;


	private float songOffset;
	private float songInterval;
	public Humanoid player1Instance;
	public Humanoid player2Instance;
	public FightData fightData;
	public GlobalVariable gv;

	public bool fightFinished = false;
	
	public override void _Ready()
	{
		//Initialize Variables
		gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
		songOffset = gv.songOffset;
		songInterval = gv.interval;

		fightData = (FightData)gv.currentFight;
		songdata = fightData.songData;
		IsBossfight = fightData.isBossFight;


		//Song initialization
		MusicHandler musicHandler = GetNode<MusicHandler>("MusicController");
		musicHandler.musicOffset = songOffset;
		musicHandler.interval = songInterval;
		musicHandler.songDataRes = songdata;

		//Load Characters

		player1Instance = fightData.player1.Instance<Humanoid>();
		player2Instance = fightData.player2.Instance<Humanoid>();

		player1Instance.atkMode = true;

		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();
		player1Instance.Position = new Vector2(72, 40 + 16 * rng.RandiRange(0,7));
		player1Instance.atkMode = true;
		rng.Randomize();
		player2Instance.Position = new Vector2(248, 40 + 16 * rng.RandiRange(0, 7));
		player2Instance.faceLeft = true;
		player2Instance.atkMode = true;
		AddChild(player1Instance);
		AddChild(player2Instance);

		musicHandler.Connect("SkippedBeat", player1Instance, "RegenInspiration");
		//musicHandler.Connect("SkippedBeat", player2Instance, "RegenInspiration");

		//Healthbar
		HealthBar healthbar1 = GetNode<HealthBar>("HealthBar");
		player1Instance.Connect("HealthChanged", healthbar1, "onHealthChanged");
		player1Instance.Connect("InspirationChanged", healthbar1, "onInspirationChanged");
		player1Instance.Connect("Defeated", this, "onPlayer1Defeat");


		//if (IsBossfight)
		//{
		//HealthBar healthbar2 = ResourceLoader.Load<HealthBar>("res://Scenes/UI/HealthBar.tscn")


		//}
		HealthBar healthbar2 = GetNode<HealthBar>("HealthBar2");
		player2Instance.Connect("HealthChanged", healthbar2, "onHealthChanged");
		player2Instance.Connect("InspirationChanged", healthbar2, "onInspirationChanged");
		player2Instance.Connect("Defeated", this, "onPlayer2Defeat");
		DialogicSharp.SetVariable("GotXP", fightData.xpGain.ToString());
		DialogicSharp.SetVariable("GotMoney", fightData.moneyGain.ToString());





	}
	public Humanoid getPlayer1()
	{
		return player1Instance;
	}
	public Humanoid getPlayer2()
	{
		return player2Instance;
	}

	public void onPlayer1Defeat()
	{
		if (fightFinished) return;
		GD.Print("Player 1 Defeated");
		Node dialogue = DialogicSharp.Start("FightLost");
		GetTree().Root.AddChild(dialogue);
		fightFinished = true;
	}
	public void onPlayer2Defeat()
	{
		if (fightFinished) return;
		GD.Print("player 2 defeated");
		gv.FinishFight();
		fightFinished = true;
	}
}
