using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class IntegrationSetupDetailService : MarshalByRefObject, IIntegrationSetupDetailService
    {
        #region Private functions and declaration
        private IntegrationSetupDetail MapObject(NullHandler oReader)
        {
            IntegrationSetupDetail oIntegrationSetupDetail = new IntegrationSetupDetail();
            oIntegrationSetupDetail.IntegrationSetupDetailID = oReader.GetInt32("IntegrationSetupDetailID");
            oIntegrationSetupDetail.IntegrationSetupID = oReader.GetInt32("IntegrationSetupID");
            oIntegrationSetupDetail.VoucherTypeID = oReader.GetInt32("VoucherTypeID");
            oIntegrationSetupDetail.BusinessUnitSetup = oReader.GetString("BusinessUnitSetup");
            oIntegrationSetupDetail.VoucherDateSetup = oReader.GetString("VoucherDateSetup");            
            oIntegrationSetupDetail.NarrationSetup = oReader.GetString("NarrationSetup");
            oIntegrationSetupDetail.ReferenceNoteSetup = oReader.GetString("ReferenceNoteSetup");
            oIntegrationSetupDetail.UpdateTable = oReader.GetString("UpdateTable");
            oIntegrationSetupDetail.KeyColumn = oReader.GetString("KeyColumn");
            oIntegrationSetupDetail.Note = oReader.GetString("Note");
            oIntegrationSetupDetail.VoucherName = oReader.GetString("VoucherName");
            return oIntegrationSetupDetail;
        }

        private IntegrationSetupDetail CreateObject(NullHandler oReader)
        {
            IntegrationSetupDetail oIntegrationSetupDetail = new IntegrationSetupDetail();
            oIntegrationSetupDetail = MapObject(oReader);
            return oIntegrationSetupDetail;
        }

        private List<IntegrationSetupDetail> CreateObjects(IDataReader oReader)
        {
            List<IntegrationSetupDetail> oIntegrationSetupDetail = new List<IntegrationSetupDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                IntegrationSetupDetail oItem = CreateObject(oHandler);
                oIntegrationSetupDetail.Add(oItem);
            }
            return oIntegrationSetupDetail;
        }

        #endregion

        #region Interface implementation
        public IntegrationSetupDetailService() { }

        public IntegrationSetupDetail Save(IntegrationSetupDetail oIntegrationSetupDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<IntegrationSetupDetail> _oIntegrationSetupDetails = new List<IntegrationSetupDetail>();
            oIntegrationSetupDetail.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = IntegrationSetupDetailDA.InsertUpdate(tc, oIntegrationSetupDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oIntegrationSetupDetail = new IntegrationSetupDetail();
                    oIntegrationSetupDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oIntegrationSetupDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oIntegrationSetupDetail;
        }

        public IntegrationSetupDetail Get(int nIntegrationSetupDetailID, Int64 nUserId)
        {
            IntegrationSetupDetail oAccountHead = new IntegrationSetupDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = IntegrationSetupDetailDA.Get(tc, nIntegrationSetupDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get IntegrationSetupDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<IntegrationSetupDetail> Gets(int nIntegrationSetupID, Int64 nUserID)
        {
            List<IntegrationSetupDetail> oIntegrationSetupDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = IntegrationSetupDetailDA.Gets(tc, nIntegrationSetupID);
                oIntegrationSetupDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get IntegrationSetupDetail", e);
                #endregion
            }

            return oIntegrationSetupDetail;
        }

        public List<IntegrationSetupDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<IntegrationSetupDetail> oIntegrationSetupDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = IntegrationSetupDetailDA.Gets(tc, sSQL);
                oIntegrationSetupDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get IntegrationSetupDetail", e);
                #endregion
            }

            return oIntegrationSetupDetail;
        }
        #endregion
    }
}
