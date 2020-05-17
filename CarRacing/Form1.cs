using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRacing
{
    public partial class Form1 : Form
    {
        PictureBox[] roadlines;
        PictureBox[] enemys;
        PictureBox[] coins;
        Random random;
        int collectedCoins;
        public Form1()
        {
            InitializeComponent();
            init_game();
        }

        void init_game()
        {
            new Settings();
            roadlines = new PictureBox[] { pbRoadLine1, pbRoadLine2, pbRoadLine3, pbRoadLine4 };
            enemys = new PictureBox[] { Enemy1, Enemy2, Enemy3 };
            coins = new PictureBox[] { Coin1, Coin2, Coin3, Coin4};
            random = new Random();
            collectedCoins = 0;
            gen_start_pos();
            lblGameOver.Visible = false;

            gameTimer.Interval = 1000 / Settings.TimerSpeed;
            gameTimer.Start();
        }

        void gen_start_pos()
        {
            int startY = -Enemy1.Height;
            int x;
            for (int i = 0; i < enemys.Length; i++)
            {
                x = random.Next(pbBoundLeft.Right + enemys[i].Width, pbBoundRight.Left - enemys[i].Width);
                enemys[i].Location = new Point(x, startY);
                startY -= this.Height / enemys.Length;
            }
        }

        void gameover_actions()
        {
            gameTimer.Stop();
            lblGameOver.Visible = true;
            Car.Image = Properties.Resources.explosion;
            DialogResult dialogResult = MessageBox.Show("Your score is " + collectedCoins.ToString() + "\nTry again?",
                "Gameover! :(", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(dialogResult == DialogResult.Yes)
            {
                Car.Image = Properties.Resources.car;
                init_game();
            }
            else
            {
                this.Close();
            }
        }

        void is_gameover() 
        { 
            for(int i = 0;i<enemys.Length; i++)
            {
                if (Car.Bounds.IntersectsWith(enemys[i].Bounds))
                {
                    Settings.IsGameOver = true;
                }
            }
        }

        void is_get_coins()
        {
            int x;
            for(int i = 0; i < coins.Length; i++)
            {
                if (Car.Bounds.IntersectsWith(coins[i].Bounds))
                {
                    collectedCoins++;
                    x = random.Next(pbBoundLeft.Right, pbBoundRight.Left);
                    coins[i].Location = new Point(x, -coins[i].Height);

                    Settings.GameSpeed += 0.5f;
                    Settings.MinSpeed += 0.5f;
                    Settings.MaxSpeed += 0.5f;
                }
            }
            lblCoins.Text = "Coins = " + collectedCoins.ToString();
        }

        void move_enemys()
        {
            int x;
            for (int i = 0; i < enemys.Length; i++)
            {
                if (enemys[i].Top >= this.Height)
                {
                    x = random.Next(pbBoundLeft.Right + enemys[i].Width, pbBoundRight.Left - enemys[i].Width);
                    enemys[i].Location = new Point(x, -enemys[i].Height);
                }
                else
                {
                    enemys[i].Top += (int)Settings.GameSpeed;
                }
            }
        }
        void move_coins()
        {
            int x;
            for (int i = 0; i < coins.Length; i++)
            {
                if (coins[i].Top >= this.Height)
                {
                    x = random.Next(pbBoundLeft.Right, pbBoundRight.Left);
                    coins[i].Location = new Point(x, -coins[i].Height);
                }
                else
                {
                    coins[i].Top += (int)Settings.GameSpeed;
                }
            }
        }

        void move_lines() 
        {
            for(int i = 0; i < roadlines.Length; i++)
            {
                if (roadlines[i].Top >= this.Height) 
                {
                    roadlines[i].Top = -roadlines[i].Height;    
                }
                else
                {
                    roadlines[i].Top += (int)Settings.GameSpeed;
                }
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            move_lines();
            move_coins();
            is_get_coins();
            move_enemys();
            is_gameover();
            if (Settings.IsGameOver)
            {
                gameover_actions();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Settings.IsGameOver)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        if (Car.Left > pbBoundLeft.Right)
                        {
                            Car.Left -= Settings.CarTurnSpeed;
                        }
                        break;
                    case Keys.Right:
                        if (Car.Right < pbBoundRight.Left)
                        {
                            Car.Left += Settings.CarTurnSpeed;
                        }
                        break;
                    case Keys.Up:
                        if (Settings.GameSpeed < Settings.MaxSpeed)
                        {
                            Settings.GameSpeed += 0.5f;
                        }
                        break;
                    case Keys.Down:
                        if (Settings.GameSpeed > Settings.MinSpeed)
                        {
                            Settings.GameSpeed -= 0.5f;
                        }
                        break;
                }
            }
        }
    }
}
