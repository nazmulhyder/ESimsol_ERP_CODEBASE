using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricExecutionOrderSpecificationDetailDA
    {
        public FabricExecutionOrderSpecificationDetailDA() { }

        #region IUD
        public static IDataReader IUD(TransactionContext tc, FabricExecutionOrderSpecificationDetail oFEOSDetail, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricExecutionOrderSpecificationDetail] %n	,%n	,%b	,%n	,%s	,%n	,%n	,%n	,%n	,%n	,%n	,%n,%n,%s,%n,%b,%n,%n, %n,%s, %n,%n",
            oFEOSDetail.FEOSDID, oFEOSDetail.FEOSID, oFEOSDetail.IsWarp, oFEOSDetail.ProductID, oFEOSDetail.ColorName, oFEOSDetail.LabdipDetailID, oFEOSDetail.EndsCount, oFEOSDetail.TotalEndActual, oFEOSDetail.Value, oFEOSDetail.Qty, oFEOSDetail.Length, oFEOSDetail.AllowanceWarp, oFEOSDetail.ValueMin, oFEOSDetail.LDNo, oFEOSDetail.SLNo, oFEOSDetail.IsYarnExist, (int)oFEOSDetail.BeamType,oFEOSDetail.TwistedGroupInt,oFEOSDetail.Allowance,oFEOSDetail.BatchNo, nUserId, nDBOperation);
        }
        public static IDataReader UpdateAllowance(TransactionContext tc, FabricExecutionOrderSpecificationDetail oFEOSDetail)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FabricExecutionOrderSpecificationDetail Set LDNo=%s, BatchNo=%s, Allowance=%n WHERE ColorName=%s and isnull(LabdipDetailID,0)=%n and ProductID=%n  and Value=%n and FEOSID=%n",oFEOSDetail.LDNo, oFEOSDetail.BatchNo, oFEOSDetail.Allowance, oFEOSDetail.ColorName, oFEOSDetail.LabdipDetailID, oFEOSDetail.ProductID, oFEOSDetail.Value, oFEOSDetail.FEOSID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("select FEOSID,ProductID,Value,LabdipDetailID,ColorName,ColorNo,ProductName,Allowance,LDNo ,BatchNo from View_FabricExecutionOrderSpecificationDetail where ColorName=%s and isnull(LabdipDetailID,0)=%n and ProductID=%n and  Value=%n and FEOSID=%n group by FEOSID,ProductID,Value,LabdipDetailID,ColorName,ColorNo,ProductName,ColorName,Allowance,LDNo,BatchNo", oFEOSDetail.ColorName, oFEOSDetail.LabdipDetailID, oFEOSDetail.ProductID, oFEOSDetail.Value, oFEOSDetail.FEOSID);
        }
        public static void UpdateQty(TransactionContext tc, FabricExecutionOrderSpecificationDetail oFEOSDetail)
        {
            tc.ExecuteNonQuery("Update FabricExecutionOrderSpecificationDetail Set Qty=%n WHERE FEOSDID=%n", oFEOSDetail.Qty,  oFEOSDetail.FEOSDID);
        }

        public static void DeleteAll(TransactionContext tc, FabricExecutionOrderSpecificationDetail oFEOSDetail, int nDBOperation, Int64 nUserId)
        {
         
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricExecutionOrderSpecificationDetail] %n	,%n	,%b	,%n	,%s	,%n	,%n	,%n	,%n	,%n	,%n	,%n,%n,%s,%n,%b,%n,%n, %n,%s, %n,%n",
           oFEOSDetail.FEOSDID, oFEOSDetail.FEOSID, oFEOSDetail.IsWarp, oFEOSDetail.ProductID, oFEOSDetail.ColorName, oFEOSDetail.LabdipDetailID, oFEOSDetail.EndsCount, oFEOSDetail.TotalEndActual, oFEOSDetail.Value, oFEOSDetail.Qty, oFEOSDetail.Length, oFEOSDetail.AllowanceWarp, oFEOSDetail.ValueMin, oFEOSDetail.LDNo, oFEOSDetail.SLNo, oFEOSDetail.IsYarnExist, (int)oFEOSDetail.BeamType, oFEOSDetail.TwistedGroupInt, oFEOSDetail.Allowance, oFEOSDetail.BatchNo, nUserId, nDBOperation);
     
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFEOSDID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrderSpecificationDetail WHERE FEOSDID=%n", nFEOSDID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFEOSID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrderSpecificationDetail WHERE FEOSID=%n order by SLNo,FEOSDID", nFEOSID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader(sSQL);
        }
        //public static void DeleteAll(TransactionContext tc, int nFEOSID)
        //{
        //    tc.ExecuteNonQuery("Delete from FabricExecutionOrderSpecificationDetail where FEOSID=%n", nFEOSID);
        //}
        public static int GetDUPS(TransactionContext tc, int nFEOSID)
        {
            object obj = tc.ExecuteScalar("Select isnull(Count(*),0) from DUPScheduleDetail where DODID in (Select DyeingOrderDetailid from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingorderFabric where FEOSID=%n))", nFEOSID);
            if (obj == null) return 0;
            return Convert.ToInt32(obj);
        }
        #endregion

    }
}
