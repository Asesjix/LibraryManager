// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.Extensions.CommandLineUtils;

namespace Microsoft.Web.LibraryManager.Tools.Commands
{
    internal class LibmanApp : BaseCommand
    {
        public LibmanApp(IHostEnvironment hostEnvironment, bool throwOnUnexpectedArg = true) 
            : base(throwOnUnexpectedArg, "dotnet libman", Resources.LibmanCommandDesc, hostEnvironment)
        {

        }

        public override BaseCommand Configure(CommandLineApplication parent = null)
        {
            base.Configure(parent);

            VersionOption("--version", GetVersion, null);

            Commands.Add(new InitCommand(HostEnvironment).Configure(this));
            Commands.Add(new InstallCommand(HostEnvironment).Configure(this));
            Commands.Add(new UninstallCommand(HostEnvironment).Configure(this));
            Commands.Add(new RestoreCommand(HostEnvironment).Configure(this));
            Commands.Add(new CleanCommand(HostEnvironment).Configure(this));
            Commands.Add(new CacheCommand(HostEnvironment).Configure(this));

            // These are shown by child commands.
            RootDir.ShowInHelpText = false;
            Verbosity.ShowInHelpText = false;

            return this;
        }

        private string GetVersion()
        {
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(GetType().Assembly.Location);
            return $"{info.ProductMajorPart}.{info.ProductMinorPart}.{info.ProductBuildPart}.{info.ProductPrivatePart}";
        }
    }
}