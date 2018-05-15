﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Web.LibraryManager.Tools.Commands;

namespace Microsoft.Web.LibraryManager.Tools.Test
{
    [TestClass]
    public class LibmanUpdateTest : CommandTestBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }

        [TestMethod]
        public void TestUpdateCommand()
        {
            var command = new UpdateCommand(HostEnvironment);
            command.Configure(null);

            string contents = @"{
  ""version"": ""1.0"",
  ""defaultProvider"": ""cdnjs"",
  ""defaultDestination"": ""wwwroot"",
  ""libraries"": [
    {
      ""library"": ""jquery@2.2.0"",
      ""files"": [ ""jquery.min.js"", ""jquery.js"" ]
    }
  ]
}";

            string libmanjsonPath = Path.Combine(WorkingDir, "libman.json");
            File.WriteAllText(libmanjsonPath, contents);

            var restoreCommand = new RestoreCommand(HostEnvironment);
            restoreCommand.Configure(null);

            restoreCommand.Execute();

            Assert.IsTrue(File.Exists(Path.Combine(WorkingDir, "wwwroot", "jquery.min.js")));
            Assert.IsTrue(File.Exists(Path.Combine(WorkingDir, "wwwroot", "jquery.js")));

            int result = command.Execute("jquery@2.2.0");

            Assert.AreEqual(0, result);
            Assert.IsTrue(File.Exists(Path.Combine(WorkingDir, "wwwroot", "jquery.min.js")));
            Assert.IsTrue(File.Exists(Path.Combine(WorkingDir, "wwwroot", "jquery.js")));

            string actualText = File.ReadAllText(libmanjsonPath);

            string expectedText = @"{
  ""version"": ""1.0"",
  ""defaultProvider"": ""cdnjs"",
  ""defaultDestination"": ""wwwroot"",
  ""libraries"": [
    {
      ""library"": ""jquery@3.3.1"",
      ""files"": [
        ""jquery.min.js"",
        ""jquery.js""
      ]
    }
  ]
}";
            Assert.AreEqual(StringHelper.NormalizeNewLines(expectedText), StringHelper.NormalizeNewLines(actualText));
        }

        [TestMethod]
        public void TestUpdateCommand_AlreadyUpToDate()
        {
            var command = new UpdateCommand(HostEnvironment);
            command.Configure(null);

            string contents = @"{
  ""version"": ""1.0"",
  ""defaultProvider"": ""cdnjs"",
  ""defaultDestination"": ""wwwroot"",
  ""libraries"": [
    {
      ""library"": ""jquery@3.3.1"",
      ""files"": [ ""jquery.min.js"", ""jquery.js"" ]
    }
  ]
}";

            string libmanjsonPath = Path.Combine(WorkingDir, "libman.json");
            File.WriteAllText(libmanjsonPath, contents);

            var restoreCommand = new RestoreCommand(HostEnvironment);
            restoreCommand.Configure(null);

            restoreCommand.Execute();

            Assert.IsTrue(File.Exists(Path.Combine(WorkingDir, "wwwroot", "jquery.min.js")));
            Assert.IsTrue(File.Exists(Path.Combine(WorkingDir, "wwwroot", "jquery.js")));

            int result = command.Execute("jquery");

            Assert.AreEqual(0, result);
            Assert.IsTrue(File.Exists(Path.Combine(WorkingDir, "wwwroot", "jquery.min.js")));
            Assert.IsTrue(File.Exists(Path.Combine(WorkingDir, "wwwroot", "jquery.js")));

            string actualText = File.ReadAllText(libmanjsonPath);

            Assert.AreEqual(StringHelper.NormalizeNewLines(contents), StringHelper.NormalizeNewLines(actualText));

            var logger = HostEnvironment.Logger as TestLogger;

            Assert.AreEqual("The library 'jquery@3.3.1' is already up to date", logger.Messages.Last().Value);
        }

        [TestMethod]
        public void TestUpdateCommand_NotInstalled()
        {
            var command = new UpdateCommand(HostEnvironment);
            command.Configure(null);

            string contents = @"{
  ""version"": ""1.0"",
  ""defaultProvider"": ""cdnjs"",
  ""defaultDestination"": ""wwwroot"",
  ""libraries"": [
    {
      ""library"": ""jquery@3.3.1"",
      ""files"": [ ""jquery.min.js"", ""jquery.js"" ]
    }
  ]
}";

            string libmanjsonPath = Path.Combine(WorkingDir, "libman.json");
            File.WriteAllText(libmanjsonPath, contents);

            var restoreCommand = new RestoreCommand(HostEnvironment);
            restoreCommand.Configure(null);

            restoreCommand.Execute();

            int result = command.Execute("jqu");

            string actualText = File.ReadAllText(libmanjsonPath);

            Assert.AreEqual(StringHelper.NormalizeNewLines(contents), StringHelper.NormalizeNewLines(actualText));

            var logger = HostEnvironment.Logger as TestLogger;

            Assert.AreEqual("No library found with id 'jqu' to update", logger.Messages.Last().Value);
        }
    }
}