using System.Drawing;
using System.Threading;

namespace AttractingBalls
{
	public class IncreasingBall
	{
		public Point DestinitionPoint;
		public int Dx;
		public Thread Thread;

		public IncreasingBall(Point destinitionPoint, int dx = Animator.BallRadius)
		{
			DestinitionPoint = destinitionPoint;
			Dx = dx;
		}

		public void Start()
		{
			if (Thread?.IsAlive ?? false)
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
			gr.FillEllipse(Brushes.Coral, DestinitionPoint.X, DestinitionPoint.Y, Dx, Dx);
		}

		private bool Move()
		{
			if (Dx > 2000)
			{
				return false;
			}
			
			DestinitionPoint.X -= 25;
			DestinitionPoint.Y -= 25;
			Dx += 25;
			return true;
		}
	}
}