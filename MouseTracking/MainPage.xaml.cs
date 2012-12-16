namespace MouseTracking
{
    public partial class MainPage
    {
        readonly MouseTracker tracker = new MouseTracker();

        public MainPage()
        {
            InitializeComponent();
            tracker.StartTracking(this);
        }
    }
}
