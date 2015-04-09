using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kulami
{
    public class SoundEffectsPlayer
    {
        private MediaPlayer soundEffectsMediaPlayer = new MediaPlayer();
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public void Close()
        {
            soundEffectsMediaPlayer.Close();
        }
        public void Stop()
        {
            soundEffectsMediaPlayer.Stop();
        }
        public void Mute()
        {
            soundEffectsMediaPlayer.IsMuted = true;
        }

        public void UnMute()
        {
            soundEffectsMediaPlayer.IsMuted = false;
        }
        public void MakeMoveSound(string move)
        {
            if (SoundSetting.SoundOn)
            {
                soundEffectsMediaPlayer.Close();
                string MovePath;
                string color = move[0].ToString();
                if (color == "R")
                    MovePath = startupPath + "/sound/soundEffects/move.wav";
                else
                    MovePath = startupPath + "/sound/soundEffects/move3.wav";
                soundEffectsMediaPlayer.Open(new Uri(MovePath));
                soundEffectsMediaPlayer.Play();
            }
        }

        public void PlayStartGameSound()
        {
            if (SoundSetting.SoundOn)
            {
                soundEffectsMediaPlayer.Close();
                string MovePath = startupPath + "/sound/soundEffects/GameStart new.wav";
                soundEffectsMediaPlayer.Open(new Uri(MovePath));
                soundEffectsMediaPlayer.Play();
            }
        }

        public void ButtonSound()
        {
            if (SoundSetting.SoundOn)
            {
                soundEffectsMediaPlayer.Close();
                string ButtonPath = startupPath + "/sound/soundEffects/button.wav";
                soundEffectsMediaPlayer.Open(new Uri(ButtonPath));
                soundEffectsMediaPlayer.Play();
            }
        }

        public void WinSound()
        {
            if (SoundSetting.SoundOn)
            {
                soundEffectsMediaPlayer.Close();
                string WinPath = startupPath + "/sound/soundEffects/win.wav";
                soundEffectsMediaPlayer.Open(new Uri(WinPath));
                soundEffectsMediaPlayer.Play();
            }
        }

        public void LostSound()
        {
            if (SoundSetting.SoundOn)
            {
                soundEffectsMediaPlayer.Close();
                string LostPath = startupPath + "/sound/soundEffects/lost.wav";
                soundEffectsMediaPlayer.Open(new Uri(LostPath));
                soundEffectsMediaPlayer.Play();
            }
        }
    }
}
