using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private ClientClass _client;
        private int _rectangleX = 50;
        private int _rectangleY = 50;

        public Form1()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.Form1_Load);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _client = new ClientClass("127.0.0.1", 12345);
            _client.PositionUpdated += OnPositionUpdated;
        }

        private void OnPositionUpdated(int x, int y)
        {
            _rectangleX = x;
            _rectangleY = y;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(Brushes.Blue, _rectangleX, _rectangleY, 50, 50);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                    _rectangleY -= 5;
                    break;
                case Keys.Down:
                    _rectangleY += 5;
                    break;
                case Keys.Left:
                    _rectangleX -= 5;
                    break;
                case Keys.Right:
                    _rectangleX += 5;
                    break;
            }

            _client.SendData($"{_rectangleX},{_rectangleY}");
            Invalidate();
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}