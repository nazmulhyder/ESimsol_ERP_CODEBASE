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
    public class SingleSalaryProcessService : MarshalByRefObject, ISingleSalaryProcessService
    {
        #region Private functions and declaration
        private SingleSalaryProcess MapObject(NullHandler oReader)
        {
            SingleSalaryProcess oSingleSalaryProcess = new SingleSalaryProcess();
            oSingleSalaryProcess.Name = oReader.GetString("Name");
            return oSingleSalaryProcess;

        }

        private SingleSalaryProcess CreateObject(NullHandler oReader)
        {
            SingleSalaryProcess oSingleSalaryProcess = MapObject(oReader);
            return oSingleSalaryProcess;
        }

        private List<SingleSalaryProcess> CreateObjects(IDataReader oReader)
        {
            List<SingleSalaryProcess> oSingleSalaryProcess = new List<SingleSalaryProcess>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SingleSalaryProcess oItem = CreateObject(oHandler);
                oSingleSalaryProcess.Add(oItem);
            }
            return oSingleSalaryProcess;
        }


        #endregion

        #region Interface implementation
        public SingleSalaryProcessService() { }
        public SingleSalaryProcess Get(string sSQL, Int64 nUserId)
        {
            SingleSalaryProcess oSingleSalaryProcess = new SingleSalaryProcess();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SingleSalaryProcessDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSingleSalaryProcess = CreateObject(oReader);
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

            return oSingleSalaryProcess;
        }

        public List<SingleSalaryProcess> Gets(string sSQL, Int64 nUserID)
        {
            List<SingleSalaryProcess> oSingleSalaryProcess = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SingleSalaryProcessDA.Gets(sSQL, tc);
                oSingleSalaryProcess = CreateObjects(reader);
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
            return oSingleSalaryProcess;
        }
        #endregion
    }
}

