using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
	public class RMRequisitionController : Controller
	{
		#region Declaration

		RMRequisition _oRMRequisition = new RMRequisition();
		List<RMRequisition> _oRMRequisitions = new  List<RMRequisition>();
		#endregion

		#region Functions

		#endregion

		#region Actions
        public ActionResult ViewRMRequisitionList(int buid, int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RMRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oRMRequisitions = new List<RMRequisition>();
            _oRMRequisitions = RMRequisition.Gets("SELECT * FROM View_RMRequisition AS HH WHERE HH.BUID = " + buid.ToString() + " AND ISNULL(HH.ApprovedBy,0) = 0 ORDER BY HH.RequisitionDate ASC", (int)Session[SessionInfo.currentUserID]); 
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
			return View(_oRMRequisitions);
		}
		public ActionResult ViewRMRequisition(int id)
		{
			_oRMRequisition = new RMRequisition();
			if (id > 0)
			{
				_oRMRequisition = _oRMRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oRMRequisition.RMRequisitionSheets = RMRequisitionSheet.Gets(id, (int)Session[SessionInfo.currentUserID]);
                foreach (RMRequisitionSheet oItem in _oRMRequisition.RMRequisitionSheets)
                {
                    oItem.YetToPPQty += oItem.PPQty;
                }
                _oRMRequisition.RMRequisitionMaterials = RMRequisitionMaterial.Gets(id, (int)Session[SessionInfo.currentUserID]);

			}
			return View(_oRMRequisition);
		}

        public ActionResult ViewRMRequisitionRevise(int id)
		{
			_oRMRequisition = new RMRequisition();
			if (id > 0)
			{
				_oRMRequisition = _oRMRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oRMRequisition.RMRequisitionSheets = RMRequisitionSheet.Gets(id, (int)Session[SessionInfo.currentUserID]);
                foreach (RMRequisitionSheet oItem in _oRMRequisition.RMRequisitionSheets)
                {
                    oItem.YetToPPQty += oItem.PPQty;
                }
                _oRMRequisition.RMRequisitionMaterials = RMRequisitionMaterial.Gets(id, (int)Session[SessionInfo.currentUserID]);

			}
			return View(_oRMRequisition);
		}


        [HttpPost]
        public JsonResult AcceptRevise(RMRequisition oRMRequisition)
        {
            _oRMRequisition = new RMRequisition();
            List<RMRequisitionMaterial> oRMRequisitionMaterials = new List<RMRequisitionMaterial>();
            //oRMRequisitionMaterials = oRMRequisition.RMRequisitionMaterials;

            try
            {
                oRMRequisitionMaterials = RMRequisitionMaterial.Gets(oRMRequisition.RMRequisitionID, (int)Session[SessionInfo.currentUserID]);
                foreach (RMRequisitionMaterial oItem in oRMRequisition.RMRequisitionMaterials)
                {
                    RMRequisitionMaterial oTempItem = new RMRequisitionMaterial();
                    oTempItem = oRMRequisitionMaterials.Where(x => x.RMRequisitionMaterialID == oItem.RMRequisitionMaterialID).FirstOrDefault();
                    double nDiff = oItem.Qty - oTempItem.MaterialOutQty;
                    if (nDiff <0)
                    {
                        if (nDiff * -1 > 0.001)
                        {
                            throw new Exception("Sorry,Can't Accept Revise, Because Already Material Out Qty is Greater than Requisition Qty. for Raw Material :[" + oTempItem.ProductCode+"]"+ oTempItem.ProductName);
                        }
                    }
                    //if(oItem.RMRequisitionMaterialID)
                }

                _oRMRequisition = oRMRequisition;
                _oRMRequisition = _oRMRequisition.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRMRequisition = new RMRequisition();
                _oRMRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRMRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


		[HttpPost]
		public JsonResult Save(RMRequisition oRMRequisition)
		{
			_oRMRequisition = new RMRequisition();
			try
			{
				_oRMRequisition = oRMRequisition;
				_oRMRequisition = _oRMRequisition.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oRMRequisition = new RMRequisition();
				_oRMRequisition.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oRMRequisition);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult Approve(RMRequisition oRMRequisition)
        {
            _oRMRequisition = new RMRequisition();
            try
            {
                _oRMRequisition = oRMRequisition;
                _oRMRequisition = _oRMRequisition.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRMRequisition = new RMRequisition();
                _oRMRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRMRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
        public JsonResult Delete(RMRequisition oRMRequisition)
		{
			string sFeedBackMessage = "";
			try
			{
                sFeedBackMessage = oRMRequisition.Delete(oRMRequisition.RMRequisitionID, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				sFeedBackMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(sFeedBackMessage);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult GetRMRequisitionSheets(ProductionSheet oProductionSheet)
        {
            List<ProductionSheet> oProductionSheets = new List<ProductionSheet>();
            List<RMRequisitionSheet> oRMRequisitionSheets = new List<RMRequisitionSheet>();
            RMRequisitionSheet oRMRequisitionSheet = new RMRequisitionSheet();
            try
            {
                string sSQL = "SELECT * FROM View_ProductionSheet AS PS WHERE PS.BUID = " + oProductionSheet.BUID + " AND  ISNULL(PS.YetToPlanQty,0)>0 AND ISNULL(SheetStatus,0)="+(int)EnumProductionSheetStatus.Production_In_Progress;
                if(!string.IsNullOrEmpty(oProductionSheet.SheetNo))
                {
                    sSQL += " AND SheetNo LIKE '%"+oProductionSheet.SheetNo+"%'";
                }
                sSQL += " Order BY  ProductionSheetID";
                oProductionSheets = ProductionSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach(ProductionSheet oItem in oProductionSheets)
                {
                    oRMRequisitionSheet = new RMRequisitionSheet();
                    oRMRequisitionSheet.ProductionSheetID = oItem.ProductionSheetID;
                    oRMRequisitionSheet.PPQty = oItem.YetToPlanQty;
                    oRMRequisitionSheet.YetToPPQty = oItem.YetToPlanQty;
                    oRMRequisitionSheet.SheetNo = oItem.SheetNo;
                    oRMRequisitionSheet.ProductCode = oItem.ProductCode;
                    oRMRequisitionSheet.ProductName = oItem.ProductName;
                    oRMRequisitionSheet.SheetQty = oItem.Quantity;
                    oRMRequisitionSheet.UnitSymbol = oItem.UnitSymbol;
                    oRMRequisitionSheets.Add(oRMRequisitionSheet);
                }
            }
            catch (Exception ex)
            {
                
                oRMRequisitionSheet = new RMRequisitionSheet();
                oRMRequisitionSheet.ErrorMessage = ex.Message;
                oRMRequisitionSheets.Add(oRMRequisitionSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRMRequisitionSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsRMRequisitionMaterial(RMRequisitionMaterial oRMRequisitionMaterial)
        {
            Product oProduct = new Product();
            List<Product> oProducts = new List<Product>();
            try
            {
                string sSQL = "SELECT * FROM View_Product AS HH WHERE HH.ProductBaseID = (SELECT MM.ProductBaseID FROM Product AS MM WHERE MM.ProductID=" + oRMRequisitionMaterial.ProductID.ToString() + ") ORDER BY HH.ProductName ASC";
                oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRMRequisitionMaterials(ProductionRecipe oProductionRecipe)
        {
            List<ProductionRecipe> oProductionRecipes = new List<ProductionRecipe>();
            List<RMRequisitionMaterial> oRMRequisitionMaterials = new List<RMRequisitionMaterial>();
            RMRequisitionMaterial oRMRequisitionMaterial = new RMRequisitionMaterial();
            try
            {
                string sSQL = "SELECT * FROM View_ProductionRecipe WHERE ProductionSheetID IN (" + oProductionRecipe.Remarks+ ") Order BY ProductionSheetID, ProductID";
                oProductionRecipes = ProductionRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach(ProductionRecipe oItem in oProductionRecipes)
                {
                    oRMRequisitionMaterial = new RMRequisitionMaterial();
                    oRMRequisitionMaterial.ProductionSheetID = oItem.ProductionSheetID;
                    oRMRequisitionMaterial.ProductionRecipeID = oItem.ProductionRecipeID;
                    oRMRequisitionMaterial.SheetNo = oItem.SheetNo;
                    oRMRequisitionMaterial.ProductID = oItem.ProductID;
                    oRMRequisitionMaterial.ProductCode = oItem.ProductCode;
                    oRMRequisitionMaterial.ProductName = oItem.ProductName;
                    oRMRequisitionMaterial.MUName = oItem.MUName;
                    oRMRequisitionMaterial.Qty = oItem.YetToRequisitionQty;
                    oRMRequisitionMaterial.RequiredQty = oItem.RequiredQty;
                    oRMRequisitionMaterial.YetToRequisitionQty = oItem.YetToRequisitionQty;
                    oRMRequisitionMaterial.QtyType = oItem.QtyType;
                    oRMRequisitionMaterial.QtyTypeInt = oItem.QtyTypeInt;
                    oRMRequisitionMaterial.QtyInPercent = oItem.QtyInPercent;
                    oRMRequisitionMaterial.MUSymbol = oItem.MUSymbol;
                    oRMRequisitionMaterials.Add(oRMRequisitionMaterial);
                }
            }
            catch (Exception ex)
            {
                oRMRequisitionMaterial = new RMRequisitionMaterial();
                oRMRequisitionMaterial.ErrorMessage = ex.Message;
                oRMRequisitionMaterials.Add(oRMRequisitionMaterial);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRMRequisitionMaterials);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRMRequisitions(RMRequisition oRMRequisition)
        {
            List<RMRequisition> oRMRequisitions = new List<RMRequisition>();   
            try
            {
                string sSQL = "";
                if (oRMRequisition.RefNo == null) { oRMRequisition.RefNo = ""; }
                if (oRMRequisition.RefNo == "")
                {
                    sSQL = "SELECT * FROM View_RMRequisition AS PS WHERE PS.BUID = " + oRMRequisition.BUID + " ORDER BY PS.RMRequisitionID ASC";
                }
                else
                {
                    sSQL = "SELECT * FROM View_RMRequisition AS PS WHERE PS.BUID = " + oRMRequisition.BUID + " AND (ISNULL(PS.PINo,'')+ISNULL(PS.RefNo,'')) LIKE '%" + oRMRequisition.RefNo + "%'  ORDER BY PS.RMRequisitionID ASC";
                }
                oRMRequisitions = RMRequisition.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRMRequisition = new RMRequisition();
                oRMRequisition.ErrorMessage = ex.Message;
                oRMRequisitions.Add(oRMRequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRMRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSheets(RMRequisition oRMRequisition)
        {
            List<RMRequisitionSheet> oRMRequisitionSheets = new List<RMRequisitionSheet>();
            try
            {
                string sSQL = "SELECT * FROM View_RMRequisitionSheet AS PS WHERE PS.RMRequisitionID = " + oRMRequisition.RMRequisitionID ;
                oRMRequisitionSheets = RMRequisitionSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                RMRequisitionSheet oRMRequisitionSheet = new RMRequisitionSheet();
                oRMRequisitionSheet.ErrorMessage = ex.Message;
                oRMRequisitionSheets.Add(oRMRequisitionSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRMRequisitionSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

		#endregion

        #region Search
        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }
        #region HttpGet For Search
        [HttpGet]
        public JsonResult Search(string sTemp)
        {
            List<RMRequisition> oRMRequisitions = new List<RMRequisition>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oRMRequisitions = RMRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRMRequisition = new RMRequisition();
                _oRMRequisition.ErrorMessage = ex.Message;
                oRMRequisitions.Add(_oRMRequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRMRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            #region Splited Data
            //Issue Date
            int nProcessCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dProcessStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dProcessEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sRefNo = sTemp.Split('~')[3];
            string sProductIDs = sTemp.Split('~')[4];
            string sProductionSheetIDs = sTemp.Split('~')[5];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[6]);
            #endregion

            string sReturn1 = "SELECT * FROM View_RMRequisition";
            string sReturn = "";

            #region REf No

            if (sRefNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo ='" + sRefNo + "'";

            }
            #endregion

            #region Product wise
            if (sProductIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RMRequisitionID IN ( SELECt RMRequisitionID FROM RMRequisitionSheet WHERE ProductID IN (" + sProductIDs + "))";
            }
            #endregion

            #region Production Sheet
            if (sProductionSheetIDs != "")
            {
                Global.TagSQL(ref sReturn);
             sReturn = sReturn + " RMRequisitionID IN ( SELECt RMRequisitionID FROM RMRequisitionSheet WHERE ProductionSheetID IN (" + sProductionSheetIDs + "))";
            }
            #endregion

            #region Process Date Wise
            if (nProcessCreateDateCom > 0)
            {
                if (nProcessCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,RequisitionDate,106)  = Convert(Date,'" + dProcessStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nProcessCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,RequisitionDate,106)  != Convert(Date,'" + dProcessStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nProcessCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,RequisitionDate,106)  > Convert(Date,'" + dProcessStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nProcessCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,RequisitionDate,106)  < Convert(Date,'" + dProcessStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nProcessCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,RequisitionDate,106) >= Convert(Date,'" + dProcessStartDate.ToString("dd MMM yyyy") + "',106)  AND Convert(Date,RequisitionDate,106)  < Convert(Date,'" + dProcessEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
                if (nProcessCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,RequisitionDate,106) < Convert(Date,'" + dProcessStartDate.ToString("dd MMM yyyy") + "',106) OR Convert(Date,RequisitionDate,106)  > Convert(Date,'" + dProcessEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
            }
            #endregion
           
            #region BU
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " BUID = " + nBUID;
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion
        #endregion

        #region PrintList
        [HttpPost]
        public ActionResult SetProductionSheetListData(RMRequisition oRMRequisition)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oRMRequisition);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductionSheetRequiredRawMaterial(int nBUID)//for plastic
        {
            _oRMRequisition = new RMRequisition();
            
            Company oCompany = new Company();
            List<ProductionProcedure> _oProductionProcedures = new List<ProductionProcedure>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oRMRequisition = (RMRequisition)Session[SessionInfo.ParamObj];
            string sSQL = "";
            if (nBUID > 0)
            {
                sSQL = "SELECT * FROM View_RMRequisitionMaterial WHERE RMRequisitionID IN (" + _oRMRequisition.Remarks + ") Order By ProductionSheetID, ProductID";
                _oRMRequisition.RMRequisitionMaterials = RMRequisitionMaterial.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            _oRMRequisition.BusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oRMRequisition.Company = oCompany;

            byte[] abytes;
            _oRMRequisition.ErrorMessage = (string)Session[SessionInfo.currentUserName];
            rptProductionSheetRequiredRawMaterial oReport = new rptProductionSheetRequiredRawMaterial();
            abytes = oReport.PrepareReport(_oRMRequisition);
            return File(abytes, "application/pdf");
        }
        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion

    }

}
