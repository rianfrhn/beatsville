using Godot;
using System;

public class References : Node
{
    public static class Char

    {
        public static readonly string SKal = "res://Scenes/Interactables/NPC/SKal.tscn";
        public static readonly string Adelle = "res://Scenes/Interactables/NPC/Adelle.tscn";
        public static readonly string Chimera = "res://Scenes/Interactables/NPC/Chimera.tscn";
        public static readonly string Cutter = "res://Scenes/Interactables/NPC/Cutter.tscn";
        public static readonly string Marione = "res://Scenes/Interactables/NPC/Marione.tscn";
        public static readonly string Rei = "res://Scenes/Interactables/NPC/Rei.tscn";
        public static readonly string Sparky = "res://Scenes/Interactables/NPC/Sparky.tscn";
        public static readonly string Khel = "res://Scenes/Interactables/NPC/Khel.tscn";
        public static readonly string Rai = "res://Scenes/Interactables/NPC/Rai.tscn";
        public static readonly string Lazarus = "res://Scenes/Interactables/NPC/Lazarus.tscn";
    }
    public static class Map
    {
        public static readonly string Beatsville1 = "res://Scenes/Map/BeatsvilleMap.tscn";
        public static readonly string HouseAdelle = "res://Scenes/Map/VillageInside/HouseAdelle.tscn";

    }
    public override void _Ready()
    {
        BV.Ref = this;
    }
}
