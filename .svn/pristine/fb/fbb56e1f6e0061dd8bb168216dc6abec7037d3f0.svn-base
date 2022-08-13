using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportSCDA
    {
        public ExportSCDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportSC oExportSC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportSC]" + "%n, %n,%n, %D, %d, %d, %s, %n, %n,%n, %n, %n",
                                    oExportSC.ExportSCID, oExportSC.ExportPIID, oExportSC.BuyerID,  oExportSC.SCDate, oExportSC.LCOpenDate, oExportSC.DeliveryDate, oExportSC.PaymentTerms, oExportSC.Production_ControlBy, oExportSC.Delivery_ControlBy, oExportSC.RateUnit, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, ExportSC oExportSC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportSC]" + "%n, %n,%n, %D, %d, %d, %s, %n, %n,%n, %n, %n",
                                    oExportSC.ExportSCID, oExportSC.ExportPIID, oExportSC.BuyerID, oExportSC.SCDate, oExportSC.LCOpenDate, oExportSC.DeliveryDate, oExportSC.PaymentTerms, oExportSC.Production_ControlBy, oExportSC.Delivery_ControlBy, oExportSC.RateUnit, (int)eEnumDBOperation, nUserID);
        }

        public static IDataReader InsertUpdateLog(TransactionContext tc, ExportSC oExportSC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportSCLog]" + "%n, %n, %D, %d, %d, %s, %n, %n, %n, %n",
                                  oExportSC.ExportSCID, oExportSC.ExportPIID, oExportSC.SCDate, oExportSC.LCOpenDate, oExportSC.DeliveryDate, oExportSC.PaymentTerms, oExportSC.Production_ControlBy, oExportSC.Delivery_ControlBy, (int)eEnumDBOperation, nUserID);
        }
        public static IDataReader AcceptRevise(TransactionContext tc, ExportSC oExportSC, Int64 nUserID)//Use for Plastict and Integrated
        {
            return tc.ExecuteReader("EXEC [SP_Accept_ExportSCRevise]" + "%n, %n, %D,  %n, %n",
                                  oExportSC.ExportSCID, oExportSC.ExportPIID, oExportSC.SCDate,oExportSC.RateUnit,  nUserID);
        }
        public static IDataReader ApproveSalesContract(TransactionContext tc, ExportSC oExportSC, Int64 nUserID)//use for Plastict and integrated
        {
            return tc.ExecuteReader("EXEC [SP_Approve_ExportSC]" + "%n, %n, %n",
                                    oExportSC.ExportSCID, oExportSC.ExportPIID,  nUserID);
        }
        public static IDataReader ExportPIToPIOrderTransfer(TransactionContext tc, int nExportPIID_TO, int nExportPIID_From, Int64 nUserID)//use for Plastict and integrated
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportPIToPIOrderTransfer]" + "%n, %n, %n",
                                    nExportPIID_TO, nExportPIID_From, nUserID);
        }
        public static IDataReader UpdateExportSC(TransactionContext tc, ExportSC oExportSC)
        {
            string sSQL1 = SQLParser.MakeSQL("Update exportSC set Production_ControlBy=%n, Delivery_ControlBy=%n,RateAdjConID=%n,QtyAdjConID=%n,DicChargeAdjConID=%n where ExportSCID=%n", oExportSC.Production_ControlBy, oExportSC.Delivery_ControlBy, oExportSC.RateAdjConID, oExportSC.QtyAdjConID, oExportSC.DicChargeAdjConID, oExportSC.ExportSCID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("Select * from View_ExportSC where ExportSCID=" + oExportSC.ExportSCID);

        }

        public static IDataReader OrderClose(TransactionContext tc, ExportSC oExportSC)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ExportSC Set IsClose=~isnull(IsClose,0) WHERE ExportSCID=%n", oExportSC.ExportSCID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportSC WHERE ExportSCID=%n", oExportSC.ExportSCID);

        }
        public static IDataReader UpdateBuyer(TransactionContext tc, ExportSC oExportSC)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ExportPI Set BuyerID=%n WHERE ExportPIID=%n",oExportSC.BuyerID, oExportSC.ExportPIID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportSC WHERE ExportSCID=%n", oExportSC.ExportSCID);

        }
      
        #endregion

      

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSC WHERE ExportSCID=%n", nID);
        }
        public static IDataReader GetPI(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSC WHERE ExportPIID=%n", nID);
        }
        /// <summary>
        /// changed by fahim0abir on date: 19 Aug 2015
        /// Previous Query: SELECT * FROM View_ExportSC WHERE PINo=%s AND PICode=%s AND PIYear=%s 
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="sPINo"></param>
        /// <returns></returns>
        public static IDataReader Get(TransactionContext tc, string sPINo, int nTexTileUnit)
        {
            int nCount = 0;
            if (sPINo.Length > 15) { 
                nCount = 15;
                return tc.ExecuteReader("SELECT * FROM View_ExportSC WHERE LEFT(PINo," + nCount + ") LIKE '%" + sPINo.Split('/')[0] + "/" + sPINo.Split('/')[1] + "/" + sPINo.Split('/')[2] + "/" + sPINo.Split('/')[3] + "%' AND RIGHT(PINo,3)='/" + sPINo.Split('/')[4] + "' AND TextileUnit=" + nTexTileUnit);

            }
            else { 
                nCount = 13;
                return tc.ExecuteReader("SELECT * FROM View_ExportSC WHERE LEFT(PINo," + nCount + ") LIKE '%" + sPINo.Split('/')[0] + "/" + sPINo.Split('/')[1] + "/" + sPINo.Split('/')[2] + "%' AND RIGHT(PINo,3)='/" + sPINo.Split('/')[3] + "' AND TextileUnit=" + nTexTileUnit);
            }
            
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT Top(150)* FROM View_ExportSC where IsRevisePI=1 AND BUID in (Select BusinessUnitID from BusinessUnit where BusinessUnit.BusinessUnitType="+(int)EnumBusinessUnitType.Dyeing+" ) Order By IssueDate DESC");
        }

        public static IDataReader GetsWaitForApproval(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * from View_ExportSC  ORDER BY ExportSCID DESC");
        }

        public static IDataReader GetsWaitForCommissionApproval(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * from View_ExportSC  WHERE PICreated=1 AND ApprovedStatus !=4 AND ContractorID>0 AND CommissionApprovedBy=0 AND PIID IN (SELECT ExportSCID FROM CommissionPercent) AND PIYear>12 ORDER BY ExportSCID DESC");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        public static IDataReader GetLogID(TransactionContext tc, int nLogID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportSCLog] WHERE LogOf=%n", nLogID);
        }
        public static IDataReader GetsLog(TransactionContext tc, int nExportSCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSCLog WHERE ExportSCID=%n", nExportSCID);
        }
        public static void SwapwithRevice(TransactionContext tc, string nExportSCID, string nPILogOf)
        {
            tc.ExecuteNonQuery(CommandType.StoredProcedure, "sp_PIandPILogSwap", nExportSCID, nPILogOf);
        }

        public static IDataReader GetsByPIIDs(TransactionContext tc, string sIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSC WHERE ExportSCID IN(%q) order by LCID,IssueDate,PICode,PINo,PIYear ", sIDs);
        }
        public static IDataReader Gets(TransactionContext tc, int nContractorID, string sLCIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSC WHERE ContractorID=%n AND ApprovedStatus in(0,1,2) AND LCID IN(%q)", nContractorID, sLCIDs);
        }
        public static IDataReader Gets(TransactionContext tc, int nContractorID, string sLCIDs, bool bPaymentType)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSC WHERE ContractorID=%n AND ApprovedStatus in(0,1,2) AND LCID IN(%q) and PaymentType=%b", nContractorID, sLCIDs, bPaymentType);
        }
        public static IDataReader GetsByLCID(TransactionContext tc, int nLCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSC WHERE LCID=%n AND LCID!=0", nLCID);
        }

        public static IDataReader GetsByBU(TransactionContext tc, int nBUID, int nProductNature)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportSC WHERE BUID=%n AND ProductNature=%n AND ISNULL(ApprovedBy,0) = 0 ", nBUID, nProductNature);
        }

        #endregion
    }
}
