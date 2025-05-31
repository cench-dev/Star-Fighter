using System.Media;

namespace StarFighter
{
    internal static class MusicPlayer
    {
        private static SoundPlayer musicPlayer;

        public static void Play()
        {
            musicPlayer = new SoundPlayer("Main_Theme.wav");
            musicPlayer.PlayLooping();
        }

        public static void Pause() => musicPlayer.Stop();
    }
}