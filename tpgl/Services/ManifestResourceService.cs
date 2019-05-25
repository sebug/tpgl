using System;
using System.IO;
using System.Reflection;

namespace tpgl.Services
{
    public class ManifestResourceService : IManifestResourceService
    {
        public string GetManifestResource(string path)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(ManifestResourceService)).Assembly;
            Stream stream = assembly.GetManifestResourceStream(path);
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }
    }
}
