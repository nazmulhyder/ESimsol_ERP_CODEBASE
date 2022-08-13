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
    public class ExportBillDA
    {
        public ExportBillDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportBill oEBill, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBill]"
                                    + "%n, %s, %n, %n, %D, %n,%n,%n,%n, %s, %s, %s, %s, %n, %n",
                                    oEBill.ExportBillID, oEBill.ExportBillNo, oEBill.Amount, (int)oEBill.State, oEBill.StartDate, oEBill.ExportLCID, oEBill.BankBranchID_Bill, oEBill.BankBranchID_Ford, oEBill.BankBranchID_Endorse, oEBill.NoteCarry, oEBill.NoOfPackages, oEBill.NetWeight, oEBill.GrossWeight, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc,  ExportBill oEBill, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportBill]"
                                    + "%n, %s, %n, %n, %D, %n,%n,%n,%n, %s, %s, %s, %s, %n, %n",
                                       oEBill.ExportBillID, oEBill.ExportBillNo, oEBill.Amount, (int)oEBill.State, oEBill.StartDate, oEBill.ExportLCID, oEBill.BankBranchID_Bill, oEBill.BankBranchID_Ford, oEBill.BankBranchID_Endorse, oEBill.NoteCarry, oEBill.NoOfPackages, oEBill.NetWeight, oEBill.GrossWeight, nUserID, (int)eEnumDBOperation);
        }

        //public static void UpdateOverDueAmount(TransactionContext tc, ExportBill oEBill)
        //{
        //    tc.ExecuteNonQuery("Update ExportBill SET OverDueAmount = " + oEBill.OverDueAmount + " WHERE ExportBillID = " + oEBill.ExportBillID);
        //}
        #endregion


        #region Generation Function
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportBill] as ExportBill  WHERE ExportBillID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportBill] as ExportBill  WHERE  ExportLCID=%n ", nID);
        }
        public static IDataReader GetsByPI(TransactionContext tc, string  nExportPIIDs)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportBill] as ExportBill  where ExportBillID in (Select aa.ExportBillID from ExportBillDetail as aa where aa.ExportPIDetailID in (Select ExportPIDetailID from ExportPIDetail where ExportPIID in (%q)))", nExportPIIDs);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportBill] as ExportBill  Order By [ExportBillNo]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(" %q ", sSQL);
        }
       
        public static IDataReader GetBills(TransactionContext tc, int eBillEvent)
        {
            return tc.ExecuteReader("Select * from [View_ExportBill] where [State]=%n AND IsActive=1", eBillEvent);
        }
      
        public static IDataReader GetByLDBCNo(TransactionContext tc, string sLDBCNo)
        {
            return tc.ExecuteReader("Select * from [View_ExportBill] where ExportBillID in( select ExportBillid from LDBC where LDBCNo=%s)", sLDBCNo);
        }

        public static IDataReader UpdateStatus(TransactionContext tc, ExportBill oExportBill)
        {
            string sSQL1 = "";
            sSQL1 = SQLParser.MakeSQL("UPDATE ExportBill SET State=%n WHERE ExportBillID=%n", oExportBill.StateInt, oExportBill.ExportBillID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportBill WHERE ExportBillID=" + oExportBill.ExportBillID);
        }
        public static IDataReader SaveDocDate(TransactionContext tc, ExportBill oExportBill, Int64 nUserID)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ExportBill Set DocDate=%d  WHERE ExportBillID=%n", NullHandler.GetNullValue(oExportBill.DocDate), oExportBill.ExportBillID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportBill  WHERE ExportBillID=%n", oExportBill.ExportBillID);

        }
        public static int GetTotalBillCount(TransactionContext tc, int nExportLCID)
        {
            object obj = tc.ExecuteScalar("SELECT Isnull(Max(Sequence),0)+1 FROM ExportBill Where ExportLCID=%n", nExportLCID);
            if (obj == null) return 0;
            return Convert.ToInt32(obj);
        }
     

        #endregion

    }
}
