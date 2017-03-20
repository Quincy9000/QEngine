using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace QEngine.System
{
	public sealed class QWindow : Game 
	{
		QScene _currentScene;

		internal QRenderer2D _spriteRenderer;

		internal QRenderer3D _modelRenderer;

		public readonly GraphicsDeviceManager DeviceManager;

		public Dictionary<string, QScene> Scenes { get; set; }

		public string RootDirectory { get; }

		float fixedDelta;

		string[] _args;

		/// <summary>
		/// Set the interval that the scene will do fixedUpdate loops at
		/// </summary>
		public float FixedDeltaTime
		{
			get
			{
				return fixedDelta;
			}
			set
			{
				if(value > 0)
					fixedDelta = 1 / value;
			}
		}

		public bool IsVerticalSync
		{
			get
			{
				return DeviceManager.SynchronizeWithVerticalRetrace;
			}
			set
			{
				DeviceManager.SynchronizeWithVerticalRetrace = value;
				DeviceManager.ApplyChanges();
			}
		}

		public bool IsFullScreen
		{
			get
			{
				return DeviceManager.IsFullScreen;
			}
			set
			{
				DeviceManager.IsFullScreen = value;
				DeviceManager.ApplyChanges();
			}
		}

		public int Width
		{
			get
			{
				return DeviceManager.PreferredBackBufferWidth;
			}
			set
			{
				DeviceManager.PreferredBackBufferWidth = value;
				DeviceManager.ApplyChanges();
			}
		}

		public int Height
		{
			get
			{
				return DeviceManager.PreferredBackBufferHeight;
			}
			set
			{
				DeviceManager.PreferredBackBufferHeight = value;
				DeviceManager.ApplyChanges();
			}
		}

		public bool IsHardwareModeSwitch
		{
			get
			{
				return DeviceManager.HardwareModeSwitch;
			}
			set
			{
				DeviceManager.HardwareModeSwitch = value;
				DeviceManager.ApplyChanges();
			}
		}

		public string Title
		{
			get
			{
				return Window.Title;
			}
			set
			{
				Window.Title = value;
			}
		}

		/// <summary>
		/// Switches to the designated scene if it exists
		/// </summary>
		/// <param name="name"></param>
		public void SwitchScene(string name)
		{
			if(Scenes.TryGetValue(name, out QScene scene))
			{
				_currentScene.Unload();
				ClearContent();
				_currentScene = scene;
				_currentScene.Load();
			}
		}

		/// <summary>
		/// Unloads and then reloads the currentScene
		/// </summary>
		public void ResetCurrentScene()
		{
			_currentScene.Unload();
			ClearContent();
			_currentScene.Load();
			QControls.Flush();
		}

		/// <summary>
		/// Clears all the content that was loaded, removes all textures and other content
		/// </summary>
		public void ClearContent()
		{
			Content.Unload();
			Content.Dispose();
			Content = new ContentManager(Services, RootDirectory);
		}

		protected override void LoadContent()
		{
			if(_args != null)
			{
				foreach (string passedInArguments in _args)
				{
					if (passedInArguments.Contains("width:"))
					{
						if (int.TryParse(passedInArguments.Split(':')[1], out int x))
						{
							Width = x;
						}
					}
					else if (passedInArguments.Contains("height:"))
					{
						if (int.TryParse(passedInArguments.Split(':')[1], out int x))
						{
							Height = x;
						}
					}
					else if (passedInArguments.Contains("vsync:"))
					{
						if (bool.TryParse(passedInArguments.Split(':')[1], out bool x))
						{
							IsVerticalSync = x;
						}
					}
				}
			}

			FixedDeltaTime = 120f;
			IsVerticalSync = false;
			IsHardwareModeSwitch = true;
			IsMouseVisible = true;
			IsFixedTimeStep = false;
			_spriteRenderer = new QRenderer2D(GraphicsDevice);
			_modelRenderer = new QRenderer3D(GraphicsDevice);
			Content.RootDirectory = RootDirectory;
			_currentScene = Scenes.First().Value;
			_currentScene.Load();
		}

		protected override void Update(GameTime gameTime)
		{
			_currentScene.Update(new QTime(gameTime, FixedDeltaTime));
		}

		protected override void Draw(GameTime gameTime)
		{
			_currentScene.Draw(_spriteRenderer);
			_currentScene.Gui(_spriteRenderer);
		}

		protected override void UnloadContent()
		{
			_currentScene.Unload();
			base.UnloadContent();
		}

		public void Run(params QScene[] scenes)
		{
			Scenes = new Dictionary<string, QScene>(5);
			if(scenes.Length > 0)
			{
				foreach(var s in scenes)
				{
					s._window = this;
					Scenes.Add(s.Name, s);
				}
			}
			else
			{
				var defaultScene = QScene.DefaultScene;
				defaultScene._window = this;
				Scenes.Add(defaultScene.Name, defaultScene);
			}
			base.Run();
		}

		public QWindow(string title, string rootDirectory, int width, int height, string[] args)
		{
			_args = args;
			DeviceManager = new GraphicsDeviceManager(this);
			Window.Title = title;
			RootDirectory = rootDirectory;
			Width = width;
			Height = height;
		}

		public QWindow(string title, string rootDirectory, int width, int height)
		{
			_args = null;
			DeviceManager = new GraphicsDeviceManager(this);
			Window.Title = title;
			RootDirectory = rootDirectory;
			Width = width;
			Height = height;
		}
	}
}