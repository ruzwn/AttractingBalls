using System;
using System.Drawing;
using System.Threading;

namespace AttractingBalls
{
	public class Ball
	{
		private readonly Size _cSize;
		private Point _locationPoint;
		private readonly int _diameter;
		private (double X, double Y) _speed;
		private Color _color;
		private readonly ColorType _colorType;
		private Thread _thread;
		private static readonly Random RandomGen = new Random();

		public bool IsThreadAlive => _thread?.IsAlive == true;

		public bool IsAlive { get; private set; }

		public ColorType ColorType => _colorType;

		public Point DestinitionPoint { get; set; }

		public Color Color => _color;

		public Ball(Size containerSize, ColorType colorType)
		{
			_cSize = containerSize;
			_diameter = Animator.BallDiameter;
			_locationPoint = new Point(
				RandomGen.Next(_cSize.Width - Animator.BallDiameter),
				RandomGen.Next(_cSize.Height - Animator.BallDiameter)
			);
			_colorType = colorType;
			SetColor();
			IsAlive = true;
		}

		public void Paint(Graphics graphics)
		{
			graphics.FillEllipse(new SolidBrush(_color), _locationPoint.X, _locationPoint.Y, _diameter, _diameter);
		}

		public void Animate()
		{
			if (!(_thread?.IsAlive ?? true))
			{
				return;
			}

			_speed.X = (DestinitionPoint.X - _locationPoint.X) / 50d;
			_speed.Y = (DestinitionPoint.Y - _locationPoint.Y) / 50d;

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
			if (_locationPoint.X + _diameter >= DestinitionPoint.X &&
			    _locationPoint.X - _diameter <= DestinitionPoint.X &&
			    _locationPoint.Y + _diameter >= DestinitionPoint.Y &&
			    _locationPoint.Y - _diameter <= DestinitionPoint.Y)
			{
				Animator.Color = _color;
				return false;
			}

			_locationPoint.X += (int) _speed.X;
			_locationPoint.Y += (int) _speed.Y;
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
