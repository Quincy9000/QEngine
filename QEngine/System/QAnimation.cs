using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace QEngine.System
{
	/// <summary>
	/// Creates an animation loop from rectangles
	/// </summary>
	public sealed class QAnimation
	{
		struct QFrame
		{
			public Rectangle SourceRectangle;
			public float Duration;
		}

		readonly List<QFrame> _frames = new List<QFrame>();

		float _accumulator;

		int _currentFrame;

		/// <summary>
		/// Creates new Animation that starts at frame zero and you need to add all the frames manually
		/// </summary>
		public QAnimation()
		{
			CurrentRectangle = _frames[_currentFrame].SourceRectangle;
		}

		/// <summary>
		/// Creates new animation from a list of frames, can you tweak what frames you want to include
		/// </summary>
		/// <param name="recs"></param>
		/// <param name="time"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public QAnimation(List<Rectangle> recs, float time, int start, int end)
		{
			SubList(recs, time, start, end);
			CurrentRectangle = _frames[_currentFrame].SourceRectangle;
		}

		/// <summary>
		/// Creates new animation from all the frames in a list of frames
		/// </summary>
		/// <param name="recs"></param>
		/// <param name="time"></param>
		public QAnimation(List<Rectangle> recs, float time)
		{
			SubList(recs, time, 0, recs.Count);
			CurrentRectangle = _frames[_currentFrame].SourceRectangle;
		}

		/// <summary>
		/// Adds a frame to the animation at the end
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="time"></param>
		public void Add(Rectangle rect, float time)
		{
			QFrame f;
			f.SourceRectangle = rect;
			f.Duration = time;
			_frames.Add(f);
		}

		/// <summary>
		/// Adds frames from list, kind of like substring
		/// </summary>
		/// <param name="recs"></param>
		/// <param name="time"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public void SubList(List<Rectangle> recs, float time, int start, int end)
		{
			for(int i = start; i < end; i++)
			{
				Add(recs[i], time);
			}
		}

		/// <summary>
		/// Sets the animation to the first frame
		/// </summary>
		public void Reset()
		{
			_accumulator = 0;
			_currentFrame = 0;
			CurrentRectangle = _frames[_currentFrame].SourceRectangle;
			IsDone = false;
		}

		/// <summary>
		/// Check if the animation is in between frames
		/// </summary>
		public bool IsPlaying => _currentFrame != 0;

		public bool IsDone { get; private set; } = false;

		/// <summary>
		/// Updates the animation smoothly no matter what the fps is running at
		/// </summary>
		/// <param name="delta"></param>
		public Rectangle Play(float delta)
		{
			_accumulator += delta;

			if(IsDone)
				return CurrentRectangle;

			//			if(_currentFrame == _frames.Count - 1 && !Loop)
			//			{
			//				IsDone = true;
			//				return;
			//			}

			if(_accumulator > _frames[_currentFrame].Duration / Multiplyer)
			{
				if(_currentFrame == _frames.Count - 1)
				{
					if(!Loop)
					{
						IsDone = true;
						return CurrentRectangle;
					}
					_currentFrame = 0;
				}
				else
				{
					_currentFrame++;
				}
				_accumulator = 0;
			}
			return CurrentRectangle = _frames[_currentFrame].SourceRectangle;
		}

		public bool Loop { get; set; } = true;

		/// <summary>
		/// Gets the current Source rectangle of the animation
		/// </summary>
		public Rectangle CurrentRectangle { get; private set; }

		public int CurrentFrame => _currentFrame;

		/// <summary>
		/// Makes the animation take less time to play, so it goes faster or slower if lower number
		/// Cannot be zero
		/// </summary>
		public float Multiplyer
		{
			get
			{
				return _multiplyer;
			}
			set
			{
				if(value > 0)
					_multiplyer = value;
			}
		}

		float _multiplyer = 1f;
	}
}