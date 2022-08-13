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

    public class StyleBudgetDetailService : MarshalByRefObject, IStyleBudgetDetailService
    {
        #region Private functions and declaration
        private StyleBudgetDetail MapObject(NullHandler oReader)
        {
            StyleBudgetDetail oStyleBudgetDetail = new StyleBudgetDetail();
            oStyleBudgetDetail.StyleBudgetDetailID = oReader.GetInt32("StyleBudgetDetailID");
            oStyleBudgetDetail.StyleBudgetID = oReader.GetInt32("StyleBudgetID");
            oStyleBudgetDetail.StyleBudgetLogDetailID = oReader.GetInt32("StyleBudgetLogDetailID");
            oStyleBudgetDetail.StyleBudgetLogID = oReader.GetInt32("StyleBudgetLogID");
            oStyleBudgetDetail.MaterialType = (EnumCostSheetMeterialType)oReader.GetInt32("MaterialType");
            oStyleBudgetDetail.MaterialTypeInInt = oReader.GetInt32("MaterialType");
            oStyleBudgetDetail.MaterialID = oReader.GetInt32("MaterialID");
            oStyleBudgetDetail.Sequence = oReader.GetInt32("Sequence");
            oStyleBudgetDetail.ProductCode = oReader.GetString("ProductCode");
            oStyleBudgetDetail.ProductName = oReader.GetString("ProductName");
            oStyleBudgetDetail.Ply = oReader.GetString("Ply");
            oStyleBudgetDetail.MaterialMarketPrice = oReader.GetDouble("MaterialMarketPrice");
            oStyleBudgetDetail.UsePercentage = oReader.GetDouble("UsePercentage");
            oStyleBudgetDetail.EstimatedCost = oReader.GetDouble("EstimatedCost");
            oStyleBudgetDetail.WastagePercentPerMaterial = oReader.GetDouble("WastagePercentPerMaterial");
            oStyleBudgetDetail.ActualGarmentsWeight = oReader.GetDouble("ActualGarmentsWeight");
            oStyleBudgetDetail.ActualProcessLoss = oReader.GetDouble("ActualProcessLoss");
            oStyleBudgetDetail.Description = oReader.GetString("Description");
            oStyleBudgetDetail.Width = oReader.GetString("Width");
            oStyleBudgetDetail.UnitID = oReader.GetInt32("UnitID");
            oStyleBudgetDetail.UnitName = oReader.GetString("UnitName");
            oStyleBudgetDetail.UnitSymbol = oReader.GetString("UnitSymbol");
            oStyleBudgetDetail.Consumption = oReader.GetDouble("Consumption");
            oStyleBudgetDetail.DyeingCost = oReader.GetDouble("DyeingCost");
            oStyleBudgetDetail.LycraCost = oReader.GetDouble("LycraCost");
            oStyleBudgetDetail.AOPCost = oReader.GetDouble("AOPCost");
            oStyleBudgetDetail.KnittingCost = oReader.GetDouble("KnittingCost");
            oStyleBudgetDetail.WashCost = oReader.GetDouble("WashCost");
            oStyleBudgetDetail.YarnDyeingCost = oReader.GetDouble("YarnDyeingCost");
            oStyleBudgetDetail.SuedeCost = oReader.GetDouble("SuedeCost");
            oStyleBudgetDetail.FinishingCost = oReader.GetDouble("FinishingCost");
            oStyleBudgetDetail.BrushingCost = oReader.GetDouble("BrushingCost");
            oStyleBudgetDetail.RateUnit = oReader.GetInt32("RateUnit");
            

            return oStyleBudgetDetail;
        }

        private StyleBudgetDetail CreateObject(NullHandler oReader)
        {
            StyleBudgetDetail oStyleBudgetDetail = new StyleBudgetDetail();
            oStyleBudgetDetail = MapObject(oReader);
            return oStyleBudgetDetail;
        }

        private List<StyleBudgetDetail> CreateObjects(IDataReader oReader)
        {
            List<StyleBudgetDetail> oStyleBudgetDetail = new List<StyleBudgetDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                StyleBudgetDetail oItem = CreateObject(oHandler);
                oStyleBudgetDetail.Add(oItem);
            }
            return oStyleBudgetDetail;
        }

        #endregion

        #region Interface implementation
        public StyleBudgetDetailService() { }

        public StyleBudgetDetail Save(StyleBudgetDetail oStyleBudgetDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<StyleBudgetDetail> _oStyleBudgetDetails = new List<StyleBudgetDetail>();
            oStyleBudgetDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = StyleBudgetDetailDA.InsertUpdate(tc, oStyleBudgetDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oStyleBudgetDetail = new StyleBudgetDetail();
                    oStyleBudgetDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oStyleBudgetDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oStyleBudgetDetail;
        }


        public StyleBudgetDetail Get(int StyleBudgetDetailID, Int64 nUserId)
        {
            StyleBudgetDetail oAccountHead = new StyleBudgetDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = StyleBudgetDetailDA.Get(tc, StyleBudgetDetailID);
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
                throw new ServiceException("Failed to Get StyleBudgetDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<StyleBudgetDetail> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<StyleBudgetDetail> oStyleBudgetDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = StyleBudgetDetailDA.Gets(LabDipOrderID, tc);
                oStyleBudgetDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StyleBudgetDetail", e);
                #endregion
            }

            return oStyleBudgetDetail;
        }


        public List<StyleBudgetDetail> GetActualSheet(int SaleOrderID, Int64 nUserID)
        {
            List<StyleBudgetDetail> oStyleBudgetDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = StyleBudgetDetailDA.GetActualSheet(SaleOrderID, tc);
                oStyleBudgetDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StyleBudgetDetail", e);
                #endregion
            }

            return oStyleBudgetDetail;
        }

        public List<StyleBudgetDetail> GetsStyleBudgetLog(int StyleBudgetLogID, Int64 nUserID)
        {
            List<StyleBudgetDetail> oStyleBudgetDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = StyleBudgetDetailDA.GetsStyleBudgetLog(StyleBudgetLogID, tc);
                oStyleBudgetDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StyleBudgetDetail", e);
                #endregion
            }

            return oStyleBudgetDetail;
        }


        public List<StyleBudgetDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<StyleBudgetDetail> oStyleBudgetDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = StyleBudgetDetailDA.Gets(tc, sSQL);
                oStyleBudgetDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StyleBudgetDetail", e);
                #endregion
            }

            return oStyleBudgetDetail;
        }
        #endregion
    }


    
}
