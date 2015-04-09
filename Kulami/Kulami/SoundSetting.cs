using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kulami
{
    public static class SoundSetting
    {
        private static bool soundOn;

        public static bool SoundOn
        {
            get { return SoundSetting.soundOn; }
            set { SoundSetting.soundOn = value; }
        }

        private static bool musicOn;

        public static bool MusicOn
        {
            get { return SoundSetting.musicOn; }
            set { SoundSetting.musicOn = value; }
        }

        static SoundSetting()
        {
            soundOn = true;
            musicOn = true;
        }
    }
}
