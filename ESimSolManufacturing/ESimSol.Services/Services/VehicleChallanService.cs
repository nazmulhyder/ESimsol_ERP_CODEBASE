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
    public class VehicleChallanService : MarshalByRefObject, IVehicleChallanService
    {
        #region Private functions and declaration
        private VehicleChallan MapObject(NullHandler oReader)
        {
            VehicleChallan oVehicleChallan = new VehicleChallan();
            oVehicleChallan.VehicleChallanID = oReader.GetInt32("VehicleChallanID");
            oVehicleChallan.ChallanNo = oReader.GetString("ChallanNo");
            oVehicleChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
            oVehicleChallan.ContractorID = oReader.GetInt32("ContractorID");
            oVehicleChallan.ApproveBy = oReader.GetInt32("ApproveBy");
            oVehicleChallan.DeliveredByID = oReader.GetInt32("DeliveredByID");
            oVehicleChallan.SaleInvoiceID = oReader.GetInt32("SaleInvoiceID");
            oVehicleChallan.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oVehicleChallan.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oVehicleChallan.ProductID = oReader.GetInt32("ProductID");
            oVehicleChallan.Note = oReader.GetString("Note");
            oVehicleChallan.GatePassNo= oReader.GetString("GatePassNo");
            oVehicleChallan.ProductName = oReader.GetString("ProductName");
            oVehicleChallan.VehicleNo = oReader.GetString("VehicleNo");
            oVehicleChallan.DeliveredByName = oReader.GetString("DeliveredByName");
            oVehicleChallan.Note = oReader.GetString("Note");
            ////derive
            oVehicleChallan.CustomerName = oReader.GetString("CustomerName");
            oVehicleChallan.PreaperByName = oReader.GetString("PreaperByName");
            oVehicleChallan.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oVehicleChallan.ApproveByName = oReader.GetString("ApproveByName");
            oVehicleChallan.SQNo = oReader.GetString("SQNo");
            oVehicleChallan.ModelNo = oReader.GetString("ModelNo");
            oVehicleChallan.EngineNo = oReader.GetString("EngineNo");
            oVehicleChallan.ChassisNo = oReader.GetString("ChassisNo");
            oVehicleChallan.InvoiceNo = oReader.GetString("InvoiceNo");
            oVehicleChallan.ChallanNoFull = oReader.GetString("ChallanNoFull");

            oVehicleChallan.IsDelivered = oReader.GetInt32("IsDelivered");
            oVehicleChallan.LotID = oReader.GetInt32("LotID");
            oVehicleChallan.LotNo = oReader.GetString("LotNo");


            oVehicleChallan.OperationUnitName = oReader.GetString("OperationUnitName");
            oVehicleChallan.BankName = oReader.GetString("BankName");
            oVehicleChallan.CurrencyName = oReader.GetString("CurrencyName");
            oVehicleChallan.CRate = oReader.GetDouble("CRate");
            oVehicleChallan.OTRAmount = oReader.GetDouble("OTRAmount");
            oVehicleChallan.NetAmount = oReader.GetDouble("NetAmount");
            oVehicleChallan.AdvanceAmount = oReader.GetDouble("AdvanceAmount");
            oVehicleChallan.DiscountAmount = oReader.GetDouble("DiscountAmount");
            oVehicleChallan.OTRAmount = oReader.GetDouble("OTRAmount");

            return oVehicleChallan;
        }


        private VehicleChallan CreateObject(NullHandler oReader)
        {
            VehicleChallan oVehicleChallan = MapObject(oReader);
            return oVehicleChallan;
        }

        private List<VehicleChallan> CreateObjects(IDataReader oReader)
        {
            List<VehicleChallan> oVehicleChallans = new List<VehicleChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VehicleChallan oItem = CreateObject(oHandler);
                oVehicleChallans.Add(oItem);
            }
            return oVehicleChallans;
        }

        #endregion

        #region Interface implementation
        public VehicleChallanService() { }

        public VehicleChallan Save(VehicleChallan oVehicleChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            double nQty = 0;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVehicleChallan.VehicleChallanID <= 0)
                {
                    reader = VehicleChallanDA.InsertUpdate(tc, oVehicleChallan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VehicleChallanDA.InsertUpdate(tc, oVehicleChallan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleChallan = new VehicleChallan();
                    oVehicleChallan = CreateObject(oReader);
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
                oVehicleChallan = new VehicleChallan();
                oVehicleChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oVehicleChallan;
        }
     
        public VehicleChallan Approve(VehicleChallan oVehicleChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = VehicleChallanDA.InsertUpdate(tc, oVehicleChallan, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleChallan = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get VehicleChallan", e);
                oVehicleChallan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oVehicleChallan;
        }
     
        public VehicleChallan Get(int nID, Int64 nUserId)
        {
            VehicleChallan oVehicleChallan = new VehicleChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleChallanDA.Get(nID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleChallan = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get VehicleChallan", e);
                oVehicleChallan.ErrorMessage = e.Message;
                #endregion
            }

            return oVehicleChallan;
        }
     
        public string Delete(VehicleChallan oVehicleChallan, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                VehicleChallanDA.Delete(tc, oVehicleChallan, EnumDBOperation.Delete, nUserId);
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
        public List<VehicleChallan> Gets(string sSQL, Int64 nUserID)
        {
            List<VehicleChallan> oVehicleChallan = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleChallanDA.Gets(sSQL, tc);
                oVehicleChallan = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleChallan", e);
                #endregion
            }
            return oVehicleChallan;
        }


     


  
        
        
        #endregion
       
    }
}
