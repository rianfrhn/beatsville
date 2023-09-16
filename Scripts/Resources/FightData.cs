using Godot;
using System;

public class FightData : Resource
{
    [Export]
    public int moneyGain;
    [Export]
    public int xpGain;
    [Export]
    public PackedScene player1;
    [Export]
    public PackedScene player2;
    [Export]
    public Resource songData;
    [Export]
    public bool isBossFight;
    [Export]
    public PackedScene map;
    [Export]
    public bool lossPrevention = false;
    public string name;

    public void Initialize(string name, bool preventLoss = false)
    {
        this.name = name;
        this.lossPrevention = preventLoss;
        DialogicSharp.SetVariable("OngoingFight", name);
    }
    public void Finished()
    {
        DialogicSharp.SetVariable(name, "1");
        GD.Print(ResourceName + " Done!");
        DialogicSharp.SetVariable("OngoingFight", "");
    }
    



}
