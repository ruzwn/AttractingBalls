using System;
using System.Windows.Forms;

namespace AttractingBalls
{
	public partial class Form1 : Form
	{
		private readonly CommonData _data;

		public Form1()
		{
			InitializeComponent();
			_data = new CommonData();
			var animator = new Animator(panel1.Size, panel1.CreateGraphics());
			animator.Start();
			var consumer = new Consumer(_data, animator);
			consumer.Start();
		}

		private void panel1_Click(object sender, EventArgs e)
		{
			var cSize = panel1.Size;
			var pRed = new Producer(_data, ColorType.Red, cSize);
			var pBlue = new Producer(_data, ColorType.Blue, cSize);
			var pGreen = new Producer(_data, ColorType.Green, cSize);

			pRed.Start();
			pBlue.Start();
			pGreen.Start();
		}
	}
}