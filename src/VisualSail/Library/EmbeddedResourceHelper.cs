using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;

namespace AmphibianSoftware.VisualSail.Library
{
	public static class EmbeddedResourceHelper
	{
		private static IDictionary<string,Image> _cache;

		private static void InitializeCache()
		{
			if (_cache == null) {
				_cache = new Dictionary<string,Image> ();
			}
		}

		private static void Dispose()
		{
			if (_cache != null) 
			{
				foreach (var key in _cache.Keys) 
				{
					_cache [key].Dispose ();
				}
			}
		}

		public static Image LoadImage(string name)
		{
			InitializeCache ();

			if (_cache.ContainsKey (name)) 
			{
				return _cache[name];
			}
			else
			{
				var assembly = Assembly.GetEntryAssembly ();

				using (var stream = assembly.GetManifestResourceStream (name)) 
				{
					var image = Image.FromStream (stream);
					_cache [name] = image;
					return image;
				}

			}
		}
		public static string[] GetResourceNames()
		{
			var assembly = Assembly.GetEntryAssembly ();
			return assembly.GetManifestResourceNames ();
		}

		public static Icon LoadIcon(string name)
		{
			var assembly = Assembly.GetEntryAssembly ();

			using (var stream = assembly.GetManifestResourceStream (name)) 
			{
				//todo: figure this out
				return null;
			}
		}
	}
}

