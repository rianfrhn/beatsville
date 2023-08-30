using Godot;
using System;

public class DefaultMusic : Node
{
    [Export(PropertyHint.File, "*.tres")]
    public string musicPath;
}
