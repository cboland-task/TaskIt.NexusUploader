# TaskIt.NexusUploader
Simple cli tool for uploading folder structures to a [Nexus](https://www.sonatype.com/nexus-repository-oss?smtNoRedir=1) (raw) repository.<br/>
If something goes wrong all uploaded files / folders will be removed from the Nexus repository.

## Installation
Like all dotnet tools this tool can be used on a project / solution level (simply add the nuget dependency) or be installed as a global tool.
Simply use the following commmand:<br/>
`dotnet tool install --global TaskIt.NexusUploader`

### Update
`dotnet tool update --global TaskIt.NexusUploader`

### Uninstall
`dotnet tool uninstall --global TaskIt.NexusUploader`

## Usage

Simply call:<br/>
`NexusUploader <parameters>`

### Parameters
The folder structure in the target Repository (=`targetUrl`) will be like this:<br/>
`<groupId>\<artefactId>\<artifactVersion>`<br/>
The `folder` will not be created as subfolder of the path. Contents of the `folder` will be placed directly under the specified path.


Parameter | Required | Description |
----------|------------ |------------ |
`-u`<br/> `--user` | yes | string - username for the nexus upload api |
`-p`<br/> `--password` | yes | string - password for the nexus upload api.<br/> The password will be Base64 encoded. |
`-t`<br/> `--targetUrl` | yes | string - Nexus repository url  |
`-f`<br/> `--folder` | false | string - Folder root for the uploaded content.<br/> Tf omitted, the current directory will be used as content root. |
`-g`<br/> `--groupId` | yes | string - The group of the artifact |
`-a`<br/> `--artifactId` | yes | string - The artifact name / id |
`-v`<br/> `--artifactVersion` | yes | string - Artifact version / revision |