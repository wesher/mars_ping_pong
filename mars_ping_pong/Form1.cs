using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace mars_ping_pong
{
    public partial class Form1 : Form
    {
        private ComboBox cmb = new ComboBox();
        private NumericUpDown numeric = new NumericUpDown();
        private Button btn = new Button();
        private TableLayoutPanel tb_layout = new TableLayoutPanel();
        public Form1()
        {
            InitializeComponent();
            this.Text = "Main Menu";

            cmb.Size = new Size(120, 150);
            this.Controls.Add(cmb);
            cmb.Items.Add("Player vs. Player");
            cmb.Items.Add("Player vs. CPU");
            cmb.SelectedItem = 0;

            Label l1 = new Label();
            l1.AutoSize = true;
            l1.Text = "Ball Speed \n(1 - fast, 100 - slow)";
            l1.Location = new Point(0, 30);
            Controls.Add(l1);

            numeric.Minimum = 1;
            numeric.Maximum = 100;
            numeric.Location = new Point(0, 70);
            this.Controls.Add(numeric);

            decimal numericValue = GET_BALL_SPEED();
            numeric.Value = numericValue;

            btn.Location = new Point(0, 100);
            btn.Size = new Size(120, 20);
            btn.Text = "Accept";
            btn.Click += CHANGE_FORM;
            this.Controls.Add(btn);

            this.AutoSize = true;

            this.FormClosed += EXIT_GAME;
        }
        private void EXIT_GAME(object sender, EventArgs e) => Application.Exit();
        private void CHANGE_FORM(object sender, EventArgs e)
        {
            SAVE_BALL_SPEED(numeric.Value);

            switch (cmb.SelectedIndex)
            {
                case 0:
                    Form2_p2 form2_P2 = new Form2_p2();
                    form2_P2.Show();
                    this.Hide();
                    break;
                case 1:
                    Form3_bot form3_ = new Form3_bot();
                    form3_.Show();
                    this.Hide();
                    break;
            }
        }
        private decimal GET_BALL_SPEED()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Resources\\BALL_SPEED.txt";
            decimal value = 0;

            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                decimal.TryParse(fileContent, out value);
            }

            return value;
        }
        private void SAVE_BALL_SPEED(decimal value)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Resources\\BALL_SPEED.txt";

            if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
            {
                File.WriteAllText(filePath, string.Empty);
            }

            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine(value);
            }
        }
    }
}