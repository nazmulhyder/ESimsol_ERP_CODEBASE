using System;
using System.IO;
using ICS.Base.Client.BOFoundation;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Base.Client.ServiceVessel;
using ICS.Base.Client.Utility;

namespace ESimSol.BusinessObjects
{
    #region PrintSetup
    [DataContract]
    public class PrintSetup : BusinessObject
    {
        public PrintSetup()
        {
            PrintSetupID = 0;
            BankID = 0;
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
            DateFormat = "dd MMM yyyy";
            IsSplit = false;
            Ischecked = false;
            PrinterGraceWidth = 0;


        }

        #region Properties

        #region PrintSetupID
        [DataMember]
        public int PrintSetupID { get; set; }
        #endregion
        #region BankID
        [DataMember]
        public int BankID { get; set; }
        #endregion
        #region Length
        [DataMember]
        public double Length { get; set; }
        #endregion
        #region Width
        [DataMember]
        public double Width { get; set; }

        #endregion


        #region PaymentMethodX
        [DataMember]
        public double PaymentMethodX { get; set; }

        #endregion
        #region paymentMethodY
        [DataMember]
        public double paymentMethodY { get; set; }
        #endregion
        #region paymentMethodL
        [DataMember]
        public double paymentMethodL { get; set; }
        #endregion
        #region paymentMethodW
        [DataMember]
        public double paymentMethodW { get; set; }
        #endregion
        #region paymentMethodFS
        [DataMember]
        public double paymentMethodFS { get; set; }
        #endregion

        #region DateX
        [DataMember]
        public double DateX { get; set; }
        #endregion
        #region DateY
        [DataMember]
        public double DateY { get; set; }
        #endregion
        #region DateL
        [DataMember]
        public double DateL { get; set; }
        #endregion
        #region DateW
        [DataMember]
        public double DateW { get; set; }
        #endregion
        #region DateFS
        [DataMember]
        public double DateFS { get; set; }
        #endregion

        #region PayToX
        [DataMember]
        public double PayToX { get; set; }
        #endregion
        #region PayToY
        [DataMember]
        public double PayToY { get; set; }
        #endregion
        #region PayToL
        [DataMember]
        public double PayToL { get; set; }
        #endregion
        #region PayToW
        [DataMember]
        public double PayToW { get; set; }
        #endregion
        #region PayToFS
        [DataMember]
        public double PayToFS { get; set; }
        #endregion

        #region AmountWordX
        [DataMember]
        public double AmountWordX { get; set; }
        #endregion
        #region AmountWordY
        [DataMember]
        public double AmountWordY { get; set; }
        #endregion
        #region AmountWordL
        [DataMember]
        public double AmountWordL { get; set; }
        #endregion
        #region AmountWordW
        [DataMember]
        public double AmountWordW { get; set; }
        #endregion
        #region AmountWordFS
        [DataMember]
        public double AmountWordFS { get; set; }
        #endregion

        #region AmountX
        [DataMember]
        public double AmountX { get; set; }
        #endregion
        #region AmountY
        [DataMember]
        public double AmountY { get; set; }
        #endregion
        #region AmountL
        [DataMember]
        public double AmountL { get; set; }
        #endregion
        #region AmountW
        [DataMember]
        public double AmountW { get; set; }
        #endregion
        #region AmountFS
        [DataMember]
        public double AmountFS { get; set; }
        #endregion

        #region token
        #region TBookNoX
        [DataMember]
        public double TBookNoX { get; set; }
        #endregion
        #region TBookNoY
        [DataMember]
        public double TBookNoY { get; set; }
        #endregion
        #region TBookNoL
        [DataMember]
        public double TBookNoL { get; set; }
        #endregion
        #region TBookNoW
        [DataMember]
        public double TBookNoW { get; set; }
        #endregion
        #region TBookNoFS
        [DataMember]
        public double TBookNoFS { get; set; }
        #endregion

        #region TPaymentTypeX
        [DataMember]
        public double TPaymentTypeX { get; set; }
        #endregion
        #region TPaymentTypeY
        [DataMember]
        public double TPaymentTypeY { get; set; }
        #endregion
        #region TPaymentTypeL
        [DataMember]
        public double TPaymentTypeL { get; set; }
        #endregion
        #region TPaymentTypeW
        [DataMember]
        public double TPaymentTypeW { get; set; }
        #endregion
        #region TPaymentTypeFS
        [DataMember]
        public double TPaymentTypeFS { get; set; }
        #endregion

        #region TDateX
        [DataMember]
        public double TDateX { get; set; }
        #endregion
        #region TDateY
        [DataMember]
        public double TDateY { get; set; }
        #endregion
        #region TDateL
        [DataMember]
        public double TDateL { get; set; }
        #endregion
        #region TDateW
        [DataMember]
        public double TDateW { get; set; }
        #endregion
        #region TDateFS
        [DataMember]
        public double TDateFS { get; set; }
        #endregion

        #region TPayToX
        [DataMember]
        public double TPayToX { get; set; }
        #endregion
        #region TPayToY
        [DataMember]
        public double TPayToY { get; set; }
        #endregion
        #region TPayToL
        [DataMember]
        public double TPayToL { get; set; }
        #endregion
        #region TPayToW
        [DataMember]
        public double TPayToW { get; set; }
        #endregion
        #region TPayToFS
        [DataMember]
        public double TPayToFS { get; set; }
        #endregion

        #region TForNoteX
        [DataMember]
        public double TForNoteX { get; set; }
        #endregion
        #region TForNoteY
        [DataMember]
        public double TForNoteY { get; set; }
        #endregion
        #region TForNoteL
        [DataMember]
        public double TForNoteL { get; set; }
        #endregion
        #region TForNoteW
        [DataMember]
        public double TForNoteW { get; set; }
        #endregion
        #region TForNoteFS
        [DataMember]
        public double TForNoteFS { get; set; }
        #endregion

        #region TAmountX
        [DataMember]
        public double TAmountX { get; set; }
        #endregion
        #region TAmountY
        [DataMember]
        public double TAmountY { get; set; }
        #endregion
        #region TAmountL
        [DataMember]
        public double TAmountL { get; set; }
        #endregion
        #region TAmountW
        [DataMember]
        public double TAmountW { get; set; }
        #endregion
        #region TAmountFS
        [DataMember]
        public double TAmountFS { get; set; }
        #endregion

        #region TVoucherNoX
        [DataMember]
        public double TVoucherNoX { get; set; }
        #endregion
        #region TVoucherNoY
        [DataMember]
        public double TVoucherNoY { get; set; }
        #endregion
        #region TVoucherNoL
        [DataMember]
        public double TVoucherNoL { get; set; }
        #endregion
        #region TVoucherNoW
        [DataMember]
        public double TVoucherNoW { get; set; }
        #endregion
        #region TVoucherNoFS
        [DataMember]
        public double TVoucherNoFS { get; set; }
        #endregion
        #endregion

        #region PrinterGraceWidth
        [DataMember]
        public double PrinterGraceWidth { get; set; }
        #endregion

        #region DateFormat
        [DataMember]
        public string DateFormat { get; set; }
        #endregion

        #region IsSplit
        [DataMember]
        public bool IsSplit { get; set; }
        #endregion
        #region Ischecked
        [DataMember]
        public bool Ischecked { get; set; }
        #endregion



        #endregion

        #region Derived Property


        #endregion


        #region Functions
        public static List<PrintSetup> Gets(Guid wcfSessionid)
        {
            return (List<PrintSetup>)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Gets")[0];
        }
        public static List<PrintSetup> GetsByBook(int nBookID, Guid wcfSessionid)
        {
            return (List<PrintSetup>)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "GetsByBook", nBookID)[0];
        }
        public static PrintSetup Get(int id, Guid wcfSessionid)
        {
            return (PrintSetup)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Get", id)[0];
        }

        public PrintSetup Save(Guid wcfSessionid)
        {
            return (PrintSetup)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "Save", this)[0];
        }

        public PrintSetup SaveForHistory(Guid wcfSessionid)
        {
            return (PrintSetup)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "SaveForHistory", this)[0];
        }


        #endregion

        #region ServiceFactory

        internal static Type ServiceType
        {
            get
            {
                return typeof(IPrintSetupService);
            }
        }
        #endregion
    }
    #endregion

    #region PrintSetups
    public class PrintSetups : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(PrintSetup item)
        {
            base.AddItem(item);
        }
        public void Remove(PrintSetup item)
        {
            base.RemoveItem(item);
        }
        public PrintSetup this[int index]
        {
            get { return (PrintSetup)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IPrintSetup interface
    [ServiceContract]
    public interface IPrintSetupService
    {
        [OperationContract]
        PrintSetup Get(int id, Int64 nUserID);
        [OperationContract]
        List<PrintSetup> GetsByBook(int nBookID, Int64 nUserID);
        [OperationContract]
        List<PrintSetup> Gets(Int64 nUserID);

        [OperationContract]
        PrintSetup Save(PrintSetup oPrintSetup, Int64 nUserID);
        [OperationContract]
        PrintSetup SaveForHistory(PrintSetup oPrintSetup, Int64 nUserID);
    }
    #endregion



}
