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
    public class GRNLandingCostService : MarshalByRefObject, IGRNLandingCostService
    {
        #region Private functions and declaration
        private GRNLandingCost MapObject(NullHandler oReader)
        {
            GRNLandingCost oGRNLandingCost = new GRNLandingCost();
            oGRNLandingCost.GRNID = oReader.GetInt32("GRNID");
            oGRNLandingCost.GRNDetailID = oReader.GetInt32("GRNDetailID");
            oGRNLandingCost.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oGRNLandingCost.ProductID = oReader.GetInt32("ProductID");
            oGRNLandingCost.Qty = oReader.GetDouble("Qty");
            oGRNLandingCost.TotalInvoiceAmount = oReader.GetDouble("TotalInvoiceAmount");
            oGRNLandingCost.GRNEffectedInvoiceAmount = oReader.GetDouble("GRNEffectedInvoiceAmount");
            oGRNLandingCost.UnitPrice = oReader.GetDouble("UnitPrice");
            oGRNLandingCost.GRNAmount = oReader.GetDouble("GRNAmount");
            oGRNLandingCost.PurchaseInvoiceID = oReader.GetInt32("PurchaseInvoiceID");
            oGRNLandingCost.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oGRNLandingCost.AccountHeadName = oReader.GetString("AccountHeadName");
            oGRNLandingCost.CurrencyID = oReader.GetInt32("CurrencyID");
            oGRNLandingCost.AmountInCurrency = oReader.GetDouble("AmountInCurrency");
            oGRNLandingCost.CRate = oReader.GetDouble("CRate");
            oGRNLandingCost.TotalLandingAmount = oReader.GetDouble("TotalLandingAmount");
            oGRNLandingCost.HeadWiseLandingCost = oReader.GetDouble("HeadWiseLandingCost");
            oGRNLandingCost.ProductName = oReader.GetString("ProductName");
            oGRNLandingCost.ImportLCNo = oReader.GetString("ImportLCNo");
            oGRNLandingCost.ImportLCID = oReader.GetInt32("ImportLCID");
            oGRNLandingCost.GRNDate = oReader.GetDateTime("GRNDate");
            return oGRNLandingCost;
        }

        private GRNLandingCost CreateObject(NullHandler oReader)
        {
            GRNLandingCost oGRNLandingCost = new GRNLandingCost();
            oGRNLandingCost = MapObject(oReader);
            return oGRNLandingCost;
        }

        private List<GRNLandingCost> CreateObjects(IDataReader oReader)
        {
            List<GRNLandingCost> oGRNLandingCost = new List<GRNLandingCost>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GRNLandingCost oItem = CreateObject(oHandler);
                oGRNLandingCost.Add(oItem);
            }
            return oGRNLandingCost;
        }

        #endregion

        #region Interface implementation
        public GRNLandingCostService() { }

        public GRNLandingCost Save(GRNLandingCost oGRNLandingCost, Int64 nUserID)
        {
            TransactionContext tc = null;
            oGRNLandingCost.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oGRNLandingCost.GRNID <= 0)
                {
                    reader = GRNLandingCostDA.InsertUpdate(tc, oGRNLandingCost, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = GRNLandingCostDA.InsertUpdate(tc, oGRNLandingCost, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGRNLandingCost = new GRNLandingCost();
                    oGRNLandingCost = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oGRNLandingCost.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save GRNLandingCost. Because of " + e.Message, e);
                #endregion
            }
            return oGRNLandingCost;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GRNLandingCost oGRNLandingCost = new GRNLandingCost();
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "GRNLandingCost", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "GRNLandingCost", id);
                oGRNLandingCost.GRNID = id;
                GRNLandingCostDA.Delete(tc, oGRNLandingCost, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete GRNLandingCost. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public GRNLandingCost Get(int id, Int64 nUserId)
        {
            GRNLandingCost oAccountHead = new GRNLandingCost();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GRNLandingCostDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get GRNLandingCost", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<GRNLandingCost> Gets(Int64 nUserID)
        {
            List<GRNLandingCost> oGRNLandingCost = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GRNLandingCostDA.Gets(tc);
                oGRNLandingCost = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GRNLandingCost", e);
                #endregion
            }

            return oGRNLandingCost;
        }

        public List<GRNLandingCost> Gets(string sSQL, Int64 nUserID)
        {
            List<GRNLandingCost> oGRNLandingCost = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GRNLandingCostDA.Gets(tc, sSQL);
                oGRNLandingCost = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GRNLandingCost", e);
                #endregion
            }

            return oGRNLandingCost;
        }

        #endregion
    }
}
