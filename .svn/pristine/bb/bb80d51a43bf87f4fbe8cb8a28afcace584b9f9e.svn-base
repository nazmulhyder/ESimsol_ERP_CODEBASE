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
    public class AttendanceAccessPointEmployeeDA
    {
        public AttendanceAccessPointEmployeeDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, AttendanceAccessPointEmployee oAAPEmp, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceAccessPointEmployee] %n, %n, %n, %b, %n, %n",
                                    oAAPEmp.AAPEmployeeID, oAAPEmp.AAPID, oAAPEmp.EmployeeID, oAAPEmp.IsActive, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nAAPEmployeeID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceAccessPointEmployee WHERE AAPEmployeeID=%n", nAAPEmployeeID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
