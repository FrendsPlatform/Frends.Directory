﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
using SimpleImpersonation;
#pragma warning disable 1591

namespace Frends.Directory
{
    public class Directory
    {
        /// <summary>
        /// Delete a directory. This task will by default throw if the directory is not empty. If the directory is not found, an empty result is returned. See https://github.com/FrendsPlatform/Frends.Directory
        /// </summary>
        /// <returns>Object { string Path, bool DirectoryNotFound } </returns>
        public static DeleteResult Delete([PropertyTab] SharedInput input, [PropertyTab] DeleteOptions options)
        {
            if (!options.UseGivenUserCredentialsForRemoteConnections)
            {
                return ExecuteDelete(input, options.DeleteRecursively);
            }

            var domainAndUserName = GetDomainAndUserName(options.UserName);
            return RunAsUser(domainAndUserName[0], domainAndUserName[1], options.Password, () => ExecuteDelete(input, options.DeleteRecursively));

        }

        /// <summary>
        /// Creates all directories and subdirectories in the specified path unless they already exist. Will not do anything if the directory exists. See https://github.com/FrendsPlatform/Frends.Directory
        /// </summary>
        /// <returns>Object { string Path } </returns>
        public static CreateResult Create([PropertyTab] SharedInput input, [PropertyTab] CreateOptions options)
        {
            if (!options.UseGivenUserCredentialsForRemoteConnections)
            {
                return ExecuteCreate(input);
            }

            var domainAndUserName = GetDomainAndUserName(options.UserName);
            return RunAsUser(domainAndUserName[0], domainAndUserName[1], options.Password, () => ExecuteCreate(input));
        }

        private static T RunAsUser<T>(string domain, string username, string password, Func<T> action) where T : DirectoryResultBase
        {
#if !NET461
            // For some reason impersonation is not working on the Core agent even when running on Windows, will have to be investigated
            // Works fine on the Framework agent for both framework and standard Processes
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new Exception("Impersonation only supported on Windows systems");
            }
#endif
            
            return Impersonation.RunAsUser(new UserCredentials(domain, username, password),
                LogonType.NewCredentials, action);
        }


        /// <summary>
        /// Moves a directory. By default will throw an error if the directory already exists. See https://github.com/FrendsPlatform/Frends.Directory
        /// </summary>
        /// <returns>Object { string TargetDirectory, string SourceDirectory }</returns>
        public static MoveResult Move([PropertyTab] MoveInput input, [PropertyTab] MoveOptions options)
        {
            if (!options.UseGivenUserCredentialsForRemoteConnections)
            {
                return ExecuteMove(input, options.IfTargetDirectoryExists);
            }

            var domainAndUserName = GetDomainAndUserName(options.UserName);
            return RunAsUser(domainAndUserName[0], domainAndUserName[1], options.Password, () =>
                    ExecuteMove(input, options.IfTargetDirectoryExists));
        }

        private static MoveResult ExecuteMove(MoveInput input, DirectoryExistsAction directoryExistsAction)
        {
            var destinationFolderPath = input.TargetDirectory;
            switch (directoryExistsAction)
            {
                case DirectoryExistsAction.Rename:
                    var count = 1;
                    while (System.IO.Directory.Exists(destinationFolderPath))
                    {
                        destinationFolderPath = $"{destinationFolderPath}({count++})";
                    }
                    break;
                case DirectoryExistsAction.Overwrite:
                    if (System.IO.Directory.Exists(destinationFolderPath))
                    {
                        System.IO.Directory.Delete(destinationFolderPath, true);
                    }
                    break;
                case DirectoryExistsAction.Throw: //Will throw if target folder exist
                    break;
            }

            System.IO.Directory.Move(input.SourceDirectory, destinationFolderPath);
            return new MoveResult(destinationFolderPath, input.SourceDirectory);
        }

        private static CreateResult ExecuteCreate(SharedInput input)
        {
            var newFolder = System.IO.Directory.CreateDirectory(input.Directory);
            return new CreateResult(newFolder.FullName);
        }

        private static DeleteResult ExecuteDelete(SharedInput input, bool optionsDeleteRecursivly)
        {
            if (!System.IO.Directory.Exists(input.Directory))
            {
                return new DeleteResult("") { DirectoryNotFound = true };
            }
            System.IO.Directory.Delete(input.Directory, optionsDeleteRecursivly);
            return new DeleteResult(input.Directory);
        }

        private static string[] GetDomainAndUserName(string username)
        {
            var domainAndUserName = username.Split('\\');
            if (domainAndUserName.Length != 2)
            {
                throw new ArgumentException($@"UserName field must be of format domain\username was: {username}");
            }
            return domainAndUserName;
        }

    }
}
