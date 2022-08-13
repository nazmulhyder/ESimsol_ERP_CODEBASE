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
    public class AbsentAmount_XLService : MarshalByRefObject, IAbsentAmount_XLService
    {
        #region Private functions and declaration
        private AbsentAmount_XL MapObject(NullHandler oReader)
        {
            AbsentAmount_XL oAbsentAmount_XL = new AbsentAmount_XL();

            oAbsentAmount_XL.EmployeeCode = oReader.GetString("EmployeeCode");
            oAbsentAmount_XL.EmployeeName = oReader.GetString("EmployeeName");
            oAbsentAmount_XL.DepartmentName = oReader.GetString("DepartmentName");
            oAbsentAmount_XL.DesignationName = oReader.GetString("DesignationName");
            oAbsentAmount_XL.DOJ = oReader.GetDateTime("DOJ");

            oAbsentAmount_XL.Basic = oReader.GetDouble("Basic");
            oAbsentAmount_XL.HRent = oReader.GetDouble("HRent");
            oAbsentAmount_XL.Medical = oReader.GetDouble("Medical");
            oAbsentAmount_XL.GrossAmount = oReader.GetDouble("GrossAmount");
            oAbsentAmount_XL.Sick = oReader.GetDouble("Sick");
            oAbsentAmount_XL.LWP = oReader.GetDouble("LWP");
            oAbsentAmount_XL.TotalAbsentAmount = oReader.GetDouble("TotalAbsentAmount");
            oAbsentAmount_XL.IsActive = oReader.GetBoolean("IsActive");

            return oAbsentAmount_XL;

        }

        private AbsentAmount_XL CreateObject(NullHandler oReader)
        {
            AbsentAmount_XL oAbsentAmount_XL = MapObject(oReader);
            return oAbsentAmount_XL;
        }

        private List<AbsentAmount_XL> CreateObjects(IDataReader oReader)
        {
            List<AbsentAmount_XL> oAbsentAmount_XLs = new List<AbsentAmount_XL>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AbsentAmount_XL oItem = CreateObject(oHandler);
                oAbsentAmount_XLs.Add(oItem);
            }
            return oAbsentAmount_XLs;
        }

        #endregion

        #region Interface implementation
        public AbsentAmount_XLService() { }
        public List<AbsentAmount_XL> Gets(DateTime StartDate, DateTime EndDate, Int64 nUserID)
        {
            List<AbsentAmount_XL> oAbsentAmount_XL = new List<AbsentAmount_XL>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AbsentAmount_XLDA.Gets(StartDate, EndDate, tc);
                oAbsentAmount_XL = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oAbsentAmount_XL;
        }

        #endregion


    }
}
