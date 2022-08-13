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

    public class CostSheetDetailService : MarshalByRefObject, ICostSheetDetailService
    {
        #region Private functions and declaration
        private CostSheetDetail MapObject(NullHandler oReader)
        {
            CostSheetDetail oCostSheetDetail = new CostSheetDetail();
            oCostSheetDetail.CostSheetDetailID = oReader.GetInt32("CostSheetDetailID");
            oCostSheetDetail.CostSheetID = oReader.GetInt32("CostSheetID");
            oCostSheetDetail.CostSheetLogDetailID = oReader.GetInt32("CostSheetLogDetailID");
            oCostSheetDetail.CostSheetLogID = oReader.GetInt32("CostSheetLogID");
            oCostSheetDetail.MaterialType = (EnumCostSheetMeterialType)oReader.GetInt32("MaterialType");
            oCostSheetDetail.MaterialTypeInInt = oReader.GetInt32("MaterialType");
            oCostSheetDetail.MaterialID = oReader.GetInt32("MaterialID");
            oCostSheetDetail.Sequence = oReader.GetInt32("Sequence");
            oCostSheetDetail.ProductCode = oReader.GetString("ProductCode");
            oCostSheetDetail.ProductName = oReader.GetString("ProductName");
            oCostSheetDetail.Ply = oReader.GetString("Ply");
            oCostSheetDetail.MaterialMarketPrice = oReader.GetDouble("MaterialMarketPrice");
            oCostSheetDetail.UsePercentage = oReader.GetDouble("UsePercentage");
            oCostSheetDetail.EstimatedCost = oReader.GetDouble("EstimatedCost");
            oCostSheetDetail.WastagePercentPerMaterial = oReader.GetDouble("WastagePercentPerMaterial");
            oCostSheetDetail.ActualGarmentsWeight = oReader.GetDouble("ActualGarmentsWeight");
            oCostSheetDetail.ActualProcessLoss = oReader.GetDouble("ActualProcessLoss");
            oCostSheetDetail.Description = oReader.GetString("Description");
            oCostSheetDetail.Width = oReader.GetString("Width");
            oCostSheetDetail.UnitID = oReader.GetInt32("UnitID");
            oCostSheetDetail.UnitName = oReader.GetString("UnitName");
            oCostSheetDetail.UnitSymbol = oReader.GetString("UnitSymbol");
            oCostSheetDetail.Consumption = oReader.GetDouble("Consumption");
            oCostSheetDetail.DyeingCost = oReader.GetDouble("DyeingCost");
            oCostSheetDetail.LycraCost = oReader.GetDouble("LycraCost");
            oCostSheetDetail.AOPCost = oReader.GetDouble("AOPCost");
            oCostSheetDetail.KnittingCost = oReader.GetDouble("KnittingCost");
            oCostSheetDetail.WashCost = oReader.GetDouble("WashCost");
            oCostSheetDetail.YarnDyeingCost = oReader.GetDouble("YarnDyeingCost");
            oCostSheetDetail.SuedeCost = oReader.GetDouble("SuedeCost");
            oCostSheetDetail.FinishingCost = oReader.GetDouble("FinishingCost");
            oCostSheetDetail.BrushingCost = oReader.GetDouble("BrushingCost");
            oCostSheetDetail.RateUnit = oReader.GetInt32("RateUnit");
            

            return oCostSheetDetail;
        }

        private CostSheetDetail CreateObject(NullHandler oReader)
        {
            CostSheetDetail oCostSheetDetail = new CostSheetDetail();
            oCostSheetDetail = MapObject(oReader);
            return oCostSheetDetail;
        }

        private List<CostSheetDetail> CreateObjects(IDataReader oReader)
        {
            List<CostSheetDetail> oCostSheetDetail = new List<CostSheetDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CostSheetDetail oItem = CreateObject(oHandler);
                oCostSheetDetail.Add(oItem);
            }
            return oCostSheetDetail;
        }

        #endregion

        #region Interface implementation
        public CostSheetDetailService() { }

        public CostSheetDetail Save(CostSheetDetail oCostSheetDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<CostSheetDetail> _oCostSheetDetails = new List<CostSheetDetail>();
            oCostSheetDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CostSheetDetailDA.InsertUpdate(tc, oCostSheetDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostSheetDetail = new CostSheetDetail();
                    oCostSheetDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oCostSheetDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oCostSheetDetail;
        }


        public CostSheetDetail Get(int CostSheetDetailID, Int64 nUserId)
        {
            CostSheetDetail oAccountHead = new CostSheetDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CostSheetDetailDA.Get(tc, CostSheetDetailID);
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
                throw new ServiceException("Failed to Get CostSheetDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<CostSheetDetail> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<CostSheetDetail> oCostSheetDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetDetailDA.Gets(LabDipOrderID, tc);
                oCostSheetDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSheetDetail", e);
                #endregion
            }

            return oCostSheetDetail;
        }


        public List<CostSheetDetail> GetActualSheet(int SaleOrderID, Int64 nUserID)
        {
            List<CostSheetDetail> oCostSheetDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetDetailDA.GetActualSheet(SaleOrderID, tc);
                oCostSheetDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSheetDetail", e);
                #endregion
            }

            return oCostSheetDetail;
        }

        public List<CostSheetDetail> GetsCostSheetLog(int CostSheetLogID, Int64 nUserID)
        {
            List<CostSheetDetail> oCostSheetDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetDetailDA.GetsCostSheetLog(CostSheetLogID, tc);
                oCostSheetDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSheetDetail", e);
                #endregion
            }

            return oCostSheetDetail;
        }


        public List<CostSheetDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<CostSheetDetail> oCostSheetDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetDetailDA.Gets(tc, sSQL);
                oCostSheetDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSheetDetail", e);
                #endregion
            }

            return oCostSheetDetail;
        }
        #endregion
    }


    
}
