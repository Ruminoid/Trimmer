using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ruminoid.Trimmer
{
    public class Config : INotifyPropertyChanged
    {
        #region ExportConfig

        private bool exportAverageWords;

        public bool ExportAverageWords
        {
            get => exportAverageWords;
            set
            {
                exportAverageWords = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region MainWindow

        private bool hideWelcome;

        public bool HideWelcome
        {
            get => hideWelcome;
            set
            {
                hideWelcome = value;
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
