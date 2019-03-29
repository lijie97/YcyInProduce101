public static class AudioControllerHelper
{
    public static AudioSubItem[ ] _ChooseSubItems( AudioItem audioItem, AudioObject useExistingAudioObj )
    {
        return _ChooseSubItems( audioItem, audioItem.SubItemPickMode, useExistingAudioObj );
    }

    public static AudioSubItem _ChooseSingleSubItem( AudioItem audioItem, AudioPickSubItemMode pickMode, AudioObject useExistingAudioObj )
    {
        return _ChooseSubItems( audioItem, pickMode, useExistingAudioObj )[ 0 ];
    }

    // used by the Inspector script
    public static AudioSubItem _ChooseSingleSubItem( AudioItem audioItem )
    {
        return _ChooseSingleSubItem( audioItem, audioItem.SubItemPickMode, null );
    }

    private static AudioSubItem[ ] _ChooseSubItems( AudioItem audioItem, AudioPickSubItemMode pickMode, AudioObject useExistingAudioObj )
    {
        if ( audioItem.subItems == null ) return null;
        int arraySize = audioItem.subItems.Length;
        if ( arraySize == 0 ) return null;

        int chosen = 0;
        AudioSubItem[ ] chosenItems;

        int lastChosen;

        bool useLastChosenOfExistingAudioObj = !object.ReferenceEquals( useExistingAudioObj, null );

        if ( useLastChosenOfExistingAudioObj ) // don't use Unity's operator here, because it will be called by GaplessAudioClipTransistion
        {
            lastChosen = useExistingAudioObj._lastChosenSubItemIndex;
        }
        else
            lastChosen = audioItem._lastChosen;

        if ( arraySize > 1 )
        {
            switch ( pickMode )
            {
            case AudioPickSubItemMode.Disabled:
                return null;

            case AudioPickSubItemMode.StartLoopSequenceWithFirst:
                if ( useLastChosenOfExistingAudioObj )
                {
                    chosen = ( lastChosen + 1 ) % arraySize;
                }
                else
                {
                    chosen = 0;
                }
                break;

            case AudioPickSubItemMode.Sequence:
                chosen = ( lastChosen + 1 ) % arraySize;
                break;

            case AudioPickSubItemMode.SequenceWithRandomStart:
                if ( lastChosen == -1 )
                {
                    chosen = UnityEngine.Random.Range( 0, arraySize );
                }
                else
                    chosen = ( lastChosen + 1 ) % arraySize;
                break;

            case AudioPickSubItemMode.Random:
                chosen = _ChooseRandomSubitem( audioItem, true, lastChosen );
                break;

            case AudioPickSubItemMode.RandomNotSameTwice:
                chosen = _ChooseRandomSubitem( audioItem, false, lastChosen );
                break;

            case AudioPickSubItemMode.AllSimultaneously:
                chosenItems = new AudioSubItem[ arraySize ];
                for ( int i = 0; i < arraySize; i++ )  // Array.Copy( audioItem.subItems, chosenItems, arraySize );  not working on Flash export
                {
                    chosenItems[ i ] = audioItem.subItems[ i ];
                }

                return chosenItems;

            case AudioPickSubItemMode.TwoSimultaneously:
                chosenItems = new AudioSubItem[ 2 ];
                chosenItems[ 0 ] = _ChooseSingleSubItem( audioItem, AudioPickSubItemMode.RandomNotSameTwice, useExistingAudioObj );
                chosenItems[ 1 ] = _ChooseSingleSubItem( audioItem, AudioPickSubItemMode.RandomNotSameTwice, useExistingAudioObj );
                return chosenItems;

            case AudioPickSubItemMode.RandomNotSameTwiceOddsEvens:
                chosen = _ChooseRandomSubitem( audioItem, false, lastChosen, true );
                break;
            }
        }

        if ( useLastChosenOfExistingAudioObj )
        {
            useExistingAudioObj._lastChosenSubItemIndex = chosen;
        }
        else
            audioItem._lastChosen = chosen;

        //Debug.Log( "chose:" + chosen );
        chosenItems = new AudioSubItem[ 1 ];
        chosenItems[ 0 ] = audioItem.subItems[ chosen ];
        return chosenItems;
    }

    private static int _ChooseRandomSubitem( AudioItem audioItem, bool allowSameElementTwiceInRow, int lastChosen, bool switchOddsEvens = false )
    {
        int arraySize = audioItem.subItems.Length; // is >= 2 at this point 
        int chosen = 0;

        float probRange;
        float lastProb = 0;

        if ( !allowSameElementTwiceInRow )
        {
            // find out probability of last chosen sub item
            if ( lastChosen >= 0 )
            {
                lastProb = audioItem.subItems[ lastChosen ]._SummedProbability;
                if ( lastChosen >= 1 )
                {
                    lastProb -= audioItem.subItems[ lastChosen - 1 ]._SummedProbability;
                }
            }
            else
                lastProb = 0;

            probRange = 1.0f - lastProb;
        }
        else
            probRange = 1.0f;

        float rnd = UnityEngine.Random.Range( 0, probRange );

        int i;
        for ( i = 0; i < arraySize - 1; i++ )
        {
            float prob;

            prob = audioItem.subItems[ i ]._SummedProbability;

            if ( switchOddsEvens )
            {
                if ( isOdd( i ) == isOdd( lastChosen ) )
                {
                    continue;
                }
            }

            if ( !allowSameElementTwiceInRow )
            {
                if ( i == lastChosen )
                {
                    if ( prob != 1.0f || !audioItem.subItems[ i ].DisableOtherSubitems ) // DisableOtherSubitems allows the same be played twice in a row 
                    {
                        continue; // do not play same audio twice
                    }
                }

                if ( i > lastChosen )
                {
                    prob -= lastProb;
                }
            }

            if ( prob > rnd )
            {
                chosen = i;
                break;
            }
        }
        if ( i == arraySize - 1 )
        {
            chosen = arraySize - 1;
        }

        return chosen;
    }

    static bool isOdd( int i )
    {
        return i % 2 != 0;
    }
}