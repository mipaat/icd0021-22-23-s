﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.Resources.WebApp.ViewModels {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class VideoSearchViewModel {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal VideoSearchViewModel() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("App.Resources.WebApp.ViewModels.VideoSearchViewModel", typeof(VideoSearchViewModel).Assembly);
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
        
        public static string NameQueryName {
            get {
                return ResourceManager.GetString("NameQueryName", resourceCulture);
            }
        }
        
        public static string NameQueryPrompt {
            get {
                return ResourceManager.GetString("NameQueryPrompt", resourceCulture);
            }
        }
        
        public static string AuthorQueryName {
            get {
                return ResourceManager.GetString("AuthorQueryName", resourceCulture);
            }
        }
        
        public static string AuthorQueryPrompt {
            get {
                return ResourceManager.GetString("AuthorQueryPrompt", resourceCulture);
            }
        }
        
        public static string SortingOptionsName {
            get {
                return ResourceManager.GetString("SortingOptionsName", resourceCulture);
            }
        }
        
        public static string DescendingName {
            get {
                return ResourceManager.GetString("DescendingName", resourceCulture);
            }
        }
    }
}