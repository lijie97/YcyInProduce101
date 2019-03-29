#if UNITY_EDITOR // Unity bug workaround: this way this file can be in subdirectorey of Standard Assets

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class AudioItemOverview : EditorWindow
{
    [MenuItem( "Window/Audio Toolkit/Item Overview" )]
    static void ShowWindow()
    {
        EditorWindow.GetWindow( typeof( AudioItemOverview ) );
    }

    static Vector2 _scrollPos;

    private Texture2D _listItemBackgroundNormal0 = null;
    private Texture2D _listItemBackgroundNormal1 = null;
    private Texture2D _listItemBackgroundClicked = null;
    private Texture2D _categoryBackgroundNormal = null;
    private Texture2D _categoryBackgroundClicked = null;

    private AudioController _selectedAC;
    private int _selectedACIndex;
    private AudioController[ ] _audioControllerList;
    private string[ ] _audioControllerNameList;

    private string _searchString;

    private bool isInitialised = false;

    private static Dictionary<string, bool> _lastFoldedOutCategories;
    private Dictionary<string, bool> _foldedOutCategories = new Dictionary<string, bool>();
    private GUIStyle _headerStyleButton;
    private GUIStyle _styleEmptyButton;
    private GUIStyle _styleListItemButton0;
    private GUIStyle _styleListItemButton1;
    private GUIStyle _styleCategoryButtonHeader;
    private GUIStyle _headerStyle;
    private int _buttonSize;

    public void Show( AudioController audioController )
    {
        if ( !isInitialised ) Initialise();

        _SetCurrentAudioController( audioController );
        _FindAudioController();
        base.Show();
    }

    void OnGUI()
    {
        if ( !isInitialised ) Initialise();

        if ( !_selectedAC )
        {
            _SetCurrentAudioController( _FindAudioController() );
        }

        if ( _selectedAC == null && Selection.activeGameObject != null )
        {
            _SetCurrentAudioController( Selection.activeGameObject.GetComponent<AudioController>() );
        }

        if ( _audioControllerNameList == null )
        {
            _FindAudioController();

            if ( _audioControllerNameList == null && _selectedAC != null ) // can happen if AC was selected by Show( AC )
            {
                _audioControllerNameList = new string[ 1 ] { _GetPrefabName( _selectedAC ) };
            }
        }

        if ( !_selectedAC )
        {
            EditorGUILayout.LabelField( "No AudioController found!" );
            return;
        }

        EditorGUILayout.BeginHorizontal();

        int newACIndex = EditorGUILayout.Popup( _selectedACIndex, _audioControllerNameList, _headerStyleButton );
        if ( newACIndex != _selectedACIndex )
        {
            _selectedACIndex = newACIndex;
            _SetCurrentAudioController( _audioControllerList[ _selectedACIndex ] );
            _SelectCurrentAudioController();
        }

        if ( _foldoutsSetFromController != _selectedAC )
        {
            _SetCategoryFoldouts();
        }

        if ( _searchString == null )
        {
            _searchString = "";
        }

        _searchString = EditorGUILayout.TextField( "                  search item: ", _searchString );
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Button( "      ", _styleEmptyButton );
        EditorGUILayout.LabelField( "Item", _headerStyle );
        EditorGUILayout.LabelField( "Sub Item", _headerStyle );

        EditorGUILayout.EndHorizontal();

        _scrollPos = EditorGUILayout.BeginScrollView( _scrollPos );

        if ( _selectedAC != null && _selectedAC.AudioCategories != null )
        {
            for ( int categoryIndex = 0; categoryIndex < _selectedAC.AudioCategories.Length; categoryIndex++ )
            {
                var category = _selectedAC.AudioCategories[ categoryIndex ];

                if ( string.IsNullOrEmpty( category.Name ) )
                {
                    Debug.LogWarning( "empty category.Name" );
                    continue;
                }


                if ( !_foldedOutCategories.ContainsKey( category.Name ) )
                {
                    Debug.LogWarning( "can not find category.Name" + category.Name );
                    continue;
                }

                EditorGUILayout.BeginHorizontal();

                if ( GUILayout.Button( ( _foldedOutCategories[ category.Name ] ? "-\t" : "+\t" ) + category.Name, _styleCategoryButtonHeader ) )
                {
                    _foldedOutCategories[ category.Name ] = !_foldedOutCategories[ category.Name ];
                }

                EditorGUILayout.EndHorizontal();

                var filteredAudioItems = new List<AudioItem>( category.AudioItems );
                if ( !string.IsNullOrEmpty( _searchString ) )
                {
                    bool catFoldedOut = false;
                    for ( int i = 0; i < filteredAudioItems.Count; ++i )
                    {
                        AudioItem item = filteredAudioItems[ i ];
                        if ( !item.Name.ToLowerInvariant().Contains( _searchString.ToLowerInvariant() ) )
                        {
                            filteredAudioItems.RemoveAt( i-- );
                        }
                        else
                        {
                            catFoldedOut = true;
                        }
                    }
                    _foldedOutCategories[ category.Name ] = catFoldedOut && filteredAudioItems.Count > 0;
                }

                if ( _foldedOutCategories[ category.Name ] )
                {
                    if ( category.AudioItems == null ) continue;

                    var sortedAudioItems = filteredAudioItems.OrderBy( x => x.Name ).ToArray();

                    for ( int itemIndex = 0; itemIndex < sortedAudioItems.Length; itemIndex++ )
                    {
                        var item = sortedAudioItems[ itemIndex ];
                        string emptySpace = "      ";

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Button( emptySpace, _styleEmptyButton );
                        GUILayout.Label( item.Name );
                        EditorGUILayout.EndHorizontal();

                        if ( item.subItems == null )
                        {
                            continue;
                        }
                        for ( int subitemIndex = 0; subitemIndex < item.subItems.Length; subitemIndex++ )
                        {
                            var subItem = item.subItems[ subitemIndex ];

                            GUILayout.BeginHorizontal();
                            GUILayout.Button( emptySpace, _styleEmptyButton );
                            EditorGUILayout.BeginHorizontal();

                            string listItemDisplay = "";
                            string listItemTypeDisplay = "     ";

                            if ( subItem.SubItemType == AudioSubItemType.Clip )
                            {
                                if ( subItem.Clip != null )
                                {
                                    listItemDisplay = subItem.Clip.name;
                                    listItemTypeDisplay += "CLIP:";
                                }
                                else
                                {
                                    listItemDisplay = "*unset*";
                                    listItemTypeDisplay += "CLIP:";
                                }
                            }
                            else
                            {
                                listItemTypeDisplay += "ITEM:";
                                listItemDisplay = subItem.ItemModeAudioID;
                            }

                            EditorGUILayout.LabelField( listItemTypeDisplay, GUILayout.MaxWidth( _buttonSize ) );

                            if ( GUILayout.Button( listItemDisplay, subitemIndex % 2 == 0 ? _styleListItemButton0 : _styleListItemButton1, GUILayout.ExpandWidth( true ) ) )
                            {
                                _selectedAC._currentInspectorSelection.currentCategoryIndex = categoryIndex;
                                _selectedAC._currentInspectorSelection.currentItemIndex = Array.FindIndex( category.AudioItems, x => x.Name == item.Name );
                                _selectedAC._currentInspectorSelection.currentSubitemIndex = subitemIndex;
                                _SelectCurrentAudioController();
                            }

                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
            }
        }

        EditorGUILayout.EndScrollView();

    }

    private void _SetCurrentAudioController( AudioController ac )
    {
        _selectedAC = ac;
        _SetCategoryFoldouts();
    }

    AudioController _foldoutsSetFromController;

    void _SetCategoryFoldouts()
    {
        _foldoutsSetFromController = _selectedAC;
        if ( _selectedAC == null ) return;
        for ( int i = 0; i < _selectedAC.AudioCategories.Length; ++i )
        {
            var name = _selectedAC.AudioCategories[ i ].Name;
            if ( !_foldedOutCategories.ContainsKey( name ) )
            {
                _foldedOutCategories[ name ] = false;
            }
        }
        
    }

    private void _SelectCurrentAudioController()
    {
        var gos = new GameObject[ 1 ];
        gos[ 0 ] = _selectedAC.gameObject;
        Selection.objects = gos;
    }

    private AudioController _FindAudioController()
    {
        _audioControllerList = FindObjectsOfType( typeof( AudioController ) ) as AudioController[ ];
        if ( _audioControllerList != null && _audioControllerList.Length > 0 )
        {
            _audioControllerNameList = new string[ _audioControllerList.Length ];
            _selectedACIndex = -1;
            for ( int i = 0; i < _audioControllerList.Length; i++ )
            {
                _audioControllerNameList[ i ] = _audioControllerList[ i ].name;
                if ( _selectedAC == _audioControllerList[ i ] )
                {
                    _selectedACIndex = i;
                }
            }
            if ( _selectedACIndex == -1 )
            {
                if ( _selectedAC != null )
                {
                    ArrayHelper.AddArrayElement<string>( ref _audioControllerNameList, _GetPrefabName( _selectedAC ) );
                    ArrayHelper.AddArrayElement<AudioController>( ref _audioControllerList, _selectedAC );
                    _selectedACIndex = _audioControllerNameList.Length - 1;
                }
                else
                {
                    _selectedACIndex = 0;
                }
            }

            if ( _selectedACIndex >= 0 )
            {

                AudioController controller = _audioControllerList[ _selectedACIndex ];
                return controller;
            }
            else
            {
                return null;
            }
        }
        return null;
    }

    private string _GetPrefabName( AudioController _selectedAC )
    {
        return "PREFAB: " + _selectedAC.name;
    }

    private static Texture2D MakeTex( int width, int height, Color col )
    {
        Color[ ] pix = new Color[ width * height ];

        for ( int i = 0; i < pix.Length; i++ )
            pix[ i ] = col;

        Texture2D result = new Texture2D( width, height );
        result.SetPixels( pix );
        result.Apply();

        return result;
    }

    private void Initialise()
    {
        isInitialised = true;
        // header

        Color gray = Color.gray * 0.6f;
        Color lighterGray = gray * 1.2f;
        Color evenLighterGray = lighterGray * 1.2f;
        _listItemBackgroundNormal0 = MakeTex( 32, 32, gray );
        _listItemBackgroundNormal1 = MakeTex( 32, 32, lighterGray );
        _listItemBackgroundClicked = MakeTex( 32, 32, evenLighterGray );

        Color blue = new Color( 0f, 0f, 0.2f );
        Color lighterBlue = blue * 1.4f;
        _categoryBackgroundNormal = MakeTex( 32, 32, blue );
        _categoryBackgroundClicked = MakeTex( 32, 32, lighterBlue );

        _buttonSize = 80;

        _headerStyle = new GUIStyle( EditorStyles.boldLabel );
        _headerStyleButton = new GUIStyle( EditorStyles.popup );
        //headerStyleButton.fixedWidth = 350;

        UnityEngine.Color acColor;

        bool isDarkSkin = _headerStyleButton.normal.textColor.grayscale > 0.5f;

        if ( isDarkSkin )
        {
            acColor = new Color( 0.9f, 0.9f, 0.5f );
        }
        else
            acColor = new Color( 0.6f, 0.1f, 0.0f );

        _headerStyleButton.normal.textColor = acColor;
        _headerStyleButton.focused.textColor = acColor;
        _headerStyleButton.active.textColor = acColor;
        _headerStyleButton.hover.textColor = acColor;

        GUIStyle styleButton = new GUIStyle( EditorStyles.miniButton );
        styleButton.fixedWidth = _buttonSize;

        _styleEmptyButton = new GUIStyle( styleButton );
        _styleEmptyButton.normal = _headerStyle.normal;
        _styleEmptyButton.focused = _headerStyle.focused;
        _styleEmptyButton.active = _headerStyle.active;
        _styleEmptyButton.hover = _headerStyle.hover;

        _styleListItemButton0 = new GUIStyle( EditorStyles.miniButton );
        _styleListItemButton0.normal.background = _listItemBackgroundNormal0;
        _styleListItemButton0.active.background = _listItemBackgroundClicked;

        _styleListItemButton1 = new GUIStyle( _styleListItemButton0 );
        _styleListItemButton1.normal.background = _listItemBackgroundNormal1;
        _styleListItemButton1.active.background = _listItemBackgroundClicked;

        _styleCategoryButtonHeader = new GUIStyle( EditorStyles.miniButton );
        _styleCategoryButtonHeader.alignment = TextAnchor.MiddleLeft;
        _styleCategoryButtonHeader.stretchWidth = true;
        _styleCategoryButtonHeader.fontStyle = FontStyle.Bold;
        _styleCategoryButtonHeader.normal.background = _categoryBackgroundNormal;
        _styleCategoryButtonHeader.active.background = _categoryBackgroundClicked;

        if ( _lastFoldedOutCategories != null )
        {
            foreach( var catName in _lastFoldedOutCategories )
            {
                _foldedOutCategories[ catName.Key ] = catName.Value;
            }
        }

        _lastFoldedOutCategories = _foldedOutCategories;
    }
}
#endif