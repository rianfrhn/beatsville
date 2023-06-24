using Godot;
using System;

public class Notifier : CanvasLayer
{
	Control container;
	RichTextLabel label;

	Timer t;
	AnimationPlayer ap;

	public override void _Ready()
	{
		container = GetNodeOrNull<Control>("Control/Container");
		label = GetNodeOrNull<RichTextLabel>("Control/Container/RichTextLabel");
		t = GetNodeOrNull<Timer>("Timer");
		ap = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
	}
	public async void Initialize(string text, float duration = 2.0f, Color c = default)
	{

		if (container == null) { QueueFree(); return; };
		if (label == null) { QueueFree(); return; };
		Color col = c == default ? Colors.White : c;
		string colString = col.ToHtml(false);
		label.BbcodeText = "[center][wave][color=#" + colString + "]" + text + "[/color][/wave][/center]";
		ap.Play("Enter");
		await ToSignal(ap, "animation_finished");
		t.WaitTime = duration;
		t.Start();
		await ToSignal(t, "timeout");
		ap.PlayBackwards("Enter");
		await ToSignal(ap, "animation_finished");
		QueueFree();

	}
}
