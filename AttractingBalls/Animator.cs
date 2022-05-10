using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace AttractingBalls
{
	public class Animator
	{
		private BufferedGraphics _bufGr;
		private Graphics _gr;
		private Thread _thread;
		private readonly List<Ball> _balls = new List<Ball>();
		private Point _destinitionPoint;
		private readonly Random _rnd = new Random();
		private IncreasingBall _increasingBall;

		private Graphics Gr
		{
			get => _gr;
			set
			{
				_gr = value;
				_bufGr = BufferedGraphicsManager
					.Current
					.Allocate(_gr, Rectangle.Ceiling(_gr.VisibleClipBounds));
				_bufGr.Graphics.Clear(Color.White);
			}
		}

		public const int BallDiameter = 100;

		public bool IsBusy { get; private set; }

		public Size CSize { get; }

		public static Color Color { get; set; } = Color.Black;

		public Animator(Size containerSize, Graphics gr)
		{
			CSize = containerSize;
			Gr = gr;
		}

		public void AddBalls(Ball[] balls, Point destinitionPoint)
		{

			// Когда один шарик достиг цели, берется из очереди шар того же цвета и двигается к своей области
			// и так с каждым шариком (один исчез, другой появился)

			IsBusy = true;
			_destinitionPoint = destinitionPoint;
			var a = _rnd.Next(256);
			var r = (int) balls[(int) ColorType.Red].Color.R;
			var g = (int) balls[(int) ColorType.Green].Color.G;
			var b = (int) balls[(int) ColorType.Blue].Color.B;
			var color = Color.FromArgb(a, r, g, b);
			_increasingBall = new IncreasingBall(_destinitionPoint, BallDiameter, color);
			foreach (var ball in balls)
			{
				ball.DestinitionPoint = _destinitionPoint;
				ball.Animate();
				_balls.Add(ball);
			}
		}

		public void Start()
		{
			if (_thread != null && _thread.IsAlive)
			{
				return;
			}

			_thread = new Thread(() =>
			{
				Graphics tmpGr;
				lock (_bufGr)
				{
					tmpGr = _bufGr.Graphics;
				}

				do
				{
					tmpGr.Clear(Color.White);
					foreach (var ball in _balls.Where(ball => ball.IsAlive))
					{
						var brush = new SolidBrush(Color);
						tmpGr.FillEllipse(brush, _destinitionPoint.X, _destinitionPoint.Y, BallDiameter, BallDiameter);
						ball.Paint(tmpGr);
					}

					if (_balls.Count(ball => ball.IsAlive) == 0 && IsBusy)
					{
						tmpGr.Clear(Color.White);
						_increasingBall.Start();
						_increasingBall.Paint(tmpGr);
						if (!_increasingBall.IsThreadAlive)
						{
							Color = Color.Black;
							_balls.Clear();
							IsBusy = false;
						}
					}

					_bufGr.Render(Gr);
					Thread.Sleep(30);
				} while (true);
			});
			_thread.IsBackground = true;
			_thread.Start();
		}
	}
}
