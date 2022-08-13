using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class DUDeliveryChallanService : MarshalByRefObject, IDUDeliveryChallanService
    {
        #region Private functions and declaration
        private DUDeliveryChallan MapObject(NullHandler oReader)
        {
            DUDeliveryChallan oDUDeliveryChallan = new DUDeliveryChallan();
            oDUDeliveryChallan.DUDeliveryChallanID = oReader.GetInt32("DUDeliveryChallanID");
            oDUDeliveryChallan.ChallanNo = oReader.GetString("ChallanNo");
            oDUDeliveryChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
            oDUDeliveryChallan.ContractorID = oReader.GetInt32("ContractorID");
            oDUDeliveryChallan.ApproveBy = oReader.GetInt32("ApproveBy");
            oDUDeliveryChallan.ReceiveByID = oReader.GetInt32("ReceiveByID");
            oDUDeliveryChallan.StoreInchargeID = oReader.GetInt32("StoreInchargeID");
            oDUDeliveryChallan.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oDUDeliveryChallan.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oDUDeliveryChallan.OrderType = oReader.GetInt32("OrderType");
            oDUDeliveryChallan.Note = oReader.GetString("Note");
            oDUDeliveryChallan.GatePassNo= oReader.GetString("GatePassNo");
            oDUDeliveryChallan.VehicleName = oReader.GetString("VehicleName");
            oDUDeliveryChallan.VehicleNo = oReader.GetString("VehicleNo");
            oDUDeliveryChallan.ReceivedByName = oReader.GetString("ReceivedByName");
            oDUDeliveryChallan.Note = oReader.GetString("Note");
            ////derive
            oDUDeliveryChallan.ContractorName = oReader.GetString("ContractorName");
            oDUDeliveryChallan.PreaperByName = oReader.GetString("PreaperByName");
            oDUDeliveryChallan.ApproveByName = oReader.GetString("ApproveByName");
            oDUDeliveryChallan.Qty = oReader.GetDouble("Qty");
            oDUDeliveryChallan.FactoryAddress = oReader.GetString("FactoryAddress");
            oDUDeliveryChallan.OrderTypeSt = oReader.GetString("OrderTypeSt");
            oDUDeliveryChallan.OrderCode = oReader.GetString("OrderCode");
            oDUDeliveryChallan.DeliveryBy = oReader.GetString("DeliveryBy");
            oDUDeliveryChallan.PackCountBy = (EnumPackCountBy)oReader.GetInt32("PackCountBy");
            
            
            return oDUDeliveryChallan;

        }


        private DUDeliveryChallan CreateObject(NullHandler oReader)
        {
            DUDeliveryChallan oDUDeliveryChallan = MapObject(oReader);
            return oDUDeliveryChallan;
        }

        private List<DUDeliveryChallan> CreateObjects(IDataReader oReader)
        {
            List<DUDeliveryChallan> oDUDeliveryChallans = new List<DUDeliveryChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryChallan oItem = CreateObject(oHandler);
                oDUDeliveryChallans.Add(oItem);
            }
            return oDUDeliveryChallans;
        }

        #endregion

        #region Interface implementation
        public DUDeliveryChallanService() { }

        public DUDeliveryChallan Save(DUDeliveryChallan oDUDeliveryChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<DUDeliveryChallanDetail> oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
            //String sDUDeliveryChallanDetaillIDs = "";
            double nQty = 0;
            try
            {
                oDUDeliveryChallanDetails = oDUDeliveryChallan.DUDeliveryChallanDetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDUDeliveryChallan.DUDeliveryChallanID <= 0)
                {
                    reader = DUDeliveryChallanDA.InsertUpdate(tc, oDUDeliveryChallan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    VoucherDA.CheckVoucherReference(tc, "DUDeliveryChallan", "DUDeliveryChallanID", oDUDeliveryChallan.DUDeliveryChallanID);
                    reader = DUDeliveryChallanDA.InsertUpdate(tc, oDUDeliveryChallan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallan = new DUDeliveryChallan();
                    oDUDeliveryChallan = CreateObject(oReader);
                }
                reader.Close();

                #region DUDeliveryChallanDetails Part
                if (oDUDeliveryChallanDetails != null)
                {
                    foreach (DUDeliveryChallanDetail oItem in oDUDeliveryChallanDetails)
                    {
                        IDataReader readertnc;
                        oItem.DUDeliveryChallanID = oDUDeliveryChallan.DUDeliveryChallanID;
                        nQty = nQty + oItem.Qty;
                        if (oItem.DUDeliveryChallanDetailID <= 0)
                        {
                            readertnc = DUDeliveryChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                        }
                        else
                        {
                            readertnc = DUDeliveryChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        readertnc.Close();
                    }
                    oDUDeliveryChallan.Qty = nQty;
                  

                }
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
                oDUDeliveryChallan = new DUDeliveryChallan();
                oDUDeliveryChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUDeliveryChallan;
        }
     
        public DUDeliveryChallan Approve(DUDeliveryChallan oDUDeliveryChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DUDeliveryChallanDA.InsertUpdate(tc, oDUDeliveryChallan, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallan = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUDeliveryChallan", e);
                oDUDeliveryChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDUDeliveryChallan;
        }
     
        public DUDeliveryChallan Get(int nID, Int64 nUserId)
        {
            DUDeliveryChallan oDUDeliveryChallan = new DUDeliveryChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDeliveryChallanDA.Get(nID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallan = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUDeliveryChallan", e);
                oDUDeliveryChallan.ErrorMessage = e.Message;
                #endregion
            }

            return oDUDeliveryChallan;
        }
        public DUDeliveryChallan GetbyDO(int nDOID, Int64 nUserId)
        {
            DUDeliveryChallan oDUDeliveryChallan = new DUDeliveryChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDeliveryChallanDA.GetbyDO(nDOID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallan = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUDeliveryChallan", e);
                oDUDeliveryChallan.ErrorMessage = e.Message;
                #endregion
            }

            return oDUDeliveryChallan;
        }
     
        public List<DUDeliveryChallan> GetsBy(string sContractorID, Int64 nUserID)
        {
            List<DUDeliveryChallan> oDUDeliveryChallan = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryChallanDA.GetsBy(tc, sContractorID);
                oDUDeliveryChallan = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryChallan", e);
                #endregion
            }
            return oDUDeliveryChallan;
        }
        public List<DUDeliveryChallan> GetsByPI(int nExportPIID, Int64 nUserID)
        {
            List<DUDeliveryChallan> oDUDeliveryChallan = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryChallanDA.GetsByPI(tc, nExportPIID);
                oDUDeliveryChallan = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryChallan", e);
                #endregion
            }
            return oDUDeliveryChallan;
        }
    
        public string Delete(DUDeliveryChallan oDUDeliveryChallan, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherDA.CheckVoucherReference(tc, "DUDeliveryChallan", "DUDeliveryChallanID", oDUDeliveryChallan.DUDeliveryChallanID);
                DUDeliveryChallanDA.Delete(tc, oDUDeliveryChallan, EnumDBOperation.Delete, nUserId);
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
        public List<DUDeliveryChallan> Gets(string sSQL, Int64 nUserID)
        {
            List<DUDeliveryChallan> oDUDeliveryChallan = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliveryChallanDA.Gets(sSQL, tc);
                oDUDeliveryChallan = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryChallan", e);
                #endregion
            }
            return oDUDeliveryChallan;
        }

        public DUDeliveryChallan UpdateFields(DUDeliveryChallan oDUDeliveryChallan, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DUDeliveryChallanDA.UpdateFields(tc, oDUDeliveryChallan);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDUDeliveryChallan = new DUDeliveryChallan();
                oDUDeliveryChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUDeliveryChallan;
        }
     
        #endregion
       
    }
}
