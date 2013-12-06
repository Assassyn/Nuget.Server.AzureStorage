using NuGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuget.Server.AzureStorage
{
    public class BlobStorage : IFileSystem
    {
        public void AddFile(string path, Action<System.IO.Stream> writeToStream)
        {
            throw new NotImplementedException();
        }

        public void AddFile(string path, System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream CreateFile(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetCreated(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetDirectories(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFiles(string path, string filter, bool recursive)
        {
            throw new NotImplementedException();
        }

        public string GetFullPath(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastAccessed(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastModified(string path)
        {
            throw new NotImplementedException();
        }

        public ILogger Logger{get;set;}

        public void MakeFileWritable(string path)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream OpenFile(string path)
        {
            throw new NotImplementedException();
        }

        public string Root
        {
            get { throw new NotImplementedException(); }
        }
    }
}
