using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Reports;


namespace ESimSolFinancial.Controllers
{
    public class ChequeBookController : Controller
    {
        #region Declaration
        ChequeBook _oChequeBook = new ChequeBook();
        List<ChequeBook> _oChequeBooks = new List<ChequeBook>();
        private string _sSQL = "";
        string _sErrorMessage = "";
        bool _bForPrint = false;
        #endregion
        #region Functions
        private bool ValidateInput(ChequeBook oChequeBook)
        {
            if (oChequeBook.BankAccountID == null || oChequeBook.BankAccountID == 0)
            {
                _sErrorMessage = "Please Bank Account";
                return false;
            }

            if (oChequeBook.FirstChequeNo == null || oChequeBook.FirstChequeNo == "")
            {
                _sErrorMessage = "Please enter First Cheque Number";
                return false;
            }
            if (oChequeBook.PageCount == null || oChequeBook.PageCount == 0)
            {
                _sErrorMessage = "Please enter Leaf count";
                return false;
            }
            if (oChequeBook.Cheques == null || oChequeBook.Cheques.Count <= 0)
            {
                _sErrorMessage = "Please Confirm Cheque Preview";
                return false;
            }
            if (oChequeBook.PageCount != oChequeBook.Cheques.Count)
            {
                _sErrorMessage = "Please Comfirm leaf count again";
                return false;
            }
            if (Convert.ToInt64(oChequeBook.FirstChequeNo) != Convert.ToInt64(oChequeBook.Cheques[0].ChequeNo))
            {
                _sErrorMessage = "Please Cheque first Cheque Number";
                return false;
            }
            
            
            return true;
        }
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM View_ChequeBook";
            string sAccountNo = _bForPrint ? "" : (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[0];
            string sBankName = _bForPrint ? "" : (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1];
            string sCompanyName = _bForPrint ? "" : (Arguments.Split(';')[1].Split('~')[2] == null) ? "" : Arguments.Split(';')[1].Split('~')[2];
            string sBookCode = _bForPrint ? "" : (Arguments.Split(';')[1].Split('~')[3] == null) ? "" : Arguments.Split(';')[1].Split('~')[3];
            string sChequeBookIDs = _bForPrint ? (Arguments.Split(';')[1].Split('~')[4] == null) ? "" : Arguments.Split(';')[1].Split('~')[4] : "";

            string sSQL = "";


            #region AccountNo
            if (sAccountNo != null)
            {
                if (sAccountNo != "")
                {
                    if (sAccountNo != "Search By Account No")
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " AccountNo LIKE '%" + sAccountNo + "%' ";
                    }
                }
            }
            #endregion
            #region BankName
            if (sBankName != null)
            {
                if (sBankName != "")
                {
                    if (sBankName != "Search By Bank")
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " BankName LIKE '%" + sBankName + "%' ";
                    }
                }
            }
            #endregion
            #region CompanyName
            if (sCompanyName != null)
            {
                if (sCompanyName != "")
                {
                    if (sCompanyName != "Search By Company")
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " CompanyName LIKE '%" + sCompanyName + "%' ";
                    }
                }
            }
            #endregion
            #region BookCode
            if (sBookCode != null)
            {
                if (sBookCode != "")
                {
                    if (sBookCode != "Search By Book No")
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " (BookCodePartOne+'-'+BookCodePartTwo) LIKE '%" + sBookCode + "%' ";
                    }
                }
            }
            #endregion
            #region ChequeBookIDs
            if (sChequeBookIDs != null)
            {
                if (sChequeBookIDs != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " ChequeBookID IN (" + sChequeBookIDs + ") ";
                }
            }
            #endregion
            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }
            _sSQL = _sSQL + " ORDER BY ChequeBookID DESC, BankName, AccountNo, BookCodePartTwo ASC";
        }
        #endregion

        #region Actions

        public ActionResult ViewChequeBooks(int menuid)
        {

            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ChequeBook).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oChequeBooks = new List<ChequeBook>();
            _oChequeBooks = ChequeBook.Gets(true,(int)Session[SessionInfo.currentUserID]);
            return View(_oChequeBooks);
        }
        public ActionResult ViewChequeBookMgt(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ChequeBook).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oChequeBooks = new List<ChequeBook>();
            _oChequeBooks = ChequeBook.Gets(true, (int)Session[SessionInfo.currentUserID]);
            return View(_oChequeBooks);
        }

        public ActionResult ViewChequeBook(int nid, string sMsg) // ChequeBookID
        {
            _oChequeBook = new ChequeBook();
            if (nid > 0)
            {
                _oChequeBook = _oChequeBook.Get(nid, (int)Session[SessionInfo.currentUserID]);
            }
            else { _oChequeBook.IsActive = true; }
            if (sMsg != "N/A")
            {
                _oChequeBook.ErrorMessage = sMsg;
            }
            _oChequeBook.Cheques = Cheque.Gets(_oChequeBook.ChequeBookID,(int)Session[SessionInfo.currentUserID]);
            return View(_oChequeBook);
        }

        [HttpPost]
        public JsonResult Refresh(ChequeBook oChequeBook)
        {
            this.MakeSQL(oChequeBook.ErrorMessage);
            _oChequeBooks = new List<ChequeBook>();
            _oChequeBooks = ChequeBook.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChequeBooks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(ChequeBook oChequeBook)
        {
            _oChequeBook = new ChequeBook();
            try
            {
                oChequeBook.BookCodePartOne = oChequeBook.BookCodePartOne == null ? "" : oChequeBook.BookCodePartOne;
                oChequeBook.BookCodePartTwo = oChequeBook.BookCodePartTwo == null ? "" : oChequeBook.BookCodePartTwo;
                oChequeBook.FirstChequeNo = oChequeBook.FirstChequeNo == null ? "" : oChequeBook.FirstChequeNo;
                oChequeBook.Note = oChequeBook.Note == null ? "" : oChequeBook.Note;
                if (!this.ValidateInput(oChequeBook))
                {
                    throw new Exception(_sErrorMessage);
                }
                _oChequeBook = oChequeBook;
                _oChequeBook = _oChequeBook.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oChequeBook = new ChequeBook();
                _oChequeBook.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChequeBook);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChequeBookActiveInActive(ChequeBook oChequeBook)
        {
            _oChequeBook = new ChequeBook();
            try
            {
                _oChequeBook = oChequeBook;
                _oChequeBook = _oChequeBook.ChequeBookActiveInActive((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oChequeBook = new ChequeBook();
                _oChequeBook.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChequeBook);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(ChequeBook oChequeBook)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oChequeBook.Delete(oChequeBook.ChequeBookID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintChequeBooks(string arguments)
        {
            _bForPrint = true;
            this.MakeSQL(arguments);
            _oChequeBooks = new List<ChequeBook>();
            _oChequeBooks = ChequeBook.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);



            string Messge = "ChequeBook List";
            rptChequeBooks oReport = new rptChequeBooks();
            byte[] abytes = oReport.PrepareReport(_oChequeBooks, oCompany, Messge);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintChequeBooksInXL(string arguments)
        {
            _bForPrint = true;
            this.MakeSQL(arguments);
            //_productsServices = new ProductsServices();
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<ChequeBookXL>));

            //We load the data
            List<ChequeBook> oChequeBooks = ChequeBook.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            int nCount = 0; double nTotal = 0;
            ChequeBookXL oChequeBookXL = new ChequeBookXL();
            List<ChequeBookXL> oChequeBookXLs = new List<ChequeBookXL>();
            foreach (ChequeBook oItem in oChequeBooks)
            {
                nCount++;
                oChequeBookXL = new ChequeBookXL();
                oChequeBookXL.SLNo = nCount.ToString();
                oChequeBookXL.AccountNo = oItem.AccountNo;
                oChequeBookXL.BookCode = oItem.BookCode;
                oChequeBookXL.PageCount = oItem.PageCount.ToString();
                oChequeBookXL.BankName = oItem.BankName;
                oChequeBookXL.BankBranchName = oItem.BankBranchName;
                oChequeBookXL.CompanyName = oItem.BusinessUnitName;
                oChequeBookXLs.Add(oChequeBookXL);
                //nTotal = nTotal + nCount;
            }

            #region Total
            //oChequeBookXL = new ChequeBookXL();
            //oChequeBookXL.Code = "";
            //oChequeBookXL.Name = "";
            //oChequeBookXL.ShortName = "Total:";
            //oChequeBookXL.SwiftCode = nTotal.ToString();
            //oChequeBookXLs.Add(oChequeBookXL);
            #endregion

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oChequeBookXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "ChequeBooks.xls");
        }

    }
        #endregion
}