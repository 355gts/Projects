﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JoelScottFitness.PayPal.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Failed to create payment for transaction \'{0}\' associated to customer \'{1}\'.")]
        public string PayPalFailedToCreatePaymentForTransactionErrorMessage {
            get {
                return ((string)(this["PayPalFailedToCreatePaymentForTransactionErrorMessage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("An error occured initiating paypal payment for customer \'{0}\', error details - \'{" +
            "1}\'.")]
        public string PayPalExceptionOccuredCreatingPaymentErrorMessage {
            get {
                return ((string)(this["PayPalExceptionOccuredCreatingPaymentErrorMessage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Payment associated to paymentId \'{0}\' and payerId \'{1}\' failed, paypal response r" +
            "eturned payment state of \'{2}\'.")]
        public string PayPalPaymentFailedErrorMessage {
            get {
                return ((string)(this["PayPalPaymentFailedErrorMessage"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("An exception occured processing paypal payment for paymentId \'{0}\' and payerId \'{" +
            "1}\', error details \'{2}\'.")]
        public string PayPalPaymentExceptionErrorMessage {
            get {
                return ((string)(this["PayPalPaymentExceptionErrorMessage"]));
            }
            set {
                this["PayPalPaymentExceptionErrorMessage"] = value;
            }
        }
    }
}
