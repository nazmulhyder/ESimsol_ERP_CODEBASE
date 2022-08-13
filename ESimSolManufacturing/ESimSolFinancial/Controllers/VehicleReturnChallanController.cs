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
    public class VehicleReturnChallanController : Controller
    {
        #region Declaration
        VehicleReturnChallan _oVehicleReturnChallan = new VehicleReturnChallan();
        List<VehicleReturnChallan> _oVehicleReturnChallans = new List<VehicleReturnChallan>();
        string _sErrorMessage = "";
        #endregion

        #region VehicleReturnChallan
        public ActionResult ViewVehicleReturnChallans(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VehicleReturnChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
           
            _oVehicleReturnChallans = new List<VehicleReturnChallan>();
            _oVehicleReturnChallans = VehicleReturnChallan.Gets("Select * from View_VehicleReturnChallan where  isnull(ApproveBy,0)=0", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.BUID = buid;
            return View(_oVehicleReturnChallans);
        }

        public ActionResult ViewVehicleReturnChallan(int nId, int buid)
        {
            _oVehicleReturnChallan = new VehicleReturnChallan();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();

            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.VehicleReturnChallan, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);

            DyeingOrder oDyeingOrder = new DyeingOrder();
            if (nId > 0)
            {
                _oVehicleReturnChallan = VehicleReturnChallan.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.WorkingUnits = oWorkingUnits;
            return View(_oVehicleReturnChallan);
        }
      
        [HttpPost]
        public JsonResult Save(VehicleReturnChallan oVehicleReturnChallan)
        {
            try
            {
               _oVehicleReturnChallan = oVehicleReturnChallan.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oVehicleReturnChallan = new VehicleReturnChallan();
                _oVehicleReturnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVehicleReturnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
        [HttpPost]
        public JsonResult Approve(VehicleReturnChallan oVehicleReturnChallan)
        {
            string sErrorMease = "";
            _oVehicleReturnChallan = oVehicleReturnChallan;
            try
            {
                _oVehicleReturnChallan = oVehicleReturnChallan.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVehicleReturnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       

        [HttpPost]
        public JsonResult Delete(VehicleReturnChallan oVehicleReturnChallan)
        {
            try
            {
                if (oVehicleReturnChallan.VehicleReturnChallanID <= 0) { throw new Exception("Please select an valid item."); }
                oVehicleReturnChallan.ErrorMessage = oVehicleReturnChallan.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oVehicleReturnChallan = new VehicleReturnChallan();
                oVehicleReturnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oVehicleReturnChallan.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsInvoice(SaleInvoice oSaleInvoice)
        {
            List<SaleInvoice> oSaleInvoices = new List<SaleInvoice>();
            try
            {
                string sSQL = MakeSQL_Invoice(oSaleInvoice);
                oSaleInvoices = SaleInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSaleInvoices = new List<SaleInvoice>();
                oSaleInvoices.Add(new SaleInvoice() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSaleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string MakeSQL_Invoice(SaleInvoice oSaleInvoice)
        {
            string sReturn1 = "SELECT * FROM View_SaleInvoice AS EB";
            string sReturn = "";

            #region InvoiceNo
            if (!string.IsNullOrEmpty(oSaleInvoice.InvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.InvoiceNo LIKE '%" + oSaleInvoice.InvoiceNo + "%'";
            }
            #endregion


            #region CustomerName
            if (!string.IsNullOrEmpty(oSaleInvoice.CustomerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.CustomerName LIKE '%" + oSaleInvoice.CustomerName + "%'";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        [HttpPost]
        public JsonResult GetLots(VehicleReturnChallan oVehicleReturnChallan)
        {
            List<Lot> oLots=new List<Lot>();
            try
            {
                oLots = Lot.GetsZeroBalance(oVehicleReturnChallan.ProductID.ToString(), oVehicleReturnChallan.WorkingUnitID.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult AdvSearch(VehicleReturnChallan oVehicleReturnChallan)
        {
            List<VehicleReturnChallan> oVehicleReturnChallans = new List<VehicleReturnChallan>();
            VehicleReturnChallan _oVehicleReturnChallan = new VehicleReturnChallan();
            string sSQL = MakeSQL(oVehicleReturnChallan);
            if (sSQL == "Error")
            {
                _oVehicleReturnChallan = new VehicleReturnChallan();
                _oVehicleReturnChallan.ErrorMessage = "Please select a searching critaria.";
                oVehicleReturnChallans = new List<VehicleReturnChallan>();
            }
            else
            {
                oVehicleReturnChallans = new List<VehicleReturnChallan>();
                oVehicleReturnChallans = VehicleReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oVehicleReturnChallans.Count == 0)
                {
                    oVehicleReturnChallans = new List<VehicleReturnChallan>();
                }
            }
            var jsonResult = Json(oVehicleReturnChallans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(VehicleReturnChallan oVehicleReturnChallan)
        {
            string sParams = oVehicleReturnChallan.Params;

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

            string sReturn1 = "SELECT * FROM View_VehicleReturnChallan AS EB";
            string sReturn = "";

            #region InvNo
            if (!string.IsNullOrEmpty(sInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ReturnChallanNoFull LIKE '%" + sInvoiceNo + "%'";
            }
            #endregion

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " EB.ReturnChallanDate", nDateCriteria_InvDate, dStart_InvDate, dEnd_InvDate);
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

        public ActionResult PrintVehicleReturnChallan(int id)
        {
            SaleInvoice _oSaleInvoice = new SaleInvoice();
            _oSaleInvoice = _oSaleInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<VehicleReturnChallan> oVCs = new List<VehicleReturnChallan>();
            oVCs = VehicleReturnChallan.Gets("SELECT TOP(1)* FROM View_VehicleReturnChallan WHERE SaleInvoiceID=" + id, (int)Session[SessionInfo.currentUserID]);

            if (oVCs.Any())
            {
                rptVehicleReturnChallan orptSaleInvoices = new rptVehicleReturnChallan();
                byte[] abytes = orptSaleInvoices.PrepareReport(_oSaleInvoice, oVCs[0], oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage orptErrorMessage = new rptErrorMessage();
                byte[] abytes = orptErrorMessage.PrepareReport("NO RETURN CHALLAN FOUND!");
                return File(abytes, "application/pdf");
            }
        }

        #endregion
    }
}