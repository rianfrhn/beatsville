using Godot;
using System;

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
	public int duration;
	[Export]
	public int cost;
	[Export]
	public int damage;
	[Export]
	public string desc;
	[Export]
	public Animation castingAnimation;
}
