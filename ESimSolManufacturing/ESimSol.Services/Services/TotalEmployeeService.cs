using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class TotalEmployeeService : MarshalByRefObject, ITotalEmployeeService
    {
        #region Private functions and declaration
        private TotalEmployee MapObject(NullHandler oReader)
        {
            TotalEmployee oTotalEmployee = new TotalEmployee();
            oTotalEmployee.BUName = oReader.GetString("BUName");
            oTotalEmployee.LocationName = oReader.GetString("LocationName");
            oTotalEmployee.DepartmentName = oReader.GetString("DepartmentName");
            oTotalEmployee.TotalEmp = oReader.GetInt32("TotalEmp");

            return oTotalEmployee;
        }

        private TotalEmployee CreateObject(NullHandler oReader)  
        {
            TotalEmployee oTotalEmployee = MapObject(oReader);
            return oTotalEmployee;
        }

        private List<TotalEmployee> CreateObjects(IDataReader oReader)
        {
            List<TotalEmployee> oTotalEmployee = new List<TotalEmployee>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TotalEmployee oItem = CreateObject(oHandler);
                oTotalEmployee.Add(oItem);
            }
            return oTotalEmployee;
        }

        #endregion

        #region Interface implementation
        public TotalEmployeeService() { }

        public List<TotalEmployee> Gets(Int64 nUserID)
        {
            List<TotalEmployee> oTotalEmployee = new List<TotalEmployee>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TotalEmployeeDA.Gets(tc);
                oTotalEmployee = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TotalEmployee", e);
                #endregion
            }
            return oTotalEmployee;
        }
        #endregion
    }
}
