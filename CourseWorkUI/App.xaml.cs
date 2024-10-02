namespace CourseWorkUI
{
    public partial class App : Application
    {
#if WINDOWS
        static public float WindowWidth { get; private set; } = 360; 
        static public float WindowHeight { get; private set; } = 800;
#endif

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

#if WINDOWS
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Width = WindowWidth;
            window.Height = WindowHeight;

            // Peak ingenuity
            window.MinimumWidth = WindowWidth;
            window.MinimumHeight = WindowHeight;
            window.MaximumWidth = WindowWidth;
            window.MaximumHeight = WindowHeight;

            return window;
        }
#endif
    }
}
