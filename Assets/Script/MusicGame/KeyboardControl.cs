using UnityEngine;
using System.Collections;

public class KeyboardControl : MonoBehaviour
{
	//This constant is only used in this class and cannot be adjusted to work with more or less strings
	//This feature is planned for the future
	const int NumStrings = 4;

	//The five button objects in the scene
	protected GameObject[] StringButtons;

	//Stores if the button is held down
	protected bool[] ButtonsPressed;

	//Stores if the button was just pressed in this frame
	protected bool[] ButtonsJustPressed;

	//KeyCodes that control the five string buttons
	protected KeyCode[] StringKeys;

	//Use this for initialization
	void Start()
	{
		ButtonsPressed = new bool[ NumStrings ];
		ButtonsJustPressed = new bool[ NumStrings ];

		for( int i = 0; i < NumStrings; ++i )
		{
			ButtonsPressed[ i ] = false;
			ButtonsJustPressed[ i ] = false;
		}

		UpdateStringKeyArray();
		SaveReferencesToStringButtons();
	}

	protected void UpdateStringKeyArray()
	{
		StringKeys = new KeyCode[ NumStrings ];

		for( int i = 0; i < NumStrings; ++i )
		{
			StringKeys[ i ] = GameObject.Find( "StringButton" + ( i + 1 ) ).GetComponent<StringButton>().Key;
		}
	}

	void SaveReferencesToStringButtons()
	{
		StringButtons = new GameObject[ NumStrings ];

		for( int i = 0; i < NumStrings; ++i )
		{
			StringButtons[ i ] = GameObject.Find( "StringButton" + ( i + 1 ) );
		}
	}

	//Update is called once per frame
	void LateUpdate()
	{
		ProcessKeyInput();
	}

	void ProcessKeyInput()
	{
		ResetButtonsJustPressedArray();

		for( int i = 0; i < NumStrings; ++i )
		{
			CheckKeyCode( StringKeys[ i ], i );
		}
	}

	void CheckKeyCode( KeyCode code, int stringIndex )
	{
		if( Input.GetKeyDown( code ) )
		{
			OnStringChange( stringIndex, true );
		}
		if( Input.GetKeyUp( code ) )
		{
			OnStringChange( stringIndex, false );
		}
		if( Input.GetKey( code ) && !ButtonsPressed[ stringIndex ] )
		{
			OnStringChange( stringIndex, true );
		}
	}


    public void BtnClickUp(int stringIndex)
    {
        OnStringChange(stringIndex, false);
    }

    public void BtnClickDown(int stringIndex)
    {
        OnStringChange(stringIndex, true);
        if (!ButtonsPressed[stringIndex])
        {
            OnStringChange(stringIndex, true);
        }
    }


	protected void ResetButtonsJustPressedArray()
	{
		for( int i = 0; i < NumStrings; ++i )
		{
			ButtonsJustPressed[ i ] = false;
		}
	}

	protected int GetNumButtonsPressed()
	{
		int pressed = 0;

		for( int i = 0; i < NumStrings; ++i )
		{
			if( ButtonsPressed[ i ] )
			{
				pressed++;
			}
		}

		return pressed;
	}

	public GameObject GetStringButton( int index )
	{
		return StringButtons[ index ];
	}

	public bool IsButtonPressed( int index )
	{
		return ButtonsPressed[ index ];
	}

	public bool WasButtonJustPressed( int index )
	{
		return ButtonsJustPressed[ index ];
	}

	void OnStringChange( int stringIndex, bool pressed )
	{
		Vector3 stringButtonPosition = StringButtons[ stringIndex ].transform.Find( "Paddle" ).transform.position;
		
		if( pressed )
		{
			//Only press this if less then two buttons are already pressed
			//The keyboard limits multiple key presses arbitrarily, sometimes its 2, sometimes 3
			//So I locked it to a maximum of two key presses at the same time for consistency
			if( GetNumButtonsPressed() < 2 )
			{
				//Move the paddle upwards
				stringButtonPosition.y = 0.16f;

				//Enable the light
				StringButtons[ stringIndex ].transform.Find( "Light" ).GetComponent<Light>().enabled = true;
			}

            //Update key state
            ButtonsPressed[stringIndex] = true;
            ButtonsJustPressed[stringIndex] = true;
        }
		else
		{
			//Move paddle down
			stringButtonPosition.y = -0.06f;

			//Disable light
			StringButtons[ stringIndex ].transform.Find( "Light" ).GetComponent<Light>().enabled = false;

			//Update key state
			ButtonsPressed[ stringIndex ] = false;
		}

		//Set paddle position
		StringButtons[ stringIndex ].transform.Find( "Paddle" ).transform.position = stringButtonPosition;
	}
}