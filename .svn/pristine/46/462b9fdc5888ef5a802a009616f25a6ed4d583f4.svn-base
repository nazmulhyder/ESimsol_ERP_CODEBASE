using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ChequeSetup
    
    public class ChequeSetup : BusinessObject
    {
        public ChequeSetup()
        {
            ChequeSetupID = 0;
            ChequeSetupName = "";

            Length = 0;
            Width = 0;

            PaymentMethodX = 0;
            paymentMethodY = 0;
            paymentMethodL = 0;
            paymentMethodW = 0;
            paymentMethodFS = 0;

            DateX = 0;
            DateY = 0;
            DateL = 0;
            DateW = 0;
            DateFS = 0;

            PayToX = 0;
            PayToY = 0;
            PayToL = 0;
            PayToW = 0;
            PayToFS = 0;

            AmountWordX = 0;
            AmountWordY = 0;
            AmountWordL = 0;
            AmountWordW = 0;
            AmountWordFS = 0;

            AmountX = 0;
            AmountY = 0;
            AmountL = 0;
            AmountW = 0;
            AmountFS = 0;

            TBookNoX = 0;
            TBookNoY = 0;
            TBookNoL = 0;
            TBookNoW = 0;
            TBookNoFS = 0;

            TPaymentTypeX = 0;
            TPaymentTypeY = 0;
            TPaymentTypeL = 0;
            TPaymentTypeW = 0;
            TPaymentTypeFS = 0;

            TDateX = 0;
            TDateY = 0;
            TDateL = 0;
            TDateW = 0;
            TDateFS = 0;

            TPayToX = 0;
            TPayToY = 0;
            TPayToL = 0;
            TPayToW = 0;
            TPayToFS = 0;

            TForNoteX = 0;
            TForNoteY = 0;
            TForNoteL = 0;
            TForNoteW = 0;
            TForNoteFS = 0;

            TAmountX = 0;
            TAmountY = 0;
            TAmountL = 0;
            TAmountW = 0;
            TAmountFS = 0;

            TVoucherNoX = 0;
            TVoucherNoY = 0;
            TVoucherNoL = 0;
            TVoucherNoW = 0;
            TVoucherNoFS = 0;

            DateFormat = "";
            IsSplit = false;
            DateSpace = 0;
            Ischecked = false;
            PrinterGraceWidth = 0;
            ChequeImageInByte = null;
            ChequeImageSize = 0;

            
            ErrorMessage = "";
            IsSubmitted = false;
        }

        #region Properties        
        public int ChequeSetupID { get; set; }        
        public string ChequeSetupName { get; set; }        
        public double Length { get; set; }        
        public double Width { get; set; }        
        public double PaymentMethodX { get; set; }        
        public double paymentMethodY { get; set; }        
        public double paymentMethodL { get; set; }        
        public double paymentMethodW { get; set; }        
        public double paymentMethodFS { get; set; }        
        public double DateX { get; set; }        
        public double DateY { get; set; }        
        public double DateL { get; set; }        
        public double DateW { get; set; }        
        public double DateFS { get; set; }        
        public double PayToX { get; set; }        
        public double PayToY { get; set; }        
        public double PayToL { get; set; }        
        public double PayToW { get; set; }        
        public double PayToFS { get; set; }        
        public double AmountWordX { get; set; }        
        public double AmountWordY { get; set; }        
        public double AmountWordL { get; set; }        
        public double AmountWordW { get; set; }        
        public double AmountWordFS { get; set; }        
        public double AmountX { get; set; }        
        public double AmountY { get; set; }        
        public double AmountL { get; set; }        
        public double AmountW { get; set; }        
        public double AmountFS { get; set; }        
        public double TBookNoX { get; set; }        
        public double TBookNoY { get; set; }        
        public double TBookNoL { get; set; }        
        public double TBookNoW { get; set; }        
        public double TBookNoFS { get; set; }        
        public double TPaymentTypeX { get; set; }        
        public double TPaymentTypeY { get; set; }        
        public double TPaymentTypeL { get; set; }        
        public double TPaymentTypeW { get; set; }        
        public double TPaymentTypeFS { get; set; }        
        public double TDateX { get; set; }
        public double TDateY { get; set; }
        public double TDateL { get; set; }
        public double TDateW { get; set; }
        public double TDateFS { get; set; }        
        public double TPayToX { get; set; }        
        public double TPayToY { get; set; }        
        public double TPayToL { get; set; }        
        public double TPayToW { get; set; }        
        public double TPayToFS { get; set; }        
        public double TForNoteX { get; set; }        
        public double TForNoteY { get; set; }        
        public double TForNoteL { get; set; }        
        public double TForNoteW { get; set; }        
        public double TForNoteFS { get; set; }        
        public double TAmountX { get; set; }        
        public double TAmountY { get; set; }        
        public double TAmountL { get; set; }        
        public double TAmountW { get; set; }        
        public double TAmountFS { get; set; }        
        public double TVoucherNoX { get; set; }        
        public double TVoucherNoY { get; set; }        
        public double TVoucherNoL { get; set; }        
        public double TVoucherNoW { get; set; }        
        public double TVoucherNoFS { get; set; }        
        public string DateFormat { get; set; }        
        public bool IsSplit { get; set; }
        public double DateSpace { get; set; }        
        public bool Ischecked { get; set; }        
        public double PrinterGraceWidth { get; set; }        
        public byte[] ChequeImageInByte { get; set; }       
        public int ChequeImageSize { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSubmitted { get; set; }
        #endregion

        #region Derive Property
        

        public string IsSplitInString { get { return this.IsSplit ? "Split" : "Not Split"; } }
        public string IscheckedInString { get { return this.Ischecked ? "Checked" : "Not Checked"; } }
        public string IsSubmittedInString { get { return this.IsSubmitted ? "Submitted" : "Not Submitted"; } }
        public int IsSplitInInt { get { return this.IsSplit ? 1 : 0; } }
        public int IscheckedInInt { get { return this.Ischecked ? 1 : 0; } }
        public int IsSubmittedInInt { get { return this.IsSubmitted ? 1 : 0; } }
        #endregion

        #region Functions
        public static List<ChequeSetup> GetsWithoutImage(int nCurrentUserID)
        {
            return ChequeSetup.Service.GetsWithoutImage(nCurrentUserID);
        }
        public static List<ChequeSetup> Gets(int nCurrentUserID)
        {
            return ChequeSetup.Service.Gets(nCurrentUserID);
        }
        public ChequeSetup Get(int id, int nCurrentUserID)
        {
            return ChequeSetup.Service.Get(id, nCurrentUserID);
        }
        public ChequeSetup Save(int nCurrentUserID)
        {
            return ChequeSetup.Service.Save(this, nCurrentUserID);
        }
        public string Delete(int id, int nCurrentUserID)
        {
            return ChequeSetup.Service.Delete(id, nCurrentUserID);
        }
        public static List<ChequeSetup> Gets(string sSQL, int nCurrentUserID)
        {
            return ChequeSetup.Service.Gets(sSQL, nCurrentUserID);
        }
        public static List<ChequeSetup> GetsByName(string sName, int nCurrentUserID)
        {
            return ChequeSetup.Service.GetsByName(sName, nCurrentUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IChequeSetupService Service
        {
            get { return (IChequeSetupService)Services.Factory.CreateService(typeof(IChequeSetupService)); }
        }
        #endregion
    }
    #endregion

    #region IChequeSetup interface
    
    public interface IChequeSetupService
    {

        List<ChequeSetup> GetsWithoutImage(int nCurrentUserID);
        List<ChequeSetup> GetsByName(string sName, int nCurrentUserID);
        
        ChequeSetup Get(int id, int nCurrentUserID);
        
        List<ChequeSetup> Gets(int nCurrentUserID);
        
        List<ChequeSetup> Gets(string sSQL, int nCurrentUserID);
        
        string Delete(int id, int nCurrentUserID);
        
        ChequeSetup Save(ChequeSetup oChequeSetup, int nCurrentUserID);
    }
    #endregion
}