using System;
using System.Runtime.Serialization;

namespace HttpMock.Server
{
	[Serializable]
	public class InvalidConsoleArgsException : Exception
	{
		public InvalidConsoleArgsException()
		{
		}

		public InvalidConsoleArgsException(string message) : base(message)
		{
		}

		public InvalidConsoleArgsException(string message, Exception inner) : base(message, inner)
		{
		}

		protected InvalidConsoleArgsException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}