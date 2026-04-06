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
        public List<Classes.PointInfo> pointInfo = new List<Classes.PointInfo>();

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
