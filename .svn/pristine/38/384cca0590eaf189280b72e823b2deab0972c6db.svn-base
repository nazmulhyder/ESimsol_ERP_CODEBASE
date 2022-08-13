using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using ReportManagement;
using iTextSharp.text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
namespace ESimSolFinancial.Controllers
{
    public class ContractorPersonalController : Controller
    {
        #region Declaration
        ContactPersonnel _oContractorPersonal = new ContactPersonnel();
        List<ContactPersonnel> _oContractorPersonals = new List<ContactPersonnel>();
        string _sErrorMessage = "";
        #endregion

        #region New Code
        [HttpPost]
        public JsonResult GetsByBuyer(ContactPersonnel oContactPersonnel)
        {
            _oContractorPersonals = new List<ContactPersonnel>();
            try
            {
                if (oContactPersonnel.ContractorID > 0)
                {
                    _oContractorPersonals = ContactPersonnel.Gets(oContactPersonnel.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oContractorPersonals = new List<ContactPersonnel>();
                _oContractorPersonal.ErrorMessage = ex.Message;
                _oContractorPersonals.Add(_oContractorPersonal);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractorPersonals);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByContractors(ContactPersonnel oContactPersonnel)
        {
            _oContractorPersonals = new List<ContactPersonnel>();
            try
            {
                string sSQL = "SELECT * FROM View_ContactPersonnel";
                string sReturn = "";

                #region Contractor
                if (!String.IsNullOrEmpty(oContactPersonnel.ContractorName))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContactPersonnelID in (select ContactPersonnelID from ContactPersonMapping where ContractorID in(" + oContactPersonnel.ContractorName + "))";
                }
                #endregion
                #region Name
                if (!String.IsNullOrEmpty(oContactPersonnel.Name))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "Name like '%" + oContactPersonnel.Name + "%'";
                }
                #endregion

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "Activity=1";

                sSQL = sSQL + "" + sReturn;
                _oContractorPersonals = ContactPersonnel.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oContractorPersonals = new List<ContactPersonnel>();
                _oContractorPersonal.ErrorMessage = ex.Message;
                _oContractorPersonals.Add(_oContractorPersonal);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractorPersonals);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Old Code

        #region Functions
        private bool ValidateInput(ContactPersonnel oContractorPersonal)
        {
            if (oContractorPersonal.ContractorID == null || oContractorPersonal.ContractorID<= 0)
            {
                _sErrorMessage = "Invalid Contractor Please try again";
                return false;
            }
            if (oContractorPersonal.Name == null || oContractorPersonal.Name == "")
            {
                _sErrorMessage = "Please enter Personnel Name";
                return false;
            }
            if (oContractorPersonal.Phone == null || oContractorPersonal.Phone == "")
            {
                _sErrorMessage = "Please enter Personnel Phone ";
                return false;
            }
            return true;
        }
        #endregion

        public ActionResult Add(int id)
        {
            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oContractorPersonal = new ContactPersonnel();
            _oContractorPersonal.ContractorID = id;
            _oContractorPersonal.SelectedContractor = "Selected Contractor :" + oContractor.Name + " [" + oContractor.Phone + "]";
            _oContractorPersonal.ContractorPersonnelForSelectedContractor = ContactPersonnel.GetsByContractor(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oContractorPersonal);
        }

        [HttpPost]
        public ActionResult Add(ContactPersonnel oContractorPersonal)
        {
            try
            {
                Contractor oContractor = new Contractor();
                if (this.ValidateInput(oContractorPersonal))
                {
                    oContractorPersonal = oContractorPersonal.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    oContractor = new Contractor();
                    oContractor = oContractor.Get(oContractorPersonal.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oContractorPersonal = new ContactPersonnel();
                    _oContractorPersonal.ContractorID = oContractorPersonal.ContractorID;
                    _oContractorPersonal.SelectedContractor = "Selected Bank :" + oContractor.Name + " [" + oContractor.Phone + "]";
                    _oContractorPersonal.ContractorPersonnelForSelectedContractor = ContactPersonnel.GetsByContractor(oContractorPersonal.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    return View(_oContractorPersonal);
                }
                TempData["message"] = _sErrorMessage;
                oContractor = new Contractor();
                oContractor = oContractor.Get(oContractorPersonal.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oContractorPersonal = new ContactPersonnel();
                _oContractorPersonal.ContractorID = oContractorPersonal.ContractorID;
                _oContractorPersonal.SelectedContractor = "Selected Bank :" + oContractor.Name + " [" + oContractor.Phone + "]";
                _oContractorPersonal.ContractorPersonnelForSelectedContractor = ContactPersonnel.GetsByContractor(oContractorPersonal.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                return View(_oContractorPersonal);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }

        }

        public ActionResult Edit(int id)
        {
            ContactPersonnel oContractorPersonal = new ContactPersonnel();
            oContractorPersonal = oContractorPersonal.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oContractorPersonal);
        }

        [HttpPost]
        public ActionResult Edit(ContactPersonnel oContractorPersonal)
        {
            try
            {
                if (this.ValidateInput(oContractorPersonal))
                {
                    oContractorPersonal = oContractorPersonal.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    return RedirectToAction("Add", "ContractorPersonal", new { id = oContractorPersonal.ContractorID });
                }
                TempData["message"] = _sErrorMessage;
                return View(oContractorPersonal);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult Delete(int id)
        {
            ContactPersonnel oContractorPersonal = new ContactPersonnel();
            oContractorPersonal = oContractorPersonal.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oContractorPersonal);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactPersonnel oContractorPersonal = new ContactPersonnel();
            oContractorPersonal = oContractorPersonal.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oContractorPersonal.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return RedirectToAction("Add", "ContractorPersonal", new { id = oContractorPersonal.ContractorID });
        }

        public ViewResult Details(int id)
        {
            ContactPersonnel oContractorPersonal = new ContactPersonnel();
            oContractorPersonal = oContractorPersonal.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oContractorPersonal);
        }
        [HttpPost]
        public JsonResult RefreshControl(Contractor oContractor)
        {
            _oContractorPersonals = new List<ContactPersonnel>();
            _oContractorPersonals = ContactPersonnel.Gets(oContractor.ContractorID,((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oContractorPersonals);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region GetBuyerContractorPerson

        [HttpGet]
        public JsonResult GetBuyerContractorPerson(int Id)//Id=ContractorID
        {
            try
            {
                string Ssql = "SELECT*FROM View_ContactPersonnel WHERE ContractorID=" + Id;
                _oContractorPersonals = new List<ContactPersonnel>();
                _oContractorPersonals = ContactPersonnel.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oContractorPersonal.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractorPersonals);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetByContractorPerson(ContactPersonnel oContactPersonnel)//Id=ContractorID
        {
            try
            {
                string Ssql = "SELECT*FROM View_ContactPersonnel WHERE ContractorID=" + oContactPersonnel.ContractorID + "AND NAME LIKE '%" + oContactPersonnel.Name + "%' ";
                _oContractorPersonals = new List<ContactPersonnel>();
                _oContractorPersonals = ContactPersonnel.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oContractorPersonal.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractorPersonals);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region CP Entry, Map & Merge
        public ActionResult ContactPersonnels(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ContactPersonnel> oContactPersonnels = new List<ContactPersonnel>();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();

            string sSQL = "SELECT * FROM View_ContactPersonnelWithImage WHERE Activity=1 and ContactPersonnelID IN (SELECT ContactPersonnelID FROM  ContactPersonMapWithBU WHERE BUID=" + buid + ") ORDER BY Name";
            oContactPersonnels = ContactPersonnel.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            
            ViewBag.BUID = buid;
            ViewBag.BUs = oBusinessUnits;
            return View(oContactPersonnels);
        }
        public ActionResult Mapping(int nId, int buid, double ts)
        {
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            ContactPersonnel oContactPersonnel = new ContactPersonnel();
            if (nId > 0)
            {
                oContactPersonnel = oContactPersonnel.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oContactPersonnel.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN (SELECT BUID FROM ContactPersonMapWithBU WHERE ContactPersonnelID=" + nId + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                oContactPersonnel.Contractors = Contractor.Gets("SELECT * FROM View_Contractor WHERE ContractorID IN (SELECT ContractorID FROM ContactPersonMapping WHERE ContactPersonnelID=" + nId + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;

            ViewBag.BUID = buid;
            return View(oContactPersonnel);
        }
        [HttpPost]
        public JsonResult Save(ContactPersonnel oContactPersonnel)
        {
            oContactPersonnel.RemoveNulls();
            ContactPersonnel _oContactPersonnel = new ContactPersonnel();
            try
            {
                _oContactPersonnel = oContactPersonnel.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oContactPersonnel = new ContactPersonnel();
                _oContactPersonnel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContactPersonnel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveContractor(ContactPersonnel oContactPersonnel)
        {
            oContactPersonnel.RemoveNulls();
            ContactPersonnel _oContactPersonnel = new ContactPersonnel();
            try
            {
                _oContactPersonnel = oContactPersonnel.IUDContractor(EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oContactPersonnel = new ContactPersonnel();
                _oContactPersonnel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContactPersonnel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult MergeContactPersonnel(ContactPersonnel oContactPersonnel)
        {
            oContactPersonnel.RemoveNulls();
            ContactPersonnel _oContactPersonnel = new ContactPersonnel();
            try
            {
                _oContactPersonnel = oContactPersonnel.MergeCP(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oContactPersonnel = new ContactPersonnel();
                _oContactPersonnel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContactPersonnel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteContractor(ContactPersonnel oContactPersonnel)
        {
            oContactPersonnel.RemoveNulls();
            ContactPersonnel _oContactPersonnel = new ContactPersonnel();
            try
            {
                _oContactPersonnel = oContactPersonnel.IUDContractor(EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oContactPersonnel = new ContactPersonnel();
                _oContactPersonnel.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContactPersonnel);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByName(ContactPersonnel oContactPersonnel)
        {
            _oContractorPersonals = new List<ContactPersonnel>();
            try
            {

                string sSQL = "SELECT top(100)* FROM View_ContactPersonnel";
                string sReturn = "";

                #region Contractor
                if (!String.IsNullOrEmpty(oContactPersonnel.ContractorName))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContactPersonnelID in (select ContactPersonnelID from ContactPersonMapping where ContractorID in(" + oContactPersonnel.ContractorName + "))";
                }
                #endregion
                #region Name
                if (!String.IsNullOrEmpty(oContactPersonnel.Name))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "Name like '%" + oContactPersonnel.Name + "%'";
                }
                #endregion

                #region Name
                if (oContactPersonnel.BUID>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ContactPersonnelID in ( select ContactPersonnelID from ContactPersonMapWithBU where ContactPersonMapWithBU.BUID in (" + oContactPersonnel.BUID + "))";
                }
                #endregion

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "Activity=1";

                sSQL = sSQL + "" + sReturn;
                _oContractorPersonals = ContactPersonnel.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //oContactPersonnel.Name = (string.IsNullOrEmpty(oContactPersonnel.Name) ? "" : oContactPersonnel.Name);
                //_oContractorPersonals = ContactPersonnel.Gets("SELECT top(200)* FROM View_ContactPersonnel WHERE Activity=1 and Name LIKE '%" + oContactPersonnel.Name + "%'", ((User)Session[SessionInfo.CurrentUser]).UserID);
                
            }
            catch (Exception ex)
            {
                _oContractorPersonals = new List<ContactPersonnel>();
                _oContractorPersonal.ErrorMessage = ex.Message;
                _oContractorPersonals.Add(_oContractorPersonal);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractorPersonals);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region PRINT TO EXCEL
        public void ExportToExcel(int buid)
        {
            List<ContactPersonnel> oContactPersonnels = new List<ContactPersonnel>();
            string _sErrorMesage;
            try
            {
                _sErrorMesage = "";
                string sSQL = "SELECT * FROM View_ContactPersonnelWithImage WHERE Activity=1 and ContactPersonnelID IN (SELECT ContactPersonnelID FROM  ContactPersonMapWithBU WHERE BUID=" + buid + ") ORDER BY Name";
                oContactPersonnels = ContactPersonnel.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oContactPersonnels.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                oContactPersonnels = new List<ContactPersonnel>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nStartCol = 2, nEndCol = 12, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("ExportIncentive");
                    sheet.Name = "Export Incentive";
                    sheet.Column(++nColumn).Width = 15; //SL
                    //sheet.Column(++nColumn).Width = 10; //TextileUnitST
                    sheet.Column(++nColumn).Width = 20; //Code
                    sheet.Column(++nColumn).Width = 35; //Name No
                    sheet.Column(++nColumn).Width = 40; //Contractor
                    sheet.Column(++nColumn).Width = 25; //Phone
                    sheet.Column(++nColumn).Width = 25; //email
                    sheet.Column(++nColumn).Width = 35; //addd
             
                    nEndCol = nColumn;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 3]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nEndCol - 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Contact Personnel"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 3]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //cell = sheet.Cells[nRowIndex, nStartCol + 10, nRowIndex, nEndCol]; cell.Merge = true;
                    //cell.Value = ""; cell.Style.Font.Bold = false;
                    //cell.Style.WrapText = true;
                    //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion


                    #region Report Data

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region
                    nColumn = 1;

                    string[] sHeader = new string[] {"SLNo","Code","Name","Contractor Name","Phone","Email", "Address"};

                    foreach (string Header in sHeader)
                    {
                        this.AddExcelHeader(ref cell, sheet, Header, nRowIndex, ++nColumn);
                    }

                    nRowIndex++;
                    #endregion

                    //string sCurrencySymbol = oContactPersonnels.Select(x => x.CurrencySymbol).FirstOrDefault();

                    #region Data
                    foreach (ContactPersonnel oItem in oContactPersonnels)
                    {
                        nCount++;
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ContactPersonnelID; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Phone; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Email; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Address; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    #endregion

                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ExportIncentive.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        public void AddExcelHeader(ref ExcelRange cell, ExcelWorksheet sheet, string sHeader, int nRowIndex, int nColumn)
        {
            OfficeOpenXml.Style.Border border;
            cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = sHeader; cell.Style.Font.Bold = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }
        #endregion
        #endregion
    }
}
