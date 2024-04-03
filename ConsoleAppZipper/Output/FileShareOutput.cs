using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ConsoleAppZipper.Output
{
    public class FileShareOutput : IOutput
    {
        private readonly string fileSharePath;

        public FileShareOutput(string fileSharePath)
        {
            this.fileSharePath = fileSharePath;
        }

        public void Save(string zipFilePath)
        {
            if (string.IsNullOrWhiteSpace(fileSharePath))
            {
                throw new InvalidOperationException("File share path not provided.");
            }

            if (!Directory.Exists(fileSharePath))
            {
                Directory.CreateDirectory(fileSharePath);

                // Set permissions for the user or group
                SetFolderPermissions(fileSharePath, FileSystemRights.FullControl, AccessControlType.Allow);

                Console.WriteLine($"Shared folder '{fileSharePath}' created successfully.");

                string outputFilePath = Path.Combine(fileSharePath, Path.GetFileName(zipFilePath));

                File.Move(zipFilePath, outputFilePath, true);
                Console.WriteLine($"ZIP file saved to file share: {outputFilePath}");
            }
            else
            {
                Console.WriteLine($"Folder '{fileSharePath}' already exists.");
            }

        }

        private static void SetFolderPermissions(string folderPath, FileSystemRights rights, AccessControlType controlType)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

            try
            {
                // Use well-known SID for "Everyone"
                SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

                // Specify the SID and set permissions
                FileSystemAccessRule accessRule = new FileSystemAccessRule(sid, rights, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, controlType);

                // Add the rule to the directory security
                directorySecurity.AddAccessRule(accessRule);

                // Set the updated directory security
                directoryInfo.SetAccessControl(directorySecurity);

                Console.WriteLine($"Permissions set for 'Everyone' on folder '{folderPath}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}