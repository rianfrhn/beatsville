using Godot;
using System;
using Godot.Collections;

[Tool]
public class SongData : Resource
{
	[Export]
	public int BPM { get; set; }
	[Export]
	public int TimeSign { get; set; }
	[Export]
	public AudioStream song { get; set; }
	[Export]
	public Array<int> beatsToMove { get; set; }
	[Export]
	public Array<int> beatsToAtk { get; set; }
	[Export]
	public Dictionary<int, string> timeChanges { get; set; }



		/*
		File file = new File();
		if (file.FileExists(audioStreamDir))
		{
			Error err = file.Open(audioStreamDir, File.ModeFlags.Read);
			byte[] buffer = file.GetBuffer((int)file.GetLen());

			if (audioStreamDir.Extension() == "wav")
			{
				AudioStreamSample streamSample = new AudioStreamSample();
				streamSample.Format = AudioStreamSample.FormatEnum.Format16Bits;

				streamSample.Stereo = true;
				streamSample.Data = buffer;
				song = streamSample;
			}
			else if (audioStreamDir.Extension() == "ogg")
			{
				AudioStreamOGGVorbis streamOGG = new AudioStreamOGGVorbis();
				streamOGG.Data = buffer;
				song = streamOGG;
			}
		}
		*/
		
	public AudioStream LoadSong(string audioStreamDir)
	{
		AudioStreamOGGVorbis song = (AudioStreamOGGVorbis)GD.Load(audioStreamDir);
		AudioStream audiostream = song;
		return audiostream;

	}

	
}
