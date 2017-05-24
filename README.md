[TOC]

# Task documentation #

## Directory.Create ##
Creates all directories and sub-directories in the specified path unless they already exist. If the target directory already exists the task will return a successful response with the path to the target directory.

### Input ###

| Property        | Type     | Description                  | Example                 |
|-----------------|----------|------------------------------|---------------------------|
| Directory       | string   | Full path for the directory that is to be created | `c:\temp\` `c:/temp/foo` |

### Options ###

| Property                                    | Type           | Description                                    | Example                   |
|---------------------------------------------|----------------|------------------------------------------------|---------------------------|
| UseGivenUserCredentialsForRemoteConnections | bool           | If set, allows you to give the user credentials to use to delete directories on remote hosts. If not set, the agent service user credentials will be used. Note: For deleting directories on the local machine, the agent service user credentials will always be used, even if this option is set.| |
| UserName                                    | string         | Needs to be of format domain\username | `example\Admin` |
| Password                                    | string         | | |

### Result ###
object

| Property        | Type     | Description                 |
|-----------------|----------|-----------------------------|
| Path            | string   | Full path for the created directory |


## Directory.Move ##

Moves a folder and all of its content to a new path. 
Parent folder for target directory needs to exist or the task will throw an exception.

### Input ###

| Property        | Type     | Description                  | Example                 |
|-----------------|----------|------------------------------|---------------------------|
| SourceDirectory | string   | Path to directory to be moved | `c:\temp\foo\` |
| TargetDirectory | string   | Path to the new place for the directory | `c:\temp\bar\` |

### Options ###

| Property                                    | Type           | Description                                    | Example                   |
|---------------------------------------------|----------------|------------------------------------------------|---------------------------|
| IfTargetDirectoryExists                     | Enum{Throw, Rename, Overwrite} | What to do if target directory already exists. Rename will create a new directory with a name that appends a number to the end, e.g. `directory(2)` | |
| UseGivenUserCredentialsForRemoteConnections | bool           | If set, allows you to give the user credentials to use to move directories on remote hosts. If not set, the agent service user credentials will be used. Note: For deleting directories on the local machine, the agent service user credentials will always be used, even if this option is set. | |
| UserName                                    | string         | Needs to be of format domain\username | `example\Admin` |
| Password                                    | string         | | |


### Result ###
object

| Property        | Type   | Description                 |
|-----------------|--------|-----------------------------|
| SourceDirectory | string | Full path for the source directory |
| TargetDirectory | string | Full path for the target directory |

## Directory.Delete ##

Deletes a directory. This task will by default throw if the directory is not empty. If the directory is not found, an empty result is returned. 

### Input ###

| Property        | Type     | Description                  | Example                 |
|-----------------|----------|------------------------------|---------------------------|
| Directory       | string   | Full path for the directory that is to be dleted| `c:\temp\` `c:/temp/foo` |

### Options ###

| Property                                    | Type           | Description                                    | Example                   |
|---------------------------------------------|----------------|------------------------------------------------|---------------------------|
| DeleteRecursively                           | bool           | Delete all files and sub-directories in the directory  | |
| UseGivenUserCredentialsForRemoteConnections | bool           | If set, allows you to give the user credentials to use to delete directories on remote hosts. If not set, the agent service user credentials will be used. Note: For deleting directories on the local machine, the agent service user credentials will always be used, even if this option is set. | |
| UserName                | string         | Needs to be of format domain\username | `example\Admin` |
| Password                | string         | | |

### Result ###
object

| Property          | Type     | Description                       |
|-------------------|----------|-----------------------------------|
| Path              | string   | Full path for the deleted directory |
| DirectoryNotFound | bool     | If set, the directory to delete was not found. Then the `Path` property will also be empty |