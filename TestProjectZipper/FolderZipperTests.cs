using ConsoleAppZipper;
using NUnit.Framework;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace TestProjectZipper
{
    [TestFixture]
    public class FolderZipperTests
    {
        private FolderZipper folderZipper;
        private readonly string testFolder = "TestFolder";
        private readonly string zipFileName = "TestFolder.zip";
        private readonly string[] excludedExtensions = { ".bin" };
        private readonly string[] excludedDirectories = { "ExcludedDir" };
        private readonly string[] excludedFiles = { "ExcludedFile.txt" };

        [SetUp]
        public void Setup()
        {
            // Create a test folder with some files and directories
            Directory.CreateDirectory(testFolder);
            File.WriteAllText(Path.Combine(testFolder, "File1.txt"), "Content 1");
            File.WriteAllText(Path.Combine(testFolder, "File2.txt"), "Content 2");
            Directory.CreateDirectory(Path.Combine(testFolder, "ExcludedDir"));
            File.WriteAllText(Path.Combine(testFolder, "ExcludedDir", "ExcludedFile.txt"), "Excluded Content");

            folderZipper = FolderZipper.Instance;
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the test folder and zip file after each test
            if (Directory.Exists(testFolder))
            {
                Directory.Delete(testFolder, true);
            }

            if (File.Exists(zipFileName))
            {
                File.Delete(zipFileName);
            }
        }

        [Test]
        public void ZipFolder_ExcludesFilesAndDirectories()
        {
            // Act
            folderZipper.Create(testFolder, zipFileName, excludedExtensions, excludedDirectories, excludedFiles);

            // Assert
            Assert.That(File.Exists(zipFileName), Is.True);

            using ZipArchive archive = ZipFile.OpenRead(zipFileName);
            
            Assert.Multiple(() =>
            {
                // Check that excluded files and directories are not present in the archive
                Assert.That(archive.Entries.Any(entry => entry.FullName.Contains("ExcludedDir")), Is.False);
                Assert.That(archive.Entries.Any(entry => entry.FullName.Contains("ExcludedFile.txt")), Is.False);

                // Check that included files are present in the archive
                Assert.That(archive.Entries.Any(entry => entry.FullName.Contains("File1.txt")), Is.True);
                Assert.That(archive.Entries.Any(entry => entry.FullName.Contains("File2.txt")), Is.True);
            });
        }

        #region Private Methods
        #region IsExtensionExcluded
        [Test]
        public void IsExtensionExcluded_WhenExtensionNotInExcludedExtensions_ReturnsFalse()
        {
            // Arrange
            string fileExtension = ".txt";
            string[] excludedExtensions = { ".jpg", ".png" };

            // Act
            MethodInfo methodInfo = typeof(FolderZipper).GetMethod("IsExtensionExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            bool result = (bool)methodInfo.Invoke(folderZipper, new object[] { fileExtension, excludedExtensions });

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsExtensionExcluded_WhenExtensionInExcludedExtensions_ReturnsTrue()
        {
            // Arrange
            string fileExtension = ".jpg";
            string[] excludedExtensions = { ".jpg", ".png" };

            // Act
            MethodInfo methodInfo = typeof(FolderZipper).GetMethod("IsExtensionExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            bool result = (bool)methodInfo.Invoke(folderZipper, new object[] { fileExtension, excludedExtensions });

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsExtensionExcluded_WithEmptyExcludedExtensions_ReturnsFalse()
        {
            // Arrange
            string fileExtension = ".txt";
            string[] excludedExtensions = { }; // Empty

            // Act
            MethodInfo methodInfo = typeof(FolderZipper).GetMethod("IsExtensionExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            bool result = (bool)methodInfo.Invoke(folderZipper, new object[] { fileExtension, excludedExtensions });

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsExtensionExcluded_WithNullExcludedExtensions_ReturnsFalse()
        {
            // Arrange
            string fileExtension = ".txt";
            string[] excludedExtensions = null;

            // Act
            MethodInfo methodInfo = typeof(FolderZipper).GetMethod("IsExtensionExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            bool result = (bool)methodInfo.Invoke(folderZipper, new object[] { fileExtension, excludedExtensions });

            // Assert
            Assert.That(result, Is.False);
        }
        #endregion

        #region IsDirectoryExcluded
        [Test]
        public void IsDirectoryExcluded_WhenDirectoryNotInExcludedDirectories_ReturnsFalse()
        {
            // Arrange
            string relativePath = "path\\to\\directory";
            string[] excludedDirectories = { "folder1", "folder2" };

            // Act
            MethodInfo methodInfo = typeof(FolderZipper).GetMethod("IsDirectoryExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            bool result = (bool)methodInfo.Invoke(folderZipper, new object[] { relativePath, excludedDirectories });

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsDirectoryExcluded_WhenDirectoryInExcludedDirectories_ReturnsTrue()
        {
            // Arrange
            string relativePath = "path\\to\\folder1";
            string[] excludedDirectories = { "folder1", "folder2" };

            // Act
            MethodInfo methodInfo = typeof(FolderZipper).GetMethod("IsDirectoryExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            bool result = (bool)methodInfo.Invoke(folderZipper, new object[] { relativePath, excludedDirectories });

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsDirectoryExcluded_WithEmptyExcludedDirectories_ReturnsFalse()
        {
            // Arrange
            string relativePath = "path\\to\\folder1";
            string[] excludedDirectories = { };

            // Act
            MethodInfo methodInfo = typeof(FolderZipper).GetMethod("IsDirectoryExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            bool result = (bool)methodInfo.Invoke(folderZipper, new object[] { relativePath, excludedDirectories });

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsDirectoryExcluded_WithNullExcludedDirectories_ReturnsFalse()
        {
            // Arrange
            string relativePath = "path\\to\\folder1";
            string[] excludedDirectories = null;

            // Act
            MethodInfo methodInfo = typeof(FolderZipper).GetMethod("IsDirectoryExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            bool result = (bool)methodInfo.Invoke(folderZipper, new object[] { relativePath, excludedDirectories });

            // Assert
            Assert.That(result, Is.False);
        }
        #endregion

        #region IsFileExcluded
        [Test]
        public void IsFileExcluded_WhenFileNotInExcludedFiles_ReturnsFalse()
        {
            // Arrange
            string fileName = "file.txt";
            string[] excludedFiles = { "file2.txt", "file3.txt" };

            // Act
            MethodInfo methodInfo = typeof(FolderZipper).GetMethod("IsFileExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            bool result = (bool)methodInfo.Invoke(folderZipper, new object[] { fileName, excludedFiles });

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsFileExcluded_WhenFileInExcludedFiles_ReturnsTrue()
        {
            // Arrange
            string fileName = "file.txt";
            string[] excludedFiles = { "file.txt", "file3.txt" };

            // Act
            MethodInfo methodInfo = typeof(FolderZipper).GetMethod("IsFileExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            bool result = (bool)methodInfo.Invoke(folderZipper, new object[] { fileName, excludedFiles });

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsFileExcluded_WithEmptyExcludedFiles_ReturnsFalse()
        {
            // Arrange
            string fileName = "file.txt";
            string[] excludedFiles = { }; // Empty

            // Act
            MethodInfo methodInfo = typeof(FolderZipper).GetMethod("IsFileExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            bool result = (bool)methodInfo.Invoke(folderZipper, new object[] { fileName, excludedFiles });

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsFileExcluded_WithNullExcludedFiles_ReturnsFalse()
        {
            // Arrange
            string fileName = "file.txt";
            string[] excludedFiles = null;

            // Act
            MethodInfo methodInfo = typeof(FolderZipper).GetMethod("IsFileExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            bool result = (bool)methodInfo.Invoke(folderZipper, new object[] { fileName, excludedFiles });

            // Assert
            Assert.That(result, Is.False);
        }
        #endregion
        #endregion
    }
}