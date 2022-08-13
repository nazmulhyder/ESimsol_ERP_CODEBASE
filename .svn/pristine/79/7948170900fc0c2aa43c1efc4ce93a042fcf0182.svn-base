using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using ICS.Core.Utility;
using System.Linq;


namespace ESimSolFinancial.Controllers
{

    public class ProductionReportController : Controller
    {
        #region Declaration
        ProductionReport _oProductionReport = new ProductionReport();
        List<ProductionReport> _oProductionReports = new List<ProductionReport>();
        #endregion

        #region Functions
        public string MakeQuery(ProductionReport oProductionReport)
        {
            string sSQL =  " ISNULL(ProductionStepType,0) = "+(int)EnumProductionStepType.Molding+" AND BUID = "+oProductionReport.BUID;
            #region Shift
            if (oProductionReport.ShiftID != 0)
            {
                sSQL += " AND ShiftID = " + oProductionReport.ShiftID;
            }
            #endregion
            #region Transaction Date

            if (oProductionReport.TransactionStartDate == oProductionReport.TransactionEndDate)
             {
                 sSQL += " AND Convert(DATE,TransactionDate,106) = Convert(DATE,'" + oProductionReport.TransactionStartDate.ToString("dd MMM yyyy") + "',106)";
             }
             else
             {
                 sSQL += " AND  Convert(DATE,TransactionDate,106)>= Convert(DATE,'" + oProductionReport.TransactionStartDate.ToString("dd MMM yyyy") + "',106) AND Convert(DATE,TransactionDate,106) < Convert(DATE,'" + oProductionReport.TransactionEndDate.AddDays(1).ToString("dd MMM yyyy") + "',106)";
             }
                             
            #endregion
            #region sheet no
            if (!string.IsNullOrEmpty(oProductionReport.SheetNo))
            {
                sSQL += " AND ProductionSheetID IN (SELECT ProductionSheetID FROM View_ProductionSheet AS PS WHERE PS.SheetNo LIKE '%" + oProductionReport.SheetNo + "%') ";
            }
            #endregion
            #region PI no
            if (!string.IsNullOrEmpty(oProductionReport.ExportPINo))
            {
                sSQL += " AND ProductionSheetID IN (SELECT ProductionSheetID FROM View_ProductionSheet AS PS WHERE PS.ExportPINo LIKE '%" + oProductionReport.ExportPINo + "%') ";
            }
            #endregion
            #region Product
            if (!string.IsNullOrEmpty(oProductionReport.ProductName))
            {
                sSQL += " AND ProductionSheetID IN (SELECT ProductionSheetID FROM View_ProductionSheet AS PS WHERE PS.ProductID IN (" + oProductionReport.ProductName + ") )";
            }
            #endregion
            #region customer
            if (!string.IsNullOrEmpty(oProductionReport.CustomerName))
            {
                sSQL += " AND ProductionSheetID IN (SELECT ProductionSheetID FROM View_ProductionSheet AS PS WHERE PS.ContractorID IN (" + oProductionReport.CustomerName + ") )";
            }
            #endregion
            #region Machine
            if (!string.IsNullOrEmpty(oProductionReport.MachineNo))
            {
                sSQL += " AND MachineID IN (" + oProductionReport.MachineNo + ") ";
            }
            #endregion
            return sSQL;
        }
        #endregion

        #region Actions
        public ActionResult ViewProductionReports(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oProductionReports = new List<ProductionReport>();
            ViewBag.BUID = buid;
            ViewBag.HRMShifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oProductionReports);
        }

        [HttpPost]
        public JsonResult GetReports(ProductionReport oProductionReport)
        {
            _oProductionReports = new List<ProductionReport>();
            string sSQL = MakeQuery(oProductionReport);
            try
            {
                _oProductionReports = ProductionReport.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionReport = new ProductionReport();
                _oProductionReport.ErrorMessage = ex.Message;
                _oProductionReports.Add(_oProductionReport);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SetProductionReportData(ProductionReport oProductionReport)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oProductionReport);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintList(string tsv)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oProductionReport = new ProductionReport();
            _oProductionReports = new List<ProductionReport>();
            _oProductionReport = (ProductionReport)Session[SessionInfo.ParamObj];
            string sSql = MakeQuery(_oProductionReport);
            string sReportHeading = "";
            if (_oProductionReport.TransactionStartDate != _oProductionReport.TransactionEndDate)
            {
                sReportHeading = "Date:( "+_oProductionReport.TransactionStartDate.ToString("dd MMM yyyy")+" To "+_oProductionReport.TransactionEndDate.ToString("dd MMM yyyy")+" )";  
            }
            else
            {
                sReportHeading = "Date : "+_oProductionReport.TransactionStartDate.ToString("dd MMM yyyy");
            }
            //if (sIDs.Split('~')[4] != "--Select One--")
            //{
            //    sReportHeading += " (" + sIDs.Split('~')[4] + ")";
            //}
            _oProductionReport.ProductionReports = ProductionReport.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            sSql = "SELECT ProductID, ProductName, SUM(MaterialOutQty) AS MaterialOutQty, MUSymbol FROM View_RMRequisitionMaterial  WHERE ISNULL(MaterialOutQty,0)>0 AND  ProductionSheetID IN  ( " + ((_oProductionReport.ProductionReports.Count > 0) ?(string.Join(",", _oProductionReport.ProductionReports.Select(x => x.ProductionSheetID))) : 0.ToString()) + " )  GRoup BY ProductID, ProductName, MUSymbol  Order BY ProductID";
            _oProductionReport.RMRequisitionMaterials = RMRequisitionMaterial.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oProductionReport.BusinessUnit = oBusinessUnit.Get(_oProductionReport.BUID, (int)Session[SessionInfo.currentUserID]);
            _oProductionReport.Remarks = sReportHeading;//use Remarks for Head print of shift
            if (_oProductionReport.ProductionReports.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                _oProductionReport.Company = oCompany;
                rptProductionReport oReport = new rptProductionReport();
                byte[] abytes = oReport.PrepareReport(_oProductionReport);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }

        }
     
        //public ActionResult PrintProductionReportsInXL()
        //{
        //    //_productsServices = new ProductsServices();
        //    var stream = new MemoryStream();
        //    var serializer = new XmlSerializer(typeof(List<ProductionReportXL>));

        //    //We load the data
        //    List<ProductionReport> oProductionReports = ProductionReport.Gets((int)Session[SessionInfo.currentUserID]);
        //    int nCount = 0; double nTotal = 0;
        //    ProductionReportXL oProductionReportXL = new ProductionReportXL();
        //    List<ProductionReportXL> oProductionReportXLs = new List<ProductionReportXL>();
        //    foreach (ProductionReport oItem in oProductionReports)
        //    {
        //        nCount++;
        //        oProductionReportXL = new ProductionReportXL();
        //        oProductionReportXL.SLNo = nCount.ToString();
        //        //oProductionReportXL.Code = oItem.Code;
        //        //oProductionReportXL.Name = oItem.Name;
        //        //oProductionReportXL.ShortName = oItem.ShortName;
        //        //oProductionReportXL.RegistrationNo = oItem.RegistrationNo;
        //        //oProductionReportXL.Address = oItem.Address;
        //        //oProductionReportXL.Phone = oItem.Phone;
        //        //oProductionReportXL.Email = oItem.Email;
        //        //oProductionReportXL.WebAddress = oItem.WebAddress;
        //        //oProductionReportXL.Note = oItem.Note;
        //        oProductionReportXLs.Add(oProductionReportXL);
        //        nTotal = nTotal + nCount;
        //    }

        //    //We turn it into an XML and save it in the memory
        //    serializer.Serialize(stream, oProductionReportXLs);
        //    stream.Position = 0;

        //    //We return the XML from the memory as a .xls file
        //    return File(stream, "application/vnd.ms-excel", "Orders.xls");
        //}

        public Image GetDBCompanyLogo(int id)
        {
            #region From DB
            Company oCompany = new Company();
            oCompany = oCompany.Get(id, (int)Session[SessionInfo.currentUserID]);
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
            #endregion
        }

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

        #endregion

        
    }
}