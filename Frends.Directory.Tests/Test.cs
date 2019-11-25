using System;
using System.IO;
using NUnit.Framework;
using Frends.Directory;

namespace Frends.Directory.Tests
{
    [TestFixture]
    public class Test : IDisposable
    {
        private DisposableFileSystem _context;

        [SetUp]
        public void Setup()
        {
            _context = new DisposableFileSystem();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        [Test]
        public void DeleteFolderNotEmptyShouldThrowIfOptionNotSet()
        {
            _context.CreateFiles("temp/foo.txt");
            var ex = Assert.Throws<IOException>(() => Directory.Delete(new SharedInput() {Directory = Path.Combine(_context.RootPath, "temp")}, new DeleteOptions() {}));
            Assert.That(ex.Message,Does.Contain("The directory is not empty"));
        }

        [Test]
        public void DeleteFolderNotEmptyShouldNotThrowIfOptionSet()
        {
            _context.CreateFiles("temp/foo.txt", "temp/foo1.txt", "temp/foo2.txt", "temp/foo3.txt", "temp/foo4.txt", "temp/foo5.txt");
            var result = Directory.Delete(new SharedInput() { Directory = Path.Combine(_context.RootPath, "temp") }, new DeleteOptions() {DeleteRecursively = true});
            Assert.That(result.Path, Is.EqualTo(Path.Combine(_context.RootPath, "temp")));
        }

        [Test]
        public void DeleteFolderShouldNotThrowIfNotExists()
        {
            _context.CreateFiles("temp/foo.txt");
            var result = Directory.Delete(new SharedInput() { Directory = Path.Combine(_context.RootPath, "temp/whatever") }, new DeleteOptions() { });
            Assert.That(result.DirectoryNotFound, Is.True, "The error flag should have been set");
        }

        [Test]
        public void DeleteFolderShouldDeleteEmptyDirectory()
        {
            _context.CreateFolder("temp");
            var res = Directory.Delete(new SharedInput() { Directory = Path.Combine(_context.RootPath, "temp") }, new DeleteOptions() { });
            Assert.That(res.Path, Is.EqualTo(Path.Combine(_context.RootPath, "temp")));
        }

        [Test]
        public void CreateFolderShouldCreateWholePath()
        {
            var newPath = Path.Combine(_context.RootPath, "temp\\foo\\bar");
            var result = Directory.Create(new SharedInput() { Directory = newPath }, new CreateOptions() {});
            Assert.That(result.Path, Is.EqualTo(newPath));
        }

        [Test]
        public void CreateFolderShouldDoNothingIfPathExists()
        {
            _context.CreateFiles("temp/foo/bar/foo.txt");
            var newPath = Path.Combine(_context.RootPath, "temp\\foo\\bar");
            var result = Directory.Create(new SharedInput() { Directory = newPath }, new CreateOptions() { });
            Assert.That(result.Path, Is.EqualTo(newPath));
        }

        [Test]
        public void MoveShouldThrowIfDestinationFolderExistsAndBehaviourIsThrow()
        {
            _context.CreateFiles("temp/foo/bar/foo.txt");
            _context.CreateFiles("temp/bar/foo.txt");
            var sourcePath = Path.Combine(_context.RootPath, "temp\\foo\\bar");
            var targetPath = Path.Combine(_context.RootPath, "temp\\bar");
            var ex = Assert.Throws<IOException>(() => Directory.Move(new MoveInput() { SourceDirectory = sourcePath, TargetDirectory = targetPath }, new MoveOptions() { IfTargetDirectoryExists = DirectoryExistsAction.Throw }));
            Assert.That(ex.Message,Does.Contain("Cannot create a file when that file already exists"));
        }

        [Test]
        public void MoveShouldCreateCopyIfDestinationFolderExistsAndBehaviourIsCopy()
        {
            _context.CreateFiles("temp/foo/bar/foo.txt");
            _context.CreateFiles("temp/bar/foo.txt");
            var sourcePath = Path.Combine(_context.RootPath, "temp\\foo\\bar");
            var targetPath = Path.Combine(_context.RootPath, "temp\\bar");
            var result = Directory.Move(new MoveInput() { SourceDirectory = sourcePath, TargetDirectory = targetPath}, new MoveOptions() { IfTargetDirectoryExists = DirectoryExistsAction.Rename});
            Assert.That(result.TargetDirectory, Is.EqualTo(targetPath+"(1)"));
        }

        [Test]
        public void MoveShouldOverwriteIfDestinationFolderExistsAndBehaviourIsOverwrite()
        {
            _context.CreateFiles("temp/foo/bar/foo.txt");
            _context.CreateFiles("temp/bar/foo.txt");
            var sourcePath = Path.Combine(_context.RootPath, "temp\\foo\\bar");
            var targetPath = Path.Combine(_context.RootPath, "temp\\bar");

            var result = Directory.Move(new MoveInput() { SourceDirectory = sourcePath, TargetDirectory = targetPath }, new MoveOptions() { IfTargetDirectoryExists = DirectoryExistsAction.Overwrite });
            Assert.That(result.TargetDirectory, Is.EqualTo(targetPath));
        }
    }
}
