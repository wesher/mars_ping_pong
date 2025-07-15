using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace mars_ping_pong
{
    public partial class Form3_bot : Form
    {
        private Timer gameTimer;
        private Rectangle player1, player2, ball;
        private int playerSpeed = 8, botSpeed = 5;
        private int ballSpeedX = 5, ballSpeedY = 5, player1Score = 0, player2Score = 0;
        private Label scoreLabel;

        public Form3_bot()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Aqua;
            this.ClientSize = new Size(800, 400);
            this.Text = "Player vs. CPU";

            player1 = new Rectangle(10, ClientSize.Height / 2 - 30, 10, 60);
            player2 = new Rectangle(ClientSize.Width - 20, ClientSize.Height / 2 - 30, 10, 60);
            ball = new Rectangle(ClientSize.Width / 2 - 10, ClientSize.Height / 2 - 10, 20, 20);

            scoreLabel = new Label();
            scoreLabel.Font = new Font("Times New Roman", 20);
            scoreLabel.Location = new Point(ClientSize.Width / 2 - 50, 10);
            scoreLabel.AutoSize = true;
            scoreLabel.Text = $"{player1Score} - {player2Score}";
            this.Controls.Add(scoreLabel);

            gameTimer = new Timer();
            gameTimer.Interval = GET_BALL_SPEED();
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            this.KeyDown += _KeyDown;
            this.Paint += _Paint;

            this.FormClosed += BACK_TO_MAIN_MENU;
        }

        private void BACK_TO_MAIN_MENU(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.Show();
        }

        private int GET_BALL_SPEED()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Resources\\BALL_SPEED.txt";
            int value = 0;

            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                int.TryParse(fileContent, out value);
            }

            return value;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            ball.X += ballSpeedX;
            ball.Y += ballSpeedY;

            if (ball.Y <= 0 || ball.Y >= ClientSize.Height - ball.Height)
            {
                ballSpeedY = -ballSpeedY;
            }

            if (ball.IntersectsWith(player1) || ball.IntersectsWith(player2))
            {
                ballSpeedX = -ballSpeedX;
            }

            if (ball.X < 0)
            {
                player2Score++;
                ResetBall();
            }
            else if (ball.X > ClientSize.Width)
            {
                player1Score++;
                ResetBall();
            }

            MoveBot();

            scoreLabel.Text = $"{player1Score} : {player2Score}";
            Invalidate();
        }

        private void MoveBot()
        {

            if (ball.Y < player2.Y)
            {
                player2.Y -= botSpeed;
            }
            else if (ball.Y + ball.Height > player2.Y + player2.Height)
            {
                player2.Y += botSpeed;
            }

            player2.Y = Math.Max(0, Math.Min(ClientSize.Height - player2.Height, player2.Y));
        }

        private void ResetBall()
        {
            ball.X = ClientSize.Width / 2 - ball.Width / 2;
            ball.Y = ClientSize.Height / 2 - ball.Height / 2;
        }

        private void _Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Blue, player1);
            e.Graphics.FillRectangle(Brushes.Red, player2);
            e.Graphics.FillEllipse(Brushes.Black, ball);
        }

        private void _KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    player1.Y -= playerSpeed;
                    break;
                case Keys.S:
                case Keys.Down:
                    player1.Y += playerSpeed;
                    break;
            }

            player1.Y = Math.Max(0, Math.Min(ClientSize.Height - player1.Height, player1.Y));
        }
    }
}