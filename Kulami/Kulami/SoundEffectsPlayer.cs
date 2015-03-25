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
        public void Mute()
        {
            soundEffectsMediaPlayer.IsMuted = true;
        }

        public void UnMute()
        {
            soundEffectsMediaPlayer.IsMuted = false;
        }
        public void MakeMoveSound()
        {
            soundEffectsMediaPlayer.Close();
            string MovePath = startupPath + "/sound/soundEffects/move.wav";
            soundEffectsMediaPlayer.Open(new Uri(MovePath));
            soundEffectsMediaPlayer.Play();
        }

        public void PlayStartGameSound()
        {
            soundEffectsMediaPlayer.Close();
            string MovePath = startupPath + "/sound/soundEffects/GameStart.wav";
            soundEffectsMediaPlayer.Open(new Uri(MovePath));
            soundEffectsMediaPlayer.Play();
        }


        public void ControlSectorSound()
        {
            soundEffectsMediaPlayer.Close();
            string GetPointPath = startupPath + "/sound/soundEffects/GetPoint.wav";
            soundEffectsMediaPlayer.Open(new Uri(GetPointPath));
            soundEffectsMediaPlayer.Play();
        }

        public void ButtonSound()
        {
            soundEffectsMediaPlayer.Close();
            string ButtonPath = startupPath + "/sound/soundEffects/button.wav";
            soundEffectsMediaPlayer.Open(new Uri(ButtonPath));
            soundEffectsMediaPlayer.Play();
        }

        public void WinSound()
        {
            soundEffectsMediaPlayer.Close();
            string WinPath = startupPath + "/sound/soundEffects/win.wav";
            soundEffectsMediaPlayer.Open(new Uri(WinPath));
            soundEffectsMediaPlayer.Play();
        }

        public void LostSound()
        {
            soundEffectsMediaPlayer.Close();
            string LostPath = startupPath + "/sound/soundEffects/lost.wav";
            soundEffectsMediaPlayer.Open(new Uri(LostPath));
            soundEffectsMediaPlayer.Play();
        }
    }
}
