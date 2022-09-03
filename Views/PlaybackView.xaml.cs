﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Ruminoid.Trimmer.LibAss;
using Unosquare.FFME.Common;
using YDock.Interface;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using Ruminoid.Trimmer.Commands;
using Ruminoid.Trimmer.Models;

namespace Ruminoid.Trimmer.Views
{
    /// <summary>
    /// PlaybackView.xaml 的交互逻辑
    /// </summary>
    public partial class PlaybackView : UserControl, IDockSource
    {

        public PlaybackView()
        {
            InitializeComponent();            
            VideoElement.PositionChanged += (o, args) =>
                Position.Time = (long)args.Position.TotalMilliseconds;
            VideoElement.MediaOpened += (o, args) =>
                Position.Total = (long)args.Info.Duration.TotalMilliseconds;
            VideoElement.MediaFailed += (o, args) => Console.WriteLine(@"[FFME] MediaFailed : " + args.ErrorException);
            VideoElement.MediaEnded += async (o, args) =>
            {
                //await VideoElement.Seek(TimeSpan.Zero);
                //await VideoElement.Play();
            };
            //VideoElement.RenderingVideo += RenderPreviewOnVideo;
            Position.OnPositionActiveChanged += () => SeekToPosition(Position.Time);
            PlaySpeedChanged += PlaybackView_PlaySpeedChanged;
        }

        private void PlaybackView_PlaySpeedChanged(double speed)
        {
            VideoElement.SpeedRatio = speed;
        }

        private void RenderPreviewOnVideo(object sender, RenderingVideoEventArgs e)
        {
            //_libAss.RenderAndBlend((int)e.Clock.TotalMilliseconds, e.Bitmap);
        }

        #region Current

        public static PlaybackView Current { get; } = new PlaybackView();

        #endregion

        //private LibASSContext _libAss = new LibASSContext();

        #region DockSource

        public IDockControl DockControl { get; set; }
        public string Header => "回放";
        public ImageSource Icon => null;

        #endregion

        public long PositionRealTime => (long)VideoElement.Position.TotalMilliseconds;

        private void SeekToPosition(long time)
        {
            var target = TimeSpan.FromMilliseconds(time);
            if (target.TotalMilliseconds < 0)
                target = TimeSpan.Zero;
            VideoElement.Seek(target).GetAwaiter().GetResult();
            Position.Time = (long)target.TotalMilliseconds;
        }

        public void JumpDuration(long duration)
        {
            var target = VideoElement.Position + TimeSpan.FromMilliseconds(duration);
            if (target.TotalMilliseconds < 0)
                target = TimeSpan.Zero;
            VideoElement.Seek(target).GetAwaiter().GetResult();
            Position.Time = (long)target.TotalMilliseconds;
        }
    }
}
