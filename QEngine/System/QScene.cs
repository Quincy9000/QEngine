using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using QEngine.Interfaces;
using QEngine.Prefabs;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace QEngine.System
{
	public class QScene
	{
		public string Name { get; }
		public QCamera MainCamera { get; private set; }
		public QConsole Console { get; private set; }
		public QDebug Debug { get; private set; }
		public QNetwork Network { get; private set; }
		public QWorld World { get; private set; }

		public QRenderer2D Renderer => _window._spriteRenderer;
		public QWindow Window => _window;
		public void ResetScene() => Window.ResetCurrentScene();
		public void SwitchScene(string name) => Window.SwitchScene(name);
		public void ExitGame() => Window.Exit();

		public Color BackgroundColor
		{
			get
			{
				return Window._spriteRenderer.BackgroundRenderColor;
			}
			set
			{
				Window._spriteRenderer.BackgroundRenderColor = value;
			}
		}

		internal SceneStates SceneState;
		internal QWindow _window;
		internal Dictionary<string, Texture2D> Textures;
		internal Dictionary<string, SpriteFont> Fonts;
		internal Dictionary<string, SoundEffect> SoundEffects;
		internal Dictionary<string, Song> Songs;
		internal List<QNode> Nodes { get; private set; }
		internal Queue<QNode> NodeQueue;
		internal Queue<QNode> DestroyQueue;

		bool IsRunning; //forthreading
		Task PhysicsThread;

		List<IQLoad> Loads;
		List<IQStart> Starts;
		List<IQFixedUpdate> Slows;
		List<IQUpdate> Fasts;
		List<IQLateUpdate> Lates;
		List<IQDraw> Draws;
		List<IQGui> Guis;
		List<IQDestroy> Destroys;

		protected virtual void OnLoad() {}
		protected virtual void OnUnload() {}

		void Add(QNode node)
		{
			//parallel add node and scripts to scene
			Nodes.Add(node);
			var s = node.Script;
			if(s is IQLoad load)
			{
				Loads.Add(load);
			}
			if(s is IQStart start)
			{
				Starts.Add(start);
			}
			if(s is IQFixedUpdate slow)
			{
				Slows.Add(slow);
			}
			if(s is IQUpdate fast)
			{
				Fasts.Add(fast);
			}
			if(s is IQLateUpdate late)
			{
				Lates.Add(late);
			}
			if(s is IQDraw draw)
			{
				Draws.Add(draw);
			}
			if(s is IQGui gui)
			{
				Guis.Add(gui);
			}
			if(s is IQDestroy destroy)
			{
				Destroys.Add(destroy);
			}
		}

		void Remove(QNode node)
		{
			Nodes.Remove(node);
			var s = node.Script;
			if(s is IQLoad load)
			{
				Loads.Remove(load);
			}
			if(s is IQStart start)
			{
				Starts.Remove(start);
			}
			if(s is IQFixedUpdate slow)
			{
				Slows.Remove(slow);
			}
			if(s is IQUpdate fast)
			{
				Fasts.Remove(fast);
			}
			if(s is IQLateUpdate late)
			{
				Lates.Remove(late);
			}
			if(s is IQDraw draw)
			{
				Draws.Remove(draw);
			}
			if(s is IQGui gui)
			{
				Guis.Remove(gui);
			}
			if(s is IQDestroy destroy)
			{
				destroy.Destroy();
				Destroys.Remove(destroy);
			}
			node.ClearComponents();
			QPool.FreeNode(node);
		}

		public void Instantiate(QBehavior script)
		{
			var node = QPool.GetNode();
			node.Scene = this;
			node.Reset();
			script.Parent = node;
			node.Script = script;
			NodeQueue.Enqueue(node);
		}

		public void Instantiate(QBehavior script, Vector2 pos)
		{
			Instantiate(script);
			script.Transform.Position = pos;
		}

		void CreateFromQueue()
		{
			while(NodeQueue.Count > 0)
			{
				var lc = Loads.Count;
				var sc = Starts.Count;

				while(NodeQueue.Count > 0)
					Add(NodeQueue.Dequeue());

				var loadCount = Loads.Count - lc;
				var startCount = Starts.Count - sc;

				var textureCount = Textures.Count;

				SceneState = SceneStates.Load;
				for(var i = Loads.Count - loadCount; i < Loads.Count; i++)
					Loads[i].Load();

				if(textureCount < Textures.Count)
					Window._spriteRenderer.CompileMega(Textures);

				SceneState = SceneStates.Start;
				for(var i = Starts.Count - startCount; i < Starts.Count; ++i)
					Starts[i].Start();
			}
		}

		void DestroyFromQueue()
		{
			SceneState = SceneStates.Unload;
			while(DestroyQueue.Count > 0)
			{
				Remove(DestroyQueue.Dequeue());
			}
		}

		void Destroy(QNode node)
		{
			//adds to queue that will be destroyed on the start of next frame so that you cant destroy so many things at once
			if(!DestroyQueue.Contains(node))
				DestroyQueue.Enqueue(node);
		}

		public void Destroy(QBehavior script)
		{
			Destroy(script.Parent);
		}

		internal void Load()
		{
			SceneState = SceneStates.Load;
			DestroyQueue = new Queue<QNode>();
			NodeQueue = new Queue<QNode>();
			Textures = new Dictionary<string, Texture2D>();
			Fonts = new Dictionary<string, SpriteFont>();
			SoundEffects = new Dictionary<string, SoundEffect>();
			Songs = new Dictionary<string, Song>();
			Nodes = new List<QNode>(100);
			Loads = new List<IQLoad>();
			Starts = new List<IQStart>();
			Slows = new List<IQFixedUpdate>();
			Fasts = new List<IQUpdate>();
			Lates = new List<IQLateUpdate>();
			Draws = new List<IQDraw>();
			Guis = new List<IQGui>();
			Destroys = new List<IQDestroy>();
			World = new QWorld(new Vector2(0, 10f));
			Instantiate(Network = new QNetwork(ConnectionType.Wait));
			Instantiate(MainCamera = new QCamera());
			Instantiate(Debug = new QDebug());
			Instantiate(Console = new QConsole(100, 10));
			OnLoad();
			CreateFromQueue();
		}

		float accumulator;

		internal void Update(QTime time)
		{
			CreateFromQueue();
			DestroyFromQueue();
			accumulator += time.Delta;
			if(accumulator > 0.25f)
				accumulator = 0.25f;
			SceneState = SceneStates.FixedUpdate;
			while(accumulator >= time.FixedDelta)
			{
				//QControls.Update();
				for(int i = 0; i < Slows.Count; i++)
					Slows[i].FixedUpdate(time.FixedDelta);
				World.Step(time.FixedDelta);
				accumulator -= time.FixedDelta;
			}
			//QControls.Update();
			SceneState = SceneStates.Update;
			for(int i = 0; i < Fasts.Count; ++i)
				Fasts[i].Update(time.Delta);
			SceneState = SceneStates.LateUpdate;
			for(int i = 0; i < Lates.Count; i++)
				Lates[i].LateUpdate(time.Delta);
			MainCamera.UpdateMatrix();
		}

		internal void Draw(QRenderer2D render)
		{
			SceneState = SceneStates.Draw;
			render.MatrixMode = MainCamera.TransformMatrix;
			render.Clear();
			render.Begin();
			for(int i = 0; i < Draws.Count; i++)
				Draws[i].Draw(render);
			render.End();
		}

		internal void Gui(QRenderer2D render)
		{
			SceneState = SceneStates.GuiDraw;
			render.MatrixMode = Matrix.Identity;
			render.Begin();
			for(int i = 0; i < Guis.Count; i++)
				Guis[i].Gui(render);
			render.End();
		}

		internal void Unload()
		{
			SceneState = SceneStates.Unload;
			OnUnload();
			for(int i = 0; i < Nodes.Count; i++)
				Destroy(Nodes[i]);
			DestroyFromQueue();
			World.Clear();
		}

		protected QScene(string name)
		{
			Name = name;
		}

		public static QScene DefaultScene => new QScene("DefaultScene");

		/// <summary>
		/// Saves a texture from gameplay to a png in the Application folder
		/// </summary>
		/// <param name="texture"></param>
		public static void SaveTextureAsPng(Texture2D texture)
		{
			Directory.CreateDirectory("Screenshots");

			DateTime time = DateTime.Now;

			using(var stream = File.Create("Screenshots/" + time.ToString("MM-dd-yy H;mm;ss") + ".png"))
			{
				texture.SaveAsPng(stream, texture.Width, texture.Height);
			}
		}

		/// <summary>
		/// Take a screenshot of what is currently in the viewport of the Application
		/// </summary>
		/// <param name="scene"></param>
		public static void TakeScreenShot(QScene scene)
		{
			using(var texture = new RenderTarget2D(scene.Window.GraphicsDevice, scene.Window.Width, scene.Window.Height))
			{
				scene.Window.GraphicsDevice.SetRenderTarget(texture);
				scene.Draw(scene.Window._spriteRenderer);
				scene.Window.GraphicsDevice.SetRenderTarget(null);
				SaveTextureAsPng(texture);
			}
		}
	}

	internal enum SceneStates
	{
		Load,
		Start,
		FixedUpdate,
		Update,
		LateUpdate,
		Draw,
		GuiDraw,
		Unload,
	}
}

//			PhysicsThread = new Task(() =>
//			{
//				Stopwatch totalTime = Stopwatch.StartNew();
//				IsRunning = true;
//				const float time = 1 / 60f;
//				double accum = 0;
//				double currentTime = DateTime.Today.TimeOfDay.TotalSeconds;
//				while(true)
//				{
//					if(!IsRunning)
//						break;
//					double newTime = totalTime.Elapsed.TotalSeconds;
//					double frameTime = newTime - currentTime;
//					if(accum > 0.25f)
//						accum = 0.25f;
//					currentTime = newTime;
//					accum += frameTime;
//					while(accum > time && !QRigidbody.IsAccessingWorld)
//					{
//						if (!IsRunning)
//							break;
//						World?.Step(time);
//						accum -= time;
//					}
//				}
//			});
//			PhysicsThread.Start();