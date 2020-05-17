using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRacing
{
    class Settings
    {
        static public int TimerSpeed;
        static public float GameSpeed;
        static public int CarTurnSpeed;
        static public bool IsGameOver;
        static public float MinSpeed;
        static public float MaxSpeed;
        public Settings()
        {
            TimerSpeed = 100;
            GameSpeed = 1;
            CarTurnSpeed = 15;
            IsGameOver = false;
            MinSpeed = 1;
            MaxSpeed = 21;
        }
    }
}
