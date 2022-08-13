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
	public class FARegisterDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FARegister FARegister, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FARegister]"
                                    + " %n,%n,%n,%n,  %s,%n,%n,%n,%n,%n,  %n,%n,%n,%n,   %d,%d,%s,%s,	%s,%s,%s,%s,%s	,%s,%n,%d,%d,%s"
                                    + ", %s,%s,%s     ,%s,%s,%s,%s		, %s,%s,%s,%s   ,%s,%s,%s,%s   ,%s,%s,  %s,%s,%s,%s,%s,%s,	%s,%s,%s, %n,%n,%n",
                                    FARegister.FARegisterID, FARegister.BUID, FARegister.LocationID, FARegister.ProductID,
                                    FARegister.FACodeFull, FARegister.FAMethod, FARegister.DEPCalculateOn, FARegister.ActualSalvage, FARegister.UseFullLife,
                                    FARegister.ActualDepRate,FARegister.CurrencyID, FARegister.Qty, FARegister.MUnitID, FARegister.ActualCostPrice, FARegister.PurchaseDate,
                                    FARegister.DepStartDate, FARegister.Note, FARegister.Model,FARegister.BrandName, FARegister.Manufacturer, FARegister.ManufacturerYear,
                                    FARegister.ProductSLNo, FARegister.CountryOfOrigin, FARegister.BasicFunction, FARegister.WarrantyPeriod,FARegister.WarrantyDate, 
                                    FARegister.WarrantyDate_Exp, FARegister.PowerConumption, FARegister.Capacity, FARegister.TechnicalSpec, FARegister.PerformanceSpec,
                                    FARegister.PortOfShipment, FARegister.LCNoWithDate, FARegister.HSCode, FARegister.AssestNote,
                                    FARegister.SupplierName, FARegister.SupplierAddress, FARegister.SupplierEmail, FARegister.SupplierPhone, 
                                    FARegister.SupplierFax, FARegister.SupplierCPName, FARegister.SupplierCPPhone, FARegister.SupplierCPEmail, FARegister.SupplierNote,
                                    FARegister.AgentName, FARegister.AgentAddress, FARegister.AgentEmail, FARegister.AgentPhone, 
                                    FARegister.AgentFax, FARegister.AgentCPName, FARegister.AgentCPPhone, FARegister.AgentCPEmail, 
                                    FARegister.AgentWorkStation, FARegister.AgentNote, FARegister.FAStatus,
                                    nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, FARegister FARegister, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FARegister]"
                         + " %n,%n,%n,%n,  %s,%n,%n,%n,%n,%n,  %n,%n,%n,%n,   %d,%d,%s,%s,	%s,%s,%s,%s,%s	,%s,%n,%d,%d,%s"
                                    + ", %s,%s,%s     ,%s,%s,%s,%s		, %s,%s,%s,%s   ,%s,%s,%s,%s   ,%s,%s,  %s,%s,%s,%s,%s,%s,	%s,%s,%s, %n,%n,%n",
                                    FARegister.FARegisterID, FARegister.BUID, FARegister.LocationID, FARegister.ProductID,
                                    FARegister.FACodeFull, FARegister.FAMethod, FARegister.DEPCalculateOn, FARegister.ActualSalvage, FARegister.UseFullLife,
                                    FARegister.ActualDepRate, FARegister.CurrencyID, FARegister.Qty, FARegister.MUnitID, FARegister.ActualCostPrice, FARegister.PurchaseDate,
                                    FARegister.DepStartDate, FARegister.Note, FARegister.Model, FARegister.BrandName, FARegister.Manufacturer, FARegister.ManufacturerYear,
                                    FARegister.ProductSLNo, FARegister.CountryOfOrigin, FARegister.BasicFunction, FARegister.WarrantyPeriod, FARegister.WarrantyDate,
                                    FARegister.WarrantyDate_Exp, FARegister.PowerConumption, FARegister.Capacity, FARegister.TechnicalSpec, FARegister.PerformanceSpec,
                                    FARegister.PortOfShipment, FARegister.LCNoWithDate, FARegister.HSCode, FARegister.AssestNote,
                                    FARegister.SupplierName, FARegister.SupplierAddress, FARegister.SupplierEmail, FARegister.SupplierPhone,
                                    FARegister.SupplierFax, FARegister.SupplierCPName, FARegister.SupplierCPPhone, FARegister.SupplierCPEmail, FARegister.SupplierNote,
                                    FARegister.AgentName, FARegister.AgentAddress, FARegister.AgentEmail, FARegister.AgentPhone,
                                    FARegister.AgentFax, FARegister.AgentCPName, FARegister.AgentCPPhone, FARegister.AgentCPEmail,
                                    FARegister.AgentWorkStation, FARegister.AgentNote, FARegister.FAStatus,
                                    nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader FARProcess(TransactionContext tc, FARegister oFARegister, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_FARegister]"
                                   + "%n,%n,%n,%n,%n,  %n,%n,%b,%b,%n,  %n,%n",
                                   oFARegister.FARegisterID, oFARegister.FAMethodInt, //oFARegister.CalculateOnInt, oFARegister.SavageValue, oFARegister.UseFullLifeMonth, 
                                   //oFARegister.UseFullLifeYear, oFARegister.Percentage, oFARegister.IsFirst, oFARegister.IsValued, 
                                   oFARegister.CurrencyID, nUserID, (int)eEnumDBOperation);
        }
		#endregion

		#region Get & Exist Function
        public static IDataReader GetFACode(TransactionContext tc, FARegister oFARegister)
        {
            return tc.ExecuteReader("select dbo.fn_GetFACode(" + oFARegister.ProductID + "," + oFARegister.BUID + "," + oFARegister.LocationID + ",'" + oFARegister.PurchaseDateInString + "') as FACodeFull");
        }
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FARegister WHERE FARegisterID=%n", nID);
		}
        public static IDataReader GetLogByLogID(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FARegisterLog WHERE FARegisterLogID=%n", nID);
        }
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FARegister");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion

        public static IDataReader FA_Process(TransactionContext tc, FARegister oFARegister, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FARegister_Process] %n", nUserID);
        }
        public static IDataReader FAGRNProcess(TransactionContext tc, GRNDetail oGRNDetail, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FAGRN_Process] %n, %n", oGRNDetail.GRNDetailID, nUserID);
        }
    }

}
