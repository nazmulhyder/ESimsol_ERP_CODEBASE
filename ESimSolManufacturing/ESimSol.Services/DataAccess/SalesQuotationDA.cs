using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
	public class SalesQuotationDA 
	{
		#region Insert Update Delete Function
		public static IDataReader InsertUpdate(TransactionContext tc, SalesQuotation oSalesQuotation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_SalesQuotation]"
                                    + "%n,%s,%s,%n, %d,%n,%n,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%n,%n,%n,%n,%n",
                                    oSalesQuotation.SalesQuotationID, oSalesQuotation.FileNo, oSalesQuotation.RefNo, oSalesQuotation.BUID, oSalesQuotation.QuotationDate, oSalesQuotation.MarketingPerson, oSalesQuotation.QuotationTypeInInt, oSalesQuotation.BuyerID, oSalesQuotation.KommFileID, oSalesQuotation.VehicleModelID, oSalesQuotation.ExteriorColorID, oSalesQuotation.InteriorColorID, oSalesQuotation.VehicleChassisID, oSalesQuotation.VehicleEngineID, oSalesQuotation.UpholsteryID, oSalesQuotation.TrimID, oSalesQuotation.WheelsID, oSalesQuotation.Remarks, oSalesQuotation.FeatureSetupName, oSalesQuotation.CurrencyID, oSalesQuotation.DiscountAmount, oSalesQuotation.DiscountPercent, oSalesQuotation.OptionTotal, oSalesQuotation.UnitPrice, oSalesQuotation.VatAmount, oSalesQuotation.RegistrationFee, oSalesQuotation.OTRAmount, oSalesQuotation.Warranty, oSalesQuotation.DeliveryDate, oSalesQuotation.AdvancePayment, oSalesQuotation.PaymentTerm, oSalesQuotation.ValidityOfOffer, oSalesQuotation.AfterSalesService, oSalesQuotation.OfferValidity, oSalesQuotation.OrderSpecifications, oSalesQuotation.VehicleInspection, oSalesQuotation.CancelOrChangeOrder, oSalesQuotation.PaymentMode, oSalesQuotation.DeliveryDescription, oSalesQuotation.PriceFluctuationClause, oSalesQuotation.CustomsClearance, oSalesQuotation.Insurance, oSalesQuotation.ForceMajeure, oSalesQuotation.FuelQuality, oSalesQuotation.SpecialInstruction, oSalesQuotation.WarrantyTerms, oSalesQuotation.Acceptance, oSalesQuotation.VehicleSpecification, oSalesQuotation.Complementary, oSalesQuotation.NewOfferPrice, oSalesQuotation.DiscountPrice, oSalesQuotation.TDSAmount, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, SalesQuotation oSalesQuotation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_SalesQuotation]"
                                     + "%n,%s,%s,%n, %d,%n,%n,%n,%n,%n,%n, %n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%n,%n,%n,%n,%n",
                                    oSalesQuotation.SalesQuotationID, oSalesQuotation.FileNo, oSalesQuotation.RefNo, oSalesQuotation.BUID, oSalesQuotation.QuotationDate, oSalesQuotation.MarketingPerson, oSalesQuotation.QuotationTypeInInt, oSalesQuotation.BuyerID, oSalesQuotation.KommFileID, oSalesQuotation.VehicleModelID, oSalesQuotation.ExteriorColorID, oSalesQuotation.InteriorColorID, oSalesQuotation.VehicleChassisID, oSalesQuotation.VehicleEngineID, oSalesQuotation.UpholsteryID, oSalesQuotation.TrimID, oSalesQuotation.WheelsID, oSalesQuotation.Remarks, oSalesQuotation.FeatureSetupName, oSalesQuotation.CurrencyID, oSalesQuotation.DiscountAmount, oSalesQuotation.DiscountPercent, oSalesQuotation.OptionTotal, oSalesQuotation.UnitPrice, oSalesQuotation.VatAmount, oSalesQuotation.RegistrationFee, oSalesQuotation.OTRAmount, oSalesQuotation.Warranty, oSalesQuotation.DeliveryDate, oSalesQuotation.AdvancePayment, oSalesQuotation.PaymentTerm, oSalesQuotation.ValidityOfOffer, oSalesQuotation.AfterSalesService, oSalesQuotation.OfferValidity, oSalesQuotation.OrderSpecifications, oSalesQuotation.VehicleInspection, oSalesQuotation.CancelOrChangeOrder, oSalesQuotation.PaymentMode, oSalesQuotation.DeliveryDescription, oSalesQuotation.PriceFluctuationClause, oSalesQuotation.CustomsClearance, oSalesQuotation.Insurance, oSalesQuotation.ForceMajeure, oSalesQuotation.FuelQuality, oSalesQuotation.SpecialInstruction, oSalesQuotation.WarrantyTerms, oSalesQuotation.Acceptance, oSalesQuotation.VehicleSpecification, oSalesQuotation.Complementary, oSalesQuotation.NewOfferPrice, oSalesQuotation.DiscountPrice, oSalesQuotation.TDSAmount, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader UpdateStatus(TransactionContext tc, SalesQuotation oSalesQuotation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_UpdateSalesQutationStatus]"
                                   + "%n,%n,%s",
                                   oSalesQuotation.SalesQuotationID, oSalesQuotation.SalesStatus, oSalesQuotation.SalesStatusRemarks);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_SalesQuotation WHERE SalesQuotationID=%n", nID);
		}
        public static IDataReader GetLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesQuotationLog WHERE SalesQuotationLogID=%n", nID);
        }
        public static IDataReader BUWiseGets(int buid, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_SalesQuotation WHERE BUID = %n",buid);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}
        public static void UpdateBQ(TransactionContext tc, SalesQuotation oSalesQuotation)
        {
            string sSQL1 = SQLParser.MakeSQL("Update SalesQuotation Set PartyWiseBankID=%n, PrintOTRAmount=%n WHERE SalesQuotationID=%n", oSalesQuotation.PartyWiseBankID, oSalesQuotation.PrintOTRAmount, oSalesQuotation.SalesQuotationID);
            tc.ExecuteNonQuery(sSQL1);

        }
		#endregion
	}

}
