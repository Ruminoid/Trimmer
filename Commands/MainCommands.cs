using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ruminoid.Trimmer.Commands
{
    public static partial class UICommands
    {

        public static RoutedUICommand Export { get; } = new RoutedUICommand(
            "导出ASS",
            "Export",
            typeof(UICommands),
            new InputGestureCollection(new Collection<InputGesture>()
            {
                new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+S")
            }));

        public static RoutedUICommand ExitApp { get; } = new RoutedUICommand(
            "退出(_E)",
            "Exit",
            typeof(UICommands),
            new InputGestureCollection(new List<InputGesture>()
            {
                new KeyGesture(Key.F4, ModifierKeys.Alt, "Alt+F4")
            }));

    }
}
