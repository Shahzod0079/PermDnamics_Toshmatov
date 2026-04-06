using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        public void OpenPages(Pages _pages)
        {
            switch (page)
            {
                case Pages.chart:
                    frame.Navigate(new Pages.Chart(this));
                    break;
            }
        }
    }
}