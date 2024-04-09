# Introduction

This is Group's number 16, Second Semester Project Documentation

**Project Name**: Heatington

## Installation

following the official documentation
> [Documentation Link](https://dotnet.github.io/docfx/index.html)

#### To install using .NET SDK (it has to be installed previously)
```shell
dotnet tool update -g docfx
```

## How does it work?
DocFx automatically creates documentation based on the contents of a file.
This Documentation is available in the `api` tab on server.


If you want to add your own, more detailed documentation. Simply add a Markdown (`.md`) file to
`/docs` directory in the right folder coresponding to the real location of a class in a project.

It's available in the `` tab.

## How to run it?

Run in the same directory as `docfx.json`
```shell
docfx docfx.json
```

Now you can preview the website on http://localhost:8080.

# Useful Resources

##### DocFx runs their own (of course) version of Markdown
> [Brief Introduction to a DocFx (15min Video)](https://youtu.be/Sz1lCeedcPI)

> [!Video https://www.youtube.com/embed/Sz1lCeedcPI]

> [Documentation about docfx markdown](https://dotnet.github.io/docfx/docs/markdown.html?tabs=linux%2Cdotnet)

