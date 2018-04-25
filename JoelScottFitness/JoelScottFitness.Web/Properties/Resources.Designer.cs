﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JoelScottFitness.Web.Properties {
    using System;
    
    
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("JoelScottFitness.Web.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to {0}_AFTER_{1}.
        /// </summary>
        internal static string AfterFileNameFormat {
            get {
                return ResourceManager.GetString("AfterFileNameFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to retrieve matching basket items from database for customer &apos;{0}&apos;, id&apos;s searched were &apos;{1}&apos;..
        /// </summary>
        internal static string BasketItemsAsyncNullErrorMessage {
            get {
                return ResourceManager.GetString("BasketItemsAsyncNullErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attempted checkout with null basket session for customer &apos;{0}&apos;..
        /// </summary>
        internal static string BasketItemsNullErrorMessage {
            get {
                return ResourceManager.GetString("BasketItemsNullErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}_BEFORE_{1}.
        /// </summary>
        internal static string BeforeFileNameFormat {
            get {
                return ResourceManager.GetString("BeforeFileNameFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}/Home/ConfirmOrder?.
        /// </summary>
        internal static string CallbackUri {
            get {
                return ResourceManager.GetString("CallbackUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Joel Scott Fitness - Confirm Account.
        /// </summary>
        internal static string ConfirmAccount {
            get {
                return ResourceManager.GetString("ConfirmAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The confirmOrderViewModel was null.
        /// </summary>
        internal static string ConfirmOrderViewModelNullErrorMessage {
            get {
                return ResourceManager.GetString("ConfirmOrderViewModelNullErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to create guest account details for customer with emaill address &apos;{0}&apos;..
        /// </summary>
        internal static string CreateGuestDetailsFailedErrorMessage {
            get {
                return ResourceManager.GetString("CreateGuestDetailsFailedErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ooops, we cannot find your customer id, please contact customer services if this continues..
        /// </summary>
        internal static string CustomerIdNullErrorMessage {
            get {
                return ResourceManager.GetString("CustomerIdNullErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1.
        /// </summary>
        internal static string DefaultItemQuantity {
            get {
                return ResourceManager.GetString("DefaultItemQuantity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to add image &apos;{0}&apos; to database..
        /// </summary>
        internal static string FailedToAddImageToDatabaseErrorMessage {
            get {
                return ResourceManager.GetString("FailedToAddImageToDatabaseErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to add item with id {0} to basket..
        /// </summary>
        internal static string FailedToAddItemToBasketErrorMessage {
            get {
                return ResourceManager.GetString("FailedToAddItemToBasketErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to associate plan &apos;{0}&apos; to purchase with id &apos;{1}&apos; for customer with id &apos;{2}&apos;..
        /// </summary>
        internal static string FailedToAssociatePlanToPurchaseErrorMessage {
            get {
                return ResourceManager.GetString("FailedToAssociatePlanToPurchaseErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to complete paypal payment, error details &apos;{0}&apos;..
        /// </summary>
        internal static string FailedToCompletePayPalPaymentErrorMessage {
            get {
                return ResourceManager.GetString("FailedToCompletePayPalPaymentErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to create message for Name: &apos;{0}&apos;, Email Address &apos;{1}&apos;, Subject &apos;{2}&apos;, Message &apos;{3}&apos;..
        /// </summary>
        internal static string FailedToCreateMessageErrorMessage {
            get {
                return ResourceManager.GetString("FailedToCreateMessageErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to create or update questionnaire for purchase with id &apos;{0}&apos;..
        /// </summary>
        internal static string FailedToCreateOrUpdateQuestionnaireErrorMessage {
            get {
                return ResourceManager.GetString("FailedToCreateOrUpdateQuestionnaireErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to delete image with id &apos;{0}&apos;..
        /// </summary>
        internal static string FailedToDeleteImageErrorMessage {
            get {
                return ResourceManager.GetString("FailedToDeleteImageErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to find customer with id &apos;{0}&apos;..
        /// </summary>
        internal static string FailedToFindCustomerErrorMessage {
            get {
                return ResourceManager.GetString("FailedToFindCustomerErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to find item with id {0}..
        /// </summary>
        internal static string FailedToFindItemErrorMessage {
            get {
                return ResourceManager.GetString("FailedToFindItemErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to retrieve order with id &apos;{0}&apos; associated to customer with id &apos;{1}&apos;..
        /// </summary>
        internal static string FailedToFindOrderErrorMessageErrorMessage {
            get {
                return ResourceManager.GetString("FailedToFindOrderErrorMessageErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to find user with account name &apos;{0}&apos;;.
        /// </summary>
        internal static string FailedToFindUserErrorMessage {
            get {
                return ResourceManager.GetString("FailedToFindUserErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to initiate paypal payment, error details &apos;{0}&apos;..
        /// </summary>
        internal static string FailedToInitiatePayPalPaymentErrorMessage {
            get {
                return ResourceManager.GetString("FailedToInitiatePayPalPaymentErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to retrieve order &apos;{0}&apos; associated to transaction with id &apos;{1}&apos;..
        /// </summary>
        internal static string FailedToRetrieveOrderErrorMessage {
            get {
                return ResourceManager.GetString("FailedToRetrieveOrderErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to retrieve plans from repository..
        /// </summary>
        internal static string FailedToRetrievePlansErrorMessage {
            get {
                return ResourceManager.GetString("FailedToRetrievePlansErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to save items pending purchase for customer &apos;{0}&apos;..
        /// </summary>
        internal static string FailedToSaveItemsForPurchase {
            get {
                return ResourceManager.GetString("FailedToSaveItemsForPurchase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to send message for Name: &apos;{0}&apos;, Email Address &apos;{1}&apos;, Subject &apos;{2}&apos;, Message &apos;{3}&apos;..
        /// </summary>
        internal static string FailedToSendMessageErrorMessage {
            get {
                return ResourceManager.GetString("FailedToSendMessageErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to send order complete email for purchase with id &apos;{0}&apos; associated to customer with id &apos;{1}&apos;..
        /// </summary>
        internal static string FailedToSendOrderCompleteEmailErrorMessage {
            get {
                return ResourceManager.GetString("FailedToSendOrderCompleteEmailErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to send order confirmation email for transaction with id &apos;{0}&apos;..
        /// </summary>
        internal static string FailedToSendOrderConfirmationEmailErrorMessage {
            get {
                return ResourceManager.GetString("FailedToSendOrderConfirmationEmailErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to send order received email for transaction with id &apos;{0}&apos;..
        /// </summary>
        internal static string FailedToSendOrderReceivedEmailErrorMessage {
            get {
                return ResourceManager.GetString("FailedToSendOrderReceivedEmailErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to send questionnaire complete email for order #{0}..
        /// </summary>
        internal static string FailedToSendQuestionnaireCompleteEmailErrorMessage {
            get {
                return ResourceManager.GetString("FailedToSendQuestionnaireCompleteEmailErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to update existing customer details for email address &apos;{0}&apos;;.
        /// </summary>
        internal static string FailedToUpdateExistingCustomerDetailsErrorMessage {
            get {
                return ResourceManager.GetString("FailedToUpdateExistingCustomerDetailsErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to update order status of transaction with id &apos;{0}&apos;..
        /// </summary>
        internal static string FailedToUpdateOrderStatusErrorMessage {
            get {
                return ResourceManager.GetString("FailedToUpdateOrderStatusErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to upload &apos;{0}&apos;..
        /// </summary>
        internal static string FailedToUploadFileErrorMessage {
            get {
                return ResourceManager.GetString("FailedToUploadFileErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to upload hall of fame associated to purchase item id &apos;{0}&apos;..
        /// </summary>
        internal static string FailedToUploadHallOfFameErrorMessage {
            get {
                return ResourceManager.GetString("FailedToUploadHallOfFameErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to upload hall of fame images associated to purchased item id &apos;{0}&apos;..
        /// </summary>
        internal static string FailedToUploadHallOfFameImagesErrorMessage {
            get {
                return ResourceManager.GetString("FailedToUploadHallOfFameImagesErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to upload plan associated to purchase id &apos;{0}&apos; for customer with id &apos;{1}&apos;..
        /// </summary>
        internal static string FailedToUploadPlanForCustomerErrorMessage {
            get {
                return ResourceManager.GetString("FailedToUploadPlanForCustomerErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Oops! something went wrong please try again later..
        /// </summary>
        internal static string GenericErrorMessage {
            get {
                return ResourceManager.GetString("GenericErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to retrieve customer details for customer with id &apos;{0}&apos;;.
        /// </summary>
        internal static string GetCustomerDetailsAsyncErrorMessage {
            get {
                return ResourceManager.GetString("GetCustomerDetailsAsyncErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} image is empty, please re-select..
        /// </summary>
        internal static string ImageUploadErrorMessage {
            get {
                return ResourceManager.GetString("ImageUploadErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to JoelScottFitness MailingList {0}.txt.
        /// </summary>
        internal static string MailingListFilename {
            get {
                return ResourceManager.GetString("MailingListFilename", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Re: {0}.
        /// </summary>
        internal static string MessageResponseSubject {
            get {
                return ResourceManager.GetString("MessageResponseSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please add a comment..
        /// </summary>
        internal static string MissingCommentErrorMessage {
            get {
                return ResourceManager.GetString("MissingCommentErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New Customer Query.
        /// </summary>
        internal static string NewCustomerQuery {
            get {
                return ResourceManager.GetString("NewCustomerQuery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Joel Scott Fitness - Order #{0} Complete.
        /// </summary>
        internal static string OrderComplete {
            get {
                return ResourceManager.GetString("OrderComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Joel Scott Fitness - Order #{0} Confirmation.
        /// </summary>
        internal static string OrderConfirmation {
            get {
                return ResourceManager.GetString("OrderConfirmation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Order with id &apos;{0}&apos; was not found..
        /// </summary>
        internal static string OrderNotFoundErrorMessage {
            get {
                return ResourceManager.GetString("OrderNotFoundErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Joel Scott Fitness - Order #{0} Received.
        /// </summary>
        internal static string OrderReceived {
            get {
                return ResourceManager.GetString("OrderReceived", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Payer id null.
        /// </summary>
        internal static string PayerIdNullErrorMessage {
            get {
                return ResourceManager.GetString("PayerIdNullErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more payment completion parameters were null or empty.
        /// </summary>
        internal static string PaymentCompletionErrorMessage {
            get {
                return ResourceManager.GetString("PaymentCompletionErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Payment completion parameter &apos;{0}&apos; is null or empty, cannot complete transaction..
        /// </summary>
        internal static string PaymentCompletionParameterNullErrorMessage {
            get {
                return ResourceManager.GetString("PaymentCompletionParameterNullErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} {1} - {2} - {3} - #{4} - {5}.pdf.
        /// </summary>
        internal static string PlanFilenameFormat {
            get {
                return ResourceManager.GetString("PlanFilenameFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Questionnaire for Order#{0} Complete.
        /// </summary>
        internal static string QuestionnaireComplete {
            get {
                return ResourceManager.GetString("QuestionnaireComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thanks, your tailored workout plan will be with you in the next 24 hours..
        /// </summary>
        internal static string QuestionnaireCompleteConfirmationMessage {
            get {
                return ResourceManager.GetString("QuestionnaireCompleteConfirmationMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Joel Scott Fitness - Reset Password.
        /// </summary>
        internal static string ResetPassword {
            get {
                return ResourceManager.GetString("ResetPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to find existing customer details for email address &apos;{0}&apos;;.
        /// </summary>
        internal static string UnableToFindExistingCustomerErrorMessage {
            get {
                return ResourceManager.GetString("UnableToFindExistingCustomerErrorMessage", resourceCulture);
            }
        }
    }
}
