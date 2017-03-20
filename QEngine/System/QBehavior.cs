using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using QEngine.Code;
using QEngine.Interfaces;
using QEngine.Prefabs;

namespace QEngine.System
{
	public abstract class QBehavior
	{
		//internal
		internal QNode Parent { get; set; }
		internal SceneStates State => Scene.SceneState;
		//Parent
		public QWindow Window => Parent.Scene.Window;
		public QScene Scene => Parent.Scene;
		public QTransform2D Transform => Parent.Transform;
		public int Id => Parent.Id;

		public string Tag
		{
			get
			{
				return Parent.Tag;
			}
			set
			{
				Parent.Tag = value;
			}
		}

		public string Name
		{
			get
			{
				if(Parent == null)
					return toName;
				return Parent.Name;
			}
			set
			{
				if(Parent == null)
					toName = value;
				else
					Parent.Name = value;
			}
		}

		//Scene
		public QWorld World => Scene.World;
		public QCamera MainCamera => Scene.MainCamera;
		public QConsole Console => Scene.Console;
		public QDebug DebugSettings => Scene.Debug;
		public void Instantiate(QBehavior script) => Scene.Instantiate(script);
		public void Instantiate(QBehavior script, Vector2 pos) => Scene.Instantiate(script, pos);

		/// <summary>
		/// Gets a component that you stored in the components before the game opened up
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetComponent<T>() where T : QComponent
		{
			if(Parent.Components.TryGetValue(typeof(T), out QComponent c))
			{
				if(c is QRigidbody r)
				{
					Transform.body = r;
				}
				if(c is QSprite s)
				{
					sprite = s;
				}
				return (T)c;
			}
			return default(T);
		}

		public T GetScript<T>() where T : QBehavior
		{
			foreach(var n in Scene.Nodes)
			{
				if(n.Script.GetType() == typeof(T))
				{
					return (T)n.Script;
				}
			}
			return default(T);
		}

		/// <summary>
		/// Searches for the object that has the exact id, ids are unique
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="searchById"></param>
		/// <returns></returns>
		public T GetScript<T>(int searchById) where T : QBehavior
		{
			foreach(var n in Scene.Nodes)
			{
				if(n.Id == searchById)
				{
					return (T)n.Script;
				}
			}
			return default(T);
		}

		/// <summary>
		/// Returns the first object that it finds thats name is of the search type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="searchByName"></param>
		/// <returns></returns>
		public T GetScript<T>(string searchByName) where T : QBehavior
		{
			foreach(var n in Scene.Nodes)
			{
				if(n.Name == searchByName)
				{
					return (T)n.Script;
				}
			}
			return default(T);
		}

		public void Destroy(QBehavior s) => Scene.Destroy(s);

		/// <summary>
		/// Gets the source of the rectangle from the megatexture instead of the texture so that its better performance
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Rectangle GetSource(string name)
		{
			if(Parent.Scene.Window._spriteRenderer.MegaTexture.Rectangles.TryGetValue(name, out Rectangle r))
			{
				return r;
			}
			return Rectangle.Empty;
		}

		#region ContentHelpers

		T Load<T>(string path)
		{
			return Window.Content.Load<T>(path);
		}

		void AddTexture(string name, Texture2D texture)
		{
			Scene.Textures.Add(name, texture);
		}

		void AddSong(string name, Song song)
		{
			Scene.Songs.Add(name, song);
		}

		void AddSoundEffect(string name, SoundEffect soundEffect)
		{
			Scene.SoundEffects.Add(name, soundEffect);
		}

		void AddFont(string name, SpriteFont font)
		{
			Scene.Fonts.Add(name, font);
		}

		public Texture2D GetTexture(string name)
		{
			if(Scene.Textures.TryGetValue(name, out Texture2D t))
			{
				return t;
			}
			return null;
		}

		public SpriteFont GetFont(string name)
		{
			if(Scene.Fonts.TryGetValue(name, out SpriteFont f))
			{
				return f;
			}
			return null;
		}

		public SoundEffect GetSoundEffect(string name)
		{
			if(Scene.SoundEffects.TryGetValue(name, out SoundEffect s))
			{
				return s;
			}
			return null;
		}

		public Song GetSong(string name)
		{
			if(Scene.Songs.TryGetValue(name, out Song s))
			{
				return s;
			}
			return null;
		}

		/// <summary>
		/// Loads a texture from the ContentManager
		/// </summary>
		/// <param name="path"></param>
		public void LoadTexture(string path)
		{
			if(State != SceneStates.Load)
				throw new QException(this, State);
			var s = path.Split('/').Last();
			if(GetTexture(s) == null)
			{
				AddTexture(s, Load<Texture2D>(path));
			}
		}

		/// <summary>
		/// Creates a rectangle texture that you can get by the name you give it!
		/// </summary>
		/// <param name="name"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="color"></param>
		public void LoadTexture(string name, int width, int height, Color color)
		{
			if(State != SceneStates.Load)
				throw new QException(this, State);
			if(GetTexture(name) == null)
			{
				AddTexture(name, Scene.Window._spriteRenderer.CreateRectangleTexture(Window.GraphicsDevice, width, height, color));
			}
		}

		/// <summary>
		/// Creates a circle texture that you can get by the name you give it!
		/// </summary>
		/// <param name="name"></param>
		/// <param name="radius"></param>
		/// <param name="color"></param>
		public void LoadTexture(string name, int radius, Color color)
		{
			if(State != SceneStates.Load)
				throw new QException(this, State);
			if(GetTexture(name) == null)
			{
				AddTexture(name, Scene.Window._spriteRenderer.CreateCircleTexture(Window.GraphicsDevice, radius, color));
			}
		}

		public void LoadFont(string path)
		{
			if(State != SceneStates.Load)
				throw new QException(this, State);
			var s = path.Split('/').Last();
			if(GetFont(s) == null)
			{
				AddFont(s, Load<SpriteFont>(path));
			}
		}

		public void LoadSong(string path)
		{
			if(State != SceneStates.Load)
				throw new QException(this, State);
			var s = path.Split('/').Last();
			if(GetSong(s) == null)
			{
				AddSong(s, Load<Song>(path));
			}
		}

		public void LoadSoundEffect(string path)
		{
			if(State != SceneStates.Load)
				throw new QException(this, State);
			var s = path.Split('/').Last();
			if(GetSoundEffect(s) == null)
			{
				AddSoundEffect(s, Load<SoundEffect>(path));
			}
		}

		/// <summary>
		/// Attaches a sprite to the node and then can draw it on the scene
		/// </summary>
		public void LoadSprite()
		{
			if(State != SceneStates.Load)
				throw new QException(this, State);
			if(!Parent.Components.ContainsKey(typeof(QSprite)))
			{
				sprite = QSprite.Sprite();
				Parent.AddComponent(sprite);
			}
		}

		public void LoadText(string nameOfFont, string text)
		{
			if(State != SceneStates.Load)
				throw new QException(this, State);
			if(!Parent.Components.ContainsKey(typeof(QLabel)))
			{
				var t = new QLabel(GetFont(nameOfFont), this)
				{
					SetText = text,
				};
				Parent.AddComponent(t);
			}
		}

		/// <summary>
		/// Creates a rigibody for you to use in the start function, if you get this component it will be null, but
		/// you have to call a QRigidbody.CreateShape fucntion to make the body not null and then give it parameters
		/// </summary>
		public void LoadRigidbody()
		{
			if(State != SceneStates.Load)
				throw new QException(this, State);
			if(!Parent.Components.ContainsKey(typeof(QRigidbody)))
			{
				var b = new QRigidbody(World);
				Transform.body = b;
				Parent.AddComponent(b);
			}
		}

		#endregion

		public void Draw(QRenderer2D render)
		{
			if(sprite != null)
			{
				render.Draw(sprite, Transform);
			}
		}

		protected QBehavior(string name)
		{
			toName = name;
		}

		//private
		QSprite sprite;
		string toName;
	}
}