using Godot;
using System;

public class PlayerController : Node2D
{
	Humanoid player;
	RayCast2D playerRaycast;
	float tweenAnimSpeed = 5;
	SongData songData;
	bool menuOpened = false;
	int selectedSkill;
	string MarkerPath = "res://Scenes/Misc/Marker.tscn";
	public override void _Ready()
	{
		player = GetParent<Humanoid>();
		playerRaycast = player.GetNode<RayCast2D>("RayCast2D");
		GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
		if (gv.playerStat != null)
			player.statsResource = gv.playerStat;

		if (!player.atkMode)
		{
			gv.currentPlayer = player;
			gv.currentScene = player.GetParent();
			/*if (gv.heldPosition.ContainsKey(player.GetParent().Name))
			{
				player.Position = gv.heldPosition[player.GetParent().Name];
			}*/
		}
	}
	public void CreateMarker(Vector2 gPos, Color col = default)
    {
		PackedScene marker = ResourceLoader.Load<PackedScene>(MarkerPath);
		Node2D markerInst = marker.Instance<Node2D>();
		GetTree().CurrentScene.AddChild(markerInst);
		markerInst.GlobalPosition = gPos;
		markerInst.Modulate = col != default? col : Colors.White;
    }

	public override void _PhysicsProcess(float delta)
	{
		if (!player.atkMode && player.currentState != Humanoid.status.Moving)
		{
			player.AnimSpeed = Input.IsActionPressed("sprint") ? 0.2f : 0.3f;
			if (Input.IsActionPressed("ui_left"))
			{
				player.Move(Vector2.Left * 16);
			}
			if (Input.IsActionPressed("ui_right"))
			{
				player.Move(Vector2.Right * 16);
			}
			if (Input.IsActionPressed("ui_up"))
			{
				player.Move(Vector2.Up * 16);
			}
			if (Input.IsActionPressed("ui_down"))
			{
				player.Move(Vector2.Down * 16);
			}
		}
	}
	public override void _UnhandledInput(InputEvent @event)
	{
		if (player.atkMode)
		{

			if (@event.IsActionPressed("switch_move_1"))
			{
				if(((Stats)player.statsResource).forms[0]!= null)
				selectedSkill = 0;
			}
			if (@event.IsActionPressed("switch_move_2"))
			{
				if (((Stats)player.statsResource).forms[1] != null)
					selectedSkill = 1;
			}
			if (@event.IsActionPressed("switch_move_3"))
			{
				if (((Stats)player.statsResource).forms[2] != null)
					selectedSkill = 2;
			}


			if (@event.IsActionPressed("click_left"))
			{
				if (player.currentState != Humanoid.status.Idle)
				{
					if (player.currentState == Humanoid.status.Attacking) return;
					player.EmitSignal("Blundered");
					return;
				}
				Vector2 GMP = GetGlobalMousePosition();
				float x = GMP.x - (GMP.x % 16) + 8;
				float y = GMP.y - (GMP.y % 16) + 8;
				float deltaX = x - player.GlobalPosition.x;
				float deltaY = y - player.GlobalPosition.y;
				songData = BV.GM.songData;
				if (!songData.beatsToMove.Contains(BV.GM.inBeat())) 
				{
					player.EmitSignal("Blundered");
					return; 
				}
				if (Mathf.Abs(deltaY) > Mathf.Abs(deltaX))
				{
					Vector2 t = Vector2.Down * 16 * Mathf.Sign(deltaY);
					player.Move(t);
					CreateMarker(player.GlobalPosition + t);
				}
				else
				{
					Vector2 t = Vector2.Right * 16 * Mathf.Sign(deltaX);
					player.Move(t);
					CreateMarker(player.GlobalPosition + t);
				}
			}

			if (@event.IsActionPressed("click_right"))
			{
				if (player.currentState != Humanoid.status.Idle)
				{
					if (player.currentState == Humanoid.status.Attacking) return;
					player.EmitSignal("Blundered");
					return;
				}
				songData = BV.GM.songData;
				if (!songData.beatsToMove.Contains(BV.GM.inBeat())) {
					player.EmitSignal("Blundered");
					return; 
				}
				Vector2 GMP = GetGlobalMousePosition();
				float x = GMP.x - (GMP.x % 16) + 8;
				float y = GMP.y - (GMP.y % 16) + 8;
				Vector2 gridMP = new Vector2(x, y);
				player.Attack(gridMP, selectedSkill);
				CreateMarker(gridMP, Colors.Red);
			}

		}
		if (!player.atkMode)
		{

			if (@event.IsActionPressed("ui_cancel"))
			{
				if (!menuOpened)
				{
					GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
					gv.OpenMenu("res://Scenes/UI/Options.tscn");
					menuOpened = true;
				}
				else
				{
					menuOpened = false;
					Options options = GetTree().Root.GetNodeOrNull<Options>("Options");
					if(options != null) options.Close();
				}

			}
			if (@event.IsActionPressed("debug"))
			{
				GlobalVariable gv = GetTree().Root.GetNode<GlobalVariable>("GlobalVariable");
				/*
				Talisman t = ResourceLoader.Load<Talisman>("res://Resources/Talismans/GOD's WILL.tres");
				gv.saveData.talismans.Add(t);*/
				gv.saveData.AddXP(5);
			}
			if (@event.IsActionPressed("click_right"))
			{

				Vector2 GMP = GetGlobalMousePosition();
				float x = GMP.x - (GMP.x % 16) + 8;
				float y = GMP.y - (GMP.y % 16) + 8;
				float deltaX = x - player.GlobalPosition.x;
				float deltaY = y - player.GlobalPosition.y;
				if (Mathf.Abs(deltaY) > Mathf.Abs(deltaX))
				{
					Vector2 t = Vector2.Down * 16 * Mathf.Sign(deltaY);
					player.CheckInteract(t);
					CreateMarker(player.GlobalPosition + t, Colors.Yellow);
				}
				else
				{
					Vector2 t = Vector2.Right * 16 * Mathf.Sign(deltaX);
					player.CheckInteract(t);
					CreateMarker(player.GlobalPosition + t, Colors.Yellow);
				}
			}
            if (@event.IsActionPressed("click_left"))
            {
				Vector2 GMP = GetGlobalMousePosition();
				float x = GMP.x - (GMP.x % 16) + 8;
				float y = GMP.y - (GMP.y % 16) + 8;
				player.SetPathFind(GMP);
				CreateMarker(new Vector2(x,y));
			}
		}

	}

}
