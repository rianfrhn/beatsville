using Godot;
using System;
using Godot.Collections;

public class SkillForm : Resource
{
	[Export]
	public int id;
	[Export]
	public string name;
	[Export]
	public Texture displayPic;
	[Export]
	public PackedScene projectile;
	[Export]
	public int cost;
	[Export]
	public int damage;
	[Export]
	public string desc;
	[Export]
	public Array<PackedScene> projectiles;
}
