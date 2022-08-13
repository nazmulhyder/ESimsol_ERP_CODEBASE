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
    public class EmployeeInfoService : MarshalByRefObject, IEmployeeInfoService
    {
        #region Private functions and declaration
        private EmployeeInfo MapObject(NullHandler oReader)
        {
            EmployeeInfo oEmployeeInfo = new EmployeeInfo();
            oEmployeeInfo.EmployeeID = oReader.GetInt32("EmployeeID");
            return oEmployeeInfo;

        }

        private EmployeeInfo CreateObject(NullHandler oReader)
        {
            EmployeeInfo oEmployeeInfo = MapObject(oReader);
            return oEmployeeInfo;
        }

        private List<EmployeeInfo> CreateObjects(IDataReader oReader)
        {
            List<EmployeeInfo> oEmployeeInfo = new List<EmployeeInfo>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeInfo oItem = CreateObject(oHandler);
                oEmployeeInfo.Add(oItem);
            }
            return oEmployeeInfo;
        }


        #endregion

        #region Interface implementation
        public EmployeeInfoService() { }

        public EmployeeInfo Get(string sSQL, Int64 nUserId)
        {
            EmployeeInfo oEmployeeInfo = new EmployeeInfo();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeInfoDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeInfo = CreateObject(oReader);
                }
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
                //oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeInfo;
        }

        public List<EmployeeInfo> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeInfo> oEmployeeInfo = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeInfoDA.Gets(sSQL, tc);
                oEmployeeInfo = CreateObjects(reader);
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
            return oEmployeeInfo;
        }


        public DataSet SearchProfile(int nEmployeeID)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = EmployeeInfoDA.SearchProfile(tc, nEmployeeID);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[4]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DataSet", e);
                #endregion
            }
            return oDataSet;
        }

        #endregion
    }
}

