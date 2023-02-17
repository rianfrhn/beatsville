using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class MusicHandler : Node
{
	AudioStreamPlayer musicStream;

	private int _currentBeat;
	private float beatLength;
	private float barLength;
	public float secs;
	public float barPercent;
	public AudioStreamPlayer audiostream;
	public bool plrMoved = false;
	private bool cooldown = false;
	private bool redCooldown = false;
	public int inWhatBeat;


	[Export]
	public float musicOffset = 0;
	[Export]
	public float interval = 0.1f;
	[Export]
	public Resource songDataRes;

	public SongData songData;
	public float songBPM = 180;
	public int songTimeSig = 6;
	private int metronomeCtrl = 0;
	private Timer metronomeTimer;
	private AudioStreamPlayer metronomeStream;


	[Signal]
	public delegate void EmitBeat(int beat); //Ini tiap beat bakal ngelakuin seuatu

	[Signal]
	public delegate void SkippedBeat();

	public override void _Ready()
	{
		GlobalMusic gm = GetTree().Root.GetNode<GlobalMusic>("GlobalMusic");
		musicStream = gm.GetPlayingAudioStream();
		GlobalHandler.CurrentMusic = this;
		if (songDataRes != null && songDataRes is SongData sd)
		{
			songData = sd;
			songBPM = sd.BPM;
			songTimeSig = sd.TimeSign;
			musicStream.Stream = sd.song;
		}
		beatLength = 60 / songBPM;
		barLength = beatLength * songTimeSig;
		musicStream.Play();
		Connect("EmitBeat", this, "QueueTimeChange");
		metronomeTimer = GetNode<Timer>("Timer");
		metronomeStream = GetNode<AudioStreamPlayer>("Metronome");
	}


	public override void _Process(float delta)
	{
		Process_Stream();
		Process_Music();
	}

	private void Process_Stream()
	{
		secs = musicStream.GetPlaybackPosition()/* +
			(float)AudioServer.GetTimeSinceLastMix()*/ -
			(float)AudioServer.GetOutputLatency() +
			musicOffset;

		barPercent = (secs % (barLength) / (barLength));
	}
	private void Process_Music()
	{
		int beatNum = Mathf.FloorToInt(secs / beatLength);
		float percentInBar = secs % barLength / barLength;
		float percentInBeats = secs % beatLength / beatLength;
		if (percentInBeats <= interval || percentInBeats >= (beatLength - interval) / beatLength)
		{
			if (!cooldown)
			{
				plrMoved = false;
			}
			int beat = Mathf.RoundToInt(percentInBar * songTimeSig + interval);
			beat = (beat == songTimeSig ? 0 : beat);
			inWhatBeat = beat + Mathf.FloorToInt(secs / barLength + interval) * songTimeSig;
		}
		else
		{
			if(inWhatBeat != -1 && !plrMoved && songData.beatsToMove.Contains(inWhatBeat % songTimeSig))
			{
				//GD.Print("Skipped beat " + inWhatBeat);
				EmitSignal("SkippedBeat");
			}
			cooldown = false;
			inWhatBeat = -1;
			
		}

		
		if (_currentBeat != beatNum)
		{
			_currentBeat = beatNum;
			EmitSignal("EmitBeat", beatNum);
			//if (songData.beatsToAtk.Contains(beatNum % songTimeSig) || songData.beatsToMove.Contains(beatNum % songTimeSig)) return;
			//EmitSignal("EmitBeat", beatNum);
			//GD.Print("moved red " + beatNum);


		}


	}
	//debug
	public int inBeat()
	{
		if(inWhatBeat != -1)
		{
			//EmitSignal("EmitBeat", inWhatBeat);
			//GD.Print("Moved by player at " + inWhatBeat);
			plrMoved = true;
			cooldown = true;
		}
		return inWhatBeat%songTimeSig;

	}
	public void QueueTimeChange(int beat)
	{
		ICollection<int> keys = songData.timeChanges.Keys;
		if (keys.Contains(beat + songTimeSig))
		{
			string beats = songData.timeChanges[beat + songTimeSig];
			Array<int> moveBeats = new Array<int>();
			Array<int> atkBeats = new Array<int>();
			for(int i=0; i<beats.Length(); i++){
				char ch = beats[i];
				if(ch == '2') { atkBeats.Add(i); }
				else if(ch == '1') { moveBeats.Add(i);}
			}
			metronomeTimer.WaitTime = 57 / songBPM;
			metronomeCtrl = 0;
			Godot.Collections.Array passed_data = new Godot.Collections.Array(beats, moveBeats, atkBeats);
			
			metronomeTimer.Connect("timeout", this, "TimeChangeBeatSounds", passed_data);
			TimeChangeBeatSounds(beats, moveBeats, atkBeats);
		}

	}
	public void TimeChangeBeatSounds(string beats, Array<int> newMoveBeat, Array<int> newAtkBeat)
	{


		if (metronomeCtrl == songTimeSig)
		{
			TimeChange(newMoveBeat, newAtkBeat);
			metronomeTimer.Disconnect("timeout", this, "TimeChangeBeatSounds");
			return;
		}
		char _beat = beats[metronomeCtrl];
		if (_beat == '2')
		{
			metronomeStream.PitchScale = 2f;
			metronomeStream.Play();
		} else if(_beat == '1')
		{
			metronomeStream.PitchScale = 1f;
			metronomeStream.Play();
		} else if(_beat == '0')
		{

		}
		metronomeTimer.Start();
		metronomeCtrl++;

	}
	public void TimeChange(Array<int> newMoveBeat, Array<int> newAtkBeat) {
		songData.beatsToAtk = newAtkBeat;
		songData.beatsToMove = newMoveBeat;

	}
}
