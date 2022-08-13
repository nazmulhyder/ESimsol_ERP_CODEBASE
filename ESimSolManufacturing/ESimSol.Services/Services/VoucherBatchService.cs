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
    public class VoucherBatchService : MarshalByRefObject, IVoucherBatchService
    {
        #region Private functions and declaration
        private VoucherBatch MapObject(NullHandler oReader)
        {
            VoucherBatch oVoucherBatch = new VoucherBatch();
            oVoucherBatch.VoucherBatchID = oReader.GetInt32("VoucherBatchID");
            oVoucherBatch.BatchNO = oReader.GetString("BatchNO");
            oVoucherBatch.CreateBy = oReader.GetInt32("CreateBy");
            oVoucherBatch.CreateDate = oReader.GetDateTime("CreateDate");
            oVoucherBatch.BatchStatus = (EnumVoucherBatchStatus)oReader.GetInt16("BatchStatus");
            oVoucherBatch.RequestTo = oReader.GetInt32("RequestTo");
            oVoucherBatch.RequestDate = oReader.GetDateTime("RequestDate");
            oVoucherBatch.CreateByName = oReader.GetString("CreateByName");
            oVoucherBatch.RequestToName = oReader.GetString("RequestToName");
            oVoucherBatch.VoucherCount = oReader.GetInt32("VoucherCount");
            
            return oVoucherBatch;
        }

        private VoucherBatch CreateObject(NullHandler oReader)
        {
            VoucherBatch oVoucherBatch = new VoucherBatch();
            oVoucherBatch = MapObject(oReader);
            return oVoucherBatch;
        }

        private List<VoucherBatch> CreateObjects(IDataReader oReader)
        {
            List<VoucherBatch> oVoucherBatch = new List<VoucherBatch>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherBatch oItem = CreateObject(oHandler);
                oVoucherBatch.Add(oItem);
            }
            return oVoucherBatch;
        }

        #endregion

        #region Interface implementation
        public VoucherBatchService() { }

        public VoucherBatch Save(VoucherBatch oVoucherBatch, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVoucherBatch.VoucherBatchID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VoucherBatch, EnumRoleOperationType.Add);
                    reader = VoucherBatchDA.InsertUpdate(tc, oVoucherBatch, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VoucherBatch, EnumRoleOperationType.Edit);
                    reader = VoucherBatchDA.InsertUpdate(tc, oVoucherBatch, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherBatch = new VoucherBatch();
                    oVoucherBatch = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VoucherBatch. Because of " + e.Message, e);
                #endregion
            }
            return oVoucherBatch;
        }
        public VoucherBatch UpdateStatus(VoucherBatch oVoucherBatch, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = VoucherBatchDA.UpdateStatus(tc, oVoucherBatch, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherBatch = new VoucherBatch();
                    oVoucherBatch = CreateObject(oReader);
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
                throw new ServiceException("Failed to Update Status. Because of " + e.Message, e);
                #endregion
            }
            return oVoucherBatch;
        }

        public string VoucherBatchTransfer(VoucherBatch oVoucherBatch, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                string sVoucherIDs = "";
                List<Voucher> oVouchers = new List<Voucher>();
                oVouchers = oVoucherBatch.Vouchers;
                foreach (Voucher oItem in oVouchers)
                {
                    sVoucherIDs = sVoucherIDs + oItem.VoucherID + ",";
                }
                if (sVoucherIDs.Length > 0)
                {
                    sVoucherIDs = sVoucherIDs.Remove(sVoucherIDs.Length - 1, 1);
                }

                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "VoucherBatch", EnumRoleOperationType.Delete);
                VoucherBatchDA.VoucherBatchTransfer(tc, oVoucherBatch.VoucherBatchID, sVoucherIDs);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete VoucherBatch. Because of " + e.Message, e);
                #endregion
            }
            return "Transfered";
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherBatch oVoucherBatch = new VoucherBatch();
                oVoucherBatch.VoucherBatchID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.VoucherBatch, EnumRoleOperationType.Delete);
                VoucherBatchDA.Delete(tc, oVoucherBatch, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete VoucherBatch. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public VoucherBatch Get(int id, int nUserId)
        {
            VoucherBatch oAccountHead = new VoucherBatch();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherBatchDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VoucherBatch", e);
                #endregion
            }

            return oAccountHead;
        }

       

        public List<VoucherBatch> Gets(int nUserID)
        {
            List<VoucherBatch> oVoucherBatch = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBatchDA.Gets(tc);
                oVoucherBatch = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBatch", e);
                #endregion
            }

            return oVoucherBatch;
        }

        public List<VoucherBatch> GetsByCreateBy(int nUserID)
        {
            List<VoucherBatch> oVoucherBatch = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBatchDA.GetsByCreateBy(tc,nUserID);
                oVoucherBatch = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBatch", e);
                #endregion
            }

            return oVoucherBatch;
        }
        public List<VoucherBatch> GetsTransferTo(int nVoucherBatchID, int nUserID)
        {
            List<VoucherBatch> oVoucherBatch = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherBatchDA.GetsTransferTo(tc,nVoucherBatchID, nUserID);
                oVoucherBatch = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBatch", e);
                #endregion
            }

            return oVoucherBatch;
        }
        public List<VoucherBatch> Gets(string sSQL,int nUserID)
        {
            List<VoucherBatch> oVoucherBatch = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if(sSQL=="")
                {
                sSQL = "Select * from VoucherBatch where VoucherBatchID in (1,2,80,272,347,370,60,45)";
                    }
                reader = VoucherBatchDA.Gets(tc, sSQL);
                oVoucherBatch = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherBatch", e);
                #endregion
            }

            return oVoucherBatch;
        }

       
        #endregion
    }   
}