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
    public class SUDeliveryChallanService : MarshalByRefObject, ISUDeliveryChallanService
    {
        #region Private functions and declaration
        private static SUDeliveryChallan MapObject(NullHandler oReader)
        {
            SUDeliveryChallan oSUDeliveryChallan = new SUDeliveryChallan();
            oSUDeliveryChallan.SUDeliveryChallanID = oReader.GetInt32("SUDeliveryChallanID");
            oSUDeliveryChallan.SUDeliveryOrderID = oReader.GetInt32("SUDeliveryOrderID");
            oSUDeliveryChallan.ChallanNo = oReader.GetString("ChallanNo");
            oSUDeliveryChallan.ChallanStatus = (EnumDeliveryChallan) oReader.GetInt32("ChallanStatus");
            oSUDeliveryChallan.ChallanStatusInInt = oReader.GetInt32("ChallanStatus");
            oSUDeliveryChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
            oSUDeliveryChallan.BuyerID = oReader.GetInt32("BuyerID");
            oSUDeliveryChallan.DeliveryTo = oReader.GetInt32("DeliveryTo");
            oSUDeliveryChallan.CheckedBy = oReader.GetInt32("CheckedBy");
            oSUDeliveryChallan.StoreID = oReader.GetInt32("StoreID");
            oSUDeliveryChallan.VehicleNo = oReader.GetString("VehicleNo");
            oSUDeliveryChallan.Remarks = oReader.GetString("Remarks");
            oSUDeliveryChallan.DriverName = oReader.GetString("DriverName");
            oSUDeliveryChallan.DriverContactNo = oReader.GetString("DriverContactNo");
            oSUDeliveryChallan.GatePassNo = oReader.GetString("GatePassNo");

            if (string.IsNullOrEmpty(oSUDeliveryChallan.GatePassNo))
            {
                oSUDeliveryChallan.GatePassNo = oSUDeliveryChallan.ChallanNo.Replace("DC", "GP");
            }

            oSUDeliveryChallan.StorePhoneNo = oReader.GetString("StorePhoneNo");
            oSUDeliveryChallan.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oSUDeliveryChallan.DeliveredBy = oReader.GetInt32("DeliveredBy");
            oSUDeliveryChallan.BuyerName = oReader.GetString("BuyerName");
            oSUDeliveryChallan.BuyerShortName = oReader.GetString("BuyerShortName");
            oSUDeliveryChallan.DeliveryToName = oReader.GetString("DeliveryToName");
            oSUDeliveryChallan.ApprovedByName = oReader.GetString("ApprovedByName");
            oSUDeliveryChallan.DeliveredByName = oReader.GetString("DeliveredByName");
            oSUDeliveryChallan.Qty = oReader.GetDouble("Qty");
            oSUDeliveryChallan.DONo = oReader.GetString("DONo");
            oSUDeliveryChallan.PINo = oReader.GetString("PINo");
            oSUDeliveryChallan.DOType = oReader.GetInt32("DOType");
            oSUDeliveryChallan.DODate = oReader.GetDateTime("DODate");
            oSUDeliveryChallan.WUName = oReader.GetString("WUName");
            oSUDeliveryChallan.BuyerContractorType = oReader.GetInt32("BuyerContractorType");
            oSUDeliveryChallan.DeliveryToContractorType = oReader.GetInt32("DeliveryToContractorType");
            oSUDeliveryChallan.DeliveryPoint = oReader.GetString("DeliveryPoint");
            oSUDeliveryChallan.DBUserName = oReader.GetString("DBUserName");
            oSUDeliveryChallan.TotalAttachment = oReader.GetInt32("TotalAttachment");
            oSUDeliveryChallan.ReturnQty = oReader.GetDouble("ReturnQty");

            oSUDeliveryChallan.DEONo = oReader.GetString("DEONo");
            oSUDeliveryChallan.FEONo = oReader.GetString("FEONo");
            oSUDeliveryChallan.FEOID = oReader.GetInt32("FEOID");
            oSUDeliveryChallan.IsInHouse = oReader.GetBoolean("IsInHouse");
            oSUDeliveryChallan.OrderType = (EnumOrderType)oReader.GetInt32("OrderType");
            oSUDeliveryChallan.CountDetail = oReader.GetInt32("CountDetail");
            oSUDeliveryChallan.CountYarnRcv = oReader.GetInt32("CountYarnRcv");
            oSUDeliveryChallan.LCNo = oReader.GetString("LCNo");
            oSUDeliveryChallan.DExeNo = oReader.GetString("DExeNo");

            return oSUDeliveryChallan;
        }

        private SUDeliveryChallan CreateObject(NullHandler oReader)
        {
            SUDeliveryChallan oSUDeliveryChallan = new SUDeliveryChallan();
            oSUDeliveryChallan = MapObject(oReader);
            return oSUDeliveryChallan;
        }
        private List<SUDeliveryChallan> CreateObjects(IDataReader oReader)
        {
            List<SUDeliveryChallan> oSUDeliveryChallan = new List<SUDeliveryChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SUDeliveryChallan oItem = CreateObject(oHandler);
                oSUDeliveryChallan.Add(oItem);
            }
            return oSUDeliveryChallan;
        }
        #endregion

        #region Interface implementation
        public SUDeliveryChallan Save(SUDeliveryChallan oSUDeliveryChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                List<SUDeliveryChallanDetail> oSUDeliveryChallanDetails = new List<SUDeliveryChallanDetail>();
                oSUDeliveryChallanDetails = oSUDeliveryChallan.SUDeliveryChallanDetails;

                if (oSUDeliveryChallan.SUDeliveryChallanID <= 0)
                {
                    reader = SUDeliveryChallanDA.InsertUpdate(tc, oSUDeliveryChallan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = SUDeliveryChallanDA.InsertUpdate(tc, oSUDeliveryChallan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSUDeliveryChallan = new SUDeliveryChallan();
                    oSUDeliveryChallan = CreateObject(oReader);
                }
                reader.Close();

                #region Delivery Challan Detail

                if (oSUDeliveryChallanDetails != null)
                {
                    string sSUDeliveryChallanDetailIDs = "";
                    foreach (SUDeliveryChallanDetail oItem in oSUDeliveryChallanDetails)
                    {
                        IDataReader readertnc;
                        oItem.SUDeliveryChallanID = oSUDeliveryChallan.SUDeliveryChallanID;
                        if (oItem.SUDeliveryChallanDetailID <= 0)
                        {
                            readertnc = SUDeliveryChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readertnc = SUDeliveryChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);
                        if (readertnc.Read())
                        {
                            sSUDeliveryChallanDetailIDs = sSUDeliveryChallanDetailIDs + oReaderTNC.GetString("SUDeliveryChallanDetailID") + ",";
                        }
                        readertnc.Close();
                    }

                    if (sSUDeliveryChallanDetailIDs.Length > 0)
                    {
                        sSUDeliveryChallanDetailIDs = sSUDeliveryChallanDetailIDs.Remove(sSUDeliveryChallanDetailIDs.Length - 1, 1);
                    }
                    SUDeliveryChallanDetail oSUDeliveryChallanDetail = new SUDeliveryChallanDetail();
                    oSUDeliveryChallanDetail.SUDeliveryChallanID = oSUDeliveryChallan.SUDeliveryChallanID;
                    SUDeliveryChallanDetailDA.Delete(tc, oSUDeliveryChallanDetail, EnumDBOperation.Delete, nUserID, sSUDeliveryChallanDetailIDs);
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSUDeliveryChallan = new SUDeliveryChallan();
                oSUDeliveryChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oSUDeliveryChallan;
        }

        public string Delete(int nSUDeliveryChallanId, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SUDeliveryChallan oSUDeliveryChallan = new SUDeliveryChallan();
                oSUDeliveryChallan.SUDeliveryChallanID = nSUDeliveryChallanId;
                SUDeliveryChallanDA.Delete(tc, oSUDeliveryChallan, EnumDBOperation.Delete, nUserId);
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
        public SUDeliveryChallan UpdateStatus(int nSUDeliveryChallanID,int nNewStatus, Int64 nUserId)
        {
            SUDeliveryChallan oSUDeliveryChallan = new SUDeliveryChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                SUDeliveryChallanDA.UpdateStatus(tc, nSUDeliveryChallanID, nNewStatus);
                IDataReader reader = SUDeliveryChallanDA.Get(tc, nSUDeliveryChallanID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSUDeliveryChallan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Delivery Challan.", e);
                #endregion
            }
            return oSUDeliveryChallan;
        }
        public SUDeliveryChallan Get(int nSUDeliveryChallanId, Int64 nUserId)
        {
            SUDeliveryChallan oSUDeliveryChallan = new SUDeliveryChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SUDeliveryChallanDA.Get(tc, nSUDeliveryChallanId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSUDeliveryChallan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Delivery Challan.", e);
                #endregion
            }
            return oSUDeliveryChallan;
        }
        /// <summary>
        /// added by fahim0abir on date: 4 AUG 2015
        /// for getting list of challans of a specific SUDeliveryOrder
        /// </summary>
        /// <param name="nSUDOID"></param>
        /// <param name="nUserID"></param>
        /// <returns></returns>
        public List<SUDeliveryChallan> GetsBySUDeliveryOrder(int nSUDOID, Int64 nUserID)
        {
            List<SUDeliveryChallan> oSUDeliveryChallans = new List<SUDeliveryChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SUDeliveryChallanDA.GetsBySUDeliveryOrder(tc,nSUDOID);
                oSUDeliveryChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Delivery Challan.", e);
                #endregion
            }
            return oSUDeliveryChallans;
        }
        /// <summary>
        /// added by fahim0abir on date: 30 JUL 2015
        /// for geting list of challans that have ChallanStatus=EnumDeliveryChallan.Approve(1) aka pending challan
        /// </summary>
        /// <returns></returns>
        public List<SUDeliveryChallan> GetsPendingChallan(Int64 nUserID)
        {
            List<SUDeliveryChallan> oSUDeliveryChallans = new List<SUDeliveryChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SUDeliveryChallanDA.GetsPendingChallan(tc);
                oSUDeliveryChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Delivery Challan.", e);
                #endregion
            }
            return oSUDeliveryChallans;
        }
        public List<SUDeliveryChallan> Gets(Int64 nUserID)
        {
            List<SUDeliveryChallan> oSUDeliveryChallans = new List<SUDeliveryChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SUDeliveryChallanDA.Gets(tc);
                oSUDeliveryChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Delivery Challan.", e);
                #endregion
            }
            return oSUDeliveryChallans;
        }

        public List<SUDeliveryChallan> Gets(string sSQL, Int64 nUserID)
        {
            List<SUDeliveryChallan> oSUDeliveryChallans = new List<SUDeliveryChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SUDeliveryChallanDA.Gets(tc, sSQL);
                oSUDeliveryChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Delivery Challan.", e);
                #endregion
            }
            return oSUDeliveryChallans;
        }

        public SUDeliveryChallan SUDeliveryChallanDisburse(int nSUDeliveryChallanId, Int64 nUserId)
        {
            SUDeliveryChallan oSUDeliveryChallan = new SUDeliveryChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SUDeliveryChallanDA.SUDeliveryChallanDisburse(tc, nSUDeliveryChallanId, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSUDeliveryChallan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSUDeliveryChallan = new SUDeliveryChallan();
                oSUDeliveryChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oSUDeliveryChallan;
        }
        #endregion
    }
}
