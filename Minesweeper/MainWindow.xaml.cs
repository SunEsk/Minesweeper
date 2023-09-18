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
using System.Windows.Threading;

namespace Minesweeper
{
    public partial class MainWindow : Window
    {
        
        private int rows = 8;
        private int columns = 8; 
        private bool[,] minefield;
        private int hiddenCellCount;
        private int mines = 5;
        

        private DispatcherTimer gameTime;
        private TimeSpan gameDuration;

        public MainWindow()
        {

           
           
            InitializeComponent();
            CreateGrid();
            CreateButtonsInGrid();
            CreateMines();
            Timer();
            gameTime.Start();
            hiddenCellCount = rows * columns;
        }
        private void CreateMines()
        {
           
            minefield = new bool[rows, columns];
            
            Random random = new Random();
            for (int i = 0; i < mines; i++)
            {
                int x = random.Next(rows);
                int y = random.Next(columns);
                minefield[x, y] = true; 
            }
        }

        private void CreateGrid() 
        {
            
            for (int row = 0; row < rows; row++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int col = 0; col < columns; col++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

        }

        // Add buttons to the grid.
        private void CreateButtonsInGrid()
        {
           

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Button button = new Button
                    {
                        Name = $"Button{row}{col}",
                        Content = "", 
                        Width = 40,   
                        Height = 40, 
                        Tag = Tuple.Create(row, col)
                    };

                    button.Click += Button_Click;
                    GameGrid.Children.Add(button);
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                }
            }
        }

        // Eventhandler for game logic
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            Tuple<int, int> position = (Tuple<int, int>)clickedButton.Tag;
            int row = position.Item1;
            int col = position.Item2;

            bool isMine = minefield[row, col];

            if (isMine)
            {
                
                MessageBox.Show("mine was clicked! game over." + gameDuration);
                ResetGame();
               
            }
            else
            {
                int nearbyMines = CountNearbyMines(row, col);
                clickedButton.Content = nearbyMines;
                
                // Hint from teacher Use recursion to reveal if minecount = 0
            }
            // Win condition
            if (hiddenCellCount == mines) 
            {
                gameTime.Stop();
                MessageBox.Show("Wauw very cool minesweeper gamer won");
                ResetGame();

            }

        }

        private int CountNearbyMines(int row, int col)
        {
            int count = 0;

            // Check nearby cells for mines
            for (int r = row - 1; r <= row + 1; r++)
            {
                for (int c = col - 1; c <= col + 1; c++)
                {
                    if (r >= 0 && r < rows && c >= 0 && c < columns && minefield[r, c])
                    {
                        // Found a nearby mine.
                        count++;
                    }
                }
            }

            return count;
        }

        private void ResetGame()
        {
            GameGrid.Children.Clear();
            CreateButtonsInGrid();
            CreateMines();
            gameDuration = TimeSpan.Zero;
            gameTime.Start();
        }

        private void Timer()
        {
            gameTime = new DispatcherTimer();
            gameTime.Interval = TimeSpan.FromSeconds(1);
            gameTime.Tick += GameTime_Tick;



        }

        private void GameTime_Tick(object? sender, EventArgs e)
        {
            gameDuration = gameDuration.Add(TimeSpan.FromSeconds(1));
            TimeLabel.Content = $"Time: {gameDuration.ToString(@"mm\:ss")}"; 
        }


    }
}