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
    public class ShiftAllowance_XLService : MarshalByRefObject, IShiftAllowance_XLService
    {
        #region Private functions and declaration
        private ShiftAllowance_XL MapObject(NullHandler oReader)
        {
            ShiftAllowance_XL oShiftAllowance_XL = new ShiftAllowance_XL();

            oShiftAllowance_XL.EmployeeCode = oReader.GetString("EmployeeCode");
            oShiftAllowance_XL.EmployeeName = oReader.GetString("EmployeeName");
            oShiftAllowance_XL.DepartmentName = oReader.GetString("DepartmentName");
            oShiftAllowance_XL.DesignationName = oReader.GetString("DesignationName");
            oShiftAllowance_XL.DOJ = oReader.GetDateTime("DOJ");

            oShiftAllowance_XL.Basic = oReader.GetDouble("Basic");
            oShiftAllowance_XL.HRent = oReader.GetDouble("HRent");
            oShiftAllowance_XL.Medical = oReader.GetDouble("Medical");
            oShiftAllowance_XL.GrossAmount = oReader.GetDouble("GrossAmount");
            oShiftAllowance_XL.TotalShiftDay = oReader.GetDouble("TotalShiftDay");
            oShiftAllowance_XL.Value = oReader.GetDouble("Value");
            oShiftAllowance_XL.ShiftAmount = oReader.GetDouble("ShiftAmount");
            oShiftAllowance_XL.IsActive = oReader.GetBoolean("IsActive");

            return oShiftAllowance_XL;

        }

        private ShiftAllowance_XL CreateObject(NullHandler oReader)
        {
            ShiftAllowance_XL oShiftAllowance_XL = MapObject(oReader);
            return oShiftAllowance_XL;
        }

        private List<ShiftAllowance_XL> CreateObjects(IDataReader oReader)
        {
            List<ShiftAllowance_XL> oShiftAllowance_XLs = new List<ShiftAllowance_XL>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ShiftAllowance_XL oItem = CreateObject(oHandler);
                oShiftAllowance_XLs.Add(oItem);
            }
            return oShiftAllowance_XLs;
        }

        #endregion

        #region Interface implementation
        public ShiftAllowance_XLService() { }
        public List<ShiftAllowance_XL> Gets(DateTime StartDate, DateTime EndDate, Int64 nUserID)
        {
            List<ShiftAllowance_XL> oShiftAllowance_XL = new List<ShiftAllowance_XL>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShiftAllowance_XLDA.Gets(StartDate, EndDate, tc);
                oShiftAllowance_XL = CreateObjects(reader);
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
            return oShiftAllowance_XL;
        }

        #endregion


    }
}
