using System;
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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class EmployeeSalaryStructureController : Controller
    {
        #region Declaration
        EmployeeSalaryStructure _oEmployeeSalaryStructure;
        private List<EmployeeSalaryStructure> _oEmployeeSalaryStructures;
        EmployeeSalaryStructureDetail _oEmployeeSalaryStructureDetail;
        #endregion

        #region Views
        public ActionResult View_EmployeeSalaryStructures(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            //_oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oEmployeeSalaryStructures);
        }

        public ActionResult View_EmployeeSalaryStructure(int nId, double ts)//nId=ESSID 
        {
            _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            if (nId > 0)
            {
                _oEmployeeSalaryStructure = EmployeeSalaryStructure.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                string Ssql = "SELECT * FROM View_EmployeeSalaryStructureDetail WHERE ESSID=" + nId;
                _oEmployeeSalaryStructure.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(Ssql, ((User)(Session[SessionInfo.CurrentUser])).UserID);


            }

            return PartialView(_oEmployeeSalaryStructure);
        }

        public ActionResult View_ITaxBasicInformation(int nId, double ts)
        {
            ITaxBasicInformation oITaxBasicInformation = new ITaxBasicInformation();
            if (nId > 0)
            {
                oITaxBasicInformation = ITaxBasicInformation.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            return PartialView(oITaxBasicInformation);
        }

        public ActionResult View_EmployeeSalaryStructures_V1(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            //_oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oEmployeeSalaryStructures);
        }

        public ActionResult View_EmployeeSalaryStructure_V1(int nId, double ts)//nId=ESSID 
        {
            _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            if (nId > 0)
            {
                _oEmployeeSalaryStructure = EmployeeSalaryStructure.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                string Ssql = "SELECT * FROM View_EmployeeSalaryStructureDetail WHERE ESSID=" + nId;
                _oEmployeeSalaryStructure.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(Ssql, ((User)(Session[SessionInfo.CurrentUser])).UserID);


                List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
                List<SalarySchemeDetail> oSalarySchemeDetails = new List<SalarySchemeDetail>();

                string sSql = "SELECT * FROM  View_SalarySchemeDetail WHERE SalarySchemeID=" + _oEmployeeSalaryStructure.SalarySchemeID + " ORDER BY SalarySchemeDetailID ";
                string sSql1 = "SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID IN (SELECT SalarySchemeDetailID FROM  SalarySchemeDetail WHERE SalarySchemeID=" + _oEmployeeSalaryStructure.SalarySchemeID + ") ORDER BY SalarySchemeDetailID ";

                oSalarySchemeDetails = SalarySchemeDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oSalarySchemeDetailCalculations = SalarySchemeDetailCalculation.Gets(sSql1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oSalarySchemeDetails = SalarySchemeDetail.GetNewSalarySchemeDetail(oSalarySchemeDetails, oSalarySchemeDetailCalculations);

                foreach (SalarySchemeDetail oSSDItem in oSalarySchemeDetails)
                {
                    foreach (EmployeeSalaryStructureDetail oESSDItem in _oEmployeeSalaryStructure.EmployeeSalaryStructureDetails)
                    {

                        if (oSSDItem.SalaryHeadID == oESSDItem.SalaryHeadID)
                        {
                            oESSDItem.Calculation = oSSDItem.Calculation;
                        }
                    }

                }

            }
            return PartialView(_oEmployeeSalaryStructure);
        }

        #endregion

        #region IUD
        [HttpPost]
        public JsonResult EmployeeSalaryStructure_IU(EmployeeSalaryStructure oEmployeeSalaryStructure)
        {

            _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            try
            {
                _oEmployeeSalaryStructure = oEmployeeSalaryStructure;

                if (_oEmployeeSalaryStructure.ESSID > 0)
                {
                    _oEmployeeSalaryStructure = _oEmployeeSalaryStructure.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oEmployeeSalaryStructure = _oEmployeeSalaryStructure.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                _oEmployeeSalaryStructure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSalaryStructure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EmployeeSalaryStructureDetail_IU(EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail)
        {

            _oEmployeeSalaryStructureDetail = new EmployeeSalaryStructureDetail();
            try
            {
                _oEmployeeSalaryStructureDetail = oEmployeeSalaryStructureDetail;

                if (_oEmployeeSalaryStructureDetail.ESSSID > 0)
                {
                    _oEmployeeSalaryStructureDetail = _oEmployeeSalaryStructureDetail.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oEmployeeSalaryStructureDetail = _oEmployeeSalaryStructureDetail.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oEmployeeSalaryStructureDetail = new EmployeeSalaryStructureDetail();
                _oEmployeeSalaryStructureDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSalaryStructureDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult EmployeeSalaryStructure_Delete(int nESSID, double ts)//nnESSID=nESSID
        {
            _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            try
            {

                _oEmployeeSalaryStructure.ESSID = nESSID;
                _oEmployeeSalaryStructure = _oEmployeeSalaryStructure.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                _oEmployeeSalaryStructure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSalaryStructure.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EmployeeSalaryStructureDetail_Delete(EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail)//nESSSID=ESSSID
        {
            string sFeedbackMessage = Global.DeleteMessage;
            try
            {
                sFeedbackMessage = oEmployeeSalaryStructureDetail.DeleteSingleSalaryStructureDetail(((User)(Session[SessionInfo.CurrentUser])).UserID);                
            }
            catch (Exception ex)
            {
                sFeedbackMessage = "";
                sFeedbackMessage = ex.Message;                
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedbackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Activity
        [HttpPost]
        public JsonResult EmployeeSalaryStructure_Activity(EmployeeSalaryStructure oEmployeeSalaryStructure)
        {
            _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            try
            {

                _oEmployeeSalaryStructure = EmployeeSalaryStructure.Activite(oEmployeeSalaryStructure.EmployeeID, oEmployeeSalaryStructure.ESSID, oEmployeeSalaryStructure.IsActive, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                _oEmployeeSalaryStructure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSalaryStructure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CopyFromOtherStructure
        [HttpGet]
        public JsonResult CopyFromOtherStructure(int nId, double ts)//nId=EmployeeID 
        {
            _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            try
            {
                string Ssql = "SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID=" + nId;
                _oEmployeeSalaryStructure = EmployeeSalaryStructure.Get(Ssql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                string Ssqld = "SELECT * FROM View_EmployeeSalaryStructureDetail WHERE ESSID=" + _oEmployeeSalaryStructure.ESSID;
                _oEmployeeSalaryStructure.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(Ssqld, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
                List<SalarySchemeDetail> oSalarySchemeDetails = new List<SalarySchemeDetail>();

                string sSql = "SELECT * FROM  View_SalarySchemeDetail WHERE SalarySchemeID=" + _oEmployeeSalaryStructure.SalarySchemeID + " ORDER BY SalarySchemeDetailID ";
                string sSql1 = "SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID IN (SELECT SalarySchemeDetailID FROM  SalarySchemeDetail WHERE SalarySchemeID=" + _oEmployeeSalaryStructure.SalarySchemeID + ") ORDER BY SalarySchemeDetailID ";

                oSalarySchemeDetails = SalarySchemeDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oSalarySchemeDetailCalculations = SalarySchemeDetailCalculation.Gets(sSql1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oSalarySchemeDetails = SalarySchemeDetail.GetNewSalarySchemeDetail(oSalarySchemeDetails, oSalarySchemeDetailCalculations);

                foreach (SalarySchemeDetail oSSDItem in oSalarySchemeDetails)
                {
                    foreach (EmployeeSalaryStructureDetail oESSDItem in _oEmployeeSalaryStructure.EmployeeSalaryStructureDetails)
                    {

                        if (oSSDItem.SalaryHeadID == oESSDItem.SalaryHeadID)
                        {
                            oESSDItem.Calculation = oSSDItem.Calculation;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                _oEmployeeSalaryStructure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSalaryStructure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SalaryStructureGetForPTI
        [HttpPost]
        public JsonResult GetSalaryStructure(EmployeeSalaryStructure oEmployeeSalaryStructure)
        {
            try
            {

                string sSql = "";
                sSql = "SELECT * FROM View_EmployeeSalaryStructure WHERE  IsActive=1 AND EmployeeID=" + oEmployeeSalaryStructure.EmployeeID;
                oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                oEmployeeSalaryStructure = EmployeeSalaryStructure.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oEmployeeSalaryStructure.EmployeeID == 0)
                {
                    throw new Exception("This Employee Has No Salary Structure !");
                }

                string Ssql = "SELECT * FROM View_EmployeeSalaryStructureDetail WHERE ESSID=" + oEmployeeSalaryStructure.ESSID;
                oEmployeeSalaryStructure.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(Ssql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
                List<SalarySchemeDetail> oSalarySchemeDetails = new List<SalarySchemeDetail>();

                string sSql_SSD = "SELECT * FROM  View_SalarySchemeDetail WHERE SalarySchemeID=" + oEmployeeSalaryStructure.SalarySchemeID + " ORDER BY SalarySchemeDetailID ";
                string sSql1 = "SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID IN (SELECT SalarySchemeDetailID FROM  SalarySchemeDetail WHERE SalarySchemeID=" + oEmployeeSalaryStructure.SalarySchemeID + ") ORDER BY SalarySchemeDetailID ";

                oSalarySchemeDetails = SalarySchemeDetail.Gets(sSql_SSD, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oSalarySchemeDetailCalculations = SalarySchemeDetailCalculation.Gets(sSql1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oSalarySchemeDetails = SalarySchemeDetail.GetNewSalarySchemeDetail(oSalarySchemeDetails, oSalarySchemeDetailCalculations);

                foreach (SalarySchemeDetail oSSDItem in oSalarySchemeDetails)
                {
                    foreach (EmployeeSalaryStructureDetail oESSDItem in oEmployeeSalaryStructure.EmployeeSalaryStructureDetails)
                    {
                        if (oSSDItem.SalaryHeadID == oESSDItem.SalaryHeadID)
                        {
                            oESSDItem.Calculation = oSSDItem.Calculation;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oEmployeeSalaryStructure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeSalaryStructure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion SalaryStructureGetForPTI

        #region print Employee Salary Structure
        public ActionResult View_PrintSalaryStructure(int nId, double ts)//nId=ESSID 
        {
            _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            return PartialView(_oEmployeeSalaryStructure);
        }

        public ActionResult PrintESalaryStructure(string sESSIDs, double ts)
        {

            EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail = new EmployeeSalaryStructureDetail();

            //string sSql = "SELECT * FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID IN(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID IN(" + sEmpID + ")) ORDER BY SalaryHeadID";
            //string sSql1 = "SELECT*FROM View_EmployeeSalaryStructure WHERE EmployeeID IN(" + sEmpID + ")";
            //string sSql2 = "SELECT * FROM SalaryHead WHERE SalaryHeadType=1";

            string sSql = "SELECT * FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID IN (" + sESSIDs + ") ORDER BY SalaryHeadID";
            string sSql1 = "SELECT*FROM View_EmployeeSalaryStructure WHERE ESSID IN(" + sESSIDs + ")";
            string sSql2 = "SELECT * FROM SalaryHead WHERE SalaryHeadType=1";



            oEmployeeSalaryStructureDetail.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalaryStructureDetail.SalaryHeads = SalaryHead.Gets(sSql2, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalaryStructureDetail.EmployeeSalaryStructures = EmployeeSalaryStructure.Gets(sSql1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalaryStructureDetail.Company = oCompanys.First();

            rptESalaryStructure oReport = new rptESalaryStructure();
            byte[] abytes = oReport.PrepareReport(oEmployeeSalaryStructureDetail);
            return File(abytes, "application/pdf");

        }
        #endregion  print Employee Salary Structure

        #region Search by Code/Name
        [HttpGet]
        public JsonResult GetByEmployeeNameCode(string sNameCode, bool bIsCode, double nts)
        {
            EmployeeSalaryStructure oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            _oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();

            try
            {
                string sSQL = "";
                if (bIsCode)
                {
                    sSQL = "SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID IN (SELECT EmployeeID FROM Employee WHERE  Code LIKE  '%" + sNameCode + "%')";
                }
                else
                {
                    sSQL = "SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID IN (SELECT EmployeeID FROM Employee WHERE  Name LIKE '%" + sNameCode + "%')";
                }
                _oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeSalaryStructure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSalaryStructures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Copy Salary Structure
        [HttpPost]
        public JsonResult CopyEmployeeSalaryStructure(int nCopyFromESSID, List<Employee> oEmployees)
        {

            _oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            try
            {
                if (nCopyFromESSID <= 0)
                {
                    throw new Exception("Please Select an Item from list");
                }
                if (oEmployees.Count <= 0)
                {
                    throw new Exception("You have to select employes from picker to copy.");
                }
                _oEmployeeSalaryStructures = EmployeeSalaryStructure.CopyEmployeeSalaryStructure(nCopyFromESSID, oEmployees, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                _oEmployeeSalaryStructure.ErrorMessage = ex.Message;
                _oEmployeeSalaryStructures.Add(_oEmployeeSalaryStructure);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSalaryStructures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ITaxBasicInformation
        [HttpPost]
        public JsonResult ITaxBasicInformation_IU(ITaxBasicInformation oITaxBasicInformation)
        {
            try
            {
                if (oITaxBasicInformation.ITaxBasicInformationID > 0)
                {
                    oITaxBasicInformation = oITaxBasicInformation.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oITaxBasicInformation = oITaxBasicInformation.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oITaxBasicInformation = new ITaxBasicInformation();
                oITaxBasicInformation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxBasicInformation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion ITaxBasicInformation

        #region Employee Information
        [HttpPost]
        public JsonResult GetSalaryStructure_Copy(EmployeeSalaryStructure oESStructure)
        {

            _oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            try
            {
                
                int nId = Convert.ToInt32(oESStructure.ErrorMessage.Split('~')[0]);
                string sType = oESStructure.ErrorMessage.Split('~')[1];
                if (sType == "SalaryStructure")
                {
                    string Ssql = "SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID=" + nId;
                    _oEmployeeSalaryStructure = EmployeeSalaryStructure.Get(Ssql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    if (_oEmployeeSalaryStructure.ESSID <= 0)
                    {
                        throw new Exception("This Employee has not assigned any salary structure. Please select an employee who has already assigned salary structure !");
                    }
                    string Ssqld = "SELECT * FROM View_EmployeeSalaryStructureDetail WHERE ESSID=" + _oEmployeeSalaryStructure.ESSID;
                    _oEmployeeSalaryStructure.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(Ssqld, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                    nId = _oEmployeeSalaryStructure.SalarySchemeID;
                    
                }

                else
                {
                    SalaryScheme oSalaryScheme = new SalaryScheme();
                    oSalaryScheme = SalaryScheme.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    nId = _oEmployeeSalaryStructure.SalarySchemeID = oSalaryScheme.SalarySchemeID;
                    _oEmployeeSalaryStructure.IsProductionBase = oSalaryScheme.IsProductionBase;
                    _oEmployeeSalaryStructure.IsAllowOverTime = oSalaryScheme.IsAllowOverTime;
                    _oEmployeeSalaryStructure.IsAttendanceDependent = oSalaryScheme.IsAttendanceDependant;
                    _oEmployeeSalaryStructure.IsAllowBankAccount = oSalaryScheme.IsAllowBankAccount;                    
                    _oEmployeeSalaryStructure.OverTimeON = oSalaryScheme.OverTimeON;
                    _oEmployeeSalaryStructure.OverTimeON = oSalaryScheme.OverTimeON;
                    _oEmployeeSalaryStructure.DividedBy = oSalaryScheme.DividedBy;
                    _oEmployeeSalaryStructure.MultiplicationBy = oSalaryScheme.MultiplicationBy;
                    _oEmployeeSalaryStructure.LateCount = oSalaryScheme.LateCount;
                    _oEmployeeSalaryStructure.EarlyLeavingCount = oSalaryScheme.EarlyLeavingCount;

                }
                //else if (sType == "SalaryScheme")
                //{
                //    SalaryScheme oSalaryScheme = new SalaryScheme();
                //    List<SalaryHead> oSalaryHeads = new List<SalaryHead>();

                //    List<SalarySchemeDetail> oSalarySchemeDetails = new List<SalarySchemeDetail>();

                //    oSalaryScheme = SalaryScheme.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                //    oSalaryScheme.SalarySchemeDetails = SalarySchemeDetail.Gets(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                //    string sSql1 = "SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID IN (SELECT SalarySchemeDetailID FROM  SalarySchemeDetail WHERE SalarySchemeID=" + nId + ") ORDER BY SalarySchemeDetailID ";

                //    oSalaryScheme.SalarySchemeDetailCalculations = SalarySchemeDetailCalculation.Gets(sSql1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                //    oSalaryScheme.SalarySchemeDetails = SalarySchemeDetail.GetNewSalarySchemeDetail(oSalaryScheme.SalarySchemeDetails, oSalaryScheme.SalarySchemeDetailCalculations);

                //    string sSql = "SELECT * FROM SalaryHead WHERE SalaryHeadType=1 AND SalaryHeadID NOT IN (SELECT SalaryHeadID FROM SalarySchemeDetail WHERE SalarySchemeID=" + nId + ")";
                //    oSalaryHeads = SalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                //    foreach (SalaryHead oitem in oSalaryHeads)
                //    {
                //        SalarySchemeDetail oSalarySchemeDetail = new SalarySchemeDetail();
                //        oSalarySchemeDetail.SalaryHeadID = oitem.SalaryHeadID;
                //        oSalarySchemeDetail.SalaryHeadName = oitem.Name;
                //        oSalarySchemeDetail.SalaryHeadType = oitem.SalaryHeadType;
                //        oSalaryScheme.SalarySchemeDetails.Add(oSalarySchemeDetail);
                //    }


                //    oSalaryScheme.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                //    _oEmployeeSalaryStructure.SalarySchemeName = oSalaryScheme.Name;

                //}


                List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
                List<SalarySchemeDetail> oSalarySchemeDetails = new List<SalarySchemeDetail>();

                string sSql = "SELECT * FROM  View_SalarySchemeDetail WHERE SalarySchemeID=" + nId + " ORDER BY SalarySchemeDetailID ";
                string sSql1 = "SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID IN (SELECT SalarySchemeDetailID FROM  SalarySchemeDetail WHERE SalarySchemeID=" + nId + ") ORDER BY SalarySchemeDetailID ";

                oSalarySchemeDetails = SalarySchemeDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oSalarySchemeDetailCalculations = SalarySchemeDetailCalculation.Gets(sSql1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oSalarySchemeDetails = SalarySchemeDetail.GetNewSalarySchemeDetail(oSalarySchemeDetails, oSalarySchemeDetailCalculations);
                int nDetailCount = _oEmployeeSalaryStructure.EmployeeSalaryStructureDetails.Count;

                foreach (SalarySchemeDetail oSSDItem in oSalarySchemeDetails)
                {
                    if (nDetailCount > 0)
                    {
                        foreach (EmployeeSalaryStructureDetail oESSDItem in _oEmployeeSalaryStructure.EmployeeSalaryStructureDetails)
                        {

                            if (oSSDItem.SalaryHeadID == oESSDItem.SalaryHeadID)
                            {
                                oESSDItem.Calculation = oSSDItem.Calculation;
                                List<SalarySchemeDetailCalculation> oSSDCs = new List<SalarySchemeDetailCalculation>();
                                oSSDCs = oSalarySchemeDetailCalculations.Where(x => x.SalarySchemeDetailID == oSSDItem.SalarySchemeDetailID).ToList();
                                oESSDItem.SalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
                                oESSDItem.SalarySchemeDetailCalculations.AddRange(oSSDCs);
                            }
                            
                        }
                    }
                    else
                    {
                        EmployeeSalaryStructureDetail oESSD = new EmployeeSalaryStructureDetail();
                        oESSD.SalaryHeadID = oSSDItem.SalaryHeadID;
                        oESSD.SalaryHeadName = oSSDItem.SalaryHeadName;
                        oESSD.SalaryHeadType = oSSDItem.SalaryHeadType;
                        oESSD.Calculation = oSSDItem.Calculation;

                        oESSD.Condition = oSSDItem.Condition;
                        oESSD.Period = oSSDItem.Period;

                        oESSD.Times = oSSDItem.Times;
                        oESSD.DeferredDay = oSSDItem.DeferredDay;
                        oESSD.ActivationAfter = oSSDItem.ActivationAfter;
                        oESSD.Amount = oSSDItem.Amount;
                        oESSD.CompAmount = oSSDItem.CompAmount;

                        List<SalarySchemeDetailCalculation> oSSDCs = new List<SalarySchemeDetailCalculation>();
                        oSSDCs = oSalarySchemeDetailCalculations.Where(x => x.SalarySchemeDetailID == oSSDItem.SalarySchemeDetailID).ToList();
                        oESSD.SalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
                        oESSD.SalarySchemeDetailCalculations.AddRange(oSSDCs);
                        _oEmployeeSalaryStructure.EmployeeSalaryStructureDetails.Add(oESSD);
                    }
                  

                }
            }
            catch (Exception ex)
            {
                _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                _oEmployeeSalaryStructure.ErrorMessage = ex.Message;
            
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSalaryStructure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Employee Information

        #region get salary structure
        [HttpPost]
        public JsonResult GetEmployeeSalaryStructure(EmployeeSalaryStructure oEmployeeSalaryStructure)
        {
            try
            {
                _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                string Sql = "SELECT * FROM View_EmployeeSalaryStructure WHERE IsActive=1 AND EmployeeID=" + oEmployeeSalaryStructure.EmployeeID;
                _oEmployeeSalaryStructure = EmployeeSalaryStructure.Get(Sql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
                _oEmployeeSalaryStructure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSalaryStructure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsEmployeeSalaryStructure(EmployeeSalaryStructure oEmployeeSalaryStructure)
        {
            try
            {
                _oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
                string Sql = "SELECT * FROM View_EmployeeSalaryStructure WHERE IsActive=1 AND EmployeeID IN(" + oEmployeeSalaryStructure.IDs + ")";//IDs=EmployeeID
                _oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets(Sql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
                _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                _oEmployeeSalaryStructure.ErrorMessage = ex.Message;
                _oEmployeeSalaryStructures.Add(_oEmployeeSalaryStructure);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSalaryStructures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Print Confirmation Letter
        public ActionResult PrintConfirmationLetter_MAMIYA(int nEmpID, double ts)
        {
            Employee oEmployee = new Employee();
            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
            oEmployee.EmployeeOfficial = oEmployeeOfficial.Get(nEmpID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            string sSql = "select * from View_EmployeeSalaryStructureDetail WHERE ESSID=(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + nEmpID + ") ";
            oEmployee.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployee.EmployeeSalaryStructure = EmployeeSalaryStructure.Get("SELECT * FROM VIEW_EmployeeSalaryStructure WHERE EmployeeID=" + nEmpID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployee.Company = oCompanys.First();
            rptConfirmationLetter_MAMIYA oReport = new rptConfirmationLetter_MAMIYA();
            byte[] abytes = oReport.PrepareReport(oEmployee);
            return File(abytes, "application/pdf");
        }
        #endregion Print Conf. Letter

        #region print XL

        public void PrintESalaryStructure_XL(string sIDs, string sIDType, double ts)
        {
            EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail = new EmployeeSalaryStructureDetail();
            string sSql = "";
            string sSql1 = "";
            string sSql2 = "";
            if (sIDType == "EmployeeID")
            {
                sSql = "SELECT * FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID IN (SELECT ESSID  FROM EmployeeSalaryStructure WHERE EmployeeID IN(" + sIDs + ")) ORDER BY SalaryHeadID";
                sSql1 = "SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID IN(" + sIDs + ") ORDER BY EmployeeCode";
                sSql2 = "SELECT * FROM SalaryHead WHERE SalaryHeadType=1";
            }

            oEmployeeSalaryStructureDetail.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalaryStructureDetail.EmployeeSalaryStructures = EmployeeSalaryStructure.Gets(sSql1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalaryStructureDetail.SalaryHeads = SalaryHead.Gets(sSql2, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            
            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            double nGross = 0;
            using (var excelPackage = new ExcelPackage())
            {
                //int nBasicTypeSalary = 0;
                //nBasicTypeSalary = oEmployeeSalaryStructureDetail.SalaryHeads.Count;
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("EMPLOYEE LIST");
                sheet.Name = "EMPLOYEE LIST";
                int n = 2;
                sheet.Column(n++).Width = 5; //SL
                sheet.Column(n++).Width = 15; //CODE
                sheet.Column(n++).Width = 30; //NAME
                sheet.Column(n++).Width = 30; //FATHER_NAME
                sheet.Column(n++).Width = 12; //JOINING
                sheet.Column(n++).Width = 12; //LOCATION
                sheet.Column(n++).Width = 20; //DEPARTMENT
                sheet.Column(n++).Width = 20; //DESIGNATION
                sheet.Column(n++).Width = 16; //EMPLOYEE TYPE
                sheet.Column(n++).Width = 16; //GENDER
                sheet.Column(n++).Width = 16; //RELIGION
                sheet.Column(n++).Width = 12; //DATE OF BORTH
                foreach (SalaryHead oItem in oEmployeeSalaryStructureDetail.SalaryHeads)
                {
                    sheet.Column(n++).Width = 12;
                }
                sheet.Column(n++).Width = 12; //GROSS
                nMaxColumn = n;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "EMPPLOYEE LIST"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CODE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NAME"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "FATHER NAME"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "JOINING DATE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "LOCATION"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DEPARTMENT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EMPLOYEE TYPE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "GENDER"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "RELIGION"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DATE OF BIRTH"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                foreach (SalaryHead oItem in oEmployeeSalaryStructureDetail.SalaryHeads)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "GROSS"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion

                #region Table Body
                
                int nSL = 0;
                
                foreach (EmployeeSalaryStructure oEItem in oEmployeeSalaryStructureDetail.EmployeeSalaryStructures)
                {

                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEItem.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEItem.EmployeeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEItem.FatherName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEItem.DateOfJoinInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEItem.LocationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEItem.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEItem.EmployeeTypeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEItem.Gender; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEItem.Religion; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEItem.DateOfBirthInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    foreach (SalaryHead oItem in oEmployeeSalaryStructureDetail.SalaryHeads)
                    {
                        List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
                        oEmployeeSalaryStructureDetails = oEmployeeSalaryStructureDetail.EmployeeSalaryStructureDetails.Where(x => x.SalaryHeadID == oItem.SalaryHeadID && x.ESSID == oEItem.ESSID).ToList();
                        double nAmount = 0;
                        if (oEmployeeSalaryStructureDetails.Count > 0)
                        {
                            nAmount = oEmployeeSalaryStructureDetails[0].Amount;
                        }


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nAmount); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oEItem.GrossAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nGross += oEItem.GrossAmount;
                    rowIndex++;
                }
                #endregion

                #region Total
                colIndex = 12;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                foreach (SalaryHead oItem in oEmployeeSalaryStructureDetail.SalaryHeads)
                {
                    List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
                    oEmployeeSalaryStructureDetails = oEmployeeSalaryStructureDetail.EmployeeSalaryStructureDetails.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).ToList();
                    double nAmount = 0;
                    nAmount = oEmployeeSalaryStructureDetail.EmployeeSalaryStructureDetails.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).Sum(x => x.Amount);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nGross); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EMPLOYEE LIST.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        #endregion  print XL

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

    }
}
