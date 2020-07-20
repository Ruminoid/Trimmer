﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml.Linq;
using Enterwell.Clients.Wpf.Notifications;
using Ruminoid.Common.Helpers;
using Ruminoid.Trimmer.Commands;
using Ruminoid.Trimmer.Views;

namespace Ruminoid.Trimmer.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;

            Closing += OnClosing;

            Closed += (sender, args) => Application.Current.Shutdown(0);

            AddCommandBindings();

            #region Document Register

            DockManager.RegisterDocument(LyricEditorView.Current);
            DockManager.RegisterDock(PlaybackView.Current);

            #endregion
        }

        #region Notifications

        public NotificationMessageManager NotificationManager { get; } = new NotificationMessageManager();

        #endregion

        #region Closing

        private void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = ExitApp();
            if (e.Cancel) return;
            LayoutHelper<Config>.SaveLayoutAndDispose(DockManager, this);
            ConfigHelper<Config>.SaveConfig(Config.Current);
        }

        #endregion

        #region Loaded

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hwnd).AddHook(WndProc);
            wndList = new List<FrameworkElement>
                {Wnd1, Wnd2, Wnd3, Wnd4, Wnd5, Wnd6, WwUp, WwMiddle, WwLeft, WwRight, WwDown, WwSkip, WwReturn};

            PlaybackView.Current.DockControl.Show();
            if (!LayoutHelper<Config>.ApplyLayout(DockManager, this)) LyricEditorView.Current.DockControl.Show();

            //sliderBinding = new Binding();
            //sliderBinding.Source = PlaybackView.Current.Position;
            //sliderBinding.Path = new PropertyPath("Percentage");

            if(Config.Current.HideWelcome) return;

            CheckBox welcomeBox = new CheckBox
            {
                Margin = new Thickness(12, 8, 12, 8),
                HorizontalAlignment = HorizontalAlignment.Left,
                Content = "不要再提醒。"
            };
            welcomeBox.Checked += (o, args) => Config.Current.HideWelcome = true;
            welcomeBox.Unchecked += (o, args) => Config.Current.HideWelcome = false;
            NotificationManager
                .CreateMessage()
                .Accent("#007ACC")
                .Background("#333")
                .HasMessage("欢迎回来！点击“添加歌词”或轻敲 Ctrl+T 以开始。")
                .Dismiss().WithButton("添加歌词", button =>
                {
                    LyricEditorView.Current.AddLyrics_Executed(null, null);
                })
                .Dismiss().WithButton("消除", button => { })
                .WithAdditionalContent(ContentLocation.Bottom,
                    new Border
                    {
                        BorderThickness = new Thickness(0, 1, 0, 0),
                        BorderBrush = new SolidColorBrush(Color.FromArgb(128, 28, 28, 28)),
                        Child = welcomeBox
                    })
                .Queue();
        }

        #endregion

        #region CaptionBar Hook

        private List<FrameworkElement> wndList;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_NCHITTEST)
            {
                if (wndList is null) return IntPtr.Zero;
                Point p = new Point();
                int pInt = lParam.ToInt32();
                p.X = (pInt << 16) >> 16;
                p.Y = pInt >> 16;
                if (WndCaption.PointFromScreen(p).Y > WndCaption.ActualHeight) return IntPtr.Zero;
                foreach (FrameworkElement element in wndList)
                {
                    Point rel = element.PointFromScreen(p);
                    if (rel.X >= 0 && rel.X <= element.ActualWidth && rel.Y >= 0 && rel.Y <= element.ActualHeight)
                    {
                        return IntPtr.Zero;
                    }
                }
                handled = true;
                return new IntPtr(2);
            }

            return IntPtr.Zero;
        }

        private const int WM_NCHITTEST = 0x0084;

        #endregion

        #region Utilities

        private void ToolBar_RemoveGrid(object sender, RoutedEventArgs e)
        {

            ToolBar toolBar = sender as ToolBar;
            if (toolBar?.Template.FindName("OverflowGrid", toolBar) is FrameworkElement overflowGrid)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            if (toolBar?.Template.FindName("MainPanelBorder", toolBar) is FrameworkElement mainPanelBorder)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }

        }

        #endregion

        #region Slider

        private Binding sliderBinding;

        private void Slider_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //BindingOperations.ClearBinding(Wnd6, RangeBase.ValueProperty);
            //Wnd6.Value = PlaybackView.Current.Position.Percentage;
            //Wnd6.ValueChanged += SliderOnValueChanged;
        }

        private void SliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //PlaybackView.Current.SeekToPosition((long)(e.NewValue * PlaybackView.Current.Position.Total));
        }

        private void Slider_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //Wnd6.ValueChanged -= SliderOnValueChanged;
            //Wnd6.SetBinding(RangeBase.ValueProperty, sliderBinding);
        }

        #endregion

    }
}
