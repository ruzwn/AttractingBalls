using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace AttractingBalls
{
	public class Animator
	{
		private BufferedGraphics _bGr;
		private Graphics _gr;
		private Thread _thread;
		private readonly List<Ball> _balls = new List<Ball>();
		private Point _destinitionPoint;

		private Graphics Gr
		{
			get => _gr;
			set
			{
				_gr = value;
				_bGr = BufferedGraphicsManager
					.Current
					.Allocate(_gr, Rectangle.Ceiling(_gr.VisibleClipBounds));
				_bGr.Graphics.Clear(Color.White);
			}
		}

		public const int BallRadius = 100;

		public bool IsBusy { get; private set; }

		public Size CSize { get; }

		public Animator(Size containerSize, Graphics gr)
		{
			CSize = containerSize;
			Gr = gr;
		}

		private IncreasingBall increasingBall;
		
		public void AddBalls(Ball[] balls, Point destinitionPoint)
		{
			IsBusy = true;
			_destinitionPoint = destinitionPoint;
			increasingBall = new IncreasingBall(_destinitionPoint, BallRadius);
			foreach (var ball in balls)
			{
				ball.DestinitionPoint = destinitionPoint;
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
				Graphics tGr;
				lock (_bGr)
				{
					tGr = _bGr.Graphics;
				}

				var pen = new Pen(Color.Black);

				do
				{
					tGr.Clear(Color.White);
					tGr.DrawEllipse(pen, _destinitionPoint.X, _destinitionPoint.Y, BallRadius, BallRadius);

					foreach (var ball in _balls.Where(ball => ball.IsAlive))
					{
						ball.Paint(tGr);
					}

					if (_balls.Count(ball => ball.IsAlive) == 0 && IsBusy)
					{
						// в потоке аниматора рисуем!!! в потоке increasingBall только двигаем
						increasingBall.Start();
						increasingBall.Paint(tGr);
						if (!increasingBall.Thread.IsAlive)
						{
							_balls.Clear();
								//tGr.Clear(Color.White);
							IsBusy = false;
						}
					}

					_bGr.Render(Gr);
					Thread.Sleep(30);
				} while (true);
			});
			_thread.IsBackground = true;
			_thread.Start();
		}
	}
}