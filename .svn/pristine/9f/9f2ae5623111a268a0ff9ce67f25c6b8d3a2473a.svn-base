using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.Services.Services
{
    public class MaxOTCEmployeeTypeService : MarshalByRefObject, IMaxOTCEmployeeTypeService
    {
        private MaxOTCEmployeeType MapObject(NullHandler oReader)
        {
            MaxOTCEmployeeType oMaxOTCEmployeeType = new MaxOTCEmployeeType();

            oMaxOTCEmployeeType.MaxOTCEmployeeTypeID = oReader.GetInt32("MaxOTCEmployeeTypeID");
            oMaxOTCEmployeeType.MaxOTConfigurationID = oReader.GetInt32("MaxOTConfigurationID");
            oMaxOTCEmployeeType.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");
            oMaxOTCEmployeeType.Remarks = oReader.GetString("Remarks");
            oMaxOTCEmployeeType.EmployeeTypeName = oReader.GetString("EmployeeTypeName");
            oMaxOTCEmployeeType.DBUserID = oReader.GetInt32("DBUserID");
            return oMaxOTCEmployeeType;
        }

        private MaxOTCEmployeeType CreateObject(NullHandler oReader)
        {
            MaxOTCEmployeeType oMaxOTCEmployeeType = new MaxOTCEmployeeType();
            oMaxOTCEmployeeType = MapObject(oReader);
            return oMaxOTCEmployeeType;
        }

        private List<MaxOTCEmployeeType> CreateObjects(IDataReader oReader)
        {
            List<MaxOTCEmployeeType> oMaxOTCEmployeeType = new List<MaxOTCEmployeeType>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MaxOTCEmployeeType oItem = CreateObject(oHandler);
                oMaxOTCEmployeeType.Add(oItem);
            }
            return oMaxOTCEmployeeType;
        }
        public List<MaxOTCEmployeeType> Gets(int id, Int64 nUserID)
        {
            List<MaxOTCEmployeeType> oMaxOTCEmployeeTypes = new List<MaxOTCEmployeeType>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MaxOTCEmployeeTypeDA.Gets(tc, id);
                oMaxOTCEmployeeTypes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                MaxOTCEmployeeType oMaxOTCEmployeeType = new MaxOTCEmployeeType();
                oMaxOTCEmployeeType.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oMaxOTCEmployeeTypes;
        }
    }
}
