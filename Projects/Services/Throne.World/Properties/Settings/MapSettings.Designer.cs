﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Throne.World.Properties.Settings {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class MapSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static MapSettings defaultInstance = ((MapSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new MapSettings())));
        
        public static MapSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public byte ElevationViolationsAllowed {
            get {
                return ((byte)(this["ElevationViolationsAllowed"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("18")]
        public int PlayerScreenRange {
            get {
                return ((int)(this["PlayerScreenRange"]));
            }
            set {
                this["PlayerScreenRange"] = value;
            }
        }
    }
}
