using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;

using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using CrystalDecisions.CrystalReports.Engine;
namespace ESimSolFinancial.Controllers
{
    public class ContractorController : Controller
    {
        #region Declaration
        Contractor _oContractor = new Contractor();
        List<Contractor> _oContractors = new List<Contractor>();
        ContactPersonnel _oContactPersonnel = new ContactPersonnel();
        List<ContactPersonnel> _oContactPersonnels = new List<ContactPersonnel>();
        List<ContractorAddress> _oContractorAddresss = new List<ContractorAddress>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        List<BuyerConcern> _oBuyerConcerns = new List<BuyerConcern>();
        BuyerConcern _oBuyerConcern = new BuyerConcern();        

        int _nRowIndex = 2;
        ExcelRange _cell;
        ExcelFill _fill;
        OfficeOpenXml.Style.Border _border;
        #endregion

        #region Contractor
        public ActionResult ViewContractors(int buid, int menuid)
        {            
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Contractor).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            _oContractors = new List<Contractor>();
            ViewBag.ContractorTypes = EnumObject.jGets(typeof(EnumContractorType));
            ViewBag.AddressTypes = EnumObject.jGets(typeof(EnumAddressType));
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.BUID = buid;
            return View(_oContractors);
        }

        public ActionResult ViewCustomerPersonalInfo(int id)
        {
            CustomerPersonalInfo oCustomerPersonalInfo = new CustomerPersonalInfo();
            try { 
            oCustomerPersonalInfo = oCustomerPersonalInfo.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception e)
            {

            }
           
            ViewBag.MarriedStatus = EnumObject.jGets(typeof(EnumMarriedStatus));
            ViewBag.CustomerID = id;
            return View(oCustomerPersonalInfo);
        }

        public ActionResult SetSessionSearchCriteria(Contractor oContractor)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oContractor);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public void ExcelCustomerPersonalInfo(int nBuid)
        {
            
            string sErrorMessage="";
            List<CustomerPersonalInfo> oCustomerPersonalInfos = new List<CustomerPersonalInfo>();
            
            try
            {
                _oContractor = (Contractor)Session[SessionInfo.ParamObj];
                string sql = "select * from View_CustomerPersonalInfo as HH where HH.CustomerID IN (" + _oContractor.Name+")";
                oCustomerPersonalInfos = CustomerPersonalInfo.Gets(sql, (int)Session[SessionInfo.currentUserID]);
                if(oCustomerPersonalInfos.Count<=0)
                {
                    sErrorMessage="No Data Found";
                }

            }
            catch(Exception e){
                oCustomerPersonalInfos=new List<CustomerPersonalInfo>();
                sErrorMessage=e.Message;
            }
            if (sErrorMessage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBuid, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                #region Excel
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                { 
                     excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Customer Personal Info");
                    sheet.Name = "Customer Personal Info";
                    sheet.Column(2).Width = 8; //SL
                    sheet.Column(3).Width = 20; //Customer Name
                    sheet.Column(4).Width = 20; //Employeer Name
                    sheet.Column(5).Width = 20; //Designation

                    sheet.Column(6).Width = 16; //Address
                    sheet.Column(7).Width = 18; //ContactNumber
                    sheet.Column(8).Width = 20; //Email Address
                    sheet.Column(9).Width = 20; //DateOfBirth
                    sheet.Column(10).Width = 20; //MarriedStatus
                    sheet.Column(11).Width = 18; //SpouseName
                    sheet.Column(12).Width = 20; //SpouseDateOfBirth
                    sheet.Column(13).Width = 20; //Anniversarydate
                    sheet.Column(14).Width = 20; //remarks
                    sheet.Column(15).Width = 15;//Contractor name
                    #region Report Header
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 15]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 15]; cell.Merge = true;
                    cell.Value = "Customer Personal Info"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 15]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion
                    #region Column Header
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Contractor Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Customer Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Employeer Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Designation"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Address"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "Contact Number"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "Email Address"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Date Of Birth"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "Married Status"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = "Spouse Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = "Spouse Date Of Birth"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = "Anniversary Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = "Remarks"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                   
                    rowIndex++;
                    #endregion
                    #region Body
                    int sl=1;
                    foreach (CustomerPersonalInfo oItem in oCustomerPersonalInfos)
                    {
                        cell = sheet.Cells[rowIndex, 2]; cell.Merge = true; cell.Value = sl; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        sl++;

                        cell = sheet.Cells[rowIndex, 3]; cell.Merge = true; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Merge = true; cell.Value = oItem.CustomerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.EmployeerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 6]; cell.Merge = true; cell.Value = oItem.Designation; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.Address; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.ContactNumber; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.EmailAddress; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.DateOfBirthSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.MarriedStatusSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 12]; cell.Merge = true; cell.Value = oItem.SpouseName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 13]; cell.Merge = true; cell.Value = oItem.SpouseDateOfBirthSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 14]; cell.Merge = true; cell.Value = oItem.AnniversaryDateSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 15]; cell.Merge = true; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                     
                        rowIndex++;
                    }
                    #endregion
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Customer_Personal_Info.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion

            }
        }
        [HttpPost]
        public JsonResult SaveCustomerPersonalInfo(CustomerPersonalInfo oCustomerPersonalInfo)
        {
            CustomerPersonalInfo _oCustomerPersonalInfo = new CustomerPersonalInfo();
            try
            {
                _oCustomerPersonalInfo = oCustomerPersonalInfo;
                _oCustomerPersonalInfo = _oCustomerPersonalInfo.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oCustomerPersonalInfo = new CustomerPersonalInfo();
                _oCustomerPersonalInfo.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCustomerPersonalInfo);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewContractor(int id, double ts)
        {
            _oContractor = new Contractor();
            List<ContractorType> oContractorTypes = new List<ContractorType>();

            if (id > 0)
            {
                _oContractor = _oContractor.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oContractor.BUWisePartys = BUWiseParty.Gets(id,(int)Session[SessionInfo.currentUserID]);
            }else
            {
                _oContractor.BUWisePartys = new List<BUWiseParty>();
            }
            List<EnumObject> oContractorTypeObjs= EnumObject.jGets(typeof(EnumContractorType));

            foreach (EnumObject oItem in oContractorTypeObjs)
            {
                ContractorType oContractorType = new ContractorType();
                oContractorType.ContractorID = id;
                oContractorType.ContractorTypeID = oItem.id;
                oContractorType.TypeName = oItem.Value;
                if (oContractorType.ContractorTypeID > 0)
                {
                    oContractorTypes.Add(oContractorType);
                }
            }
            ViewBag.ContractorTypeObj = oContractorTypes;
            ViewBag.ContractorTypes = ContractorType.Gets(id, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oContractor);
        }
        [HttpPost]
        public JsonResult Save(Contractor oContractor)
        {
            _oContractor = new Contractor();
            try
            {
                _oContractor = oContractor;
                _oContractor.ContractorType = (EnumContractorType)oContractor.ContractorTypeInInt;

                _oContractor = _oContractor.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oContractor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Contractor oContractor)
        {
            string sMessage = "";
            try
            {
                if (oContractor.ContractorID <= 0) { throw new Exception("Please select a valid contractor."); }
                sMessage = oContractor.Delete(oContractor.ContractorID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CommitActivity(int id, bool IsActive)
        {
            Contractor oContractor = new Contractor();
            try
            {
                oContractor = oContractor.CommitActivity(id, IsActive, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oContractor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oContractor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PrintContractors(FormCollection DataCollection)
        {
            _oContractor = new Contractor();
            _oContractor.Contractors = new JavaScriptSerializer().Deserialize<List<Contractor>>(DataCollection["txtContractorCollectionList"]);

            string sSQL = "";
            string sContractorIDs = string.Join(",", _oContractor.Contractors.Select(o=>o.ContractorID));
            if (!string.IsNullOrEmpty(sContractorIDs))
            {
                

                sSQL = "SELECT * FROM View_ContactPersonnel WHERE ContractorID IN (" + sContractorIDs + ")";
                _oContractor.ContactPersonnels = ContactPersonnel.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
         
            
            string Messge = "Contractor List";
            rptContractors oReport = new rptContractors();
            byte[] abytes = oReport.PrepareReport(_oContractor, Messge, oCompany);
            return File(abytes, "application/pdf");
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

        [HttpPost]
        public JsonResult UpdateVatAndReg(Contractor oContractor)
        {
            _oContractor = new Contractor();
            try
            {
                _oContractor = oContractor.Get(oContractor.ContractorID, (int)Session[SessionInfo.currentUserID]);
                _oContractor.VAT = oContractor.VAT;
                _oContractor.TIN = oContractor.TIN;
                _oContractor = _oContractor.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oContractor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateNeedCutOff(Contractor oContractor)
        {
            _oContractor = new Contractor();
            try
            {
                _oContractor = oContractor.UpdateNeedCutOff((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oContractor = new Contractor();
                _oContractor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCountry(Contractor oContractor)
        {
            _oContractor = new Contractor();
            try
            {
                _oContractor = oContractor.UpdateCountry((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oContractor = new Contractor();
                _oContractor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Contractor Searching
        [HttpPost]
        public JsonResult Get(Contractor oContractor)
        {
            _oContractor = new Contractor();
            try
            {
                if (oContractor.ContractorID <= 0) { throw new Exception("Please select a valid contractor."); }
                _oContractor = _oContractor.Get(oContractor.ContractorID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oContractor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetWithFactoryAddress(Contractor oContractor)
        {
            _oContractor = new Contractor();
            try
            {
                if (oContractor.ContractorID <= 0) { throw new Exception("Please select a valid contractor."); }
                _oContractor = _oContractor.Get(oContractor.ContractorID, (int)Session[SessionInfo.currentUserID]);

                string sSQL = "SELECT TOP(1)* FROM ContractorAddress WHERE ContractorID = " + _oContractor.ContractorID + " AND AddressType=" + (int)EnumAddressType.Factory + "";
                List<ContractorAddress> oContractorAddresss = new List<ContractorAddress>();
                oContractorAddresss = ContractorAddress.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oContractorAddresss.Count > 0)
                {
                    _oContractor.Address = oContractorAddresss[0].Address;
                }
            
            }
            catch (Exception ex)
            {
                _oContractor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetContractors(Contractor oContractor)
        {
            _oContractors = new List<Contractor>();


            string sSQL = GetSearchSQL(oContractor.Params);
            _oContractors = Contractor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oContractors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSearchSQL(string sString)
        {

            string sContractorName = Convert.ToString(sString.Split('~')[0]);
            bool IsBuyer = Convert.ToBoolean(sString.Split('~')[1]);
            bool IsFactory = Convert.ToBoolean(sString.Split('~')[2]);
            bool IsSupplier = Convert.ToBoolean(sString.Split('~')[3]);
            bool IsGroup = false;
            string sReturn1 = "";
            string sReturn = " ";
            #region Make String
            if (sContractorName != null)
            {
                if (sContractorName != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Name Like '%" + sContractorName + "%' ";
                }
            }
            if (IsSupplier == true && IsBuyer == true && IsFactory == true)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorType IN (1,2,3) ";
            }
            else if (IsSupplier == true && IsBuyer == true && IsFactory == false)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorType IN (1,2) ";
            }
            else if (IsSupplier == true && IsBuyer == false && IsFactory == true)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorType IN (1,3) ";
            }
            else if (IsSupplier == true && IsBuyer == false && IsFactory == false)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorType IN (1) ";
            }
            else if (IsSupplier == false && IsBuyer == true && IsFactory == true)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorType IN (2,3) ";
            }
            else if (IsSupplier == false && IsBuyer == true && IsFactory == false)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorType IN (2) ";
            }
            else if (IsSupplier == false && IsBuyer == false && IsFactory == true)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorType IN (3) ";
            }
            else if (IsGroup == false)
            {
                string sTemp = "";
                sTemp = " GroupID in (select ContractorID from Contractor  where ContractorID>0 " + sReturn + ")";
                Global.TagSQL(ref sReturn, true);
                sReturn = sReturn + sTemp;
            }
            else
            {

                sReturn = sReturn + "";
            }




            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        [HttpPost]
        public JsonResult ContractorSearchByNameType(Contractor oContractor) // Added By Sagor on 24 May 2014 For Enter Event Searching
        {
            _oContractors = new List<Contractor>();
            try
            {
                string sType = oContractor.Params.Split('~')[0];
                string sName = oContractor.Params.Split('~')[1].Trim();
                if (sName == "@ContractorID") sName = "";
                int nBUID = 0;
                if(oContractor.Params.Split('~').Count()>2)
                {

                    nBUID = Convert.ToInt32(oContractor.Params.Split('~')[2]);
                }
                _oContractors = Contractor.GetsByNamenType(sName, sType, nBUID, (int)Session[SessionInfo.currentUserID]);
                if (_oContractors.Count <= 0) { throw new Exception("No information found."); }
            }
            catch (Exception ex)
            {
                oContractor = new Contractor();
                oContractor.ErrorMessage = ex.Message;
                _oContractors.Add(oContractor);
            }
            var jsonResult = Json(_oContractors, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ContractorSearchByPIStatus(Contractor oContractor)
        {
            _oContractors = new List<Contractor>();
            try
            {
                string sType = oContractor.Params.Split('~')[0];
                string sName = oContractor.Params.Split('~')[1].Trim();
                int nPIStatus = Convert.ToInt32(oContractor.Params.Split('~')[2]);
                var nBUID = 0;
                if (oContractor.Params.Split('~').Count() > 3)
                {
                    nBUID = Convert.ToInt32(oContractor.Params.Split('~')[3]);
                }
                // Supplier=1; Buyer=2, Factory=3, Company=5,
                string sReturn1 = "SELECT * FROM Contractor";
                string sReturn = "";

                #region sType
                if (!string.IsNullOrEmpty(sType))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID IN ( SELECT ContractorID FROM ContractorType WHERE ContractorType IN (" + sType + "))";
                }
                #endregion

                #region sName
                if (!string.IsNullOrEmpty(sName))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Name Like '%" + sName + "%' ";
                }
                #endregion

                #region PI Status
                if (nPIStatus > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID IN (SELECT ContractorID FROM ExportPI WHERE PIStatus=" + nPIStatus + ") ";
                }
                #endregion
                #region BUID
                if (nBUID>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID IN ( SELECT ContractorID FROM BUWiseParty WHERE BUID = "+nBUID+") ";
                }
                #endregion
                string sSQL = sReturn1 + " " + sReturn + " ORDER BY Name";
                _oContractors = Contractor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oContractors.Count <= 0) { throw new Exception("No information found."); }

            }
            catch (Exception ex)
            {
                oContractor = new Contractor();
                oContractor.ErrorMessage = ex.Message;
                _oContractors.Add(oContractor);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsbyContractorType(Contractor oContractor)
        {
            List<Contractor> oContractors = new List<Contractor>();
            try
            {
                if (oContractor.ContractorTypeInInt == 0)
                {
                    oContractors = Contractor.Gets((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oContractors = Contractor.GetsByNamenType("", oContractor.ContractorTypeInInt.ToString(),0, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oContractors = new List<Contractor>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oContractors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ContractorSearch()
        {
            List<Contractor> oContractors = new List<Contractor>();
            return PartialView(oContractors);
        }

        public ActionResult ContractorSearchByKeyPress(string sStr, int nType, double nts) // Added By Sagor on 24 May 2014 For Enter Event Searching
        {
            List<Contractor> oContractors = new List<Contractor>();
            Contractor oContractor = new Contractor();
            try
            {
                // Supplier=1; Buyer=2, Factory=3, Company=5,
                string sSQL = "";
                if (nType > 0)
                {
                    sSQL = "and Name Like '%" + sStr + "%' and ContractorType=" + nType + "";
                }
                else
                {
                    sSQL = "and Name Like '%" + sStr + "%'";
                }
                oContractors = Contractor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oContractors.Count <= 0)
                {
                    throw new Exception("Nothing Found.");
                }

            }
            catch (Exception ex)
            {
                oContractors = new List<Contractor>();
                oContractor.ErrorMessage = ex.Message;
                oContractors.Add(oContractor);
            }
            return PartialView(oContractors);
        }

        [HttpGet]
        public JsonResult Gets(int Temp, double ts)
        {
            List<Contractor> oContractors = new List<Contractor>();
            try
            {
                if (Temp == 0)
                {
                    oContractors = Contractor.Gets((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oContractors = Contractor.GetsByNamenType("", Temp.ToString(),0, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oContractors = new List<Contractor>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oContractors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BUWiseContractorSearch(Contractor oContractor)
        {
            List<Contractor> oContractors = new List<Contractor>();
            try
            {
                string sCName = oContractor.Params.Split('~')[0];
                int nBUID = Convert.ToInt32(oContractor.Params.Split('~')[1]);
                string sSQL = "SELECT * FROM Contractor";
                string sReturn = "";
                if(sCName!=null && sCName!="")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Name Like '%" + sCName + "%' ";
                }
                if (nBUID >0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID IN ( SELECT ContractorID FROM BUWiseParty WHERE BUID= " + nBUID + ")";
                }
                sSQL += sReturn;
                oContractors = Contractor.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
               
            }
            catch (Exception ex)
            {
                oContractors = new List<Contractor>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oContractors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BUAndTypeWiseContractorSearch(Contractor oContractor)
        {
            List<Contractor> oContractors = new List<Contractor>();
            try
            {
                string sCName = oContractor.Params.Split('~')[0];
                int nBUID = Convert.ToInt32(oContractor.Params.Split('~')[1]);
                int nCType = Convert.ToInt32(oContractor.Params.Split('~')[2]);


                string sSQL = "SELECT * FROM View_Contractor";
                string sReturn = "";
                if (sCName != null && sCName != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Name Like '%" + sCName + "%' ";
                }
                if (nBUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID IN ( SELECT ContractorID FROM BUWiseParty WHERE BUID= " + nBUID.ToString() + ")";
                }
                if (nCType > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID IN (SELECT HH.ContractorID FROM ContractorType AS HH WHERE HH.ContractorType=" + nCType.ToString() + ")";
                }
                sSQL = sSQL + sReturn + " ORDER BY Name ASC";
                oContractors = Contractor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oContractors = new List<Contractor>();
            }

            var jsonResult = Json(oContractors, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Contract Personnel
        public ActionResult ViewContactPersonnel(int id, int buid)
        {
            _oContractor = new Contractor();
            if (id > 0)
            {
                _oContractor = _oContractor.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oContractor.ContactPersonnels = ContactPersonnel.GetsByContractor(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.BUID = buid;
            return View(_oContractor);
        }

        [HttpPost]
        public JsonResult SaveContactPersonnel(ContactPersonnel oContactPersonnel)
        {
            _oContactPersonnel = new ContactPersonnel();
            try
            {
                _oContactPersonnel = oContactPersonnel;
                _oContactPersonnel = _oContactPersonnel.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oContactPersonnel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContactPersonnel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditContactPersonnel(ContactPersonnel oContactPersonnel)
        {
            _oContactPersonnel = new ContactPersonnel();
            _oContactPersonnel = oContactPersonnel;
            _oContactPersonnel = _oContactPersonnel.Save((int)Session[SessionInfo.currentUserID]);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContactPersonnel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteContactPersonnel(ContactPersonnel oContactPersonnel)
        {
            string sFeedBackMessage = "";
            try
            {
                if (oContactPersonnel.ContactPersonnelID <= 0) { throw new Exception("Please select a valid contact personnel."); }
                sFeedBackMessage = oContactPersonnel.Delete(oContactPersonnel.ContactPersonnelID, (int)Session[SessionInfo.currentUserID]);

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
        public JsonResult GetContactPersonnels(Contractor oContractor)
        {
            _oContactPersonnels = new List<ContactPersonnel>();
            _oContactPersonnel = new ContactPersonnel();
            try
            {
                if (oContractor.ContractorID <= 0) { throw new Exception("Please select a valid contractor."); }
                _oContactPersonnels = ContactPersonnel.Gets(oContractor.ContractorID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oContactPersonnel.ErrorMessage = ex.Message;
                _oContactPersonnels.Add(_oContactPersonnel);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oContactPersonnels);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetContactPersonnel(ContactPersonnel oContactPersonnel)
        {
            _oContactPersonnel = new ContactPersonnel();
            try
            {
                if (oContactPersonnel.ContactPersonnelID <= 0) { throw new Exception("Please select a valid contact personnel."); }
                _oContactPersonnel = _oContactPersonnel.Get(oContactPersonnel.ContactPersonnelID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oContactPersonnel.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContactPersonnel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ContractorAddress
        [HttpPost]
        public JsonResult DeleteContractorAddress(ContractorAddress oContractorAddress)
        {
            string sFeedBackMessage = "";
            try
            {
                if (oContractorAddress.ContractorAddressID <= 0) { throw new Exception("Please select a valid contact personnel."); }
                sFeedBackMessage = oContractorAddress.Delete(oContractorAddress.ContractorAddressID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Buyer Concern
        public ActionResult ViewBuyerConcern(int id, double ts)
        {
            _oContractor = new Contractor();
            if (id > 0)
            {
                _oContractor = _oContractor.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oContractor.BuyerConcerns = BuyerConcern.GetsByContractor(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oContractor);
        }
        [HttpPost]
        public JsonResult GetBuyerConcerns(Contractor oContractor)
        {
            _oBuyerConcerns = new List<BuyerConcern>();
            try
            {
                _oBuyerConcerns = BuyerConcern.GetsByContractor(oContractor.ContractorID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oContactPersonnel = new ContactPersonnel();
                _oContactPersonnel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBuyerConcerns);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveBuyerConcern(BuyerConcern oBuyerConcern)
        {
            _oBuyerConcern = new BuyerConcern();
            try
            {
                _oBuyerConcern = oBuyerConcern;
                _oBuyerConcern = _oBuyerConcern.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBuyerConcern.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBuyerConcern);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteBuyerConcern(int id)
        {
            string sFeedBackMessage = "";
            _oBuyerConcern = new BuyerConcern();
            try
            {
                sFeedBackMessage = _oBuyerConcern.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public void PrintXL(string sParams)
        {
            _oContractor = new Contractor();
            _oContractors = new List<Contractor>();
            string sCName = sParams.Split('~')[0];
            int nBUID = Convert.ToInt32(sParams.Split('~')[1]);
            int nCType = Convert.ToInt32(sParams.Split('~')[2]);
            string sSearchingCriteria = "";
            
            string sSQL = "SELECT * FROM Contractor";
            string sReturn = "";
            if (sCName != null && sCName != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Name Like '%" + sCName + "%' ";
                sSearchingCriteria = "Search With Name : ''" + sCName + "''";
            }
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN ( SELECT ContractorID FROM BUWiseParty WHERE BUID= " + nBUID.ToString() + ")";
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                if (sSearchingCriteria == "")
                {
                    sSearchingCriteria = "Search With Business Unit : ''" + oBusinessUnit.ShortName + "''";
                }
                else
                {
                    sSearchingCriteria = sSearchingCriteria + " & Business Unit : ''" + oBusinessUnit.ShortName+"''";
                }
            }
            if (nCType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (SELECT HH.ContractorID FROM ContractorType AS HH WHERE HH.ContractorType=" + nCType.ToString() + ")";
                if (sSearchingCriteria == "")
                {
                    sSearchingCriteria = "Search With Contractor Type  : ''" + EnumObject.jGet((EnumContractorType)nCType)+"''";
                }
                else
                {
                    sSearchingCriteria = sSearchingCriteria + " & Contractor Type  : ''" + EnumObject.jGet((EnumContractorType)nCType)+"''";
                }
            }
            sSQL = sSQL + sReturn + " ORDER BY Name ASC";
            _oContractors = Contractor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            _oContactPersonnels = new List<ContactPersonnel>();
            _oContractorAddresss = new List<ContractorAddress>();

            //_oContractor = new Contractor();
            //if (!string.IsNullOrEmpty(sParams))
            //{
            //    string sSQL = "SELECT * FROM Contractor WHERE ContractorID IN (" + sParams + ")";
            //    _oContractor.Contractors = Contractor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);


            //    sSQL = "SELECT * FROM View_ContactPersonnel WHERE ContractorID IN (" + sParams + ")";
            //    _oContactPersonnels = ContactPersonnel.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //}
         

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var _sheet = excelPackage.Workbook.Worksheets.Add("Contractor List");
                _sheet.Name = "Contractor";
                _sheet.Column(2).Width = 10; //SL
                _sheet.Column(3).Width = 40; //BuyerName
                _sheet.Column(4).Width = 40; //OfficeAddress
                _sheet.Column(5).Width = 40; //FactoryAddress
                _sheet.Column(6).Width = 40; //ContactPerson
                _sheet.Column(7).Width = 30; //Contact
                _sheet.Column(8).Width = 30; //Email
                

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                _sheet.Cells[_nRowIndex, 2, _nRowIndex, 8].Merge = true;
                _cell = _sheet.Cells[_nRowIndex, 2]; _cell.Value = oCompany.Name; _cell.Style.Font.Bold = true;
                _cell.Style.Font.Size = 20; _cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                _nRowIndex = _nRowIndex + 1;

                _sheet.Cells[_nRowIndex, 2, _nRowIndex, 8].Merge = true;
                _cell = _sheet.Cells[_nRowIndex, 2]; _cell.Value = "Contractor List"; _cell.Style.Font.Bold = true; _cell.Style.Font.UnderLine = true;
                _cell.Style.Font.Size = 15; _cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                _nRowIndex = _nRowIndex + 1;

                if (sSearchingCriteria != "")
                {
                    _sheet.Cells[_nRowIndex, 2, _nRowIndex, 8].Merge = true;
                    _cell = _sheet.Cells[_nRowIndex, 2]; _cell.Value = sSearchingCriteria; _cell.Style.Font.Bold = false; _cell.Style.Font.UnderLine = true;
                    _cell.Style.Font.Size = 15; _cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    _nRowIndex = _nRowIndex + 1;
                }
                _nRowIndex = _nRowIndex + 1;
                #endregion

                #region Column Header

                _cell = _sheet.Cells[_nRowIndex, 2]; _cell.Value = "SL"; _cell.Style.Font.Bold = true;
                _fill = _cell.Style.Fill; _fill.PatternType = ExcelFillStyle.Solid; _fill.BackgroundColor.SetColor(Color.Cyan);
                _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                _cell = _sheet.Cells[_nRowIndex, 3]; _cell.Value = "Buyer Name"; _cell.Style.Font.Bold = true;
                _fill = _cell.Style.Fill; _fill.PatternType = ExcelFillStyle.Solid; _fill.BackgroundColor.SetColor(Color.Cyan);
                _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                _cell = _sheet.Cells[_nRowIndex, 4]; _cell.Value = "PI Address(Bond)"; _cell.Style.Font.Bold = true;
                _fill = _cell.Style.Fill; _fill.PatternType = ExcelFillStyle.Solid; _fill.BackgroundColor.SetColor(Color.Cyan);
                _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                _cell = _sheet.Cells[_nRowIndex, 5]; _cell.Value = "Address(HO)"; _cell.Style.Font.Bold = true;
                _fill = _cell.Style.Fill; _fill.PatternType = ExcelFillStyle.Solid; _fill.BackgroundColor.SetColor(Color.Cyan);
                _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                _cell = _sheet.Cells[_nRowIndex, 6]; _cell.Value = "Factory Address"; _cell.Style.Font.Bold = true;
                _fill = _cell.Style.Fill; _fill.PatternType = ExcelFillStyle.Solid; _fill.BackgroundColor.SetColor(Color.Cyan);
                _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                _cell = _sheet.Cells[_nRowIndex, 7]; _cell.Value = "Contact Person"; _cell.Style.Font.Bold = true;
                _fill = _cell.Style.Fill; _fill.PatternType = ExcelFillStyle.Solid; _fill.BackgroundColor.SetColor(Color.Cyan);
                _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                _cell = _sheet.Cells[_nRowIndex, 8]; _cell.Value = "Contact"; _cell.Style.Font.Bold = true;
                _fill = _cell.Style.Fill; _fill.PatternType = ExcelFillStyle.Solid; _fill.BackgroundColor.SetColor(Color.Cyan);
                _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                _cell = _sheet.Cells[_nRowIndex, 9]; _cell.Value = "Email"; _cell.Style.Font.Bold = true;
                _fill = _cell.Style.Fill; _fill.PatternType = ExcelFillStyle.Solid; _fill.BackgroundColor.SetColor(Color.Cyan);
                _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;
                _nRowIndex = _nRowIndex + 1;

                #endregion

                List<ContactPersonnel> oContactPersonnels = new List<ContactPersonnel>();
                List<ContractorAddress> oContractorAddresss = new List<ContractorAddress>();

                #region Report Data
                string sFactoryAddress = "";

                int nCount = 0;
                foreach (Contractor oItem in _oContractors)
                {
                    oContactPersonnels = _oContactPersonnels.Where(o => o.ContractorID == oItem.ContractorID).ToList();                    
                    if (oContactPersonnels.Count>0)
                    {
                        foreach (ContactPersonnel oItemContactPersonnel in oContactPersonnels)
                        {
                            nCount++;
                            _cell = _sheet.Cells[_nRowIndex, 2]; _cell.Value = nCount.ToString(); _cell.Style.Font.Bold = false;
                            _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                            _cell = _sheet.Cells[_nRowIndex, 3]; _cell.Value = oItem.Name; _cell.Style.Font.Bold = false;
                            _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                            _cell = _sheet.Cells[_nRowIndex, 4]; _cell.Value = oItem.Address; _cell.Style.Font.Bold = false;
                            _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                            _cell = _sheet.Cells[_nRowIndex, 5]; _cell.Value = sFactoryAddress; _cell.Style.Font.Bold = false;
                            _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                            _cell = _sheet.Cells[_nRowIndex, 6]; _cell.Value = ""; _cell.Style.Font.Bold = false;
                            _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                            _cell = _sheet.Cells[_nRowIndex, 7]; _cell.Value = oItemContactPersonnel.Name; _cell.Style.Font.Bold = false;
                            _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                            _cell = _sheet.Cells[_nRowIndex, 8]; _cell.Value = oItemContactPersonnel.Phone; _cell.Style.Font.Bold = false;
                            _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                            _cell = _sheet.Cells[_nRowIndex, 9]; _cell.Value = oItemContactPersonnel.Email; _cell.Style.Font.Bold = false;
                            _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                            _nRowIndex++;
                        }
                    }
                    else
                    {
                        nCount++;
                        _cell = _sheet.Cells[_nRowIndex, 2]; _cell.Value = nCount.ToString(); _cell.Style.Font.Bold = false;
                        _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                        _cell = _sheet.Cells[_nRowIndex, 3]; _cell.Value = oItem.Name; _cell.Style.Font.Bold = false;
                        _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                        _cell = _sheet.Cells[_nRowIndex, 4]; _cell.Value = oItem.Address; _cell.Style.Font.Bold = false;
                        _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                        _cell = _sheet.Cells[_nRowIndex, 5]; _cell.Value = oItem.Address2; _cell.Style.Font.Bold = false;
                        _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                        _cell = _sheet.Cells[_nRowIndex, 6]; _cell.Value = oItem.Address3; _cell.Style.Font.Bold = false;
                        _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                        _cell = _sheet.Cells[_nRowIndex, 7]; _cell.Value = ""; _cell.Style.Font.Bold = false;
                        _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                        _cell = _sheet.Cells[_nRowIndex, 8]; _cell.Value = oItem.Phone; _cell.Style.Font.Bold = false;
                        _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                        _cell = _sheet.Cells[_nRowIndex, 9]; _cell.Value = oItem.Email; _cell.Style.Font.Bold = false;
                        _border = _cell.Style.Border; _border.Bottom.Style = _border.Top.Style = _border.Left.Style = _border.Right.Style = ExcelBorderStyle.Thin;

                        _nRowIndex++;
                    }
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Contractor.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        #region Autocomplete Gets
        [HttpGet]
        public JsonResult GetsContractorAutocomplete(string ContractorName)
        {           
            List<Contractor> oContractors = new List<Contractor>();
            ContractorName = ContractorName == null ? "" : ContractorName;
            string sSQL = "SELECT * FROM View_Contractor AS HH WHERE HH.Name LIKE '%" + ContractorName + "%' ORDER BY HH.Name ASC";
            oContractors = Contractor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(oContractors, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            // Query is not required as of version 1.2.5
            //jsonResult = { "query": "Unit",    "suggestions": }
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetsContractorAutocomplete2(string ContractorName)
       {
            int nContractorID = 0;
            try
            {
                nContractorID = (int)Session["ContractorID"];
            }
            catch (Exception ex)
            {
                nContractorID = 0;
            }
                        
            List<Contractor> oContractors = new List<Contractor>();
            ContractorName = ContractorName == null ? "" : ContractorName;
            string sSQL = "SELECT * FROM View_Contractor AS HH WHERE HH.Name LIKE '%" + ContractorName + "%' ORDER BY HH.Name ASC";
            if (nContractorID > 0)
            {
                sSQL = "SELECT * FROM View_Contractor AS HH WHERE HH.ContractorID =" + nContractorID.ToString() + " ORDER BY HH.Name ASC";
            }
            oContractors = Contractor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(oContractors, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            // Query is not required as of version 1.2.5
            //jsonResult = { "query": "Unit",    "suggestions": }
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SetAutocompleteSessionData(Contractor oContractor)
        {
            List<Contractor> oContractors = new List<Contractor>();
            this.Session.Remove("ContractorID");
            this.Session.Add("ContractorID", oContractor.ContractorID);
            var jsonResult = Json(oContractors, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;        
            return jsonResult;
        }
        #endregion

        #region Crystal Report
        public ActionResult CrystalReportTest()
        {
            List<Contractor> oContractors = new List<Contractor>();
            oContractors = Contractor.GetsByBU(2, (int)Session[SessionInfo.currentUserID]);

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptContractor.rpt"));
            rd.SetDataSource(oContractors);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");
            }
            catch { throw; }
        }
        #endregion
       
    }
}
