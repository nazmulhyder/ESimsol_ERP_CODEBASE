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

    public class ProductionExecutionPlanDetailBreakdownService : MarshalByRefObject, IProductionExecutionPlanDetailBreakdownService
    {
        #region Private functions and declaration
        private ProductionExecutionPlanDetailBreakdown MapObject(NullHandler oReader)
        {
            ProductionExecutionPlanDetailBreakdown oPEPDB = new ProductionExecutionPlanDetailBreakdown();
            oPEPDB.ProductionExecutionPlanDetailBreakdownID = oReader.GetInt32("ProductionExecutionPlanDetailBreakdownID");
            oPEPDB.ProductionExecutionPlanDetailID = oReader.GetInt32("ProductionExecutionPlanDetailID");
            oPEPDB.WorkingDate = oReader.GetDateTime("WorkingDate");
            oPEPDB.RegularTime = oReader.GetDouble("RegularTime");
            oPEPDB.OverTime = oReader.GetDouble("OverTime");
            oPEPDB.DailyProduction = oReader.GetDouble("DailyProduction");
            oPEPDB.RecapQty = oReader.GetDouble("RecapQty");
            oPEPDB.ExecutionQty = oReader.GetDouble("ExecutionQty");
            oPEPDB.EfficencyInParcent = oReader.GetDouble("EfficencyInParcent");
            oPEPDB.PLineConfigureID = oReader.GetInt32("PLineConfigureID");
            oPEPDB.BUID= oReader.GetInt32("BUID");
            oPEPDB.ProductionUnitID = oReader.GetInt32("ProductionUnitID");
            oPEPDB.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oPEPDB.ProductionExecutionPlanID = oReader.GetInt32("ProductionExecutionPlanID");
            oPEPDB.RecapNo = oReader.GetString("RecapNo");
            oPEPDB.StyleNo = oReader.GetString("StyleNo");
            oPEPDB.BuyerName = oReader.GetString("BuyerName");
            oPEPDB.UnitSymbol = oReader.GetString("UnitSymbol");            
            return oPEPDB;
        }

        private ProductionExecutionPlanDetailBreakdown CreateObject(NullHandler oReader)
        {
            ProductionExecutionPlanDetailBreakdown oProductionExecutionPlanDetailBreakdown = new ProductionExecutionPlanDetailBreakdown();
            oProductionExecutionPlanDetailBreakdown = MapObject(oReader);
            return oProductionExecutionPlanDetailBreakdown;
        }

        private List<ProductionExecutionPlanDetailBreakdown> CreateObjects(IDataReader oReader)
        {
            List<ProductionExecutionPlanDetailBreakdown> oProductionExecutionPlanDetailBreakdown = new List<ProductionExecutionPlanDetailBreakdown>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionExecutionPlanDetailBreakdown oItem = CreateObject(oHandler);
                oProductionExecutionPlanDetailBreakdown.Add(oItem);
            }
            return oProductionExecutionPlanDetailBreakdown;
        }

        #endregion

        #region Interface implementation
        public ProductionExecutionPlanDetailBreakdownService() { }

        public ProductionExecutionPlanDetailBreakdown Save(ProductionExecutionPlanDetailBreakdown oProductionExecutionPlanDetailBreakdown, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<ProductionExecutionPlanDetailBreakdown> _oProductionExecutionPlanDetailBreakdowns = new List<ProductionExecutionPlanDetailBreakdown>();
            oProductionExecutionPlanDetailBreakdown.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ProductionExecutionPlanDetailBreakdownDA.InsertUpdate(tc, oProductionExecutionPlanDetailBreakdown, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionExecutionPlanDetailBreakdown = new ProductionExecutionPlanDetailBreakdown();
                    oProductionExecutionPlanDetailBreakdown = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProductionExecutionPlanDetailBreakdown.ErrorMessage = e.Message;
                #endregion
            }
            return oProductionExecutionPlanDetailBreakdown;
        }
        public string Paste(ProductionExecutionPlanDetailBreakdown oProductionExecutionPlanDetailBreakdown, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sReturnMessage = "sucessfull";
            try
            {

                tc = TransactionContext.Begin(true);
                ProductionExecutionPlanDetailBreakdownDA.Paste(tc, oProductionExecutionPlanDetailBreakdown, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                sReturnMessage = e.Message.Split('~')[0];
                #endregion
            }
            return sReturnMessage;
        }

        public ProductionExecutionPlanDetailBreakdown Get(int ProductionExecutionPlanDetailBreakdownID, Int64 nUserId)
        {
            ProductionExecutionPlanDetailBreakdown oAccountHead = new ProductionExecutionPlanDetailBreakdown();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionExecutionPlanDetailBreakdownDA.Get(tc, ProductionExecutionPlanDetailBreakdownID);
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
                throw new ServiceException("Failed to Get ProductionExecutionPlanDetailBreakdown", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ProductionExecutionPlanDetailBreakdown> Gets(int ProductionExecutionPlanDetailID, Int64 nUserID)
        {
            List<ProductionExecutionPlanDetailBreakdown> oProductionExecutionPlanDetailBreakdown = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionExecutionPlanDetailBreakdownDA.Gets(ProductionExecutionPlanDetailID, tc);
                oProductionExecutionPlanDetailBreakdown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionExecutionPlanDetailBreakdown", e);
                #endregion
            }

            return oProductionExecutionPlanDetailBreakdown;
        }

        public List<ProductionExecutionPlanDetailBreakdown> GetsByPEPPlanID(int nProductionExecutionPlanID, Int64 nUserID)
        {
            List<ProductionExecutionPlanDetailBreakdown> oProductionExecutionPlanDetailBreakdown = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductionExecutionPlanDetailBreakdownDA.GetsByPEPPlanID(nProductionExecutionPlanID, tc);
                oProductionExecutionPlanDetailBreakdown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionExecutionPlanDetailBreakdown", e);
                #endregion
            }

            return oProductionExecutionPlanDetailBreakdown;
        }

        

        public List<ProductionExecutionPlanDetailBreakdown> Gets(string sSQL, Int64 nUserID)
        {
            List<ProductionExecutionPlanDetailBreakdown> oProductionExecutionPlanDetailBreakdown = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionExecutionPlanDetailBreakdownDA.Gets(tc, sSQL);
                oProductionExecutionPlanDetailBreakdown = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionExecutionPlanDetailBreakdown", e);
                #endregion
            }

            return oProductionExecutionPlanDetailBreakdown;
        }
        #endregion
    }
 
 
}
