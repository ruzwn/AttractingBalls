using System;
using System.Drawing;
using System.Threading;

namespace AttractingBalls
{
	public class Ball
	{
		private readonly Size _cSize;
		private Point _locationPoint;
		private readonly int _radius;
		private (int X, int Y) _speed;
		private Color _color;
		private readonly ColorType _colorType;
		private Thread _thread;
		private static readonly Random RandomGen = new Random();

		public bool IsThreadAlive => _thread?.IsAlive == true;

		public bool IsAlive { get; private set; }

		public ColorType ColorType => _colorType;

		public Point DestinitionPoint { get; set; }

		public Ball(Size containerSize, ColorType colorType)
		{
			_cSize = containerSize;
			_radius = Animator.BallRadius;
			_locationPoint = new Point(RandomGen.Next(_cSize.Width), RandomGen.Next(_cSize.Height));
			_colorType = colorType;
			SetColor();
			IsAlive = true;
		}

		public void Paint(Graphics graphics)
		{
			graphics.FillEllipse(new SolidBrush(_color), _locationPoint.X, _locationPoint.Y, _radius, _radius);
		}

		public void Animate()
		{
			if (!(_thread?.IsAlive ?? true))
			{
				return;
			}

			_speed.X = (DestinitionPoint.X - _locationPoint.X) / 100;
			_speed.Y = (DestinitionPoint.Y - _locationPoint.Y) / 100;
			
			_thread = new Thread(() =>
			{
				do
				{
					Thread.Sleep(30);
				} while (Move());

				IsAlive = false;
			});
			_thread.IsBackground = true;
			_thread.Start();
		}

		private bool Move()
		{
			if (_locationPoint.X + _radius >= DestinitionPoint.X &&
			    _locationPoint.X - _radius <= DestinitionPoint.X &&
			    _locationPoint.Y + _radius >= DestinitionPoint.Y &&
			    _locationPoint.Y - _radius <= DestinitionPoint.Y)
			{
				return false;
			}

			_locationPoint.X += _speed.X;
			_locationPoint.Y += _speed.Y;
			return true;
		}

		private void SetColor()
		{
			var alpha = RandomGen.Next(256);
			switch (_colorType)
			{
				case ColorType.Red:
					_color = Color.FromArgb(alpha, Color.Red);
					break;
				case ColorType.Blue:
					_color = Color.FromArgb(alpha, Color.Blue);
					break;
				case ColorType.Green:
					_color = Color.FromArgb(alpha, Color.Green);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}