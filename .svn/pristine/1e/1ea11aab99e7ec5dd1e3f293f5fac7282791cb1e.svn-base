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

    public class ProductionExecutionPlanService : MarshalByRefObject, IProductionExecutionPlanService
    {
        #region Private functions and declaration
        private ProductionExecutionPlan MapObject(NullHandler oReader)
        {
            ProductionExecutionPlan oProductionExecutionPlan = new ProductionExecutionPlan();
            oProductionExecutionPlan.ProductionExecutionPlanID = oReader.GetInt32("ProductionExecutionPlanID");
            oProductionExecutionPlan.RefNo = oReader.GetString("RefNo");
            oProductionExecutionPlan.OrderRecapID = oReader.GetInt32("OrderRecapID");

            oProductionExecutionPlan.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oProductionExecutionPlan.PlanDate = oReader.GetDateTime("PlanDate");
            oProductionExecutionPlan.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oProductionExecutionPlan.FactoryShipmentDate = oReader.GetDateTime("FactoryShipmentDate");
            oProductionExecutionPlan.ProductionQty = oReader.GetDouble("ProductionQty");
            oProductionExecutionPlan.PlanExecutionQty = oReader.GetDouble("PlanExecutionQty");
            oProductionExecutionPlan.Note = oReader.GetString("Note");
            oProductionExecutionPlan.ApproveBy = oReader.GetInt32("ApproveBy");
            oProductionExecutionPlan.BuyerID = oReader.GetInt32("BuyerID");
            oProductionExecutionPlan.BUID = oReader.GetInt32("BUID");
            oProductionExecutionPlan.FBUID = oReader.GetInt32("FBUID");
            oProductionExecutionPlan.ProductID = oReader.GetInt32("ProductID");
            oProductionExecutionPlan.BuyerName = oReader.GetString("BuyerName");
            oProductionExecutionPlan.ApproveByName = oReader.GetString("ApproveByName");
            oProductionExecutionPlan.ProductName = oReader.GetString("ProductName");
            oProductionExecutionPlan.RecapNo = oReader.GetString("RecapNo");
            oProductionExecutionPlan.MerchandiserName = oReader.GetString("MerchandiserName");
            oProductionExecutionPlan.StyleNo = oReader.GetString("StyleNo");
            oProductionExecutionPlan.PlanStatus = (EnumPlanStatus)oReader.GetInt32("PlanStatus");
            oProductionExecutionPlan.PlanStatusInInt = oReader.GetInt32("PlanStatus");
            oProductionExecutionPlan.RecapQty = oReader.GetDouble("RecapQty");
            oProductionExecutionPlan.SMV = oReader.GetDouble("SMV");
            
            return oProductionExecutionPlan;
        }

        private ProductionExecutionPlan CreateObject(NullHandler oReader)
        {
            ProductionExecutionPlan oProductionExecutionPlan = new ProductionExecutionPlan();
            oProductionExecutionPlan = MapObject(oReader);
            return oProductionExecutionPlan;
        }

        private List<ProductionExecutionPlan> CreateObjects(IDataReader oReader)
        {
            List<ProductionExecutionPlan> oProductionExecutionPlan = new List<ProductionExecutionPlan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionExecutionPlan oItem = CreateObject(oHandler);
                oProductionExecutionPlan.Add(oItem);
            }
            return oProductionExecutionPlan;
        }

        #endregion

        #region Interface implementation
        public ProductionExecutionPlanService() { }

        public ProductionExecutionPlan Save(ProductionExecutionPlan oProductionExecutionPlan, Int64 nUserID)
        {
            List<ProductionExecutionPlanDetail> oProductionExecutionPlanDetails = new List<ProductionExecutionPlanDetail>();
            ProductionExecutionPlanDetail oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
            List<ProductionExecutionPlanDetailBreakdown> oProductionExecutionPlanDetailBreakdowns = new List<ProductionExecutionPlanDetailBreakdown>();
            ProductionExecutionPlanDetailBreakdown oProductionExecutionPlanDetailBreakdown = new ProductionExecutionPlanDetailBreakdown();
            oProductionExecutionPlanDetails = oProductionExecutionPlan.ProductionExecutionPlanDetails;
            int nTempProductionExecutionDetailID = 0;
            string sProductionExecutionPlanDetailIDs = "";
            string sProductionExecutionPlanDetailBreakdownIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProductionExecutionPlan.ProductionExecutionPlanID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProductionExecutionPlan, EnumRoleOperationType.Add);
                    reader = ProductionExecutionPlanDA.InsertUpdate(tc, oProductionExecutionPlan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProductionExecutionPlan, EnumRoleOperationType.Edit);
                    reader = ProductionExecutionPlanDA.InsertUpdate(tc, oProductionExecutionPlan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionExecutionPlan = new ProductionExecutionPlan();
                    oProductionExecutionPlan = CreateObject(oReader);
                }
                reader.Close();
                #region Courier Bill Detail Part
                if (oProductionExecutionPlanDetails != null)
                {
                    foreach (ProductionExecutionPlanDetail oItem in oProductionExecutionPlanDetails)
                    {
                        oProductionExecutionPlanDetailBreakdowns = new List<ProductionExecutionPlanDetailBreakdown>();
                        oItem.ProductionExecutionPlanID = oProductionExecutionPlan.ProductionExecutionPlanID;
                        oProductionExecutionPlanDetailBreakdowns = oItem.ProductionExecutionPlanDetailBreakdowns;

                        IDataReader readerdetail;
                        if (oItem.ProductionExecutionPlanDetailID <= 0)
                        {
                            readerdetail = ProductionExecutionPlanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = ProductionExecutionPlanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            nTempProductionExecutionDetailID = oReaderDetail.GetInt32("ProductionExecutionPlanDetailID");
                            sProductionExecutionPlanDetailIDs = sProductionExecutionPlanDetailIDs + oReaderDetail.GetString("ProductionExecutionPlanDetailID") + ",";

                        }
                        readerdetail.Close();
                        if (oProductionExecutionPlanDetailBreakdowns.Count > 0)
                        {
                            foreach (ProductionExecutionPlanDetailBreakdown oPEPDBD in oProductionExecutionPlanDetailBreakdowns)
                            {
                                IDataReader readerBrakDown;
                                oPEPDBD.ProductionExecutionPlanDetailID = nTempProductionExecutionDetailID;
                                if (oPEPDBD.ProductionExecutionPlanDetailBreakdownID <= 0)
                                {
                                    readerBrakDown = ProductionExecutionPlanDetailBreakdownDA.InsertUpdate(tc, oPEPDBD, EnumDBOperation.Insert, nUserID, "");
                                }
                                else
                                {
                                    readerBrakDown = ProductionExecutionPlanDetailBreakdownDA.InsertUpdate(tc, oPEPDBD, EnumDBOperation.Update, nUserID, "");
                                }
                                NullHandler oReaderBreakDown = new NullHandler(readerBrakDown);
                                if (readerBrakDown.Read())
                                {
                                    sProductionExecutionPlanDetailBreakdownIDs = sProductionExecutionPlanDetailBreakdownIDs + oReaderBreakDown.GetString("ProductionExecutionPlanDetailBreakdownID") + ",";
                                }
                                readerBrakDown.Close();
                            }
                            if (sProductionExecutionPlanDetailBreakdownIDs.Length > 0)
                            {
                                sProductionExecutionPlanDetailBreakdownIDs = sProductionExecutionPlanDetailBreakdownIDs.Remove(sProductionExecutionPlanDetailBreakdownIDs.Length - 1, 1);
                            }
                            oProductionExecutionPlanDetailBreakdown = new ProductionExecutionPlanDetailBreakdown();
                            oProductionExecutionPlanDetailBreakdown.ProductionExecutionPlanDetailID = nTempProductionExecutionDetailID;
                            ProductionExecutionPlanDetailBreakdownDA.Delete(tc, oProductionExecutionPlanDetailBreakdown, EnumDBOperation.Delete, nUserID, sProductionExecutionPlanDetailBreakdownIDs);
                            sProductionExecutionPlanDetailBreakdownIDs = "";
                        }
                    }
                    if (sProductionExecutionPlanDetailIDs.Length > 0)
                    {
                        sProductionExecutionPlanDetailIDs = sProductionExecutionPlanDetailIDs.Remove(sProductionExecutionPlanDetailIDs.Length - 1, 1);
                    }
                     oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
                    oProductionExecutionPlanDetail.ProductionExecutionPlanID = oProductionExecutionPlan.ProductionExecutionPlanID;
                    ProductionExecutionPlanDetailDA.Delete(tc, oProductionExecutionPlanDetail, EnumDBOperation.Delete, nUserID, sProductionExecutionPlanDetailIDs);

                }

                #endregion

                reader = ProductionExecutionPlanDA.Get(tc, oProductionExecutionPlan.ProductionExecutionPlanID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionExecutionPlan = new ProductionExecutionPlan();
                    oProductionExecutionPlan = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProductionExecutionPlan = new ProductionExecutionPlan();
                oProductionExecutionPlan.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oProductionExecutionPlan;
        }


        public ProductionExecutionPlan AcceptRevise(ProductionExecutionPlan oProductionExecutionPlan, Int64 nUserID)
        {
            List<ProductionExecutionPlanDetail> oProductionExecutionPlanDetails = new List<ProductionExecutionPlanDetail>();
            ProductionExecutionPlanDetail oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
            List<ProductionExecutionPlanDetailBreakdown> oProductionExecutionPlanDetailBreakdowns = new List<ProductionExecutionPlanDetailBreakdown>();
            ProductionExecutionPlanDetailBreakdown oProductionExecutionPlanDetailBreakdown = new ProductionExecutionPlanDetailBreakdown();
            oProductionExecutionPlanDetails = oProductionExecutionPlan.ProductionExecutionPlanDetails;
            int nTempProductionExecutionDetailID = 0;
            string sProductionExecutionPlanDetailIDs = "";
            string sProductionExecutionPlanDetailBreakdownIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProductionExecutionPlan, EnumRoleOperationType.Revise);
                reader = ProductionExecutionPlanDA.AcceptRevise(tc, oProductionExecutionPlan, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionExecutionPlan = new ProductionExecutionPlan();
                    oProductionExecutionPlan = CreateObject(oReader);
                }
                reader.Close();
                #region Courier Bill Detail Part
                if (oProductionExecutionPlanDetails != null)
                {
                    foreach (ProductionExecutionPlanDetail oItem in oProductionExecutionPlanDetails)
                    {
                        oProductionExecutionPlanDetailBreakdowns = new List<ProductionExecutionPlanDetailBreakdown>();
                        oItem.ProductionExecutionPlanID = oProductionExecutionPlan.ProductionExecutionPlanID;
                        oProductionExecutionPlanDetailBreakdowns = oItem.ProductionExecutionPlanDetailBreakdowns;
                        IDataReader readerdetail;
                        if (oItem.ProductionExecutionPlanDetailID <= 0)
                        {
                            readerdetail = ProductionExecutionPlanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = ProductionExecutionPlanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            nTempProductionExecutionDetailID = oReaderDetail.GetInt32("ProductionExecutionPlanDetailID");
                            sProductionExecutionPlanDetailIDs = sProductionExecutionPlanDetailIDs + oReaderDetail.GetString("ProductionExecutionPlanDetailID") + ",";
                        }
                        readerdetail.Close();
                        if (oProductionExecutionPlanDetailBreakdowns.Count > 0)
                        {
                            foreach (ProductionExecutionPlanDetailBreakdown oPEPDBD in oProductionExecutionPlanDetailBreakdowns)
                            {
                                IDataReader readerBrakDown;
                                oPEPDBD.ProductionExecutionPlanDetailID = nTempProductionExecutionDetailID;
                                if (oPEPDBD.ProductionExecutionPlanDetailBreakdownID <= 0)
                                {
                                    readerBrakDown = ProductionExecutionPlanDetailBreakdownDA.InsertUpdate(tc, oPEPDBD, EnumDBOperation.Insert, nUserID, "");
                                }
                                else
                                {
                                    readerBrakDown = ProductionExecutionPlanDetailBreakdownDA.InsertUpdate(tc, oPEPDBD, EnumDBOperation.Update, nUserID, "");
                                }
                                NullHandler oReaderBreakDown = new NullHandler(readerBrakDown);
                                if (readerBrakDown.Read())
                                {
                                    sProductionExecutionPlanDetailBreakdownIDs = sProductionExecutionPlanDetailBreakdownIDs + oReaderBreakDown.GetString("ProductionExecutionPlanDetailBreakdownID") + ",";
                                }
                                readerBrakDown.Close();
                            }
                            if (sProductionExecutionPlanDetailBreakdownIDs.Length > 0)
                            {
                                sProductionExecutionPlanDetailBreakdownIDs = sProductionExecutionPlanDetailBreakdownIDs.Remove(sProductionExecutionPlanDetailBreakdownIDs.Length - 1, 1);
                            }
                            oProductionExecutionPlanDetailBreakdown = new ProductionExecutionPlanDetailBreakdown();
                            oProductionExecutionPlanDetailBreakdown.ProductionExecutionPlanDetailID = nTempProductionExecutionDetailID;
                            ProductionExecutionPlanDetailBreakdownDA.Delete(tc, oProductionExecutionPlanDetailBreakdown, EnumDBOperation.Delete, nUserID, sProductionExecutionPlanDetailBreakdownIDs);
                            sProductionExecutionPlanDetailBreakdownIDs = "";
                        }
                    }
                    if (sProductionExecutionPlanDetailIDs.Length > 0)
                    {
                        sProductionExecutionPlanDetailIDs = sProductionExecutionPlanDetailIDs.Remove(sProductionExecutionPlanDetailIDs.Length - 1, 1);
                    }
                     oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
                    oProductionExecutionPlanDetail.ProductionExecutionPlanID = oProductionExecutionPlan.ProductionExecutionPlanID;
                    ProductionExecutionPlanDetailDA.Delete(tc, oProductionExecutionPlanDetail, EnumDBOperation.Delete, nUserID, sProductionExecutionPlanDetailIDs);
                }

                #endregion

                reader = ProductionExecutionPlanDA.Get(tc, oProductionExecutionPlan.ProductionExecutionPlanID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionExecutionPlan = new ProductionExecutionPlan();
                    oProductionExecutionPlan = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProductionExecutionPlan = new ProductionExecutionPlan();
                oProductionExecutionPlan.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oProductionExecutionPlan;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductionExecutionPlan oProductionExecutionPlan = new ProductionExecutionPlan();
                oProductionExecutionPlan.ProductionExecutionPlanID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ProductionExecutionPlan, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ProductionExecutionPlan", id);
                ProductionExecutionPlanDA.Delete(tc, oProductionExecutionPlan, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ProductionExecutionPlan. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ProductionExecutionPlan Get(int id, Int64 nUserId)
        {
            ProductionExecutionPlan oProductionExecutionPlan = new ProductionExecutionPlan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionExecutionPlanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionExecutionPlan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ProductionExecutionPlan", e);
                #endregion
            }

            return oProductionExecutionPlan;
        }

        public ProductionExecutionPlan GetByOrderRecap(int nOrderRecapId, Int64 nUserId)
        {
            ProductionExecutionPlan oProductionExecutionPlan = new ProductionExecutionPlan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionExecutionPlanDA.GetByOrderRecap(tc, nOrderRecapId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionExecutionPlan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ProductionExecutionPlan", e);
                #endregion
            }

            return oProductionExecutionPlan;
        }

        public ProductionExecutionPlan Approve(int id, Int64 nUserId)
        {
            ProductionExecutionPlan oProductionExecutionPlan = new ProductionExecutionPlan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                ProductionExecutionPlanDA.Approve(tc, id, nUserId);
                IDataReader reader = ProductionExecutionPlanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionExecutionPlan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ProductionExecutionPlan", e);
                #endregion
            }

            return oProductionExecutionPlan;
        }


        public ProductionExecutionPlan RequestForRevise(int id, Int64 nUserId)
        {
            ProductionExecutionPlan oProductionExecutionPlan = new ProductionExecutionPlan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                ProductionExecutionPlanDA.RequestForRevise(tc, id, nUserId);
                IDataReader reader = ProductionExecutionPlanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionExecutionPlan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ProductionExecutionPlan", e);
                #endregion
            }

            return oProductionExecutionPlan;
        }

        public List<ProductionExecutionPlan> Gets(Int64 nUserID)
        {
            List<ProductionExecutionPlan> oProductionExecutionPlan = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionExecutionPlanDA.Gets(tc);
                oProductionExecutionPlan = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionExecutionPlan", e);
                #endregion
            }

            return oProductionExecutionPlan;
        }

        public List<ProductionExecutionPlan> Gets(string sSQL, Int64 nUserID)
        {
            List<ProductionExecutionPlan> oProductionExecutionPlan = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionExecutionPlanDA.Gets(tc, sSQL);
                oProductionExecutionPlan = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionExecutionPlan", e);
                #endregion
            }

            return oProductionExecutionPlan;
        }

        #endregion
    }   
   
}
