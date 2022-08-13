using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class InvoiceLotService : MarshalByRefObject, IInvoiceLotService
    {
        #region Private functions and declaration
        private InvoiceLot MapObject(NullHandler oReader)
        {
            InvoiceLot oInvoiceLot = new InvoiceLot();
            oInvoiceLot.InvoiceLotID = oReader.GetInt32("InvoiceLotID");
            oInvoiceLot.PurchaseInvoiceDetailID = oReader.GetInt32("PurchaseInvoiceDetailID");
            oInvoiceLot.SerialNo = oReader.GetString("SerialNo");
            oInvoiceLot.EngineNo = oReader.GetString("EngineNo");
            oInvoiceLot.AlternatorNo = oReader.GetString("AlternatorNo");
            oInvoiceLot.ModuleNo = oReader.GetString("ModuleNo");
            oInvoiceLot.Others = oReader.GetString("Others");
            oInvoiceLot.GRNDetailID = oReader.GetInt32("GRNDetailID");
            oInvoiceLot.InvoiceProductName = oReader.GetString("InvoiceProductName");
            return oInvoiceLot;
        }

        private InvoiceLot CreateObject(NullHandler oReader)
        {
            InvoiceLot oInvoiceLot = new InvoiceLot();
            oInvoiceLot = MapObject(oReader);
            return oInvoiceLot;
        }

        private List<InvoiceLot> CreateObjects(IDataReader oReader)
        {
            List<InvoiceLot> oInvoiceLots = new List<InvoiceLot>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                InvoiceLot oItem = CreateObject(oHandler);
                oInvoiceLots.Add(oItem);
            }
            return oInvoiceLots;
        }
        #endregion

        #region Interface implementation
        public InvoiceLotService() { }

        #region New Version
        public InvoiceLot Get(int nInvoiceLotID, Int64 nUserId)
        {
            InvoiceLot oInvoiceLot = new InvoiceLot();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = InvoiceLotDA.Get(tc, nInvoiceLotID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oInvoiceLot = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Invoice Lot", e);
                #endregion
            }

            return oInvoiceLot;
        }

        public List<InvoiceLot> Gets(int nInvoiceDetailID, Int64 nUserId)
        {
            List<InvoiceLot> oInvoiceLots = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InvoiceLotDA.Gets(nInvoiceDetailID, tc);
                oInvoiceLots = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Purchase Invoice Products", e);
                #endregion
            }
            return oInvoiceLots;
        }

        public List<InvoiceLot> Gets(string sSQL, Int64 nUserId)
        {
            List<InvoiceLot> oInvoiceLot = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InvoiceLotDA.Gets(tc, sSQL);
                oInvoiceLot = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get InvoiceLot", e);
                #endregion
            }

            return oInvoiceLot;
        }

        public List<InvoiceLot> GetsByInvoice(int nInvoiceId, Int64 nUserId)
        {
            List<InvoiceLot> oInvoiceLots = new List<InvoiceLot>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InvoiceLotDA.GetsByInvoice(tc, nInvoiceId);
                oInvoiceLots = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Invoice Lot", e);
                #endregion
            }

            return oInvoiceLots;
        }

        #endregion

        public string Delete(InvoiceLot oInvoiceLot, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                InvoiceLotDA.Delete(tc, oInvoiceLot, EnumDBOperation.Delete, nUserID, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;                
                #endregion
            }
            return Global.DeleteMessage;
        }
        public List<InvoiceLot> Saves(List<InvoiceLot> oInvoiceLots, Int64 nUserID)
        {
            List<InvoiceLot> oTempInvoiceLots = new List<InvoiceLot>();
            InvoiceLot oInvoiceLot = new InvoiceLot();
            string sInvoiceLotIDs = ""; int nInvoiceDetailID =0;
            TransactionContext tc = null; 
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (InvoiceLot oItem in oInvoiceLots)
                {
                    nInvoiceDetailID = oItem.PurchaseInvoiceDetailID;
                    IDataReader reader;
                    if (oItem.InvoiceLotID <= 0)
                    {
                        reader = InvoiceLotDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = InvoiceLotDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oInvoiceLot = new InvoiceLot();
                        oInvoiceLot = CreateObject(oReader);
                        oTempInvoiceLots.Add(oInvoiceLot);
                        sInvoiceLotIDs = sInvoiceLotIDs + oInvoiceLot.InvoiceLotID.ToString() + ",";
                    }
                    reader.Close();
                }

                if (sInvoiceLotIDs.Length > 0)
                {
                    sInvoiceLotIDs = sInvoiceLotIDs.Remove(sInvoiceLotIDs.Length - 1, 1);
                }

                oInvoiceLot = new InvoiceLot();
                oInvoiceLot.PurchaseInvoiceDetailID = nInvoiceDetailID;
                InvoiceLotDA.Delete(tc, oInvoiceLot, EnumDBOperation.Delete, nUserID, sInvoiceLotIDs);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTempInvoiceLots = new List<InvoiceLot>();
                oInvoiceLot = new InvoiceLot();
                oInvoiceLot.ErrorMessage = e.Message.Split('~')[0];
                oTempInvoiceLots.Add(oInvoiceLot);
                #endregion
            }
            return oTempInvoiceLots;
        }

        public InvoiceLot Save(InvoiceLot oInvoiceLot, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oInvoiceLot.InvoiceLotID <= 0)
                {
                    reader = InvoiceLotDA.InsertUpdate(tc, oInvoiceLot, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = InvoiceLotDA.InsertUpdate(tc, oInvoiceLot, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oInvoiceLot = new InvoiceLot();
                    oInvoiceLot = CreateObject(oReader);                    
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                
                oInvoiceLot = new InvoiceLot();
                oInvoiceLot.ErrorMessage = e.Message.Split('~')[0];                
                #endregion
            }
            return oInvoiceLot;
        }

        #endregion
    }
}
