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
    public class CostCalculationDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CostCalculation oCostCalculation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CostCalculation]"
                                   + "%n,%s,%d,%d,%n,%n, %n,%n,%n,%n,%n,  %n,%n,%n,%n,%n,  %n,%n,%n,%n,%n,%n, %n,%n,%n,%n, %s,   %n,%n,%n,%n,%n,   %n,%n,%n,%n,%n,  %n,%n,%n,%n,%n,  %n,%n,%n,%n,%n",
                                    oCostCalculation.CostCalculationID, oCostCalculation.FileNo, oCostCalculation.DateOfIssue, oCostCalculation.DateOfExpire, oCostCalculation.VehicleModelID, oCostCalculation.CurrencyID,
                                    oCostCalculation.BasePrice, oCostCalculation.CRate, oCostCalculation.BasePriceBC, oCostCalculation.DutyPercent, oCostCalculation.DutyAmount,
                                    oCostCalculation.CustomAndInsurenceFeePercent, oCostCalculation.CustomAndInsurenceFeeAmount, oCostCalculation.TransportCost, oCostCalculation.LandedCost, oCostCalculation.LandedCostBC,
                                    oCostCalculation.MarginRate,
                                    oCostCalculation.AdditionalCost, oCostCalculation.AdditionalCostBC, oCostCalculation.TotalLandedCost, oCostCalculation.TotalLandedCostBC,
                                    oCostCalculation.ExShowroomPrice, oCostCalculation.ExShowroomPriceBC, oCostCalculation.MarginAmount, oCostCalculation.MarginAmountBC, oCostCalculation.OfferPriceBC,
                                    oCostCalculation.Remarks,
                                    oCostCalculation.CDPercent, oCostCalculation.CDAmount, oCostCalculation.RDPercent, oCostCalculation.RDAmount,
                                    oCostCalculation.SDPercent, oCostCalculation.SDAmount, oCostCalculation.VATPercent, oCostCalculation.VATAmount,
                                    oCostCalculation.AITPercent, oCostCalculation.AITAmount, oCostCalculation.ProfitForATVPercent, oCostCalculation.ProfitForATVAmount,
                                    oCostCalculation.TotalValueForATV, oCostCalculation.ATVPercent, oCostCalculation.ATVAmount, oCostCalculation.TotalDutyAmount, oCostCalculation.TotalDutyPercent, nUserID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, CostCalculation oCostCalculation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CostCalculation]"
                                   + "%n,%s,%d,%d,%n,%n, %n,%n,%n,%n,%n,  %n,%n,%n,%n,%n,  %n,%n,%n,%n,%n,%n, %n,%n,%n,%n, %s,   %n,%n,%n,%n,%n,   %n,%n,%n,%n,%n,  %n,%n,%n,%n,%n,  %n,%n,%n,%n,%n",
                                    oCostCalculation.CostCalculationID, oCostCalculation.FileNo, oCostCalculation.DateOfIssue, oCostCalculation.DateOfExpire, oCostCalculation.VehicleModelID, oCostCalculation.CurrencyID,
                                    oCostCalculation.BasePrice, oCostCalculation.CRate, oCostCalculation.BasePriceBC, oCostCalculation.DutyPercent, oCostCalculation.DutyAmount,
                                    oCostCalculation.CustomAndInsurenceFeePercent, oCostCalculation.CustomAndInsurenceFeeAmount, oCostCalculation.TransportCost, oCostCalculation.LandedCost, oCostCalculation.LandedCostBC,
                                    oCostCalculation.MarginRate,
                                    oCostCalculation.AdditionalCost, oCostCalculation.AdditionalCostBC, oCostCalculation.TotalLandedCost, oCostCalculation.TotalLandedCostBC,
                                    oCostCalculation.ExShowroomPrice, oCostCalculation.ExShowroomPriceBC, oCostCalculation.MarginAmount, oCostCalculation.MarginAmountBC, oCostCalculation.OfferPriceBC,
                                    oCostCalculation.Remarks,
                                    oCostCalculation.CDPercent, oCostCalculation.CDAmount, oCostCalculation.RDPercent, oCostCalculation.RDAmount,
                                    oCostCalculation.SDPercent, oCostCalculation.SDAmount, oCostCalculation.VATPercent, oCostCalculation.VATAmount,
                                    oCostCalculation.AITPercent, oCostCalculation.AITAmount, oCostCalculation.ProfitForATVPercent, oCostCalculation.ProfitForATVAmount,
                                    oCostCalculation.TotalValueForATV, oCostCalculation.ATVPercent, oCostCalculation.ATVAmount, oCostCalculation.TotalDutyAmount, oCostCalculation.TotalDutyPercent, nUserID, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CostCalculation WHERE CostCalculationID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
