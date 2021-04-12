namespace ScotPolWpfApp.ViewModels
{
    public class MainViewModel : Caliburn.Micro.PropertyChangedBase
    {
        private const string WindowTitleDefault = "The non-web app pol  !";

        private string _windowTitle = WindowTitleDefault;

        public string WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                NotifyOfPropertyChange(() => WindowTitle);
            }
        }

    }
}
