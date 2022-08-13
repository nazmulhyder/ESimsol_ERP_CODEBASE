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


    public class StyleBudgetService : MarshalByRefObject, IStyleBudgetService
    {
        #region Private functions and declaration
        private StyleBudget MapObject(NullHandler oReader)
        {
            StyleBudget oStyleBudget = new StyleBudget();
            oStyleBudget.StyleBudgetID = oReader.GetInt32("StyleBudgetID");
            oStyleBudget.PostStyleBudgetID = oReader.GetInt32("PostStyleBudgetID");
            oStyleBudget.StyleBudgetLogID = oReader.GetInt32("StyleBudgetLogID");            
            oStyleBudget.RefObjectID = oReader.GetInt32("RefObjectID");
            oStyleBudget.BUID = oReader.GetInt32("BUID");
            oStyleBudget.FileNo = oReader.GetString("FileNo");
            oStyleBudget.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oStyleBudget.StyleNo = oReader.GetString("StyleNo");
            oStyleBudget.SpecialFinish = oReader.GetString("SpecialFinish");
            oStyleBudget.KnittingPatternName = oReader.GetString("KnittingPatternName");
            oStyleBudget.BuyerName = oReader.GetString("BuyerName");
            oStyleBudget.BrandName = oReader.GetString("BrandName");
            oStyleBudget.ColorRange = oReader.GetString("ColorRange");
            oStyleBudget.SizeRange = oReader.GetString("SizeRange");
            oStyleBudget.MerchandiserID = oReader.GetInt32("MerchandiserID");
            oStyleBudget.StyleBudgetStatus = (EnumStyleBudgetStatus)oReader.GetInt32("StyleBudgetStatus");
            oStyleBudget.StatusInInt = oReader.GetInt32("StyleBudgetStatus");
            oStyleBudget.CostingDate = oReader.GetDateTime("CostingDate");
            oStyleBudget.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oStyleBudget.ApproxQty = oReader.GetDouble("ApproxQty");
            oStyleBudget.UnitID = oReader.GetInt32("UnitID");
            oStyleBudget.UnitName = oReader.GetString("UnitName");
            oStyleBudget.DeptName = oReader.GetString("DeptName");
            oStyleBudget.WeightPerDozen = oReader.GetDouble("WeightPerDozen");
            oStyleBudget.WeightUnitID = oReader.GetInt32("WeightUnitID");
            oStyleBudget.WastageInPercent = oReader.GetDouble("WastageInPercent");
            oStyleBudget.GG = oReader.GetString("GG");
            oStyleBudget.FabricDescription = oReader.GetString("FabricDescription");
            oStyleBudget.StyleDescription = oReader.GetString("StyleDescription");
            oStyleBudget.CurrencyID = oReader.GetInt32("CurrencyID");
            oStyleBudget.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oStyleBudget.ProcessLoss = oReader.GetDouble("ProcessLoss");
            oStyleBudget.FabricWeightPerDozen = oReader.GetDouble("FabricWeightPerDozen");
            oStyleBudget.FabricUnitPrice = oReader.GetDouble("FabricUnitPrice");
            oStyleBudget.FabricCostPerDozen = oReader.GetDouble("FabricCostPerDozen");
            oStyleBudget.AccessoriesCostPerDozen = oReader.GetDouble("AccessoriesCostPerDozen");
            oStyleBudget.ProductionCostPerDozen = oReader.GetDouble("ProductionCostPerDozen");
            oStyleBudget.BuyingCommission = oReader.GetDouble("BuyingCommission");
            oStyleBudget.BankingCost = oReader.GetDouble("BankingCost");
            oStyleBudget.FOBPricePerPcs = oReader.GetDouble("FOBPricePerPcs");
            oStyleBudget.OfferPricePerPcs = oReader.GetDouble("OfferPricePerPcs");
            oStyleBudget.CMCost = oReader.GetDouble("CMCost");
            oStyleBudget.ConfirmPricePerPcs = oReader.GetDouble("ConfirmPricePerPcs");
            oStyleBudget.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oStyleBudget.ApprovedByName = oReader.GetString("ApprovedByName");
            oStyleBudget.WeightUnitSymbol = oReader.GetString("WeightUnitSymbol");
            oStyleBudget.WeightUnitName = oReader.GetString("WeightUnitName");
            oStyleBudget.MerchandiserName = oReader.GetString("MerchandiserName");
            oStyleBudget.StyleBudgetType = (EnumStyleBudgetType)oReader.GetInt32("StyleBudgetType");
            oStyleBudget.StyleBudgetTypeInInt = oReader.GetInt32("StyleBudgetType");
            oStyleBudget.GarmentsName = oReader.GetString("GarmentsName");
            oStyleBudget.YarnCategoryName = oReader.GetString("YarnCategoryName");
            oStyleBudget.PreparedByName = oReader.GetString("PreparedByName");
            
            oStyleBudget.Count = oReader.GetString("Count");
            oStyleBudget.ApprovedDate = oReader.GetDateTime("ApprovedDate");
          
            
            oStyleBudget.PrintPricePerPcs = oReader.GetDouble("PrintPricePerPcs");
            oStyleBudget.EmbrodaryPricePerPcs = oReader.GetDouble("EmbrodaryPricePerPcs");
            oStyleBudget.TestPricePerPcs = oReader.GetDouble("TestPricePerPcs");
            oStyleBudget.OthersPricePerPcs = oReader.GetDouble("OthersPricePerPcs");
            oStyleBudget.CourierPricePerPcs = oReader.GetDouble("CourierPricePerPcs");
            oStyleBudget.OthersCaption = oReader.GetString("OthersCaption");
            oStyleBudget.CourierCaption = oReader.GetString("CourierCaption");

            

            oStyleBudget.FabricCostPerPcs = oReader.GetDouble("FabricCostPerPcs");
            oStyleBudget.AccessoriesCostPerPcs = oReader.GetDouble("AccessoriesCostPerPcs");
            oStyleBudget.CMCostPerPcs = oReader.GetDouble("CMCostPerPcs");
            oStyleBudget.FOBPricePerDozen = oReader.GetDouble("FOBPricePerDozen");
            oStyleBudget.OfferPricePerDozen = oReader.GetDouble("OfferPricePerDozen");
            oStyleBudget.ConfirmPricePerDozen = oReader.GetDouble("ConfirmPricePerDozen");
            oStyleBudget.PrintPricePerDozen = oReader.GetDouble("PrintPricePerDozen");
            oStyleBudget.EmbrodaryPricePerDozen = oReader.GetDouble("EmbrodaryPricePerDozen");
            oStyleBudget.TestPricePerDozen = oReader.GetDouble("TestPricePerDozen");
            oStyleBudget.OthersPricePerDozen = oReader.GetDouble("OthersPricePerDozen");
            oStyleBudget.CourierPricePerDozen = oReader.GetDouble("CourierPricePerDozen");
            oStyleBudget.BudgetTitle = oReader.GetString("BudgetTitle");
            oStyleBudget.CostSheetID = oReader.GetInt32("CostSheetID");
       
            return oStyleBudget;
        }

        private StyleBudget CreateObject(NullHandler oReader)
        {
            StyleBudget oStyleBudget = new StyleBudget();
            oStyleBudget = MapObject(oReader);
            return oStyleBudget;
        }

        private List<StyleBudget> CreateObjects(IDataReader oReader)
        {
            List<StyleBudget> oStyleBudget = new List<StyleBudget>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                StyleBudget oItem = CreateObject(oHandler);
                oStyleBudget.Add(oItem);
            }
            return oStyleBudget;
        }

        #endregion

        #region Interface implementation
        public StyleBudgetService() { }

        public StyleBudget Save(StyleBudget oStyleBudget, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<StyleBudgetDetail> oStyleBudgetDetails = new List<StyleBudgetDetail>();
               
                List<StyleBudgetCM> oStyleBudgetCMs = new List<StyleBudgetCM>();
                List<StyleBudgetRecap> oStyleBudgetRecaps = new List<StyleBudgetRecap>();
            
                StyleBudgetDetail oStyleBudgetDetail = new StyleBudgetDetail();
        
                oStyleBudgetCMs = oStyleBudget.StyleBudgetCMs;
                oStyleBudgetDetails = oStyleBudget.StyleBudgetDetails;
       
                oStyleBudgetRecaps = oStyleBudget.StyleBudgetRecaps;
                string sStyleBudgetDetailIDs = ""; string sStyleBudgetRecapIDs = "", sStyleBudgetCMIDs = "";
                

                #region Style Budget part
                IDataReader reader;
                if (oStyleBudget.StyleBudgetID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.StyleBudget, EnumRoleOperationType.Add);
                    reader = StyleBudgetDA.InsertUpdate(tc, oStyleBudget, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.StyleBudget, EnumRoleOperationType.Edit);
                    reader = StyleBudgetDA.InsertUpdate(tc, oStyleBudget, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oStyleBudget = new StyleBudget();
                    oStyleBudget = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region  Style Budget Detail Part
                if (oStyleBudgetDetails != null)
                {
                    foreach (StyleBudgetDetail oItem in oStyleBudgetDetails)
                    {
                        IDataReader readerdetail;
                        oItem.StyleBudgetID = oStyleBudget.StyleBudgetID;
                        if (oItem.StyleBudgetDetailID <= 0)
                        {
                            readerdetail = StyleBudgetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = StyleBudgetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sStyleBudgetDetailIDs = sStyleBudgetDetailIDs + oReaderDetail.GetString("StyleBudgetDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sStyleBudgetDetailIDs.Length > 0)
                    {
                        sStyleBudgetDetailIDs = sStyleBudgetDetailIDs.Remove(sStyleBudgetDetailIDs.Length - 1, 1);
                    }
                    oStyleBudgetDetail = new StyleBudgetDetail();
                    oStyleBudgetDetail.StyleBudgetID = oStyleBudget.StyleBudgetID;
                    StyleBudgetDetailDA.Delete(tc, oStyleBudgetDetail, EnumDBOperation.Delete, nUserID, sStyleBudgetDetailIDs);

                }

                #endregion


                #region  Style Budget CM Part
                if (oStyleBudgetCMs != null)
                {
                    foreach (StyleBudgetCM oItem in oStyleBudgetCMs)
                    {
                        IDataReader readerdetail;
                        oItem.StyleBudgetID = oStyleBudget.StyleBudgetID;
                        if (oItem.StyleBudgetCMID <= 0)
                        {
                            readerdetail = StyleBudgetCMDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = StyleBudgetCMDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sStyleBudgetCMIDs = sStyleBudgetCMIDs + oReaderDetail.GetString("StyleBudgetCMID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sStyleBudgetCMIDs.Length > 0)
                    {
                        sStyleBudgetCMIDs = sStyleBudgetCMIDs.Remove(sStyleBudgetCMIDs.Length - 1, 1);
                    }
                    StyleBudgetCM oStyleBudgetCM = new StyleBudgetCM();
                    oStyleBudgetCM.StyleBudgetID = oStyleBudget.StyleBudgetID;
                    StyleBudgetCMDA.Delete(tc, oStyleBudgetCM, EnumDBOperation.Delete, nUserID, sStyleBudgetCMIDs);

                }

                #endregion

                #region  Style Budget Recap Part
                if (oStyleBudgetRecaps != null)
                {
                    foreach (StyleBudgetRecap oItem in oStyleBudgetRecaps)
                    {
                        IDataReader readerdetail;
                        oItem.StyleBudgetID = oStyleBudget.StyleBudgetID;
                        if (oItem.StyleBudgetRecapID <= 0)
                        {
                            readerdetail = StyleBudgetRecapDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = StyleBudgetRecapDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sStyleBudgetRecapIDs = sStyleBudgetRecapIDs + oReaderDetail.GetString("StyleBudgetRecapID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sStyleBudgetRecapIDs.Length > 0)
                    {
                        sStyleBudgetRecapIDs = sStyleBudgetRecapIDs.Remove(sStyleBudgetRecapIDs.Length - 1, 1);
                    }
                    StyleBudgetRecap oStyleBudgetRecap = new StyleBudgetRecap();
                    oStyleBudgetRecap.StyleBudgetID = oStyleBudget.StyleBudgetID;
                    StyleBudgetRecapDA.Delete(tc, oStyleBudgetRecap, EnumDBOperation.Delete, nUserID, sStyleBudgetRecapIDs);
                }

                #endregion

                #region Style Budget Get
                if (oStyleBudget.StyleBudgetType == EnumStyleBudgetType.PostBudget)
                {
                    reader = StyleBudgetDA.Get(tc, oStyleBudget.RefObjectID);
                }
                else
                {
                    reader = StyleBudgetDA.Get(tc, oStyleBudget.StyleBudgetID);
                }
                
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oStyleBudget = CreateObject(oReader);
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
                oStyleBudget.ErrorMessage = Message;

                #endregion
            }
            return oStyleBudget;
        }
        public StyleBudget AcceptStyleBudgetRevise(StyleBudget oStyleBudget, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<StyleBudgetDetail> oStyleBudgetDetails = new List<StyleBudgetDetail>();
                StyleBudgetDetail oStyleBudgetDetail = new StyleBudgetDetail();
              
          
                oStyleBudgetDetails = oStyleBudget.StyleBudgetDetails;
            
                string sStyleBudgetDetailIDs = "";

                #region Style Budget part

                if (oStyleBudget.StyleBudgetID > 0)
                {
                    IDataReader reader;
                    reader = StyleBudgetDA.AcceptStyleBudgetRevise(tc, oStyleBudget, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oStyleBudget = new StyleBudget();
                        oStyleBudget = CreateObject(oReader);
                    }
                    reader.Close();

                #endregion

                    #region Style Budget Detail Detail Part
                    if (oStyleBudgetDetails != null)
                    {
                        foreach (StyleBudgetDetail oItem in oStyleBudgetDetails)
                        {
                            IDataReader readerdetail;
                            oItem.StyleBudgetID = oStyleBudget.StyleBudgetID;
                            if (oItem.StyleBudgetDetailID <= 0)
                            {
                                readerdetail = StyleBudgetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = StyleBudgetDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sStyleBudgetDetailIDs = sStyleBudgetDetailIDs + oReaderDetail.GetString("StyleBudgetDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sStyleBudgetDetailIDs.Length > 0)
                        {
                            sStyleBudgetDetailIDs = sStyleBudgetDetailIDs.Remove(sStyleBudgetDetailIDs.Length - 1, 1);
                        }
                        oStyleBudgetDetail = new StyleBudgetDetail();
                        oStyleBudgetDetail.StyleBudgetID = oStyleBudget.StyleBudgetID;
                        StyleBudgetDetailDA.Delete(tc, oStyleBudgetDetail, EnumDBOperation.Delete, nUserID, sStyleBudgetDetailIDs);

                    }

                    #endregion

                    #region StyleBudget Get
                    reader = StyleBudgetDA.Get(tc, oStyleBudget.StyleBudgetID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oStyleBudget = CreateObject(oReader);
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
                oStyleBudget.ErrorMessage = Message;

                #endregion
            }
            return oStyleBudget;
        }
        public StyleBudget ChangeStatus(StyleBudget oStyleBudget, ApprovalRequest oApprovalRequest, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oStyleBudget.StyleBudgetActionType == EnumStyleBudgetActionType.Approve)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.StyleBudget, EnumRoleOperationType.Approved);
                }
                if (oStyleBudget.StyleBudgetActionType == EnumStyleBudgetActionType.Cancel)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.StyleBudget, EnumRoleOperationType.Cancel);
                }
                reader = StyleBudgetDA.ChangeStatus(tc, oStyleBudget, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oStyleBudget = new StyleBudget();
                    oStyleBudget = CreateObject(oReader);
                }
                reader.Close();
                if (oStyleBudget.StyleBudgetStatus == EnumStyleBudgetStatus.Req_For_App)
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
                oStyleBudget.ErrorMessage = Message;

                #endregion
            }
            return oStyleBudget;
        }


        public string Delete(int nStyleBudgetID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                StyleBudget oStyleBudget = new StyleBudget();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.StyleBudget, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "StyleBudget", nStyleBudgetID);
                oStyleBudget.StyleBudgetID = nStyleBudgetID;
                StyleBudgetDA.Delete(tc, oStyleBudget, EnumDBOperation.Delete, nUserId);
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

        public StyleBudget Get(int id, Int64 nUserId)
        {
            StyleBudget oAccountHead = new StyleBudget();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = StyleBudgetDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get StyleBudget", e);
                #endregion
            }

            return oAccountHead;
        }

        public StyleBudget GetLog(int id, Int64 nUserId)
        {
            StyleBudget oAccountHead = new StyleBudget();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = StyleBudgetDA.GetLog(tc, id);
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
                throw new ServiceException("Failed to Get StyleBudget", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<StyleBudget> Gets(Int64 nUserID)
        {
            List<StyleBudget> oStyleBudget = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = StyleBudgetDA.Gets(tc);
                oStyleBudget = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StyleBudget", e);
                #endregion
            }

            return oStyleBudget;
        }

        public List<StyleBudget> GetsStyleBudgetLog(int id, Int64 nUserID)
        {
            List<StyleBudget> oStyleBudget = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = StyleBudgetDA.GetsStyleBudgetLog(id, tc);
                oStyleBudget = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StyleBudget", e);
                #endregion
            }

            return oStyleBudget;
        }

        public List<StyleBudget> Gets(string sSQL, Int64 nUserID)
        {
            List<StyleBudget> oStyleBudget = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = StyleBudgetDA.Gets(tc, sSQL);
                oStyleBudget = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StyleBudget", e);
                #endregion
            }

            return oStyleBudget;
        }
        #endregion
    }
    
    
   
}
