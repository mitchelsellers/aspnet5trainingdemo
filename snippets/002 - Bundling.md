# Bundling & Minification

The goal of this section of the examples is to utilize available tools in .NET 5 to help bundle/minify resources.  This is ONLY for those that do not have other proceses in place.  (Gulp, etc.)

## Install NuGet Package

```
Install-Package BuildBundlerMinifier
```

## Create New BundleConfig.json

Add a file named `bundleconfig.json` to the root of the web project with the following content.

```
[
  {
    "outputFileName": "wwwroot/css/combined.min.css",
    "inputFiles": [
      "wwwroot/lib/bootstrap/dist/css/bootstrap.css",
      "wwwroot/css/site.css"
    ],
    "minify": {
      "enabled": true
    }
  },
  {
    "outputFileName": "wwwroot/js/combined.min.js",
    "inputFiles": [
      "wwwroot/lib/jquery/dist/jquery.js",
      "wwwroot/lib/bootstrap/dist/js/bootstrap.js",
      "wwwroot/lib/jquery-validation/dist/jquery.validate.js",
      "wwwroot/lib/jquery-validation/dist/additional-methods.js",
      "wwwroot/lib/jquery-validation-unobtrustive/jquery.validate.unobtrusive.js",
      "wwwroot/js/site.js"
    ],
    "minify": {
      "enabled": true
    }
  }
]
```

 ### Rebuild solution

 ## Update /Views/Shared/Layout.cshtml
 
Edit Layout, replacing all CSS with this line

```
<link rel="stylesheet" href="/css/combined.min.css" asp-append-version="true"/>
```

Edit Layout, replacing all scripts with this line
```
<script src="~/js/combined.min.js" asp-append-version="true"></script>
```

Delete the _Validation scripts partial