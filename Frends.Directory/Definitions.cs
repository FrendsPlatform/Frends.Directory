using System.ComponentModel;
using Frends.Tasks.Attributes;
#pragma warning disable 1591

namespace Frends.Directory
{
    public class SharedInput
    {
        [DefaultValue("\"c:\\temp\"")]
        public string Directory { get; set; }
    }

    public class CreateOptions
    {
        /// <summary>
        /// If set, allows you to give the user credentials to use to create directories on remote hosts.
        /// If not set, the agent service user credentials will be used.
        /// Note: For creating directories on the local machine, the agent service user credentials will always be used, even if this option is set.
        /// </summary>
        public bool UseGivenUserCredentialsForRemoteConnections { get; set; }

        /// <summary>
        /// This needs to be of format domain\username
        /// </summary>
        [DefaultValue("\"domain\\username\"")]
        [ConditionalDisplay(nameof(UseGivenUserCredentialsForRemoteConnections), true)]
        public string UserName { get; set; }

        [PasswordPropertyText]
        [ConditionalDisplay(nameof(UseGivenUserCredentialsForRemoteConnections), true)]
        public string Password { get; set; }
    }
    public class DeleteOptions
    {
        /// <summary>
        /// Delete all files and sub folders
        /// </summary>
        public bool DeleteRecursively { get; set; }

        /// <summary>
        /// If set, allows you to give the user credentials to use to delete directories on remote hosts.
        /// If not set, the agent service user credentials will be used.
        /// Note: For deleting directories on the local machine, the agent service user credentials will always be used, even if this option is set.
        /// </summary>
        public bool UseGivenUserCredentialsForRemoteConnections { get; set; }

        /// <summary>
        /// This needs to be of format domain\username
        /// </summary>
        [DefaultValue("\"domain\\username\"")]
        [ConditionalDisplay(nameof(UseGivenUserCredentialsForRemoteConnections), true)]
        public string UserName { get; set; }

        [PasswordPropertyText]
        [ConditionalDisplay(nameof(UseGivenUserCredentialsForRemoteConnections), true)]
        public string Password { get; set; }
    }

    public class MoveInput
    {
        [DefaultValue("\"c:\\temp\"")]
        public string SourceDirectory { get; set; }

        [DefaultValue("\"c:\\newTemp\"")]
        public string TargetDirectory { get; set; }
    }

    public enum DirectoryExistsAction { Throw, Rename, Overwrite }

    public class MoveOptions
    {
        /// <summary>
        /// What to do if target directory already exists.
        ///  * Throw - Throw an error (the default)
        ///  * Rename - Create a new directory with a name that appends a number to the end, e.g. "directory(2)"
        ///  * Overwrite - Overwrite the target directory, by removing it first before moving the source directory
        /// </summary>
        public DirectoryExistsAction IfTargetDirectoryExists { get; set; }

        /// <summary>
        /// If set, allows you to give the user credentials to use to move directories on remote hosts.
        /// If not set, the agent service user credentials will be used.
        /// Note: For moving directories on the local machine, the agent service user credentials will always be used, even if this option is set.
        /// </summary>
        public bool UseGivenUserCredentialsForRemoteConnections { get; set; }

        /// <summary>
        /// This needs to be of format domain\username
        /// </summary>
        [DefaultValue("\"domain\\username\"")]
        [ConditionalDisplay(nameof(UseGivenUserCredentialsForRemoteConnections), true)]
        public string UserName { get; set; }

        [PasswordPropertyText]
        [ConditionalDisplay(nameof(UseGivenUserCredentialsForRemoteConnections), true)]
        public string Password { get; set; }
    }

    public class DeleteResult
    {
        public DeleteResult(string path)
        {
            Path = path;
        }
        public string Path { get; set; }
        public bool DirectoryNotFound { get; set; }
    }

    public class CreateResult
    {
        public CreateResult(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
    }

    public class MoveResult
    {
        public MoveResult(string targetDirectory, string sourceDirectory)
        {
            TargetDirectory = targetDirectory;
            SourceDirectory = sourceDirectory;
        }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
    }
}
