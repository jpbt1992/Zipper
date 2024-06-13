# Zipper
Command line application that can create a zip of a folder and its subfolders, excluding certain extensions, folders, or file names.

The program also allows the output file to be generated to a local folder, copied to a file share, or sent as an attachment via Email (SMTP).
Develop the program using OOP and SOLID best practices and respective unit and integration tests.

## Requirement #1
The user can invoke the application via the command line by passing the following arguments:
- the folder to zip (e.g. C:\\temp)
- the final name of the zip file (e.g. final.zip)
- a list of extensions to exclude (e.g. .bmp, .jpg, .txt)
- a list of directories to exclude (e.g. git, directory)
- a list of files to exclude (e.g. file1, file2)
- output type (e.g. localFile, filesShare, SMTP)
- optional parameters according to the type of output (e.g. fileshare path)
  
## Requirement #2
All files and folders must be included in the output file in a ZIP file.

## Requirement #3
Create an “outputs” design in which it is easy to develop new outputs in the future.

## Requirement #4
Develop unit tests for the code, achieving as much coverage as possible.

## Requirement #5
The application can be run on either .NET Core 3.1 or .NET Framework 4.8.
