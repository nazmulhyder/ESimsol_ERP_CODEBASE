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

    public class AdjustmentRequisitionSlipService : MarshalByRefObject, IAdjustmentRequisitionSlipService
    {
        #region Private functions and declaration
        private AdjustmentRequisitionSlip MapObject(NullHandler oReader)
        {
            AdjustmentRequisitionSlip oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
            oAdjustmentRequisitionSlip.AdjustmentRequisitionSlipID = oReader.GetInt32("AdjustmentRequisitionSlipID");
            oAdjustmentRequisitionSlip.ARSlipNo = oReader.GetString("ARSlipNo");
            oAdjustmentRequisitionSlip.Date = oReader.GetDateTime("Date");          
            oAdjustmentRequisitionSlip.RequestedByID = oReader.GetInt32("RequestedByID");
            oAdjustmentRequisitionSlip.RequestedTime = oReader.GetDateTime("RequestedTime");
            oAdjustmentRequisitionSlip.AprovedByID = oReader.GetInt32("AprovedByID");
            oAdjustmentRequisitionSlip.ApprovedTime = oReader.GetDateTime("ApprovedTime");
            oAdjustmentRequisitionSlip.Note = oReader.GetString("Note");
            oAdjustmentRequisitionSlip.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oAdjustmentRequisitionSlip.PreaperByName = oReader.GetString("PreaperByName");
            oAdjustmentRequisitionSlip.RequestedByName = oReader.GetString("RequestedByName");
            oAdjustmentRequisitionSlip.AprovedByName = oReader.GetString("AprovedByName");
            oAdjustmentRequisitionSlip.WUName = oReader.GetString("WUName");
            oAdjustmentRequisitionSlip.AdjustmentType = (EnumAdjustmentType)oReader.GetInt32("AdjustmentType");
            oAdjustmentRequisitionSlip.AdjustmentTypeInt = oReader.GetInt32("AdjustmentType");
            oAdjustmentRequisitionSlip.InoutType = (EnumInOutType)oReader.GetInt32("InoutType");
            oAdjustmentRequisitionSlip.InoutTypeInInt = oReader.GetInt32("InoutType");
            oAdjustmentRequisitionSlip.IsWillVoucherEffect = oReader.GetBoolean("IsWillVoucherEffect");
            //oAdjustmentRequisitionSlip.OUShortName = oReader.GetString("OUShortName");
            //oAdjustmentRequisitionSlip.LocationShortName = oReader.GetString("LocationShortName");
            return oAdjustmentRequisitionSlip;
        }

        private AdjustmentRequisitionSlip CreateObject(NullHandler oReader)
        {
            AdjustmentRequisitionSlip oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
            oAdjustmentRequisitionSlip = MapObject(oReader);
            return oAdjustmentRequisitionSlip;
        }

        private List<AdjustmentRequisitionSlip> CreateObjects(IDataReader oReader)
        {
            List<AdjustmentRequisitionSlip> oAdjustmentRequisitionSlips = new List<AdjustmentRequisitionSlip>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AdjustmentRequisitionSlip oItem = CreateObject(oHandler);
                oAdjustmentRequisitionSlips.Add(oItem);
            }
            return oAdjustmentRequisitionSlips;
        }
        #endregion

        #region Interface implementation
        public AdjustmentRequisitionSlipService() { }

        public AdjustmentRequisitionSlip Save(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<AdjustmentRequisitionSlipDetail> oAdjustmentRequisitionSlipDetails = new List<AdjustmentRequisitionSlipDetail>();
            String sAdjustmentRequisitionSlipDetaillIDs = "";
            try
            {
                oAdjustmentRequisitionSlipDetails = oAdjustmentRequisitionSlip.ARSDetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oAdjustmentRequisitionSlip.AdjustmentRequisitionSlipID <= 0)
                {
                    reader = AdjustmentRequisitionSlipDA.InsertUpdate(tc, oAdjustmentRequisitionSlip, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = AdjustmentRequisitionSlipDA.InsertUpdate(tc, oAdjustmentRequisitionSlip, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
                    oAdjustmentRequisitionSlip = CreateObject(oReader);
                }
                reader.Close();

                #region AdjustmentRequisitionSlipDetails Part
                if (oAdjustmentRequisitionSlipDetails != null)
                {
                    foreach (AdjustmentRequisitionSlipDetail oItem in oAdjustmentRequisitionSlipDetails)
                    {
                        IDataReader readertnc;
                        oItem.AdjustmentRequisitionSlipID = oAdjustmentRequisitionSlip.AdjustmentRequisitionSlipID;
                        if (oItem.AdjustmentRequisitionSlipDetailID <= 0)
                        {
                            readertnc = AdjustmentRequisitionSlipDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readertnc = AdjustmentRequisitionSlipDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        if (readertnc.Read())
                        {
                            sAdjustmentRequisitionSlipDetaillIDs = sAdjustmentRequisitionSlipDetaillIDs + oReaderTNC.GetString("AdjustmentRequisitionSlipDetailID") + ",";
                        }
                        readertnc.Close();
                    }
                    if (sAdjustmentRequisitionSlipDetaillIDs.Length > 0)
                    {
                        sAdjustmentRequisitionSlipDetaillIDs = sAdjustmentRequisitionSlipDetaillIDs.Remove(sAdjustmentRequisitionSlipDetaillIDs.Length - 1, 1);
                    }
                    AdjustmentRequisitionSlipDetail oAdjustmentRequisitionSlipDetail = new AdjustmentRequisitionSlipDetail();
                    oAdjustmentRequisitionSlipDetail.AdjustmentRequisitionSlipID = oAdjustmentRequisitionSlip.AdjustmentRequisitionSlipID;
                    AdjustmentRequisitionSlipDetailDA.Delete(tc, oAdjustmentRequisitionSlipDetail, EnumDBOperation.Delete, nUserID, sAdjustmentRequisitionSlipDetaillIDs);
                    sAdjustmentRequisitionSlipDetaillIDs = "";

                }
                #endregion

                #region get oAdjustmentRequisitionSlip
                //IDataReader readerARS = AdjustmentRequisitionSlipDA.Get(oAdjustmentRequisitionSlip.AdjustmentRequisitionSlipID, tc);
                //NullHandler oReaderARS = new NullHandler(readerARS);
                //if (readerARS.Read())
                //{
                //    oAdjustmentRequisitionSlip = CreateObject(oReaderARS);
                //}
                //readerARS.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
                oAdjustmentRequisitionSlip.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oAdjustmentRequisitionSlip;
        }
        public AdjustmentRequisitionSlip Approve(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = AdjustmentRequisitionSlipDA.InsertUpdate(tc, oAdjustmentRequisitionSlip, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAdjustmentRequisitionSlip = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get AdjustmentRequisitionSlip", e);
                oAdjustmentRequisitionSlip.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oAdjustmentRequisitionSlip;
        }

        public AdjustmentRequisitionSlip Get(int nDOID, Int64 nUserId)
        {
            AdjustmentRequisitionSlip oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AdjustmentRequisitionSlipDA.Get(nDOID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAdjustmentRequisitionSlip = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get AdjustmentRequisitionSlip", e);
                oAdjustmentRequisitionSlip.ErrorMessage = e.Message;
                #endregion
            }

            return oAdjustmentRequisitionSlip;
        }
        public string Delete(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                AdjustmentRequisitionSlipDA.Delete(tc, oAdjustmentRequisitionSlip, EnumDBOperation.Delete, nUserId);
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
        public List<AdjustmentRequisitionSlip> Gets(string sSQL, Int64 nUserID)
        {
            List<AdjustmentRequisitionSlip> oAdjustmentRequisitionSlip = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AdjustmentRequisitionSlipDA.Gets(sSQL, tc);
                oAdjustmentRequisitionSlip = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AdjustmentRequisitionSlip", e);
                #endregion
            }
            return oAdjustmentRequisitionSlip;
        }
        public AdjustmentRequisitionSlip UpdateVoucherEffect(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AdjustmentRequisitionSlipDA.UpdateVoucherEffect(tc, oAdjustmentRequisitionSlip);
                IDataReader reader;
                reader = AdjustmentRequisitionSlipDA.Get(oAdjustmentRequisitionSlip.AdjustmentRequisitionSlipID,tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
                    oAdjustmentRequisitionSlip = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
                oAdjustmentRequisitionSlip.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oAdjustmentRequisitionSlip;

        }
        #endregion
    }
}
