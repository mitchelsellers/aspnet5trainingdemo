# Automated Build Prep

## Add Version # support to all project

```
  <PropertyGroup>
    <Version>0.0.0</Version>
  </PropertyGroup>
```

## Add Build Support Files

### Add build/aspnetcore.yml 

This holds the full build stuff.

```
#Define required parameters
parameters:
  PublishTarget: 'win-x64'
  BuildConfiguration: 'Release'
  NetCoreVersion: 5.0.301
  EfCoreVersion: 5.0.8
  InstallNetCoreTools: false
  InstallEfCoreTools: false
  BuildProjectsPattern: '**/*.csproj'
  TestProjectsPattern: '**/*tests.csproj'
  RunUnitTests: True
  ScriptEntityFramework: False
  PatchEfScripts: True
  EFContextProjectDirectory: ''
  EFStartupProjectDirectory: ''

#Validate all parameters

jobs:
- job: BuildAspnetCore
  displayName: Build ASP.NET Core App

  pool:
    vmImage: 'windows-2019'
    demands: vstest

  steps:
  - task: iowacomputergurus.dotnetcore-pipeline-tasks.set-dot-net-core-assembly-version.set-dotnetcore-assembly-version@2
    displayName: 'Set .NET Core Assembly Version'

  - task: UseDotNet@2
    displayName: 'Use .Net Core sdk  ${{ parameters.NetCoreVersion }}'
    inputs:
      version: ${{ parameters.NetCoreVersion }}
    condition: and(succeeded(), eq('${{ parameters.InstallNetCoreTools }}', 'True'))

  - script: 'dotnet tool install --global dotnet-ef --version ${{ parameters.EfCoreVersion }}'
    displayName: 'Install EFCore Tools  ${{ parameters.EfCoreVersion }}'
    condition: and(succeeded(), eq('${{ parameters.InstallEfCoreTools }}', 'True'))

  - task: DotNetCoreCLI@2
    displayName: Build
    inputs:
      projects: '**/*.sln'
      arguments: '--configuration ${{ parameters.BuildConfiguration }}'

  - task: DotNetCoreCLI@2
    displayName: Test
    inputs:
      command: test
      projects: '${{ parameters.TestProjectsPattern }}'
      arguments: '--configuration ${{ parameters.BuildConfiguration }} --no-build --collect "Code coverage"'
    condition: and(succeeded(), eq('${{ parameters.RunUnitTests }}', 'True'))

  - task: DotNetCoreCLI@2
    displayName: Publish
    inputs:
      command: publish
      publishWebProjects: True
      arguments: '-r ${{ parameters.PublishTarget }} --configuration ${{ parameters.BuildConfiguration }} --output $(build.artifactstagingdirectory)/App'
      zipAfterPublish: True
    condition: and(succeeded(), ne(variables['Build.Reason'], 'PullRequest'))


  - task: iowacomputergurus.dotnetcore-pipeline-tasks.azure-devops-efcore-script-task.azure-devops-efcore-script-task@2
    displayName: 'Script EFCore Migrations - ${{ parameters.PatchEfScripts }}'
    inputs:
      contextProjectDirectory: ${{ parameters.EFContextProjectDirectory }}
      startupProjectDirectory: ${{ parameters.EFStartupProjectDirectory }}
      patchForIdempotentIndexBug: ${{ parameters.PatchEfScripts }}
    condition: and(succeeded(), eq('${{ parameters.ScriptEntityFramework }}', 'True'), ne(variables['Build.Reason'], 'PullRequest'))


  - task: PublishBuildArtifacts@1
    displayName: 'Publish Artifact'
    inputs:
      PathtoPublish: '$(build.artifactstagingdirectory)/App'
      ArtifactName: PublishedApp
    condition: and(succeeded(), ne(variables['Build.Reason'], 'PullRequest'))


  - task: PublishBuildArtifacts@1
    displayName: 'Publish Artifact: Migrations'
    inputs:
      PathtoPublish: '$(Build.ArtifactStagingDirectory)/Migrations'
      ArtifactName: MigrationScript
    condition: and(succeeded(), eq('${{ parameters.ScriptEntityFramework }}', 'True'), ne(variables['Build.Reason'], 'PullRequest'))
```

### Add azure-popelines.yml to project toot

```
trigger:
  branches:
    include:
      - develop
      - main

variables:
  Version.Major: '1'
  Version.Minor: '1'
  Version.CounterKey: 1.1
  Version.Revision: $[counter(variables['Version.CounterKey'], 0)]
  SYSTEM_ACCESSTOKEN: $(System.AccessToken)

#Force a clean formatted name
name: $(Version.Major).$(Version.Minor).$(Version.Revision)

jobs: 
- template: build/aspnetcore.yml
  parameters:
    PublishTarget: 'win-x64'
    EFContextProjectDirectory: 'src/SampleWeb.Data'
    EFStartupProjectDirectory: 'src/SampleWeb.Web'
    NetCoreVersion: 5.0.301
    EfCoreVersion: 5.0.8
    InstallNetCoreTools: True
    InstallEfCoreTools: True
    ScriptEntityFramework: True    
    PatchEfScripts: False
```

***

  Notes for SETUP
  
  Veersion - $(MajorVersion).$(MinorVersion)$(rev:.r)
  
  Version (Releae) - $(Build.BuildNumber)-Release-$(rev:r)