using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Xml.Serialization;

namespace ESimSol.Controllers
{
    public class LoanRequestController : Controller
    {
        #region Declaration
        LoanRequest _oLoanRequest = new LoanRequest();
        List<LoanRequest> _oLoanRequests = new List<LoanRequest>();
        #endregion

        #region LoanRequest
        public ActionResult ViewLoanRequests(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oLoanRequests = new List<LoanRequest>();
            string sSQL = "Select * from View_LoanRequest Where LoanRequestID<>0 And RequestStatus=" + (short)EnumRequestStatus.Request;
            _oLoanRequests = LoanRequest.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumLoanTypes = Enum.GetValues(typeof(EnumLoanType)).Cast<EnumLoanType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EnumRequestStatuss = Enum.GetValues(typeof(EnumRequestStatus)).Cast<EnumRequestStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EnumCompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = Global.CapitalSpilitor(x.ToString()), Value = ((int)x).ToString() }).ToList();
          
            return View(_oLoanRequests);
        }

        public ActionResult ViewLoanRequest(int nId, double ts)
        {
            _oLoanRequest = new LoanRequest();
            if (nId > 0)
            {
                _oLoanRequest = LoanRequest.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oLoanRequest.LoanRequestID > 0)
                {
                    //_oLoanRequest.IsAdjustable = (_oLoanRequest.EmployeeLoanID > 0) ? true : false;
                    ViewBag.LoanInstallments = this.GenerateInstallment(_oLoanRequest); 

                    string sSQL = "SELECT * FROM View_EmployeeOfficial Where EmployeeID=" + _oLoanRequest.EmployeeID;
                    var EmpOfficial = EmployeeOfficial.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sSQL = "SELECT * FROM View_EmployeeSalaryStructure Where EmployeeID=" + _oLoanRequest.EmployeeID;
                    var EmpSalary = EmployeeSalaryStructure.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    _oLoanRequest.OfficialInfo = (EmpOfficial.FirstOrDefault() != null) ? EmpOfficial.FirstOrDefault().DesignationName + " at " + EmpOfficial.FirstOrDefault().DepartmentName + " of " + EmpOfficial.FirstOrDefault().LocationName + ".  Joining Date: " + EmpOfficial.FirstOrDefault().DateOfJoinInString + ", Confirmation Date: " + EmpOfficial.FirstOrDefault().DateOfConfirmationInString + ", Year of Service: " + Global.GetDuration(EmpOfficial.FirstOrDefault().DateOfJoin, DateTime.Now) : "";
                    _oLoanRequest.SalaryInfo = (EmpSalary.FirstOrDefault() != null) ? EmpSalary.FirstOrDefault().PaymentCycleInString + " - " + Global.MillionFormat(EmpSalary.FirstOrDefault().GrossAmount) : "";
                    _oLoanRequest.PFInfo = this.GetPFInfo(_oLoanRequest.EmployeeID);
                    var oInfo = this.GetLastLoanInfo(_oLoanRequest.EmployeeID, _oLoanRequest.LoanRequestID, _oLoanRequest.IsPFLoan);
                    _oLoanRequest.EmployeeLoanID = (_oLoanRequest.IsAdjustable) ? oInfo.Item1 : 0;
                    _oLoanRequest.LastLoanInfo = oInfo.Item2;
                }
                
            }
            Company oCompany=new Company();
            oCompany = oCompany.Get(((User)Session[SessionInfo.CurrentUser]).CompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CurrencySymbol = (oCompany!=null && oCompany.CompanyID>0)? oCompany.CurrencySymbol: "tk";
            ViewBag.EnumLoanTypes = Enum.GetValues(typeof(EnumLoanType)).Cast<EnumLoanType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oLoanRequest);
        }



        [HttpPost]
        public JsonResult Save(LoanRequest oLoanRequest)
        {
            try
            {
                if (oLoanRequest.LoanRequestID <= 0)
                {
                    oLoanRequest = oLoanRequest.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oLoanRequest = oLoanRequest.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oLoanRequest = new LoanRequest();
                oLoanRequest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLoanRequest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(LoanRequest oLoanRequest)
        {
            try
            {
                if (oLoanRequest.LoanRequestID <= 0) { throw new Exception("Please select a valid item."); }
                oLoanRequest = oLoanRequest.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLoanRequest = new LoanRequest();
                oLoanRequest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLoanRequest.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approve(LoanRequest oLoanRequest)
        {
            try
            {
                if (oLoanRequest.LoanRequestID <= 0)
                    throw new Exception("Please select a valid loan request.");
                else if (oLoanRequest.RequestStatus == EnumRequestStatus.Approve)
                    throw new Exception("Already approved.");
                else if (oLoanRequest.RequestStatus == EnumRequestStatus.Cancel)
                    throw new Exception("Already canceled");

                var oELIs = GenerateInstallment(oLoanRequest).Where(x => x.Type == 1 && x.ELInstallmentID == 0 && x.ESDetailID==0).ToList();
               
                oLoanRequest = oLoanRequest.Approval(oELIs,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLoanRequest = new LoanRequest();
                oLoanRequest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLoanRequest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult Gets(LoanRequest oLoanRequest)
        {
            _oLoanRequests = new List<LoanRequest>();
            try
            {
                
                string sEmployeeIDs = (string.IsNullOrEmpty(oLoanRequest.Params.Split('~')[0])) ? "" : oLoanRequest.Params.Split('~')[0].Trim();
                short nLoanType =  Convert.ToInt16(oLoanRequest.Params.Split('~')[1]);
                short nLoanStatus =  Convert.ToInt16(oLoanRequest.Params.Split('~')[2]);
                short nDateComparator =  Convert.ToInt16(oLoanRequest.Params.Split('~')[3]);
                DateTime dtFrom =  Convert.ToDateTime(oLoanRequest.Params.Split('~')[4]);
                DateTime dtTO =  Convert.ToDateTime(oLoanRequest.Params.Split('~')[5]);


                string sSQL = "Select * from View_LoanRequest Where LoanRequestID<>0 ";

                if (sEmployeeIDs != "") 
                    sSQL = sSQL + " And EmployeeID In (" + sEmployeeIDs + ")";
                if (nLoanType>0)
                    sSQL = sSQL + " And LoanType=" + nLoanType;
                if (nLoanStatus > 0)
                    sSQL = sSQL + " And RequestStatus=" + nLoanStatus;

                if((int)EnumRequestStatus.Request==nLoanStatus && nDateComparator>0)
                {
                    sSQL += " And " + Global.DateSQLGenerator("RequestDate", nDateComparator, dtFrom, dtTO, false);
                }
                else if((int)EnumRequestStatus.Approve==nLoanStatus && nDateComparator>0)
                {
                    string query = "Select LoanRequestID from EmployeeLoanAmount Where ApproveBy>0 And EmployeeLoanID In ( "+
                                   "Select EmployeeLoanID from EmployeeLoan Where EmployeeLoanID<>0 "+ ((sEmployeeIDs!="")? " And EmployeeID In ("+sEmployeeIDs+")":""); 
                    query += " And "+ Global.DateSQLGenerator("ApproveDate", nDateComparator, dtFrom, dtTO, false);
                    query += " )";

                    sSQL += " And LoanRequestID In ("+ query +")";
                }
                _oLoanRequests = LoanRequest.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLoanRequests = new List<LoanRequest>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLoanRequests);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByCode(LoanRequest oLoanRequest)
        {
            _oLoanRequests = new List<LoanRequest>();
            try
            {
                string sLoanRequestNo = (string.IsNullOrEmpty(oLoanRequest.Params.Split('~')[0])) ? "" : oLoanRequest.Params.Split('~')[0].Trim();
                string sEmpNameCode = (string.IsNullOrEmpty(oLoanRequest.Params.Split('~')[1])) ? "" : oLoanRequest.Params.Split('~')[1].Trim();

                string sSQL = "Select * from View_LoanRequest Where LoanRequestID<>0 ";
                if (sLoanRequestNo != "") 
                    sSQL += " And RequestNo Like '%" + sLoanRequestNo + "%'";
                if (sEmpNameCode != "")
                    sSQL += " And EmployeeID In (Select EmployeeID from Employee Where Code Like '%"+sEmpNameCode+"%' Or Name Like '%"+sEmpNameCode+"%')";
               
                _oLoanRequests = LoanRequest.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLoanRequests = new List<LoanRequest>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLoanRequests);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmployeeInfo(LoanRequest oLoanRequest)
        {
            try
            {
                string sSQL = "";

                sSQL = "SELECT * FROM View_EmployeeOfficial Where EmployeeID=" + oLoanRequest.EmployeeID;
                var EmpOfficial = EmployeeOfficial.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
                sSQL = "SELECT * FROM View_EmployeeSalaryStructure Where EmployeeID=" + oLoanRequest.EmployeeID;
                var EmpSalary = EmployeeSalaryStructure.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oLoanRequest.OfficialInfo = (EmpOfficial.FirstOrDefault() != null) ? EmpOfficial.FirstOrDefault().DesignationName + " at " + EmpOfficial.FirstOrDefault().DepartmentName + " of " + EmpOfficial.FirstOrDefault().LocationName + ".  Joining Date: " + EmpOfficial.FirstOrDefault().DateOfJoinInString + ", Confirmation Date: " + EmpOfficial.FirstOrDefault().DateOfConfirmationInString + ", Year of Service: " + Global.GetDuration(EmpOfficial.FirstOrDefault().DateOfJoin, DateTime.Now) : "";
                oLoanRequest.SalaryInfo = (EmpSalary.FirstOrDefault() != null) ? EmpSalary.FirstOrDefault().PaymentCycleInString + " - " + Global.MillionFormat(EmpSalary.FirstOrDefault().GrossAmount) : "";
                oLoanRequest.PFInfo = this.GetPFInfo(oLoanRequest.EmployeeID);
                var oInfo = this.GetLastLoanInfo(oLoanRequest.EmployeeID, oLoanRequest.LoanRequestID, oLoanRequest.IsPFLoan);
                oLoanRequest.EmployeeLoanID = oInfo.Item1; //oLoanRequest.EmployeeLoanID;
                oLoanRequest.LastLoanInfo = oInfo.Item2;
                
                
            }
            catch (Exception ex)
            {
                oLoanRequest = new LoanRequest();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLoanRequest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsInstallment(LoanRequest oLoanRequest)
        {
            var oELIs = this.GenerateInstallment(oLoanRequest);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oELIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        public ActionResult PrintLoanRequest(int nId, double nts)
        {
            _oLoanRequest = new LoanRequest();

            List<EmployeeLoanAmount> oELAs = new List<EmployeeLoanAmount>();
            List<EmployeeLoanInstallment> oELIs = new List<EmployeeLoanInstallment>();
            List<EmployeeLoanRefund> oEmployeeLoanRefunds = new List<EmployeeLoanRefund>();
            if (nId > 0)
            {
                _oLoanRequest = LoanRequest.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oLoanRequest.LoanRequestID > 0)
                {

                    if (_oLoanRequest.RequestStatus == EnumRequestStatus.Approve)
                    {
                        string sSQL = "Select * from View_EmployeeLoanAmount Where LoanRequestID=" + _oLoanRequest.LoanRequestID;
                        oELAs = EmployeeLoanAmount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        return RedirectToAction("PrintEmployeeLoan", "EmployeeLoan", new { nId = oELAs.FirstOrDefault().EmployeeLoanID, nts = nts });

                    }
                    else
                    {
                        //_oLoanRequest.IsAdjustable = (_oLoanRequest.EmployeeLoanID > 0) ? true : false;
                        oELIs = this.GenerateInstallment(_oLoanRequest);

                        string sSQL = "SELECT * FROM View_EmployeeOfficial Where EmployeeID=" + _oLoanRequest.EmployeeID;
                        var EmpOfficial = EmployeeOfficial.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        sSQL = "SELECT * FROM View_EmployeeSalaryStructure Where EmployeeID=" + _oLoanRequest.EmployeeID;
                        var EmpSalary = EmployeeSalaryStructure.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


                        _oLoanRequest.OfficialInfo = (EmpOfficial.FirstOrDefault() != null) ? EmpOfficial.FirstOrDefault().DesignationName + " at " + EmpOfficial.FirstOrDefault().DepartmentName + " of " + EmpOfficial.FirstOrDefault().LocationName + ".  Joining Date: " + EmpOfficial.FirstOrDefault().DateOfJoinInString + ", Confirmation Date: " + EmpOfficial.FirstOrDefault().DateOfConfirmationInString + ", Year of Service: " + Global.GetDuration(EmpOfficial.FirstOrDefault().DateOfJoin, DateTime.Now) : "";
                        _oLoanRequest.SalaryInfo = (EmpSalary.FirstOrDefault() != null) ? EmpSalary.FirstOrDefault().PaymentCycleInString + " - " + Global.MillionFormat(EmpSalary.FirstOrDefault().GrossAmount) : "";
                        _oLoanRequest.PFInfo = this.GetPFInfo(_oLoanRequest.EmployeeID);

                        sSQL = "Select * from View_EmployeeLoanAmount Where ELAID<>0 ";
                        if (_oLoanRequest.EmployeeLoanID > 0)
                            sSQL += " And EmployeeLoanID=" + _oLoanRequest.EmployeeLoanID + "";
                        else
                            sSQL += " And LoanRequestID=" + _oLoanRequest.LoanRequestID + "";

                        oELAs = EmployeeLoanAmount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        if (!oELAs.Exists(x => x.LoanRequestID == _oLoanRequest.LoanRequestID))
                        {
                            var oELA = new EmployeeLoanAmount();
                            oELA.LoanRequestID = _oLoanRequest.LoanRequestID;
                            oELA.LoanDisburseDate = _oLoanRequest.ExpectDate;
                            oELA.Amount = _oLoanRequest.LoanAmount;
                            oELAs.Add(oELA);
                        }

                        if (_oLoanRequest.EmployeeLoanID > 0)
                        {
                            sSQL = "Select * from View_EmployeeLoanRefund Where ApproveBy>0 And EmployeeLoanID=" + _oLoanRequest.EmployeeLoanID + "";
                            oEmployeeLoanRefunds = EmployeeLoanRefund.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                 
                    }
                }

            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptLoanRequest oReport = new rptLoanRequest();
            byte[] abytes = oReport.PrepareReport(_oLoanRequest, new EmployeeLoan(), oELIs, oELAs, oEmployeeLoanRefunds, oCompany);
            return File(abytes, "application/pdf");

        }

   
        private List<EmployeeLoanInstallment> GenerateInstallment(LoanRequest oLoanRequest)
        {
            List<EmployeeLoanInstallment> oELIs = new List<EmployeeLoanInstallment>();
            try
            {
                EnumLoanType LoanType = oLoanRequest.LoanType;
                double nRemainingAmount = 0;
                double nPreviousInstallmentAmount = 0;
                var EmpLoanInsts = new List<EmployeeLoanInstallment>();
                var EmpLoanInitailAdjustables = new List<EmployeeLoanInstallment>();
                string sSQL = "";
                if (oLoanRequest.LoanRequestID > 0 && oLoanRequest.EmployeeLoanID>0)
                {
                    sSQL = "Select * from View_EmployeeLoanAmount Where EmployeeLoanID=" + oLoanRequest.EmployeeLoanID + " And LoanRequestID !=" + oLoanRequest.LoanRequestID + "";
                    var ELAs = EmployeeLoanAmount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    nPreviousInstallmentAmount = ELAs.Sum(x => x.Amount);

                    sSQL = "Select * from EmployeeLoanInstallment Where ESDetailID>0 And EmployeeLoanID In (" + oLoanRequest.EmployeeLoanID + ")";
                    EmpLoanInsts = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    //string sSQL = "Select * from EmployeeLoanInstallment Where EmployeeLoanID In (Select ISNULL(MAX(EmployeeLoanID),0) from EmployeeLoan Where EmployeeID = " + oLoanRequest.EmployeeID + " And EmployeeLoanID Not In (Select ISNULL(EmployeeLoanID,0) from EmployeeLoanAmount Where LoanRequestID=" + oLoanRequest.LoanRequestID + "))";
                    //EmpLoanInsts = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //nPreviousInstallmentAmount = EmpLoanInsts.Sum(x => x.InstallmentAmount);
                }
                else if (oLoanRequest.LoanRequestID > 0 && oLoanRequest.RequestStatus == EnumRequestStatus.Approve)
                {
                    sSQL = "Select * from EmployeeLoanInstallment Where EmployeeLoanID In (Select EmployeeLoanID from EmployeeLoanAmount Where LoanRequestID =" + oLoanRequest.LoanRequestID + ")";
                    EmpLoanInsts = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (oLoanRequest.LoanRequestID <= 0 && oLoanRequest.EmployeeLoanID > 0)
                {
                    sSQL = "Select * from EmployeeLoanInstallment Where EmployeeLoanID In (" + oLoanRequest.EmployeeLoanID + ")";
                    EmpLoanInsts = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    
                    EmpLoanInitailAdjustables = EmpLoanInsts.Where(x => x.ESDetailID <= 0 && x.InstallmentDate < oLoanRequest.ExpectDate).ToList();
                    nPreviousInstallmentAmount = EmpLoanInsts.Where(x=>x.ESDetailID>0).Sum(x => x.InstallmentAmount);
                }


                int nSalaryDate = 0;
                DateTime dtInstallment = DateTime.MinValue;
                DateTime dtDisburseDate = DateTime.MinValue;
                sSQL = "SELECT * FROM View_EmployeeSalaryStructure Where EmployeeID=" + oLoanRequest.EmployeeID;
                var EmpSalarys = EmployeeSalaryStructure.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                List<EmployeeSalary> oEmployeeSalarys=new List<EmployeeSalary>();
                sSQL = "SELECT top(1)* FROM View_EmployeeSalary Where EmployeeID=" + oLoanRequest.EmployeeID + " Order By EmployeeSalaryID DESC";
                oEmployeeSalarys = EmployeeSalary.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
                if ((oLoanRequest.RequestStatus == EnumRequestStatus.None || oLoanRequest.RequestStatus == EnumRequestStatus.Request) && oEmployeeSalarys.Any() && oEmployeeSalarys.First().EmployeeSalaryID > 0)
                {
                    var oEmployeeSalary = oEmployeeSalarys.First();
                    if (oEmployeeSalary.EndDate > oLoanRequest.ExpectDate)
                        throw new Exception("Loan disburse date must be later than " + oEmployeeSalary.EndDate.ToString("dd MMM yyyy")+". Already salary is processed on this date.");
                }
                

                if (EmpSalarys.Count() <= 0 || EmpSalarys == null)
                    throw new Exception("Employee salary date yet not defined.");
                else
                {
                    nSalaryDate = EmpSalarys.FirstOrDefault().StartDay;
                }
                if (oLoanRequest.EmployeeLoanID > 0)
                {
                    sSQL = "Select top(1)* from View_EmployeeLoanAmount Where EmployeeLoanID=" + oLoanRequest.EmployeeLoanID + " Order By ELAID";
                    var ELAs = EmployeeLoanAmount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    dtDisburseDate = ELAs.FirstOrDefault().LoanDisburseDate;
                }

                if (oLoanRequest.LoanRequestID <= 0 && oLoanRequest.EmployeeLoanID > 0)
                {
                    if (dtDisburseDate >= oLoanRequest.ExpectDate)
                        throw new Exception("Loan disburse date must be later than " + dtDisburseDate.ToString("dd MMM yyyy"));
                    else if (EmpLoanInsts.Where(x => x.ESDetailID > 0).Any() && EmpLoanInsts.Where(x => x.ESDetailID > 0).LastOrDefault().InstallmentDate >= oLoanRequest.ExpectDate)
                        throw new Exception("Loan disburse date must be later than " + EmpLoanInsts.Where(x => x.ESDetailID > 0).LastOrDefault().InstallmentDate.ToString("dd MMM yyyy"));

                    var oInstallment = EmpLoanInsts.Where(x => x.ESDetailID <= 0);
                    var oInstallmentSettled = EmpLoanInsts.Where(x => x.ESDetailID > 0);

                    string sLastSettledDate = (oInstallmentSettled != null && oInstallmentSettled.Any()) ? oInstallmentSettled.LastOrDefault().InstallmentDate.AddDays(1).ToString("dd MMM yyyy") : "";
                    string sNextSelltedDate = (oInstallment != null && oInstallment.Any()) ? oInstallment.FirstOrDefault().InstallmentDateStr : "";

                    if (oInstallment != null && oInstallment.Any() && oInstallment.FirstOrDefault().InstallmentDate < oLoanRequest.ExpectDate)
                        throw new Exception("Adjustment doesn't possible." + ((sLastSettledDate != "" && sNextSelltedDate != "") ? "Date Range Must Between " + sLastSettledDate + " to " + sNextSelltedDate + "" : "Date Will be " + sNextSelltedDate + ""));
                    else if (oInstallment == null || !oInstallment.Any())
                        throw new Exception("Unable to adjustment. No remaining installment found");
                }
                //Loan Is  When oLoanRequest.EmployeeLoanID>0

                //Sum of The Total Loan as Show In Grid Top
                EmployeeLoanInstallment oELI = new EmployeeLoanInstallment();
                oELI.Type = 0;
                oELI.InstallmentDate = (oLoanRequest.EmployeeLoanID > 0) ? dtDisburseDate : oLoanRequest.ExpectDate;
                oELI.InstallmentAmount = oLoanRequest.LoanAmount + nPreviousInstallmentAmount;
                oELI.InterestPerInstallment = 0;
                oELI.TotalAmount = 0;
                oELI.Balance = oLoanRequest.LoanAmount + nPreviousInstallmentAmount;
                oELIs.Add(oELI);


                if (oLoanRequest.RequestStatus != EnumRequestStatus.Approve)
                {
                    //Add Loan Installment In the Grid Which Is Settled 
                    foreach (EmployeeLoanInstallment oItem in EmpLoanInsts.Where(x => x.ESDetailID > 0).ToList())
                    {
                        oItem.Type = 1;
                        oItem.TotalAmount = oItem.InstallmentAmount + oItem.InterestPerInstallment;
                        oItem.Balance = oLoanRequest.LoanAmount + nPreviousInstallmentAmount - (oELIs.Where(x => x.Type != 0).Sum(x => x.InstallmentAmount) + oItem.InstallmentAmount);
                        oELIs.Add(oItem);
                    }

                    //Not AddJustable
                    //Add Loan Installment In the Grid Which Is Not Settled And Found Before Loan Expect Date 
                    if (!oLoanRequest.IsAdjustable)
                    {
                        foreach (EmployeeLoanInstallment oItem in EmpLoanInitailAdjustables)
                        {
                            oItem.Type = 1;
                            oItem.TotalAmount = oItem.InstallmentAmount + oItem.InterestPerInstallment;
                            oItem.Balance = oLoanRequest.LoanAmount + nPreviousInstallmentAmount - (oELIs.Where(x => x.Type != 0).Sum(x => x.InstallmentAmount) + oItem.InstallmentAmount);
                            oELIs.Add(oItem);
                        }
                    }


                    #region Find New Installment Date

                    if (oLoanRequest.IsAdjustable)
                    {
                        int dateStart = (nSalaryDate > 1) ? nSalaryDate - 1 : nSalaryDate;
                        if (oELIs.LastOrDefault().InstallmentDate < oLoanRequest.ExpectDate)
                        {
                            dtInstallment = new DateTime(oLoanRequest.ExpectDate.Year, oLoanRequest.ExpectDate.Month, dateStart);
                            dtInstallment = (dtInstallment < oLoanRequest.ExpectDate) ? dtInstallment.AddMonths(1) : dtInstallment;
                        }
                        else
                        {
                            dtInstallment = new DateTime(oELIs.LastOrDefault().InstallmentDate.Year, oELIs.LastOrDefault().InstallmentDate.Month, dateStart);
                            dtInstallment = (dtInstallment <= oELIs.LastOrDefault().InstallmentDate) ? dtInstallment.AddMonths(1) : dtInstallment;
                        }
                    }
                    else if (oELIs.LastOrDefault().InstallmentDate.Day < nSalaryDate)
                    {
                        int dateStart = (nSalaryDate > 1) ? nSalaryDate - 1 : nSalaryDate;

                        dtInstallment = new DateTime(oELIs.LastOrDefault().InstallmentDate.Year, oELIs.LastOrDefault().InstallmentDate.Month, dateStart);
                        if (oELIs.LastOrDefault().ESDetailID > 0 && dtInstallment == oELIs.LastOrDefault().InstallmentDate)
                            dtInstallment = dtInstallment.AddMonths(1);

                    }
                    else if (oELIs.LastOrDefault().InstallmentDate.Day >= nSalaryDate)
                    {
                        if (oELIs.LastOrDefault().InstallmentDate.Month == 12)
                        {
                            dtInstallment = new DateTime(oELIs.LastOrDefault().InstallmentDate.Year + 1, 1, nSalaryDate);
                        }
                        else
                        {
                            dtInstallment = new DateTime(oELIs.LastOrDefault().InstallmentDate.Year, oELIs.LastOrDefault().InstallmentDate.Month + 1, nSalaryDate);
                        }

                        dtInstallment = (dtInstallment.AddDays(-1) == oELIs.LastOrDefault().InstallmentDate) ? dtInstallment : dtInstallment.AddDays(-1);
                    }
                    #endregion

                    double nAdjustLoanAmount = 0;
                    double.TryParse((oLoanRequest.LoanRequestID > 0) ? oLoanRequest.LoanAmount.ToString() : oLoanRequest.Params, out nAdjustLoanAmount);

                    #region
                    //while (dtInstallment.Month < oLoanRequest.ExpectDate.Month && dtInstallment.Year <= oLoanRequest.ExpectDate.Year)
                    //{
                    //    oELIs.Where(x => x.ESDetailID > 0).ToList().ForEach(p => p.Balance = p.Balance - nAdjustLoanAmount);

                    //    nRemainingAmount = oLoanRequest.LoanAmount + nPreviousInstallmentAmount - nAdjustLoanAmount - oELIs.Where(x => x.Type != 0).Sum(x => x.InstallmentAmount);
                    //    oELI = new EmployeeLoanInstallment();
                    //    oELI.Type = 1;
                    //    oELI.InstallmentDate = dtInstallment;
                    //    oELI.InstallmentAmount = (nRemainingAmount >= oLoanRequest.InstallmentAmount) ? oLoanRequest.InstallmentAmount + ((nRemainingAmount - oLoanRequest.InstallmentAmount > 100) ? 0 : nRemainingAmount - oLoanRequest.InstallmentAmount) : nRemainingAmount;
                    //    oELI.InterestPerInstallment = Math.Round(((nRemainingAmount * oLoanRequest.InterestRate) / 100) * ((oELI.InstallmentDate - oELIs.LastOrDefault().InstallmentDate).TotalDays + (((oELIs.Count() < 2) ? 1 : 0))) / 365, 0);
                    //    oELI.TotalAmount = oELI.InstallmentAmount + oELI.InterestPerInstallment;
                    //    oELI.Balance = oLoanRequest.LoanAmount + nPreviousInstallmentAmount - nAdjustLoanAmount - (oELIs.Where(x => x.Type != 0).Sum(x => x.InstallmentAmount) + oELI.InstallmentAmount);
                    //    oELIs.Add(oELI);
                    //    dtInstallment = dtInstallment.AddMonths(1);
                    //    oLoanRequest.NoOfInstallment--;
                    //}
                    #endregion

                    double nAdjustmentInterest = 0;

                    #region When It's Adjustable
                    if (oLoanRequest.EmployeeLoanID > 0)
                    {
                        if (oELIs.Where(x => x.Type == 1).Any())
                        {
                            var nDays = (dtInstallment - oELIs.Where(x => x.Type == 1).LastOrDefault().InstallmentDate).TotalDays;
                            nDays = (nDays > 0) ? nDays - 1 : 0;
                            if (oLoanRequest.LoanRequestID > 0)
                            {
                                double nAmount = oELIs.Where(x => x.Type == 1 && x.ESDetailID > 0).Sum(x => x.InstallmentAmount);
                                nAdjustmentInterest = InterestCalculation((nPreviousInstallmentAmount - nAmount), oLoanRequest.InterestRate, nDays, true);
                            }
                            else
                            {
                                nAdjustmentInterest = InterestCalculation((oLoanRequest.LoanAmount - nAdjustLoanAmount), oLoanRequest.InterestRate, nDays, true);
                            }
                        }
                        else
                        {
                            if(nAdjustLoanAmount>0){
                                 var objELI = oELIs.Where(x => x.Type == 0).First();
                                 var nDays = (dtInstallment - objELI.InstallmentDate).TotalDays;
                                 nDays = (nDays > 0) ? nDays - 1 : 0;
                                 nAdjustmentInterest = InterestCalculation((objELI.InstallmentAmount - nAdjustLoanAmount), oLoanRequest.InterestRate, nDays, true);
                            }
                        }
                      
                    }
                    #endregion

                    #region Installment Generate
                    for (int i = 1; i <= oLoanRequest.NoOfInstallment; i++)
                    {
                        nRemainingAmount = oLoanRequest.LoanAmount + nPreviousInstallmentAmount - oELIs.Where(x => x.Type != 0).Sum(x => x.InstallmentAmount); //+ ((oLoanRequest.EmployeeLoanID>0 && i==1)? (-1)*nAdjustLoanAmount : 0);
                        oELI = new EmployeeLoanInstallment();
                        oELI.Type = 1;
                        oELI.InstallmentDate = (i == 1) ? dtInstallment : oELIs.LastOrDefault().InstallmentDate.AddMonths(1);
                        oELI.InstallmentAmount = (nRemainingAmount >= oLoanRequest.InstallmentAmount) ? oLoanRequest.InstallmentAmount + ((nRemainingAmount - oLoanRequest.InstallmentAmount > 100) ? 0 : nRemainingAmount - oLoanRequest.InstallmentAmount) : nRemainingAmount;
                        var dtPrevious = (i == 1 && oLoanRequest.IsAdjustable) ? oLoanRequest.ExpectDate : oELIs.LastOrDefault().InstallmentDate;
                        var bIsIssueDate = (oELIs.Count() < 2 || (i == 1 && oLoanRequest.IsAdjustable)) ? true : false; // For Add Loan Issue Day 
  
                        // Math.Round(((nRemainingAmount * oLoanRequest.InterestRate) / 100) * ((oELI.InstallmentDate - dtPrevious).TotalDays + ((bIsIssueDate) ? 1 : 0)) / 365, 0)
                        oELI.InterestPerInstallment = ((i == 1) ? nAdjustmentInterest : 0) + InterestCalculation(((i == 1 && oLoanRequest.IsAdjustable)? nAdjustLoanAmount :  nRemainingAmount), oLoanRequest.InterestRate, (oELI.InstallmentDate - dtPrevious).TotalDays, bIsIssueDate);
                        oELI.TotalAmount = oELI.InstallmentAmount + oELI.InterestPerInstallment;
                        oELI.Balance = oLoanRequest.LoanAmount + nPreviousInstallmentAmount - (oELIs.Where(x => x.Type != 0).Sum(x => x.InstallmentAmount) + oELI.InstallmentAmount);
                        oELIs.Add(oELI);
                    }
                    #endregion
                }
                else
                {

                    sSQL = "Select * from EmployeeLoanInstallment Where ESDetailID<=0 And EmployeeLoanID In (" + oLoanRequest.EmployeeLoanID + ")";
                    var oELInstallments = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oELInstallments.Any() && oELInstallments.FirstOrDefault().ELInstallmentID>0)
                        oELInstallments.ForEach(x => { EmpLoanInsts.Add(x); });

                    EmpLoanInsts.ForEach(x =>
                    {
                        x.Type = 1;
                        x.TotalAmount = x.InstallmentAmount + x.InterestPerInstallment;
                        x.Balance = oELIs.FirstOrDefault().Balance - (oELIs.Where(p => p.Type != 0 && p.Balance > 0).Sum(p => p.InstallmentAmount) + x.InstallmentAmount);
                        oELIs.Add(x);
                    });
                }
            }
            catch (Exception ex)
            {
                EmployeeLoanInstallment oELI = new EmployeeLoanInstallment();
                oELI.Type = -1;
                oELI.ErrorMessage = ex.Message;
                oELIs = new List<EmployeeLoanInstallment>();
                oELIs.Add(oELI);
            }
            return oELIs;
        }


        private double InterestCalculation(double nAmount, double nRate, double nDays, bool bIsIssueDate)
        {
            return Math.Round(((nAmount * nRate) / 100) * (nDays + ((bIsIssueDate) ? 1 : 0)) / 365, 0); ;
        }

        private Tuple<int,string> GetLastLoanInfo(int nEmpId, int nLoanRequestId, bool IsPFLoan)
        {

            string sLastLoanInfo = "N/A";
            int nEmployeeLoanID = 0;

            string sSQL = "";
            if (nLoanRequestId > 0)
            {
                sSQL = "Select * from View_EmployeeLoanAmount Where LoanRequestID = " + nLoanRequestId;
                var ELAs = EmployeeLoanAmount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                nEmployeeLoanID = (ELAs.Count() > 0 && ELAs.FirstOrDefault().ELAID > 0) ? ELAs.FirstOrDefault().EmployeeLoanID : 0;
            }

            sSQL = "Select * from View_EmployeeLoan Where IsPFLoan =" + ((IsPFLoan) ? 1 : 0) + " And EmployeeLoanID = (Select ISNULL(MAX(EmployeeLoanID),0) from EmployeeLoan Where EmployeeID = " + nEmpId + " " + ((nEmployeeLoanID > 0) ? " And EmployeeLoanID != " + nEmployeeLoanID : "") + " )";
            var EmpLoans = EmployeeLoan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var EmpLoan = ((EmpLoans != null && EmpLoans.Count() > 0) ? EmpLoans.FirstOrDefault() : new EmployeeLoan());

            if(EmpLoan.EmployeeLoanID>0){
                sSQL = "Select * from View_EmployeeLoanAmount Where EmployeeLoanID In(" + EmpLoan.EmployeeLoanID + ")";
                var EmpLoanAmounts = EmployeeLoanAmount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "Select * from EmployeeLoanInstallment Where EmployeeLoanID=" + EmpLoan.EmployeeLoanID;
                var EmpLoanInsts = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                double nInterestRate = EmpLoan.InterestRate;
                DateTime dtNewEncash = (EmpLoanInsts.Where(x => x.ESDetailID > 0).Count()>0) ? EmpLoanInsts.Where(x => x.ESDetailID > 0).LastOrDefault().InstallmentDate.AddMonths(1) : (EmpLoanInsts.Count() > 0) ? EmpLoanInsts.FirstOrDefault().InstallmentDate.AddMonths(-1) : DateTime.Now;


                sLastLoanInfo = "Disburse Date: " + EmpLoanAmounts.FirstOrDefault().LoanDisburseDateStr +
                    "; Amount:" + Global.MillionFormat(EmpLoanAmounts.Sum(x => x.Amount)) +
                    "; No of Inst.: " + EmpLoanInsts.Where(x => x.ELInstallmentID > 0).Count() +
                    "; Encash Inst.: " + EmpLoanInsts.Where(x => x.ELInstallmentID > 0 && x.ESDetailID > 0).Count() +
                    "; Pending Inst.: " + EmpLoanInsts.Where(x => x.ELInstallmentID > 0 && x.ESDetailID == 0).Count() +
                    "; Interest Rate: " + Global.MillionFormat(EmpLoan.InterestRate) +
                    "; Last Inst.: " + Global.MillionFormat((EmpLoanInsts.Where(x => x.ELInstallmentID > 0 && x.ESDetailID > 0).Count() > 0) ? EmpLoanInsts.Where(x => x.ELInstallmentID > 0 && x.ESDetailID > 0).LastOrDefault().InstallmentAmount : 0) +
                    "; Balance: " + Global.MillionFormat(EmpLoanInsts.Where(x => x.ELInstallmentID > 0 && x.ESDetailID == 0).Sum(x => x.InstallmentAmount)) +
                    "~" + (Int16)EmpLoan.LoanType + "~" + nInterestRate + "~" + dtNewEncash.ToString("dd MMM yyyy") + "~" + ((EmpLoan.IsPFLoan) ? 1 : 0);

                nEmployeeLoanID = EmpLoan.EmployeeLoanID;
            }
            return new Tuple<int, string>(nEmployeeLoanID, sLastLoanInfo);
        }

        private string GetPFInfo(int nEmployeeId)
        {
            string sPFInfo = "N/A";

            try
            {
                string sSQL = "SELECT * FROM View_PFmember Where EmployeeID=" + nEmployeeId;
                var PFMems = PFmember.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                var PFMem = (PFMems.FirstOrDefault() != null) ? PFMems.FirstOrDefault() : new PFmember();

                if (PFMem.PFMID > 0)
                {
                    sPFInfo = "PF Balance: " + Global.MillionFormat(PFMem.PFBalance);

                    sSQL = "Select * from EmployeeLoanSetup Where ApproveBy<>0 And InactiveBy=0";
                    var EmpLoanSetups = EmployeeLoanSetup.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    var EmpLoanSetup = (EmpLoanSetups.FirstOrDefault() != null) ? EmpLoanSetups.FirstOrDefault() : new EmployeeLoanSetup();


                    sSQL = "Select * from EmployeeLoanAmount Where EmployeeLoanID In (Select EmployeeLoanID from EmployeeLoan Where EmployeeID=" + nEmployeeId + " And IsPFLoan=1)";
                    var ELAs = EmployeeLoanAmount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sSQL = "Select * from EmployeeLoanInstallment Where ESDetailID>0 And EmployeeLoanID In (Select EmployeeLoanID from EmployeeLoan Where EmployeeID=" + nEmployeeId + " And IsPFLoan=1)";
                    var ELIs = EmployeeLoanInstallment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sSQL = "Select * from View_LoanRequest WHere EmployeeID="+ nEmployeeId +" And IsPFLoan=1 And RequestStatus=1";
                    var LoanRequests = LoanRequest.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


                    double nLoanAmount = 0;
                    if (EmpLoanSetup.ELSID > 0)
                    {
                        nLoanAmount = (PFMem.PFBalance * EmpLoanSetup.LimitInPercentOfPF) / 100;
                    }
                    else
                    {
                        nLoanAmount = PFMem.PFBalance;
                    }
                    nLoanAmount = nLoanAmount - (ELAs.Sum(x => x.Amount) - ELIs.Sum(x => x.InstallmentAmount)) - LoanRequests.Sum(x=>x.LoanAmount);
                    nLoanAmount = (nLoanAmount < 0) ? 0 : nLoanAmount;
                    sPFInfo += "; Max. Loan Amount: " + Global.MillionFormat(nLoanAmount);

                }
            }
            catch(Exception e)
            {
                sPFInfo = "N/A";
            }

            return sPFInfo;
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


        #region Loan Import
        public ActionResult ViewLoanImport(double nts)
        {
            List<LoanRequest> _oLoanRequests = new List<LoanRequest>();
            ViewBag.Message = "";
            return View(_oLoanRequests);
        }

        private List<LoanRequest> GetLoanRequestFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<LoanRequest> oLoanRequests = new List<LoanRequest>();
            LoanRequest oLoanRequest = new LoanRequest();
            if (PostedFile.ContentLength > 0)
            {
                fileExtension = System.IO.Path.GetExtension(PostedFile.FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    fileDirectory = Server.MapPath("~/Content/") + PostedFile.FileName;
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                    PostedFile.SaveAs(fileDirectory);
                    string excelConnectionString = string.Empty;
                    //connection String for xls file format.
                    //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                    ////excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }
                    excelConnection.Close();
                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    oRows = ds.Tables[0].Rows;
                    EnumLoanType LoanType=EnumLoanType.None;
                    for (int i = 0; i < oRows.Count; i++)
                    {
                        LoanType=EnumLoanType.None;
                        oLoanRequest = new LoanRequest();
                        oLoanRequest.EmployeeCode = Convert.ToString(oRows[i]["Code"] == DBNull.Value ? "" : oRows[i]["Code"]);
                        var oEmployee = Employee.Get("Select * from View_Employee Where Code='" + oLoanRequest.EmployeeCode + "'", ((User)Session[SessionInfo.CurrentUser]).UserID);
                        oLoanRequest.EmployeeID = (oEmployee!=null || oEmployee.EmployeeID>0)? oEmployee.EmployeeID : 0;
                        //oLoanRequest.RequestDate = Convert.ToDateTime(oRows[i]["RequestDate"] == DBNull.Value ? DateTime.Today : oRows[i]["RequestDate"]);

                        Enum.TryParse(Convert.ToString((oRows[i]["LoanType"] == DBNull.Value) ? "None" : oRows[i]["LoanType"]), out LoanType);
                        oLoanRequest.LoanType = LoanType;

                        oLoanRequest.ExpectDate = Convert.ToDateTime(oRows[i]["ExpectDate"] == DBNull.Value ? DateTime.Today : oRows[i]["ExpectDate"]);
                        oLoanRequest.Purpose = Convert.ToString(oRows[i]["Purpose"] == DBNull.Value ? "" : oRows[i]["Purpose"]);
                        oLoanRequest.LoanAmount = Convert.ToDouble(oRows[i]["LoanAmount"] == DBNull.Value ? 0 : oRows[i]["LoanAmount"]);
                        oLoanRequest.NoOfInstallment = Convert.ToInt16(oRows[i]["NoOfInstallment"] == DBNull.Value ? 0 : oRows[i]["NoOfInstallment"]);
                        oLoanRequest.InstallmentAmount = Convert.ToDouble(oRows[i]["InstallmentAmount"] == DBNull.Value ? 0 : oRows[i]["InstallmentAmount"]);
                        oLoanRequest.InterestRate = Convert.ToDouble(oRows[i]["InterestRate"] == DBNull.Value ? 0 : oRows[i]["InterestRate"]);
                        oLoanRequest.Remarks = Convert.ToString(oRows[i]["Remarks"] == DBNull.Value ? "" : oRows[i]["Remarks"]);
                        oLoanRequests.Add(oLoanRequest);
                    }
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                }
                else
                {
                    throw new Exception("File not supported");
                }
            }
            return oLoanRequests;
        }

        [HttpPost]
        public ActionResult ViewLoanImport(HttpPostedFileBase fileLoanInfo)
        {
            LoanRequest oLoanRequest = new LoanRequest();
            List<LoanRequest> oLoanRequests = new List<LoanRequest>();
            List<LoanRequest> oLRErrors = new List<LoanRequest>();
            int nItem = 0;
            try
            {
                if (fileLoanInfo == null) { throw new Exception("File not Found"); }
                var oLRs = this.GetLoanRequestFromExcel(fileLoanInfo);
                nItem = oLRs.Count();
                oLRErrors = oLRs.Where(x => x.EmployeeID <= 0 || x.LoanType == EnumLoanType.None || x.LoanAmount == 0 || x.NoOfInstallment == 0 || x.InstallmentAmount == 0 || x.InterestRate==0 ).ToList();
                oLRs.RemoveAll(x => x.EmployeeID <= 0 || x.LoanType == EnumLoanType.None || x.LoanAmount == 0 || x.NoOfInstallment == 0 || x.InstallmentAmount == 0 || x.InterestRate == 0);

                foreach (LoanRequest oItem in oLRs)
                {
                    oLoanRequest = new LoanRequest();
                    oLoanRequest = oItem.IUD((short)EnumDBOperation.Insert,((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oLoanRequest.LoanRequestID > 0)
                    {
                        var oELIs = this.GenerateInstallment(oLoanRequest);
                        oLoanRequest = oLoanRequest.Approval(oELIs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        if (oLoanRequest.LoanRequestID > 0 && oLoanRequest.RequestStatus == EnumRequestStatus.Approve)
                        {
                            oLoanRequests.Add(oLoanRequest);
                        }
                        else
                        {
                            var sMessage=oLoanRequest.IUD((short)EnumDBOperation.Delete,((User)Session[SessionInfo.CurrentUser]).UserID).ErrorMessage;
                            if(sMessage==Global.DeleteMessage)
                            {
                                oLoanRequest.LoanRequestID=0;
                                oLoanRequest.EmployeeID=0;
                                oLoanRequest.ProceedBy=0;
                                oLRErrors.Add(oLoanRequest);
                            }
                        }
                    }
                    else
                    {
                        oLRErrors.Add(oItem);
                    }
                }

                if (oLoanRequests.Count() > 0)
                {
                    if (nItem > oLoanRequests.Count())
                        throw new Exception("Number of " + oLoanRequests.Count() + " records are saved out of "+ nItem +" records.\n Please check the below list which are not saved." );
                    else
                        throw new Exception("Porcessed Successfully.");
                }
                else
                {
                    throw new Exception("Unable to save.");
                }
            }
            catch (Exception ex)
            {
                oLoanRequest = new LoanRequest();
                oLoanRequest.ErrorMessage = ex.Message;
            }
            ViewBag.Message = (oLoanRequest.ErrorMessage != null || oLoanRequest.ErrorMessage!="")? oLoanRequest.ErrorMessage : "";
            return View(oLRErrors);
        }


        public ActionResult XLLoanFormat(double nts)
        {
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<LoanFormat>));
            List<LoanFormat> oLoanFormats = new List<LoanFormat>();
            LoanFormat oLoanFormat = new LoanFormat();
            oLoanFormats.Add(oLoanFormat);
            serializer.Serialize(stream, oLoanFormats);
            stream.Position = 0;
            return File(stream, "application/vnd.ms-excel", "XLLoanFormat.xls");
        }
        #endregion
    }

    public  class LoanFormat
    {
        public LoanFormat()
        {
            Code = "Code";
            Name = "Name";
            RequestDate = "RequestDate";
            LoanType = "LoanType";
            ExpectDate = "ExpectDate";
            Purpose = "Purpose";
            LoanAmount = "LoanAmount";
            NoOfInstallment = "NoOfInstallment";
            InstallmentAmount = "InstallmentAmount";
            InterestRate = "InterestRate";
            Remarks = "Remarks";
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public string RequestDate { get; set; }
        public string LoanType { get; set; }
        public string ExpectDate { get; set; }
        public string Purpose { get; set; }
        public string LoanAmount { get; set; }
        public string NoOfInstallment { get; set; }
        public string InstallmentAmount { get; set; }
        public string InterestRate { get; set; }
        public string Remarks { get; set; }
    }
}