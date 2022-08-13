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
	public class RecycleProcessController : Controller
	{
		#region Declaration

		RecycleProcess _oRecycleProcess = new RecycleProcess();
		List<RecycleProcess> _oRecycleProcesss = new  List<RecycleProcess>();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewRecycleProcesList(int buid, int productNature, int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RecycleProcess).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oRecycleProcesss = new List<RecycleProcess>();
            string sSQL = "SELECT * FROM View_RecycleProcess  AS HH WHERE HH.BUID = " + buid.ToString() + " AND ISNULL(HH.ApprovedBy,0) = 0 ORDER BY RecycleProcessID ASC";
            _oRecycleProcesss = RecycleProcess.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RecycleProcess, EnumStoreType.ReceiveStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            ViewBag.ProductNature = productNature;
			return View(_oRecycleProcesss);
		}
		public ActionResult ViewRecycleProces(int id, int buid)
		{
			_oRecycleProcess = new RecycleProcess();
			if (id > 0)
			{
				_oRecycleProcess = _oRecycleProcess.Get(id, (int)Session[SessionInfo.currentUserID]);
                List<RecycleProcessDetail> oRecycleProcessDetails = new List<RecycleProcessDetail>();
                oRecycleProcessDetails = RecycleProcessDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                
                _oRecycleProcess.RecycleProcessDetails = oRecycleProcessDetails.Where(x=>x.ProcessProductType==EnumProcessProductType.RawMaterial).ToList();
                _oRecycleProcess.DamageDetails = oRecycleProcessDetails.Where(x => x.ProcessProductType == EnumProcessProductType.DamageProduct).ToList();
			}
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RecycleProcess, EnumStoreType.ReceiveStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.WorkingUnitsDamage = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RecycleProcess, EnumStoreType.IssueStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.RecycleType = EnumObject.jGets(typeof(EnumRecycleProcessType));
           
			return View(_oRecycleProcess);
		}

     

		[HttpPost]
		public JsonResult Save(RecycleProcess oRecycleProcess)
		{
			_oRecycleProcess = new RecycleProcess();
			try
			{
                if ((int)oRecycleProcess.RecycleProcessType == 2)
                {
                    for (var i = 0; i < oRecycleProcess.DamageDetails.Count; i++)
                    {
                        oRecycleProcess.RecycleProcessDetails.Add(oRecycleProcess.DamageDetails[i]);
                    }
                }
                _oRecycleProcess = oRecycleProcess.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oRecycleProcess = new RecycleProcess();
				_oRecycleProcess.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oRecycleProcess);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult Approve(RecycleProcess oRecycleProcess)
        {
            _oRecycleProcess = new RecycleProcess();
            try
            {
                _oRecycleProcess = oRecycleProcess;
                _oRecycleProcess = _oRecycleProcess.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRecycleProcess = new RecycleProcess();
                _oRecycleProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRecycleProcess);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
        public JsonResult Delete(RecycleProcess oRecycleProcess)
		{
			string sFeedBackMessage = "";
			try
			{
                sFeedBackMessage = oRecycleProcess.Delete(oRecycleProcess.RecycleProcessID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();

            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.RecycleProcess, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.RecycleProcess, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                if (oProducts.Count <= 0)
                {
                    oProducts = Product.GetsByBU(oProduct.BUID, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

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
            List<RecycleProcess> oRecycleProcesss = new List<RecycleProcess>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oRecycleProcesss = RecycleProcess.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRecycleProcess = new RecycleProcess();
                _oRecycleProcess.ErrorMessage = ex.Message;
                oRecycleProcesss.Add(_oRecycleProcess);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRecycleProcesss);
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
            int nWorkingUnitID = Convert.ToInt32(sTemp.Split('~')[5]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[6]);
            int nProductNature = (int)EnumProductNature.Hanger;
            if (sTemp.Split('~')[7] != "null")
            {
                nProductNature = Convert.ToInt32(sTemp.Split('~')[7]);
            }
            #endregion

            string sReturn1 = "SELECT * FROM View_RecycleProcess";
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
                sReturn = sReturn + " RecycleProcessID IN ( SELECt RecycleProcessID FROM RecycleProcessDetail WHERE ProductID IN (" + sProductIDs + "))";
            }
            #endregion
            
            #region Process Date Wise
            if (nProcessCreateDateCom > 0)
            {
                if (nProcessCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,ProcessDate,106)  = Convert(Date,'" + dProcessStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nProcessCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,ProcessDate,106)  != Convert(Date,'" + dProcessStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nProcessCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,ProcessDate,106)  > Convert(Date,'" + dProcessStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nProcessCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,ProcessDate,106)  < Convert(Date,'" + dProcessStartDate.ToString("dd MMM yyyy") + "',106)";
                }
                if (nProcessCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,ProcessDate,106) >= Convert(Date,'" + dProcessStartDate.ToString("dd MMM yyyy") + "',106)  AND Convert(Date,ProcessDate,106)  < Convert(Date,'" + dProcessEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
                if (nProcessCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Convert(Date,ProcessDate,106) < Convert(Date,'" + dProcessStartDate.ToString("dd MMM yyyy") + "',106) OR Convert(Date,ProcessDate,106)  > Convert(Date,'" + dProcessEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
                }
            }
            #endregion
            #region working unit
            if (nWorkingUnitID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " WorkingUnitID = " + nWorkingUnitID;
            }
            #endregion
            #region BU
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " BUID = " + nBUID;
            }
            #endregion
            #region Product Nature
            if (nProductNature > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " ProductNature = " + nProductNature;
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion
        #endregion

        #region PrintList
        public ActionResult PrintList(string sIDs)
        {
            _oRecycleProcess = new RecycleProcess();
            string sSQL = "SELECT * FROM View_RecycleProcess WHERE RecycleProcessID IN (" + sIDs + ") ORDER BY RecycleProcessID  ASC";
            _oRecycleProcess.RecycleProcessList = RecycleProcess.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptRecycleProcessList oReport = new rptRecycleProcessList();
            byte[] abytes = oReport.PrepareReport(_oRecycleProcess, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintRecycleProcessPreview(int id)
        {
            _oRecycleProcess = new RecycleProcess();
            _oRecycleProcess = _oRecycleProcess.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oRecycleProcess.RecycleProcessDetails = RecycleProcessDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptRecycleProcess oReport = new rptRecycleProcess();
            byte[] abytes = oReport.PrepareReport(_oRecycleProcess, oCompany);
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
