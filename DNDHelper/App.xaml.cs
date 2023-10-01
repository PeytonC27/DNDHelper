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

            windows.Height = 600;
            windows.Width = 800;

            return windows;
        }
    }
}