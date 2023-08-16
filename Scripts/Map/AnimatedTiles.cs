using Godot;
using Godot.Collections;
using System;

public class AnimatedTiles : TileMap
{
	[Export]
	Array<int> animatedTileIDs;
	TileSet ts;
	public override void _Ready()
	{
		ts = TileSet;
		UpdateAnim();
		BV.GM.Connect("SongChanged", this, "UpdateAnim");
	}
	public void UpdateAnim()
	{
		if (animatedTileIDs == null) return;
		foreach (int i in animatedTileIDs)
		{
			AnimatedTexture animatedTexture = (AnimatedTexture)ts.TileGetTexture(i);
			animatedTexture.Fps = BV.GM.songBPM / 60.0f;
		}
	}
	
}
