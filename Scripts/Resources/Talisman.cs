using Godot;
using System;
using Godot.Collections;
[Serializable]
[Tool]
public class Talisman : Resource
{
	[Export]
	public int id;
	[Export]
	public string name;
	[Export]
	public string description;
	[Export]
	public Texture displayPic;

	[Export]
	public int health = 0;
	[Export]
	public int healthRegen = 0;
	[Export]
	public int inspiration = 0;
	[Export]
	public int inspirationRegen = 0;
	[Export]
	public int defense = 0;
	[Export]
	public int strength = 0;
	[Export]
	public int competence = 0;

}
