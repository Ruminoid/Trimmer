using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MetroRadiance.UI;
using Ruminoid.Common.Timing;
using Ruminoid.Trimmer.Models;

namespace Ruminoid.Trimmer.Views
{
    public partial class PlaybackView : INotifyPropertyChanged
    {

        #region PlaybackControl

        private bool _mediaLoaded;

        public bool MediaLoaded
        {
            get => _mediaLoaded;
            set
            {
                _mediaLoaded = value;
                OnPropertyChanged();
            }
        }
        private double _playSpeed = 1;

        public double PlaySpeed
        {
            get => _playSpeed;
            set
            {
                if (value > 3) value = 3;
                if (value < 0.1) value = 0.1;
                _playSpeed = value;
                OnPropertyChanged();
                PlaySpeedChanged?.Invoke(_playSpeed);
            }
        }
        public delegate void PlaySpeedChangedHandler(double speed);
        public event PlaySpeedChangedHandler PlaySpeedChanged;
        public Position Position { get; } = new Position();

        private bool _playing;

        public bool Playing
        {
            get => _playing;
            set
            {
                if (!MediaLoaded) return;
                _playing = value;
                if (value)
                {
                    VideoElement.Play();
                    PlayButtonIconType = "Pause";
                    PlayButtonText = "暂停";
                }
                else
                {
                    VideoElement.Pause();
                    PlayButtonIconType = "Run";
                    PlayButtonText = "播放";
                }
                if (value) ThemeService.Current.ChangeAccent(Accent.Orange);
                else ThemeService.Current.ChangeAccent(Accent.Blue);
                OnPropertyChanged();
            }
        }
        public string _playButtonIconType = "Run";
        public string PlayButtonIconType
        {
            get => _playButtonIconType;
            set
            {
                _playButtonIconType = value;
                OnPropertyChanged();
            }
        }
        public string _playButtonText = "播放";
        public string PlayButtonText
        {
            get => _playButtonText;
            set
            {
                _playButtonText = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
