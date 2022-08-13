using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportPIDA
    {
        public ExportPIDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportPI oExportPI, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPI]" + "%n, %n, %s, %n, %d, %d, %n, %n, %n, %n, %n, %n, %n, %n, %b, %b, %n, %n, %d, %d, %s, %n, %d, %n, %n, %n, %s, %s, %s, %n, %n, %n, %n, %n, %n, %s, %n, %n,%s, %n, %n",
                                    oExportPI.ExportPIID,  oExportPI.PaymentTypeInInt, oExportPI.PINo, oExportPI.PIStatusInInt, oExportPI.IssueDate, oExportPI.ValidityDate, oExportPI.ContractorID, oExportPI.BuyerID, oExportPI.MKTEmpID, oExportPI.BankBranchID, oExportPI.BankAccountID, oExportPI.CurrencyID, oExportPI.Qty, oExportPI.Amount, oExportPI.IsLIBORRate, oExportPI.IsBBankFDD, oExportPI.LCTermID, oExportPI.OverdueRate, NullHandler.GetNullValue(oExportPI.LCOpenDate), NullHandler.GetNullValue(oExportPI.DeliveryDate), oExportPI.Note, oExportPI.ApprovedBy, NullHandler.GetNullValue(oExportPI.ApprovedDate), oExportPI.LCID, oExportPI.ExportPIPrintSetupID, oExportPI.ShipmentTermInInt, oExportPI.ColorInfo, oExportPI.DepthOfShade, oExportPI.YarnCount, oExportPI.DeliveryToID, oExportPI.BUID, (int)oExportPI.PIType,oExportPI.ContractorContactPerson, oExportPI.BuyerContactPerson,  oExportPI.RateUnit,oExportPI.NoteTwo,(int)oExportPI.ProductNature, oExportPI.OrderSheetID,oExportPI.SampleInvoiceIDs,  (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, ExportPI oExportPI, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportPI]" + "%n, %n, %s, %n, %d, %d, %n, %n, %n, %n, %n, %n, %n, %n, %b, %b, %n, %n, %d, %d, %s, %n, %d, %n, %n, %n, %s, %s, %s, %n, %n, %n, %n, %n, %n, %s, %n, %n,%s, %n, %n",
                                    oExportPI.ExportPIID, oExportPI.PaymentTypeInInt, oExportPI.PINo, oExportPI.PIStatusInInt, oExportPI.IssueDate, oExportPI.ValidityDate, oExportPI.ContractorID, oExportPI.BuyerID, oExportPI.MKTEmpID, oExportPI.BankBranchID, oExportPI.BankAccountID, oExportPI.CurrencyID, oExportPI.Qty, oExportPI.Amount, oExportPI.IsLIBORRate, oExportPI.IsBBankFDD, oExportPI.LCTermID, oExportPI.OverdueRate, NullHandler.GetNullValue(oExportPI.LCOpenDate), NullHandler.GetNullValue(oExportPI.DeliveryDate), oExportPI.Note, oExportPI.ApprovedBy, NullHandler.GetNullValue(oExportPI.ApprovedDate), oExportPI.LCID, oExportPI.ExportPIPrintSetupID, oExportPI.ShipmentTermInInt, oExportPI.ColorInfo, oExportPI.DepthOfShade, oExportPI.YarnCount, oExportPI.DeliveryToID, oExportPI.BUID, (int)oExportPI.PIType, oExportPI.ContractorContactPerson, oExportPI.BuyerContactPerson, oExportPI.RateUnit, oExportPI.NoteTwo, (int)oExportPI.ProductNature, oExportPI.OrderSheetID, oExportPI.SampleInvoiceIDs, (int)eEnumDBOperation, nUserID);
        }
        public static IDataReader UpdatePIStatus(TransactionContext tc, ExportPI oExportPI, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_ExportPIStatusUpdate]" + "%n, %n, %n", oExportPI.ExportPIID, oExportPI.ExportPIActionInInt, nUserID);
        }
        public static IDataReader UpdatePaymentType(TransactionContext tc, ExportPI oExportPI, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPI_NonLC]" + "%n, %n, %n,%n", oExportPI.ExportPIID, oExportPI.PaymentTypeInInt, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader UpdatePINo(TransactionContext tc, ExportPI oExportPI, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPI_NoChange]" + "%n, %n, %s,%n,%n", oExportPI.ExportPIID, oExportPI.PaymentTypeInInt, oExportPI.PINo, oExportPI.OrderSheetID, nUserID);
        }
        public static IDataReader Approve(TransactionContext tc, ExportPI oExportPI, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            //return tc.ExecuteReader("EXEC [SP_IUD_ExportPISignatory]" + "%n, %n, %s,%n,%n", oExportPI.ExportPIID, eEnumDBOperation, oExportPI.PINo, oExportPI.OrderSheetID, nUserID);
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPISignatory]"
                                   + "%n,%n,%n,%n,%n,%n,%b,%n,%n",
                                   0, oExportPI.ExportPIID, 0, 0, oExportPI.ReviseNo, 0, 0, nUserID, (int)eEnumDBOperation);
        }
        public static void UpdatePIInfo(TransactionContext tc, ExportPI oExportPI)
        {
            tc.ExecuteNonQuery("Update ExportPI SET ConversionRate = %n, SCRemarks = %s, BankChargeInfo = %s, BankCharge = %n, MasterContactNo = %s, PartyName = %s, PartyAddress = %s  WHERE ExportPIID = %n", oExportPI.ConversionRate, oExportPI.SCRemarks, oExportPI.BankChargeInfo, oExportPI.BankCharge, oExportPI.MasterContactNo, oExportPI.PartyName, oExportPI.PartyAddress, oExportPI.ExportPIID);
        }
        public static IDataReader Copy(TransactionContext tc, ExportPI oExportPI, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CopyExportPI]" + "%n, %n", oExportPI.ExportPIID, nUserID);
        }
        public static IDataReader PISWAP(TransactionContext tc, ExportPI oExportPI, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [sp_IUD_ExportPISwap]" + "%n, %n", oExportPI.ExportPIID, oExportPI.ExportPILogID, nUserID);
        }
        
        #endregion

        #region Accept Export PI Revise
        public static IDataReader AcceptExportPIRevise(TransactionContext tc, ExportPI oExportPI, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPILog]" + "%n,  %n, %s, %n, %d, %d, %n, %n, %n, %n, %n, %n, %n, %b, %b, %n, %n, %d, %d, %s, %n, %d, %n, %n, %b, %n, %n, %n, %n, %n, %s, %n, %n, %s, %s, %s, %n, %n, %n",
                                    oExportPI.ExportPIID, 
                                    oExportPI.PaymentTypeInInt,
                                    oExportPI.PINo, 
                                    oExportPI.PIStatusInInt, 
                                    oExportPI.IssueDate, 
                                    oExportPI.ValidityDate, 
                                    oExportPI.ContractorID, 
                                    oExportPI.BuyerID, 
                                    oExportPI.MKTEmpID, 
                                    oExportPI.BankBranchID, 
                                    oExportPI.CurrencyID, 
                                    oExportPI.Qty, 
                                    oExportPI.Amount, 
                                    oExportPI.IsLIBORRate, 
                                    oExportPI.IsBBankFDD, 
                                    oExportPI.LCTermID, 
                                    oExportPI.OverdueRate, 
                                    NullHandler.GetNullValue(oExportPI.LCOpenDate), 
                                    NullHandler.GetNullValue(oExportPI.DeliveryDate), 
                                    oExportPI.Note, 
                                    oExportPI.ApprovedBy, 
                                    NullHandler.GetNullValue(oExportPI.ApprovedDate), 
                                    oExportPI.LCID, 
                                    oExportPI.ExportPIPrintSetupID,
                                    oExportPI.IsCreateReviseNo,
                                    oExportPI.BUID,
                                    oExportPI.PIType,
                                    oExportPI.ContractorContactPerson,
                                    oExportPI.BuyerContactPerson,
                                    oExportPI.RateUnit,
                                     oExportPI.NoteTwo,
                                     oExportPI.BankAccountID,
                                     oExportPI.ShipmentTermInInt,
                                     oExportPI.ColorInfo,
                                     oExportPI.YarnCount,
                                     oExportPI.DepthOfShade,
                                     (int)oExportPI.ProductNature,
                                     oExportPI.OrderSheetID,
                                    nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPI WHERE ExportPIID=%n", nID);
        }
        public static IDataReader GetByLog(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPILog WHERE ExportPILogID=%n", nID);
        }
        //
        /// <summary>
        /// changed by fahim0abir on date: 19 Aug 2015
        /// Previous Query: SELECT * FROM View_ExportPI WHERE PINo=%s AND PICode=%s AND PIYear=%s 
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="sPINo"></param>
        /// <returns></returns>
        public static IDataReader Get(TransactionContext tc, string sPINo, int nTexTileUnit)
        {
            int nCount = 0;
            if (sPINo.Length > 15) { 
                nCount = 15;
                return tc.ExecuteReader("SELECT * FROM View_ExportPI WHERE LEFT(PINo," + nCount + ") LIKE '%" + sPINo.Split('/')[0] + "/" + sPINo.Split('/')[1] + "/" + sPINo.Split('/')[2] + "/" + sPINo.Split('/')[3] + "%' AND RIGHT(PINo,3)='/" + sPINo.Split('/')[4] + "' AND TextileUnit=" + nTexTileUnit);

            }
            else { 
                nCount = 13;
                return tc.ExecuteReader("SELECT * FROM View_ExportPI WHERE LEFT(PINo," + nCount + ") LIKE '%" + sPINo.Split('/')[0] + "/" + sPINo.Split('/')[1] + "/" + sPINo.Split('/')[2] + "%' AND RIGHT(PINo,3)='/" + sPINo.Split('/')[3] + "' AND TextileUnit=" + nTexTileUnit);
            }

            
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPI");
        }

        public static IDataReader GetsWaitForApproval(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * from View_ExportPI  ORDER BY ExportPIID DESC");
        }

        public static IDataReader GetsWaitForCommissionApproval(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * from View_ExportPI  WHERE PICreated=1 AND ApprovedStatus !=4 AND ContractorID>0 AND CommissionApprovedBy=0 AND PIID IN (SELECT ExportPIID FROM CommissionPercent) AND PIYear>12 ORDER BY ExportPIID DESC");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        public static IDataReader GetLogID(TransactionContext tc, int nLogID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportPILog] WHERE LogOf=%n", nLogID);
        }
        public static IDataReader GetsLog(TransactionContext tc, int nExportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPILog WHERE ExportPIID=%n", nExportPIID);
        }
        public static void SwapwithRevice(TransactionContext tc, string nExportPIID, string nPILogOf)
        {
            tc.ExecuteNonQuery(CommandType.StoredProcedure, "sp_PIandPILogSwap", nExportPIID, nPILogOf);
        }

        public static IDataReader GetsByPIIDs(TransactionContext tc, string sIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPI WHERE ExportPIID IN(%q) order by LCID,IssueDate,PICode,PINo,PIYear ", sIDs);
        }
        public static IDataReader Gets(TransactionContext tc, int nContractorID, string sLCIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPI WHERE ContractorID=%n AND ApprovedStatus in(0,1,2) AND LCID IN(%q)", nContractorID, sLCIDs);
        }
        public static IDataReader Gets(TransactionContext tc, int nContractorID, string sLCIDs, bool bPaymentType)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPI WHERE ContractorID=%n AND ApprovedStatus in(0,1,2) AND LCID IN(%q) and PaymentType=%b", nContractorID, sLCIDs, bPaymentType);
        }
        public static IDataReader GetsByLCID(TransactionContext tc, int nLCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPI WHERE LCID=%n AND LCID!=0", nLCID);
        }

        public static IDataReader GetsByLCIDGroup(TransactionContext tc, string sExportIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportPI WHERE LCID IN(%q) AND LCID!=0", sExportIDs);
        }
        //Added By Fauzul on 2 June 2013
        public static IDataReader GetsPIDetails(TransactionContext tc, string sLCIDs)
        {
            return tc.ExecuteReader("select * from [View_ExportPI] where LCID in (" + sLCIDs + ") order by LCID");

        }

        #endregion
    }
}
