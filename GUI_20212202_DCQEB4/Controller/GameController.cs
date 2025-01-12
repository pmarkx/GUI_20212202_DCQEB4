﻿using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;
using UI.Renderer;
using UI.VM;
using static Logic.GameLogic;

namespace UI.Controller
{
    internal class GameController
    {
        IGameControl control;
        public event TickHappened GameTickHappened;
        public DispatcherTimer UITimer;
        public GameController(IGameControl control)
        {
            this.control = control;
            control.GameTickInterval = Constants.GameTickInterval;
            control.EnemyMovementInterval = Constants.EnemyMovementInterval;
            control.EnemySpawnInterval = Constants.EnemySpawnInterval;
            control.BulletMoveInterval = Constants.BulletMoveInterval;
            control.ShootingBetweenInterval = Constants.ShootingBetweenInterval;
            control.LifeSpawnInterval = Constants.LifeSpawnInterval;
            control.AmmoSpawnInterval = Constants.AmmoSpawnInterval;
            control.AmmoMoveInterval = Constants.AmmoMoveInterval;
            control.LifeMoveInterval = Constants.LifeMoveInterval;
            control.GameTickHappened += Control_GameTickHappened;
            UITimer = new DispatcherTimer();
            UITimer.Interval = Constants.UIRefreshInterval;
            control.StartGame();
        }
        public void RefreshScoreTable(MainWindowViewModel vm)
        {
            vm.CurrentScore = control.Score;
            ;
            vm.ScoreText = $"Score: {control.Score}";
            vm.LifeText = control.Life==1 ? $"Life: {control.Life}" : $"Lives: { control.Life}";
            vm.AmmoText = $"Ammo: {control.Ammo}";
        }

        private void Control_GameTickHappened()
        {
            this.GameTickHappened?.Invoke();
        }

        public void KeyPressed(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    control.Move(Constants.Directions.up);
                    break;
                case Key.Down:
                    control.Move(Constants.Directions.down);
                    break;
                case Key.Space:
                    control.Shoot();
                    break;
                case Key.F5:
                    control.Save();
                    break;
                case Key.F6:
                    control.Load();
                    break;
                case Key.P:
                    control.Pause();
                    break;
                case Key.Escape:
                    control.GameOver = true;
                    break;
            }
        }
    }
}
