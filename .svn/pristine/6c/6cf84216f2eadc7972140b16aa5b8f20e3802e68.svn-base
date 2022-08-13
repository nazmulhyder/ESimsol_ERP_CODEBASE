using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class ProcessManagementPermissionDA
    {
        public ProcessManagementPermissionDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ProcessManagementPermission oProcessManagementPermission, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProcessManagementPermission] %n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                   oProcessManagementPermission.PMPID, oProcessManagementPermission.UserID,
                   oProcessManagementPermission.CompanyID, oProcessManagementPermission.LocationID,
                   oProcessManagementPermission.DepartmentID, oProcessManagementPermission.ProcessManagementType,
                   oProcessManagementPermission.ProcessType, oProcessManagementPermission.ProcessStatus,
                   nUserID, nDBOperation);
            
        
        }

        public static IDataReader Activity(int nPMPID, bool IsActive, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE ProcessManagementPermission SET IsActive=%b WHERE PMPID=%n;SELECT * FROM View_ProcessManagementPermission WHERE PMPID=%n", IsActive, nPMPID, nPMPID);

        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get( int nPMPID,TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProcessManagementPermission WHERE PMPID=%n", nPMPID);
        }
        public static IDataReader Gets(TransactionContext tc, int nUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProcessManagementPermission WHERE UserID=%n",nUID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
