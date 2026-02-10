using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Race
{
    public partial class RaceGame : Form
    {
        public Random rnd = new Random();

        private Label[] LanesOne = new Label[5];
        private Label[] LanesTwo = new Label[5];
        private Label[] LanesMenuOne = new Label[5];
        private Label[] LanesMenuTwo = new Label[5];

        private int score = 0;
        private int coins = 0;
        private int carSpeed = 1;

        public RaceGame()
        {
            InitializeComponent();
        }

        private void RaceGame_Load(object sender, EventArgs e)
        {
            LanesOne[0] = LaneOne1;
            LanesOne[1] = LaneOne2;
            LanesOne[2] = LaneOne3;
            LanesOne[3] = LaneOne4;
            LanesOne[4] = LaneOne5;

            LanesTwo[0] = LaneTwo1;
            LanesTwo[1] = LaneTwo2;
            LanesTwo[2] = LaneTwo3;
            LanesTwo[3] = LaneTwo4;
            LanesTwo[4] = LaneTwo5;

            LanesMenuOne[0] = MenuOneLane1;
            LanesMenuOne[1] = MenuOneLane2;
            LanesMenuOne[2] = MenuOneLane3;
            LanesMenuOne[3] = MenuOneLane4;
            LanesMenuOne[4] = MenuOneLane5;

            LanesMenuTwo[0] = MenuTwoLane1;
            LanesMenuTwo[1] = MenuTwoLane2;
            LanesMenuTwo[2] = MenuTwoLane3;
            LanesMenuTwo[3] = MenuTwoLane4;
            LanesMenuTwo[4] = MenuTwoLane5;

            panelMenu.Show();
        }

        private void timerRoad_Tick(object sender, EventArgs e)
        {
            labelScore.Text = "Score: " + score / 10;
            score++;

            RoadMove(LanesOne);
            RoadMove(LanesTwo);

            CoinMove(Coin1);
            CoinMove(Coin2);
            CoinMove(Coin3);

            coinsCollect();
        }

        private void CoinMove(PictureBox coin)
        {
            coin.Top += carSpeed;

            if (IsObjectOutsideWindow(coin)) SetObjectToPosition(coin);
        }

        private void RoadMove(Label[] road)
        {
            for (int i = 0; i < road.Length; ++i)
            {
                road[i].Top += carSpeed;

                if (road[i].Top >= Height) road[i].Top = -road[i].Height;
            }
        }

        private void coinsCollect()
        {
            if (IsCarInsideObject(Coin1)) AddCoin(Coin1, 1);

            if (IsCarInsideObject(Coin2)) AddCoin(Coin2, 2);

            if (IsCarInsideObject(Coin3)) AddCoin(Coin3, 3);
        }

        private bool IsCarInsideObject(PictureBox obj)
        {
            return mainCar.Bounds.IntersectsWith(obj.Bounds);
        }

        private void AddCoin(PictureBox coin, int numOfCoin)
        {
            coins++;
            labelCoins.Text = "Coins: " + coins;

            coin.Top = -coin.Height;

            switch (numOfCoin)
            {
                case 1: coin.Left = rnd.Next(0, 120); break;
                case 2: coin.Left = rnd.Next(120, 240); break;
                case 3: coin.Left = rnd.Next(240, 300); break;
            }
        }

        private void RaceGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (carSpeed != 0)
            {
                if (e.KeyCode == Keys.Right)
                {
                    if (mainCar.Left < ClientSize.Width - mainCar.Width / 2) mainCar.Left += 9;
                    else mainCar.Left = 0 - mainCar.Width / 2;
                }

                if (e.KeyCode == Keys.Left)
                {
                    if (mainCar.Left > 0 - mainCar.Width / 2) mainCar.Left -= 9;
                    else mainCar.Left = ClientSize.Width - mainCar.Width / 2;
                }
            }

            if (e.KeyCode == Keys.Up)
            {
                if (carSpeed < 10) carSpeed++;
            }

            if (e.KeyCode == Keys.Down)
            {
                if (carSpeed > 0) carSpeed--;
            }

            if (e.KeyCode == Keys.Escape)
            {
                timerRoad.Enabled = false;
                timerTowardCars.Enabled = false;
                panelPause.Show();
            }
        }

        private void timerTowardCars_Tick(object sender, EventArgs e)
        {
            towardCarMove(towardCar1, 1);
            towardCarMove(towardCar2, 2);
            towardCarMove(towardCar3, 3);

            if (IsCarInsideObject(towardCar1)) GameOver();
            if (IsCarInsideObject(towardCar2)) GameOver();
            if (IsCarInsideObject(towardCar3)) GameOver();
        }

        private void towardCarMove(PictureBox car, int towardCarSpeed)
        {
            car.Top += carSpeed + towardCarSpeed;
            if (IsObjectOutsideWindow(car)) SetObjectToPosition(car);
        }

        private bool IsObjectOutsideWindow(PictureBox obj)
        {
            return obj.Top > Height;
        }
        private void GameOver()
        {
            timerRoad.Stop();
            timerTowardCars.Stop();
            if (coins < 15)
            {
                DialogResult dd = MessageBox.Show("Game Over!", "Приехали!");
                panelPause.Show();
                panelMenu.Show();
            }
            else
            {
                DialogResult dr = MessageBox.Show("Продолжить? (-15 coins)", "Приехали!",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes) Restart();
                else
                {
                    panelPause.Show();
                    panelMenu.Show();
                }
            }
        }

        private void Restart()
        {
            coins -= 15;

            PrepareGame();
        }
        private void StartGame()
        {
            score = 0;

            PrepareGame();

            panelPause.Hide();
            panelGame.Show();
            panelMenu.Hide();
        }

        private void PrepareGame()
        {
            labelCoins.Text = "Coins: " + coins;
            carSpeed = 1;

            timerRoad.Start();
            timerTowardCars.Start();

            SetObjectToPosition(towardCar1);
            SetObjectToPosition(towardCar2);
            SetObjectToPosition(towardCar3);
        }

        private void timerMenu_Tick(object sender, EventArgs e)
        {
            RoadMove(LanesMenuOne);
            RoadMove(LanesMenuTwo);

            towardCarMove(CarMenu1, 5);
            towardCarMove(CarMenu2, 3);
            towardCarMove(CarMenu3, 4);
        }

        private void SetObjectToPosition(PictureBox obj)
        {
            obj.Top = -obj.Height;
            obj.Left = rnd.Next(0, Width - obj.Width);
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            timerRoad.Enabled = false;
            timerTowardCars.Enabled = false;
            panelPause.Show();
        }

        private void buttonResume_Click(object sender, EventArgs e)
        {
            timerRoad.Enabled = true;
            timerTowardCars.Enabled = true;
            panelPause.Hide();
        }

        private void buttonExit_Click(object sender, EventArgs e) => panelMenu.Show();

        private void buttonHelp_Click(object sender, EventArgs e) => Help.ShowHelp(this, Path.Combine(Application.StartupPath, "help.chm"), HelpNavigator.TableOfContents);

        private void buttonStart_Click(object sender, EventArgs e) => StartGame();

        private void buttonMenuExit_Click(object sender, EventArgs e) => Close();
    }
}