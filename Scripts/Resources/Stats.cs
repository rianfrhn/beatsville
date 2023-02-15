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
    public int maxDefense;
    [Export]
    public int baseAtk;

    [Export]
    Resource atkForm1;
    [Export]
    Resource atkForm2;
    [Export]
    Resource atkForm3;

}
