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
    public class ServiceChargeService : MarshalByRefObject, IServiceChargeService
    {
        #region Private functions and declaration
        private ServiceCharge MapObject(NullHandler oReader)
        {
            ServiceCharge oServiceCharge = new ServiceCharge();
            oServiceCharge.ServiceChargeID = oReader.GetInt32("ServiceChargeID");
            oServiceCharge.Name = oReader.GetString("Name");
            oServiceCharge.Note = oReader.GetString("Note");
            return oServiceCharge;

        }

        private ServiceCharge CreateObject(NullHandler oReader)
        {
            ServiceCharge oServiceCharge = MapObject(oReader);
            return oServiceCharge;
        }

        private List<ServiceCharge> CreateObjects(IDataReader oReader)
        {
            List<ServiceCharge> oServiceCharge = new List<ServiceCharge>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ServiceCharge oItem = CreateObject(oHandler);
                oServiceCharge.Add(oItem);
            }
            return oServiceCharge;
        }


        #endregion

        #region Interface implementation
        public ServiceChargeService() { }
        public ServiceCharge IUD(ServiceCharge oServiceCharge, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    reader = ServiceChargeDA.IUD(tc, oServiceCharge, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oServiceCharge = new ServiceCharge();
                        oServiceCharge  = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = ServiceChargeDA.IUD(tc, oServiceCharge, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oServiceCharge.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oServiceCharge = new ServiceCharge();
                oServiceCharge.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oServiceCharge;
        }



        public ServiceCharge Get(string sSQL, Int64 nUserId)
        {
            ServiceCharge oServiceCharge = new ServiceCharge();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ServiceChargeDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceCharge = CreateObject(oReader);
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

            return oServiceCharge;
        }


        public ServiceCharge Get(int id, Int64 nUserId)
        {
            ServiceCharge oServiceCharge = new ServiceCharge();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ServiceChargeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceCharge = CreateObject(oReader);
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

            return oServiceCharge;
        }

        public List<ServiceCharge> Gets(string sSQL, Int64 nUserID)
        {
            List<ServiceCharge> oServiceCharge = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ServiceChargeDA.Gets(sSQL, tc);
                oServiceCharge = CreateObjects(reader);
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
            return oServiceCharge;
        }
        #endregion
    }
}

