using System;
using System.Windows;
using System.Windows.Controls;

namespace PermDnamics_Toshmatov.Pages
{

    public partial class Main : Page
    {

        public Main()
        {
            InitializeComponent();
        }

        private void OpenPageChart (object sender, RoutedEventArgs e)
        {
            float value = Convert.ToUInt32(tb_value.Text);
            mainWindow.pointsInfo.Add(new Classes.PointInfo(value));
            mainWindow.OpenPages(MainWindow.pages.chart);
        }
    }
}
