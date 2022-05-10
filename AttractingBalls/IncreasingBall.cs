using System.Drawing;
using System.Threading;

namespace AttractingBalls
{
	public class IncreasingBall
	{
		private Point _locationPoint;
		private int _diameter;
		private readonly SolidBrush _brush;
		private Thread _thread;

		public bool IsThreadAlive => _thread?.IsAlive ?? false;

		public IncreasingBall(Point locationPoint, int diameter, Color color)
		{
			_locationPoint = locationPoint;
			_diameter = diameter;
			_brush = new SolidBrush(color);
		}

		public void Start()
		{
			if (!(_thread?.IsAlive ?? true))
			{
				return;
			}

			_thread = new Thread(() =>
			{
				do
				{
					Thread.Sleep(30);
				} while (Move());
			});
			_thread.IsBackground = true;
			_thread.Start();
		}

		public void Paint(Graphics gr)
		{
			gr.FillEllipse(_brush, _locationPoint.X, _locationPoint.Y, _diameter, _diameter);
		}

		private bool Move()
		{
			if (_diameter > 2000)
			{
				return false;
			}

			_locationPoint.X -= 10;
			_locationPoint.Y -= 10;
			_diameter += 25;
			return true;
		}
	}
}
