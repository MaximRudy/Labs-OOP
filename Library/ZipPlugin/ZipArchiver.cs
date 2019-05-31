using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using PluginInterface;
using ICSharpCode.SharpZipLib.Zip;

namespace ZipPlugin
{
    [Description("Zip-архиватор")]
    public class ZipArchiver : IPlugin
    {
        public void Archive(string fileName, Stream sourceStream)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                using (ZipOutputStream zip = new ZipOutputStream(fs))
                {
                    ZipEntry entry = new ZipEntry(GetEntryName(fileName));
                    zip.PutNextEntry(entry);
                    sourceStream.Seek(0, SeekOrigin.Begin);
                    byte[] bytes = new byte[sourceStream.Length];
                    sourceStream.Read(bytes, 0, (int)sourceStream.Length);
                    zip.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public Stream Dearchive(string fileName)
        {
            Stream stream = new MemoryStream();
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                ZipFile zip = new ZipFile(fs);
                ZipEntry entry = zip.GetEntry(ZipEntry.CleanName(GetEntryName(fileName)));
                using (Stream zipStream = zip.GetInputStream(entry))
                {
                    byte[] buffer = new byte[fs.Length];
                    int read = -1;
                    while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        stream.Write(buffer, 0, read);
                    }
                }
            }
            return stream;
        }

        public string GetExt()
        {
            return "zip";
        }

        private string GetEntryName(string fileName)
        {
            return fileName.Remove(fileName.LastIndexOf('.')).Substring(fileName.LastIndexOf('\\') + 1);
        }
    }
}