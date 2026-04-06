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

namespace PermDnamics_Toshmatov.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
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
