using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frends.Directory.Tests
{
    public class DisposableFileSystem : IDisposable
    {
        public DisposableFileSystem()
        {
            RootPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            System.IO.Directory.CreateDirectory(RootPath);
            DirectoryInfo = new DirectoryInfo(RootPath);
        }

        public string RootPath { get; }

        public DirectoryInfo DirectoryInfo { get; }

        public DisposableFileSystem CreateFolder(string path)
        {
            System.IO.Directory.CreateDirectory(Path.Combine(RootPath, path));
            return this;
        }

        public DisposableFileSystem CreateFile(string path, string content)
        {
            System.IO.File.WriteAllText(Path.Combine(RootPath, path), content);
            return this;
        }

        public DisposableFileSystem CreateFiles(params string[] fileRelativePaths)
        {
            foreach (var path in fileRelativePaths)
            {
                var fullPath = Path.Combine(RootPath, path);
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                System.IO.File.WriteAllText(
                    fullPath,
                    string.Format("Automatically generated for testing on {0:yyyy}/{0:MM}/{0:dd} {0:hh}:{0:mm}:{0:ss}", DateTime.UtcNow));
            }

            return this;
        }

        public void Dispose()
        {
            try
            {
                System.IO.Directory.Delete(RootPath, true);
            }
            catch
            {
                // Don't throw if this fails.
            }
        }
    }
}
