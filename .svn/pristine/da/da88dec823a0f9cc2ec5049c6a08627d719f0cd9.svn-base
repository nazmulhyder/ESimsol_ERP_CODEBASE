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
	public class ServiceOrderDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, ServiceOrder oServiceOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_ServiceOrder]"
                                    + "%n,%s,%n,%n,%n,%n,%n,%s,%d,%d,%D,%D,%s,%n,%n,%D,%D,%n,%d,%s,%s,%s,%s,%s,%s,%s,%s,%s,%n,%n,%n,%n,%n,%n,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%n,%b,%n,%n,%n",
									oServiceOrder.ServiceOrderID,oServiceOrder.ServiceOrderNo, oServiceOrder.ServiceOrderType,oServiceOrder.VehicleRegistrationID,oServiceOrder.AdvisorID,oServiceOrder.CustomerID,oServiceOrder.ContactPersonID,oServiceOrder.KilometerReading,oServiceOrder.ServiceOrderDate,oServiceOrder.IssueDate,oServiceOrder.RcvDateTime,oServiceOrder.DelDateTime,oServiceOrder.Remarks,oServiceOrder.OrderStatus,oServiceOrder.ApproveByID,oServiceOrder.ActualRcvDateTime,oServiceOrder.ActualDelDateTime,oServiceOrder.LastUpdateBy,oServiceOrder.LastUpdateDateTime,oServiceOrder.MobilityService,oServiceOrder.IPNo,oServiceOrder.IPExpDate,oServiceOrder.SoldByDealer,oServiceOrder.NoShowStatus,oServiceOrder.ReasonOfVisit,oServiceOrder.ExtendedWarranty,oServiceOrder.ServicePlan,oServiceOrder.RSAPolicyNo,oServiceOrder.FuelStatus,oServiceOrder.NoOfKeys,oServiceOrder.ENetAmount,oServiceOrder.ELCAmount,oServiceOrder.EPartsAmount,oServiceOrder.ModeOfPayment,oServiceOrder.IsTaxesApplicable,oServiceOrder.IsWindows,oServiceOrder.IsWiperBlades,oServiceOrder.IsLIghts,oServiceOrder.IsExhaustSys,oServiceOrder.IsUnderbody,oServiceOrder.IsEngineComp,oServiceOrder.IsWashing,oServiceOrder.IsOilLevel,oServiceOrder.IsCoolant,oServiceOrder.IsWindWasher,oServiceOrder.IsBreakes,oServiceOrder.IsAxle,oServiceOrder.IsMonograms,oServiceOrder.IsPolishing,oServiceOrder.IsOwnersManual,oServiceOrder.IsScheManual,oServiceOrder.IsNavManual,oServiceOrder.IsWBook,oServiceOrder.IsRefGuide,oServiceOrder.IsSpareWheel,oServiceOrder.IsToolKits,oServiceOrder.IsFloorMats,oServiceOrder.IsMudFlaps,oServiceOrder.IsWarningT,oServiceOrder.IsFirstAidKit,oServiceOrder.NoOfCDs,oServiceOrder.IsOtherLoose,oServiceOrder.CurrencyID, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, ServiceOrder oServiceOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_ServiceOrder]"
                                    + "%n,%s,%n,%n,%n,%n,%n,%s,%d,%d,%D,%D,%s,%n,%n,%D,%D,%n,%d,%s,%s,%s,%s,%s,%s,%s,%s,%s,%n,%n,%n,%n,%n,%n,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%n,%b,%n,%n,%n",
                                    oServiceOrder.ServiceOrderID, oServiceOrder.ServiceOrderNo, oServiceOrder.ServiceOrderType, oServiceOrder.VehicleRegistrationID, oServiceOrder.AdvisorID, oServiceOrder.CustomerID, oServiceOrder.ContactPersonID, oServiceOrder.KilometerReading, oServiceOrder.ServiceOrderDate, oServiceOrder.IssueDate, oServiceOrder.RcvDateTime, oServiceOrder.DelDateTime, oServiceOrder.Remarks, oServiceOrder.OrderStatus, oServiceOrder.ApproveByID, oServiceOrder.ActualRcvDateTime, oServiceOrder.ActualDelDateTime, oServiceOrder.LastUpdateBy, oServiceOrder.LastUpdateDateTime, oServiceOrder.MobilityService, oServiceOrder.IPNo, oServiceOrder.IPExpDate, oServiceOrder.SoldByDealer, oServiceOrder.NoShowStatus, oServiceOrder.ReasonOfVisit, oServiceOrder.ExtendedWarranty, oServiceOrder.ServicePlan, oServiceOrder.RSAPolicyNo, oServiceOrder.FuelStatus, oServiceOrder.NoOfKeys, oServiceOrder.ENetAmount, oServiceOrder.ELCAmount, oServiceOrder.EPartsAmount, oServiceOrder.ModeOfPayment, oServiceOrder.IsTaxesApplicable, oServiceOrder.IsWindows, oServiceOrder.IsWiperBlades, oServiceOrder.IsLIghts, oServiceOrder.IsExhaustSys, oServiceOrder.IsUnderbody, oServiceOrder.IsEngineComp, oServiceOrder.IsWashing, oServiceOrder.IsOilLevel, oServiceOrder.IsCoolant, oServiceOrder.IsWindWasher, oServiceOrder.IsBreakes, oServiceOrder.IsAxle, oServiceOrder.IsMonograms, oServiceOrder.IsPolishing, oServiceOrder.IsOwnersManual, oServiceOrder.IsScheManual, oServiceOrder.IsNavManual, oServiceOrder.IsWBook, oServiceOrder.IsRefGuide, oServiceOrder.IsSpareWheel, oServiceOrder.IsToolKits, oServiceOrder.IsFloorMats, oServiceOrder.IsMudFlaps, oServiceOrder.IsWarningT, oServiceOrder.IsFirstAidKit, oServiceOrder.NoOfCDs, oServiceOrder.IsOtherLoose,oServiceOrder.CurrencyID, nUserID, (int)eEnumDBOperation);
        }
        public static void UpdateStatus(TransactionContext tc, ServiceOrder oServiceOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ServiceOrder]"
                                   + "%n,%s,%n,%n,%n,%n,%n,%s,%d,%d,%D,%D,%s,%n,%n,%D,%D,%n,%d,%s,%s,%s,%s,%s,%s,%s,%s,%s,%n,%n,%n,%n,%n,%n,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%b,%n,%b,%n,%n,%n",
                                    oServiceOrder.ServiceOrderID, oServiceOrder.ServiceOrderNo, oServiceOrder.ServiceOrderType, oServiceOrder.VehicleRegistrationID, oServiceOrder.AdvisorID, oServiceOrder.CustomerID, oServiceOrder.ContactPersonID, oServiceOrder.KilometerReading, oServiceOrder.ServiceOrderDate, oServiceOrder.IssueDate, oServiceOrder.RcvDateTime, oServiceOrder.DelDateTime, oServiceOrder.Remarks, oServiceOrder.OrderStatus, oServiceOrder.ApproveByID, oServiceOrder.ActualRcvDateTime, oServiceOrder.ActualDelDateTime, oServiceOrder.LastUpdateBy, oServiceOrder.LastUpdateDateTime, oServiceOrder.MobilityService, oServiceOrder.IPNo, oServiceOrder.IPExpDate, oServiceOrder.SoldByDealer, oServiceOrder.NoShowStatus, oServiceOrder.ReasonOfVisit, oServiceOrder.ExtendedWarranty, oServiceOrder.ServicePlan, oServiceOrder.RSAPolicyNo, oServiceOrder.FuelStatus, oServiceOrder.NoOfKeys, oServiceOrder.ENetAmount, oServiceOrder.ELCAmount, oServiceOrder.EPartsAmount, oServiceOrder.ModeOfPayment, oServiceOrder.IsTaxesApplicable, oServiceOrder.IsWindows, oServiceOrder.IsWiperBlades, oServiceOrder.IsLIghts, oServiceOrder.IsExhaustSys, oServiceOrder.IsUnderbody, oServiceOrder.IsEngineComp, oServiceOrder.IsWashing, oServiceOrder.IsOilLevel, oServiceOrder.IsCoolant, oServiceOrder.IsWindWasher, oServiceOrder.IsBreakes, oServiceOrder.IsAxle, oServiceOrder.IsMonograms, oServiceOrder.IsPolishing, oServiceOrder.IsOwnersManual, oServiceOrder.IsScheManual, oServiceOrder.IsNavManual, oServiceOrder.IsWBook, oServiceOrder.IsRefGuide, oServiceOrder.IsSpareWheel, oServiceOrder.IsToolKits, oServiceOrder.IsFloorMats, oServiceOrder.IsMudFlaps, oServiceOrder.IsWarningT, oServiceOrder.IsFirstAidKit, oServiceOrder.NoOfCDs, oServiceOrder.IsOtherLoose, oServiceOrder.CurrencyID, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_ServiceOrder WHERE ServiceOrderID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_ServiceOrder ORDER BY ServiceOrderDate DESC");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
