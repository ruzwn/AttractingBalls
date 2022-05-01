using System.Drawing;
using System.Threading;

namespace AttractingBalls
{
	public class Producer
	{
		private readonly CommonData _data;
		private readonly ColorType _colorType;
		private readonly Size _containerSize;
		private Thread _thread;

		public Producer(CommonData data, ColorType colorType, Size containerSize)
		{
			_data = data;
			_colorType = colorType;
			_containerSize = containerSize;
		}

		public void Start()
		{
			if (_thread?.IsAlive ?? false)
			{
				return;
			}

			_thread = new Thread(() =>
			{
				var ball = new Ball(_containerSize, _colorType);
				_data.PutData(ball);
			});
			_thread.Start();
		}
	}
}