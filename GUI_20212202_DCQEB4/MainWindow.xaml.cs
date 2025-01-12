﻿using Logic;
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
using UI;
using UI.Controller;
using UI.VM;

namespace GUI_20212202_DCQEB4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameController controller;
        GameLogic logic;
        public MainWindow()
        {
            InitializeComponent();
            myMediaElement.Play();
            logic = new GameLogic();
            display.SetupModel(logic);
            controller = new GameController(logic);
            controller.UITimer.Tick += UITimer_Tick;
            //controller.GameTickHappened += Controller_GameTickHappened;
            controller.UITimer.Start();
        }

        private void UITimer_Tick(object? sender, EventArgs e)
        {
            controller.RefreshScoreTable(this.DataContext as MainWindowViewModel);
            display.InvalidateVisual();
            if (logic.IsPause)
            {
                myMediaElement.Pause();
                PauseImage.Visibility = Visibility.Visible;
            }
            else
            {
                PauseImage.Visibility = Visibility.Hidden;
                myMediaElement.Play();
            }
            if (logic.GameOver)
            {
                controller.UITimer.Stop();
                myMediaElement.Stop();
                new WindowScore(this.DataContext as MainWindowViewModel).ShowDialog();
                MessageBoxResult result = MessageBox.Show("GAME OVER!\nRetry?", "Game Over", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        new MainWindow().ShowDialog();
                        this.Close();
                        break;
                    case MessageBoxResult.No:
                        new MenuWindow().ShowDialog();
                        this.Close();
                        break;
                }
            }

        }
        //private void Controller_GameTickHappened()
        //{

        //    display.InvalidateVisual();

        //}

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
            //display.InvalidateVisual();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
            e.Handled = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            controller.KeyPressed(e.Key);
            e.Handled = true;
            // display.InvalidateVisual();
        }
    }
}
