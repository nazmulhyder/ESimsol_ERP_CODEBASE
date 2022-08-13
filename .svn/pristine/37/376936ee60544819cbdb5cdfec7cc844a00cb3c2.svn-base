using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class VehicleChallanController : Controller
    {
        #region Declaration
        VehicleChallan _oVehicleChallan = new VehicleChallan();
        List<VehicleChallan> _oVehicleChallans = new List<VehicleChallan>();
        string _sErrorMessage = "";
        #endregion

        #region VehicleChallan
        public ActionResult ViewVehicleChallans(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VehicleChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
           
            _oVehicleChallans = new List<VehicleChallan>();
            _oVehicleChallans = VehicleChallan.Gets("Select * from View_VehicleChallan where  isnull(ApproveBy,0)=0", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.BUID = buid;
            return View(_oVehicleChallans);
        }

        public ActionResult ViewVehicleChallan(int nId, int buid)
        {
            _oVehicleChallan = new VehicleChallan();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();

            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.VehicleChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            DyeingOrder oDyeingOrder = new DyeingOrder();
            if (nId > 0)
            {
                _oVehicleChallan = VehicleChallan.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.WorkingUnits = oWorkingUnits;
            return View(_oVehicleChallan);
        }
      
        [HttpPost]
        public JsonResult Save(VehicleChallan oVehicleChallan)
        {
            try
            {
               _oVehicleChallan = oVehicleChallan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oVehicleChallan = new VehicleChallan();
                _oVehicleChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVehicleChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
        [HttpPost]
        public JsonResult Approve(VehicleChallan oVehicleChallan)
        {
            string sErrorMease = "";
            _oVehicleChallan = oVehicleChallan;
            try
            {
                _oVehicleChallan = oVehicleChallan.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVehicleChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       

        [HttpPost]
        public JsonResult Delete(VehicleChallan oVehicleChallan)
        {
            try
            {
                if (oVehicleChallan.VehicleChallanID <= 0) { throw new Exception("Please select an valid item."); }
                oVehicleChallan.ErrorMessage = oVehicleChallan.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oVehicleChallan = new VehicleChallan();
                oVehicleChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVehicleChallan.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLots(VehicleChallan oVehicleChallan)
        {
            List<Lot> oLots=new List<Lot>();
            try
            {
                oLots = Lot.GetsBy(oVehicleChallan.ProductID.ToString(), oVehicleChallan.WorkingUnitID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLots.Add(new Lot() { ErrorMessage=ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ADV SEARCH

        [HttpPost]
        public JsonResult AdvSearch(VehicleChallan oVehicleChallan)
        {
            List<VehicleChallan> oVehicleChallans = new List<VehicleChallan>();
            VehicleChallan _oVehicleChallan = new VehicleChallan();
            string sSQL = MakeSQL(oVehicleChallan);
            if (sSQL == "Error")
            {
                _oVehicleChallan = new VehicleChallan();
                _oVehicleChallan.ErrorMessage = "Please select a searching critaria.";
                oVehicleChallans = new List<VehicleChallan>();
            }
            else
            {
                oVehicleChallans = new List<VehicleChallan>();
                oVehicleChallans = VehicleChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oVehicleChallans.Count == 0)
                {
                    oVehicleChallans = new List<VehicleChallan>();
                }
            }
            var jsonResult = Json(oVehicleChallans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(VehicleChallan oVehicleChallan)
        {
            string sParams = oVehicleChallan.Params;

            int nDateCriteria_InvDate = 0;

            string sInvoiceNo = "",
                   sRegIDs = "",
                   sBuyerIDs = "";

            DateTime dStart_InvDate = DateTime.Today,
                     dEnd_InvDate = DateTime.Today;

            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                sInvoiceNo = sParams.Split('~')[nCount++];
                nDateCriteria_InvDate = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_InvDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_InvDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                //sRegIDs = sParams.Split('~')[nCount++];
                sBuyerIDs = sParams.Split('~')[nCount++];
            }

            string sReturn1 = "SELECT * FROM View_VehicleChallan AS EB";
            string sReturn = "";

            #region InvNo
            if (!string.IsNullOrEmpty(sInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ChallanNoFull LIKE '%" + sInvoiceNo + "%'";
            }
            #endregion

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " EB.ChallanDate", nDateCriteria_InvDate, dStart_InvDate, dEnd_InvDate);
            #endregion

            #region Model IDs
            if (!string.IsNullOrEmpty(sRegIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.VehicleRegistrationID IN (" + sRegIDs + ") ";
            }
            #endregion

            #region Buyer IDs
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ContractorID IN (" + sBuyerIDs + ") ";
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region GetCompanyLogo
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

        #region Print
        public ActionResult PrintVehicleChallan(int nId, double nts, bool bPrintFormat, int nTitleType)
        {
            _oVehicleChallan = new VehicleChallan();
            ExportSCDO oExportSCDO = new ExportSCDO();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            try
            {
                if (nId > 0)
                {
                    _oVehicleChallan = VehicleChallan.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //rptVehicleChallan oReport = new rptVehicleChallan();
            //byte[] abytes = oReport.PrepareReport(_oVehicleChallan, oCompany, oBusinessUnit, nTitleType, oDUOrderSetup);
            //return File(abytes, "application/pdf");
            return null;
        }
        #endregion
    }
}