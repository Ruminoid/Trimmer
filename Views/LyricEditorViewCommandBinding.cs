﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ruminoid.Common.Timing;
using Ruminoid.Trimmer.Commands;
using Ruminoid.Trimmer.Dialogs;
using Ruminoid.Trimmer.Models;

namespace Ruminoid.Trimmer.Views
{
    public partial class LyricEditorView
    {

        public void AddCommandBindings()
        {
            Application.Current.MainWindow?.CommandBindings.Add(new CommandBinding(
                UICommands.AddLyrics,
                AddLyrics_Executed,
                CanExecute));
            Application.Current.MainWindow?.CommandBindings.Add(new CommandBinding(
                UICommands.ClearLyrics,
                ClearLyrics_Executed,
                CanExecute));
        }

        public void ClearLyrics_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(MessageBox.Show("确认清空所有歌词吗？", "确认", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                LrcModel.Current.ClearAll();
            }
        }

        public void AddLyrics_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditLineDialog.ShowAddDialog();
            string data = EditLineDialog.GetData();
            if (string.IsNullOrEmpty(data)) return;
            LrcModel.Current.AddLyrics(data, EditLineDialog.GetIsDualLanguage());
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        #region Operations

        public void Apply() => LrcModel.Current.Apply(new Position(PlaybackView.Current.PositionRealTime));

        public void Undo() => LrcModel.Current.Undo();

        public void Skip() => LrcModel.Current.Skip();

        public void Break() => LrcModel.Current.Break(new Position(PlaybackView.Current.PositionRealTime));

        private void EditLineButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button s = sender as Button;
            if (!(s?.DataContext is LrcLine line)) return;
            EditLineDialog.ShowEditDialog((line.Origin ?? "").Replace("\r", "").Replace("\n", ""));
            string data = EditLineDialog.GetData();
            if (string.IsNullOrEmpty(data)) return;
            LrcModel.Current.ResetLineData(line, data);
        }

        private void DeleteLineButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "删除这行？",
                "删除",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);
            if (result != MessageBoxResult.Yes) return;
            if (!(sender is Button s)) return;
            LrcLine line = s.DataContext as LrcLine;
            LrcModel.Current.RemoveLine(line);
        }

        #endregion

        #region Auto Scroll

        private void OnSetTargeting(LrcLine lrcline)
        {
            try
            {
                RootView.ScrollIntoView(lrcline);
            }
            catch (Exception)
            {
                // Ignore
            }
        }

        #endregion

    }
}
