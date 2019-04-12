using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class setSongList  {


    private static List<string> songList = new List<string>() ;
  
    public static void _addSongList(string songName)
    {
        if (songList.Contains(songName))
            return;       
        songList.Add(songName);
    }


    public static List<string> GetSongList
    {
        get { return songList; }
    }


    public static void ClearSongList()
    {
        songList.Clear();
    }


}
