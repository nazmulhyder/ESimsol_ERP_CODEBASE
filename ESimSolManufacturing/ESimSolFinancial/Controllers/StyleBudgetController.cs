using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
 
using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{

    public class StyleBudgetController : Controller
    {

        #region Declartion
        StyleBudget _oStyleBudget = new StyleBudget();
        List<StyleBudget> _oStyleBudgets = new List<StyleBudget>();
        StyleBudgetDetail _oStyleBudgetDetail = new StyleBudgetDetail();
        List<StyleBudgetDetail> _oStyleBudgetDetails = new List<StyleBudgetDetail>();
        ApprovalRequest _oApprovalRequest = new ApprovalRequest();
        #endregion

        #region function
        private StyleBudget ConvertToStyleBudget(CostSheet oCostSheet)
        {
            StyleBudget oStyleBudget = new StyleBudget();
            oStyleBudget.TechnicalSheetID = oCostSheet.TechnicalSheetID;
            oStyleBudget.StyleNo = oCostSheet.StyleNo;
            oStyleBudget.SpecialFinish = oCostSheet.SpecialFinish;
            oStyleBudget.BuyerName = oCostSheet.BuyerName;
            oStyleBudget.ColorRange = oCostSheet.ColorRange;
            oStyleBudget.SizeRange = oCostSheet.SizeRange;
            oStyleBudget.MerchandiserID = oCostSheet.MerchandiserID;
            oStyleBudget.ShipmentDate = oCostSheet.ShipmentDate;
            oStyleBudget.ApproxQty = oCostSheet.ApproxQty;
            oStyleBudget.UnitID = oCostSheet.UnitID;
            oStyleBudget.UnitName = oCostSheet.UnitName;
            oStyleBudget.WeightPerDozen = oCostSheet.WeightPerDozen;
            oStyleBudget.WeightUnitID = oCostSheet.WeightUnitID;
            oStyleBudget.WastageInPercent = oCostSheet.WastageInPercent;
            oStyleBudget.GG = oCostSheet.GG;
            oStyleBudget.FabricDescription = oCostSheet.FabricDescription;
            oStyleBudget.StyleDescription = oCostSheet.StyleDescription;
            oStyleBudget.CurrencyID = oCostSheet.CurrencyID;
            oStyleBudget.CurrencySymbol = oCostSheet.CurrencySymbol;
            oStyleBudget.ProcessLoss = oCostSheet.ProcessLoss;
            oStyleBudget.FabricWeightPerDozen = oCostSheet.FabricWeightPerDozen;
            oStyleBudget.FabricUnitPrice = oCostSheet.FabricUnitPrice;
            oStyleBudget.FabricCostPerDozen = oCostSheet.FabricCostPerDozen;
            oStyleBudget.AccessoriesCostPerDozen = oCostSheet.AccessoriesCostPerDozen;
            oStyleBudget.ProductionCostPerDozen = oCostSheet.ProductionCostPerDozen;
            oStyleBudget.BuyingCommission = oCostSheet.BuyingCommission;
            oStyleBudget.FOBPricePerPcs = oCostSheet.FOBPricePerPcs;
            oStyleBudget.WeightUnitSymbol = oCostSheet.WeightUnitSymbol;
            oStyleBudget.MerchandiserName = oCostSheet.MerchandiserName;
            oStyleBudget.WeightUnitName = oCostSheet.WeightUnitName;
            oStyleBudget.StyleBudgetType = EnumStyleBudgetType.PreBudget;
            oStyleBudget.StyleBudgetTypeInInt = (int)EnumStyleBudgetType.PreBudget;
            oStyleBudget.BankingCost = oCostSheet.BankingCost;
            oStyleBudget.GarmentsName = oCostSheet.GarmentsName;
            oStyleBudget.BUID = oCostSheet.BUID;
            oStyleBudget.OfferPricePerPcs = oCostSheet.OfferPricePerPcs;
            oStyleBudget.YarnCategoryName = oCostSheet.YarnCategoryName;
            oStyleBudget.Count = oCostSheet.Count;
            oStyleBudget.CMCost = oCostSheet.CMCost;
            oStyleBudget.ConfirmPricePerPcs = oCostSheet.ConfirmPricePerPcs;
            oStyleBudget.DeptName = oCostSheet.DeptName;
            oStyleBudget.FabricCostPerPcs = oCostSheet.FabricCostPerPcs;
            oStyleBudget.AccessoriesCostPerPcs = oCostSheet.AccessoriesCostPerPcs;
            oStyleBudget.CMCostPerPcs = oCostSheet.CMCostPerPcs;
            oStyleBudget.FOBPricePerDozen = oCostSheet.FOBPricePerDozen;
            oStyleBudget.OfferPricePerDozen = oCostSheet.OfferPricePerDozen;
            oStyleBudget.ConfirmPricePerDozen = oCostSheet.ConfirmPricePerDozen;
            oStyleBudget.PrintPricePerDozen = oCostSheet.PrintPricePerDozen;
            oStyleBudget.EmbrodaryPricePerDozen = oCostSheet.EmbrodaryPricePerDozen;
            oStyleBudget.TestPricePerDozen = oCostSheet.TestPricePerDozen;
            oStyleBudget.OthersPricePerDozen = oCostSheet.OthersPricePerDozen;
            oStyleBudget.CourierPricePerDozen = oCostSheet.CourierPricePerDozen;
            oStyleBudget.PrintPricePerPcs = oCostSheet.PrintPricePerPcs;
            oStyleBudget.EmbrodaryPricePerPcs = oCostSheet.EmbrodaryPricePerPcs;
            oStyleBudget.TestPricePerPcs = oCostSheet.TestPricePerPcs;
            oStyleBudget.OthersPricePerPcs = oCostSheet.OthersPricePerPcs;
            oStyleBudget.CourierPricePerPcs = oCostSheet.CourierPricePerPcs;
            oStyleBudget.OthersCaption = oCostSheet.OthersCaption;//default
            oStyleBudget.CourierCaption = oCostSheet.CourierCaption;//default            
            oStyleBudget.BudgetTitle = "Budget";
            oStyleBudget.CostSheetID = oCostSheet.CostSheetID; 
            //cost Sheet details
            List<StyleBudgetDetail> oStyleBudgetDetails = new List<StyleBudgetDetail>();
            foreach (CostSheetDetail oItem in oCostSheet.CostSheetDetails)
            {
                StyleBudgetDetail oStyleBudgetDetail = new StyleBudgetDetail();
                oStyleBudgetDetail.MaterialType = oItem.MaterialType;
                oStyleBudgetDetail.MaterialID = oItem.MaterialID;
                oStyleBudgetDetail.ProductCode = oItem.ProductCode;
                oStyleBudgetDetail.ProductName = oItem.ProductName;
                oStyleBudgetDetail.Ply = oItem.Ply;
                oStyleBudgetDetail.MaterialMarketPrice = oItem.MaterialMarketPrice;
                oStyleBudgetDetail.UsePercentage = oItem.UsePercentage;
                oStyleBudgetDetail.EstimatedCost = oItem.EstimatedCost;
                oStyleBudgetDetail.MaterialTypeInInt = oItem.MaterialTypeInInt;
                oStyleBudgetDetail.ActualGarmentsWeight = oItem.ActualGarmentsWeight;
                oStyleBudgetDetail.ActualProcessLoss = oItem.ActualProcessLoss;
                oStyleBudgetDetail.Description = oItem.Description;
                oStyleBudgetDetail.Width = oItem.Width;
                oStyleBudgetDetail.Consumption = oItem.Consumption;
                oStyleBudgetDetail.UnitName = oItem.UnitName;
                oStyleBudgetDetail.Sequence = oItem.Sequence;
                oStyleBudgetDetail.UnitID = oItem.UnitID;
                oStyleBudgetDetail.UnitSymbol = oItem.UnitSymbol;
                oStyleBudgetDetail.WastagePercentPerMaterial = oItem.WastagePercentPerMaterial;
                oStyleBudgetDetail.DyeingCost = oItem.DyeingCost;
                oStyleBudgetDetail.LycraCost = oItem.LycraCost;
                oStyleBudgetDetail.AOPCost = oItem.AOPCost;
                oStyleBudgetDetail.KnittingCost = oItem.KnittingCost;
                oStyleBudgetDetail.WashCost = oItem.WashCost;
                oStyleBudgetDetail.YarnDyeingCost = oItem.YarnDyeingCost;
                oStyleBudgetDetail.SuedeCost = oItem.SuedeCost;
                oStyleBudgetDetail.FinishingCost = oItem.FinishingCost;
                oStyleBudgetDetail.BrushingCost = oItem.BrushingCost;
                oStyleBudgetDetail.RateUnit = oItem.RateUnit;
                oStyleBudgetDetails.Add(oStyleBudgetDetail);
            }
            oStyleBudget.StyleBudgetDetails = oStyleBudgetDetails;
            oStyleBudget.StyleBudgetYarnDetails = GetSpecificDetails(oStyleBudget.StyleBudgetDetails, true);
            oStyleBudget.StyleBudgetAccessoriesDetails = GetSpecificDetails(oStyleBudget.StyleBudgetDetails, false);
            //CostSheetCMs
            List<StyleBudgetCM> oStyleBudgetCMs = new List<StyleBudgetCM>();
            foreach (CostSheetCM oItem in oCostSheet.CostSheetCMs)
            {
                StyleBudgetCM oStyleBudgetCM = new StyleBudgetCM();
                oStyleBudgetCM.NumberOfMachine = oItem.NumberOfMachine;
                oStyleBudgetCM.MachineCost = oItem.MachineCost;
                oStyleBudgetCM.ProductionPerDay = oItem.ProductionPerDay;
                oStyleBudgetCM.BufferDays = oItem.BufferDays; 
                oStyleBudgetCM.TotalRequiredDays = oItem.TotalRequiredDays;
                oStyleBudgetCM.CMAdditionalPerent = oItem.CMAdditionalPerent; 
                oStyleBudgetCM.CSQty = oItem.CSQty;
                oStyleBudgetCM.CMPart = oItem.CMPart;
                oStyleBudgetCMs.Add(oStyleBudgetCM);
            }
            oStyleBudget.StyleBudgetCMs = oStyleBudgetCMs;
            return oStyleBudget;
        }

        private List<StyleBudgetDetail> GetSpecificDetails(List<StyleBudgetDetail> oStyleBudgetDetails, bool IsYarn)
        {
            _oStyleBudgetDetails = new List<StyleBudgetDetail>();

            foreach (StyleBudgetDetail oItem in oStyleBudgetDetails)
            {
                if (IsYarn == true)
                {
                    if (oItem.MaterialType == EnumCostSheetMeterialType.Yarn)
                    {
                        _oStyleBudgetDetails.Add(oItem);
                    }
                }
                else
                {
                    if (oItem.MaterialType == EnumCostSheetMeterialType.Accessories)
                    {
                        _oStyleBudgetDetails.Add(oItem);
                    }
                }
            }

            return _oStyleBudgetDetails;
        }

       
        #endregion

        #region Budget Management
        public ActionResult ViewBudgetMgt(int nCSID)
        {
            _oStyleBudgets = new List<StyleBudget>();
            CostSheet oCostSheet = new CostSheet();
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.StyleBudget).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            string sSQL = "SELECT * FROM View_StyleBudget WHERE StyleBudgetType = " +(int)EnumStyleBudgetType.PreBudget + " AND CostSheetID=" + nCSID;
           _oStyleBudgets = StyleBudget.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
           ViewBag.CostSheet = oCostSheet.Get(nCSID, (int)Session[SessionInfo.currentUserID]);
           return View(_oStyleBudgets);
        }

        #endregion

        #region Add, Edit, Delete

        #region Style Budget Entry
        public ActionResult ViewStyleBudget(int CSID, int id, int RefObjectID, int StyleBudgetType)
        {
            _oStyleBudget = new StyleBudget();
            if (id > 0)//PRe Cost
            {
                _oStyleBudget = _oStyleBudget.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oStyleBudget.StyleBudgetDetails = StyleBudgetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oStyleBudget.StyleBudgetRecaps = StyleBudgetRecap.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oStyleBudget.StyleBudgetCMs = StyleBudgetCM.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oStyleBudget.StyleBudgetYarnDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, true);
                _oStyleBudget.StyleBudgetAccessoriesDetails= GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, false);
            }
            else if(RefObjectID>0)//For Post Cost
            {
                _oStyleBudget = _oStyleBudget.Get(RefObjectID, (int)Session[SessionInfo.currentUserID]);
                _oStyleBudget.StyleBudgetDetails = StyleBudgetDetail.Gets(RefObjectID, (int)Session[SessionInfo.currentUserID]);
                _oStyleBudget.StyleBudgetRecaps = StyleBudgetRecap.Gets(RefObjectID, (int)Session[SessionInfo.currentUserID]);
                _oStyleBudget.StyleBudgetCMs = StyleBudgetCM.Gets(RefObjectID, (int)Session[SessionInfo.currentUserID]);

                _oStyleBudget.StyleBudgetID = 0;//ID Reset
                foreach (StyleBudgetDetail oItem in _oStyleBudget.StyleBudgetDetails) { oItem.StyleBudgetID = 0; oItem.StyleBudgetDetailID = 0; }
                foreach (StyleBudgetRecap oItem in _oStyleBudget.StyleBudgetRecaps) { oItem.StyleBudgetID = 0; oItem.StyleBudgetRecapID = 0; }
                foreach (StyleBudgetCM oItem in _oStyleBudget.StyleBudgetCMs) { oItem.StyleBudgetID = 0; oItem.StyleBudgetCMID = 0; }
                _oStyleBudget.StyleBudgetYarnDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, true);
                _oStyleBudget.StyleBudgetAccessoriesDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, false);

                
                _oStyleBudget.RefObjectID = RefObjectID;
            }
            else
            {
                CostSheet oCostSheet = new CostSheet();
                oCostSheet = oCostSheet.Get(CSID, (int)Session[SessionInfo.currentUserID]);
                oCostSheet.CostSheetDetails = CostSheetDetail.Gets(CSID, (int)Session[SessionInfo.currentUserID]);
                oCostSheet.CostSheetCMs = CostSheetCM.Gets(CSID, (int)Session[SessionInfo.currentUserID]);
                _oStyleBudget = ConvertToStyleBudget(oCostSheet);//convert CS to Style Budget 
            }
            _oStyleBudget.CountUnits = MeasurementUnit.Gets(EnumUniteType.Count, (int)Session[SessionInfo.currentUserID]);
            _oStyleBudget.WeightUnits = MeasurementUnit.Gets(EnumUniteType.Weight, (int)Session[SessionInfo.currentUserID]);
            _oStyleBudget.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.StyleBudgetType = StyleBudgetType;
            ViewBag.CostSheetID= CSID;
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.StyleBudget).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            return View(_oStyleBudget);
        }
    
        

        #endregion

        #region Style Budget Revise
        public ActionResult ViewStyleBudgetRevise(int id, int StyleBudgetType)
        {
            _oStyleBudget = new StyleBudget();
            if (id > 0)
            {
                _oStyleBudget = _oStyleBudget.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oStyleBudget.StyleBudgetDetails = StyleBudgetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oStyleBudget.StyleBudgetYarnDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, true);
                _oStyleBudget.StyleBudgetAccessoriesDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, false);
            }
            _oStyleBudget.CountUnits = MeasurementUnit.Gets(EnumUniteType.Count, (int)Session[SessionInfo.currentUserID]);
            _oStyleBudget.WeightUnits = MeasurementUnit.Gets(EnumUniteType.Weight, (int)Session[SessionInfo.currentUserID]);
            _oStyleBudget.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.OperationType = StyleBudgetType;
            return View(_oStyleBudget);
        }
        #endregion

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(StyleBudget oStyleBudget)
        {
            _oStyleBudget = new StyleBudget();
            List<StyleBudgetDetail> oStyleBudgetDetails = new List<StyleBudgetDetail>();
            try
            {
                _oStyleBudget = oStyleBudget;
                foreach (StyleBudgetDetail oItem in oStyleBudget.StyleBudgetYarnDetails)
                {
                    _oStyleBudgetDetail = new StyleBudgetDetail();
                    _oStyleBudgetDetail = oItem;
                    oStyleBudgetDetails.Add(_oStyleBudgetDetail);
                }
                foreach (StyleBudgetDetail oItem in oStyleBudget.StyleBudgetAccessoriesDetails)
                {
                    _oStyleBudgetDetail = new StyleBudgetDetail();
                    _oStyleBudgetDetail = oItem;
                    oStyleBudgetDetails.Add(_oStyleBudgetDetail);
                }
               
                _oStyleBudget.StyleBudgetDetails = oStyleBudgetDetails;
                _oStyleBudget.StyleBudgetType = (EnumStyleBudgetType)_oStyleBudget.StyleBudgetTypeInInt;
                _oStyleBudget = _oStyleBudget.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oStyleBudget = new StyleBudget();
                _oStyleBudget.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oStyleBudget);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP AcceptStyleBudgetRevise
        [HttpPost]
        public JsonResult AcceptStyleBudgetRevise(StyleBudget oStyleBudget)
        {
            _oStyleBudget = new StyleBudget();
            List<StyleBudgetDetail> oStyleBudgetDetails = new List<StyleBudgetDetail>();
            try
            {
                _oStyleBudget = oStyleBudget;
                foreach (StyleBudgetDetail oItem in oStyleBudget.StyleBudgetYarnDetails)
                {
                    _oStyleBudgetDetail = new StyleBudgetDetail();
                    _oStyleBudgetDetail = oItem;
                    oStyleBudgetDetails.Add(_oStyleBudgetDetail);
                }
                foreach (StyleBudgetDetail oItem in oStyleBudget.StyleBudgetAccessoriesDetails)
                {
                    _oStyleBudgetDetail = new StyleBudgetDetail();
                    _oStyleBudgetDetail = oItem;
                    oStyleBudgetDetails.Add(_oStyleBudgetDetail);
                }
                _oStyleBudget.StyleBudgetDetails = oStyleBudgetDetails;
                _oStyleBudget = _oStyleBudget.AcceptStyleBudgetRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oStyleBudget = new StyleBudget();
                _oStyleBudget.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oStyleBudget);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP GET Delete
        [HttpPost]
        public JsonResult Delete(StyleBudget oStyleBudget)
        {
            string smessage = "";
            try
            {
                smessage = oStyleBudget.Delete(oStyleBudget.StyleBudgetID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region LoadCostingInfo
        [HttpPost]
        public JsonResult LoadCostingInfo(TechnicalSheet oTechnicalSheet)
        {
            _oStyleBudget = new StyleBudget();
            _oStyleBudgets = new List<StyleBudget>();
            string sSQL = "SELECT top 1 * FROM View_StyleBudget WHERE TechnicalSheetID = " + oTechnicalSheet.TechnicalSheetID;
        
            try
            {

                _oStyleBudgets = StyleBudget.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
                if (_oStyleBudgets.Count > 0)
                {
                    
                    _oStyleBudget = _oStyleBudgets[0];
                    _oStyleBudget.StyleBudgetDetails = StyleBudgetDetail.Gets(_oStyleBudget.StyleBudgetID, (int)Session[SessionInfo.currentUserID]);
                    foreach (StyleBudgetDetail oItem in _oStyleBudget.StyleBudgetDetails) { oItem.StyleBudgetID = 0; oItem.StyleBudgetDetailID = 0; }//Reset id
                    _oStyleBudget.StyleBudgetYarnDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, true);
                    _oStyleBudget.StyleBudgetAccessoriesDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, false);
                    _oStyleBudget.StyleBudgetStepDetails = _oStyleBudget.StyleBudgetDetails.Where(x => x.MaterialType == EnumCostSheetMeterialType.Production_Step).ToList();
                   
                   
                    //Retset
                    _oStyleBudget.StyleBudgetID = 0;//Reset for new
                  
                    
                }
            }
            catch (Exception ex)
            {
                _oStyleBudget = new StyleBudget();
                _oStyleBudget.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oStyleBudget);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

     

        #region Hitory Gets
        [HttpPost]
        public JsonResult GetStyleBudgetReviseHistory(StyleBudget oStyleBudget)
        {
            _oStyleBudgets = new List<StyleBudget>();
            try
            {
                _oStyleBudgets = StyleBudget.GetsStyleBudgetLog(oStyleBudget.StyleBudgetID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oStyleBudget = new StyleBudget();
                _oStyleBudget.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oStyleBudgets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Style Budget Log
        public ActionResult ViewStyleBudgetLog(int id, double ts) // id is log id
        {
            _oStyleBudget = new StyleBudget();
            if (id > 0)
            {
                _oStyleBudget = _oStyleBudget.GetLog(id, (int)Session[SessionInfo.currentUserID]);
                _oStyleBudget.StyleBudgetDetails = StyleBudgetDetail.GetsStyleBudgetLog(id, (int)Session[SessionInfo.currentUserID]);
                _oStyleBudget.StyleBudgetYarnDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, true);
                _oStyleBudget.StyleBudgetAccessoriesDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, false);
            }
            _oStyleBudget.CountUnits = MeasurementUnit.Gets(EnumUniteType.Count, (int)Session[SessionInfo.currentUserID]);
            _oStyleBudget.WeightUnits = MeasurementUnit.Gets(EnumUniteType.Weight, (int)Session[SessionInfo.currentUserID]);
            _oStyleBudget.Currencies = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.StyleBudget).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            return PartialView(_oStyleBudget);
        }
        #endregion


        #region HTTP Approve
        [HttpPost]
        public JsonResult Approve(StyleBudget oStyleBudget)
        {
            _oStyleBudget = new StyleBudget();
            List<StyleBudgetDetail> oStyleBudgetDetails = new List<StyleBudgetDetail>();
            try
            {
                _oStyleBudget = oStyleBudget;
                _oStyleBudget.StatusInInt = (int)EnumStyleBudgetStatus.Req_For_App;
                //foreach (StyleBudgetDetail oItem in oStyleBudget.StyleBudgetYarnDetails)
                //{
                //    _oStyleBudgetDetail = new StyleBudgetDetail();
                //    _oStyleBudgetDetail = oItem;
                //    oStyleBudgetDetails.Add(_oStyleBudgetDetail);
                //}
                //foreach (StyleBudgetDetail oItem in oStyleBudget.StyleBudgetAccessoriesDetails)
                //{
                //    _oStyleBudgetDetail = new StyleBudgetDetail();
                //    _oStyleBudgetDetail = oItem;
                //    oStyleBudgetDetails.Add(_oStyleBudgetDetail);
                //}
                //_oStyleBudget.StyleBudgetDetails = oStyleBudgetDetails;
                //_oStyleBudget = _oStyleBudget.Save((int)Session[SessionInfo.currentUserID]);
                _oStyleBudget.StyleBudgetActionType = EnumStyleBudgetActionType.Approve;
                _oStyleBudget = SetStyleBudgetStatus(_oStyleBudget);
                _oStyleBudget.ApprovalRequest = new ApprovalRequest();
                _oStyleBudget = _oStyleBudget.ChangeStatus(_oStyleBudget.ApprovalRequest, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oStyleBudget = new StyleBudget();
                _oStyleBudget.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oStyleBudget);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(StyleBudget oStyleBudget)
        {
            _oStyleBudget = new StyleBudget();
            _oStyleBudget = oStyleBudget;
            try
            {
                if (oStyleBudget.ActionTypeExtra == "RequestForApproved")
                {

                    _oStyleBudget.StyleBudgetActionType = EnumStyleBudgetActionType.RequestForApproval;

                }
                else if (oStyleBudget.ActionTypeExtra == "UndoRequest")
                {

                    _oStyleBudget.StyleBudgetActionType = EnumStyleBudgetActionType.UndoRequest;

                }
                //else if (oStyleBudget.ActionTypeExtra == "Approve")
                //{

                //    _oStyleBudget.StyleBudgetActionType = EnumStyleBudgetActionType.Approve;

                //}
                else if (oStyleBudget.ActionTypeExtra == "UndoApprove")
                {

                    _oStyleBudget.StyleBudgetActionType = EnumStyleBudgetActionType.UndoApprove;
                }

                else if (oStyleBudget.ActionTypeExtra == "RequestForRevise")
                {

                    _oStyleBudget.StyleBudgetActionType = EnumStyleBudgetActionType.Request_revise;
                }
                else if (oStyleBudget.ActionTypeExtra == "Cancel")
                {

                    _oStyleBudget.StyleBudgetActionType = EnumStyleBudgetActionType.Cancel;
                }

                //_oStyleBudget.Note = oStyleBudget.Note;
                //_oStyleBudget.OperationBy = oStyleBudget.OperationBy;
                oStyleBudget = SetStyleBudgetStatus(_oStyleBudget);

                if (oStyleBudget.ActionTypeExtra == "RequestForApproved") // for SEt Approval Request Value
                {
                    oStyleBudget.ApprovalRequest.RequestBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                    oStyleBudget.ApprovalRequest.OperationType = EnumApprovalRequestOperationType.StyleBudget;

                }
                else
                {
                    oStyleBudget.ApprovalRequest = new ApprovalRequest();
                }

                _oStyleBudget = oStyleBudget.ChangeStatus(oStyleBudget.ApprovalRequest, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oStyleBudget = new StyleBudget();
                _oStyleBudget.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oStyleBudget);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Set Status
        private StyleBudget SetStyleBudgetStatus(StyleBudget oStyleBudget)//Set EnumOrderStatus Value
        {
            switch (oStyleBudget.StatusInInt)
            {
                case 1:
                    {
                        oStyleBudget.StyleBudgetStatus = EnumStyleBudgetStatus.Initialized;
                        break;
                    }
                case 2:
                    {
                        oStyleBudget.StyleBudgetStatus = EnumStyleBudgetStatus.Req_For_App;
                        break;
                    }
                case 3:
                    {
                        oStyleBudget.StyleBudgetStatus = EnumStyleBudgetStatus.Approved;
                        break;
                    }

                case 4:
                    {
                        oStyleBudget.StyleBudgetStatus = EnumStyleBudgetStatus.RequestForRevise;
                        break;
                    }
                case 5:
                    {
                        oStyleBudget.StyleBudgetStatus = EnumStyleBudgetStatus.Cancel;
                        break;
                    }
            }

            return oStyleBudget;
        }
        #endregion
        #endregion

        #region Advance Search
        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }
        #region HttpGet For Search
        [HttpGet]
        public JsonResult Search(string sTemp)
        {
            List<StyleBudget> oStyleBudgets = new List<StyleBudget>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oStyleBudgets = StyleBudget.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oStyleBudget = new StyleBudget();
                _oStyleBudget.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oStyleBudgets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region GetSQL
        private string GetSQL(string sTemp)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
  
            int nCostingDateComboValue = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dStartCositngDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dEndCostingDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            int nShipmentDateComboValue = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dStartShipmentDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dEndShipmentDate = Convert.ToDateTime(sTemp.Split('~')[5]);

            string sFileNo = sTemp.Split('~')[6];
            string sTechnicalSheetIDs = sTemp.Split('~')[7];
            int nApproveByID = Convert.ToInt32(sTemp.Split('~')[8]);
            string nMerchandiserIDs = sTemp.Split('~')[9];
            string sStatus = sTemp.Split('~')[10];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[11]);
            int nSessionID = Convert.ToInt32(sTemp.Split('~')[12]);
            string nBuyerIDs = sTemp.Split('~')[13];
            string nDepartmentIDs = sTemp.Split('~')[14];
            int nOperationType= Convert.ToInt32(sTemp.Split('~')[15]);

            string sReturn1 = "SELECT * FROM View_StyleBudget";
            string sReturn = "";

            #region File No

            if (sFileNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FileNo ='" + sFileNo + "'";
            }
            #endregion

            #region MerchandiserID
            if (!string.IsNullOrEmpty(nMerchandiserIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MerchandiserID IN (" + nMerchandiserIDs+")";
            }
            #endregion

            #region Buyer
            if (!string.IsNullOrEmpty(nBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + nBuyerIDs + ")";
            }
            #endregion
            #region DepartmentID
            if (!string.IsNullOrEmpty(nDepartmentIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Dept IN (" + nDepartmentIDs + ")";
            }
            #endregion

            #region Approve ByID
            if (nApproveByID!= 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApprovedBy = " + nApproveByID;
            }
            #endregion


            #region SessionID
            if (nSessionID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID = " + nSessionID;
            }
            #endregion

            #region Operation type
            if (nOperationType!= 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OperationType = " + nOperationType;
            }
            #endregion

            #region Technical Sheets 
            if (sTechnicalSheetIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TechnicalSheetID IN (" + sTechnicalSheetIDs + ")";
            }
            #endregion

            #region Status
                if (sStatus != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " StyleBudgetStatus IN (" + sStatus + ")";
                }
            #endregion

            #region Costing  Date Wise
            if (nCostingDateComboValue > 0)
            {
                if (nCostingDateComboValue == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CostingDate = '" + dStartCositngDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCostingDateComboValue == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CostingDate != '" + dStartCositngDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCostingDateComboValue == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CostingDate > '" + dStartCositngDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCostingDateComboValue == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CostingDate < '" + dStartCositngDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCostingDateComboValue == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CostingDate>= '" + dStartCositngDate.ToString("dd MMM yyyy") + "' AND CostingDate < '" + dEndCostingDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCostingDateComboValue == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CostingDate< '" + dStartCositngDate.ToString("dd MMM yyyy") + "' OR CostingDate > '" + dEndCostingDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region Shipment Date Wise
            if (nShipmentDateComboValue > 0)
            {
                if (nShipmentDateComboValue == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate = '" + dStartShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateComboValue == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate != '" + dStartShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateComboValue == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate > '" + dStartShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateComboValue == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate < '" + dStartShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateComboValue == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate>= '" + dStartShipmentDate.ToString("dd MMM yyyy") + "' AND ShipmentDate < '" + dEndShipmentDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateComboValue == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate< '" + dStartShipmentDate.ToString("dd MMM yyyy") + "' OR ShipmentDate > '" + dEndShipmentDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region BU
            if (nBUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn += "  TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY StyleBudgetID";
            return sReturn;
        }
        #endregion

      
#endregion


    #endregion

     
        //public ActionResult PrintStyleBudgetList(string sIDs)
        //{
        //    _oStyleBudget = new StyleBudget();
        //    string sSQL = "SELECT * FROM View_StyleBudget WHERE StyleBudgetID IN (" + sIDs + ")";
        //    _oStyleBudget.StyleBudgetList = StyleBudget.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    rptStyleBudgetList oReport = new rptStyleBudgetList();
        //    byte[] abytes = oReport.PrepareReport(_oStyleBudget, oCompany);
        //    return File(abytes, "application/pdf");
        //}
        //public ActionResult PrintStyleBudgetPreview(int id)
        //{
        //    _oStyleBudget = new StyleBudget();
        //    TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
        //    _oStyleBudget = _oStyleBudget.Get(id, (int)Session[SessionInfo.currentUserID]);
        //    _oStyleBudget.StyleBudgetCMs = StyleBudgetCM.Gets(id, (int)Session[SessionInfo.currentUserID]);
        //    _oStyleBudget.TechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(_oStyleBudget.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
        //    _oStyleBudget.StyleBudgetDetails = StyleBudgetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
        //    _oStyleBudget.StyleBudgetYarnDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, true);
        //    _oStyleBudget.StyleBudgetAccessoriesDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, false);
           
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //    rptStyleBudget oReport = new rptStyleBudget();
        //    byte[] abytes = oReport.PrepareReport(_oStyleBudget, oCompany);
        //    return File(abytes, "application/pdf");
        //}
 

        public ActionResult BudgetPrint(int id)
        {
            _oStyleBudget = new StyleBudget();
            StyleBudget oPostStyleBudget = new StyleBudget();
            List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
            List<PAMDetail> oPAMDetails = new List<PAMDetail>();
            List<OrderRecapDetail> oORDlsForPostBudget = new List<OrderRecapDetail>();
            
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            _oStyleBudget = _oStyleBudget.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oStyleBudget.TechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(_oStyleBudget.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oStyleBudget.TechnicalSheetSizeList = TechnicalSheetSize.Gets(_oStyleBudget.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);

            _oStyleBudget.StyleBudgetCMs = StyleBudgetCM.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oStyleBudget.StyleBudgetRecaps = StyleBudgetRecap.Gets(id, (int)Session[SessionInfo.currentUserID]);
            StringBuilder sSQL = new StringBuilder("SELECT * FROM View_OrderRecapDetail WHERE OrderRecapID IN (SELECT RefID FROM StyleBudgetRecap WHERE RefType="+(int)EnumStyleBudgetRecapType.OrderRecap +" AND StyleBudgetID = " + id + ")");
            oOrderRecapDetails = OrderRecapDetail.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);

            sSQL = new StringBuilder("SELECT * FROM View_PAMDetail WHERE PAMID IN (SELECT RefID FROM StyleBudgetRecap WHERE RefType=" + (int)EnumStyleBudgetRecapType.PAM + " AND StyleBudgetID = " + id + ")");
            oPAMDetails = PAMDetail.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
            foreach(PAMDetail oDItem in  oPAMDetails)
            {
                OrderRecapDetail oOrderRecapDetail = new OrderRecapDetail();
                oOrderRecapDetail.ColorID = oDItem.ColorID;
                oOrderRecapDetail.ColorName = oDItem.ColorName;
                oOrderRecapDetail.Quantity = oDItem.Quantity;
                oOrderRecapDetail.UnitPrice = _oStyleBudget.StyleBudgetRecaps.Where(x=>x.RefType ==EnumStyleBudgetRecapType.PAM & x.RefID==oDItem.PAMID).FirstOrDefault().UnitPrice;
                oOrderRecapDetail.OrderRecapNo = _oStyleBudget.StyleBudgetRecaps.Where(x => x.RefType == EnumStyleBudgetRecapType.PAM & x.RefID == oDItem.PAMID).FirstOrDefault().RefNo;
                oOrderRecapDetails.Add(oOrderRecapDetail);
            }
    
            _oStyleBudget.StyleBudgetDetails = StyleBudgetDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oStyleBudget.StyleBudgetYarnDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, true);
            _oStyleBudget.StyleBudgetAccessoriesDetails = GetSpecificDetails(_oStyleBudget.StyleBudgetDetails, false);

            //Post Costiog
            if(_oStyleBudget.PostStyleBudgetID>0)
            {
                oPostStyleBudget = oPostStyleBudget.Get(_oStyleBudget.PostStyleBudgetID, (int)Session[SessionInfo.currentUserID]);
                oPostStyleBudget.TechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(oPostStyleBudget.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                oPostStyleBudget.TechnicalSheetSizeList = TechnicalSheetSize.Gets(oPostStyleBudget.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
               oPostStyleBudget.StyleBudgetCMs = StyleBudgetCM.Gets(oPostStyleBudget.StyleBudgetID, (int)Session[SessionInfo.currentUserID]);
               oPostStyleBudget.StyleBudgetRecaps = StyleBudgetRecap.Gets(oPostStyleBudget.StyleBudgetID, (int)Session[SessionInfo.currentUserID]);
               sSQL = new StringBuilder("SELECT * FROM View_OrderRecapDetail WHERE OrderRecapID IN (SELECT RefID FROM StyleBudgetRecap WHERE RefType=" + (int)EnumStyleBudgetRecapType.OrderRecap + " AND StyleBudgetID = " + oPostStyleBudget.StyleBudgetID + ")");
               oORDlsForPostBudget = OrderRecapDetail.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);


               sSQL = new StringBuilder("SELECT * FROM View_PAMDetail WHERE PAMID IN (SELECT RefID FROM StyleBudgetRecap WHERE RefType=" + (int)EnumStyleBudgetRecapType.PAM + " AND StyleBudgetID = " + oPostStyleBudget.StyleBudgetID + ")");
               oPAMDetails = PAMDetail.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);
               foreach (PAMDetail oDItem in oPAMDetails)
               {
                   OrderRecapDetail oOrderRecapDetail = new OrderRecapDetail();
                   oOrderRecapDetail.ColorID = oDItem.ColorID;
                   oOrderRecapDetail.ColorName = oDItem.ColorName;
                   oOrderRecapDetail.Quantity = oDItem.Quantity;
                   oOrderRecapDetail.UnitPrice = oPostStyleBudget.StyleBudgetRecaps.Where(x => x.RefType == EnumStyleBudgetRecapType.PAM & x.RefID == oDItem.PAMID).FirstOrDefault().UnitPrice;
                   oOrderRecapDetail.OrderRecapNo = oPostStyleBudget.StyleBudgetRecaps.Where(x => x.RefType == EnumStyleBudgetRecapType.PAM & x.RefID == oDItem.PAMID).FirstOrDefault().RefNo;
                   oORDlsForPostBudget.Add(oOrderRecapDetail);
               }

               oPostStyleBudget.StyleBudgetDetails = StyleBudgetDetail.Gets(oPostStyleBudget.StyleBudgetID, (int)Session[SessionInfo.currentUserID]);
               oPostStyleBudget.StyleBudgetYarnDetails = GetSpecificDetails(oPostStyleBudget.StyleBudgetDetails, true);
               oPostStyleBudget.StyleBudgetAccessoriesDetails = GetSpecificDetails(oPostStyleBudget.StyleBudgetDetails, false);
            }
            
            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
          //  oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.StyleBudgetPriview, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oStyleBudget.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
           
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<BodyMeasure> oBodyMeasures = new List<BodyMeasure>();
            oBodyMeasures = BodyMeasure.Gets(id, (int)Session[SessionInfo.currentUserID]);

            rptCostingBudget oReport = new rptCostingBudget();
            byte[] abytes = oReport.PrepareReport(_oStyleBudget, oPostStyleBudget, oCompany, oBusinessUnit, oBodyMeasures, oSignatureSetups, oOrderRecapDetails, oORDlsForPostBudget);
            return File(abytes, "application/pdf");
        }
      
        public Image GetLargeImage(int id)
        {
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            oTechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "companyLogo.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetEmployees(Employee oEmployee)
        {
            List<Employee> oEmployees = new List<Employee>();
            string sSQL = "SELECT * FROM View_Employee WHERE EmployeeDesignationType = " + (int)EnumEmployeeDesignationType.Merchandiser;
            if(!String.IsNullOrEmpty(oEmployee.Name))
            {
                sSQL += " AND Name Like '%" + oEmployee.Name + "%'";
            }
            try
            {
                oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                Employee _oEmployee = new Employee();
                _oEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetStyleImageInBase64(TechnicalSheet oTechnicalSheet)
        {

            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            if (oTechnicalSheet.IsFronImage == true)
            {
                oTechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(oTechnicalSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oTechnicalSheetImage = oTechnicalSheetImage.GetBackImage(oTechnicalSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            }
            if (oTechnicalSheetImage.LargeImage == null)
            {
                oTechnicalSheetImage.LargeImage = new byte[10];
            }
            return Json(new { base64imgage = Convert.ToBase64String(oTechnicalSheetImage.LargeImage) }, JsonRequestBehavior.AllowGet);
        }
        
        #region Search Style OR Buyer by Press Enter
        [HttpGet]
        public JsonResult SearchStyleAndBuyer(string sTempData, bool bIsStyle, int BUID, int OperationType, double ts)
        {
            _oStyleBudgets = new List<StyleBudget>();
            string sSQL = "";
            if (bIsStyle == true)
            {
                sSQL = "SELECT * FROM View_StyleBudget WHERE StyleNo LIKE ('%" + sTempData + "%')";
            }
            else
            {
                sSQL = "SELECT * FROM View_StyleBudget WHERE BuyerName LIKE ('%" + sTempData + "%')";
            }
            sSQL += " AND BUID = "+BUID;
            sSQL += " AND OperationType=" + OperationType;
            #region User Set
             if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {

                sSQL += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion
            try
            {
                StyleBudget oStyleBudget = new StyleBudget();
                _oStyleBudgets = StyleBudget.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oStyleBudget = new StyleBudget();
                _oStyleBudget.ErrorMessage = ex.Message;
                _oStyleBudgets.Add(_oStyleBudget);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oStyleBudgets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
   
}
