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
	public class RMConsumptionController : Controller
	{
		#region Declaration
		RMConsumption _oRMConsumption = new RMConsumption();
		List<RMConsumption> _oRMConsumptions = new  List<RMConsumption>();
		#endregion

		#region Actions
		public ActionResult ViewRMConsumptions(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RMConsumption).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oRMConsumptions = new List<RMConsumption>();
            _oRMConsumptions = RMConsumption.Gets("SELECT * FROM View_RMConsumption AS HH WHERE ISNULL(HH.ApprovedBy,0) = 0 ORDER BY RMConsumptionID ASC", (int)Session[SessionInfo.currentUserID]);

            List<ESimSol.BusinessObjects.User> oApprovedUsers = new List<User>();
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT ApprovedBy FROM RMConsumption) ORDER BY UserName ASC";
            oApprovedUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ApprovedUsers = oApprovedUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
			return View(_oRMConsumptions);
		}

		public ActionResult ViewRMConsumption(int id)
		{
			_oRMConsumption = new RMConsumption();
			if (id > 0)
			{
				_oRMConsumption = _oRMConsumption.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oRMConsumption.RMConsumptionDetails = RMConsumptionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
			}            
            
            #region Trigger Type
            EnumObject oEnumObject = new EnumObject();
            List<EnumObject> oEnumObjects = new List<EnumObject>();
            
            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumTriggerParentsType.RouteSheet;
            oEnumObject.Value = "Yarn(Dyeline Sheet)";
            oEnumObjects.Add(oEnumObject);

            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumTriggerParentsType.RouteSheetDetail;
            oEnumObject.Value = "Dyes/Chemical(Dyeline Sheet)";
            oEnumObjects.Add(oEnumObject);

            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumTriggerParentsType.ProductionRecipe;
            oEnumObject.Value = "Raw Material(Plastic)";
            oEnumObjects.Add(oEnumObject);

            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumTriggerParentsType._FabricSalesContract_GrayIssue;
            oEnumObject.Value = "Gray Fabric Issue(Finishing)";
            oEnumObjects.Add(oEnumObject);

            oEnumObject = new EnumObject();
            oEnumObject.id = (int)EnumTriggerParentsType.S2SLotTransfer;//it may be chnage after finalize Mithela Project
            oEnumObject.Value = "Dyes/Chemical(Finishing)";
            oEnumObjects.Add(oEnumObject);
            #endregion

            ViewBag.TriggerTypes = oEnumObjects;
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oRMConsumption);
		}

        [HttpPost]
        public JsonResult GetsRawMaterial(RMConsumption oRMConsumption)
        {            
            List<ITransaction> oITransactions = new List<ITransaction>();
            RMConsumptionDetail oRMConsumptionDetail = new RMConsumptionDetail();
            List<RMConsumptionDetail> oRMConsumptionDetails = new List<RMConsumptionDetail>();
            try
            {
                //EnumTriggerParentsType :  RouteSheet/Yarn = 106, RouteSheetDetail/DyesChemicale = 107, ProductionRecipe/Plastic Raw Material = 116,
                //string sSQL = "SELECT TOP 300 * FROM View_ITransaction  AS HH WHERE HH.InOutType=102 AND HH.TriggerParentType = " + oRMConsumption.TriggeTypeInt.ToString() + " AND HH.BUID = " + oRMConsumption.BUID.ToString() + " AND CONVERT(DATE,CONVERT(VARCHAR(12),HH.TransactionTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + oRMConsumption.ConsumptionDate.ToString("dd MMM yyyy") + "',106)) AND HH.ITransactionID NOT IN (SELECT MM.ITransactionID FROM RMConsumptionDetail AS MM)";
                string sSQL =
                "SELECT	TOP 500 " +
                " HH.ITransactionID," +
                " HH.ProductID, " +
                " HH.LotID, " +
                " HH.Qty, " +
                " HH.CurrentBalance, " +
                " HH.MUnitID, " +
                " HH.UnitPrice, " +
                " HH.CurrencyID, " +
                " HH.InOutType, " +
                " HH.[Description]," +
                " HH.WorkingUnitID, " +
                " HH.TriggerParentID, " +
                " HH.TriggerParentType, " +
                " HH.[DateTime] AS TransactionTime, " +
                " HH.DBUserID," +
                " Product.ProductCode, " +
                " Product.ProductName," +
                " MeasurementUnit.UnitName, " +
                " MeasurementUnit.Symbol AS USymbol," +
                " Lot.LotNo," +
                " (SELECT NN.StoreName FROM View_WorkingUnit AS NN WHERE NN.WorkingUnitID = HH.WorkingUnitID) AS StoreName," +
                " CASE WHEN (ISNULL(HH.TriggerParentType,0)=106) THEN 'RS No-'+ISNULL((SELECT FP.RouteSheetNo FROM RouteSheet AS FP WHERE FP.RouteSheetID=HH.TriggerParentID ),'')" +
                     "WHEN (ISNULL(HH.TriggerParentType,0)=107) THEN 'RS No-'+ISNULL((SELECT FP.RouteSheetNo FROM RouteSheet AS FP WHERE FP.RouteSheetID IN (SELECT RouteSheetID FROM RouteSheetDetail WHERE RouteSheetDetailID=HH.TriggerParentID )),'')			 " +
                     "WHEN (ISNULL(HH.TriggerParentType,0)=116) THEN 'SheetNo-'+ISNULL((SELECT FP.SheetNo FROM ProductionSheet AS FP WHERE FP.ProductionSheetID IN (SELECT ProductionSheetID FROM ProductionRecipe AS NN WHERE NN.ProductionRecipeID =HH.TriggerParentID )),'')" +
                " END AS RefNo " +

                " FROM ITransaction AS HH" +
                " INNER JOIN Lot ON HH.LotID = Lot.LotID" +
                " INNER JOIN Product ON HH.ProductID = Product.ProductID" +
                " INNER JOIN	MeasurementUnit ON HH.MUnitID = MeasurementUnit.MeasurementUnitID" +
                " WHERE HH.InOutType=102"
                  + ((oRMConsumption.TriggeTypeInt == (int)EnumTriggerParentsType.RouteSheet) ? " AND HH.WorkingUnitID not in (Select RouteSheetSetup.WorkingUnitIDWIP from RouteSheetSetup)" : "") +
                " AND HH.TriggerParentType =" + oRMConsumption.TriggeTypeInt.ToString() +
                " AND Lot.BUID=" + oRMConsumption.BUID.ToString() +
                " AND CONVERT(DATE,CONVERT(VARCHAR(12),HH.[DateTime],106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + oRMConsumption.ConsumptionDate.ToString("dd MMM yyyy") + "',106))" +
                " AND HH.ITransactionID NOT IN (SELECT MM.ITransactionID FROM RMConsumptionDetail AS MM)";

                oITransactions = ITransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (ITransaction oItem in oITransactions)
                {
                    oRMConsumptionDetail = new RMConsumptionDetail();
                    oRMConsumptionDetail.RMConsumptionDetailID = 0;
                    oRMConsumptionDetail.RMConsumptionID = 0;
                    oRMConsumptionDetail.ITransactionID = oItem.ITransactionID;
                    oRMConsumptionDetail.ProductID = oItem.ProductID;
                    oRMConsumptionDetail.LotID = oItem.LotID;
                    oRMConsumptionDetail.WUID = oItem.WorkingUnitID;
                    oRMConsumptionDetail.MUnitID = oItem.MUnitID;
                    oRMConsumptionDetail.Qty = oItem.Qty;
                    oRMConsumptionDetail.UnitPrice = oItem.UnitPrice;
                    oRMConsumptionDetail.Amount = (oItem.UnitPrice * oItem.Qty);
                    oRMConsumptionDetail.ProductCode = oItem.ProductCode;
                    oRMConsumptionDetail.ProductName = oItem.ProductName;
                    oRMConsumptionDetail.WUName = oItem.StoreName;
                    oRMConsumptionDetail.MUName = oItem.USymbol;
                    oRMConsumptionDetail.LotNo = oItem.LotNo;
                    oRMConsumptionDetail.RefNo = oItem.RefNo;
                    oRMConsumptionDetails.Add(oRMConsumptionDetail);
                }
            }
            catch (Exception ex)
            {
                oRMConsumptionDetail = new RMConsumptionDetail();
                oRMConsumptionDetail.ErrorMessage = ex.Message;
                oRMConsumptionDetails = new List<RMConsumptionDetail>();
                oRMConsumptionDetails.Add(oRMConsumptionDetail);
            }

            var jsonResult = Json(oRMConsumptionDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

		[HttpPost]
		public JsonResult Save(RMConsumption oRMConsumption)
		{
			_oRMConsumption = new RMConsumption();
			try
			{
				_oRMConsumption = oRMConsumption;
				_oRMConsumption = _oRMConsumption.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oRMConsumption = new RMConsumption();
				_oRMConsumption.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oRMConsumption);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult GetSuggestMaterialConsumptionDate(RMConsumption oRMConsumption)
        {

            _oRMConsumption = new RMConsumption();
            try
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "SELECT ISNULL(MIN(HH.TransactionTime),GETDATE()) AS SuggestDate FROM View_ITransaction  AS HH";
                string sWhereCluse = "";
                if (oCompany.BaseAddress.ToUpper() == "MITHELA")
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " HH.TransactionTime>='01 Dec 2018'";
                }
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + "HH.InOutType=102 AND HH.TriggerParentType = " + oRMConsumption.TriggeTypeInt.ToString() + " AND HH.BUID = " + oRMConsumption.BUID.ToString() + " AND HH.ITransactionID NOT IN (SELECT MM.ITransactionID FROM RMConsumptionDetail AS MM)";
                sSQL = sSQL + sWhereCluse;
                _oRMConsumption = _oRMConsumption.GetSuggestMaterialConsumptionDate(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRMConsumption = new RMConsumption();
                _oRMConsumption.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRMConsumption);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult YetToMaterialConsumptionDate(RMConsumption oRMConsumption)
        {
            string sYetToEffectDate = "";
            try
            {
                string sSQL = "SELECT DISTINCT TOP 30 CONVERT(VARCHAR(12),HH.TransactionTime,106) AS SuggestDate FROM View_ITransaction  AS HH WHERE HH.TransactionTime IS NOT NULL AND HH.InOutType=102 AND HH.TriggerParentType = " + oRMConsumption.TriggeTypeInt.ToString() + " AND HH.BUID = " + oRMConsumption.BUID.ToString() + " AND HH.ITransactionID NOT IN (SELECT MM.ITransactionID FROM RMConsumptionDetail AS MM)";
                sYetToEffectDate = _oRMConsumption.YetToMaterialConsumptionDate(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sYetToEffectDate = "";
                sYetToEffectDate = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sYetToEffectDate);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 


		[HttpPost]
        public JsonResult Delete(RMConsumption oRMConsumption)
		{
			string sFeedBackMessage = "";
			try
			{				
				sFeedBackMessage = oRMConsumption.Delete(oRMConsumption.RMConsumptionID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approved(RMConsumption oRMConsumption)
        {            
            try
            {
                oRMConsumption = oRMConsumption.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRMConsumption = new RMConsumption();
                oRMConsumption.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRMConsumption);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 
		#endregion

        #region Advance Search
        [HttpPost]
        public JsonResult GetsByConsumptionNo(RMConsumption oRMConsumption)
        {
            List<RMConsumption> oRMConsumptions = new List<RMConsumption>();            
            try
            {
                string sSQL = "SELECT * FROM View_RMConsumption AS HH WHERE HH.ConsumptionNo LIKE '%" + oRMConsumption.ConsumptionNo + "%' ORDER BY HH.RMConsumptionID ASC";                
                oRMConsumptions = RMConsumption.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRMConsumptions = new List<RMConsumption>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRMConsumptions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvanceSearch(RMConsumption oRMConsumption)
        {
            _oRMConsumptions = new List<RMConsumption>();
            try
            {
                string sSQL = this.GetSQL(oRMConsumption);
                _oRMConsumptions = RMConsumption.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRMConsumption = new RMConsumption();
                _oRMConsumptions = new List<RMConsumption>();
                _oRMConsumption.ErrorMessage = ex.Message;
                _oRMConsumptions.Add(_oRMConsumption);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRMConsumptions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(RMConsumption oRMConsumption)
        {
            EnumCompareOperator eConsumptionDate = (EnumCompareOperator)Convert.ToInt32(oRMConsumption.Remarks.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(oRMConsumption.Remarks.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(oRMConsumption.Remarks.Split('~')[2]);
            string sRemarks = oRMConsumption.Remarks.Split('~')[3];
            
            string sReturn1 = "SELECT * FROM View_RMConsumption";
            string sReturn = "";

            #region BUID
            if (oRMConsumption.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oRMConsumption.BUID.ToString();
            }
            #endregion

            #region ConsumptionNo
            oRMConsumption.ConsumptionNo = oRMConsumption.ConsumptionNo == null ? "" : oRMConsumption.ConsumptionNo;
            if (oRMConsumption.ConsumptionNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ConsumptionNo LIKE '%" + oRMConsumption.ConsumptionNo + "%'";
            }
            #endregion
            
            #region RMConsumption Date
            if (eConsumptionDate != EnumCompareOperator.None)
            {
                if (eConsumptionDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ConsumptionDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eConsumptionDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ConsumptionDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eConsumptionDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ConsumptionDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eConsumptionDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ConsumptionDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eConsumptionDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ConsumptionDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eConsumptionDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ConsumptionDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region ApprovedBy
            if (oRMConsumption.ApprovedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApprovedBy,0) = " + oRMConsumption.ApprovedBy.ToString();
            }
            #endregion

            #region Remarks
            if (sRemarks != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Remarks LIKE '%" + sRemarks + "%'";
            }
            #endregion

            sReturn = sReturn1 + sReturn+ " ORDER BY RMConsumptionID ASC";
            return sReturn;
        }
        #endregion

        #region Print RMConsumptions
        public ActionResult PrintRMConsumptions(string Param)
        {
            _oRMConsumptions = new List<RMConsumption>();
            string sSQLQuery = "SELECT * FROM View_RMConsumption AS HH WHERE HH.RMConsumptionID IN (" + Param + ") ORDER BY HH.RMConsumptionID ASC";
            _oRMConsumptions = RMConsumption.Gets(sSQLQuery, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptRMConsumptions oReport = new rptRMConsumptions();
            byte[] abytes = oReport.PrepareReport(_oRMConsumptions, oCompany);
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
