namespace Skyline.Automation.SatPort.Helpers
{
	using System;
	using System.Runtime.Serialization;

	public class SatPortException : Exception
	{
		public SatPortException()
		{
		}

		public SatPortException(string message) : base(message)
		{
		}

		public SatPortException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected SatPortException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Override ToString() to customize the exception string representation
		public override string ToString()
		{
			// Just return the message, omitting the exception type information
			return Message + (InnerException != null ? " ---> " + InnerException.ToString() : string.Empty);
		}
	}
}