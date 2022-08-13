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
    public class DesignationDA
    {
        public DesignationDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Designation oDesignation, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Designation]"
                                    + "%n, %s, %s, %n, %s, %n, %n, %n, %b, %n, %n, %n, %u",
                                    oDesignation.DesignationID, oDesignation.Code, oDesignation.Name, oDesignation.HRResponsibilityID, oDesignation.Description, oDesignation.ParentID, oDesignation.Sequence, oDesignation.RequiredPerson, oDesignation.IsActive, oDesignation.EmployeeTypeID, nUserId, (int)eEnumDBOperation, oDesignation.NameInBangla);
        }

        public static void Delete(TransactionContext tc, Designation oDesignation, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Designation]"
                                    + "%n, %s, %s, %n, %s, %n, %n, %n, %b, %n, %n, %n, %u",
                                    oDesignation.DesignationID, oDesignation.Code, oDesignation.Name, oDesignation.HRResponsibilityID, oDesignation.Description, oDesignation.ParentID, oDesignation.Sequence, oDesignation.RequiredPerson, oDesignation.IsActive, oDesignation.EmployeeTypeID, nUserId, (int)eEnumDBOperation, oDesignation.NameInBangla);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Designation WHERE DesignationID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Designation");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsXL(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
