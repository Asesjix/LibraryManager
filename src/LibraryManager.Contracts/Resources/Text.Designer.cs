﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Web.LibraryManager.Contracts.Resources {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Text {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Text() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Web.LibraryManager.Contracts.Resources.Text", typeof(Text).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Conflicting libraries found: {0}.
        /// </summary>
        internal static string ErrorConflictingLibraries {
            get {
                return ResourceManager.GetString("ErrorConflictingLibraries", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} could not be written to disk. Make sure the file name is correct.
        /// </summary>
        internal static string ErrorCouldNotWriteFile {
            get {
                return ResourceManager.GetString("ErrorCouldNotWriteFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;{0}&quot; library is already installed by the &quot;{1}&quot; provider.
        /// </summary>
        internal static string ErrorLibraryAlreadyInstalled {
            get {
                return ResourceManager.GetString("ErrorLibraryAlreadyInstalled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The library &quot;{0}&quot; cannot be installed as it conflicts with &quot;{1}&quot;.
        ///Please specify a different destination..
        /// </summary>
        internal static string ErrorLibraryCannotInstallDueToConflicts {
            get {
                return ResourceManager.GetString("ErrorLibraryCannotInstallDueToConflicts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The library &quot;{0}&quot; could not be updated to &quot;{1}&quot;. Another library &quot;{1}&quot; is already installed.
        /// </summary>
        internal static string ErrorLibraryCannotUpdateDueToConflicts {
            get {
                return ResourceManager.GetString("ErrorLibraryCannotUpdateDueToConflicts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The library &quot;{0}&quot; could not be updated to &quot;{1}&quot;. The following files are not valid for the &quot;{1}&quot;: {2}.
        /// </summary>
        internal static string ErrorLibraryCannotUpdateDueToFileConflicts {
            get {
                return ResourceManager.GetString("ErrorLibraryCannotUpdateDueToFileConflicts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The library id is undefined.
        /// </summary>
        internal static string ErrorLibraryIdIsUndefined {
            get {
                return ResourceManager.GetString("ErrorLibraryIdIsUndefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The manifest file contains syntax errors.
        /// </summary>
        internal static string ErrorManifestMalformed {
            get {
                return ResourceManager.GetString("ErrorManifestMalformed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Version &quot;{0}&quot; is not supported by this version of Library Manager.
        /// </summary>
        internal static string ErrorNotSupportedVersion {
            get {
                return ResourceManager.GetString("ErrorNotSupportedVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;destination&quot; is undefined..
        /// </summary>
        internal static string ErrorPathIsUndefined {
            get {
                return ResourceManager.GetString("ErrorPathIsUndefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;destination&quot; must be inside the working directory.
        /// </summary>
        internal static string ErrorPathOutsideWorkingDirectory {
            get {
                return ResourceManager.GetString("ErrorPathOutsideWorkingDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The provider is undefined.
        /// </summary>
        internal static string ErrorProviderIsUndefined {
            get {
                return ResourceManager.GetString("ErrorProviderIsUndefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;{0}&quot; provider could not be found.
        /// </summary>
        internal static string ErrorProviderUnknown {
            get {
                return ResourceManager.GetString("ErrorProviderUnknown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;{0}&quot; file path could not be resolved by the &quot;{1}&quot; provider.
        /// </summary>
        internal static string ErrorUnableToResolveFilePath {
            get {
                return ResourceManager.GetString("ErrorUnableToResolveFilePath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;{0}&quot; library could not be resolved by the &quot;{1}&quot; provider.
        /// </summary>
        internal static string ErrorUnableToResolveSource {
            get {
                return ResourceManager.GetString("ErrorUnableToResolveSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unknown exception occurred.
        /// </summary>
        internal static string ErrorUnknownException {
            get {
                return ResourceManager.GetString("ErrorUnknownException", resourceCulture);
            }
        }
    }
}
