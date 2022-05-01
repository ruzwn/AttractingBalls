using System;
using System.Drawing;
using System.Threading;

namespace AttractingBalls
{
	public class Consumer
	{
		private readonly CommonData _data;
		private readonly Animator _animator;
		private Thread _thread;
		private readonly Random _rnd = new Random();

		public Consumer(CommonData data, Animator animator)
		{
			_data = data;
			_animator = animator;
		}

		public void Start()
		{
			if (_thread?.IsAlive ?? false)
			{
				return;
			}

			_thread = new Thread(() =>
			{
				while (true)
				{
					var balls = _data.GetData();
					var destinitionPoint = new Point(_rnd.Next(_animator.CSize.Width), _rnd.Next(_animator.CSize.Height));
					while (_animator.IsBusy)
					{
						Thread.Sleep(30);
					}

					_animator.AddBalls(balls, destinitionPoint);
				}
			});
			_thread.Start();
		}
	}
}