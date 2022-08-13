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
    public class TotalEmployeeDA
    {
        public TotalEmployeeDA() { }

        #region Get

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT (SELECT BUName FROM View_DepartmentRequirementPolicy WHERE DepartmentID = VE.DepartmentID"
                                     +" AND LocationID = VE.LocationID) BUName,LocationName,DepartmentName, COUNT(DISTINCT(EmployeeID)) TotalEmp"
                                     +" FROM View_Employee VE WHERE LocationName IS NOT NULL AND DepartmentName IS NOT NULL AND IsActive=1"
                                      + " GROUP BY LocationID,LocationName,DepartmentID,DepartmentName ORDER BY DepartmentName");
        }

        #endregion


    }
}
