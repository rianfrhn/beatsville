using Godot;
using System;
using Godot.Collections;

[Serializable]
[Tool]
public class Stats : Resource
{
	[Export]
	public int maxHealth;
	[Export]
	public int healthRegen;
	[Export]
	public int maxInspiration;
	[Export]
	public int inspirationRegen;
	[Export]
	public int defense;
	[Export]
	public int strength;
	[Export]
	public Resource portrait;

	[Export]
	public Array<Resource> forms;
}
