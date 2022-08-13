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


    public class CostSheetService : MarshalByRefObject, ICostSheetService
    {
        #region Private functions and declaration
        private CostSheet MapObject(NullHandler oReader)
        {
            CostSheet oCostSheet = new CostSheet();
            oCostSheet.CostSheetID = oReader.GetInt32("CostSheetID");
            oCostSheet.CostSheetLogID = oReader.GetInt32("CostSheetLogID");
            oCostSheet.RefObjectID = oReader.GetInt32("RefObjectID");
            oCostSheet.BUID = oReader.GetInt32("BUID");
            oCostSheet.FileNo = oReader.GetString("FileNo");
            oCostSheet.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oCostSheet.StyleNo = oReader.GetString("StyleNo");
            oCostSheet.SpecialFinish = oReader.GetString("SpecialFinish");
            oCostSheet.KnittingPatternName = oReader.GetString("KnittingPatternName");
            oCostSheet.BuyerName = oReader.GetString("BuyerName");
            oCostSheet.BrandName = oReader.GetString("BrandName");
            oCostSheet.ColorRange = oReader.GetString("ColorRange");
            oCostSheet.SizeRange = oReader.GetString("SizeRange");
            oCostSheet.MerchandiserID = oReader.GetInt32("MerchandiserID");
            oCostSheet.CostSheetStatus = (EnumCostSheetStatus)oReader.GetInt32("CostSheetStatus");
            oCostSheet.StatusInInt = oReader.GetInt32("CostSheetStatus");
            oCostSheet.CostingDate = oReader.GetDateTime("CostingDate");
            oCostSheet.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oCostSheet.ApproxQty = oReader.GetDouble("ApproxQty");
            oCostSheet.UnitID = oReader.GetInt32("UnitID");
            oCostSheet.UnitName = oReader.GetString("UnitName");
            oCostSheet.DeptName = oReader.GetString("DeptName");
            oCostSheet.WeightPerDozen = oReader.GetDouble("WeightPerDozen");
            oCostSheet.WeightUnitID = oReader.GetInt32("WeightUnitID");
            oCostSheet.WastageInPercent = oReader.GetDouble("WastageInPercent");
            oCostSheet.GG = oReader.GetString("GG");
            oCostSheet.FabricDescription = oReader.GetString("FabricDescription");
            oCostSheet.StyleDescription = oReader.GetString("StyleDescription");
            oCostSheet.CurrencyID = oReader.GetInt32("CurrencyID");
            oCostSheet.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oCostSheet.ProcessLoss = oReader.GetDouble("ProcessLoss");
            oCostSheet.FabricWeightPerDozen = oReader.GetDouble("FabricWeightPerDozen");
            oCostSheet.FabricUnitPrice = oReader.GetDouble("FabricUnitPrice");
            oCostSheet.FabricCostPerDozen = oReader.GetDouble("FabricCostPerDozen");
            oCostSheet.AccessoriesCostPerDozen = oReader.GetDouble("AccessoriesCostPerDozen");
            oCostSheet.ProductionCostPerDozen = oReader.GetDouble("ProductionCostPerDozen");
            oCostSheet.BuyingCommission = oReader.GetDouble("BuyingCommission");
            oCostSheet.BankingCost = oReader.GetDouble("BankingCost");
            oCostSheet.FOBPricePerPcs = oReader.GetDouble("FOBPricePerPcs");
            oCostSheet.OfferPricePerPcs = oReader.GetDouble("OfferPricePerPcs");
            oCostSheet.CMCost = oReader.GetDouble("CMCost");
            oCostSheet.ConfirmPricePerPcs = oReader.GetDouble("ConfirmPricePerPcs");
            oCostSheet.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oCostSheet.ApprovedByName = oReader.GetString("ApprovedByName");
            oCostSheet.WeightUnitSymbol = oReader.GetString("WeightUnitSymbol");
            oCostSheet.WeightUnitName = oReader.GetString("WeightUnitName");
            oCostSheet.MerchandiserName = oReader.GetString("MerchandiserName");
            oCostSheet.CostSheetType = (EnumCostSheetType)oReader.GetInt32("CostSheetType");
            oCostSheet.CostSheetTypeInInt = oReader.GetInt32("CostSheetType");
            oCostSheet.GarmentsName = oReader.GetString("GarmentsName");
            oCostSheet.YarnCategoryName = oReader.GetString("YarnCategoryName");
            oCostSheet.PreparedByName = oReader.GetString("PreparedByName");
            
            oCostSheet.Count = oReader.GetString("Count");
            oCostSheet.ApprovedDate = oReader.GetDateTime("ApprovedDate");
          
            oCostSheet.PrintPricePerPcs = oReader.GetDouble("PrintPricePerPcs");
            oCostSheet.EmbrodaryPricePerPcs = oReader.GetDouble("EmbrodaryPricePerPcs");
            oCostSheet.TestPricePerPcs = oReader.GetDouble("TestPricePerPcs");
            oCostSheet.OthersPricePerPcs = oReader.GetDouble("OthersPricePerPcs");
            oCostSheet.CourierPricePerPcs = oReader.GetDouble("CourierPricePerPcs");
            oCostSheet.OthersCaption = oReader.GetString("OthersCaption");
            oCostSheet.CourierCaption = oReader.GetString("CourierCaption");

            

            oCostSheet.FabricCostPerPcs = oReader.GetDouble("FabricCostPerPcs");
            oCostSheet.AccessoriesCostPerPcs = oReader.GetDouble("AccessoriesCostPerPcs");
            oCostSheet.CMCostPerPcs = oReader.GetDouble("CMCostPerPcs");
            oCostSheet.FOBPricePerDozen = oReader.GetDouble("FOBPricePerDozen");
            oCostSheet.OfferPricePerDozen = oReader.GetDouble("OfferPricePerDozen");
            oCostSheet.ConfirmPricePerDozen = oReader.GetDouble("ConfirmPricePerDozen");
            oCostSheet.PrintPricePerDozen = oReader.GetDouble("PrintPricePerDozen");
            oCostSheet.EmbrodaryPricePerDozen = oReader.GetDouble("EmbrodaryPricePerDozen");
            oCostSheet.TestPricePerDozen = oReader.GetDouble("TestPricePerDozen");
            oCostSheet.OthersPricePerDozen = oReader.GetDouble("OthersPricePerDozen");
            oCostSheet.CourierPricePerDozen = oReader.GetDouble("CourierPricePerDozen");

       
            return oCostSheet;
        }

        private CostSheet CreateObject(NullHandler oReader)
        {
            CostSheet oCostSheet = new CostSheet();
            oCostSheet = MapObject(oReader);
            return oCostSheet;
        }

        private List<CostSheet> CreateObjects(IDataReader oReader)
        {
            List<CostSheet> oCostSheet = new List<CostSheet>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CostSheet oItem = CreateObject(oHandler);
                oCostSheet.Add(oItem);
            }
            return oCostSheet;
        }

        #endregion

        #region Interface implementation
        public CostSheetService() { }

        public CostSheet Save(CostSheet oCostSheet, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<CostSheetDetail> oCostSheetDetails = new List<CostSheetDetail>();
                List<CostSheetPackage> oCostSheetPackages = new List<CostSheetPackage>();
                List<CostSheetPackageDetail> oCostSheetPackageDetails = new List<CostSheetPackageDetail>();
                List<CostSheetCM> oCostSheetCMs = new List<CostSheetCM>();
                List<StyleBudgetRecap> oCostSheetRecaps = new List<StyleBudgetRecap>();
                CostSheetPackage oCostSheetPackage = new CostSheetPackage();
                CostSheetDetail oCostSheetDetail = new CostSheetDetail();
                CostSheetPackageDetail oCostSheetPackageDetail = new CostSheetPackageDetail();
                oCostSheetCMs = oCostSheet.CostSheetCMs;
                oCostSheetDetails = oCostSheet.CostSheetDetails;
                oCostSheetPackages = oCostSheet.CostSheetPackages;
                string sCostSheetDetailIDs = ""; string sCostSheetPackageIDs = "", sCostSheetRecapIDs = "", sCostSheetCMIDs = "";
                int nTempCostSheetPackageID = 0;

                #region Cost Sheet part
                IDataReader reader;
                if (oCostSheet.CostSheetID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CostSheet, EnumRoleOperationType.Add);
                    reader = CostSheetDA.InsertUpdate(tc, oCostSheet, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CostSheet, EnumRoleOperationType.Edit);
                    reader = CostSheetDA.InsertUpdate(tc, oCostSheet, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostSheet = new CostSheet();
                    oCostSheet = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region  Cost Sheet Detail Part
                if (oCostSheetDetails != null)
                {
                    foreach (CostSheetDetail oItem in oCostSheetDetails)
                    {
                        IDataReader readerdetail;
                        oItem.CostSheetID = oCostSheet.CostSheetID;
                        if (oItem.CostSheetDetailID <= 0)
                        {
                            readerdetail = CostSheetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = CostSheetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sCostSheetDetailIDs = sCostSheetDetailIDs + oReaderDetail.GetString("CostSheetDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sCostSheetDetailIDs.Length > 0)
                    {
                        sCostSheetDetailIDs = sCostSheetDetailIDs.Remove(sCostSheetDetailIDs.Length - 1, 1);
                    }
                    oCostSheetDetail = new CostSheetDetail();
                    oCostSheetDetail.CostSheetID = oCostSheet.CostSheetID;
                    CostSheetDetailDA.Delete(tc, oCostSheetDetail, EnumDBOperation.Delete, nUserID, sCostSheetDetailIDs);

                }

                #endregion

                #region  Cost Sheet Package Part
                if (oCostSheetPackages != null)
                {
                    foreach (CostSheetPackage oItem in oCostSheetPackages)
                    {
                        IDataReader readerPackage;
                        oItem.CostSheetID = oCostSheet.CostSheetID;
                        oCostSheetPackageDetails = new List<CostSheetPackageDetail>();
                        oCostSheetPackageDetails = oItem.CostSheetPackageDetails;
                        if (oItem.CostSheetPackageID <= 0)
                        {
                            readerPackage = CostSheetPackageDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerPackage = CostSheetPackageDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderPackage = new NullHandler(readerPackage);
                        if (readerPackage.Read())
                        {
                            nTempCostSheetPackageID = oReaderPackage.GetInt32("CostSheetPackageID");
                            sCostSheetPackageIDs = sCostSheetPackageIDs + oReaderPackage.GetString("CostSheetPackageID") + ",";
                        }
                        readerPackage.Close();
                        if(oCostSheetPackageDetails!=null)
                        {
                            foreach (CostSheetPackageDetail oDetailItem in oCostSheetPackageDetails)
                            {
                                IDataReader readerPackageDetail;
                                oDetailItem.CostSheetPackageID = nTempCostSheetPackageID;
                                if (oDetailItem.CostSheetPackageDetailID <= 0)
                                {
                                    readerPackageDetail = CostSheetPackageDetailDA.InsertUpdate(tc, oDetailItem, EnumDBOperation.Insert, nUserID);
                                }
                                else
                                {
                                    readerPackageDetail = CostSheetPackageDetailDA.InsertUpdate(tc, oDetailItem, EnumDBOperation.Update, nUserID);
                                }
                                NullHandler oReaderPackageDetail = new NullHandler(readerPackageDetail);
                                if (readerPackageDetail.Read())
                                {

                                }
                                readerPackageDetail.Close();
                            }
                        }
                    }
                    if (sCostSheetPackageIDs.Length > 0)
                    {
                        sCostSheetPackageIDs = sCostSheetPackageIDs.Remove(sCostSheetPackageIDs.Length - 1, 1);
                    }
                    oCostSheetPackage = new CostSheetPackage();
                    oCostSheetPackage.CostSheetID = oCostSheet.CostSheetID;
                    CostSheetPackageDA.Delete(tc, oCostSheetPackage, EnumDBOperation.Delete, nUserID, sCostSheetPackageIDs);

                }

                #endregion

                #region  Cost Sheet CM Part
                if (oCostSheetCMs != null)
                {
                    foreach (CostSheetCM oItem in oCostSheetCMs)
                    {
                        IDataReader readerdetail;
                        oItem.CostSheetID = oCostSheet.CostSheetID;
                        if (oItem.CostSheetCMID <= 0)
                        {
                            readerdetail = CostSheetCMDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = CostSheetCMDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sCostSheetCMIDs = sCostSheetCMIDs + oReaderDetail.GetString("CostSheetCMID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sCostSheetCMIDs.Length > 0)
                    {
                        sCostSheetCMIDs = sCostSheetCMIDs.Remove(sCostSheetCMIDs.Length - 1, 1);
                    }
                    CostSheetCM oCostSheetCM = new CostSheetCM();
                    oCostSheetCM.CostSheetID = oCostSheet.CostSheetID;
                    CostSheetCMDA.Delete(tc, oCostSheetCM, EnumDBOperation.Delete, nUserID, sCostSheetCMIDs);

                }

                #endregion

                #region Cost sheet Get

                reader = CostSheetDA.Get(tc, oCostSheet.CostSheetID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostSheet = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oCostSheet.ErrorMessage = Message;

                #endregion
            }
            return oCostSheet;
        }


        public CostSheet AcceptCostSheetRevise(CostSheet oCostSheet, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<CostSheetDetail> oCostSheetDetails = new List<CostSheetDetail>();
                CostSheetDetail oCostSheetDetail = new CostSheetDetail();
              
          
                oCostSheetDetails = oCostSheet.CostSheetDetails;
            
                string sCostSheetDetailIDs = "";

                #region Cost Sheet part

                if (oCostSheet.CostSheetID > 0)
                {
                    IDataReader reader;
                    reader = CostSheetDA.AcceptCostSheetRevise(tc, oCostSheet, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oCostSheet = new CostSheet();
                        oCostSheet = CreateObject(oReader);
                    }
                    reader.Close();

                #endregion

                    #region Cost Sheet Detail Detail Part
                    if (oCostSheetDetails != null)
                    {
                        foreach (CostSheetDetail oItem in oCostSheetDetails)
                        {
                            IDataReader readerdetail;
                            oItem.CostSheetID = oCostSheet.CostSheetID;
                            if (oItem.CostSheetDetailID <= 0)
                            {
                                readerdetail = CostSheetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = CostSheetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sCostSheetDetailIDs = sCostSheetDetailIDs + oReaderDetail.GetString("CostSheetDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sCostSheetDetailIDs.Length > 0)
                        {
                            sCostSheetDetailIDs = sCostSheetDetailIDs.Remove(sCostSheetDetailIDs.Length - 1, 1);
                        }
                        oCostSheetDetail = new CostSheetDetail();
                        oCostSheetDetail.CostSheetID = oCostSheet.CostSheetID;
                        CostSheetDetailDA.Delete(tc, oCostSheetDetail, EnumDBOperation.Delete, nUserID, sCostSheetDetailIDs);

                    }

                    #endregion


                    #region CostSheet Get
                    reader = CostSheetDA.Get(tc, oCostSheet.CostSheetID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oCostSheet = CreateObject(oReader);
                    }
                    reader.Close();
                }
                    #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oCostSheet.ErrorMessage = Message;

                #endregion
            }
            return oCostSheet;
        }

        public CostSheet ChangeStatus(CostSheet oCostSheet, ApprovalRequest oApprovalRequest, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCostSheet.CostSheetActionType == EnumCostSheetActionType.Approve)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CostSheet, EnumRoleOperationType.Approved);
                }
                if (oCostSheet.CostSheetActionType == EnumCostSheetActionType.Cancel)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CostSheet, EnumRoleOperationType.Cancel);
                }
                reader = CostSheetDA.ChangeStatus(tc, oCostSheet, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostSheet = new CostSheet();
                    oCostSheet = CreateObject(oReader);
                }
                reader.Close();
                if (oCostSheet.CostSheetStatus == EnumCostSheetStatus.Req_For_App)
                {
                    IDataReader ApprovalRequestreader;
                    ApprovalRequestreader = ApprovalRequestDA.InsertUpdate(tc, oApprovalRequest, EnumDBOperation.Insert, nUserID);
                    if (ApprovalRequestreader.Read())
                    {

                    }
                    ApprovalRequestreader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oCostSheet.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save CostSheetDetail. Because of " + e.Message, e);
                #endregion
            }
            return oCostSheet;
        }


        public string Delete(int nCostSheetID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CostSheet oCostSheet = new CostSheet();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.CostSheet, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "CostSheet", nCostSheetID);
                oCostSheet.CostSheetID = nCostSheetID;
                CostSheetDA.Delete(tc, oCostSheet, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }


        public CostSheet Get(int id, Int64 nUserId)
        {
            CostSheet oAccountHead = new CostSheet();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CostSheetDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get CostSheet", e);
                #endregion
            }

            return oAccountHead;
        }

        public CostSheet GetLog(int id, Int64 nUserId)
        {
            CostSheet oAccountHead = new CostSheet();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CostSheetDA.GetLog(tc, id);
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
                throw new ServiceException("Failed to Get CostSheet", e);
                #endregion
            }

            return oAccountHead;
        }


        public List<CostSheet> Gets(Int64 nUserID)
        {
            List<CostSheet> oCostSheet = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetDA.Gets(tc);
                oCostSheet = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSheet", e);
                #endregion
            }

            return oCostSheet;
        }

        public List<CostSheet> GetsCostSheetLog(int id, Int64 nUserID)
        {
            List<CostSheet> oCostSheet = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetDA.GetsCostSheetLog(id, tc);
                oCostSheet = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSheet", e);
                #endregion
            }

            return oCostSheet;
        }

        public List<CostSheet> Gets(string sSQL, Int64 nUserID)
        {
            List<CostSheet> oCostSheet = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetDA.Gets(tc, sSQL);
                oCostSheet = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSheet", e);
                #endregion
            }

            return oCostSheet;
        }



        #endregion
    }
    
    
   
}
