using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ruminoid.Trimmer.Commands
{
    public static partial class UICommands
    {

        public static RoutedUICommand AddLyrics { get; } = new RoutedUICommand(
            "添加歌词(_A)...",
            "AddLyrics",
            typeof(UICommands),
            new InputGestureCollection(new List<InputGesture>
            {
                new KeyGesture(Key.T, ModifierKeys.Control, "Ctrl+T"),
                new KeyGesture(Key.F5, ModifierKeys.None, "F5")
            }));

        public static RoutedUICommand EditSkipData { get; } = new RoutedUICommand(
            "编辑跳过单字(_E)",
            "EditSkipData",
            typeof(UICommands),
            new InputGestureCollection());

        public static RoutedUICommand ReloadSkipData { get; } = new RoutedUICommand(
            "重载跳过单字(_R)",
            "ReloadSkipData",
            typeof(UICommands),
            new InputGestureCollection());

    }
}
