﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Microsoft.Web.LibraryManager.Tools.Commands
{
    internal class InitCommand : BaseCommand
    {
        public InitCommand(IHostEnvironment hostEnvironment, bool throwOnUnexpectedArg = true)
            : base(throwOnUnexpectedArg, "init", Resources.InitCommandDesc, hostEnvironment)
        {
        }

        public CommandOption DefaultProvider { get; private set; }
        public CommandOption DefaultDestination { get; private set; }


        public override BaseCommand Configure(CommandLineApplication parent)
        {
            base.Configure(parent);

            DefaultProvider = Option("--default-provider|-dp", Resources.DefaultProviderOptionDesc, CommandOptionType.SingleValue);
            DefaultDestination = Option("--default-destination|-d", Resources.DefaultDestinationOptionDesc, CommandOptionType.SingleValue);

            return this;
        }

        protected async override Task<int> ExecuteInternalAsync()
        {
            FailIfLibmanJsonExists();
            string defaultProvider = string.Empty;
            string defaultDestination = string.Empty;

            defaultProvider = DefaultProvider.HasValue() 
                ? DefaultProvider.Value() 
                : HostEnvironment.InputReader.GetUserInput(nameof(DefaultProvider));

            defaultDestination = DefaultDestination.HasValue()
                ? DefaultDestination.Value()
                : HostEnvironment.InputReader.GetUserInput(nameof(DefaultDestination));

            Manifest manifest = await GetManifestAsync();
            manifest.DefaultDestination = defaultDestination;
            manifest.DefaultProvider = defaultProvider;

            // Add a version to the file.
            manifest.AddVersion(Manifest.SupportedVersions.Last().ToString());

            await manifest.SaveAsync(Settings.ManifestFileName, CancellationToken.None);

            return 0;
        }

        private void FailIfLibmanJsonExists()
        {
            if (File.Exists(Settings.ManifestFileName))
            {
                throw new Exception(Resources.InitFailedLibmanJsonFileExists);
            }
        }
    }
}
