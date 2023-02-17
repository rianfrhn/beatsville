using Godot;
using System;
using Godot.Collections;

public class GlobalVariable : Node
{
	/// <summary>
	/// Music Offset for fight scenes. The higher the number the higher the delay.
	/// </summary>
	public float songOffset = 0;

	/// <summary>
	/// How many percent of the beat can player input.
	/// <para>
	/// Ex: 0.15f, therefore player can input between -15% and 15% in beats. therefore a 30% input rate in beat.
	/// <br/>A 50% interval lets you input in the whole beat.
	/// </para>
	/// </summary>
	public float interval = 0.15f;



	/// <summary>
	/// String that stores from what scene. Will then be passed to the Position2d Inside of the map
	/// </summary>
	public string fromScene;

	//Fight Scene Variables

	/// <summary>
	/// String of player node path
	/// </summary>
	[Export]
	public string player1dir, player2dir;

	/// <summary>
	/// Initialize the fight song with its bpm etc. To make resource, add resource in project, attach songdata as script.
	/// </summary>
	[Export]
	public string songdatadir;

	/// <summary>
	/// Bossfight condition. If true ("y"), player cant run.
	/// </summary>
	[Export]
	public bool IsBossfight;

	//Player data
	Resource playerStat;
	int money;

	public Node currentScene;
	public Camera2D currentCamera;
	public string currentMusic;
	public Humanoid currentPlayer;
	
	public Dictionary<string, Vector2> heldPosition = new Dictionary<string, Vector2>();
	/// <summary>
	/// Used to initialize the fight
	/// </summary>
	/// <param name="player1">String directory of player1</param>
	/// <param name="player2">String directory of player2</param>
	/// <param name="songDatadir">Song data resource</param>
	/// <param name="isBossfight">If is bossfight, no borders. Player can run</param>
	/// 

	public void InitializeFightData(string player1, string player2, string songData, bool isBossfight, int moneygain, int xpgain)
	{
		player1dir = player1;
		player2dir = player2;
		songdatadir = songData;
		IsBossfight = isBossfight;

		GD.Print(player1dir, player2dir, songdatadir, IsBossfight);
	}

	public void HoldPosition(string sceneDir)
    {
		if (heldPosition.ContainsKey(currentScene.Name)) heldPosition[currentScene.Name] = currentPlayer.GlobalPosition;
		else heldPosition.Add(currentScene.Name, currentPlayer.GlobalPosition);
		DialogicSharp.SetVariable("HeldScene", sceneDir);
		DialogicSharp.SetVariable("HeldMusic", currentMusic);


	}
	public void OpenMenu(string directory)
	{
		PackedScene ps = ResourceLoader.Load<PackedScene>(directory);
		Control instance = ps.Instance<Control>();
		GetTree().Root.AddChild(instance);
		if(currentCamera != null)
		{
			instance.RectGlobalPosition = currentCamera.GetCameraScreenCenter() - instance.RectSize/2;
		}


	}
}
