namespace HttpMock.VisualServer.Model
{
	public class RouteUI
	{
		public string Method { get; set; }

		public string Path { get; set; }

		public ResponseUI Response { get; set; }

		internal RouteUI Clone()
		{
			return new RouteUI
			{
				Method = Method,
				Path = Path,
				Response = Response.Clone()
			};
		}

		internal void Update(RouteUI source)
		{
			Method = source.Method;
			Path = source.Path;
			Response = source.Response.Clone();
		}
	}
}
