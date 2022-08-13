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
    public class FNOrderFabricTransferService : MarshalByRefObject, IFNOrderFabricTransferService
    {
        #region Private functions and declaration
        private FNOrderFabricTransfer MapObject(NullHandler oReader)
        {
            FNOrderFabricTransfer oFNOrderFabricTransfer = new FNOrderFabricTransfer();
            oFNOrderFabricTransfer.FNOrderFabricTransferID = oReader.GetInt32("FNOrderFabricTransferID");
            oFNOrderFabricTransfer.FSCDID_From = oReader.GetInt32("FSCDID_From");
            oFNOrderFabricTransfer.FSCDID_To = oReader.GetInt32("FSCDID_To");
            oFNOrderFabricTransfer.FNOrderFabricReceiveID_From = oReader.GetInt32("FNOrderFabricReceiveID_From");
            oFNOrderFabricTransfer.FNOrderFabricReceiveID_To = oReader.GetInt32("FNOrderFabricReceiveID_To");
            oFNOrderFabricTransfer.Qty = oReader.GetDouble("Qty");
            oFNOrderFabricTransfer.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oFNOrderFabricTransfer.LastUpdateDateTime = Convert.ToDateTime(oReader.GetDateTime("LastUpdateDateTime"));
            oFNOrderFabricTransfer.LastUpdateByName = oReader.GetString("LastUpdateByName");
            oFNOrderFabricTransfer.Dispo_From = oReader.GetString("Dispo_From");
            oFNOrderFabricTransfer.Dispo_To = oReader.GetString("Dispo_To");
            oFNOrderFabricTransfer.Lot_From = oReader.GetString("Lot_From");
            oFNOrderFabricTransfer.Lot_To = oReader.GetString("Lot_To");
            oFNOrderFabricTransfer.Fabric_From = oReader.GetString("Fabric_From");
            return oFNOrderFabricTransfer;
        }
        private FNOrderFabricTransfer CreateObject(NullHandler oReader)
        {
            FNOrderFabricTransfer oFNOrderFabricTransfer = new FNOrderFabricTransfer();
            oFNOrderFabricTransfer = MapObject(oReader);
            return oFNOrderFabricTransfer;
        }

        private List<FNOrderFabricTransfer> CreateObjects(IDataReader oReader)
        {
            List<FNOrderFabricTransfer> oFNOrderFabricTransfer = new List<FNOrderFabricTransfer>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNOrderFabricTransfer oItem = CreateObject(oHandler);
                oFNOrderFabricTransfer.Add(oItem);
            }
            return oFNOrderFabricTransfer;
        }
        #endregion
        #region Interface implementation
        public FNOrderFabricTransfer Save(FNOrderFabricTransfer oFNOrderFabricTransfer, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFNOrderFabricTransfer.FNOrderFabricTransferID <= 0)
                {

                    reader = FNOrderFabricTransferDA.InsertUpdate(tc, oFNOrderFabricTransfer, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FNOrderFabricTransferDA.InsertUpdate(tc, oFNOrderFabricTransfer, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNOrderFabricTransfer = new FNOrderFabricTransfer();
                    oFNOrderFabricTransfer = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oFNOrderFabricTransfer = new FNOrderFabricTransfer();
                    oFNOrderFabricTransfer.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFNOrderFabricTransfer;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNOrderFabricTransfer oFNOrderFabricTransfer = new FNOrderFabricTransfer();
                oFNOrderFabricTransfer.FNOrderFabricTransferID = id;
                DBTableReferenceDA.HasReference(tc, "FNOrderFabricTransfer", id);
                FNOrderFabricTransferDA.Delete(tc, oFNOrderFabricTransfer, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public FNOrderFabricTransfer Get(int id, Int64 nUserId)
        {
            FNOrderFabricTransfer oFNOrderFabricTransfer = new FNOrderFabricTransfer();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FNOrderFabricTransferDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNOrderFabricTransfer = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FNOrderFabricTransfer", e);
                #endregion
            }
            return oFNOrderFabricTransfer;
        }
        public List<FNOrderFabricTransfer> Gets(Int64 nUserID)
        {
            List<FNOrderFabricTransfer> oFNOrderFabricTransfers = new List<FNOrderFabricTransfer>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNOrderFabricTransferDA.Gets(tc);
                oFNOrderFabricTransfers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FNOrderFabricTransfer oFNOrderFabricTransfer = new FNOrderFabricTransfer();
                oFNOrderFabricTransfer.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFNOrderFabricTransfers;
        }
        public List<FNOrderFabricTransfer> Gets(string sSQL, Int64 nUserID)
        {
            List<FNOrderFabricTransfer> oFNOrderFabricTransfers = new List<FNOrderFabricTransfer>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNOrderFabricTransferDA.Gets(tc, sSQL);
                oFNOrderFabricTransfers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNOrderFabricTransfer", e);
                #endregion
            }
            return oFNOrderFabricTransfers;
        }
        public List<FNOrderFabricTransfer> SaveList(List<FNOrderFabricTransfer> oFNOrderFabricTransfers, Int64 nUserID)
        {
            FNOrderFabricTransfer oFNOrderFabricTransfer = new FNOrderFabricTransfer();
            List<FNOrderFabricTransfer> _oFNOrderFabricTransfers = new List<FNOrderFabricTransfer>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FNOrderFabricTransfer oTempFNOrderFabricTransfer in oFNOrderFabricTransfers)
                {
                    IDataReader reader;
                    if (oTempFNOrderFabricTransfer.FNOrderFabricTransferID <= 0)
                    {
                        reader = FNOrderFabricTransferDA.InsertUpdate(tc, oTempFNOrderFabricTransfer, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FNOrderFabricTransferDA.InsertUpdate(tc, oTempFNOrderFabricTransfer, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNOrderFabricTransfer = new FNOrderFabricTransfer();
                        oFNOrderFabricTransfer = CreateObject(oReader);
                    }
                    reader.Close();
                    _oFNOrderFabricTransfers.Add(oFNOrderFabricTransfer);
                }
                tc.End();

            }
            catch (Exception e)
            {
                if (tc != null)
                {
                    tc.HandleError();
                    oFNOrderFabricTransfer = new FNOrderFabricTransfer();
                    oFNOrderFabricTransfer.ErrorMessage = e.Message;
                    _oFNOrderFabricTransfers = new List<FNOrderFabricTransfer>();
                    _oFNOrderFabricTransfers.Add(oFNOrderFabricTransfer);
                }
            }
            return _oFNOrderFabricTransfers;
        }
        public List<FNOrderFabricTransfer> ReturnFabrics(List<FNOrderFabricTransfer> oFNOrderFabricTransfers, Int64 nUserID)
        {
            FNOrderFabricTransfer oFNOrderFabricTransfer = new FNOrderFabricTransfer();
            List<FNOrderFabricTransfer> _oFNOrderFabricTransfers = new List<FNOrderFabricTransfer>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FNOrderFabricTransfer oTempFNOrderFabricTransfer in oFNOrderFabricTransfers)
                {
                    IDataReader reader = null;
                    if (oTempFNOrderFabricTransfer.FNOrderFabricReceiveID_From > 0 && oTempFNOrderFabricTransfer.Qty>0)
                    {
                        reader = FNOrderFabricTransferDA.InsertUpdate(tc, oTempFNOrderFabricTransfer, EnumDBOperation.Return, nUserID);
                    }         
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNOrderFabricTransfer = new FNOrderFabricTransfer();
                        oFNOrderFabricTransfer = CreateObject(oReader);
                    }
                    reader.Close();
                    _oFNOrderFabricTransfers.Add(oFNOrderFabricTransfer);
                }
                tc.End();

            }
            catch (Exception e)
            {
                if (tc != null)
                {
                    tc.HandleError();
                    oFNOrderFabricTransfer = new FNOrderFabricTransfer();
                    oFNOrderFabricTransfer.ErrorMessage = e.Message;
                    _oFNOrderFabricTransfers = new List<FNOrderFabricTransfer>();
                    _oFNOrderFabricTransfers.Add(oFNOrderFabricTransfer);
                }
            }
            return _oFNOrderFabricTransfers;
        }

        #endregion
    }
}
