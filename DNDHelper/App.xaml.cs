namespace DNDHelper
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var windows = base.CreateWindow(activationState);

            windows.Height = 750;
            windows.Width = 1000;

            return windows;
        }
    }
}