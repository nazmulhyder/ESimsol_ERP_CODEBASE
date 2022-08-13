using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
   public class CustomerPersonalInfoDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CustomerPersonalInfo oCustomerPersonalInfo, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CustomerPersonalInfo]" + "%n,%n,%s,%s,%s,%s,%s,%s,%d,%n,%s,%d,%d,%s,%n,%n",
                                    oCustomerPersonalInfo.CustomerPersonalInfoID,
                                    oCustomerPersonalInfo.CustomerID,
                                    oCustomerPersonalInfo.CustomerName,
                                    oCustomerPersonalInfo.EmployeerName,
                                    oCustomerPersonalInfo.Designation,
                                    oCustomerPersonalInfo.Address,
                                    oCustomerPersonalInfo.ContactNumber,
                                    oCustomerPersonalInfo.EmailAddress,
                                    oCustomerPersonalInfo.DateOfBirth,
                                    oCustomerPersonalInfo.MarriedStatus,
                                    oCustomerPersonalInfo.SpouseName,
                                    oCustomerPersonalInfo.SpouseDateOfBirth,
                                    oCustomerPersonalInfo.AnniversaryDate,
                                    oCustomerPersonalInfo.Remarks,
                                    (int)eEnumDBOperation,
                                    nUserID);
        }
        public static void Delete(TransactionContext tc, CustomerPersonalInfo oCustomerPersonalInfo, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CustomerPersonalInfo]" + "%n,%n,%s,%s,%s,%s,%s,%s,%d,%n,%s,%d,%d,%s,%n,%n",
                                     oCustomerPersonalInfo.CustomerPersonalInfoID,
                                    oCustomerPersonalInfo.CustomerID,
                                    oCustomerPersonalInfo.CustomerName,
                                    oCustomerPersonalInfo.EmployeerName,
                                    oCustomerPersonalInfo.Designation,
                                    oCustomerPersonalInfo.Address,
                                    oCustomerPersonalInfo.ContactNumber,
                                    oCustomerPersonalInfo.EmailAddress,
                                    oCustomerPersonalInfo.DateOfBirth,
                                    oCustomerPersonalInfo.MarriedStatus,
                                    oCustomerPersonalInfo.SpouseName,
                                    oCustomerPersonalInfo.SpouseDateOfBirth,
                                    oCustomerPersonalInfo.AnniversaryDate,
                                    oCustomerPersonalInfo.Remarks,
                                    (int)eEnumDBOperation,
                                    nUserID);
        }
       
        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CustomerPersonalInfo WHERE CustomerID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CustomerPersonalInfo");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
