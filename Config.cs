using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ruminoid.Common.Helpers;

namespace Ruminoid.Trimmer
{
    [RuminoidProduct("Trimmer")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Config : INotifyPropertyChanged
    {
        #region MainWindow

        [JsonProperty]
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
