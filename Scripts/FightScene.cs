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
	public GlobalHandler gh;

	public bool fightFinished = false;


	public override void _EnterTree()
	{
		//Initialize Variables
		gh = BV.GH;
		songOffset = gh.songOffset;

		fightData = (FightData)gh.currentFight;
		songdata = fightData.songData;
		IsBossfight = fightData.isBossFight;

		GlobalMusic globalMusic = BV.GM;
		globalMusic.musicOffset = songOffset;
		globalMusic.ChangeSongData((SongData)songdata);
	}

	public override void _Ready()
	{


		//Song initialization

		GlobalMusic globalMusic = BV.GM;

		//Map
		if(fightData.map != null)
		{
			TileMap tm = GetNodeOrNull<TileMap>("TileMap");
			if (tm != null)
			{
				int pos = tm.GetPositionInParent();
				tm.QueueFree();
				TileMap newTm = fightData.map.Instance<TileMap>();
				AddChild(newTm);
				MoveChild(newTm, pos);



			}
		}

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

		globalMusic.Connect("SkippedBeat", player1Instance, "RegenInspiration");
		//musicHandler.Connect("SkippedBeat", player2Instance, "RegenInspiration");

		//Healthbar
		HealthBar healthbar1 = GetNode<HealthBar>("CanvasLayer/HealthBar");
		healthbar1.picture.Texture = player1Instance.battlePicture;
		player1Instance.Connect("HealthChanged", healthbar1, "onHealthChanged");
		player1Instance.Connect("InspirationChanged", healthbar1, "onInspirationChanged");
		player1Instance.Connect("Defeated", this, "onPlayer1Defeat");


		if (IsBossfight)
		{
			TileMap barrierTile = GetNodeOrNull<TileMap>("BarrierTileMap");
			if (barrierTile != null) barrierTile.Visible = true;
			else barrierTile.Visible = false;

		}
		HealthBar healthbar2 = GetNode<HealthBar>("CanvasLayer/HealthBar2");
		healthbar2.picture.Texture = player2Instance.battlePicture;
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
		gh.FinishFight();
		fightFinished = true;
	}
}
