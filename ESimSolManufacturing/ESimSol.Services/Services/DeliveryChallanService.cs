using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class DeliveryChallanService : MarshalByRefObject, IDeliveryChallanService
    {
        #region Private functions and declaration
        private DeliveryChallan MapObject(NullHandler oReader)
        {
            DeliveryChallan oDeliveryChallan = new DeliveryChallan();
            oDeliveryChallan.DeliveryChallanID = oReader.GetInt32("DeliveryChallanID");
            oDeliveryChallan.BUID = oReader.GetInt32("BUID");
            oDeliveryChallan.ChallanType = (EnumChallanType)oReader.GetInt16("ChallanType");
            oDeliveryChallan.ChallanNo = oReader.GetString("ChallanNo");
            oDeliveryChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
            oDeliveryChallan.ChallanStatus = (EnumChallanStatus)oReader.GetInt16("ChallanStatus");
            oDeliveryChallan.DeliveryOrderID = oReader.GetInt32("DeliveryOrderID");
            oDeliveryChallan.ContractorID = oReader.GetInt32("ContractorID");
            oDeliveryChallan.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oDeliveryChallan.GatePassNo = oReader.GetString("GatePassNo");
            oDeliveryChallan.VehicleName = oReader.GetString("VehicleName");
            oDeliveryChallan.VehicleNo = oReader.GetString("VehicleNo");
            oDeliveryChallan.ReceivedByName = oReader.GetString("ReceivedByName");
            oDeliveryChallan.Note = oReader.GetString("Note");
            oDeliveryChallan.ApproveBy = oReader.GetInt32("ApproveBy");
            oDeliveryChallan.ApproveDate = oReader.GetDateTime("ApproveDate");
            oDeliveryChallan.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oDeliveryChallan.StoreInchargeID = oReader.GetInt32("StoreInchargeID");
            oDeliveryChallan.BUName = oReader.GetString("BUName");
            oDeliveryChallan.DONo = oReader.GetString("DONo");
            oDeliveryChallan.ContractorName = oReader.GetString("ContractorName");
            oDeliveryChallan.ApproveByName = oReader.GetString("ApproveByName");
            oDeliveryChallan.WUName = oReader.GetString("WUName");
            oDeliveryChallan.DeliveryToName = oReader.GetString("DeliveryToName");
            oDeliveryChallan.RefNo = oReader.GetString("RefNo");
            oDeliveryChallan.DeliveryToAddress = oReader.GetString("DeliveryToAddress");
            oDeliveryChallan.YetToReturnChallanQty = oReader.GetDouble("YetToReturnChallanQty");
            oDeliveryChallan.ProductNatureInInt = oReader.GetInt32("ProductNature");
            oDeliveryChallan.PIID = oReader.GetInt32("PIID");
            oDeliveryChallan.BuyerID = oReader.GetInt32("BuyerID");
            oDeliveryChallan.BuyerName = oReader.GetString("BuyerName");
            oDeliveryChallan.VehicleDateTime = oReader.GetDateTime("VehicleDateTime");
            return oDeliveryChallan;
        }

        private DeliveryChallan CreateObject(NullHandler oReader)
        {
            DeliveryChallan oDeliveryChallan = new DeliveryChallan();
            oDeliveryChallan = MapObject(oReader);
            return oDeliveryChallan;
        }

        private List<DeliveryChallan> CreateObjects(IDataReader oReader)
        {
            List<DeliveryChallan> oDeliveryChallan = new List<DeliveryChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DeliveryChallan oItem = CreateObject(oHandler);
                oDeliveryChallan.Add(oItem);
            }
            return oDeliveryChallan;
        }

        #endregion

        #region Interface implementation
        public DeliveryChallan IUD(DeliveryChallan oDeliveryChallan, short nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sDeliveryChallanDetailIDs = "";
            List<DeliveryChallanDetail> oDeliveryChallanDetails = new List<DeliveryChallanDetail>();
            DeliveryChallanDetail oDeliveryChallanDetail = new DeliveryChallanDetail();
            oDeliveryChallanDetails = oDeliveryChallan.DeliveryChallanDetails;

            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (short)EnumDBOperation.Insert || nDBOperation == (short)EnumDBOperation.Update)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DeliveryChallan, ((nDBOperation == (short)EnumDBOperation.Insert) ? EnumRoleOperationType.Add : EnumRoleOperationType.Edit));
                    if (nDBOperation == (short)EnumDBOperation.Update)
                    {
                        VoucherDA.CheckVoucherReference(tc, "DeliveryChallan", "DeliveryChallanID", oDeliveryChallan.DeliveryChallanID);
                    }
                    IDataReader reader;
                    reader = DeliveryChallanDA.InsertUpdate(tc, oDeliveryChallan, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDeliveryChallan = new DeliveryChallan();
                        oDeliveryChallan = CreateObject(oReader);
                    }
                    reader.Close();

                    #region Delivery Order Detail Part
                    foreach (DeliveryChallanDetail oItem in oDeliveryChallanDetails)
                    {
                        IDataReader readerdetail;
                        oItem.DeliveryChallanID = oDeliveryChallan.DeliveryChallanID;
                        if (oItem.DeliveryChallanDetailID <= 0)
                        {
                            readerdetail = DeliveryChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = DeliveryChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sDeliveryChallanDetailIDs = sDeliveryChallanDetailIDs + oReaderDetail.GetString("DeliveryChallanDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sDeliveryChallanDetailIDs.Length > 0)
                    {
                        sDeliveryChallanDetailIDs = sDeliveryChallanDetailIDs.Remove(sDeliveryChallanDetailIDs.Length - 1, 1);
                    }
                    oDeliveryChallanDetail = new DeliveryChallanDetail();
                    oDeliveryChallanDetail.DeliveryChallanID = oDeliveryChallan.DeliveryChallanID;
                    DeliveryChallanDetailDA.Delete(tc, oDeliveryChallanDetail, EnumDBOperation.Delete, nUserID, sDeliveryChallanDetailIDs);
                    #endregion

                    #region Get Production Order
                    reader = DeliveryChallanDA.Get(tc, oDeliveryChallan.DeliveryChallanID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDeliveryChallan = new DeliveryChallan();
                        oDeliveryChallan = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DeliveryChallan, EnumRoleOperationType.Delete);
                    VoucherDA.CheckVoucherReference(tc, "DeliveryChallan", "DeliveryChallanID", oDeliveryChallan.DeliveryChallanID);
                    DeliveryChallanDA.Delete(tc, oDeliveryChallan, nDBOperation, nUserID);
                    oDeliveryChallan = new DeliveryChallan();
                    oDeliveryChallan.ErrorMessage = Global.DeleteMessage;
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oDeliveryChallan = new DeliveryChallan();
                    oDeliveryChallan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oDeliveryChallan;
        }

        public DeliveryChallan Get(int id, Int64 nUserId)
        {
            DeliveryChallan oDeliveryChallan = new DeliveryChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DeliveryChallanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliveryChallan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DeliveryChallan", e);
                #endregion
            }
            return oDeliveryChallan;
        }

        public List<DeliveryChallan> Gets(string sSQL, Int64 nUserID)
        {
            List<DeliveryChallan> oDeliveryChallans = new List<DeliveryChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DeliveryChallanDA.Gets(tc, sSQL);
                oDeliveryChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Delivery Order", e);
                #endregion
            }
            return oDeliveryChallans;
        }

        public DeliveryChallan Approve(DeliveryChallan oDeliveryChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Valid Delivery Order
                if (oDeliveryChallan.DeliveryChallanID <= 0)
                {
                    throw new Exception("Invalid delivery order!");
                }
                if (oDeliveryChallan.ApproveBy != 0)
                {
                    throw new Exception("Your selected delivery order already approved!");
                }
                #endregion

                #region Delivery Challan Approve

                  AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DeliveryChallan, EnumRoleOperationType.Approved);
                  IDataReader reader = DeliveryChallanDA.Approve(tc, oDeliveryChallan.DeliveryChallanID, nUserID);
                  NullHandler oReader = new NullHandler(reader);
                  if (reader.Read())
                  {
                      oDeliveryChallan = CreateObject(oReader);
                  }
                  reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oDeliveryChallan = new DeliveryChallan();
                    oDeliveryChallan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oDeliveryChallan;
        }


        public DeliveryChallan UpdateVehicleTime(DeliveryChallan oDeliveryChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<DeliveryChallan> oDeliveryChallans = new List<DeliveryChallan>();
                oDeliveryChallans = oDeliveryChallan.DeliveryChallans;
                tc = TransactionContext.Begin(true);
             
                #region Delivery Challan Update vehicle time
                foreach (DeliveryChallan oItem in oDeliveryChallans)
                {
                    oItem.VehicleDateTime = oDeliveryChallan.VehicleDateTime;
                    DeliveryChallanDetailDA.UpdateVehicleTime(tc, oItem, nUserID);  
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oDeliveryChallan = new DeliveryChallan();
                    oDeliveryChallan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oDeliveryChallan;
        }

        #endregion
    }

}
