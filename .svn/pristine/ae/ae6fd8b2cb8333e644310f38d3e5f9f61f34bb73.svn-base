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
 
    [Serializable]
    public class ProductionScheduleService : MarshalByRefObject, IProductionScheduleService
    {
        #region Private functions and declaration
        private ProductionSchedule MapObject(NullHandler oReader)
        {
            ProductionSchedule oProductionSchedule = new ProductionSchedule();
            
            oProductionSchedule.ProductionScheduleID = oReader.GetInt32("ProductionScheduleID");
            oProductionSchedule.BatchGroup = oReader.GetInt32("BatchGroup");
            oProductionSchedule.ScheduleStatus = (EnumProductionScheduleStatus)oReader.GetInt16("ScheduleStatus");
            oProductionSchedule.ProductionScheduleNo = oReader.GetString("ProductionScheduleNo");
            oProductionSchedule.ScheduleStability = oReader.GetString("ScheduleStability");
            oProductionSchedule.MachineID = oReader.GetInt16("MachineID");
            oProductionSchedule.BatchNo = (EnumNumericOrder)oReader.GetInt16("BatchNo");
            oProductionSchedule.LocationID = oReader.GetInt32("LocationID");
            oProductionSchedule.BUID = oReader.GetInt32("BUID");
            oProductionSchedule.ProductionQty = oReader.GetDouble("ProductionQty");
            oProductionSchedule.ScheduleType = oReader.GetBoolean("ScheduleType");
            oProductionSchedule.StartTime = oReader.GetDateTime("StartTime");
            oProductionSchedule.EndTime = oReader.GetDateTime("EndTime");
            oProductionSchedule.DBUserID = oReader.GetInt32("DBUserID");
            oProductionSchedule.MachineNo = oReader.GetString("MachineNo");
            oProductionSchedule.MachineName = oReader.GetString("MachineName");
            oProductionSchedule.UsesWeight = oReader.GetString("UsesWeight");
            oProductionSchedule.LocationName = oReader.GetString("LocationName");

            return oProductionSchedule;
        }

        private ProductionSchedule CreateObject(NullHandler oReader)
        {
            ProductionSchedule oProductionSchedule = new ProductionSchedule();
            oProductionSchedule = MapObject(oReader);
            return oProductionSchedule;
        }

        private List<ProductionSchedule> CreateObjects(IDataReader oReader)
        {
            List<ProductionSchedule> oProductionSchedules = new List<ProductionSchedule>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionSchedule oItem = CreateObject(oHandler);
                oProductionSchedules.Add(oItem);
            }
            return oProductionSchedules;
        }
        #endregion

        #region Interface implementation
        public ProductionScheduleService() { }
        public ProductionSchedule Save(ProductionSchedule oProductionSchedule, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                List<ProductionScheduleDetail> oProductionScheduleDetails = new List<ProductionScheduleDetail>();
                ProductionScheduleDetail oProductionScheduleDetail = new ProductionScheduleDetail();
                oProductionScheduleDetails = oProductionSchedule.ProductionScheduleDetails;
                string sProductionScheduleDetailIDs = "";

                IDataReader reader;
                if (oProductionSchedule.ProductionScheduleID <= 0)
                {
                    reader = ProductionScheduleDA.InsertUpdate(tc, oProductionSchedule, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ProductionScheduleDA.InsertUpdate(tc, oProductionSchedule, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionSchedule = new ProductionSchedule();
                    oProductionSchedule = CreateObject(oReader);
                }
                reader.Close();

                #region Production Schedule Detail Part
                foreach (ProductionScheduleDetail oItem in oProductionScheduleDetails)
                {
                    IDataReader readerdetail;
                    oItem.ProductionScheduleID = oProductionSchedule.ProductionScheduleID;
                    if (oItem.ProductionScheduleDetailID <= 0)
                    {
                        readerdetail = ProductionScheduleDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = ProductionScheduleDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sProductionScheduleDetailIDs = sProductionScheduleDetailIDs + oReaderDetail.GetString("ProductionScheduleDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sProductionScheduleDetailIDs.Length > 0)
                {
                    sProductionScheduleDetailIDs = sProductionScheduleDetailIDs.Remove(sProductionScheduleDetailIDs.Length - 1, 1);
                }
                oProductionScheduleDetail = new ProductionScheduleDetail();
                oProductionScheduleDetail.ProductionScheduleID = oProductionSchedule.ProductionScheduleID;
                ProductionScheduleDetailDA.Delete(tc, oProductionScheduleDetail, EnumDBOperation.Delete, nUserID, sProductionScheduleDetailIDs);
                #endregion
 

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProductionSchedule.ErrorMessage = e.Message;
                #endregion
            }

            return oProductionSchedule;

        }

        public int GetsMax(string sSql, Int64 nUserID)
        {
            int maxValue=0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                maxValue=Convert.ToInt32(ProductionScheduleDA.GetsMax(tc, sSql));
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException(e.Message, e);
                #endregion
            }
            return maxValue;
        }


        public List<ProductionSchedule> Gets(string sSql, Int64 nUserID)
        {
            List<ProductionSchedule> ProductionScheduleList = null;
            TransactionContext tc = null;
            IDataReader reader=null;
            try
            {
                tc = TransactionContext.Begin(true);
                reader = ProductionScheduleDA.Refresh(tc, sSql);
                ProductionScheduleList = CreateObjects(reader);
                reader.Close();
                tc.End();



            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException(e.Message, e);
                #endregion
            }
            return ProductionScheduleList;
        }


        public String Delete(ProductionSchedule oProductionSchedule, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductionScheduleDA.Delete(tc, oProductionSchedule, EnumDBOperation.Delete, nUserID);
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
            return "Delete successfully";
        }


        public String Update(int nId1, int nId2, double ProductionQtyFirst, double ProductionQtySecond, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductionScheduleDA.Update(tc, nId1, nId2, ProductionQtyFirst, ProductionQtySecond);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException(e.Message, e);
                #endregion
            }
            return "Order Change Successfully";
        }

        public List<ProductionSchedule> Gets(Int64 nUserID)
        {
            List<ProductionSchedule> oProductionSchedules = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionScheduleDA.Gets(tc);
                oProductionSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get ProductionSchedules", e);
                #endregion
            }

            return oProductionSchedules;
        }

        public List<ProductionSchedule> Gets(DateTime dStartDate, DateTime dEndDate, string sLocationIDs,Int64 nUserID)
        {
            List<ProductionSchedule> oProductionSchedules = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionScheduleDA.Gets(tc, dStartDate,  dEndDate,  sLocationIDs);
                oProductionSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get ProductionSchedules", e);
                #endregion
            }

            return oProductionSchedules;
        }

        public ProductionSchedule Get(int Id, Int64 nUserID)
        {
            ProductionSchedule oProductionSchedule = new ProductionSchedule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionScheduleDA.Get(tc, Id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionSchedule = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProductionSchedule = new ProductionSchedule();
                oProductionSchedule.ErrorMessage = e.Message;
                #endregion
            }



            return oProductionSchedule;
        }


        public DataSet GetsGroupBy(string sSql, Int64 nUserID)
        {
            TransactionContext tc = null;
            //DataTable oDataTable = new DataTable();
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionScheduleDA.GetsGroupBy(tc, sSql);
                //oDataSet.Load(reader);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[5]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                #endregion
            }
            return oDataSet;
        }



        #region Get Waiting Production Quantity


        public Double GetWaitingProductionQuantity(string sSql, Int64 nUserID)
        {
            double nWaitingTotalQuantity = 0;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nWaitingTotalQuantity = Convert.ToDouble(ProductionScheduleDA.GetWaitingProductionQuantity(tc, sSql));
                tc.End();
            }
            catch(Exception e)
            {

                #region Handel Exception
                if (tc != null)
                {
                    tc.HandleError();
                    ExceptionLog.Write(e);
                }
                #endregion

            }

            return nWaitingTotalQuantity;
        }

        #endregion

        #region Get Unpublish Production Schedule


        public int GetUnpublishProductionSchedule(string sSql, Int64 nUserID)
        {
            int nUnpublishProductionSchedule = 0;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nUnpublishProductionSchedule = Convert.ToInt32(ProductionScheduleDA.GetUnpublishProductionSchedule(tc, sSql));
                tc.End();
            }
            catch (Exception e)
            {

                #region Handel Exception
                if (tc != null)
                {
                    tc.HandleError();
                    ExceptionLog.Write(e);
                }
                #endregion

            }

            return nUnpublishProductionSchedule;
        }

        #endregion

        #endregion

    }
}
