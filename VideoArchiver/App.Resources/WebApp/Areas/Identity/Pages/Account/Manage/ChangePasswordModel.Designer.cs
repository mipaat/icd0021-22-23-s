﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.Resources.WebApp.Areas.Identity.Pages.Account.Manage {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ChangePasswordModel {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ChangePasswordModel() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("App.Resources.WebApp.Areas.Identity.Pages.Account.Manage.ChangePasswordModel", typeof(ChangePasswordModel).Assembly);
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
        
        public static string OldPassword {
            get {
                return ResourceManager.GetString("OldPassword", resourceCulture);
            }
        }
        
        public static string OldPasswordPrompt {
            get {
                return ResourceManager.GetString("OldPasswordPrompt", resourceCulture);
            }
        }
        
        public static string NewPassword {
            get {
                return ResourceManager.GetString("NewPassword", resourceCulture);
            }
        }
        
        public static string NewPasswordPrompt {
            get {
                return ResourceManager.GetString("NewPasswordPrompt", resourceCulture);
            }
        }
        
        public static string ConfirmPassword {
            get {
                return ResourceManager.GetString("ConfirmPassword", resourceCulture);
            }
        }
        
        public static string ConfirmPasswordPrompt {
            get {
                return ResourceManager.GetString("ConfirmPasswordPrompt", resourceCulture);
            }
        }
        
        public static string ComparePasswordErrorMessage {
            get {
                return ResourceManager.GetString("ComparePasswordErrorMessage", resourceCulture);
            }
        }
        
        public static string YourPasswordHasBeenChanged {
            get {
                return ResourceManager.GetString("YourPasswordHasBeenChanged", resourceCulture);
            }
        }
    }
}
