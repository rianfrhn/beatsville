using Godot;
using System;
using Godot.Collections;

public class BeatAction : Node
{
	int beatNum;
	Control beatContainer;
	CenterContainer beatTemplate;
	Array<CenterContainer> beats;
	TextureRect met;
	TextureRect metHand;
	TextureRect indicator;
	TextureRect indicatorArrow;
	ColorRect indicatorIndicator;

	int metSign = 1;

	public override void _Ready()
	{
		Initialize();
	}
	public async void Initialize()
	{
		beats = new Array<CenterContainer>();
		beatNum = BV.GM.songData.TimeSign;
		beatContainer = GetNode<Control>("Left/Beats");
		beatTemplate = beatContainer.GetNode<CenterContainer>("Beat");
		met = GetNode<TextureRect>("Left/Metronome");
		metHand = GetNode<TextureRect>("Left/Metronome/MetronomeHand");
		indicator = GetNode<TextureRect>("Left/Indicator");
		indicatorArrow = indicator.GetNode<TextureRect>("Arrow");
		indicatorIndicator = indicator.GetNode<ColorRect>("IndicatorIndi");
		TextureRect indicatorColor = indicator.GetNode<TextureRect>("Color");

		GradientTexture colorGradient = new GradientTexture();
		Gradient grad = new Gradient();
		grad.InterpolationMode = Gradient.InterpolationModeEnum.Constant;
		grad.Colors = new Color[]
		{
			Colors.Red,
			Colors.Green,
			Colors.Red,
		};
		grad.Offsets = new float[]
		{
			0,
			0.5f - BV.GM.interval,
			0.5f + BV.GM.interval
		};
		colorGradient.Gradient = grad;
		indicatorColor.Texture = colorGradient;


		for (int i = 0; i < beatNum; i++)
		{
			CenterContainer b = (CenterContainer)beatTemplate.Duplicate();
			b.Name = i.ToString();
			beatContainer.AddChild(b);
			beats.Add(b);
		}
		beatTemplate.QueueFree();
		BV.GM.Connect("EmitBeat", this, "BeatPulse");
		await ToSignal(GetTree().CurrentScene, "ready");
		BV.GV.currentAtkPlayer.Connect("Acted", this, "onPlayerAction");
	}
	public void BeatPulse(int beat)
	{
		int b = beat % BV.GM.songData.TimeSign;
		TextureRect cb = beats[b].GetNode<TextureRect>("Beat");
		SceneTreeTween tw = GetTree().CreateTween();
		if (BV.GM.songData.beatsToAtk.Contains(b))
		{
			cb.Modulate = new Color(0, 1, 0,1);
		}
		else if (BV.GM.songData.beatsToMove.Contains(b))
		{
			cb.Modulate = new Color(0, 0, 1,1);
		}
		else
		{
			cb.Modulate = new Color(1, 0, 0,1);
		}

		tw.TweenProperty(cb, "modulate:a", 0.0f, 60 / BV.GM.songBPM);
		if(BV.GM.songTimeSig %2 == 1 && b==0)
			metSign *= -1;
	}
	public override void _Process(float delta)
	{
		metHand.RectRotation = metSign * 60 * Mathf.Sin(BV.GM.barPercent * BV.GM.songTimeSig * Mathf.Pi);
		float percent = BV.GM.percentInBeats + 0.5f;
		if (percent > 1) percent -= 1;
		percent *= indicator.RectSize.x;
		indicatorArrow.RectPosition = new Vector2(percent - 1.5f, -2);
		
	}
	public void onPlayerAction(bool validMove)
	{
		TextureRect metHandIndicator = (TextureRect)metHand.Duplicate();
		metHandIndicator.Modulate = validMove ? new Color(0, 1, 0) : new Color(1, 0, 0);
		met.AddChild(metHandIndicator);
		SceneTreeTween indTw = GetTree().CreateTween();
		indTw.TweenProperty(metHandIndicator, "modulate:a", 0.0f, 1.5f);
		indTw.TweenCallback(metHandIndicator, "queue_free");
		
		ColorRect indicatorShadow = (ColorRect)indicatorIndicator.Duplicate();
		indicator.AddChild(indicatorShadow);
		indicatorShadow.Modulate = new Color(0.1f, 0.1f, 0.1f, 1);
		indicatorShadow.Visible = true;
		indicatorShadow.RectPosition = new Vector2(indicatorArrow.RectPosition.x + 1.5f - 0.5f, 0);
		SceneTreeTween indShadTw = GetTree().CreateTween();
		indShadTw.TweenProperty(indicatorShadow, "modulate:a", 0.0f, 2.0f);
		indShadTw.TweenCallback(indicatorShadow, "queue_free");

	}



}
