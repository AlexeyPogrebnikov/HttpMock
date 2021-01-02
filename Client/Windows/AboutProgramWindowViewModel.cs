using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HttpMock.Client.Windows
{
    public class AboutProgramWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //TODO takes version from assembly
        public string Version => "0.1";

        public string Author => "Alexey Pogrebnikov";
    }
}