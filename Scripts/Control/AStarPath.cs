using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class AStarPath : Node2D
{
	[Export]
	public bool visualizePath;
	[Export]
	public bool useDiagonalPathFinding = false;
	[Export]
	public int nonWalkableId;
	private Godot.Collections.Array nonWalkables;
	public List<Vector2> pathNodeList = new List<Vector2>();
	private TileMap tileMapWorld;
	private AStar2D aStar2DPath = new AStar2D();
	private Vector2 startPos = Vector2.Zero;
	private Vector2 endPos = Vector2.Zero;

	public Vector2 halfTileSize;
	private Vector2 mapSize = Vector2.Zero;




	public override void _Ready()
	{
		SetAsToplevel(true);
	}
	private void GetTileMapBounds()
	{
		var tilemapSize = tileMapWorld.GetUsedRect();
		mapSize = new Vector2(tilemapSize.Size.x, tilemapSize.Size.y);
		GD.Print("MAP SIZE = " + mapSize);


	}
	private bool IsTileOutsideMap(Vector2 tile)
	{
		if (tile.x < 0 || tile.y < 0 || tile.x >= mapSize.x || tile.y >= mapSize.y) return true;
		return false;
	}
	private int CalculateUniqueTileId(Vector2 tile)
	{
		int x = (int)tile.x;
		int y = (int)tile.y;
		int id = ((x + y) * (x + y + 1) / 2) + y;
		return id;
	}
	private void SetStartPos(Vector2 value)
	{
		if (nonWalkables.Contains(value)) return;
		if (IsTileOutsideMap(value)) return;
		startPos = value;
		if(endPos != null && endPos != startPos)
		{
			//Calculate path
			CalculateAstarPath();
		}
	}
	private void SetEndPos(Vector2 value)
	{
		if (nonWalkables.Contains(value)) return;
		if (IsTileOutsideMap(value)) return;
		endPos = value;
	}
	private void CalculateAstarPath()
	{
		var tileStartId = CalculateUniqueTileId(startPos);
		var tileEndId = CalculateUniqueTileId(endPos);
		pathNodeList = aStar2DPath.GetPointPath(tileStartId, tileEndId).ToList();

	}
	private Godot.Collections.Array AddWalkableTiles(Godot.Collections.Array nonWalkables)
	{
		Godot.Collections.Array walkables = new Godot.Collections.Array();

		for(int y = 0; y< mapSize.y; ++y)
		{
			for(int x = 0; x<mapSize.x; ++x)
			{
				var tile = new Vector2(x, y);
				if (nonWalkables.Contains(tile)) continue;
				walkables.Add(tile);
				var tileId = CalculateUniqueTileId(tile);

				aStar2DPath.AddPoint(tileId, new Vector2(tile.x, tile.y));

			}
		}
		return walkables;

	}
	private void ConnectWalkableTiles(Godot.Collections.Array walkables)
	{
		foreach(Vector2 tile in walkables)
		{
			var tileId = CalculateUniqueTileId(tile);

			Vector2[] neighboringTiles =
			{
				new Vector2(tile.x+1, tile.y),
				new Vector2(tile.x-1, tile.y),
				new Vector2(tile.x, tile.y+1),
				new Vector2(tile.x, tile.y-1)
			};
			foreach (var neighborTile in neighboringTiles)
			{
				var neighborTileId = CalculateUniqueTileId(neighborTile);
				if (IsTileOutsideMap(neighborTile)) continue;
				if (!aStar2DPath.HasPoint(neighborTileId)) continue;
				aStar2DPath.ConnectPoints(tileId, neighborTileId, false);
			}
		}
	}
	public void SetTileMap(TileMap tileMap)
	{
		tileMapWorld = tileMap;
		InitializeAstarPathFind();
	}
	private void InitializeAstarPathFind()
	{
		GetTileMapBounds();
		halfTileSize = tileMapWorld.CellSize / 2;
		nonWalkables = tileMapWorld.GetUsedCellsById(nonWalkableId);
		var walkableTilesList = AddWalkableTiles(nonWalkables);
		ConnectWalkableTiles(walkableTilesList);
		SetStartPos(Vector2.Zero);
		SetEndPos(Vector2.Zero);

	}
	public bool SetAstarPath(Vector2 startPos, Vector2 endPos)
	{
		if(tileMapWorld == null)
		{
			GD.PrintErr("Tilemap is null");
			return false;
		}
		this.startPos = tileMapWorld.WorldToMap(startPos);
		this.endPos = tileMapWorld.WorldToMap(endPos);

		if(tileMapWorld.GetCell((int)startPos.x, (int)startPos.y) != 0
		&& tileMapWorld.GetCell((int)endPos.x, (int)endPos.y) != 0)
		{
			CalculateAstarPath();
			return true;
		}
		return false;
	}
	public override void _Draw()
	{
		if (!visualizePath) return;
		var path = pathNodeList;

		if (path == null || path.Count <= 0) return;

		var pathStart = path[0];
		var pathEnd = path[path.Count - 1];
		var lastPathPos = tileMapWorld.MapToWorld(new Vector2(pathStart.x, pathStart.y)) + halfTileSize;
		for(int i=0; i<path.Count; i++)
		{
			var currentPathPos = tileMapWorld.MapToWorld(new Vector2(path[i].x, path[i].y)) + halfTileSize;
			DrawLine(lastPathPos, currentPathPos, new Color(1, 1, 1), 2f);

			if (i == 1)
			{
				DrawCircle(lastPathPos, 2.0f, new Color(1, 0, 0));
				DrawCircle(currentPathPos, 2.0f, new Color(1, 1, 1));
			} else if(i == path.Count -1){
				DrawCircle(currentPathPos, 2.0f, new Color(0, 1, 0));
			}
			else
			{
				DrawCircle(currentPathPos, 2.0f, new Color(0.8f, 0.8f, 0.8f));
			}
			lastPathPos = currentPathPos;
		}
	}
	public override void _Process(float delta)
	{
		if (visualizePath)
		{
			Update();
		}
	}






}
