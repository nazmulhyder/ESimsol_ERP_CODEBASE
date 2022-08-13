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
    public class PayrollProcessManagementObjectService : MarshalByRefObject, IPayrollProcessManagementObjectService
    {
        #region Private functions and declaration
        private PayrollProcessManagementObject MapObject(NullHandler oReader)
        {
            PayrollProcessManagementObject oPayrollProcessManagementObject = new PayrollProcessManagementObject();
            oPayrollProcessManagementObject.PPMOID = oReader.GetInt32("PPMOID");
            oPayrollProcessManagementObject.PPMID = oReader.GetInt32("PPMID");
            oPayrollProcessManagementObject.PPMObject = (EnumPPMObject)oReader.GetInt16("PPMObject");
            oPayrollProcessManagementObject.PPMObjectInt = (int)(EnumPPMObject)oReader.GetInt16("PPMObject");
            oPayrollProcessManagementObject.ObjectID = oReader.GetInt32("ObjectID");
            return oPayrollProcessManagementObject;

        }

        private PayrollProcessManagementObject CreateObject(NullHandler oReader)
        {
            PayrollProcessManagementObject oPayrollProcessManagementObject = MapObject(oReader);
            return oPayrollProcessManagementObject;
        }

        private List<PayrollProcessManagementObject> CreateObjects(IDataReader oReader)
        {
            List<PayrollProcessManagementObject> oPayrollProcessManagementObject = new List<PayrollProcessManagementObject>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PayrollProcessManagementObject oItem = CreateObject(oHandler);
                oPayrollProcessManagementObject.Add(oItem);
            }
            return oPayrollProcessManagementObject;
        }

        #endregion

        #region Interface implementation
        public PayrollProcessManagementObjectService() { }

        public PayrollProcessManagementObject IUD(PayrollProcessManagementObject oPayrollProcessManagementObject, int nDBOperation, Int64 nUserID)
        {

           
            
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PayrollProcessManagementObjectDA.IUD(tc, oPayrollProcessManagementObject, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oPayrollProcessManagementObject = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPayrollProcessManagementObject.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oPayrollProcessManagementObject.PPMOID = 0;
                #endregion
            }
            return oPayrollProcessManagementObject;
        }

        public PayrollProcessManagementObject Get(int nPPMOID, Int64 nUserId)
        {
            PayrollProcessManagementObject oPayrollProcessManagementObject = new PayrollProcessManagementObject();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PayrollProcessManagementObjectDA.Get(nPPMOID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayrollProcessManagementObject = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get PayrollProcessManagementObject", e);
                oPayrollProcessManagementObject.ErrorMessage = e.Message;
                #endregion
            }

            return oPayrollProcessManagementObject;
        }
        public List<PayrollProcessManagementObject> Gets(Int64 nUserID)
        {
            List<PayrollProcessManagementObject> oPayrollProcessManagementObject = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PayrollProcessManagementObjectDA.Gets(tc);
                oPayrollProcessManagementObject = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PayrollProcessManagementObject", e);
                #endregion
            }
            return oPayrollProcessManagementObject;
        }

        public List<PayrollProcessManagementObject> Gets(string sSQL, Int64 nUserID)
        {
            List<PayrollProcessManagementObject> oPayrollProcessManagementObject = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PayrollProcessManagementObjectDA.Gets(sSQL, tc);
                oPayrollProcessManagementObject = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PayrollProcessManagementObject", e);
                #endregion
            }
            return oPayrollProcessManagementObject;
        }

        #endregion
    
    }
}
