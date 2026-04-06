using System.Collections.Generic;
using System.Windows;
using PermDnamics_Toshmatov.Pages;


namespace PermDnamics_Toshmatov
{

    public partial class MainWindow : Window
    {

        public List<Classes.PointInfo> pointsInfo = new List<Classes.PointInfo>();

        public enum Pages
        {
            main,
            chart
        }

        public MainWindow()
        {
            InitializeComponent();
            OpenPages(Pages.main);
        }

        public void OpenPages(Pages _pages)
        {
            if (_pages == Pages.main)
                frame.Navigate(new Main(this));   
            else if (_pages == Pages.chart)
                frame.Navigate(new Chart(this));  
        }
    }
}
    
    

        
    
