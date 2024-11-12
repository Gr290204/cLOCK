using System.Timers;


namespace MauiApp3
{
    public partial class MainPage : ContentPage
    {
        private Grid secGrid;
        private Grid minGrid;
        private Grid hGrid;

        public MainPage()
        {
            InitializeComponent();
            secGrid = CreateSquareGrid(15, 16);
            minGrid = CreateSquareGrid(15, 14);
            hGrid = CreateSquareGrid(3, 49);
            minGrid.AddWithSpan(hGrid, 1, 1, 14, 14);
            secGrid.AddWithSpan(minGrid, 1, 1, 14, 14);
            Content = secGrid;
           
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += ReDraw;
            timer.Start();

        }

        private void ReDraw(object? sender, ElapsedEventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            var coordsec = NumberToCoordinates(currentTime.Second, 60);
            var coordmin = NumberToCoordinates(currentTime.Minute, 60);
            var coordh = NumberToCoordinates(currentTime.Hour % 12, 12);
            var coordseclast = NumberToCoordinates(currentTime.Second-1 == -1 ? 59 : currentTime.Second-1, 60);
            var coordminlast = NumberToCoordinates(currentTime.Minute - 1 == -1 ? 59 : currentTime.Minute - 1, 60);
            var coordhlast = NumberToCoordinates(currentTime.Hour%12 - 1 == -1 ? 11 : currentTime.Hour%12 - 1, 12);
            ChangeColor((int)coordsec.Item1, (int)coordsec.Item2, secGrid, Colors.Red);
            ChangeColor((int)coordmin.Item1, (int)coordmin.Item2, minGrid, Colors.Red);
            ChangeColor((int)coordh.Item1, (int)coordh.Item2, hGrid, Colors.Red);
            ChangeColor((int)coordseclast.Item1, (int)coordseclast.Item2, secGrid, Colors.Black);
            ChangeColor((int)coordminlast.Item1, (int)coordminlast.Item2, minGrid, Colors.Black);
            ChangeColor((int)coordhlast.Item1, (int)coordhlast.Item2, hGrid, Colors.Black);

        }

        static void ChangeColor(int row, int col, Grid grid, Color color)
        {
            BoxView? boxView = grid.Children.FirstOrDefault(c => grid.GetRow(c) == row && grid.GetColumn(c) == col) as BoxView;
            if (boxView != null)
            {
                boxView.Color = color;
            }
        }


        private Grid CreateSquareGrid(int size, int cellSize)
        {
            var grid = new Grid
            {
                RowDefinitions = new RowDefinitionCollection(),
                ColumnDefinitions = new ColumnDefinitionCollection()
            };

            for (int i = 0; i < size + 1; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(cellSize) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(cellSize) });
            }

            for (int i = 0; i < size; i++)
            {
                grid.AddWithSpan(new BoxView { Color = Colors.Black }, i + 1, 0);
                grid.AddWithSpan(new BoxView { Color = Colors.Black }, i, size);
                grid.AddWithSpan(new BoxView { Color = Colors.Black }, 0, i);
                grid.AddWithSpan(new BoxView { Color = Colors.Black }, size, i + 1);
            }

            return grid;
        }
        static (double, double) NumberToCoordinates(int num, int maxNumber)
        {
            if (num < 0 || num > maxNumber)
            {
                throw new ArgumentOutOfRangeException("Число должно быть в диапазоне от 0 до " + maxNumber + ".");
            }

            double maxX = maxNumber / 4.0; 
            double maxY = maxNumber;

            if (num == 0)
            {
                return (0, 0);
            }
            else if (num == maxNumber / 4)
            {
                return (0, maxY / 4);
            }
            else if (num == maxNumber / 2)
            {
                return (maxX, maxY / 4);
            }
            else if (num == (3 * maxNumber) / 4)
            {
                return (maxX, 0);
            }
            else if (num == maxNumber)
            {
                return (0, 0);
            }
            if (num > 0 && num < maxNumber / 4)
            {
                return (0, num);
            }
            else if (num > maxNumber / 4 && num < maxNumber / 2)
            {
                return (0 + (num - maxNumber / 4) * (maxX - 0) / (maxNumber / 2 - maxNumber / 4), maxY / 4);
            }
            else if (num > maxNumber / 2 && num < (3 * maxNumber) / 4)
            {
                return (maxX, maxY / 4 - (num - maxNumber / 2) * (maxY / 4 - 0) / ((3 * maxNumber) / 4 - maxNumber / 2));
            }
            else { 
                return (maxX - (num - (3 * maxNumber) / 4) * (maxX - 0) / (maxNumber - (3 * maxNumber) / 4), 0); 
            }
        }
    }
}