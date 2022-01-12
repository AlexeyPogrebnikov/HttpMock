namespace HttpMock.Core
{
	public class Route
	{
		private RouteKey _key;

		public string Method
		{
			get => _key.Method;
			set => _key.Method = value;
		}

		public string Path
		{
			get => _key.Path;
			set => _key.Path = value;
		}

		public Response Response { get; set; }

		public Route Clone()
		{
			return new Route
			{
				Method = Method,
				Path = Path,
				Response = Response.Clone()
			};
		}

		private struct RouteKey
		{
			internal string Method { get; set; }

			internal string Path { get; set; }
		}

		public override bool Equals(object obj)
		{
			if (obj is Route route)
				return _key.Equals(route._key);

			return false;
		}

		public override int GetHashCode()
		{
			return _key.GetHashCode();
		}
	}
}