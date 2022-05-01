using System.Collections.Generic;
using System.Threading;

namespace AttractingBalls
{
	public class CommonData
	{
		private readonly Queue<Ball>[] _data = {new Queue<Ball>(), new Queue<Ball>(), new Queue<Ball>()};

		private const int MaxQueueLength = 3;

		public void PutData(Ball ball)
		{
			lock (_data)
			{
				var colorType = ball.ColorType;
				while (_data[(int) colorType].Count >= MaxQueueLength - 1)
				{
					Monitor.Wait(_data);
				}

				_data[(int) colorType].Enqueue(ball);
				Monitor.PulseAll(_data);
			}
		}

		public Ball[] GetData()
		{
			var result = new Ball[3];
			lock (_data)
			{
				foreach (var queue in _data)
				{
					while (queue.Count == 0)
					{
						Monitor.Wait(_data);
					}
				}

				for (var i = 0; i < _data.Length; i++)
				{
					result[i] = _data[i].Dequeue();
				}

				Monitor.PulseAll(_data);
			}

			return result;
		}
	}
}