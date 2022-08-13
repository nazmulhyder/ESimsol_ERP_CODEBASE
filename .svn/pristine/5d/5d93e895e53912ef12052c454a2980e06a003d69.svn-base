using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ServiceWorkDA
    {
        public ServiceWorkDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ServiceWork oServiceWork, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ServiceWork]"
                                    + "%n , %s, %s, %n, %s, %n, %n",
                                    oServiceWork.ServiceWorkID, oServiceWork.ServiceCode, oServiceWork.ServiceName, oServiceWork.ServiceTypeInt, oServiceWork.Remarks,  (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ServiceWork oServiceWork, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ServiceWork]"
                                    + "%n , %s, %s, %n, %s, %n, %n",
                                    oServiceWork.ServiceWorkID, oServiceWork.ServiceCode, oServiceWork.ServiceName, oServiceWork.ServiceTypeInt, oServiceWork.Remarks, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ServiceWork WHERE ServiceWorkID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ServiceWork Order By [ServiceName]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_ServiceWork
        }

        public static IDataReader GetsByServiceCode(TransactionContext tc, string sServiceCode)
        {
            string sSql = "SELECT * FROM ServiceWork WHERE ServiceCode like '%" + sServiceCode + "%' Order By [ServiceName]";
            return tc.ExecuteReader( sSql);
        }

        public static IDataReader GetsByServiceNameWithType(TransactionContext tc, string sServiceName, int nServiceType)
        {
            string sSql = "SELECT * FROM ServiceWork WHERE ServiceName like '%" + sServiceName + "%' AND ServiceType = "+ nServiceType + " Order By [ServiceName]";
            return tc.ExecuteReader(sSql);
        }
        #endregion
    }
}
