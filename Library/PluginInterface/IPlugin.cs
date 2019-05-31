using System;
using System.IO;

namespace PluginInterface
{
    public interface IPlugin
    {
        void Archive(string fileName, Stream sourceStream);

        Stream Dearchive(string fileName);

        string GetExt();
    }
}
