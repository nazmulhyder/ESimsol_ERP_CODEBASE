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
    public class FabricQtyAllowService : MarshalByRefObject, IFabricQtyAllowService
    {
        #region Private functions and declaration
        private FabricQtyAllow MapObject(NullHandler oReader)
        {
            FabricQtyAllow oFabricQtyAllow = new FabricQtyAllow();
            oFabricQtyAllow.FabricQtyAllowID = oReader.GetInt32("FabricQtyAllowID");
            oFabricQtyAllow.AllowType = (EnumFabricQtyAllowType)oReader.GetInt32("AllowType");
            oFabricQtyAllow.Qty_From = oReader.GetDouble("Qty_From");
            oFabricQtyAllow.Qty_To = oReader.GetDouble("Qty_To");
            oFabricQtyAllow.Percentage = oReader.GetDouble("Percentage");
            oFabricQtyAllow.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oFabricQtyAllow.LastUpdateDateTime = Convert.ToDateTime(oReader.GetDateTime("LastUpdateDateTime"));
            oFabricQtyAllow.LastUpdateByName = oReader.GetString("LastUpdateByName");
            oFabricQtyAllow.MUnitName = oReader.GetString("MUnitName");
            oFabricQtyAllow.Note = oReader.GetString("Note");
            oFabricQtyAllow.MunitID = oReader.GetInt32("MUnitID");
            oFabricQtyAllow.AllowTypeInInt = oReader.GetInt32("AllowType");
            oFabricQtyAllow.OrderType = (EnumFabricRequestType)oReader.GetInt32("OrderType");
            oFabricQtyAllow.OrderTypeInt = oReader.GetInt32("OrderType");
            oFabricQtyAllow.WarpWeftType = (EnumWarpWeft)oReader.GetInt32("WarpWeftType");
            oFabricQtyAllow.WarpWeftTypeInt = oReader.GetInt32("WarpWeftType");

            return oFabricQtyAllow;
        }

        private FabricQtyAllow CreateObject(NullHandler oReader)
        {
            FabricQtyAllow oFabricQtyAllow = new FabricQtyAllow();
            oFabricQtyAllow = MapObject(oReader);
            return oFabricQtyAllow;
        }

        private List<FabricQtyAllow> CreateObjects(IDataReader oReader)
        {
            List<FabricQtyAllow> oFabricQtyAllow = new List<FabricQtyAllow>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricQtyAllow oItem = CreateObject(oHandler);
                oFabricQtyAllow.Add(oItem);
            }
            return oFabricQtyAllow;
        }

        #endregion

        #region Interface implementation
        public FabricQtyAllowService() { }

        public FabricQtyAllow Save(FabricQtyAllow oFabricQtyAllow, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricQtyAllow.FabricQtyAllowID <= 0)
                {
                    reader = FabricQtyAllowDA.InsertUpdate(tc, oFabricQtyAllow, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricQtyAllowDA.InsertUpdate(tc, oFabricQtyAllow, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricQtyAllow = new FabricQtyAllow();
                    oFabricQtyAllow = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save FabricQtyAllow. Because of " + e.Message, e);
                #endregion
            }
            return oFabricQtyAllow;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricQtyAllow oFabricQtyAllow = new FabricQtyAllow();
                oFabricQtyAllow.FabricQtyAllowID = id;
                FabricQtyAllowDA.Delete(tc, oFabricQtyAllow, EnumDBOperation.Delete, nUserId);
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

        public List<FabricQtyAllow> Gets(Int64 nUserID)
        {
            List<FabricQtyAllow> oFabricQtyAllows = new List<FabricQtyAllow>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricQtyAllowDA.Gets(tc);
                oFabricQtyAllows = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricQtyAllow", e);
                #endregion
            }
            return oFabricQtyAllows;
        }
        public List<FabricQtyAllow> Gets(string sSQL,Int64 nUserID)
        {
            List<FabricQtyAllow> oFabricQtyAllows = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricQtyAllowDA.Gets(tc,sSQL);
                oFabricQtyAllows = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricQtyAllow", e);
                #endregion
            }
            return oFabricQtyAllows;
        }
        public FabricQtyAllow Get(int nId, Int64 nUserId) 
        {
            FabricQtyAllow oFabricQtyAllow = new FabricQtyAllow();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricQtyAllowDA.Get(tc, nId); 
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricQtyAllow = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricQtyAllow", e);
                #endregion
            }
            return oFabricQtyAllow;
        }
        public List<FabricQtyAllow> Getsby(int nFSCD, Int64 nUserID)
        {
            int nOrderType = 0;
            string sSql = "";
            List<FabricQtyAllow> oFabricQtyAllows = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                 nOrderType = FabricQtyAllowDA.GetsFabricOrderType(tc, nFSCD);
                IDataReader reader = null;
                sSql = "SELECT * FROM FabricQtyAllow ";
                if(nOrderType==(int)EnumFabricRequestType.Bulk)
                {
                    sSql = sSql + " where ordertype="+(int)EnumFabricRequestType.Bulk;
                }
                else
                {
                    sSql = sSql + " where ordertype!=" + (int)EnumFabricRequestType.Bulk;
                }
                reader = FabricQtyAllowDA.Gets(tc, sSql);
                oFabricQtyAllows = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricQtyAllow", e);
                #endregion
            }
            return oFabricQtyAllows;
        }
        #endregion
    }   
}
