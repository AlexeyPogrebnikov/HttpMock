using System;

namespace HttpMock.Server
{
	[Serializable]
	public class InvalidConsoleArgsException : Exception
	{
		public InvalidConsoleArgsException() { }
		public InvalidConsoleArgsException(string message) : base(message) { }
		public InvalidConsoleArgsException(string message, Exception inner) : base(message, inner) { }
		protected InvalidConsoleArgsException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}