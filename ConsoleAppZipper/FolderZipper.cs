using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ConsoleAppZipper
{
    public class FolderZipper
    {
        #region Private Variables
        private static FolderZipper instance = null;
        private static readonly object lockObject = new object();
        #endregion

        private FolderZipper() { }

        public static FolderZipper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        instance ??= new FolderZipper();
                    }
                }

                return instance;
            }
        }

        public void Create(
            string folderToZip,
            string zipFileName,
            string[] excludedExtensions,
            string[] excludedDirectories,
            string[] excludedFiles)
        {
            File.Delete(zipFileName);

            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), zipFileName);

            using ZipArchive archive = ZipFile.Open(outputPath, ZipArchiveMode.Create);
            foreach (string file in Directory.GetFiles(folderToZip, "*.*", SearchOption.AllDirectories))
            {
                string fileName = Path.GetFileName(file);
                string fileExtension = Path.GetExtension(file);
                string relativeSourcePath = Path.GetRelativePath(folderToZip, file);

                bool isExtensionExcluded = IsExtensionExcluded(fileExtension, excludedExtensions);
                bool isDirectoryExcluded = IsDirectoryExcluded(relativeSourcePath, excludedDirectories);
                bool isFileExcluded = IsFileExcluded(fileName, excludedFiles);

                if (!isExtensionExcluded && !isDirectoryExcluded && !isFileExcluded)
                {
                    // Create an entry in the zip archive with the relative path
                    ZipArchiveEntry entry = archive.CreateEntry(relativeSourcePath);

                    // Add the file to the zip archive
                    using Stream entryStream = entry.Open();
                    using FileStream fileStream = File.OpenRead(file);

                    fileStream.CopyTo(entryStream);
                }
            }
        }

        #region Exclusions
        private bool IsExtensionExcluded(string fileExtension, string[] excludedExtensions)
        {
            if (excludedExtensions == null)
            {
                return false;
            }

            return excludedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
        }

        private bool IsDirectoryExcluded(string relativePath, string[] excludedDirectories)
        {
            bool found = false;

            if (excludedDirectories == null)
            {
                return found;
            }

            string[] directories = relativePath.Split("\\");

            foreach (string directory in directories.Where(where => !where.Contains('.')))
            {
                if (excludedDirectories.Contains(directory, StringComparer.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        private bool IsFileExcluded(string fileName, string[] excludedFiles)
        {
            bool found = false;

            if (excludedFiles == null)
            {
                return found;
            }

            foreach (string excludedFile in excludedFiles)
            {
                if (fileName.Contains(excludedFile, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }
        #endregion
    }
}