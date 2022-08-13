using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

namespace ESimSolFinancial.Controllers
{
    public class SubcontractController : Controller
    {
        #region Declaration
        Subcontract _oSubcontract = new Subcontract();
        List<Subcontract> _oSubcontracts = new List<Subcontract>();
        #endregion

        #region Actions
        public ActionResult ViewSubcontracts(int sct, int buid, int menuid)
        {
            //sct = 0 means Subcontract Issue & sct = 1 means Subcontract Received
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Subcontract).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            #region Gets Data
            _oSubcontracts = new List<Subcontract>();
            if (sct == 0)
            {
                string sSQL = "SELECT * FROM View_Subcontract AS HH WHERE HH.ContractStatus=" + ((int)EnumSubContractStatus.Initialized).ToString() + " AND HH.IssueBUID =" + buid.ToString() + "  ORDER BY SubcontractID ASC";
                _oSubcontracts = Subcontract.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                string sSQL = "SELECT * FROM View_Subcontract AS HH WHERE HH.ContractStatus=" + ((int)EnumSubContractStatus.Approved).ToString() + " AND HH.ContractBUID =" + buid.ToString() + "  ORDER BY SubcontractID ASC";
                _oSubcontracts = Subcontract.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            #endregion

            #region Gets Business Unit
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string sTempSQL = "SELECT * FROM View_BusinessUnit AS HH WHERE HH.BusinessUnitType IN (2,3) ORDER BY BusinessUnitID ASC";
            oBusinessUnits = BusinessUnit.Gets(sTempSQL, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.SCT = sct;
            ViewBag.BusinessUnits = oBusinessUnits;
            return View(_oSubcontracts);
        }
        public ActionResult ViewSubcontract(int id, int ptuid)
        {
            _oSubcontract = new Subcontract();
            if (id > 0)
            {
                _oSubcontract = _oSubcontract.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oSubcontract = new Subcontract(); 
                PTUUnit2 oPTUUnit2 = new PTUUnit2();                
                oPTUUnit2 = oPTUUnit2.Get(ptuid, (int)Session[SessionInfo.currentUserID]);

                #region PTU to Sub Contract
                _oSubcontract.SubcontractID = 0;
                _oSubcontract.SubcontractNo = "";
                _oSubcontract.ContractStatus = EnumSubContractStatus.Initialized;
                _oSubcontract.ContractStatusInt = 0;
                _oSubcontract.IssueBUID = oPTUUnit2.BUID;
                _oSubcontract.ContractBUID = 0;
                _oSubcontract.PTU2ID = oPTUUnit2.PTUUnit2ID;
                _oSubcontract.IssueDate = DateTime.Today;
                _oSubcontract.ExportSCID = oPTUUnit2.ExportSCID;
                _oSubcontract.ExportSCDetailID = oPTUUnit2.ExportSCDetailID;
                _oSubcontract.ProductID = oPTUUnit2.ProductID;
                _oSubcontract.ColorID = oPTUUnit2.ColorID;
                _oSubcontract.MoldRefID = oPTUUnit2.ModelReferenceID;
                _oSubcontract.UintID = oPTUUnit2.UnitID;
                _oSubcontract.Qty = oPTUUnit2.ProductionCapacity;
                _oSubcontract.RateUnit = oPTUUnit2.RateUnit;
                _oSubcontract.UnitPrice = oPTUUnit2.UnitPrice;
                _oSubcontract.CurrencyID = oPTUUnit2.CurrencyID;
                _oSubcontract.CRate = oPTUUnit2.ConversionRate;
                _oSubcontract.ApprovedBy = 0;
                _oSubcontract.IssueBUName = oPTUUnit2.BUName;
                _oSubcontract.IssueBUShortName = "";
                _oSubcontract.ContarctBUName = "";
                _oSubcontract.ContarctBUShortName = "";
                _oSubcontract.PINo = oPTUUnit2.ExportPINo;                
                _oSubcontract.ExportSCDate = oPTUUnit2.PIDate;
                _oSubcontract.ContractorName = oPTUUnit2.ContractorName;
                _oSubcontract.BuyerName = oPTUUnit2.BuyerName;
                _oSubcontract.ProductCode = oPTUUnit2.ProductCode;
                _oSubcontract.ProductName = oPTUUnit2.ProductName;
                _oSubcontract.ColorName = oPTUUnit2.ColorName;
                _oSubcontract.UnitName = oPTUUnit2.UnitName;
                _oSubcontract.UnitSymbol = oPTUUnit2.UnitSymbol;
                _oSubcontract.MoldName = oPTUUnit2.ModelReferenceName;
                _oSubcontract.ProductionCapacity = oPTUUnit2.ProductionCapacity;
                _oSubcontract.ApprovedByName = "";
                _oSubcontract.Remarks = "";
                #endregion
            }

            #region Gets Business Unit & Company
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string sSQL = "SELECT * FROM View_BusinessUnit AS HH WHERE HH.BusinessUnitID !=" + _oSubcontract.IssueBUID.ToString() + " AND HH.BusinessUnitType IN (2,3) ORDER BY BusinessUnitID ASC";
            oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany;
            return View(_oSubcontract);
        }
        #endregion

        #region Post functions
        [HttpPost]
        public JsonResult Save(Subcontract oSubcontract)
        {
            _oSubcontract = new Subcontract();
            try
            {
                _oSubcontract = oSubcontract;
                _oSubcontract = _oSubcontract.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSubcontract = new Subcontract();
                _oSubcontract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSubcontract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Subcontract oSubcontract)
        {
            string sFeedBackMessage = "";
            try
            {                
                sFeedBackMessage = oSubcontract.Delete(oSubcontract.SubcontractID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approved(Subcontract oSubcontract)
        {            
            try
            {
                oSubcontract = oSubcontract.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oSubcontract = new Subcontract();
                oSubcontract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSubcontract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Received(Subcontract oSubcontract)
        {
            try
            {
                oSubcontract = oSubcontract.Received((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oSubcontract = new Subcontract();
                oSubcontract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSubcontract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendToProduction(Subcontract oSubcontract)
        {
            try
            {
                oSubcontract = oSubcontract.SendToProduction((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oSubcontract = new Subcontract();
                oSubcontract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSubcontract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult SubcontractPrintList(string sIDs, double ts)
        {
            _oSubcontract = new Subcontract();
            _oSubcontracts = new List<Subcontract>();
            string sSql = "SELECT * FROM View_Subcontract WHERE SubcontractID IN (" + sIDs + ")";
            _oSubcontracts = Subcontract.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            int nUserID = Convert.ToInt32(Session[SessionInfo.currentUserID]);
            _oSubcontract.SubcontractList = _oSubcontracts;
            if (_oSubcontract.SubcontractList.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(_oSubcontracts[0].IssueBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany.Name = oBusinessUnit.Name;
                oCompany.Address = oBusinessUnit.Address;
                oCompany.Phone = oBusinessUnit.Phone;
                oCompany.Email = oBusinessUnit.Email;
                oCompany.WebAddress = oBusinessUnit.WebAddress;
                _oSubcontract.Company = oCompany;

                rptSubcontractList oReport = new rptSubcontractList();
                byte[] abytes = oReport.PrepareReport(_oSubcontract);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }

        }

        public ActionResult SubcontractPrintPreview(int id)
        {
            _oSubcontract = new Subcontract();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oSubcontract = _oSubcontract.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oSubcontract.BusinessUnit = oBusinessUnit.Get(_oSubcontract.IssueBUID, (int)Session[SessionInfo.currentUserID]);                
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oSubcontract.Company = oCompany;

            byte[] abytes;
            rptSubcontractPreview oReport = new rptSubcontractPreview();
            abytes = oReport.PrepareReport(_oSubcontract);
            return File(abytes, "application/pdf");
        }
        #endregion Print

        #region Searching
        private string GetSQL(string sTemp)
        {
            int nSubcontractDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dSubcontractStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dSubcontractEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);            
            string sSubcontractNo = sTemp.Split('~')[3];
            string sBuyerIDs = sTemp.Split('~')[4];
            int IsCheckedApproved = Convert.ToInt32(sTemp.Split('~')[5]);
            int IsCheckedNotApproved = Convert.ToInt32(sTemp.Split('~')[6]);
            int nBusinessUnitID = Convert.ToInt32(sTemp.Split('~')[7]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[8]);
            int nSCT = Convert.ToInt32(sTemp.Split('~')[9]);

            string sReturn1 = "SELECT * FROM View_Subcontract";
            string sReturn = "";

            #region SubcontractNo
            if (sSubcontractNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SubcontractNo LIKE '%" + sSubcontractNo + "%'";
            }
            #endregion

            #region Buyer Name
            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region BusinessUnit
            if (nBusinessUnitID > 0)
            {
                if (nSCT == 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractBUID =" + nBusinessUnitID.ToString();

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueBUID =" + nBUID.ToString();
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueBUID =" + nBusinessUnitID.ToString();

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractBUID =" + nBUID.ToString();
                }
            }
            else
            {
                if (nSCT == 0)
                {  
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueBUID =" + nBUID.ToString();
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractBUID =" + nBUID.ToString();
                }
            }
            #endregion

            #region SubcontractDate
            if (nSubcontractDate > 0)
            {
                if (nSubcontractDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate = '" + dSubcontractStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSubcontractDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate != '" + dSubcontractStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSubcontractDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate > '" + dSubcontractStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSubcontractDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate < '" + dSubcontractStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nSubcontractDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate>= '" + dSubcontractStartDate.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dSubcontractEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nSubcontractDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate< '" + dSubcontractStartDate.ToString("dd MMM yyyy") + "' OR IssueDate > '" + dSubcontractEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion

            #region IsApproved
            if (IsCheckedApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApprovedBy,0) != 0";
            }
            #endregion

            #region IsNotApproved
            if (IsCheckedNotApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  ISNULL(ApprovedBy,0) = 0";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY SubcontractID ASC";
            return sReturn;
        }
        [HttpGet]
        public JsonResult SubcontractsAdvSearch(string Temp)
        {
            List<Subcontract> oSubcontracts = new List<Subcontract>();
            try
            {
                string sSQL = GetSQL(Temp);
                oSubcontracts = Subcontract.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oSubcontracts = new List<Subcontract>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSubcontracts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByContractNo(Subcontract oSubcontract)
        {
            List<Subcontract> oSubcontracts = new List<Subcontract>();
            try
            {
                if (oSubcontract.SCT == 0)
                {
                    string sSQL = "SELECT * FROM View_Subcontract AS HH WHERE HH.IssueBUID=" + oSubcontract.IssueBUID.ToString() + " AND (HH.SubcontractNo+HH.PINo+HH.ContractorName) LIKE '%" + oSubcontract.SubcontractNo + "%' ORDER BY SubcontractID ASC";
                    oSubcontracts = Subcontract.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    string sSQL = "SELECT * FROM View_Subcontract AS HH WHERE HH.ContractBUID=" + oSubcontract.IssueBUID.ToString() + " AND (HH.SubcontractNo+HH.PINo+HH.ContractorName) LIKE '%" + oSubcontract.SubcontractNo + "%' ORDER BY SubcontractID ASC";
                    oSubcontracts = Subcontract.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oSubcontracts = new List<Subcontract>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSubcontracts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public Image GetCompanyLogo(Company oCompany)
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
    }
}