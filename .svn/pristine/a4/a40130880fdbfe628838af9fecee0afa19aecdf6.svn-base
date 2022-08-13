using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LabdipAssignedPersonnelDA
    {
        public LabdipAssignedPersonnelDA() { }


        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, LabdipAssignedPersonnel oLAP, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabdipAssignedPersonnel]" + "%n, %n, %n, %n, %n", oLAP.LabdipAssignedPersonnelID, oLAP.LabdipDetailID, oLAP.EmployeeID,  nUserID, nDBOperation);
        }

        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabdipAssignedPersonnel WHERE LabdipAssignedPersonnelID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
