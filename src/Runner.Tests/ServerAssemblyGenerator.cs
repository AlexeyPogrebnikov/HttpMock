﻿using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace HttpMock.Runner.Tests
{
	internal static class ServerAssemblyGenerator
	{
		internal static void Generate(string serverFileName, string version)
		{
			StringBuilder asmInfo = new();

			asmInfo.AppendLine("using System.Reflection;");
			asmInfo.AppendLine($"[assembly: AssemblyVersion(\"{version}\")]");

			SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(asmInfo.ToString(), encoding: Encoding.Default);

			string mscorlibPath = typeof(object).Assembly.Location;
			MetadataReference mscorlib = MetadataReference.CreateFromFile(mscorlibPath,
				new MetadataReferenceProperties(MetadataImageKind.Assembly));
			CSharpCompilationOptions options = new(OutputKind.DynamicallyLinkedLibrary);

			var compilation = CSharpCompilation.Create("TestServer.dll",
				references: new[] {mscorlib},
				syntaxTrees: new[] {syntaxTree},
				options: options);

			using MemoryStream dllStream = new();
			using MemoryStream pdbStream = new();
			using Stream win32resStream = compilation.CreateDefaultWin32Resources(
				true,
				false,
				null,
				null);
			EmitResult result = compilation.Emit(
				dllStream,
				pdbStream,
				win32Resources: win32resStream);

			File.WriteAllBytes(serverFileName, dllStream.ToArray());
		}
	}
}