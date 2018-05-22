// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Web.LibraryManager.Contracts
{
    /// <summary>
    /// Helper class for basic read and write file operations 
    /// </summary>
    public static class FileHelpers
    {

        /// <summary>
        /// Writes a Stream to a destination file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sourceStream"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<bool> WriteToFileAsync(string fileName, Stream sourceStream, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            string directoryPath = Path.GetDirectoryName(fileName);

            if (directoryPath != null && !string.IsNullOrEmpty(directoryPath))
            {
                DirectoryInfo dir = Directory.CreateDirectory(directoryPath);
                using (FileStream destination = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await sourceStream.CopyToAsync(destination);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Reads from a file a returns its content as text
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<string> ReadFileAsTextAsync(string fileName, CancellationToken cancellationToken)
        {
            using (Stream s = await ReadFileAsStreamAsync(fileName, cancellationToken).ConfigureAwait(false))
            using (var r = new StreamReader(s, Encoding.UTF8, true, 8192, true))
            {
                return await r.ReadToEndAsync().WithCancellation(cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Copies a file from source to destination
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        /// <returns></returns>
        public static bool FileCopy(string sourceFile, string destinationFile)
        {
            try
            {
                File.Copy(sourceFile, destinationFile, true);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Reads from a file a returns its content as Stream
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<Stream> ReadFileAsStreamAsync(string fileName, CancellationToken cancellationToken)
        {
            return Task.FromResult<Stream>(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1, useAsync: true));
        }

        /// <summary>
        /// Returns true if both files have same same, size and last written time
        /// </summary>
        /// <param name="destinationFile"></param>
        /// <param name="cacheFile"></param>
        /// <returns></returns>
        public static bool AreFilesUpToDate(FileInfo destinationFile, FileInfo cacheFile)
        {
            if (string.Compare(destinationFile.Name, cacheFile.Name) != 0 ||
                cacheFile.Length != destinationFile.Length ||
                cacheFile.LastWriteTime.CompareTo(destinationFile.LastWriteTime) > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="deleteCleanFolders"></param>
        /// <returns></returns>
        public static bool DeleteFiles(IEnumerable<string> filePaths, CancellationToken cancellationToken, bool deleteCleanFolders = true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            HashSet<string> directories = new HashSet<string>();

            try
            {
                foreach (string filePath in filePaths)
                {
                    if (File.Exists(filePath))
                    {
                        DeleteFileFromDisk(filePath);
                    }

                    if (deleteCleanFolders)
                    {
                        string directoryPath = Path.GetDirectoryName(filePath);
                        if (Directory.Exists(directoryPath))
                        {
                            if (!directories.Contains(directoryPath))
                            {
                                directories.Add(directoryPath);
                            }
                        }
                    }
                }

                if (deleteCleanFolders)
                {
                    DeleteEmptyFoldersFromDisk(directories);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes folders if empty
        /// </summary>
        /// <param name="folderPaths"></param>
        /// <returns></returns>
        public static bool DeleteEmptyFoldersFromDisk(IEnumerable<string> folderPaths)
        {

            try
            {
                foreach (string path in folderPaths)
                {
                    DirectoryInfo directory = new DirectoryInfo(path);
                    if (directory.Exists && IsDirectoryEmpty(path))
                    {
                        directory.Delete(true);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Returns true is directory is empty
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        /// <summary>
        /// Deletes a file from disk if exists
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DeleteFileFromDisk(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    File.Delete(filePath);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
