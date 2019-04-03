using UnityEngine;
using System.Collections;

public class SongPlayer : MonoBehaviour
{
	public SongData Song;

	protected float SmoothAudioTime = 0f;
	protected bool AudioStopEventFired = false;
	protected bool WasPlaying = false;
	protected bool IsSongPlaying = false;

	void Update()
	{
		if( IsPlaying() )
		{
			AudioStopEventFired = false;
			WasPlaying = true;
			UpdateSmoothAudioTime();
		}
	}

	protected void OnSongStopped()
	{
		if( !GetComponent<AudioSource>().clip )
		{
			return;
		}

		//I want to check if the song has finished playing automatically.
		//Sometimes this is triggered when the song is at the end, 
		//and sometimes it has already been reset to the beginning of the song.
		if( GetComponent<AudioSource>().time == GetComponent<AudioSource>().clip.length
		 || ( WasPlaying && GetComponent<AudioSource>().time == 0 ) )
		{
			IsSongPlaying = false;
			GetComponent<GuitarGameplay>().OnSongFinished();
		}
	}

	protected void UpdateSmoothAudioTime()
	{
		//Smooth audio time is used because the audio.time has smaller discreet steps and therefore the notes wont move
		//as smoothly. This uses Time.deltaTime to progress the audio time
		SmoothAudioTime += Time.deltaTime;

		if( SmoothAudioTime >= GetComponent<AudioSource>().clip.length )
		{
			SmoothAudioTime = GetComponent<AudioSource>().clip.length;
			OnSongStopped();
		}

		//Sometimes the audio jumps or lags, this checks if the smooth audio time is off and corrects it
		//making the notes jump or lag along with the audio track
		if( IsSmoothAudioTimeOff() )
		{
			CorrectSmoothAudioTime();
		}
	}

	protected bool IsSmoothAudioTimeOff()
	{
		//Negative SmoothAudioTime means the songs playback is delayed
		if( SmoothAudioTime < 0 )
		{
			return false;
		}

		//Check if my smooth time and the actual audio time are of by 0.1
		return Mathf.Abs( SmoothAudioTime - GetComponent<AudioSource>().time ) > 0.1f;
	}

	protected void CorrectSmoothAudioTime()
	{
		SmoothAudioTime = GetComponent<AudioSource>().time;
	}

	public void Play()
	{
		IsSongPlaying = true;

		if( SmoothAudioTime < 0 )
		{
			StartCoroutine( PlayDelayed( Mathf.Abs( SmoothAudioTime ) ) );
		}
		else
		{
			GetComponent<AudioSource>().Play();
			SmoothAudioTime = GetComponent<AudioSource>().time;
		}
	}

	protected IEnumerator PlayDelayed( float delay )
	{
		yield return new WaitForSeconds( delay );

		GetComponent<AudioSource>().Play();
	}

	public void Pause()
	{
		IsSongPlaying = false;
		GetComponent<AudioSource>().Pause();
	}

	public void Stop()
	{
		GetComponent<AudioSource>().Stop();
		WasPlaying = false;
		IsSongPlaying = false;
	}

	public bool IsPlaying()
	{
		return IsSongPlaying;
	}

	public void SetSong( SongData song )
	{
		Song = song;
		gameObject.GetComponent<AudioSource>().time = 0;
		gameObject.GetComponent<AudioSource>().clip = Song.BackgroundTrack;
		gameObject.GetComponent<AudioSource>().pitch = 1;

		SmoothAudioTime = MyMath.BeatsToSeconds( -Song.AudioStartBeatOffset, Song.BeatsPerMinute );
	}

	public float GetCurrentBeat( bool songDataEditor = false )
	{
		if( songDataEditor )
		{
			SmoothAudioTime = GetComponent<AudioSource>().time;
		}

		return MyMath.SecondsToBeats( SmoothAudioTime, Song.BeatsPerMinute ) - Song.AudioStartBeatOffset;
	}
}