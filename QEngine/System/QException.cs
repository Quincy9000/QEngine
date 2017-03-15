using System;
using System.IO;
using QEngine.System;

namespace QEngine.Code
{
	/// <summary>
	/// Exception that will be thrown when something that isn't supposed to be called
	/// </summary>
	public sealed class QException : Exception
	{
		public QException()
		{
		}

		internal QException(QBehavior script, SceneStates sceneState)
		{
			string message = $"The game crashed on script {script}, and during the state {sceneState}";
			File.WriteAllText("log.txt", message);
		}

		public QException(string message) : base(message)
		{
		}

		public QException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}