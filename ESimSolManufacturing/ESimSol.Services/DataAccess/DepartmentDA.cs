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

    public class DepartmentDA
    {
        public DepartmentDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Department oDepartment, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Department]"
                                    + "%n, %s, %s, %s, %n, %n, %n, %b, %n, %n, %u",
                                    oDepartment.DepartmentID, oDepartment.Code, oDepartment.Name, oDepartment.Description, oDepartment.ParentID, oDepartment.Sequence, oDepartment.RequiredPerson, oDepartment.IsActive, nUserId, (int)eEnumDBOperation, oDepartment.NameInBangla);
        }

        public static void Delete(TransactionContext tc, Department oDepartment, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Department]"
                                    + "%n, %s, %s, %s, %n, %n, %n, %b, %n, %n, %u",
                                    oDepartment.DepartmentID, oDepartment.Code, oDepartment.Name, oDepartment.Description, oDepartment.ParentID, oDepartment.Sequence, oDepartment.RequiredPerson, oDepartment.IsActive, nUserId, (int)eEnumDBOperation, oDepartment.NameInBangla);
        }
        #endregion
        
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM Department WHERE DepartmentID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Department");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsDeptWithParent(TransactionContext tc, string DeptName)
        {
            return tc.ExecuteReader("EXEC [SP_DepartmentByName]"
                                    + "%s", DeptName);
        }
        public static IDataReader GetDepartmentHierarchy(TransactionContext tc, string sDepartmentIDs)
        {
            return tc.ExecuteReader("EXEC [SP_GetHierarchyList]" + "%s,%s,%s,%b,%b,%s,%b","Department","DepartmentID","ParentID",1,1,sDepartmentIDs,1);
        }
        public static IDataReader GetsXL(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
   
}
