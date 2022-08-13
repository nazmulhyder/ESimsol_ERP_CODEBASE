using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ExportPIDetailService : MarshalByRefObject, IExportPIDetailService
    {
        #region Private functions and declaration
        private ExportPIDetail MapObject(NullHandler oReader)
        {
            ExportPIDetail oExportPIDetail = new ExportPIDetail();
            oExportPIDetail.ExportPIDetailID = oReader.GetInt32("ExportPIDetailID");
            oExportPIDetail.ExportPILogID = oReader.GetInt32("ExportPILogID");
            oExportPIDetail.ProductID = oReader.GetInt32("ProductID");
            oExportPIDetail.Qty = oReader.GetDouble("Qty");
            oExportPIDetail.MUnitID = oReader.GetInt32("MUnitID");
            oExportPIDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportPIDetail.Amount = oReader.GetDouble("Amount");
            oExportPIDetail.ProductCode = oReader.GetString("ProductCode");
            oExportPIDetail.ProductName = oReader.GetString("ProductName");
            oExportPIDetail.PINo = oReader.GetString("PINo");
            oExportPIDetail.MUName = oReader.GetString("MUName");
            oExportPIDetail.Currency = oReader.GetString("Currency");
            oExportPIDetail.ReferenceCaption = oReader.GetString("ReferenceCaption");
            oExportPIDetail.ColorInfo = oReader.GetString("ColorInfo");
            oExportPIDetail.BuyerReference = oReader.GetString("BuyerReference");
            oExportPIDetail.StyleNo = oReader.GetString("StyleNo");
            oExportPIDetail.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportPIDetail.FabricID = oReader.GetInt32("FabricID");
            oExportPIDetail.PolyMUnitID = oReader.GetInt32("PolyMUnitID");
            oExportPIDetail.ExportPIDetailLogID = oReader.GetInt32("ExportPIDetailLogID");
            oExportPIDetail.AdjQty = oReader.GetDouble("AdjQty");
            oExportPIDetail.AdjRate = oReader.GetDouble("AdjRate");
            oExportPIDetail.DocCharge = oReader.GetDouble("DocCharge");
            oExportPIDetail.AdjValue = oReader.GetDouble("AdjValue");
            oExportPIDetail.CRate = oReader.GetDouble("CRate");
            oExportPIDetail.CRateTwo = oReader.GetDouble("CRateTwo");
            oExportPIDetail.QtyCom = oReader.GetDouble("QtyCom");
            oExportPIDetail.HSCode = oReader.GetString("HSCode");
            oExportPIDetail.ColorID = oReader.GetInt32("ColorID");
            oExportPIDetail.ColorName = oReader.GetString("ColorName");
            oExportPIDetail.ProductDescription = oReader.GetString("ProductDescription");
            oExportPIDetail.SizeName = oReader.GetString("SizeName");
            oExportPIDetail.ModelReferenceID = oReader.GetInt32("ModelReferenceID");
            oExportPIDetail.ModelReferenceName = oReader.GetString("ModelReferenceName");
            oExportPIDetail.Measurement = oReader.GetString("Measurement");
            oExportPIDetail.IssueDate = oReader.GetDateTime("IssueDate");
            oExportPIDetail.OrderSheetDetailID = oReader.GetInt32("OrderSheetDetailID");
            oExportPIDetail.IsApplySizer = oReader.GetBoolean("IsApplySizer");
            oExportPIDetail.YetToProductionOrderQty = oReader.GetDouble("YetToProductionOrderQty");
            oExportPIDetail.ColorQty = oReader.GetInt32("ColorQty");
            oExportPIDetail.YetToDeliveryOrderQty = oReader.GetDouble("YetToDeliveryOrderQty");
            /// derive for Fabric
            oExportPIDetail.FabricNo = oReader.GetString("FabricNo");
            oExportPIDetail.FabricWidth = oReader.GetString("FabricWidth");
            oExportPIDetail.Construction = oReader.GetString("Construction");
            oExportPIDetail.ProcessType = oReader.GetInt32("ProcessType");
            oExportPIDetail.FabricWeave = oReader.GetInt32("FabricWeave");
            oExportPIDetail.FinishType = oReader.GetInt32("FinishType");
            oExportPIDetail.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oExportPIDetail.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oExportPIDetail.FinishTypeName = oReader.GetString("FinishTypeName");
            oExportPIDetail.ExportQualityID = oReader.GetInt32("ExportQualityID");
            oExportPIDetail.ExportQuality = oReader.GetString("ExportQuality");
            oExportPIDetail.FSCNo = oReader.GetString("FSCNo");
            oExportPIDetail.RecipeID = oReader.GetInt32("RecipeID");
            oExportPIDetail.RecipeName = oReader.GetString("RecipeName");
            oExportPIDetail.IsDeduct = oReader.GetBoolean("IsDeduct");
            oExportPIDetail.Shrinkage = oReader.GetString("Shrinkage");
            oExportPIDetail.Weight = oReader.GetString("Weight");

            oExportPIDetail.SaleType = (EnumProductionType)oReader.GetInt16("SaleType");
            oExportPIDetail.ShadeType = (EnumDepthOfShade)oReader.GetInt16("ShadeType");
            oExportPIDetail.DyeingType = oReader.GetInt32("DyeingType");
            oExportPIDetail.PackingType = oReader.GetInt32("PackingType");
            

            return oExportPIDetail;
        }
        private ExportPIDetail CreateObject(NullHandler oReader)
        {
            ExportPIDetail oExportPIDetail = new ExportPIDetail();
            oExportPIDetail = MapObject(oReader);
            return oExportPIDetail;
        }      
        private List<ExportPIDetail> CreateObjects(IDataReader oReader)
        {
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPIDetail oItem = CreateObject(oHandler);
                oExportPIDetails.Add(oItem);
            }
            return oExportPIDetails;
        }
        #endregion

        #region Interface implementation
        public ExportPIDetailService() { }
      
        public ExportPIDetail Get(int id, Int64 nUserId)
        {
            ExportPIDetail oExportPIDetail = new ExportPIDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPIDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportPIDetail", e);
                #endregion
            }

            return oExportPIDetail;
        }

        public List<ExportPIDetail> Gets(int nExportPIID, Int64 nUserId)
        {
            List<ExportPIDetail> oExportPIDetail = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDetailDA.Gets(tc , nExportPIID);
                oExportPIDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIDetail", e);
                #endregion
            }

            return oExportPIDetail;
        }
        public List<ExportPIDetail> Gets(Int64 nUserId)
        {
            List<ExportPIDetail> oExportPIDetail = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPIDetailDA.Gets(tc);
                oExportPIDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIDetail", e);
                #endregion
            }

            return oExportPIDetail;
        }
        public List<ExportPIDetail> GetsByPI(int nExportPIID,Int64 nUserId)
        {
            List<ExportPIDetail> oExportPIDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDetailDA.GetsByPI(tc, nExportPIID);
                oExportPIDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPIDetail", e);
                #endregion
            }

            return oExportPIDetail;
        }
        public List<ExportPIDetail> GetsByPIAndSortByOrderSheet(int nExportPIID, Int64 nUserId)
        {
            List<ExportPIDetail> oExportPIDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDetailDA.GetsByPIAndSortByOrderSheet(tc, nExportPIID);
                oExportPIDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPIDetail", e);
                #endregion
            }

            return oExportPIDetail;
        }

        public List<ExportPIDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<ExportPIDetail> oExportPIDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDetailDA.Gets(tc, sSQL);
                oExportPIDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPIDetail", e);
                #endregion
            }

            return oExportPIDetail;
        }
        public List<ExportPIDetail> GetsLog(int nExportPIID, Int64 nUserId)
        {
            List<ExportPIDetail> oExportPIDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDetailDA.GetsLog(tc, nExportPIID);
                oExportPIDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIDetails", e);
                #endregion
            }

            return oExportPIDetails;
        }

        //GetsLogDetail
        public List<ExportPIDetail> GetsLogDetail(int nExportPILogID, Int64 nUserId)
        {
            List<ExportPIDetail> oExportPIDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDetailDA.GetsLogDetail(tc, nExportPILogID);
                oExportPIDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIDetails", e);
                #endregion
            }

            return oExportPIDetails;
        }
        public ExportPIDetail UpdateQuality(ExportPIDetail oExportPIDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPIDetailDA.UpdateQuality(tc, oExportPIDetail);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPIDetail = new ExportPIDetail();
                oExportPIDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportPIDetail;
        }
        public ExportPIDetail UpdateCRate(ExportPIDetail oExportPIDetail, Int64 nUserId)
        {
            int nStatus =0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                nStatus = SalesCommissionDA.GetStatus(tc, oExportPIDetail.ExportPIID);
                if(nStatus> (int)EnumLSalesCommissionStatus.Initialize)
                {
                    throw new Exception("Commission sate Change not Possible. Commission status is " + ((EnumLSalesCommissionStatus)nStatus).ToString() + "");
                }

                IDataReader reader = ExportPIDetailDA.UpdateCRate(tc, oExportPIDetail);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPIDetail = new ExportPIDetail();
                oExportPIDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportPIDetail;
        }
        #endregion
    }
}
