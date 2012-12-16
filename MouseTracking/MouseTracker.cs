using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;

namespace MouseTracking
{
    public class MouseTracker
    {
        public MouseTracker()
        {
            TraceMouse = true;
        }

        public bool TraceMouse { get; set; }

        public void StartTracking(UserControl sender)
        {
            sender.Loaded += sender_Loaded;
            //TODO: Hook into specific FrameworkElements events?  MouseOver, select, click, etc
        }

        void sender_Loaded(object sender, RoutedEventArgs e)
        {
            var page = (UserControl) sender;
            page.MouseLeftButtonDown += page_MouseLeftButtonDown;
            page.SizeChanged += page_SizeChanged;
            page.MouseMove += page_MouseMove;
            page.KeyDown += page_KeyDown;
            page.Unloaded += page_Unloaded;

            var data = string.Format("resolution={0}x{1}", 
                                     HtmlPage.Window.Eval("screen.width"),
                                     HtmlPage.Window.Eval("screen.height"));

            Write(DateTime.Now, page.ToString(), "Loaded", data);
        }

        void page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var data = string.Format("ClickCount={0}", e.ClickCount);

            Write(DateTime.Now, sender.ToString(), "page_MouseLeftButtonDown", data);
        }

        void page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var data = string.Format("screen={0}x{1} page={2}x{3}", 
                                     (Application.Current.RootVisual as FrameworkElement).ActualWidth,
                                     (Application.Current.RootVisual as FrameworkElement).ActualHeight,
                                     (sender as FrameworkElement).ActualWidth,
                                     (sender as FrameworkElement).ActualHeight);

            Write(DateTime.Now, sender.ToString(), "SizeChanged", data);
                                     
        }

        private void page_KeyDown(object sender, KeyEventArgs e)
        {
            Write(DateTime.Now, sender.ToString(), "KeyDown", string.Format("key={0}", e.Key));
        }
        
        void page_Unloaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Removing event handlers");

            var page = (UserControl)sender;
            page.Loaded -= sender_Loaded; 
            page.MouseLeftButtonDown -= page_MouseLeftButtonDown;
            page.SizeChanged -= page_SizeChanged;
            page.MouseMove -= page_MouseMove;
            page.KeyDown -= page_KeyDown;
            page.Unloaded -= page_Unloaded;
        }

        void page_MouseMove(object sender, MouseEventArgs e)
        {
            Write(DateTime.Now, sender.ToString(), "MouseMove", string.Format("coord={0},{1}", e.GetPosition(null).X, e.GetPosition(null).Y));
        }

        private void Write(DateTime time, string page, string eventName, string data)
        {
            string ipAddress = Application.Current.Resources["ipAddress"].ToString();

            // call service here (ipaddress can be retrieved serverside using HttpContext object)
            //  for now we'll just output to debug window

            if (TraceMouse)
                Debug.WriteLine("{0} {1} {2} {3} {4}", ipAddress, time.ToString("MM/dd/yyyy HH:mm:ss.fff"), page, eventName, data);
        }
    }
}