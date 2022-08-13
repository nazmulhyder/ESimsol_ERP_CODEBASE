using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Net.Mail;
using OfficeOpenXml;
using OfficeOpenXml.Style;

using CrystalDecisions.CrystalReports.Engine;

namespace ESimSolFinancial.Controllers
{
    public class ServiceScheduleController : Controller
    {
        #region Declaration
        ServiceSchedule _oServiceSchedule = new ServiceSchedule();
        List<ServiceSchedule> _oServiceSchedules = new List<ServiceSchedule>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewServiceSchedules(int nPIID, int buid)
        {
            PreInvoice oPreInvoice = new PreInvoice();
            List<ServiceSchedule> oServiceSchedules = new List<ServiceSchedule>();
            if(nPIID>0)
            {
                oPreInvoice = oPreInvoice.Get(nPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oServiceSchedules = ServiceSchedule.Gets("SELECT * FROM View_ServiceSchedule WHERE PreInvoiceID = " + nPIID + " ORDER BY ServiceDate", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.PreInvoice = oPreInvoice;
            //ViewBag.ChangeTypes = EnumObject.jGets(typeof(EnumServiceILaborChargeType));
            List<EnumObject> oTemp = EnumObject.jGets(typeof(EnumServiceILaborChargeType)).Where(x => x.id == (int)EnumServiceILaborChargeType.Paying || x.id == (int)EnumServiceILaborChargeType.Complementary ).ToList();
            ViewBag.ChangeTypes = oTemp;
            return View(oServiceSchedules);
        }
        public ActionResult ViewServiceScheduleMgts(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ServiceSchedule> oServiceSchedules = new List<ServiceSchedule>();
            ViewBag.ServiceStatusList = EnumObject.jGets(typeof(EnumServiceScheduleStatus));
            return View(oServiceSchedules);
        }

        #region SearchSchedules
        [HttpPost]
        public JsonResult SearchSchedules(ServiceSchedule oServiceSchedule)
        {
            _oServiceSchedules = new List<ServiceSchedule>();
            try
            {
                StringBuilder sSQL = new StringBuilder();
                sSQL = MakSQL(oServiceSchedule);
                _oServiceSchedules = ServiceSchedule.Gets(sSQL.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oServiceSchedule = new ServiceSchedule();
                _oServiceSchedule.ErrorMessage = ex.Message;
                _oServiceSchedules.Add(_oServiceSchedule);
            }
            var jsonResult = Json(_oServiceSchedules, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private StringBuilder MakSQL(ServiceSchedule oServiceSchedule)
        {
            StringBuilder sSQL = new StringBuilder("SELECT * FROM View_ServiceSchedule WHERE ServiceScheduleID!=0 ");
            bool bIsDateSearch = Convert.ToBoolean(oServiceSchedule.Param.Split('~')[0]);
            DateTime dStartDate= Convert.ToDateTime(oServiceSchedule.Param.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(oServiceSchedule.Param.Split('~')[2]);
            string sCustomerIDs = oServiceSchedule.Param.Split('~')[3];
            if(!string.IsNullOrEmpty(oServiceSchedule.InvoiceNo))
            {
                sSQL.Append(" AND InvoiceNo LIKE '%"+oServiceSchedule.InvoiceNo+"%'");
            }
            if (!string.IsNullOrEmpty(oServiceSchedule.ModelNo))
            {
                sSQL.Append(" AND ModelNo LIKE '%" + oServiceSchedule.ModelNo + "%'");
            }
            if (oServiceSchedule.Status!=EnumServiceScheduleStatus.None)
            {
                sSQL.Append(" AND Status=" +(int)oServiceSchedule.Status);
            }
            if (!string.IsNullOrEmpty(sCustomerIDs))
            {
                sSQL.Append(" AND ContractorID IN (" + sCustomerIDs+")");
            }
            if(bIsDateSearch)
            {
                sSQL.Append(" AND ServiceDate >= '" + dStartDate.ToString("dd MMM yyyy") + "' AND ServiceDate < '" + dEndDate.AddDays(1).ToString("dd MMM yyyy") + "'");
            }
            sSQL.Append(" Order By ServiceDate");//order by
            return sSQL;
        }
        #endregion

        #region SendEmailOrSMS
        [HttpPost]
        public JsonResult SendEmailOrSMS(ServiceSchedule oServiceSchedule)
        {
            _oServiceSchedule = new  ServiceSchedule ();
            try
            {
                
                if (!oServiceSchedule.IsEmailSend)
                {
                    _oServiceSchedule = oServiceSchedule.SendEmailOrSMS(false, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
                else
                {    
                    #region Email Region
                    oServiceSchedule.ServiceSchedules = ServiceSchedule.Gets("SELECT * FROM View_ServiceSchedule WHERE ServiceScheduleID IN ( "+oServiceSchedule.Param+")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (ServiceSchedule oItem in oServiceSchedule.ServiceSchedules)
                    {
                        Contractor oCustomer = new Contractor();
                        oCustomer = oCustomer.Get(oItem.ContractorID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        if (Global.IsValidMail(oCustomer.Email))
                        {
                            List<string> emialTos = new List<string>();
                            emialTos.Add(oCustomer.Email);

                            StringBuilder subject = new StringBuilder("Audi Car Service");

                            StringBuilder bodyInfo = new StringBuilder("Dear Mr. " + oCustomer.Name + "<br><br>");
                            bodyInfo.Append("Good Day!<br> Greetings from Audi Bangladesh.<br><br>");
                            bodyInfo.Append("This mail is to inform you that, your vehicle Model No : <strong>" + oItem.ModelNo + "</strong> & Chassis No: <strong>" + oItem.ChassisNo + "</strong>  servicing is scheduled on " + oItem.ServiceDate.ToString("dd MMM yyyy") + " at " + oItem.ServiceDate.ToString("hh:mm tt")+ " .<br>");
                            bodyInfo.Append("You are requested to send your vehicle at Audi Service center accordingly.<br><br>");
                            bodyInfo.Append("Should you require any further information, please do not hesitate to contact us.<br><br><br><br>");

                            bodyInfo.Append("<strong>Abdus Salam Toton</strong><br>");
                            bodyInfo.Append("_____________________________<br>");
                            bodyInfo.Append("<strong>Administration Manager</strong><br>");

                            bodyInfo.Append("<p style='font-size:11px;'>Audi Service<br>Progress Motors Imports Limited<br>429/432, Tejgaon I/A <br>Dhaka - 1208 <br>Bangladesh<br>Phone   +880 2 889 1243<br>Mobile  +880 1933 336 215 <br>Hotline +880 19 CALL AUDI<br>mailto:abdus.sikder@pmilbd.com</p>");
                            #region Email Credential
                            EmailConfig oEmailConfig = new EmailConfig();
                            oEmailConfig = oEmailConfig.GetByBU(1, (int)Session[SessionInfo.currentUserID]);//it will 1 when Host 
                            #endregion

                            #region Attachment
                            
                            #endregion

                            oItem.EmailBody = bodyInfo.ToString();//Set mail info
                            Global.MailSend(subject.ToString(), bodyInfo.ToString(), emialTos, new List<string>(), new List<Attachment>(), oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
                        }
                    }
                    #endregion
                    _oServiceSchedule = oServiceSchedule.SendEmailOrSMS(true, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oServiceSchedule = new ServiceSchedule();
                _oServiceSchedule.ErrorMessage = ex.Message;
                
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Done
        [HttpPost]
        public JsonResult Done(ServiceSchedule oServiceSchedule)
        {
            _oServiceSchedule = new ServiceSchedule();
            try
            {
                _oServiceSchedule = oServiceSchedule.Done(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oServiceSchedule = new ServiceSchedule();
                _oServiceSchedule.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PhoneCall
        [HttpPost]
        public JsonResult PhoneCall(ServiceSchedule oServiceSchedule)
        {
            _oServiceSchedule = new ServiceSchedule();
            try
            {
                _oServiceSchedule = oServiceSchedule.PhoneCall(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oServiceSchedule = new ServiceSchedule();
                _oServiceSchedule.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region Re Schedule 
        [HttpPost]
        public JsonResult GetServiceSchedule(ServiceSchedule oServiceSchedule)
        {
            _oServiceSchedule = new ServiceSchedule();
            try
            {
                _oServiceSchedule = oServiceSchedule.Get(oServiceSchedule.ServiceScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oServiceSchedule = new ServiceSchedule();
                _oServiceSchedule.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReSchedule(ServiceSchedule oServiceSchedule)
        {
            _oServiceSchedule = new ServiceSchedule();
            try
            {
                _oServiceSchedule = oServiceSchedule.ReSchedule(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oServiceSchedule = new ServiceSchedule();
                _oServiceSchedule.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult Save(ServiceSchedule oServiceSchedule)
        {
            _oServiceSchedule = new ServiceSchedule();
            try
            {
                _oServiceSchedule = oServiceSchedule;
                _oServiceSchedule = _oServiceSchedule.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oServiceSchedule = new ServiceSchedule();
                _oServiceSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


    

        #region GetServiceSchedules
        [HttpPost]
        public JsonResult GetServiceSchedules(ServiceInvoice oServiceInvoice)
        {
            _oServiceSchedules = new List<ServiceSchedule>();
            try
            {
                StringBuilder sSQL = new StringBuilder("SELECT * FROM View_ServiceSchedule WHERE ISNULL(IsDone,0)=0 AND PreInvoiceID IN (SELECT HH.PreInvoiceID FROM View_PreInvoice AS HH WHERE HH.VehicleEngineID IN (SELECT VR.VehicleEngineID FROM VehicleRegistration AS VR WHERE VR.VehicleRegistrationID IN (SELECT VehicleRegistrationID FROM ServiceInvoice WHERE ServiceInvoiceID = " + oServiceInvoice.ServiceInvoiceID + ")))");

                _oServiceSchedules = ServiceSchedule.Gets(sSQL.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oServiceSchedule = new ServiceSchedule();
                _oServiceSchedule.ErrorMessage = ex.Message;
                _oServiceSchedules.Add(_oServiceSchedule);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceSchedules);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        public JsonResult SaveAll(PreInvoice oPreInvoice)
        {
            PreInvoice objPreInvoice = new PreInvoice();
            ServiceSchedule objServiceSchedule = new ServiceSchedule();
            try
            {
                foreach (ServiceSchedule oItem in oPreInvoice.ServiceSchedules)
                {
                    oItem.ServiceDate = Convert.ToDateTime(oItem.TempServiceDateSt);
                }
                if (oPreInvoice.PreInvoiceID > 0)
                {
                    objPreInvoice = oPreInvoice.SaveAll(oPreInvoice.ServiceSchedules, oPreInvoice.PreInvoiceID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                objPreInvoice = new PreInvoice();
                objPreInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(objPreInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(ServiceSchedule oServiceSchedule)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oServiceSchedule.Delete(oServiceSchedule.ServiceScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Get(ServiceSchedule oServiceSchedule)
        {
            _oServiceSchedule = new ServiceSchedule();
            try
            {
                if (oServiceSchedule.ServiceScheduleID > 0)
                {
                    _oServiceSchedule = _oServiceSchedule.Get(oServiceSchedule.ServiceScheduleID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oServiceSchedule = new ServiceSchedule();
                _oServiceSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
       
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                ServiceSchedule oServiceSchedule = new ServiceSchedule();
                sFeedBackMessage = oServiceSchedule.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #region Print
        [HttpPost]
        public JsonResult StoreIDsInSession(ServiceSchedule oServiceSchedule)
        {

                this.Session.Remove(SessionInfo.ParamObj);
                this.Session.Add(SessionInfo.ParamObj, oServiceSchedule);
                
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public void PrepareServiceScheduleInXL(double ts)
        {
            _oServiceSchedules = new List<ServiceSchedule>();

            _oServiceSchedule = new ServiceSchedule();
             _oServiceSchedule = (ServiceSchedule)Session[SessionInfo.ParamObj];
            StringBuilder sSQL = new StringBuilder("SELECT * FROM View_ServiceSchedule WHERE ServiceScheduleID IN ("+_oServiceSchedule.Param+")");
            _oServiceSchedules = ServiceSchedule.Gets(sSQL.ToString(), (int)Session[SessionInfo.currentUserID]);


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
           

            int nSL = 0;
            #region Export Excel
            int nRowIndex = 2, nEndRow = 0, nStartCol = 2, nEndCol = 20, nTempCol = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Service Schedule List");
                sheet.View.FreezePanes(5, 7);


                string sReportHeader = " ";
                #region Report Body & Header
               
                    sheet.Column(nTempCol).Width = 10; nTempCol++;//SL no
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Purchase date or status
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Customer Name
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//CRM
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Model
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Last Servie Done
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Upcomming Service Date

                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Contact NO
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Total free Service
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Remaining free Service
                    sheet.Column(nTempCol).Width = 15; nTempCol++;//Warrenty
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Remainging warre
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Phone call
                    sheet.Column(nTempCol).Width = 10; nTempCol++;//SMS
                    sheet.Column(nTempCol).Width = 10; nTempCol++;//Email
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Vin
                    sheet.Column(nTempCol).Width = 20; nTempCol++;//Reg
                    sheet.Column(nTempCol).Width = 10; nTempCol++;//Komm No
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Sales Person
                    sheet.Column(nTempCol).Width = 30; nTempCol++;//Remarks
               
                #endregion

               

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol-8]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[nRowIndex, nEndCol-7, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Service Schedule Mgt."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

          
                #region column title
                

                    nTempCol = 2;
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Purchase date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Customer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "CRM"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Model"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Last Service Done"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Upcoming Service Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Contact No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Total Free Service"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Remaining Free Sevice"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Warranty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Remain Waranty Duration"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

			
                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Phone Call"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "SMS"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Email"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Vin"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Reg"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Komm No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "SALES Person"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                #region Data
                    
                    int  nTempColIndex = 0;
                    foreach (ServiceSchedule oItem in _oServiceSchedules)
                    {
                            
                            nSL++;
                            nTempColIndex = 2;//REset 
                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; 
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = "" + oItem.InvoiceDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.CustomerName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.CRM; cell.Style.Font.Bold = false; 
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.ModelNo; cell.Style.Font.Bold = false; 
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.LastServiceDone!=DateTime.MinValue?oItem.LastServiceDone.ToString("dd MMM yyyy"):"-"; cell.Style.Font.Bold = false; cell.Merge = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value =oItem.UpcomingServiceDate!=DateTime.MinValue? oItem.UpcomingServiceDate.ToString("dd MMM yyyy"):"-"; cell.Style.Font.Bold = false; 
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            //Sl	Purchase date	Customer Name	CRM	Model 	Last Service Done 	Upcoming Service Date 	Contact No	Total free Service	Remaining Free Sevice 	Warranty 	Remain Waranty Duration	Phone Call	SMS	Email	Vin	Reg	Comm	SALES Person	Remarks
                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.CustomerPhoneNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.TotalFreeService; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.RemainingFreeService; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.Warrenty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.RemaingWarrenty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.IsPhoneCallSt; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Phone Call	SMS	Email	Vin	Reg	Comm	SALES Person	Remarks

                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.IsSMSSendSt; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.IsEmailSendSt; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.VinNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.RegistrationNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.kommNo; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.SalesPersonName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, nTempColIndex++]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                      
                        nEndRow = nRowIndex;
                        nRowIndex++;
                      }

                        #endregion
        

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=DeliveryVehicleRegister(Bulk).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion



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
        public Image GetCompanyTitle(Company oCompany)
        {
            if (oCompany.OrganizationTitle != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyImageTitle.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
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
