﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.34014
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BuZzDownloader.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        /// <summary>
        /// BuZz&apos;s URL
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("BuZz\'s URL")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://buzz.3on.fr/")]
        public string BuZzUrl {
            get {
                return ((string)(this["BuZzUrl"]));
            }
        }
        
        /// <summary>
        /// User login
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("User login")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public string BuZzLogin {
            get {
                return ((string)(this["BuZzLogin"]));
            }
            set {
                this["BuZzLogin"] = value;
            }
        }
        
        /// <summary>
        /// User password
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("User password")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public string BuZzPwd {
            get {
                return ((string)(this["BuZzPwd"]));
            }
            set {
                this["BuZzPwd"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("./Downloads")]
        public string DownloadFolder {
            get {
                return ((string)(this["DownloadFolder"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int StatusFrequency {
            get {
                return ((int)(this["StatusFrequency"]));
            }
        }
    }
}
