using System;
using System.Windows;
using System.Windows.Controls;

namespace PermDnamics_Toshmatov.Pages
{

    public partial class Main : Page
    {
        public MainWindow mainWindow;

        public Main()
        {
            InitializeComponent();
        }

        private void OpenPageChart(object sender, RoutedEventArgs e)
        {
            float value = Convert.ToInt32(tb_value.Text);
            mainWindow.pointsInfo.Add(new Classes.PointInfo(value));
            mainWindow.OpenPages(MainWindow.Pages.chart);
        }
    }
}
