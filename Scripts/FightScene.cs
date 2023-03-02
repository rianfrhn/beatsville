using Godot;
using System;

public class FightScene : Node2D
{
	/// <summary>
	/// Initialize the player nodes
	/// </summary>
	public string player1, player2;

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
	private Humanoid player1Instance;
	private Humanoid player2Instance;
	
	public override void _Ready()
	{
		//Initialize Variables
		GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
		songOffset = gv.songOffset;
		songInterval = gv.interval;
		player1 = gv.player1dir;
		player2 = gv.player2dir;
		songdata = ResourceLoader.Load(gv.songdatadir);
		IsBossfight = gv.IsBossfight;


		//Song initialization
		MusicHandler musicHandler = GetNode<MusicHandler>("MusicController");
		musicHandler.musicOffset = songOffset;
		musicHandler.interval = songInterval;
		musicHandler.songData = (SongData)songdata;

		//Load Characters
		PackedScene pspl1 = (PackedScene)ResourceLoader.Load(player1);
		PackedScene pspl2 = (PackedScene)ResourceLoader.Load(player2);

		player1Instance = (Humanoid)pspl1.Instance();
		player2Instance = (Humanoid)pspl2.Instance();

		player1Instance.atkMode = true;

		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();
		player1Instance.Position = new Vector2(72, 40 + 16 * rng.RandiRange(0,7));
		rng.Randomize();
		player2Instance.Position = new Vector2(248, 40 + 16 * rng.RandiRange(0, 7));
		player2Instance.faceLeft = true;
		AddChild(player1Instance);
		AddChild(player2Instance);

		musicHandler.Connect("SkippedBeat", player1Instance, "RegenInspiration");
		//musicHandler.Connect("SkippedBeat", player2Instance, "RegenInspiration");

		//Healthbar
		HealthBar healthbar1 = GetNode<HealthBar>("HealthBar");
		player1Instance.Connect("HealthChanged", healthbar1, "onHealthChanged");
		player1Instance.Connect("InspirationChanged", healthbar1, "onInspirationChanged");


		//if (IsBossfight)
		//{
		//HealthBar healthbar2 = ResourceLoader.Load<HealthBar>("res://Scenes/UI/HealthBar.tscn")


		//}
		HealthBar healthbar2 = GetNode<HealthBar>("HealthBar2");
		player2Instance.Connect("HealthChanged", healthbar2, "onHealthChanged");
		player2Instance.Connect("InspirationChanged", healthbar2, "onInspirationChanged");





	}
	public Humanoid getPlayer1()
	{
		return player1Instance;
	}
	public Humanoid getPlayer2()
	{
		return player2Instance;
	}
}
