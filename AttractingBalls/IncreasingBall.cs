using System.Drawing;
using System.Threading;

namespace AttractingBalls
{
	public class IncreasingBall
	{
		private Point _destinitionPoint;
		private int _dx;
		public Thread Thread;

		public IncreasingBall(Point destinitionPoint, int dx)
		{
			_destinitionPoint = destinitionPoint;
			_dx = dx;
		}

		public void Start()
		{
			if (!(Thread?.IsAlive ?? true))
			{
				return;
			}
			
			Thread = new Thread(() =>
			{
				do
				{
					Thread.Sleep(30);
				} while (Move());
			});
			Thread.Start();
		}

		public void Paint(Graphics gr)
		{
			gr.FillEllipse(Brushes.Coral, _destinitionPoint.X, _destinitionPoint.Y, _dx, _dx);
		}

		private bool Move()
		{
			if (_dx > 2000)
			{
				return false;
			}
			
			_destinitionPoint.X -= 10;
			_destinitionPoint.Y -= 10;
			_dx += 25;
			return true;
		}
	}
}