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
    public class TransferRequisitionSlipService : MarshalByRefObject, ITransferRequisitionSlipService
    {
        #region Private functions and declaration
        private TransferRequisitionSlip MapObject(NullHandler oReader)
        {
            TransferRequisitionSlip oTransferRequisitionSlip = new TransferRequisitionSlip();
            oTransferRequisitionSlip.TRSID = oReader.GetInt32("TRSID");
            oTransferRequisitionSlip.TRSNO = oReader.GetString("TRSNO");
            oTransferRequisitionSlip.FullTRSNO = oReader.GetString("FullTRSNO");
            oTransferRequisitionSlip.RequisitionType = (EnumRequisitionType)oReader.GetInt32("RequisitionType");
            oTransferRequisitionSlip.RequisitionTypeInt = oReader.GetInt32("RequisitionType");
            oTransferRequisitionSlip.TransferStatus = (EnumTransferStatus)oReader.GetInt32("TransferStatus");
            oTransferRequisitionSlip.TransferStatusInt = oReader.GetInt32("TransferStatus");
            oTransferRequisitionSlip.IssueBUID = oReader.GetInt32("IssueBUID");
            oTransferRequisitionSlip.IssueWorkingUnitID = oReader.GetInt32("IssueWorkingUnitID");
            oTransferRequisitionSlip.ReceivedBUID = oReader.GetInt32("ReceivedBUID");
            oTransferRequisitionSlip.ReceivedWorkingUnitID = oReader.GetInt32("ReceivedWorkingUnitID");
            oTransferRequisitionSlip.PreparedByID = oReader.GetInt32("PreparedByID");
            oTransferRequisitionSlip.AuthorisedByID = oReader.GetInt32("AuthorisedByID");
            oTransferRequisitionSlip.Remark = oReader.GetString("Remark");
            oTransferRequisitionSlip.IssueDateTime = oReader.GetDateTime("IssueDateTime");
            oTransferRequisitionSlip.VehicleNo = oReader.GetString("VehicleNo");
            oTransferRequisitionSlip.DriverName = oReader.GetString("DriverName");
            oTransferRequisitionSlip.GatePassNo = oReader.GetString("GatePassNo");
            oTransferRequisitionSlip.DisburseByUserID = oReader.GetInt32("DisburseByUserID");
            oTransferRequisitionSlip.ReceivedByUserID = oReader.GetInt32("ReceivedByUserID");
            oTransferRequisitionSlip.IssueStoreName = oReader.GetString("IssueStoreName");
            oTransferRequisitionSlip.ReceivedStoreName = oReader.GetString("ReceivedStoreName");
            oTransferRequisitionSlip.IssueStoreNameWitoutLocation = oReader.GetString("IssueStoreNameWitoutLocation");
            oTransferRequisitionSlip.ReceivedStoreNameWitoutLocation = oReader.GetString("ReceivedStoreNameWitoutLocation");

            oTransferRequisitionSlip.PreparedByName = oReader.GetString("PreparedByName");
            oTransferRequisitionSlip.AythorisedByName = oReader.GetString("AythorisedByName");
            oTransferRequisitionSlip.DisbursedByName = oReader.GetString("DisbursedByName");
            oTransferRequisitionSlip.ReceivedByName = oReader.GetString("ReceivedByName");
            oTransferRequisitionSlip.IssueBUName = oReader.GetString("IssueBUName");
            oTransferRequisitionSlip.IssueBUShortName = oReader.GetString("IssueBUShortName");
            oTransferRequisitionSlip.ReceivedBUName = oReader.GetString("ReceivedBUName");
            oTransferRequisitionSlip.ReceivedBUShortName = oReader.GetString("ReceivedBUShortName");
            oTransferRequisitionSlip.TotalQty = oReader.GetDouble("TotalQty");
            oTransferRequisitionSlip.MUName = oReader.GetString("MUName");
            oTransferRequisitionSlip.ChallanNo = oReader.GetString("ChallanNo");
            oTransferRequisitionSlip.IsWillVoucherEffect = oReader.GetBoolean("IsWillVoucherEffect");
            oTransferRequisitionSlip.TRSRefType = (EnumTRSRefType)oReader.GetInt32("TRSRefType"); ;
            oTransferRequisitionSlip.TRSRefTypeInt = oReader.GetInt32("TRSRefType"); ;
            oTransferRequisitionSlip.TRSRefID = oReader.GetInt32("TRSRefID"); ;
            return oTransferRequisitionSlip;
        }

        private TransferRequisitionSlip CreateObject(NullHandler oReader)
        {
            TransferRequisitionSlip oRequisitionSlip = new TransferRequisitionSlip();
            oRequisitionSlip = MapObject(oReader);
            return oRequisitionSlip;
        }

        private List<TransferRequisitionSlip> CreateObjects(IDataReader oReader)
        {
            List<TransferRequisitionSlip> oRequisitionSlip = new List<TransferRequisitionSlip>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TransferRequisitionSlip oItem = CreateObject(oHandler);
                oRequisitionSlip.Add(oItem);
            }
            return oRequisitionSlip;
        }

        #endregion

        #region Interface implementation
        public TransferRequisitionSlipService() { }
        public TransferRequisitionSlip Save(TransferRequisitionSlip oTransferRequisitionSlip, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                List<TransferRequisitionSlipDetail> oTransferRequisitionSlipDetails = new List<TransferRequisitionSlipDetail>();
                TransferRequisitionSlipDetail oTransferRequisitionSlipDetail = new TransferRequisitionSlipDetail();
                oTransferRequisitionSlipDetails = oTransferRequisitionSlip.TransferRequisitionSlipDetails;
                string sTRSDetailIDs = "";

                IDataReader reader;
                if (oTransferRequisitionSlip.TRSID <= 0)
                {
                    reader = TransferRequisitionSlipDA.InsertUpdate(tc, oTransferRequisitionSlip, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = TransferRequisitionSlipDA.InsertUpdate(tc, oTransferRequisitionSlip, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTransferRequisitionSlip = new TransferRequisitionSlip();
                    oTransferRequisitionSlip = CreateObject(oReader);
                }
                reader.Close();

                #region TransferRequisitionSlip Detail Part
                if (oTransferRequisitionSlipDetails != null)
                {
                    if (oTransferRequisitionSlipDetails.Count > 0)
                    {
                        foreach (TransferRequisitionSlipDetail oItem in oTransferRequisitionSlipDetails)
                        {
                            IDataReader readerdetail;
                            oItem.TRSID = oTransferRequisitionSlip.TRSID;
                            if (oItem.TRSDetailID <= 0)
                            {
                                readerdetail = TransferRequisitionSlipDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = TransferRequisitionSlipDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sTRSDetailIDs = sTRSDetailIDs + oReaderDetail.GetString("TRSDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sTRSDetailIDs.Length > 0)
                        {
                            sTRSDetailIDs = sTRSDetailIDs.Remove(sTRSDetailIDs.Length - 1, 1);
                        }
                    }
                }
                oTransferRequisitionSlipDetail = new TransferRequisitionSlipDetail();
                oTransferRequisitionSlipDetail.TRSID = oTransferRequisitionSlip.TRSID;
                TransferRequisitionSlipDetailDA.Delete(tc, oTransferRequisitionSlipDetail, EnumDBOperation.Delete, nUserID, sTRSDetailIDs);

                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTransferRequisitionSlip.ErrorMessage = e.Message;
                #endregion
            }

            return oTransferRequisitionSlip;

        }     
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TransferRequisitionSlip oTransferRequisitionSlip = new TransferRequisitionSlip();
                oTransferRequisitionSlip.TRSID = id;
                TransferRequisitionSlipDA.Delete(tc, oTransferRequisitionSlip, EnumDBOperation.Delete, nUserId);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                TransferRequisitionSlip oRequisitionSlip = new TransferRequisitionSlip();
                oRequisitionSlip.ErrorMessage = e.Message;
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public TransferRequisitionSlip Get(int nTRSID, Int64 nUserId)
        {
            TransferRequisitionSlip oAccountHead = new TransferRequisitionSlip();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TransferRequisitionSlipDA.Get(tc, nTRSID);
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
                throw new ServiceException("Failed to Get RequisitionSlip", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<TransferRequisitionSlip> Gets(string sSQL, Int64 nUserID)
        {
            List<TransferRequisitionSlip> oRequisitionSlip = new List<TransferRequisitionSlip>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TransferRequisitionSlipDA.Gets(tc, sSQL, nUserID);
                oRequisitionSlip = CreateObjects(reader);
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

            return oRequisitionSlip;
        }
        public TransferRequisitionSlip Approved(TransferRequisitionSlip oTransferRequisitionSlip, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TransferRequisitionSlipDA.Approved(tc, oTransferRequisitionSlip, nUserID);
                IDataReader reader = TransferRequisitionSlipDA.Get(tc, oTransferRequisitionSlip.TRSID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTransferRequisitionSlip = new TransferRequisitionSlip();
                    oTransferRequisitionSlip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save TransferRequisitionSlip. Because of " + e.Message, e);
                oTransferRequisitionSlip = new TransferRequisitionSlip();
                oTransferRequisitionSlip.ErrorMessage = e.Message;
                #endregion
            }
            return oTransferRequisitionSlip;
        }

        public TransferRequisitionSlip UndoApproved(TransferRequisitionSlip oTransferRequisitionSlip, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TransferRequisitionSlipDA.UndoApproved(tc, oTransferRequisitionSlip, nUserID);
                IDataReader reader = TransferRequisitionSlipDA.Get(tc, oTransferRequisitionSlip.TRSID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTransferRequisitionSlip = new TransferRequisitionSlip();
                    oTransferRequisitionSlip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save TransferRequisitionSlip. Because of " + e.Message, e);
                oTransferRequisitionSlip = new TransferRequisitionSlip();
                oTransferRequisitionSlip.ErrorMessage = e.Message;
                #endregion
            }
            return oTransferRequisitionSlip;
        }

        public TransferRequisitionSlip Disburse(TransferRequisitionSlip oTransferRequisitionSlip, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = TransferRequisitionSlipDA.Disburse(tc, oTransferRequisitionSlip, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTransferRequisitionSlip = new TransferRequisitionSlip();
                    oTransferRequisitionSlip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save TransferRequisitionSlip. Because of " + e.Message, e);
                oTransferRequisitionSlip = new TransferRequisitionSlip();
                oTransferRequisitionSlip.ErrorMessage = e.Message;
                #endregion
            }
            return oTransferRequisitionSlip;
        }
        public TransferRequisitionSlip Received(TransferRequisitionSlip oTransferRequisitionSlip, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = TransferRequisitionSlipDA.Received(tc, oTransferRequisitionSlip, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTransferRequisitionSlip = new TransferRequisitionSlip();
                    oTransferRequisitionSlip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save TransferRequisitionSlip. Because of " + e.Message, e);
                oTransferRequisitionSlip = new TransferRequisitionSlip();
                oTransferRequisitionSlip.ErrorMessage = e.Message;
                #endregion
            }
            return oTransferRequisitionSlip;
        }
        public TransferRequisitionSlip UpdateVoucherEffect(TransferRequisitionSlip oTransferRequisitionSlip, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TransferRequisitionSlipDA.UpdateVoucherEffect(tc, oTransferRequisitionSlip);
                IDataReader reader;
                reader = TransferRequisitionSlipDA.Get(tc, oTransferRequisitionSlip.TRSID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTransferRequisitionSlip = new TransferRequisitionSlip();
                    oTransferRequisitionSlip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oTransferRequisitionSlip = new TransferRequisitionSlip();
                oTransferRequisitionSlip.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oTransferRequisitionSlip;

        }
        #endregion
    }
}
