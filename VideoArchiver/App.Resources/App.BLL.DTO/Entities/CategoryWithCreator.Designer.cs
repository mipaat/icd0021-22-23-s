﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.Resources.App.BLL.DTO.Entities {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CategoryWithCreator {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CategoryWithCreator() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("App.Resources.App.BLL.DTO.Entities.CategoryWithCreator", typeof(CategoryWithCreator).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string Name {
            get {
                return ResourceManager.GetString("Name", resourceCulture);
            }
        }
        
        public static string IsPublic {
            get {
                return ResourceManager.GetString("IsPublic", resourceCulture);
            }
        }
        
        public static string IsAssignable {
            get {
                return ResourceManager.GetString("IsAssignable", resourceCulture);
            }
        }
        
        public static string Platform {
            get {
                return ResourceManager.GetString("Platform", resourceCulture);
            }
        }
        
        public static string IdOnPlatform {
            get {
                return ResourceManager.GetString("IdOnPlatform", resourceCulture);
            }
        }
        
        public static string Creator {
            get {
                return ResourceManager.GetString("Creator", resourceCulture);
            }
        }
    }
}
