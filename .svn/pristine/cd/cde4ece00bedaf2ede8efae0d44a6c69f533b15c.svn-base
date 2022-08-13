using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.DataAccess
{
    public class ExportDocSetupDA
    {
        public ExportDocSetupDA() { }

        #region Insert Function

        public static IDataReader InsertUpdate(TransactionContext tc, ExportDocSetup oExportDocSetup, EnumDBOperation eEnumDBPurchaseLC, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportDocSetup]"
                                    + " %n,%n,%n,%b,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%s,%s,%s,%s,%b,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%b,%b,%n, %s,%b,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s, %s, %s, %b,%s,%s,%s,%s,%s,%s,%s,%s,%n,%n,%n, %s,%s,%s,%s,%n,%n,%s,%n,%s,%n,%s,%s,%n,%n,%n,%n, %n,%n",
             oExportDocSetup.ExportDocSetupID
            , oExportDocSetup.CompanyID
            , (int)oExportDocSetup.DocumentTypeInt
            , oExportDocSetup.IsPrintHeader
            , oExportDocSetup.DocName
            , oExportDocSetup.DocHeader
            , oExportDocSetup.Beneficiary
            , oExportDocSetup.BillNo
            , oExportDocSetup.NoAndDateOfDoc
            , oExportDocSetup.ProformaInvoiceNoAndDate
            , oExportDocSetup.AccountOf
            , oExportDocSetup.DocumentaryCreditNoDate
            , oExportDocSetup.AgainstExportLC
            , oExportDocSetup.PortofLoading
            , oExportDocSetup.FinalDestination
            , oExportDocSetup.IssuingBank
            , oExportDocSetup.NegotiatingBank
            , oExportDocSetup.CountryofOrigin
            , oExportDocSetup.TermsofPayment
            , oExportDocSetup.AmountInWord
            , oExportDocSetup.Wecertifythat
            , oExportDocSetup.Certification
            , oExportDocSetup.IsPrintOriginal
            , oExportDocSetup.IsPrintGrossNetWeight
            , oExportDocSetup.IsPrintDeliveryBy
            , oExportDocSetup.IsPrintTerm
            , oExportDocSetup.IsPrintQty
            , oExportDocSetup.IsPrintUnitPrice
            , oExportDocSetup.IsPrintValue
            , oExportDocSetup.IsPrintWeight
            , oExportDocSetup.IsPrintFrieghtPrepaid
            , oExportDocSetup.IsShowAmendmentNo
            , oExportDocSetup.ClauseOne
            , oExportDocSetup.ClauseTwo
            , oExportDocSetup.ClauseThree
            , oExportDocSetup.ClauseFour
            , oExportDocSetup.Activity
            , oExportDocSetup.Carrier
            , oExportDocSetup.Account
            , oExportDocSetup.NotifyParty
            , oExportDocSetup.Remarks
            , oExportDocSetup.IRC
            , oExportDocSetup.GarmentsQty
            , oExportDocSetup.HSCode
            , oExportDocSetup.AreaCode
            , oExportDocSetup.SpecialNote
            , oExportDocSetup.Remark
            , oExportDocSetup.IsVat
            , oExportDocSetup.IsRegistration
            , oExportDocSetup.BUID
            , oExportDocSetup.DeliveryTo
            , oExportDocSetup.IsPrintInvoiceDate
           , oExportDocSetup.AuthorisedSignature
           , oExportDocSetup.ReceiverSignature
           , oExportDocSetup.For
           , oExportDocSetup.MUnitName
           , oExportDocSetup.NetWeightName
           , oExportDocSetup.GrossWeightName
           , oExportDocSetup.Bag_Name
           , oExportDocSetup.CountryofOriginName
           , oExportDocSetup.SellingOnAbout
           , oExportDocSetup.PortofLoadingName
           , oExportDocSetup.FinalDestinationName
           , oExportDocSetup.TO
           , oExportDocSetup.TruckNo_Print
           , oExportDocSetup.Driver_Print
          , oExportDocSetup.ShippingMark
          , oExportDocSetup.ReceiverCluse
          , oExportDocSetup.ForCaptionInDubleLine
          , oExportDocSetup.CarrierName
          , oExportDocSetup.DescriptionOfGoods
          , oExportDocSetup.MarkSAndNos
          , oExportDocSetup.QtyInOne
          , oExportDocSetup.QtyInTwo
          , oExportDocSetup.ValueName
          , oExportDocSetup.UPName
          , oExportDocSetup.NoOfBag
          , oExportDocSetup.FontSize_Normal
          , oExportDocSetup.FontSize_Bold
          , oExportDocSetup.FontSize_ULine
          , oExportDocSetup.ChallanNo
          , oExportDocSetup.CTPApplicant
          , oExportDocSetup.GRPNoDate
          , oExportDocSetup.ASPERPI
          , (EnumExportGoodsDesViewType)oExportDocSetup.GoodsDesViewType
          , oExportDocSetup.PrintOn
          , oExportDocSetup.ToTheOrderOf
          , oExportDocSetup.OrderOfBankTypeInInt
          , oExportDocSetup.TermsOfShipment
          , oExportDocSetup.ProductPrintType
          , oExportDocSetup.TextWithGoodsCol
          , oExportDocSetup.TextWithGoodsRow
          , oExportDocSetup.BagCount
          , oExportDocSetup.GrossWeightPTage
          , oExportDocSetup.WeightPBag
          , (int)oExportDocSetup.ExportLCType
          , nUserId
          , (int)eEnumDBPurchaseLC);
        }

        public static IDataReader InsertUpdate_Bill(TransactionContext tc, ExportDocSetup oExportDocSetup, EnumDBOperation eEnumDBPurchaseLC, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBillDoc]"
                                    + "%n, %n,%n, %n, %b,%s,%s,%s,%s, %s, %s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s, %s,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%n,%s,%b,%s,%s,%s,%s, %s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%b,%s,%s,%s,%s,%s,%s,%s,%s,%s, %s,%s,%s, %s ,%s ,%n ,%n ,%n, %s, %s,%s, %d ,%n,%n,%n,%s,%n, %n,%n,%n",
             oExportDocSetup.ExportBillDocID
             , oExportDocSetup.ExportDocSetupID
            , oExportDocSetup.CompanyID
            , (int)oExportDocSetup.DocumentTypeInt
            , oExportDocSetup.IsPrintHeader
            , oExportDocSetup.DocName
            , oExportDocSetup.DocHeader
            , oExportDocSetup.Beneficiary
            , oExportDocSetup.BillNo
            , oExportDocSetup.NoAndDateOfDoc
            , oExportDocSetup.ProformaInvoiceNoAndDate
            , oExportDocSetup.AccountOf
            , oExportDocSetup.DocumentaryCreditNoDate
            , oExportDocSetup.AgainstExportLC
            , oExportDocSetup.PortofLoading
            , oExportDocSetup.FinalDestination
            , oExportDocSetup.IssuingBank
            , oExportDocSetup.NegotiatingBank
            , oExportDocSetup.CountryofOrigin
            , oExportDocSetup.TermsofPayment
            , oExportDocSetup.AmountInWord
            , oExportDocSetup.Wecertifythat
            , oExportDocSetup.Certification
            , oExportDocSetup.IsPrintOriginal
            , oExportDocSetup.IsPrintGrossNetWeight
            , oExportDocSetup.IsPrintDeliveryBy
            , oExportDocSetup.IsPrintTerm
            , oExportDocSetup.IsPrintQty
            , oExportDocSetup.IsPrintUnitPrice
            , oExportDocSetup.IsPrintValue
            , oExportDocSetup.IsPrintWeight
            , oExportDocSetup.IsPrintFrieghtPrepaid
            , oExportDocSetup.IsShowAmendmentNo
            , oExportDocSetup.ClauseOne
            , oExportDocSetup.ClauseTwo
            , oExportDocSetup.ClauseThree
            , oExportDocSetup.ClauseFour
            , oExportDocSetup.Carrier
            , oExportDocSetup.Account
            , oExportDocSetup.NotifyParty
            , oExportDocSetup.Remarks
            , oExportDocSetup.IRC
            , oExportDocSetup.GarmentsQty
            , oExportDocSetup.HSCode
            , oExportDocSetup.AreaCode
            , oExportDocSetup.SpecialNote
            , oExportDocSetup.Remark
            , oExportDocSetup.BUID
            , oExportDocSetup.DeliveryTo
            , oExportDocSetup.IsPrintInvoiceDate
           , oExportDocSetup.AuthorisedSignature
           , oExportDocSetup.ReceiverSignature
           , oExportDocSetup.For
           , oExportDocSetup.MUnitName
           , oExportDocSetup.NetWeightName
           , oExportDocSetup.GrossWeightName
           , oExportDocSetup.Bag_Name
           , oExportDocSetup.CountryofOriginName
           , oExportDocSetup.SellingOnAbout
           , oExportDocSetup.PortofLoadingName
           , oExportDocSetup.FinalDestinationName
           , oExportDocSetup.TO
           , oExportDocSetup.TruckNo_Print
           , oExportDocSetup.Driver_Print
           , oExportDocSetup.ShippingMark
           , oExportDocSetup.ReceiverCluse
           , oExportDocSetup.ForCaptionInDubleLine
           , oExportDocSetup.CarrierName
           , oExportDocSetup.DescriptionOfGoods
           , oExportDocSetup.MarkSAndNos
           , oExportDocSetup.QtyInOne
           , oExportDocSetup.QtyInTwo
           , oExportDocSetup.ValueName
           , oExportDocSetup.UPName
           , oExportDocSetup.NoOfBag
          , oExportDocSetup.ChallanNo
          , oExportDocSetup.CTPApplicant
          , oExportDocSetup.GRPNoDate
          , oExportDocSetup.ASPERPI
          , oExportDocSetup.DeliveryBy
          , oExportDocSetup.FoBTerm
          , oExportDocSetup.PrintOn
          , oExportDocSetup.BagCount
          , oExportDocSetup.WeightPBag
          , oExportDocSetup.TruckNo
          , oExportDocSetup.DriverName
          , oExportDocSetup.TRNo
          , NullHandler.GetNullValue(oExportDocSetup.TRDate)
          , (EnumNotifyBy)oExportDocSetup.NotifyBy
          , (EnumExportGoodsDesViewType)oExportDocSetup.GoodsDesViewType
          , oExportDocSetup.ExportBillID
          , oExportDocSetup.ToTheOrderOf
          , oExportDocSetup.OrderOfBankTypeInInt
          , oExportDocSetup.GrossWeightPTage
          , nUserId
          , (int)eEnumDBPurchaseLC);

        }
        public static void UpdateSequence(TransactionContext tc, ExportDocSetup oExportDocSetup)
        {
            tc.ExecuteNonQuery("UPDATE ExportDocSetup SET  Sequence=%n WHERE ExportDocSetupID=%n", oExportDocSetup.Sequence, oExportDocSetup.ExportDocSetupID);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, ExportDocSetup oExportDocSetup, EnumDBOperation eEnumDBPurchaseLC, Int64 nUserId)
        {
        
             tc.ExecuteNonQuery("EXEC [SP_IUD_ExportDocSetup]"
                                    + " %n,%n,%n,%b,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%s,%s,%s,%s,%b,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%b,%b,%n, %s,%b,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s, %s, %s, %b,%s,%s,%s,%s,%s,%s,%s,%s,%n,%n,%n, %s,%s,%s,%s,%n,%n,%s,%n,%s,%n,%s,%s,%n,%n,%n,%n, %n,%n",
             oExportDocSetup.ExportDocSetupID
            , oExportDocSetup.CompanyID
            , (int)oExportDocSetup.DocumentTypeInt
            , oExportDocSetup.IsPrintHeader
            , oExportDocSetup.DocName
            , oExportDocSetup.DocHeader
            , oExportDocSetup.Beneficiary
            , oExportDocSetup.BillNo
            , oExportDocSetup.NoAndDateOfDoc
            , oExportDocSetup.ProformaInvoiceNoAndDate
            , oExportDocSetup.AccountOf
            , oExportDocSetup.DocumentaryCreditNoDate
            , oExportDocSetup.AgainstExportLC
            , oExportDocSetup.PortofLoading
            , oExportDocSetup.FinalDestination
            , oExportDocSetup.IssuingBank
            , oExportDocSetup.NegotiatingBank
            , oExportDocSetup.CountryofOrigin
            , oExportDocSetup.TermsofPayment
            , oExportDocSetup.AmountInWord
            , oExportDocSetup.Wecertifythat
            , oExportDocSetup.Certification
            , oExportDocSetup.IsPrintOriginal
            , oExportDocSetup.IsPrintGrossNetWeight
            , oExportDocSetup.IsPrintDeliveryBy
            , oExportDocSetup.IsPrintTerm
            , oExportDocSetup.IsPrintQty
            , oExportDocSetup.IsPrintUnitPrice
            , oExportDocSetup.IsPrintValue
            , oExportDocSetup.IsPrintWeight
            , oExportDocSetup.IsPrintFrieghtPrepaid
            , oExportDocSetup.IsShowAmendmentNo
            , oExportDocSetup.ClauseOne
            , oExportDocSetup.ClauseTwo
            , oExportDocSetup.ClauseThree
            , oExportDocSetup.ClauseFour
            , oExportDocSetup.Activity
            , oExportDocSetup.Carrier
            , oExportDocSetup.Account
            , oExportDocSetup.NotifyParty
            , oExportDocSetup.Remarks
            , oExportDocSetup.IRC
            , oExportDocSetup.GarmentsQty
            , oExportDocSetup.HSCode
            , oExportDocSetup.AreaCode
            , oExportDocSetup.SpecialNote
            , oExportDocSetup.Remark
            , oExportDocSetup.IsVat
            , oExportDocSetup.IsRegistration
            , oExportDocSetup.BUID
            , oExportDocSetup.DeliveryTo
            , oExportDocSetup.IsPrintInvoiceDate
           , oExportDocSetup.AuthorisedSignature
           , oExportDocSetup.ReceiverSignature
           , oExportDocSetup.For
           , oExportDocSetup.MUnitName
           , oExportDocSetup.NetWeightName
           , oExportDocSetup.GrossWeightName
           , oExportDocSetup.Bag_Name
           , oExportDocSetup.CountryofOriginName
           , oExportDocSetup.SellingOnAbout
           , oExportDocSetup.PortofLoadingName
           , oExportDocSetup.FinalDestinationName
           , oExportDocSetup.TO
           , oExportDocSetup.TruckNo_Print
           , oExportDocSetup.Driver_Print
          , oExportDocSetup.ShippingMark
          , oExportDocSetup.ReceiverCluse
          , oExportDocSetup.ForCaptionInDubleLine
          , oExportDocSetup.CarrierName
          , oExportDocSetup.DescriptionOfGoods
          , oExportDocSetup.MarkSAndNos
          , oExportDocSetup.QtyInOne
          , oExportDocSetup.QtyInTwo
          , oExportDocSetup.ValueName
          , oExportDocSetup.UPName
          , oExportDocSetup.NoOfBag
          , oExportDocSetup.FontSize_Normal
          , oExportDocSetup.FontSize_Bold
          , oExportDocSetup.FontSize_ULine
          , oExportDocSetup.ChallanNo
          , oExportDocSetup.CTPApplicant
          , oExportDocSetup.GRPNoDate
          , oExportDocSetup.ASPERPI
          , (EnumExportGoodsDesViewType)oExportDocSetup.GoodsDesViewType
          , oExportDocSetup.PrintOn
          , oExportDocSetup.ToTheOrderOf
          , oExportDocSetup.OrderOfBankTypeInInt
          , oExportDocSetup.TermsOfShipment
          , oExportDocSetup.ProductPrintType
          , oExportDocSetup.TextWithGoodsCol
          , oExportDocSetup.TextWithGoodsRow
          , oExportDocSetup.BagCount
          , oExportDocSetup.GrossWeightPTage
          , oExportDocSetup.WeightPBag
          , (int)oExportDocSetup.ExportLCType
          , nUserId
          , (int)eEnumDBPurchaseLC);
        }
        #endregion

        #region Generation Function

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportDocSetup WHERE ExportDocSetupID=%n ORDER BY Sequence ASC", nID);
        }
        public static IDataReader Gets(TransactionContext tc, bool bActivity, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportDocSetup WHERE Activity=%b AND BUID = %n ORDER BY Sequence ASC", bActivity, nBUID);
        }
        public static IDataReader GetsByType(TransactionContext tc, int nExportLCType, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportDocSetup WHERE ExportLCType=%n  AND BUID = %n and Activity=1 ORDER BY Sequence ASC", nExportLCType, nBUID);
        }
        public static IDataReader BUWiseGets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportDocSetup WHERE BUID=%n ORDER BY Sequence ASC", nBUID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nExportBillID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillDoc WHERE ExportBillID=%n ORDER BY Sequence ASC", nExportBillID);
        }
        public static IDataReader GetBy(TransactionContext tc, int nExportDocSetupID, int nExportBillID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillDoc Where ExportDocSetupID=%n and ExportBillID=%n", nExportDocSetupID, nExportBillID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportDocSetup ORDER BY Sequence ASC");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Activate(TransactionContext tc, ExportDocSetup oExportDocSetup)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ExportDocSetup Set Activity=~Activity WHERE ExportDocSetupID=%n", oExportDocSetup.ExportDocSetupID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportDocSetup WHERE ExportDocSetupID=%n", oExportDocSetup.ExportDocSetupID);

        }
        public static int GetExportBillDocID(TransactionContext tc, int nExportDocSetupID, int nExportBillID)
        {
            object obj = tc.ExecuteScalar("SELECT ExportBillDocID FROM ExportBillDoc Where ExportDocSetupID=%n and ExportBillID=%n", nExportDocSetupID, nExportBillID);
            if (obj == null) return 0;
            return Convert.ToInt32(obj);
        }
        #endregion
    }


}
