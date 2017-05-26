- [Frends.Directory](#frends.directory)
   - [Installing](#installing)
   - [Building](#building)
   - [Contributing](#contributing)
   - [Documentation](#documentation)
     - [Directory.Create](#directory.create)
       - [Input](#input)
       - [Options](#options)
       - [Result](#result)
     - [Directory.Move](#directory.move)
       - [Input](#input)
       - [Options](#options)
       - [Result](#result)
     - [Directory.Delete](#directory.delete)
       - [Input](#input)
       - [Options](#options)
       - [Result](#result)
   - [License](#license)

# Frends.Directory
FRENDS Tasks to preform various operations on file system directories.

## Installing
You can install the task via FRENDS UI Task view or you can find the nuget package from the following nuget feed
`https://www.myget.org/F/frends/api/v2`

## Building
Ensure that you have `https://www.myget.org/F/frends/api/v2` added to your nuget feeds

Clone a copy of the repo

`git clone https://github.com/FrendsPlatform/Frends.Directory.git`

Restore dependencies

`nuget restore frends.directory`

Rebuild the project

Run Tests with nunit3. Tests can be found under

`Frends.Directory.Tests\bin\Release\Frends.Directory.Tests.dll`

Create a nuget package

`nuget pack nuspec/Frends.Directory.nuspec`

## Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

## Documentation

### Directory.Create
Creates all directories and sub-directories in the specified path unless they already exist. If the target directory already exists the task will return a successful response with the path to the target directory.

#### Input

| Property        | Type     | Description                  | Example                 |
|-----------------|----------|------------------------------|---------------------------|
| Directory       | string   | Full path for the directory that is to be created | `c:\temp\` `c:/temp/foo` |

#### Options

| Property                                    | Type           | Description                                    | Example                   |
|---------------------------------------------|----------------|------------------------------------------------|---------------------------|
| UseGivenUserCredentialsForRemoteConnections | bool           | If set, allows you to give the user credentials to use to delete directories on remote hosts. If not set, the agent service user credentials will be used. Note: For deleting directories on the local machine, the agent service user credentials will always be used, even if this option is set.| |
| UserName                                    | string         | Needs to be of format domain\username | `example\Admin` |
| Password                                    | string         | | |

#### Result
object

| Property        | Type     | Description                 |
|-----------------|----------|-----------------------------|
| Path            | string   | Full path for the created directory |


### Directory.Move

Moves a folder and all of its content to a new path. 
Parent folder for target directory needs to exist or the task will throw an exception.

#### Input 

| Property        | Type     | Description                  | Example                 |
|-----------------|----------|------------------------------|---------------------------|
| SourceDirectory | string   | Path to directory to be moved | `c:\temp\foo\` |
| TargetDirectory | string   | Path to the new place for the directory | `c:\temp\bar\` |

#### Options 

| Property                                    | Type           | Description                                    | Example                   |
|---------------------------------------------|----------------|------------------------------------------------|---------------------------|
| IfTargetDirectoryExists                     | Enum{Throw, Rename, Overwrite} | What to do if target directory already exists. Rename will create a new directory with a name that appends a number to the end, e.g. `directory(2)` | |
| UseGivenUserCredentialsForRemoteConnections | bool           | If set, allows you to give the user credentials to use to move directories on remote hosts. If not set, the agent service user credentials will be used. Note: For deleting directories on the local machine, the agent service user credentials will always be used, even if this option is set. | |
| UserName                                    | string         | Needs to be of format domain\username | `example\Admin` |
| Password                                    | string         | | |


#### Result 
object

| Property        | Type   | Description                 |
|-----------------|--------|-----------------------------|
| SourceDirectory | string | Full path for the source directory |
| TargetDirectory | string | Full path for the target directory |

### Directory.Delete 

Deletes a directory. This task will by default throw if the directory is not empty. If the directory is not found, an empty result is returned. 

#### Input 

| Property        | Type     | Description                  | Example                 |
|-----------------|----------|------------------------------|---------------------------|
| Directory       | string   | Full path for the directory that is to be dleted| `c:\temp\` `c:/temp/foo` |

#### Options 

| Property                                    | Type           | Description                                    | Example                   |
|---------------------------------------------|----------------|------------------------------------------------|---------------------------|
| DeleteRecursively                           | bool           | Delete all files and sub-directories in the directory  | |
| UseGivenUserCredentialsForRemoteConnections | bool           | If set, allows you to give the user credentials to use to delete directories on remote hosts. If not set, the agent service user credentials will be used. Note: For deleting directories on the local machine, the agent service user credentials will always be used, even if this option is set. | |
| UserName                | string         | Needs to be of format domain\username | `example\Admin` |
| Password                | string         | | |

#### Result
object

| Property          | Type     | Description                       |
|-------------------|----------|-----------------------------------|
| Path              | string   | Full path for the deleted directory |
| DirectoryNotFound | bool     | If set, the directory to delete was not found. Then the `Path` property will also be empty |

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
