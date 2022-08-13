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

    public class ProductionExecutionPlanDetailService : MarshalByRefObject, IProductionExecutionPlanDetailService
    {
        #region Private functions and declaration
        private ProductionExecutionPlanDetail MapObject(NullHandler oReader)
        {
            ProductionExecutionPlanDetail oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
            oProductionExecutionPlanDetail.ProductionExecutionPlanDetailID = oReader.GetInt32("ProductionExecutionPlanDetailID");
            oProductionExecutionPlanDetail.ProductionExecutionPlanID = oReader.GetInt32("ProductionExecutionPlanID");
            oProductionExecutionPlanDetail.StartDate = oReader.GetDateTime("StartDate");
            oProductionExecutionPlanDetail.EndDate = oReader.GetDateTime("EndDate");
            oProductionExecutionPlanDetail.WorkingDays = oReader.GetInt32("WorkingDays");
            oProductionExecutionPlanDetail.MachineQty = oReader.GetDouble("MachineQty");
            oProductionExecutionPlanDetail.ProductionPerHour = oReader.GetDouble("ProductionPerHour");
            oProductionExecutionPlanDetail.AvgDailyProduction = oReader.GetDouble("AvgDailyProduction");
            oProductionExecutionPlanDetail.TotalProduction = oReader.GetDouble("TotalProduction");
            oProductionExecutionPlanDetail.PLineConfigureID = oReader.GetInt32("PLineConfigureID");
            oProductionExecutionPlanDetail.ProductionUnitID = oReader.GetInt32("ProductionUnitID");
            oProductionExecutionPlanDetail.PUShortName = oReader.GetString("PUShortName");
            oProductionExecutionPlanDetail.LineNo = oReader.GetString("LineNo");
            return oProductionExecutionPlanDetail;
        }

        private ProductionExecutionPlanDetail CreateObject(NullHandler oReader)
        {
            ProductionExecutionPlanDetail oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
            oProductionExecutionPlanDetail = MapObject(oReader);
            return oProductionExecutionPlanDetail;
        }

        private List<ProductionExecutionPlanDetail> CreateObjects(IDataReader oReader)
        {
            List<ProductionExecutionPlanDetail> oProductionExecutionPlanDetail = new List<ProductionExecutionPlanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionExecutionPlanDetail oItem = CreateObject(oHandler);
                oProductionExecutionPlanDetail.Add(oItem);
            }
            return oProductionExecutionPlanDetail;
        }

        #endregion

        #region Interface implementation
        public ProductionExecutionPlanDetailService() { }

        public ProductionExecutionPlanDetail Save(ProductionExecutionPlanDetail oProductionExecutionPlanDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<ProductionExecutionPlanDetail> _oProductionExecutionPlanDetails = new List<ProductionExecutionPlanDetail>();
            oProductionExecutionPlanDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ProductionExecutionPlanDetailDA.InsertUpdate(tc, oProductionExecutionPlanDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
                    oProductionExecutionPlanDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProductionExecutionPlanDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oProductionExecutionPlanDetail;
        }


        public ProductionExecutionPlanDetail Get(int ProductionExecutionPlanDetailID, Int64 nUserId)
        {
            ProductionExecutionPlanDetail oAccountHead = new ProductionExecutionPlanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionExecutionPlanDetailDA.Get(tc, ProductionExecutionPlanDetailID);
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
                throw new ServiceException("Failed to Get ProductionExecutionPlanDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ProductionExecutionPlanDetail> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<ProductionExecutionPlanDetail> oProductionExecutionPlanDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionExecutionPlanDetailDA.Gets(LabDipOrderID, tc);
                oProductionExecutionPlanDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionExecutionPlanDetail", e);
                #endregion
            }

            return oProductionExecutionPlanDetail;
        }

        public List<ProductionExecutionPlanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ProductionExecutionPlanDetail> oProductionExecutionPlanDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionExecutionPlanDetailDA.Gets(tc, sSQL);
                oProductionExecutionPlanDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionExecutionPlanDetail", e);
                #endregion
            }

            return oProductionExecutionPlanDetail;
        }
        #endregion
    }
 
}
