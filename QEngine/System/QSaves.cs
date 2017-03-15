using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace QEngine.System
{
	/// <summary>
	/// Preferences class that can store data to local game location for non-important game data
	/// etc..Player location or music volume
	/// </summary>
	public static class QSaves
	{
		static Dictionary<string, int> PreferencesI { get; set; } = new Dictionary<string, int>();

		static Dictionary<string, bool> PreferencesB { get; set; } = new Dictionary<string, bool>();

		static Dictionary<string, float> PreferencesF { get; set; } = new Dictionary<string, float>();

		static Dictionary<string, string> PreferencesS { get; set; } = new Dictionary<string, string>();

		static string SaveLocationI = "Saves/PrefsI.bin";

		static string SaveLocationB = "Saves/PrefsB.bin";

		static string SaveLocationF = "Saves/PrefsF.bin";

		static string SaveLocationS = "Saves/PrefsS.bin";

		internal static async Task Save()
		{
			if(!Directory.Exists("Saves"))
			{
				Directory.CreateDirectory("Saves");
			}
			await Task.Factory.StartNew(() =>
			{
				using(FileStream fs = new FileStream(SaveLocationI, FileMode.OpenOrCreate))
				using(BinaryWriter bw = new BinaryWriter(fs))
				{
					int count = PreferencesI.Count;

					bw.Write(count);

					foreach(var i in PreferencesI)
					{
						bw.Write(i.Key);
						bw.Write(i.Value);
					}
				}
				using(FileStream fs = new FileStream(SaveLocationB, FileMode.OpenOrCreate))
				using(BinaryWriter bw = new BinaryWriter(fs))
				{
					int count = PreferencesB.Count;

					bw.Write(count);

					foreach(var i in PreferencesB)
					{
						bw.Write(i.Key);
						bw.Write(i.Value);
					}
				}
				using(FileStream fs = new FileStream(SaveLocationF, FileMode.OpenOrCreate))
				using(BinaryWriter bw = new BinaryWriter(fs))
				{
					int count = PreferencesF.Count;

					bw.Write(count);

					foreach(var i in PreferencesF)
					{
						bw.Write(i.Key);
						bw.Write(i.Value);
					}
				}
				using(FileStream fs = new FileStream(SaveLocationS, FileMode.OpenOrCreate))
				using(BinaryWriter bw = new BinaryWriter(fs))
				{
					int count = PreferencesS.Count;

					bw.Write(count);

					foreach(var i in PreferencesS)
					{
						bw.Write(i.Key);
						bw.Write(i.Value);
					}
				}
			});
		}

		public static void ClearAll()
		{
			File.Delete(SaveLocationB);
			File.Delete(SaveLocationF);
			File.Delete(SaveLocationI);
			File.Delete(SaveLocationS);
		}

		internal static async Task Load()
		{
			if(!Directory.Exists("Saves"))
			{
				//Directory.CreateDirectory("Saves");
				await Save();
			}
			await Task.Factory.StartNew(() =>
			{
				using(FileStream fs = new FileStream(SaveLocationI, FileMode.OpenOrCreate))
				using(BinaryReader br = new BinaryReader(fs))
				{
					int count = br.Read();

					for(int i = 0; i < count; i++)
					{
						string key = br.ReadString();
						int value = br.Read();

						PreferencesI[key] = value;
					}
				}
				using(FileStream fs = new FileStream(SaveLocationB, FileMode.OpenOrCreate))
				using(BinaryReader br = new BinaryReader(fs))
				{
					int count = br.Read();

					for(int i = 0; i < count; i++)
					{
						string key = br.ReadString();
						bool value = br.ReadBoolean();

						PreferencesB[key] = value;
					}
				}
				using(FileStream fs = new FileStream(SaveLocationF, FileMode.OpenOrCreate))
				using(BinaryReader br = new BinaryReader(fs))
				{
					int count = br.ReadInt32();

					for(int i = 0; i < count; i++)
					{
						string key = br.ReadString();
						float value = br.ReadSingle();

						PreferencesF[key] = value;
					}
				}
				using(FileStream fs = new FileStream(SaveLocationS, FileMode.OpenOrCreate))
				using(BinaryReader br = new BinaryReader(fs))
				{
					int count = br.Read();

					for(int i = 0; i < count; i++)
					{
						string key = br.ReadString();
						string value = br.ReadString();

						PreferencesS[key] = value;
					}
				}
			});
		}

		public static void AddInt(string name, int value)
		{
			PreferencesI[name] = value;
		}

		public static void AddFloat(string name, float value)
		{
			PreferencesF[name] = value;
		}

		public static void AddString(string name, string value)
		{
			PreferencesS[name] = value;
		}

		public static void AddBool(string name, bool value)
		{
			PreferencesB[name] = value;
		}

		public static int GetInt(string name)
		{
			if(PreferencesI.TryGetValue(name, out int v))
			{
				return v;
			}
			throw new FileNotFoundException();
		}

		public static float GetFloat(string name)
		{
			if(PreferencesF.TryGetValue(name, out float v))
			{
				return v;
			}
			throw new FileNotFoundException();
		}

		public static bool GetBool(string name)
		{
			if(PreferencesB.TryGetValue(name, out bool v))
			{
				return v;
			}
			throw new FileNotFoundException();
		}

		public static string GetString(string name)
		{
			if(PreferencesS.TryGetValue(name, out string v))
			{
				return v;
			}
			throw new FileNotFoundException();
		}
	}
}