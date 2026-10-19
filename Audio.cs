using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Media;
using System.Text;

namespace fpdungeonCrawler
{
    public class Audiomanager
    {        

        public void PlayAudio(UnmanagedMemoryStream stream)
        {            
            WaveFileReader audiofile = new (stream);
            WaveOut player = new();
            player.Init(audiofile);
            player.Play();
        }

        public void PlayLoop(UnmanagedMemoryStream stream)
        {
            WaveFileReader audiofile = new(stream);
            WaveOut musicplayer = new();
            musicplayer.Init(audiofile);
            musicplayer.Play();
        }
    }
}
