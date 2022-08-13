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
    public class RSRawLotService : MarshalByRefObject, IRSRawLotService
    {
        #region Private functions and declaration
        private RSRawLot MapObject(NullHandler oReader)
        {
            RSRawLot oRSRawLot = new RSRawLot();
            oRSRawLot.RSRawLotID = oReader.GetInt32("RSRawLotID");
            oRSRawLot.RSShiftID = oReader.GetInt32("RSShiftID");
            oRSRawLot.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRSRawLot.LotID = oReader.GetInt32("LotID");
            oRSRawLot.ProductType = (EnumProductType)oReader.GetInt32("ProductType");
            oRSRawLot.ProductID = oReader.GetInt32("ProductID");
            oRSRawLot.ProductBaseID = oReader.GetInt32("ProductBaseID");
            oRSRawLot.CurrencyID = oReader.GetInt32("CurrencyID");
            oRSRawLot.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oRSRawLot.ProductName = oReader.GetString("ProductName");
            oRSRawLot.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRSRawLot.OperationUnitName = oReader.GetString("OperationUnitName");
            oRSRawLot.MUnit = oReader.GetString("MUnit");
            oRSRawLot.LotNo = oReader.GetString("LotNo");
            oRSRawLot.Qty = oReader.GetDouble("Qty");
            oRSRawLot.NumOfCone = oReader.GetInt32("NumOfCone");
            oRSRawLot.UnitPrice = oReader.GetDouble("UnitPrice");
            oRSRawLot.FCUnitPrice = oReader.GetDouble("FCUnitPrice");
            oRSRawLot.Balance = Math.Round(oReader.GetDouble("Balance"),5);
            oRSRawLot.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oRSRawLot.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            return oRSRawLot;
        }
        private RSRawLot CreateObject(NullHandler oReader)
        {
            RSRawLot oRSRawLot = new RSRawLot();
            oRSRawLot = MapObject(oReader);
            return oRSRawLot;
        }
        private List<RSRawLot> CreateObjects(IDataReader oReader)
        {
            List<RSRawLot> oRSRawLot = new List<RSRawLot>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RSRawLot oItem = CreateObject(oHandler);
                oRSRawLot.Add(oItem);
            }
            return oRSRawLot;
        }
        #endregion

        #region Interface implementation
        public RSRawLotService() { }
        public RSRawLot Save(RSRawLot oRSRawLot, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oRSRawLot.RSRawLotID <= 0)
                {
                    reader = RSRawLotDA.InsertUpdate(tc, oRSRawLot, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = RSRawLotDA.InsertUpdate(tc, oRSRawLot, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSRawLot = new RSRawLot();
                    oRSRawLot = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save RSRawLot. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oRSRawLot;
        }
        public List<RSRawLot> SaveMultiple(List<RSRawLot> oRSRawLots, Int64 nUserID)
        {
            List<RSRawLot> oRSRawLots_Result = new List<RSRawLot>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                foreach (RSRawLot oItem in oRSRawLots) 
                {
                    if (oItem.RSRawLotID <= 0)
                    {
                        reader = RSRawLotDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = RSRawLotDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        RSRawLot oRSRawLot = new RSRawLot();
                        oRSRawLot = CreateObject(oReader);
                        oRSRawLots_Result.Add(oRSRawLot);
                    }
                    reader.Close();
                }
               
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save RSRawLot. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oRSRawLots_Result;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                RSRawLot oRSRawLot = new RSRawLot();
                oRSRawLot.RSRawLotID = id;
                RSRawLotDA.Delete(tc, oRSRawLot, EnumDBOperation.Delete, nUserId);
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
        public RSRawLot Get(int id, Int64 nUserId)
        {
            RSRawLot oRSRawLot = new RSRawLot();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = RSRawLotDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSRawLot = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get RSRawLot", e);
                #endregion
            }
            return oRSRawLot;
        }
        public List<RSRawLot> Gets(Int64 nUserID)
        {
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSRawLotDA.Gets(tc);
                oRSRawLots = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RSRawLot", e);
                #endregion
            }
            return oRSRawLots;
        }
        public List<RSRawLot> Gets(string sSQL, Int64 nUserID)
        {
            List<RSRawLot> oRSRawLots = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSRawLotDA.Gets(tc, sSQL);
                oRSRawLots = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RSRawLot", e);
                #endregion
            }
            return oRSRawLots;
        }
        public List<RSRawLot> GetsByRSID(int RSID, Int64 nUserID)
        {
            List<RSRawLot> oRSRawLots = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSRawLotDA.GetsByRSID(tc, RSID);
                oRSRawLots = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RSRawLot", e);
                #endregion
            }
            return oRSRawLots;
        }

        #endregion
    }
}