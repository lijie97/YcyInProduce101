using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor( typeof( SongData ) )]
public class SongDataEditor : Editor
{
	//Needed to draw 2D lines and rectangles
	private static Texture2D _coloredLineTexture;
	private static Color _coloredLineColor;

	private GameObject GuitarObject;
	private AudioSource MetronomeSource;
	private SongPlayer SongPlayer;
	private AudioClip LastAudioClip;

	//Dimensions of the editor
	//Song View is the one with the black background, where you can add notes etc.
	//ProgressBar, or Progress View is the small preview on the right, where you can navigate through the song
	private Rect SongViewRect;
	private float SongViewProgressBarWidth = 20f;
	private float SongViewHeight = 400f;

	//Metronome Vars
	private static bool UseMetronome;
	private float LastMetronomeBeat = Mathf.NegativeInfinity;

	//Helper vars to handle mouse clicks
	private Vector2 MouseUpPosition = Vector2.zero;
	private bool LastClickWasRightMouseButton;

	//Currently selected note index
	private int SelectedNote = -1;

	//Song overview texture (the one on the right which you can use to navigate)
	private Texture2D ProgressViewTexture;

	//Timer to calculate editor performance
	Timer PerformanceTimer = new Timer();

	[MenuItem( "Assets/Create/Song" )]
	public static void CreateNewSongAsset()
	{
		SongData asset = ScriptableObject.CreateInstance<SongData>();
		AssetDatabase.CreateAsset( asset, "Assets/NewSong.asset" );
		
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}

	protected void OnEnable()
	{
		//Setup object references
		GuitarObject = GameObject.Find( "Guitar" );

		if( GuitarObject == null )
		{
			return;
		}

		SongPlayer = GuitarObject.GetComponent<SongPlayer>();
		MetronomeSource = GameObject.Find( "Metronome" ).GetComponent<AudioSource>();
		LastAudioClip = SongPlayer.Song.BackgroundTrack;
		
		//Prepare playback
		SongPlayer.SetSong( (SongData)target );
		LastMetronomeBeat = -Mathf.Ceil( SongPlayer.Song.AudioStartBeatOffset );


		RedrawProgressViewTexture();
	}

	protected void RedrawProgressViewTexture()
	{
		int width = (int)SongViewProgressBarWidth;
		int height = (int)SongViewHeight;

		if( !ProgressViewTexture )
		{
			//Create empty texture if it doesn't exist
			ProgressViewTexture = new Texture2D( width, height );
			ProgressViewTexture.wrapMode = TextureWrapMode.Clamp;
		}

		//Draw Background Color
		Color[] BackgroundColor = new Color[ width * height ];
		for( int i = 0; i < width * height; ++i )
		{
			BackgroundColor[ i ] = new Color( 0.13f, 0.1f, 0.26f );
		}

		ProgressViewTexture.SetPixels( 0, 0, width, height, BackgroundColor );

		//Calculate the scale in which the notes are drawn, so they all fit into the progress view
		float totalBeats = SongPlayer.Song.GetLengthInBeats();
		float heightOfOneBeat = 1f / totalBeats * (float)height;

		//Draw all notes
		for( int i = 0; i < SongPlayer.Song.Notes.Count; ++i )
		{
			//Which string does this note belong to?
			int stringIndex = SongPlayer.Song.Notes[ i ].StringIndex;

			//Which color does this string have?
			Color color = GuitarObject.GetComponent<GuitarGameplay>().GetColor( stringIndex );

			//Calculate position of the note
			int y = (int)Mathf.Round( ( ( SongPlayer.Song.Notes[ i ].Time + SongPlayer.Song.AudioStartBeatOffset - 1 ) / totalBeats ) * height );
			int x = (int)( width / 2 ) + ( stringIndex - 2 ) * 4;

			//Get the trail length (0 = no trail)
			float length = SongPlayer.Song.Notes[ i ].Length;

			//Draw 3x3 pixel rectangle
			for( int j = -1; j < 2; ++j )
			{
				for( int k = -1; k < 2; ++k )
				{
					ProgressViewTexture.SetPixel( x + j, y + k, color );
				}
			}

			//Draw trail
			if( length > 0 )
			{
				for( int lengthY = y; lengthY < y + length * heightOfOneBeat; ++lengthY )
				{
					ProgressViewTexture.SetPixel( x, lengthY, color );
				}
			}
		}
			
		ProgressViewTexture.Apply();
	}

	public override void OnInspectorGUI()
	{
		DrawInspector();

		//Check for mouse events
		if( Event.current.isMouse )
		{
			if( Event.current.type == EventType.MouseDown )
			{
				OnMouseDown( Event.current );
			}
			else if( Event.current.type == EventType.MouseUp )
			{
				OnMouseUp( Event.current );
			}
		}

		//Check for key input events
		if( Event.current.isKey )
		{
			if( Event.current.type == EventType.KeyDown )
			{
				OnKeyDown( Event.current );
			}
		}

		if( Event.current.type == EventType.ValidateCommand )
		{
			switch( Event.current.commandName )
			{
				case "UndoRedoPerformed":
					RedrawProgressViewTexture();
					break;
			}
		}


		if( GUI.changed )
		{
			SongData targetData = target as SongData;
			if( targetData.BackgroundTrack != LastAudioClip )
			{
				SongPlayer.SetSong( (SongData)target );
				LastAudioClip = targetData.BackgroundTrack;
			}
		}

		UpdateMetronome();
		RepaintGui();
	}

	protected void OnKeyDown( Event e )
	{
		switch( e.keyCode )
		{
			case KeyCode.UpArrow:
				//Up arrow advances the song by one beat
				GuitarObject.GetComponent<AudioSource>().time += MyMath.BeatsToSeconds( 1f, SongPlayer.Song.BeatsPerMinute );
				e.Use();
				break;
			case KeyCode.DownArrow:
				//Down arrow reverses the song by one beat
				GuitarObject.GetComponent<AudioSource>().time -= MyMath.BeatsToSeconds( 1f, SongPlayer.Song.BeatsPerMinute );
				e.Use();
				break;
			case KeyCode.RightControl:
				//Right CTRL plays/pauses the song
				OnPlayPauseClicked();
				e.Use();
				break;
			case KeyCode.LeftArrow:
				//Left arrow selects the previous note
				if( SelectedNote != -1 && SelectedNote > 0 )
				{
					SelectedNote--;
					Repaint();
				}
				break;
			case KeyCode.RightArrow:
				//Right arrow selects the next note
				if( SelectedNote != -1 && SelectedNote < SongPlayer.Song.Notes.Count )
				{
					SelectedNote++;
					Repaint();
				}
				break;
			case KeyCode.Delete:
				//DEL removes the currently selected note
				if( SelectedNote != -1 )
				{
					Undo.RegisterUndo( SongPlayer.Song, "Remove Note" );

					SongPlayer.Song.RemoveNote( SelectedNote );
					SelectedNote = -1;
					EditorUtility.SetDirty( target );
					RedrawProgressViewTexture();

					Repaint();
				}
				break;
		}
	}

	protected GUIStyle GetWarningBoxStyle()
	{
		GUIStyle box = new GUIStyle( "box" );

		box.normal.textColor = EditorStyles.miniLabel.normal.textColor;
		box.imagePosition = ImagePosition.ImageLeft;
		box.stretchWidth = true;
		box.alignment = TextAnchor.UpperLeft;

		return box;
	}

	protected void WarningBox( string text, string tooltip = "" )
	{
		GUIStyle box = GetWarningBoxStyle();

		Texture2D warningIcon = (Texture2D)Resources.Load( "Warning", typeof( Texture2D ) );
		GUIContent content = new GUIContent( " " + text, warningIcon, tooltip );
		GUILayout.Label( content, box );
	}

	protected void DrawInspector()
	{
		if( GuitarObject == null )
		{
			WarningBox( "Guitar Object could not be found." );
			WarningBox( "Did you load the GuitarUnity scene?" );
			return;
		}

		//Time the performance of the editor window
		PerformanceTimer.Clear();
		PerformanceTimer.StartTimer( "Draw Inspector" );

		GUILayout.Label( "Song Data", EditorStyles.boldLabel );

		DrawDefaultInspector();

		if( SongPlayer.Song.BackgroundTrack == null )
		{
			WarningBox( "Please set a background track!" );
			return;
		}

		if( SongPlayer.Song.BeatsPerMinute == 0 )
		{
			WarningBox( "Please set the beats per minute!" );
		}

		if( SelectedNote >= SongPlayer.Song.Notes.Count )
		{
			SelectedNote = -1;
		}

		if( SelectedNote == -1 )
		{
			//If no note is selected, still draw greyed out inspector elements 
			//so the editor doesn't jump when notes are selected and deselected

			GUI.enabled = false;
			GUILayout.Label( "No Note selected", EditorStyles.boldLabel );

			EditorGUILayout.FloatField( "Time", 0 );
			EditorGUILayout.IntField( "String", 0 );
			EditorGUILayout.FloatField( "Length", 0 );

			EditorGUILayout.BeginHorizontal();
				GUILayout.Space( 15 );
				GUILayout.Button( "Remove Note" );
			EditorGUILayout.EndHorizontal();

			GUI.enabled = true;
		}
		else
		{
			//Draw Header and Next/Previous Note Buttons
			EditorGUILayout.BeginHorizontal();

				GUILayout.Label( "Note " + SelectedNote.ToString(), EditorStyles.boldLabel );
				
				if( SelectedNote == 0 )
				{
					GUI.enabled = false;
				}
				if( GUILayout.Button( "<" ) )
				{
					SelectedNote--;
				}
				GUI.enabled = true;

				if( SelectedNote == SongPlayer.Song.Notes.Count - 1 )
				{
					GUI.enabled = false;
				}
				if( GUILayout.Button( ">" ) )
				{
					SelectedNote++;
				}
				GUI.enabled = true;

			EditorGUILayout.EndHorizontal();

			//Draw note data
			float newTime = EditorGUILayout.FloatField( "Time", SongPlayer.Song.Notes[ SelectedNote ].Time );
			int newStringIndex = EditorGUILayout.IntField( "String", SongPlayer.Song.Notes[ SelectedNote ].StringIndex );
			float newLength = EditorGUILayout.FloatField( "Length", SongPlayer.Song.Notes[ SelectedNote ].Length );

			newStringIndex = Mathf.Clamp( newStringIndex, 0, 4 );

			//If note has changed, register undo, commit changes and redraw progress view
			if( newTime != SongPlayer.Song.Notes[ SelectedNote ].Time
				|| newStringIndex != SongPlayer.Song.Notes[ SelectedNote ].StringIndex
				|| newLength != SongPlayer.Song.Notes[ SelectedNote ].Length )
			{
				Undo.RegisterUndo( SongPlayer.Song, "Edit Note" );

				SongPlayer.Song.Notes[ SelectedNote ].Time = newTime;
				SongPlayer.Song.Notes[ SelectedNote ].StringIndex = newStringIndex;
				SongPlayer.Song.Notes[ SelectedNote ].Length = newLength;

				RedrawProgressViewTexture();

				Repaint();
			}

			//Remove Note Button
			//15px Space is added to the front to match the default unity style
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space( 15 );
			if( GUILayout.Button( "Remove Note" ) )
			{
				Undo.RegisterUndo( SongPlayer.Song, "Remove Note" );

				SongPlayer.Song.RemoveNote( SelectedNote );
				SelectedNote = -1;
				RedrawProgressViewTexture();
				EditorUtility.SetDirty( target );

				Repaint();
			}
			EditorGUILayout.EndHorizontal();
		}
			
		GUILayout.Label( "Song", EditorStyles.boldLabel );

		//Draw song player controls
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space( 15 );

			string buttonLabel = "Play Song";
			if( IsPlaying() )
			{
				buttonLabel = "Pause Song";
			}

			if( GUILayout.Button( buttonLabel ) )
			{
				OnPlayPauseClicked();
			}
			if( GUILayout.Button( "Stop Song" ) )
			{
				OnStopClicked();
			}
		EditorGUILayout.EndHorizontal();

		//Add playback speed slider
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space( 15 );
			GUILayout.Label( "Playback Speed", EditorStyles.structHeadingLabel );
			GuitarObject.GetComponent<AudioSource>().pitch = GUILayout.HorizontalSlider( GuitarObject.GetComponent<AudioSource>().pitch, 0, 1 );
		EditorGUILayout.EndHorizontal();

		//Draw Use Metronome toggle
		UseMetronome = EditorGUILayout.Toggle( "Metronome", UseMetronome );

		//Draw song editor
		SongViewRect = GUILayoutUtility.GetRect( GUILayoutUtility.GetLastRect().width, SongViewHeight );

		PerformanceTimer.StartTimer( "Draw Background" );
		DrawRectangle( 0, SongViewRect.yMin, SongViewRect.width, SongViewRect.height, Color.black );

		PerformanceTimer.StartTimer( "Draw Progress View" );
		DrawProgressView();

		PerformanceTimer.StartTimer( "Draw Main View" );
		DrawMainView();

		PerformanceTimer.StopTimer();

		DrawEditorPerformancePanel();
	}

	protected void DrawEditorPerformancePanel()
	{
		List<TimerData> Timers = PerformanceTimer.GetTimers();

		GUILayout.Label( "Editor Performance", EditorStyles.boldLabel );

		for( int i = 0; i < Timers.Count; ++i )
		{
			float displayMs = Mathf.Round( Timers[ i ].Time * 10000 ) / 10;
			GUILayout.Label( Timers[ i ].Name + " " + displayMs + "ms" );
		}

		float deltaTime = PerformanceTimer.GetTotalTime();
		float fps = Mathf.Round( 10 / deltaTime ) / 10;
		float msPerFrame = Mathf.Round( deltaTime * 10000 ) / 10;
		GUILayout.Label( "Total " + msPerFrame + "ms/frame (" + fps + "FPS)" );
	}

	protected void UpdateMetronome()
	{
		if( !UseMetronome )
		{
			return;
		}

		if( !IsPlaying() )
		{
			return;
		}

		float currentWholeBeat = Mathf.Floor( SongPlayer.GetCurrentBeat( true ) + 0.05f );
		if( currentWholeBeat > LastMetronomeBeat )
		{
			LastMetronomeBeat = currentWholeBeat;

			MetronomeSource.Stop();
			MetronomeSource.time = 0f;
			MetronomeSource.Play();
		}
	}

	protected void RepaintGui()
	{
		if( IsPlaying() )
		{
			Repaint();
		}
	}

	protected Rect GetProgressViewRect()
	{
		return new Rect( SongViewRect.width - SongViewProgressBarWidth, SongViewRect.yMin, SongViewProgressBarWidth, SongViewRect.height );
	}

	protected bool IsPlaying()
	{
		if( GuitarObject == null )
		{
			return false;
		}

		return GuitarObject.GetComponent<AudioSource>().isPlaying;
	}

	protected void DrawMainView()
	{
		float totalWidth = SongViewRect.width - SongViewProgressBarWidth;

		if( totalWidth < 0 )
		{
			return;
		}

		DrawBeats();
		DrawStrings();
		DrawTimeMarker();
		DrawGridNotesAndHandleMouseClicks();
		DrawNotes();
	}

	protected void DrawTimeMarker()
	{
		float heightOfOneBeat = SongViewRect.height / 6f;

		DrawLine( new Vector2( SongViewRect.xMin, SongViewRect.yMax - heightOfOneBeat )
				, new Vector2( SongViewRect.xMax - SongViewProgressBarWidth, SongViewRect.yMax - heightOfOneBeat )
				, new Color( 1f, 0f, 0f )
				, 4 );
	}

	protected void DrawStrings()
	{
		float totalWidth = SongViewRect.width - SongViewProgressBarWidth;
		float stringDistance = totalWidth / 6;

		for( int i = 0; i < 4; ++i )
		{
			float x = stringDistance * ( i + 1 );
			DrawVerticalLine( new Vector2( x, SongViewRect.yMin )
							, new Vector2( x, SongViewRect.yMax )
							, new Color( 0.4f, 0.4f, 0.4f )
							, 3 );
		}
	}

	protected void DrawNotes()
	{
		//Calculate positioning variables
		float heightOfOneBeat = SongViewRect.height / 6f;
		float totalWidth = SongViewRect.width - SongViewProgressBarWidth;
		float stringDistance = totalWidth / 6;
		
		Note note;

		for( int i = 0; i < SongPlayer.Song.Notes.Count; ++i )
		{
			note = SongPlayer.Song.Notes[ i ];

			if( note.Time > SongPlayer.GetCurrentBeat( true ) + 6.5f )
			{
				//If note is not visible, skip it and draw next note
				continue;
			}

			if( note.Time + note.Length < SongPlayer.GetCurrentBeat( true ) - 0.5f )
			{
				//If note is not visible, skip it and draw next note
				continue;
			}

			//How far has the note progressed
			float progressOnNeck = 1 - ( note.Time - SongPlayer.GetCurrentBeat( true ) ) / 6f;

			//Get note color
			Color color = GuitarObject.GetComponent<GuitarGameplay>().GetColor( note.StringIndex );

			//Get note position
			float y = SongViewRect.yMin + progressOnNeck * SongViewRect.height;
			float x = SongViewRect.xMin + ( note.StringIndex + 1 ) * stringDistance;

			//If note is selected, draw a white rectangle around it
			if( SelectedNote == i )
			{
				DrawRectangle( x - 9, y - 9, 17, 17, new Color( 1f, 1f, 1f ), SongViewRect );
			}

			//Draw note rectangle
			DrawRectangle( x - 7, y - 7, 13, 13, color, SongViewRect );

			//Draw trail
			if( note.Length > 0 )
			{
				float trailYTop = y - note.Length * heightOfOneBeat;
				float trailYBot = y;

				DrawVerticalLine( new Vector2( x, trailYBot ), new Vector2( x, trailYTop ), color, 7, SongViewRect );
			}
		}
	}

	//This function is a little bit iffy, 
	//It not only draws the grey circles which you can click
	//but it also handles the mouse clicks which add/remove notes
	protected void DrawGridNotesAndHandleMouseClicks()
	{
		//Grid notes are only drawn when the song is paused
		if( IsPlaying() )
		{
			return;
		}

		float heightOfOneBeat = SongViewRect.height / 6f;
		float totalWidth = SongViewRect.width - SongViewProgressBarWidth;
		float stringDistance = totalWidth / 6;
		float numNotesPerBeat = 4f;

		//Calculate the offset (from 0 to 1) how far the current beat has progressed
		float beatOffset = SongPlayer.GetCurrentBeat( true );
		beatOffset -= (int)beatOffset;

		//Get the texture of the grey circles
		Texture2D GridNoteTexture = (Texture2D)UnityEngine.Resources.Load( "GridNote", typeof( Texture2D ) );

		//Draw on each of the five strings
		for( int i = 0; i < 4; ++i )
		{
			float x = stringDistance * ( i + 1 );

			for( int j = 0; j < 7 * numNotesPerBeat; ++j )
			{
				float y = SongViewRect.yMax - ( j / numNotesPerBeat - beatOffset ) * heightOfOneBeat;

				//Calculate beat value of this grid position
				float beat = (float)j / numNotesPerBeat + Mathf.Ceil( SongPlayer.GetCurrentBeat( true ) );

				Rect rect = new Rect( x - 7, y - 7, 13, 13 );

				if( beat > SongPlayer.Song.GetLengthInBeats() )
				{
					//Dont draw grid notes beyond song length
					continue;
				}

				if( rect.yMin < SongViewRect.yMin && rect.yMax < SongViewRect.yMin )
				{
					//Dont draw grid notes that are not visible in the current frame
					continue;
				}

				if( rect.yMin > SongViewRect.yMax && rect.yMax > SongViewRect.yMax )
				{
					//Dont draw grid notes that are not visible in the current frame
					continue;
				}
				
				//Clip the draw rectangle to the song view
				rect.yMin = Mathf.Clamp( rect.yMin, SongViewRect.yMin, SongViewRect.yMax );
				rect.yMax = Mathf.Clamp( rect.yMax, SongViewRect.yMin, SongViewRect.yMax );
				
				GUI.DrawTexture( rect, GridNoteTexture, ScaleMode.ScaleAndCrop, true );

				//Correct mouse offset
				y -= heightOfOneBeat;

				//Check if current grid note contains the mouse position
				//MouseUpPosition is set to Vector2( -1337, -1337 ) if no click occured this frame
				if( rect.Contains( MouseUpPosition ) )
				{
					//Correct beat offset in positive space
					if( SongPlayer.GetCurrentBeat( true ) > 0 )
					{
						beat -= 1;
					}

					//Check if a note is already present
					SelectedNote = SongPlayer.Song.GetNoteIndex( beat, i );

					if( SelectedNote == -1 )
					{
						//If note wasn't present, add the note on left mouse button click
						if( LastClickWasRightMouseButton == false )
						{
							Undo.RegisterUndo( SongPlayer.Song, "Add Note" );

							SelectedNote = SongPlayer.Song.AddNote( beat, i );
							EditorUtility.SetDirty( target );
							RedrawProgressViewTexture();
						}
					}
					else
					{
						//If note is present, remove the note on right mouse button click
						if( LastClickWasRightMouseButton )
						{
							Undo.RegisterUndo( SongPlayer.Song, "Remove Note" );

							SongPlayer.Song.RemoveNote( SelectedNote );
							SelectedNote = -1;
							EditorUtility.SetDirty( target );
							RedrawProgressViewTexture();
						}
					}

					Repaint();
				}
			}
		}

		//Reset mouse values
		MouseUpPosition = new Vector2( -1337, -1337 );
		LastClickWasRightMouseButton = false;
	}

	protected void DrawBeats()
	{
		float heightOfOneBeat = SongViewRect.height / 6f;

		//Calculate the offset (from 0 to 1) how far the current beat has progressed
		float beatOffset = SongPlayer.GetCurrentBeat( true );
		beatOffset -= (int)beatOffset;

		for( int i = 0; i < 7; ++i )
		{
			float y = SongViewRect.yMax - ( i - beatOffset ) * heightOfOneBeat;
			DrawLine( new Vector2( SongViewRect.xMin, y )
					, new Vector2( SongViewRect.xMax - SongViewProgressBarWidth, y )
					, new Color( 0.1f, 0.1f, 0.1f )
					, 2, SongViewRect );
		}
	}

	protected void DrawProgressView()
	{
		GUI.DrawTexture( GetProgressViewRect(), ProgressViewTexture );
		DrawProgressViewTimeMarker();
	}

	protected void DrawProgressViewBackground()
	{
		Rect rect  = GetProgressViewRect();
		DrawRectangle( rect.xMin, rect.yMin, rect.width, rect.height, new Color( 0.13f, 0.1f, 0.26f ) );
	}

	protected void DrawProgressViewTimeMarker()
	{
		Rect rect  = GetProgressViewRect();

		float previewProgress = 0f;
		if( GuitarObject && GuitarObject.GetComponent<AudioSource>().clip )
		{
			previewProgress = GuitarObject.GetComponent<AudioSource>().time / GuitarObject.GetComponent<AudioSource>().clip.length;
		}

		float previewProgressTop = rect.yMin + rect.height * ( 1 - previewProgress );
		DrawLine( new Vector2( rect.xMin, previewProgressTop ), new Vector2( rect.xMax + rect.width, previewProgressTop ), Color.red, 2 );
	}

	protected void OnMouseDown( Event e )
	{
		if( GetProgressViewRect().Contains( e.mousePosition ) )
		{
			OnProgressViewClicked( e.mousePosition );
		}
	}

	protected void OnMouseUp( Event e )
	{
		if( SongViewRect.Contains( e.mousePosition ) && !GetProgressViewRect().Contains( e.mousePosition ) )
		{
			OnSongViewMouseUp( e.mousePosition );

			if( e.button == 1 )
			{
				LastClickWasRightMouseButton = true;
			}
		}
	}

	protected void OnSongViewMouseUp( Vector2 mousePosition )
	{
		MouseUpPosition = mousePosition;

		Repaint();
	}

	protected void OnProgressViewClicked( Vector2 mousePosition )
	{
		float progress = 1 - (float)( mousePosition.y - SongViewRect.yMin ) / SongViewRect.height;

		GuitarObject.GetComponent<AudioSource>().time = GuitarObject.GetComponent<AudioSource>().clip.length * progress;
	}

	protected void OnPlayPauseClicked()
	{
		if( IsPlaying() )
		{
			GuitarObject.GetComponent<AudioSource>().Pause();
		}
		else
		{
			GuitarObject.GetComponent<AudioSource>().Play();
		}
	}

	protected void OnStopClicked()
	{
		if( !GuitarObject )
		{
			return;
		}

		GuitarObject.GetComponent<AudioSource>().Stop();
		GuitarObject.GetComponent<AudioSource>().time = 0f;
		LastMetronomeBeat = -Mathf.Ceil( SongPlayer.Song.AudioStartBeatOffset );
	}

	//2D Draw Functions
	//Found on the unity forums: http://forum.unity3d.com/threads/17066-How-to-draw-a-GUI-2D-quot-line-quot/page2
	//Added clipping rectangle
	public static void DrawLine( Vector2 lineStart, Vector2 lineEnd, Color color, int thickness, Rect clip )
	{
		if( ( lineStart.y < clip.yMin && lineEnd.y < clip.yMin )
		 || ( lineStart.y > clip.yMax && lineEnd.y > clip.yMax )
		 || ( lineStart.x < clip.xMin && lineEnd.x < clip.xMin )
		 || ( lineStart.x > clip.xMax && lineEnd.x > clip.xMax ) )
		{
			return;
		}

		lineStart.x = Mathf.Clamp( lineStart.x, clip.xMin, clip.xMax );
		lineStart.y = Mathf.Clamp( lineStart.y, clip.yMin, clip.yMax );

		lineEnd.x = Mathf.Clamp( lineEnd.x, clip.xMin, clip.xMax );
		lineEnd.y = Mathf.Clamp( lineEnd.y, clip.yMin, clip.yMax );

		DrawLine( lineStart, lineEnd, color, thickness );
	}

	public static void DrawLine( Vector2 lineStart, Vector2 lineEnd, Color color, int thickness )
	{
		if( lineStart.x == lineStart.y )
		{
			DrawVerticalLine( lineStart, lineEnd, color, thickness );
		}

		if( !_coloredLineTexture )
		{
			_coloredLineTexture = new Texture2D( 1, 1 );
			_coloredLineTexture.wrapMode = TextureWrapMode.Repeat;
		}

		if( _coloredLineColor != color )
		{
			_coloredLineColor = color;
			_coloredLineTexture.SetPixel( 0, 0, _coloredLineColor );
			_coloredLineTexture.Apply();
		}
		DrawLineStretched( lineStart, lineEnd, _coloredLineTexture, thickness );
	}

	public static void DrawVerticalLine( Vector2 lineStart, Vector2 lineEnd, Color color, int thickness, Rect clip )
	{
		if( ( lineStart.y < clip.yMin && lineEnd.y < clip.yMin )
		 || ( lineStart.y > clip.yMax && lineEnd.y > clip.yMax )
		 || ( lineStart.x < clip.xMin && lineEnd.x < clip.xMin )
		 || ( lineStart.x > clip.xMax && lineEnd.x > clip.xMax ) )
		{
			return;
		}

		lineStart.x = Mathf.Clamp( lineStart.x, clip.xMin, clip.xMax );
		lineStart.y = Mathf.Clamp( lineStart.y, clip.yMin, clip.yMax );

		lineEnd.x = Mathf.Clamp( lineEnd.x, clip.xMin, clip.xMax );
		lineEnd.y = Mathf.Clamp( lineEnd.y, clip.yMin, clip.yMax );

		DrawVerticalLine( lineStart, lineEnd, color, thickness );
	}

	public static void DrawVerticalLine( Vector2 lineStart, Vector2 lineEnd, Color color, int thickness )
	{
		if( lineStart.x != lineEnd.x )
		{
			DrawLine( lineStart, lineEnd, color, thickness );
			return;
		}

		float x = lineStart.x;
		float xOffset = (float)thickness;
		float y = lineStart.y + ( lineEnd.y - lineStart.y ) / 2;
		int newThickness = (int)( Mathf.Abs( Mathf.Floor( lineStart.y - lineEnd.y ) ) );

		DrawLine( new Vector2( x - xOffset / 2, y ), new Vector2( x + xOffset / 2, y ), color, newThickness );
	}

	public static void DrawLineStretched( Vector2 lineStart, Vector2 lineEnd, Texture2D texture, int thickness )
	{
		Vector2 lineVector = lineEnd - lineStart;

		if( lineVector.x == 0 )
		{
			return;
		}

		float angle = Mathf.Rad2Deg * Mathf.Atan( lineVector.y / lineVector.x );

		if( lineVector.x < 0 )
		{
			angle += 180;
		}

		if( thickness < 1 )
		{
			thickness = 1;
		}

		// The center of the line will always be at the center
		// regardless of the thickness.
		int thicknessOffset = (int)Mathf.Ceil( thickness / 2 );

		GUIUtility.RotateAroundPivot( angle, lineStart );

		GUI.DrawTexture( new Rect( lineStart.x,
								 lineStart.y - thicknessOffset,
								 lineVector.magnitude,
								 thickness ),
						texture );

		GUIUtility.RotateAroundPivot( -angle, lineStart );
	}

	private void DrawRectangle( float left, float top, float width, float height, Color color )
	{
		DrawRectangle( new Rect( left, top, width, height ), color );
	}

	private void DrawRectangle( float left, float top, float width, float height, Color color, Rect clip )
	{
		DrawRectangle( new Rect( left, top, width, height ), color, clip );
	}

	private void DrawRectangle( Rect rect, Color color, Rect clip )
	{
		DrawVerticalLine( new Vector2( rect.xMin + rect.width / 2, rect.yMin ), new Vector2( rect.xMin + rect.width / 2, rect.yMax ), color, (int)rect.width, clip );
	}

	private void DrawRectangle( Rect rect, Color color )
	{
		DrawRectangle( rect, color, rect );
	}
}