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
    public class VehicleReturnChallanService : MarshalByRefObject, IVehicleReturnChallanService
    {
        #region Private functions and declaration
        private VehicleReturnChallan MapObject(NullHandler oReader)
        {
            VehicleReturnChallan oVehicleReturnChallan = new VehicleReturnChallan();
            oVehicleReturnChallan.VehicleReturnChallanID = oReader.GetInt32("VehicleReturnChallanID");
            oVehicleReturnChallan.ReturnChallanNo = oReader.GetString("ReturnChallanNo");
            oVehicleReturnChallan.ReturnChallanDate = oReader.GetDateTime("ReturnChallanDate");
            oVehicleReturnChallan.ContractorID = oReader.GetInt32("ContractorID");
            oVehicleReturnChallan.ApproveBy = oReader.GetInt32("ApproveBy");
            oVehicleReturnChallan.ReceivedByID = oReader.GetInt32("ReceivedByID");
            oVehicleReturnChallan.SaleInvoiceID = oReader.GetInt32("SaleInvoiceID");
            oVehicleReturnChallan.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oVehicleReturnChallan.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oVehicleReturnChallan.ProductID = oReader.GetInt32("ProductID");
            oVehicleReturnChallan.Note = oReader.GetString("Note");
            oVehicleReturnChallan.Refund_Amount= oReader.GetDouble("Refund_Amount");
            oVehicleReturnChallan.ProductName = oReader.GetString("ProductName");
            oVehicleReturnChallan.VehicleNo = oReader.GetString("VehicleNo");
            oVehicleReturnChallan.ReceivedByName = oReader.GetString("ReceivedByName");
            oVehicleReturnChallan.Note = oReader.GetString("Note");
            ////derive
            oVehicleReturnChallan.CustomerName = oReader.GetString("CustomerName");
            oVehicleReturnChallan.PreaperByName = oReader.GetString("PreaperByName");
            oVehicleReturnChallan.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oVehicleReturnChallan.ApproveByName = oReader.GetString("ApproveByName");
            oVehicleReturnChallan.SQNo = oReader.GetString("SQNo");
            oVehicleReturnChallan.ModelNo = oReader.GetString("ModelNo");
            oVehicleReturnChallan.EngineNo = oReader.GetString("EngineNo");
            oVehicleReturnChallan.ChassisNo = oReader.GetString("ChassisNo");
            oVehicleReturnChallan.InvoiceNo = oReader.GetString("InvoiceNo");
            oVehicleReturnChallan.ReturnChallanNoFull = oReader.GetString("ReturnChallanNoFull");

            oVehicleReturnChallan.IsReceived = oReader.GetInt32("IsReceived");
            oVehicleReturnChallan.LotID = oReader.GetInt32("LotID");
            oVehicleReturnChallan.LotNo = oReader.GetString("LotNo");


            oVehicleReturnChallan.OperationUnitName = oReader.GetString("OperationUnitName");
            oVehicleReturnChallan.BankName = oReader.GetString("BankName");
            oVehicleReturnChallan.CurrencyName = oReader.GetString("CurrencyName");
            oVehicleReturnChallan.CRate = oReader.GetDouble("CRate");
            oVehicleReturnChallan.OTRAmount = oReader.GetDouble("OTRAmount");
            oVehicleReturnChallan.NetAmount = oReader.GetDouble("NetAmount");
            oVehicleReturnChallan.AdvanceAmount = oReader.GetDouble("AdvanceAmount");
            oVehicleReturnChallan.DiscountAmount = oReader.GetDouble("DiscountAmount");
            oVehicleReturnChallan.OTRAmount = oReader.GetDouble("OTRAmount");

            return oVehicleReturnChallan;
        }


        private VehicleReturnChallan CreateObject(NullHandler oReader)
        {
            VehicleReturnChallan oVehicleReturnChallan = MapObject(oReader);
            return oVehicleReturnChallan;
        }

        private List<VehicleReturnChallan> CreateObjects(IDataReader oReader)
        {
            List<VehicleReturnChallan> oVehicleReturnChallans = new List<VehicleReturnChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VehicleReturnChallan oItem = CreateObject(oHandler);
                oVehicleReturnChallans.Add(oItem);
            }
            return oVehicleReturnChallans;
        }

        #endregion

        #region Interface implementation
        public VehicleReturnChallanService() { }

        public VehicleReturnChallan Save(VehicleReturnChallan oVehicleReturnChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            double nQty = 0;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVehicleReturnChallan.VehicleReturnChallanID <= 0)
                {
                    reader = VehicleReturnChallanDA.InsertUpdate(tc, oVehicleReturnChallan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VehicleReturnChallanDA.InsertUpdate(tc, oVehicleReturnChallan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleReturnChallan = new VehicleReturnChallan();
                    oVehicleReturnChallan = CreateObject(oReader);
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
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oVehicleReturnChallan = new VehicleReturnChallan();
                oVehicleReturnChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oVehicleReturnChallan;
        }
     
        public VehicleReturnChallan Approve(VehicleReturnChallan oVehicleReturnChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = VehicleReturnChallanDA.InsertUpdate(tc, oVehicleReturnChallan, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleReturnChallan = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get VehicleReturnChallan", e);
                oVehicleReturnChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oVehicleReturnChallan;
        }
     
        public VehicleReturnChallan Get(int nID, Int64 nUserId)
        {
            VehicleReturnChallan oVehicleReturnChallan = new VehicleReturnChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleReturnChallanDA.Get(nID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleReturnChallan = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get VehicleReturnChallan", e);
                oVehicleReturnChallan.ErrorMessage = e.Message;
                #endregion
            }

            return oVehicleReturnChallan;
        }
     
        public string Delete(VehicleReturnChallan oVehicleReturnChallan, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                VehicleReturnChallanDA.Delete(tc, oVehicleReturnChallan, EnumDBOperation.Delete, nUserId);
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
        public List<VehicleReturnChallan> Gets(string sSQL, Int64 nUserID)
        {
            List<VehicleReturnChallan> oVehicleReturnChallan = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleReturnChallanDA.Gets(sSQL, tc);
                oVehicleReturnChallan = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleReturnChallan", e);
                #endregion
            }
            return oVehicleReturnChallan;
        }


     


  
        
        
        #endregion
       
    }
}
