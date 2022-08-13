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
	public class FNRequisitionController : Controller
	{
		#region Declaration

		FNRequisition _oFNRequisition = new FNRequisition();
		List<FNRequisition> _oFNRequisitions = new  List<FNRequisition>();
        FNRequisitionDetail _oFNRequisitionDetail = new FNRequisitionDetail();
        List<FNRequisitionDetail> _oFNRequisitionDetails = new List<FNRequisitionDetail>();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewFNRequisitions(int TreatmentProcess, int menuid, int buid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FNRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
           
			_oFNRequisitions = new List<FNRequisition>();

            List<AuthorizationUserOEDO> oAUOEDOs = new List<AuthorizationUserOEDO>();
            string sSQL = "SELECT * FROM View_AuthorizationUserOEDO Where DBObjectName='FNRequisitionDetail' And UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "";
            sSQL = "SELECT * FROM View_FNRequisition WHERE (ReceiveBy IS NULL OR ReceiveBy<=0) AND RequestBy= " + (int)Session[SessionInfo.currentUserID];
            if(oAUOEDOs.Count>0)//if has Permission for disburse by
            {
                sSQL += " OR ISNULL(DisburseBy,0)=0";
            }
            else
            {
                sSQL += " AND ISNULL(ReceiveBy,0)=0";
            }
            if (TreatmentProcess == 0)
            {
                sSQL += " AND ApproveBy is not null  AND ApproveBy!=0";
            }
            else
            {
                sSQL += " AND TreatmentID=" + TreatmentProcess;
            }
            _oFNRequisitions = FNRequisition.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            Dictionary<string, bool> hasPermisson = new Dictionary<string, bool>();
            hasPermisson.Add("Disburse", ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Disburse, "FNRequisitionDetail", oAUOEDOs));
            ViewBag.buid = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.Permisson = hasPermisson;
            ViewBag.TreatmentProcess = TreatmentProcess;
            ViewBag.CurrentUserID = ((User)(Session[SessionInfo.CurrentUser])).UserID;
            _oFNRequisitions = _oFNRequisitions.Where(x => x.DisburseBy == 0).ToList();
			return View(_oFNRequisitions);
		}
		public ActionResult ViewFNRequisition(int id,int TreatmentId, int buid)
		{
			_oFNRequisition = new FNRequisition();
			if (id > 0)
			{
				_oFNRequisition = _oFNRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oFNRequisition.FNRequisitionDetails = FNRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                foreach(FNRequisitionDetail oitem in _oFNRequisition.FNRequisitionDetails)
                {
                    if (oitem.DisburseQty <= 0) { oitem.DisburseQty = oitem.RequiredQty; }
                }
                
			}
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment));
            ViewBag.Shades = EnumObject.jGets(typeof(EnumShade));
            ViewBag.Shifts = EnumObject.jGets(typeof(EnumShift));
            ViewBag.TreatmentProcess = TreatmentId;
            ViewBag.BUID = buid;
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(0, EnumModuleName.FNRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);// WorkingUnit.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.WorkingUnitReceives = WorkingUnit.GetsPermittedStore(0, EnumModuleName.FNRequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);// WorkingUnit.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFNRequisition);
		}

        public ActionResult ViewFNRequisitions_Open(int TreatmentProcess, int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFNRequisitions = new List<FNRequisition>();

            List<AuthorizationUserOEDO> oAUOEDOs = new List<AuthorizationUserOEDO>();
            string sSQL = "SELECT * FROM View_AuthorizationUserOEDO Where DBObjectName='FNRequisitionDetail' And UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "";
            sSQL = "SELECT * FROM View_FNRequisition_Open WHERE (ReceiveBy IS NULL OR ReceiveBy<=0) AND RequestBy= " + (int)Session[SessionInfo.currentUserID];
            if (oAUOEDOs.Count > 0)//if has Permission for disburse by
            {
                sSQL += " OR ISNULL(DisburseBy,0)=0";
            }
            else
            {
                sSQL += " AND ISNULL(ReceiveBy,0)=0";
            }
            if (TreatmentProcess == 0)
            {
                sSQL += " AND ApproveBy is not null  AND ApproveBy!=0";
            }
            else
            {
                sSQL += " AND TreatmentID=" + TreatmentProcess;
            }
            _oFNRequisitions = FNRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            Dictionary<string, bool> hasPermisson = new Dictionary<string, bool>();
            hasPermisson.Add("Disburse", ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Disburse, "FNRequisitionDetail", oAUOEDOs));
            ViewBag.buid = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.Permisson = hasPermisson;
            ViewBag.TreatmentProcess = TreatmentProcess;
            ViewBag.CurrentUserID = ((User)(Session[SessionInfo.CurrentUser])).UserID;
            _oFNRequisitions = _oFNRequisitions.Where(x => x.DisburseBy == 0).ToList();
            return View(_oFNRequisitions);
        }
        public ActionResult ViewFNRequisition_Open(int id, int TreatmentId, int buid)
        {
            _oFNRequisition = new FNRequisition();
            _oFNRequisition.IsRequisitionOpen = true;
            if (id > 0)
            {
                _oFNRequisition = _oFNRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oFNRequisition.FNRequisitionDetails = FNRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment));
            ViewBag.Shades = EnumObject.jGets(typeof(EnumShade));
            ViewBag.Shifts = EnumObject.jGets(typeof(EnumShift));
            ViewBag.TreatmentProcess = TreatmentId;
            ViewBag.BUID = buid;
            ViewBag.IsRequisitionOpen = true;
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FNRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);// WorkingUnit.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.WorkingUnitReceives = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FNRequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);// WorkingUnit.BUWiseGets(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFNRequisition);
        }


		[HttpPost]
		public JsonResult Save(FNRequisition oFNRequisition)
	{
			_oFNRequisition = new FNRequisition();
            FNRequisitionDetail oFNRequisitionDetail = new FNRequisitionDetail();
			try
			{
				_oFNRequisition = oFNRequisition;
				_oFNRequisition = _oFNRequisition.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFNRequisition = new FNRequisition();
				_oFNRequisition.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFNRequisition);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        public JsonResult Approve(FNRequisition oFNRequisition)
        {
            _oFNRequisition = new FNRequisition();
            try
            {
                _oFNRequisition = oFNRequisition;
                _oFNRequisition = _oFNRequisition.Approval(true, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRequisition = new FNRequisition();
                _oFNRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Receive(FNRequisition oFNRequisition)
        {
            _oFNRequisition = new FNRequisition();
            try
            {
                _oFNRequisition = oFNRequisition;
                _oFNRequisition = _oFNRequisition.Received((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRequisition = new FNRequisition();
                _oFNRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Disburse(FNRequisition oFNRequisition)
        {
            _oFNRequisition = new FNRequisition();
            try
            {
                _oFNRequisition = oFNRequisition;
                _oFNRequisition = _oFNRequisition.Disburse((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRequisition = new FNRequisition();
                _oFNRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //UndoApprove
        [HttpPost]
        public JsonResult UndoApprove(FNRequisition oFNRequisition)
        {
            _oFNRequisition = new FNRequisition();
            try
            {
                _oFNRequisition = oFNRequisition;
                _oFNRequisition = _oFNRequisition.Approval(false, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRequisition = new FNRequisition();
                _oFNRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
        public JsonResult Delete(FNRequisition oFNRequisition)
		{
			string sFeedBackMessage = "";
			try
			{
                sFeedBackMessage = oFNRequisition.Delete(oFNRequisition.FNRID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult SaveFNRDetail(FNRequisitionDetail oFNRequisitionDetail)
        {
            try
            {
                oFNRequisitionDetail = oFNRequisitionDetail.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oFNRequisitionDetail = new FNRequisitionDetail();
                oFNRequisitionDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNRequisitionDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetLotsOrder(FNRequisitionDetail oFNRequisitionDetail)
        {
            Lot oLot = new Lot();
            List<Lot> oLots = new List<Lot>();

            try
            {
                if(!string.IsNullOrEmpty(oFNRequisitionDetail.ErrorMessage))
                {
                    int WorkingUnitID = Convert.ToInt32(oFNRequisitionDetail.ErrorMessage.Split('~')[0]);
                    int ProductID = Convert.ToInt32(oFNRequisitionDetail.ErrorMessage.Split('~')[1]);

                    oLots = Lot.Gets("select * from View_Lot Where ProductID = '" + ProductID + "' AND WorkingUnitID = '" + WorkingUnitID + "'", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch(Exception ex)
            {
                oLot.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFNRDetail(FNRequisitionDetail oFNPC)
        {
            string sMessage = "";
            try
            {
                if (oFNPC.FNRDetailID >0)
                {
                sMessage = oFNPC.Delete(oFNPC.FNRDetailID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNPC = new FNRequisitionDetail();
                sMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

		#endregion

        #region Search
        [HttpPost]
        public JsonResult AdvSearch(FNRequisition oFNRequisition)
        {
            _oFNRequisitions = new List<FNRequisition>();
            string sSQL = "SELECT * FROM View_FNRequisition";
            string sTemp = "";
            try
            {
                int nCreateDateCom = Convert.ToInt32(oFNRequisition.ErrorMessage.Split('~')[0]);
                DateTime dStartDate = Convert.ToDateTime(oFNRequisition.ErrorMessage.Split('~')[1]);
                DateTime dEndDate = Convert.ToDateTime(oFNRequisition.ErrorMessage.Split('~')[2]);

                if (nCreateDateCom > 0)
                {
                    Global.TagSQL(ref sTemp);
                    if (nCreateDateCom == 1)
                    {
                        sTemp += " RequestDate = '" + dStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nCreateDateCom == 2)
                    {

                        sTemp = sTemp + " RequestDate != '" + dStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nCreateDateCom == 3)
                    {

                        sTemp = sTemp + " RequestDate > '" + dStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nCreateDateCom == 4)
                    {
                        sTemp = sTemp + " RequestDate < '" + dStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nCreateDateCom == 5)
                    {
                        sTemp = sTemp + " RequestDate>= '" + dStartDate.ToString("dd MMM yyyy") + "' AND RequestDate < '" + dEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    }
                    if (nCreateDateCom == 6)
                    {
                        sTemp = sTemp + " RequestDate < '" + dStartDate.ToString("dd MMM yyyy") + "' OR RequestDate > '" + dEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    }
                }

                //if (oFNRequisition.IsRequisitionOpen) 
                //{
                //    Global.TagSQL(ref sTemp);
                //    sTemp = sTemp + " ISNULL(IsRequisitionOpen,0) = 1";
                //}
                //else
                //{
                //    Global.TagSQL(ref sTemp);
                //    sTemp = sTemp + " ISNULL(IsRequisitionOpen,0) = 0";
                //}

                sSQL += sTemp;
                _oFNRequisitions = FNRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRequisition = new FNRequisition();
                _oFNRequisition.ErrorMessage = ex.Message;
                _oFNRequisitions.Add(_oFNRequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Search(FNRequisition oFNRequisition)
        {
            int nCreateDateCom = Convert.ToInt32(oFNRequisition.sParam.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(oFNRequisition.sParam.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(oFNRequisition.sParam.Split('~')[2]);
            string sDispoNo = Convert.ToString(oFNRequisition.sParam.Split('~')[3]);
            //string sFNRNo = "";

            //if (oFNRequisition.sParam.Split('~').Length > 4) 
            //{
            //    sFNRNo = Convert.ToString(oFNRequisition.sParam.Split('~')[4]);
            //}

            _oFNRequisitions = new List<FNRequisition>();
            string sSQL = "SELECT * FROM View_FNRequisition";
            string sTemp = "";
            try
            {
                if (!string.IsNullOrEmpty(oFNRequisition.FNExONo) && oFNRequisition.FNExONo != "undefined")
                {
                    Global.TagSQL(ref sTemp);
                    sTemp += " FNExONo LIKE '%" +oFNRequisition.FNExONo+ "%'";
                }
                if (!string.IsNullOrEmpty(oFNRequisition.FNRNo) && oFNRequisition.FNRNo != "undefined")
                {
                    Global.TagSQL(ref sTemp);
                    sTemp += " FNRNo LIKE '%" + oFNRequisition.FNRNo + "%'";
                }

                if (!string.IsNullOrEmpty(sDispoNo) && sDispoNo != "undefined")
                {
                    Global.TagSQL(ref sTemp);
                    sTemp += " ExeNoFull LIKE '%" + sDispoNo + "%'";
                }
                

                //if (oFNRequisition.IsRequisitionOpen)
                //{
                //    Global.TagSQL(ref sTemp);
                //    sTemp += " ISNULL(IsRequisitionOpen,0) = 1";
                //}
                //else 
                //{
                //    Global.TagSQL(ref sTemp);
                //    sTemp += " ISNULL(IsRequisitionOpen,0) = 0";
                //}


                if (nCreateDateCom > 0)
                {
                   Global.TagSQL(ref sTemp);
                   if (nCreateDateCom == 1)
                   {
                       sTemp += " RequestDate = '" + dStartDate.ToString("dd MMM yyyy") + "'";
                   }
                   if (nCreateDateCom == 2)
                   {

                       sTemp = sTemp + " RequestDate != '" + dStartDate.ToString("dd MMM yyyy")  + "'";
                   }
                   if (nCreateDateCom == 3)
                   {

                       sTemp = sTemp + " RequestDate > '" + dStartDate.ToString("dd MMM yyyy")  + "'";
                   }
                   if (nCreateDateCom == 4)
                   {
                       sTemp = sTemp + " RequestDate < '" + dStartDate.ToString("dd MMM yyyy")  + "'";
                   }
                   if (nCreateDateCom == 5)
                   {
                       sTemp = sTemp + " RequestDate>= '" + dStartDate.ToString("dd MMM yyyy") + "' AND RequestDate < '" + dEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                   }
                   if (nCreateDateCom == 6)
                   {
                       sTemp = sTemp + " RequestDate < '" + dStartDate.ToString("dd MMM yyyy") + "' OR RequestDate > '" + dEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                   }
                }
                sSQL += sTemp;
                _oFNRequisitions = FNRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRequisition = new FNRequisition();
                _oFNRequisition.ErrorMessage = ex.Message;
                _oFNRequisitions.Add(_oFNRequisition);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oFNRequisitions);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jSonResult = Json(_oFNRequisitions, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        #endregion

        #region Get Products from Stock Floor
        [HttpPost]
        public JsonResult GetProductsByNameFROMFNReq(ProductBase oProductBase)
        {
            List<FNRequisitionDetail> oFNRequisitionDetails = new List<FNRequisitionDetail>();
            try
            {
                string subQuery="";
                if(!string.IsNullOrEmpty(oProductBase.Params))
                    subQuery = "AND FNRID IN (SELECT FNRID FROM FNRequisition WHERE  ReceiveBy>0 "+((oProductBase.Params!="")?"AND  TreatmentID IN (" + oProductBase.Params + ")":"")+  ")";

                string sSQL = "SELECT top(20)* FROM View_FNRequisitionDetail WHERE LotBalance>0 and FNRID> 0 AND ProductName LIke '%" + oProductBase.ProductName + "%'" + subQuery + " ORDER BY FNRDetailID DESC";
                oFNRequisitionDetails = FNRequisitionDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFNRequisitionDetails = new List<FNRequisitionDetail>();
            }
            var jsonResult = Json(oFNRequisitionDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetLots(PreTreatmentStock oPreTreatmentStock) //this function use in FNProduction for RawMaterial Consumption 
        {
            List<PreTreatmentStock> oPreTreatmentStocks = new List<PreTreatmentStock>();
            try
            {
                oPreTreatmentStocks = PreTreatmentStock.GetsStock(oPreTreatmentStock.ProductID, oPreTreatmentStock.TreatmentID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oPreTreatmentStock = new PreTreatmentStock();
                oPreTreatmentStock.ErrorMessage = ex.Message;
                oPreTreatmentStocks.Add(oPreTreatmentStock);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPreTreatmentStocks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //public JsonResult GetProducts_OLD(Product oProduct)
        //{
        //    List<Product> oProducts = new List<Product>();
        //    try
        //    {
        //        oProducts = new List<Product>();

        //        string sName = oProduct.ProductName;
        //        int nWUID = oProduct.WorkingUnitID;
        //        //int nSplit = oProduct.Params.Split('~').Count();
        //        string sSQL = "Select * from VIEW_Product as P Where P.ProductID<>0 ";
        //        if (!String.IsNullOrEmpty(sName))
        //        {
        //            sSQL = sSQL + " and P.ProductName Like '%" + sName + "%'";
        //        }
        //        sSQL = sSQL + " and P.ProductCategoryID in ( select ProductCategoryID from [dbo].[fn_GetProductCategoryByMTR] ( 'FNRequisitionDetail', " + (int)EnumOperationFunctionality._Add + ",  " + (int)EnumTriggerParentsType._FNRequisitionDetail + ",  " + (int)EnumInOutType.Disburse + ",0, " + nWUID + ", 0, " + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")) order by ProductCategoryID";
        //        oProducts = Product.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        oProduct = new Product();
        //        oProduct.ErrorMessage = ex.Message;
        //        oProducts.Add(oProduct);
        //    }
        //    var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}
        public JsonResult GetProducts(Lot oLot)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
               string sSql=string.Empty;

               sSql = "SELECT * FROM View_Product AS HH WHERE HH.Activity = 1 AND  ProductID IN (SELECT ProductID FROM Lot WHERE Balance >0" + ((oLot.WorkingUnitID > 0) ? " AND WorkingUnitID =" + oLot.WorkingUnitID + ")" : "") + " ";
               if (!string.IsNullOrEmpty(oLot.ProductName))
               {
                sSql = sSql + "and HH.ProductName LIKE '%" + oLot.ProductName + "%'";
               }
               sSql = sSql + " ORDER BY HH.ProductName";
               oProducts = Product.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                Product oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }

            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetProducts_All(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            EnumProductUsages eEnumProductUsages = new EnumProductUsages();

            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            List<FNRequisitionDetail> oFNRequisitionDetails = new List<FNRequisitionDetail>();
            List<FNRequisitionDetail> oFNRequisitionDetails_Consumed = new List<FNRequisitionDetail>();

            try
            {
                eEnumProductUsages = EnumProductUsages.Dyes_Chemical;
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.FNRequisition, eEnumProductUsages, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.FNRequisition, eEnumProductUsages, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (!string.IsNullOrEmpty(oProduct.ErrorMessage))
                {
                    string ProductName = oProduct.ErrorMessage.Split('~')[0];
                    int nTempFSCDID = Convert.ToInt32(oProduct.ErrorMessage.Split('~')[1]);
                    int TreatmentID = Convert.ToInt32(oProduct.ErrorMessage.Split('~')[2]);
                    int parentID = Convert.ToInt32(oProduct.ErrorMessage.Split('~')[3]);

                    //    oFNRecipes = FNRecipe.Gets("SELECT Temp.* FROM(SELECT FSCDID, ProductID,ProductCode, ProductName, SUM(ISNULL(Qty,0))as Qty FROM View_FNRecipe WHERE FNTreatment = '" + TreatmentID + "' AND  ProductName LIKE '%" + ProductName + "%'  GROUP BY FSCDID, ProductID, ProductName, ProductCode) Temp WHERE Temp.FSCDID =" + nTempFSCDID + " AND Temp.ProductID NOT IN (SELECT ProductID FROM FNRequisitionDetail Where FNRID = '" + parentID + "')", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //    oFNRequisitionDetails_Consumed = FNRequisitionDetail.Gets("SELECT ProductID, RequiredQty, DisburseQty, LotBalance FROM View_FNRequisitionDetail WHERE FNRID IN (SELECT FNRID FROM FNRequisition WHERE FNExODetailID =" + nTempFSCDID + " )", ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oProducts.ForEach(oitem =>
                       oFNRequisitionDetails.Add(new FNRequisitionDetail
                       {
                           FNRDetailID = 0,
                           FNRID = parentID,
                           ProductID     = oitem.ProductID,
                           //LotID       = oFNRequisitionDetails_Consumed.Where(x=>x.ProductID==oFNR.ProductID).Select(x=>x.LotID).FirstOrDefault() ,
                           //LotNo       = oFNRequisitionDetails_Consumed.Where(x=>x.ProductID==oFNR.ProductID).Select(x=>x.LotNo).FirstOrDefault() ,
                           //LotBalance  = oFNRequisitionDetails_Consumed.Where(x=>x.ProductID==oFNR.ProductID).Select(x=>x.LotBalance).FirstOrDefault() ,
                           //RequiredQty   = oFNRecipes.Where(x => x.ProductID == oitem.ProductID).FirstOrDefault().Qty - oFNRequisitionDetails_Consumed.Where(x => x.ProductID == oitem.ProductID).Sum(x => x.DisburseQty),
                           //DisburseQty   = oFNRequisitionDetails_Consumed.Where(x => x.ProductID == oitem.ProductID).Sum(x => x.DisburseQty),
                           ProductCode   = oitem.ProductCode,
                           Remarks       = "",
                           ProductName   = oitem.ProductName,
                           MUName        = oitem.MUnit,
                       }));
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            var jsonResult = Json(oFNRequisitionDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetProductsFSCD(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            FNRecipe oFNRecipe = new FNRecipe();
            List<Lot> oLots = new List<Lot>();
            List<WorkingUnit> oWUs = new List<WorkingUnit>();
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            List<FNRequisitionDetail> oFNRequisitionDetails = new List<FNRequisitionDetail>();
            List<FNRequisitionDetail> oFNRequisitionDetails_Consumed = new List<FNRequisitionDetail>();

            try
            {
                if(!string.IsNullOrEmpty(oProduct.ErrorMessage))
                {
                    string ProductName = oProduct.ErrorMessage.Split('~')[0];
                    int nTempFSCDID = Convert.ToInt32(oProduct.ErrorMessage.Split('~')[1]);
                    int TreatmentID = Convert.ToInt32(oProduct.ErrorMessage.Split('~')[2]);
                    int parentID = Convert.ToInt32(oProduct.ErrorMessage.Split('~')[3]);
                    //oFNRecipes = FNRecipe.Gets("SELECT ProductID, ProductName, SUM(ISNULL(Qty,0))as Qty FROM View_FNRecipe WHERE FNTreatment = '" + TreatmentID + "' AND FSCDID = '" + nTempFSCDID + "' AND ProductName LIKE '%" + ProductName + "%' GROUP BY ProductID, ProductName", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //oFNRecipes = FNRecipe.Gets("SELECT Temp.* FROM(SELECT ProductID, ProductName, SUM(ISNULL(Qty,0))as Qty FROM View_FNRecipe WHERE FNTreatment = '" + TreatmentID + "' AND FSCDID = '" + nTempFSCDID + "' AND ProductName LIKE '%" + ProductName + "%'  GROUP BY ProductID, ProductName) Temp WHERE Temp.ProductID NOT IN (SELECT ProductID FROM FNRequisitionDetail Where FNRID = '" + parentID + "')", ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oFNRecipes = FNRecipe.Gets("SELECT Temp.* FROM(SELECT FSCDID, ProductID,ProductCode, ProductName, SUM(ISNULL(Qty,0))as Qty FROM View_FNRecipe WHERE "
                                                + (TreatmentID>0 ? "FNTreatment = '" + TreatmentID + "' AND  " : "")
                                                +"ProductName LIKE '%" + ProductName + "%'  GROUP BY FSCDID, ProductID, ProductName, ProductCode) Temp WHERE Temp.FSCDID =" + nTempFSCDID + " AND Temp.ProductID NOT IN (SELECT ProductID FROM FNRequisitionDetail Where FNRID = '" + parentID + "')", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFNRequisitionDetails_Consumed = FNRequisitionDetail.Gets("SELECT ProductID, RequiredQty, DisburseQty, LotBalance FROM View_FNRequisitionDetail WHERE FNRID IN (SELECT FNRID FROM FNRequisition WHERE ISNULL(DisburseBy,0)<>0 AND FNExODetailID =" + nTempFSCDID + " )", ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oWUs = WorkingUnit.GetsPermittedStore(oProduct.BUID, EnumModuleName.FNRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

                    if (oFNRecipes.Any() && oWUs.Any())
                        oLots = Lot.Gets("SELECT * FROM View_Lot WHERE WorkingUnitID IN (" + string.Join(",", oWUs.Select(x => x.WorkingUnitID)) + ") AND ProductID IN (" + string.Join(",", oFNRecipes.Select(x => x.ProductID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                    oFNRecipes.ForEach(oFNR => 
                        oFNRequisitionDetails.Add(new FNRequisitionDetail
                        {
                            FNRDetailID = 0,
                            FNRID       = parentID,
                            ProductID   = oFNR.ProductID,
                            //LotID = oFNRequisitionDetails_Consumed.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.LotID).FirstOrDefault(),
                            //LotNo = oFNRequisitionDetails_Consumed.Where(x => x.ProductID == oFNR.ProductID).Select(x => x.LotNo).FirstOrDefault(),
                            LotBalance = oLots.Where(x => x.ProductID == oFNR.ProductID).Sum(x => x.Balance),
                            Qty = oFNR.Qty,
                            RequiredQty = oFNR.Qty - oFNRequisitionDetails_Consumed.Where(x => x.ProductID == oFNR.ProductID).Sum(x => x.DisburseQty),
                            DisburseQty = oFNRequisitionDetails_Consumed.Where(x=>x.ProductID==oFNR.ProductID).Sum(x=>x.DisburseQty),
                            ProductCode = oFNR.ProductCode,
                            Remarks     = "",
                            ProductName = oFNR.ProductName,
                            MUName      = "",
                        }));
                }
            }
            catch (Exception ex)
            {
                oFNRequisitionDetails.Add(new FNRequisitionDetail() {ErrorMessage = ex.Message });
            }
            var jsonResult = Json(oFNRequisitionDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Print
        public ActionResult Print(int FNRID)
        {
            FNRequisition oFNRequisition = new FNRequisition();
            oFNRequisition = oFNRequisition.Get(FNRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFNRequisition.FNRequisitionDetails = FNRequisitionDetail.Gets(oFNRequisition.FNRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptFNRequisition oReport = new rptFNRequisition();
            byte[] abytes = oReport.PrepareReport(oFNRequisition, oCompany);
            return File(abytes, "application/pdf");
        }

        public ActionResult PIWiseDispoStatement(int FNRID)
        {
            _oFNRequisitions = new List<FNRequisition>();
            _oFNRequisitionDetails = new List<FNRequisitionDetail>();
            List<ExportPI> oExportPIs = new List<ExportPI>();
            List<FabricSalesContractDetail> oFSCDs = new List<FabricSalesContractDetail>();
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            FabricSalesContract oFSC = new FabricSalesContract();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPIDetail WHERE OrderSheetDetailID = (SELECT FNExODetailID FROM FNRequisition WHERE FNRID=" + FNRID + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFSCDs = FabricSalesContractDetail.Gets("SELECT * FROM View_FabricSalesContractDetail WHERE FabricSalesContractID = (SELECT DD.FabricSalesContractID FROM FabricSalesContractDetail AS DD WHERE DD.FabricSalesContractDetailID = (SELECT FNExODetailID FROM FNRequisition WHERE FNRID=" + FNRID + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oExportPIs.Count > 0)
            {
                oFSCDs = FabricSalesContractDetail.Gets("SELECT * FROM View_FabricSalesContractDetail WHERE FabricSalesContractDetailID IN (SELECT OrderSheetDetailID FROM ExportPIDetail WHERE ExportPIID IN (" + string.Join(",", oExportPIs.Select(x => x.ExportPIID)) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFSCDs.Count > 0)
                {
                    oFSC = FabricSalesContract.Gets("SELECT * FROM View_FabricSalesContract WHERE FabricSalesContractID = " + oFSCDs[0].FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID).FirstOrDefault();
                    _oFNRequisitions = FNRequisition.Gets("SELECT * FROM View_FNRequisition WHERE FNExODetailID IN (" + string.Join(",", oFSCDs.Select(x => x.FabricSalesContractDetailID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFNRequisitionDetails = FNRequisitionDetail.Gets("SELECT * FROM View_FNRequisitionDetail WHERE FNRID IN (" + string.Join(",", _oFNRequisitions.Select(x => x.FNRID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                oFNRecipes = FNRecipe.Gets("SELECT * FROM View_FNRecipe WHERE FSCDID IN (SELECT OrderSheetDetailID FROM ExportPIDetail WHERE ExportPIID IN (" + string.Join(",", oExportPIs.Select(x => x.ExportPIID)) + ")) AND ISNULL(IsProcess,0) = 0", ((User)Session[SessionInfo.CurrentUser]).UserID);
                rptPIWiseDispoStatement oReport = new rptPIWiseDispoStatement();
                byte[] abytes = oReport.PrepareReport(oExportPIs, oFSC, oFSCDs, _oFNRequisitions, _oFNRequisitionDetails, oFNRecipes, oCompany);
                return File(abytes, "application/pdf");
            }
            else if (oFSCDs.Count > 0)
            {
                oFSC = FabricSalesContract.Gets("SELECT * FROM View_FabricSalesContract WHERE FabricSalesContractID = " + oFSCDs[0].FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID).FirstOrDefault();
                _oFNRequisitions = FNRequisition.Gets("SELECT * FROM View_FNRequisition WHERE FNExODetailID IN (" + string.Join(",", oFSCDs.Select(x => x.FabricSalesContractDetailID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFNRequisitionDetails = FNRequisitionDetail.Gets("SELECT * FROM View_FNRequisitionDetail WHERE FNRID IN (" + string.Join(",", _oFNRequisitions.Select(x => x.FNRID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFNRecipes = FNRecipe.Gets("SELECT * FROM View_FNRecipe WHERE FSCDID IN (" + string.Join(",", oFSCDs.Select(x => x.FabricSalesContractDetailID)) + ") AND ISNULL(IsProcess,0) = 0", ((User)Session[SessionInfo.CurrentUser]).UserID);
                rptPIWiseDispoStatement oReport = new rptPIWiseDispoStatement();
                byte[] abytes = oReport.PrepareReport(oExportPIs, oFSC, oFSCDs, _oFNRequisitions, _oFNRequisitionDetails, oFNRecipes, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No PI Found!!");
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PrintFNRequisitions(string sParams, int nBUID)
        {
            _oFNRequisitions = new List<FNRequisition>();
            string sSQL = "SELECT * FROM View_FNRequisition";
            string sTemp = "";
            try
            {
                int nCreateDateCom = Convert.ToInt32(sParams.Split('~')[0]);
                DateTime dStartDate = Convert.ToDateTime(sParams.Split('~')[1]);
                DateTime dEndDate = Convert.ToDateTime(sParams.Split('~')[2]);
                if (nCreateDateCom > 0)
                {
                    Global.TagSQL(ref sTemp);
                    if (nCreateDateCom == 1)
                    {
                        sTemp += " RequestDate = '" + dStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nCreateDateCom == 2)
                    {

                        sTemp = sTemp + " RequestDate != '" + dStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nCreateDateCom == 3)
                    {

                        sTemp = sTemp + " RequestDate > '" + dStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nCreateDateCom == 4)
                    {
                        sTemp = sTemp + " RequestDate < '" + dStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nCreateDateCom == 5)
                    {
                        sTemp = sTemp + " RequestDate>= '" + dStartDate.ToString("dd MMM yyyy") + "' AND RequestDate < '" + dEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    }
                    if (nCreateDateCom == 6)
                    {
                        sTemp = sTemp + " RequestDate < '" + dStartDate.ToString("dd MMM yyyy") + "' OR RequestDate > '" + dEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    }
                }
                sSQL += sTemp;
                _oFNRequisitions = FNRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRequisitions = new List<FNRequisition>();
            }

            if (_oFNRequisitions.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (nBUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                rptFNRequisitionList oReport = new rptFNRequisitionList();
                byte[] abytes = oReport.PrepareReport(_oFNRequisitions, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
        }

        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
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
