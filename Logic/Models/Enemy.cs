﻿namespace Logic.Models
{
    public class Enemy : DynamicObject
    {
        public Enemy(int xPosition, int yPosition) : base(xPosition, yPosition, "E", 3, false, 0)
        {
        }

        public override int Priority => 1;

        public override void Tick()
        {
            base.Tick();
            YPosition--;
            if (Life<=0)
            {
                IsLive = false;
            }
        }
    }
}
