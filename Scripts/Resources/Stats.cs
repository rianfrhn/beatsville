using Godot;
using System;

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
    Resource atkForm1;
    [Export]
    Resource atkForm2;
    [Export]
    Resource atkForm3;

}
