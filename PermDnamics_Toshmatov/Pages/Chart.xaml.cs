using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using PermDnamics_Toshmatov.Models;


namespace PermDnamics_Toshmatov.Pages
{

    public partial class Chart : Page
    {
        public Line averageLine;

        public MainWindow mainWindow;

        public double actualHeightCanvas = 0;
        public double maxValue = 0;
        double averageValue = 0;

        public double maxVolatility = 0;
        public double averageVolatility = 0;
        public Line averageLineVolatility;


        public DispatcherTimer dispatcher = new DispatcherTimer();

        //Функция создание графика
        public void CreateChart()
        {
            canvas.Children.Clear();
            averageLine = null;

            for (int i = 0; i < mainWindow.pointsInfo.Count; i++)
            {
                if (mainWindow.pointsInfo[i].value > maxValue)
                    maxValue = mainWindow.pointsInfo[i].value;
            }

            for (int i = 0; i < mainWindow.pointsInfo.Count; i++)
            {
                Line line = new Line();
                line.X1 = i * 20;
                line.X2 = (i + 1) * 20;
                if (i == 0)
                    line.Y1 = actualHeightCanvas;
                else
                    line.Y1 = actualHeightCanvas - ((mainWindow.pointsInfo[(i - 1)].value / maxValue) * actualHeightCanvas);
                line.Y2 = actualHeightCanvas - ((mainWindow.pointsInfo[i].value / maxValue) * actualHeightCanvas);
                line.StrokeThickness = 2;
                mainWindow.pointsInfo[i].line = line;
                canvas.Children.Add(line);
            }

            DrawAverageLine(); 
        }

        //Функция создание отдльной точки
        public void CreatePoint()
        {
            Line line = new Line();
            line.X1 = (mainWindow.pointsInfo.Count - 1) * 20;
            line.X2 = mainWindow.pointsInfo.Count * 20;
            line.Y1 = actualHeightCanvas - ((mainWindow.pointsInfo[(mainWindow.pointsInfo.Count - 2)].value / maxValue) * actualHeightCanvas);
            line.Y2 = actualHeightCanvas - ((mainWindow.pointsInfo[(mainWindow.pointsInfo.Count - 1)].value / maxValue) * actualHeightCanvas);
            line.StrokeThickness = 2;
            mainWindow.pointsInfo[(mainWindow.pointsInfo.Count - 1)].line = line;
            canvas.Children.Add(line);

            DrawAverageLine(); 
        }

        //Функция управление графиком
        public void ControlCreateChart()
        {
            double value = mainWindow.pointsInfo[mainWindow.pointsInfo.Count - 1].value;
            if (value < maxValue)
            {
                CreatePoint();
            }
            else
            {
                CreateChart();
            }

            ColorChart();
            DrawAverageLine();
        }

        //Функция управление цветом графика
        public void ColorChart()
        {
            double value = mainWindow.pointsInfo[mainWindow.pointsInfo.Count - 1].value;
            for (int i = 0; i < mainWindow.pointsInfo.Count; i++)
                averageValue += mainWindow.pointsInfo[i].value;
            averageValue /= mainWindow.pointsInfo.Count;

            for (int i = 0; i < mainWindow.pointsInfo.Count; i++)
            {
                if (value < averageValue)
                    mainWindow.pointsInfo[i].line.Stroke = Brushes.Red;
                else
                    mainWindow.pointsInfo[i].line.Stroke = Brushes.Green;
            }

            //canvas.Width = mainWindow.pointsInfo.Count * 20 + 300;
            scroll.ScrollToHorizontalOffset(canvas.Width);

            current_value.Content = "Тек. знач: " + Math.Round(value, 2);
            average_value.Content = "Сред. знач: " + Math.Round(averageValue, 2);

            DrawAverageLine();
        }

        //Функция пересоздание графика при изменениии размера
        public void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            actualHeightCanvas = mainWindow.Height - 50d;

            CreateChart();
            ColorChart();
            DrawAverageLine();
            DrawAverageLineVolatility();
        }

        //Создание графика генерации новых значений 
        public DispatcherTimer dispatherTimer = new DispatcherTimer();

        public Chart(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;

            if (mainWindow.pointsVolatility.Count == 0)
                mainWindow.pointsVolatility.Add(new Classes.PointInfo(0));

            dispatherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatherTimer.Tick += CreateNewValue;
            dispatherTimer.Start();

            CreateChart();
            ColorChart();
            CreateVolatilityChart();
        }

        private void CreateNewValue(object sender, EventArgs e)
        {
            Random random = new Random();

            double value = mainWindow.pointsInfo[mainWindow.pointsInfo.Count - 1].value;
            double newValue = value * (random.NextDouble() + 0.5d);
            mainWindow.pointsInfo.Add(new Classes.PointInfo(newValue));

            // Волатильность = отклонение от среднего
            double avg = mainWindow.pointsInfo.Average(p => p.value);
            double volatility = Math.Abs(newValue - avg) / avg * 100;
            mainWindow.pointsVolatility.Add(new Classes.PointInfo(volatility));

            ControlCreateChart();
            CreateVolatilityChart();
        }


        // Функция отображения среднего значения на графике
        public void DrawAverageLine()
        {
            if (averageLine != null) canvas.Children.Remove(averageLine);

            // Ширина = видимая область ScrollViewer (или ActualWidth Canvas, если она фиксирована)
            double width = scroll.ViewportWidth;
            if (width <= 0) width = mainWindow.ActualWidth - 50;
            if (maxValue == 0) return;

            averageLine = new Line();
            averageLine.X1 = 0;
            averageLine.X2 = width;

            double height = actualHeightCanvas;
            double averageY = height - ((averageValue / maxValue) * height);
            averageY = Math.Max(0, Math.Min(height, averageY)); // не выходить за пределы

            averageLine.Y1 = averageY;
            averageLine.Y2 = averageY;
            averageLine.Stroke = Brushes.Green;
            averageLine.StrokeThickness = 2;

            canvas.Children.Add(averageLine);
        }

        //Для оценки хорошо
        // Функция сохранения графика в БД
        private void SaveChart_Click(object sender, RoutedEventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Введите название:", "Сохранение", "График_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            if (string.IsNullOrEmpty(name)) return;

            string data = string.Join(",", mainWindow.pointsInfo.Select(p => p.value));

            using (var db = new ChartContext())
            {
                db.Database.EnsureCreated();
                db.ChartSaves.Add(new ChartSave { Name = name, Data = data, SaveDate = DateTime.Now });
                db.SaveChanges();
            }
            MessageBox.Show("Сохранено!");
        }

        // Функция загрузка из БД
        private void LoadChart_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ChartContext())
            {
                var charts = db.ChartSaves.OrderByDescending(c => c.SaveDate).ToList();
                if (charts.Count == 0)
                {
                    MessageBox.Show("Нет графиков");
                    return;
                }

                string names = string.Join("\n", charts.Select((c, i) => $"{i + 1}. {c.Name}"));
                string input = Microsoft.VisualBasic.Interaction.InputBox($"Выберите номер:\n{names}", "Загрузка", "1");

                if (int.TryParse(input, out int index) && index >= 1 && index <= charts.Count)
                {
                    var selected = charts[index - 1];
                    var values = selected.Data.Split(',').Select(double.Parse).ToList();

                    mainWindow.pointsInfo.Clear();
                    foreach (var val in values)
                        mainWindow.pointsInfo.Add(new Classes.PointInfo(val));

                    maxValue = values.Max();
                    CreateChart();
                    ColorChart();
                    DrawAverageLine();
                    MessageBox.Show("Загружено!");
                }
            }
        }
        //Для оценки Отлично
        //Функция создание фторого графика
        public void CreateVolatilityChart()
        {
            canvasVolatility.Children.Clear();
            averageLineVolatility = null;

            if (mainWindow.pointsVolatility.Count == 0) return;

            double max = mainWindow.pointsVolatility.Max(v => v.value);
            if (max > maxVolatility) maxVolatility = max;
            if (maxVolatility == 0) maxVolatility = 1;

            for (int i = 0; i < mainWindow.pointsVolatility.Count; i++)
            {
                Line line = new Line();
                line.X1 = i * 20;
                line.X2 = (i + 1) * 20;

                double height = 200;
                if (i == 0)
                    line.Y1 = height;
                else
                    line.Y1 = height - ((mainWindow.pointsVolatility[i - 1].value / maxVolatility) * height);
                line.Y2 = height - ((mainWindow.pointsVolatility[i].value / maxVolatility) * height);

                line.StrokeThickness = 2;
                line.Stroke = Brushes.Orange;
                mainWindow.pointsVolatility[i].line = line;
                canvasVolatility.Children.Add(line);
            }

            DrawAverageLineVolatility();
        }
        //Функция создние линию среднего для волатильности
        public void DrawAverageLineVolatility()
        {
            if (averageLineVolatility != null) canvasVolatility.Children.Remove(averageLineVolatility);
            if (mainWindow.pointsVolatility.Count == 0) return;

            double width = scroll.ViewportWidth;
            if (width <= 0) width = mainWindow.ActualWidth - 50;
            if (maxVolatility == 0) return;

            averageVolatility = mainWindow.pointsVolatility.Average(v => v.value);
            double avgY = 200 - ((averageVolatility / maxVolatility) * 200);
            avgY = Math.Max(0, Math.Min(200, avgY)); // не выходить за пределы

            averageLineVolatility = new Line();
            averageLineVolatility.X1 = 0;
            averageLineVolatility.X2 = width;
            averageLineVolatility.Y1 = avgY;
            averageLineVolatility.Y2 = avgY;
            averageLineVolatility.Stroke = Brushes.Blue;
            averageLineVolatility.StrokeThickness = 2;

            canvasVolatility.Children.Add(averageLineVolatility);
        }

    }
}
