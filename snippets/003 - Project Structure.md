# Expanding Project Architecture

## Create a new project SampleWeb.Data

This new project should be setup as a .NET Core Class Library.  (Could be .NET Standard as well)

## Install these packages

```
 Install-Package Microsoft.EntityFrameworkCore
 Install-Package Microsoft.EntityFrameworkCore.SqlServer
 Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

## Add Project Reference for SampleWeb

Add a project Reference to Data from SampleWeb

## Move Objects
Move the contents of the SampleWeb/Data folder to the Data project root & correct namespaces