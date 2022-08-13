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
    public class SampleInvoiceSetupService : MarshalByRefObject, ISampleInvoiceSetupService
    {
        #region Private functions and declaration
        private SampleInvoiceSetup MapObject(NullHandler oReader)
        {
            SampleInvoiceSetup oSampleInvoiceSetup = new SampleInvoiceSetup();
            oSampleInvoiceSetup.SampleInvoiceSetupID = oReader.GetInt32("SampleInvoiceSetupID");
            oSampleInvoiceSetup.InvoiceType = (EnumSampleInvoiceType)oReader.GetInt32("InvoiceType");
            oSampleInvoiceSetup.BUID = oReader.GetInt32("BUID");
            oSampleInvoiceSetup.Activity = oReader.GetBoolean("Activity");
            oSampleInvoiceSetup.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oSampleInvoiceSetup.PrintNo = oReader.GetInt32("PrintNo");
            oSampleInvoiceSetup.PrintName = oReader.GetString("PrintName");
            oSampleInvoiceSetup.Code = oReader.GetString("Code");
            oSampleInvoiceSetup.ShortName = oReader.GetString("ShortName");
            oSampleInvoiceSetup.Name = oReader.GetString("Name");
            oSampleInvoiceSetup.IsRateChange = oReader.GetBoolean("IsRateChange");
            return oSampleInvoiceSetup;
        }

        private SampleInvoiceSetup CreateObject(NullHandler oReader)
        {
            SampleInvoiceSetup oSampleInvoiceSetup = new SampleInvoiceSetup();
            oSampleInvoiceSetup = MapObject(oReader);
            return oSampleInvoiceSetup;
        }

        private List<SampleInvoiceSetup> CreateObjects(IDataReader oReader)
        {
            List<SampleInvoiceSetup> oSampleInvoiceSetups = new List<SampleInvoiceSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SampleInvoiceSetup oItem = CreateObject(oHandler);
                oSampleInvoiceSetups.Add(oItem);
            }
            return oSampleInvoiceSetups;
        }

        #endregion

        #region Interface implementation
        public SampleInvoiceSetupService() { }


        public SampleInvoiceSetup Save(SampleInvoiceSetup oSampleInvoiceSetup, Int64 nUserId)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                #region SampleInvoiceSetup
                IDataReader reader;
                if (oSampleInvoiceSetup.SampleInvoiceSetupID <= 0)
                {
                    reader = SampleInvoiceSetupDA.InsertUpdate(tc, oSampleInvoiceSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = SampleInvoiceSetupDA.InsertUpdate(tc, oSampleInvoiceSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoiceSetup = new SampleInvoiceSetup();
                    oSampleInvoiceSetup = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSampleInvoiceSetup = new SampleInvoiceSetup();
                oSampleInvoiceSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oSampleInvoiceSetup;
        }

        public String Delete(SampleInvoiceSetup oSampleInvoiceSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SampleInvoiceSetupDA.Delete(tc, oSampleInvoiceSetup, EnumDBOperation.Delete, nUserID);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public SampleInvoiceSetup Get(int id, Int64 nUserId)
        {
            SampleInvoiceSetup oSampleInvoiceSetup = new SampleInvoiceSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SampleInvoiceSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoiceSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oSampleInvoiceSetup;
        }

        public List<SampleInvoiceSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<SampleInvoiceSetup> oSampleInvoiceSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceSetupDA.Gets(sSQL, tc);
                oSampleInvoiceSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SampleInvoiceSetup", e);
                #endregion
            }
            return oSampleInvoiceSetup;
        }
        public SampleInvoiceSetup GetByType(int nInvoiceType, int nBUID, Int64 nUserId)
        {
            SampleInvoiceSetup oSampleInvoiceSetup = new SampleInvoiceSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SampleInvoiceSetupDA.GetByType(tc,  nInvoiceType,  nBUID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoiceSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oSampleInvoiceSetup;
        }
        public SampleInvoiceSetup GetByBU(int nBUID, Int64 nUserId)
        {
            SampleInvoiceSetup oSampleInvoiceSetup = new SampleInvoiceSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SampleInvoiceSetupDA.GetByBU(tc, nBUID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoiceSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oSampleInvoiceSetup;
        }

        public List<SampleInvoiceSetup> Gets(Int64 nUserId)
        {
            List<SampleInvoiceSetup> oSampleInvoiceSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceSetupDA.Gets(tc);
                oSampleInvoiceSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oSampleInvoiceSetups;
        }
        public List<SampleInvoiceSetup> Gets(int nBUID, Int64 nUserId)
        {
            List<SampleInvoiceSetup> oSampleInvoiceSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SampleInvoiceSetupDA.Gets(tc, nBUID);
                oSampleInvoiceSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oSampleInvoiceSetups;
        }

        public SampleInvoiceSetup Activate(SampleInvoiceSetup oSampleInvoiceSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SampleInvoiceSetupDA.Activate(tc, oSampleInvoiceSetup);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSampleInvoiceSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSampleInvoiceSetup = new SampleInvoiceSetup();
                oSampleInvoiceSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oSampleInvoiceSetup;
        }


        #endregion
    }
}