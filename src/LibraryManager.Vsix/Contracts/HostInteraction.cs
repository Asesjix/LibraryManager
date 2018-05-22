// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.Web.LibraryManager.Contracts;

namespace Microsoft.Web.LibraryManager.Vsix
{
    internal class HostInteraction : IHostInteraction
    {
        public HostInteraction(string configFilePath, ILogger logger)
        {
            string cwd = Path.GetDirectoryName(configFilePath);
            WorkingDirectory = cwd;
            Logger = logger;
        }

        public string WorkingDirectory { get; }
        public string CacheDirectory => Constants.CacheFolder;
        public ILogger Logger { get; internal set; }

        public async Task<bool> WriteFileAsync(string relativePath, Func<Stream> content, ILibraryInstallationState state, CancellationToken cancellationToken)
        {
            FileInfo absolutePath = new FileInfo(Path.Combine(WorkingDirectory, relativePath));

            if (absolutePath.Exists)
            {
                return true;
            }

            if (!absolutePath.FullName.StartsWith(WorkingDirectory))
            {
                throw new UnauthorizedAccessException();
            }

            absolutePath.Directory.Create();

            using (Stream stream = content.Invoke())
            {
                if (stream == null)
                    return false;

                VsHelpers.CheckFileOutOfSourceControl(absolutePath.FullName);

                
                await FileHelpers.WriteToFileAsync(absolutePath.FullName, stream, cancellationToken);

            }

            Logger.Log(string.Format(LibraryManager.Resources.Text.FileWrittenToDisk, relativePath.Replace(Path.DirectorySeparatorChar, '/')), LogLevel.Operation);

            return true;
        }

        public async Task<bool> DeleteFilesAsync(IEnumerable<string> relativeFilePaths, CancellationToken cancellationToken, bool deleteCleanFolders = true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            await Task.Run(() => {

                HashSet<string> directories = new HashSet<string>();

                foreach (string relativeFilePath in relativeFilePaths)
                {
                    string absoluteFile = new FileInfo(Path.Combine(WorkingDirectory, relativeFilePath)).FullName;

                    if (cancellationToken.IsCancellationRequested)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    if (File.Exists(absoluteFile))
                    {
                        DeleteFileAsync(absoluteFile);
                    }

                    if (deleteCleanFolders)
                    {
                        string directoryPath = Path.GetDirectoryName(absoluteFile);
                        if (Directory.Exists(directoryPath))
                        {
                            if (!directories.Contains(directoryPath))
                            {
                                directories.Add(directoryPath);
                                // TO DO : replace for DeleteFolder that also calls 
                                // DeleteFolderFromProject as needed
                            }
                        }
                    }
                }

                if (deleteCleanFolders)
                {
                    FileHelpers.DeleteEmptyFoldersFromDisk(directories);
                }

            });
            

            return true;
        }

        private bool DeleteFileAsync(string filePath)
        {
            ProjectItem item = VsHelpers.DTE.Solution.FindProjectItem(filePath);
            Project project = item?.ContainingProject;
            bool deleteSucceeded = false;

            if (project != null)
            {
                deleteSucceeded = VsHelpers.DeleteFileFromProject(item);
            }
            else
            {
                deleteSucceeded = FileHelpers.DeleteFileFromDisk(filePath);
            }

            if (deleteSucceeded)
            {
                Logger.Log(string.Format(LibraryManager.Resources.Text.FileDeleted, filePath.Replace(Path.DirectorySeparatorChar, '/')), LogLevel.Operation);
            }
            else
            {
                Logger.Log(string.Format(LibraryManager.Resources.Text.FileDeleteFail, filePath.Replace(Path.DirectorySeparatorChar, '/')), LogLevel.Operation);
            }

            return deleteSucceeded;
        }

        private Task<bool> DeleteFolderFromDisk (string folderPath)
        {
            return Task.Run(() =>
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(folderPath);
                    if (directory.Exists && IsDirectoryEmpty(folderPath))
                    {
                        directory.Delete(true);
                    }

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });

        }

        private bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public Task<Stream> ReadFileAsync(string filePath, CancellationToken cancellationToken)
        {
            return FileHelpers.ReadFileAsStreamAsync(filePath, cancellationToken);
        }

        public Task<bool> CopyFile(string destinationPath, Func<string> sourcePath, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            try
            {
                FileInfo absoluteDestinationFile = new FileInfo(Path.Combine(WorkingDirectory, destinationPath));

                if (absoluteDestinationFile.Exists)
                {
                    return Task.FromResult(true);
                }

                if (!absoluteDestinationFile.FullName.StartsWith(WorkingDirectory))
                {
                    throw new UnauthorizedAccessException();
                }

                absoluteDestinationFile.Directory.Create();
                string sourceAbsolutePath = sourcePath.Invoke();

                FileHelpers.FileCopy(sourceAbsolutePath, absoluteDestinationFile.FullName);

                Logger.Log(string.Format(LibraryManager.Resources.Text.FileWrittenToDisk, absoluteDestinationFile.FullName.Replace(Path.DirectorySeparatorChar, '/')), LogLevel.Operation);

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
