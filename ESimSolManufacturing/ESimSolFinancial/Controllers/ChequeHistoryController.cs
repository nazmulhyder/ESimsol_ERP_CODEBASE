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
    public class ChequeHistoryController : Controller
    {
        #region Declaration
        ChequeHistory _oChequeHistory = new ChequeHistory();
        List<ChequeHistory> _oChequeHistorys = new List<ChequeHistory>();
        private string _sSQL = "";
        string _sErrorMessage = "";
        bool _bForPrint = false;
        #endregion
        #region Functions
        private bool ValidateInput(ChequeHistory oChequeHistory)
        {
            //if (oChequeHistory.Name == null || oChequeHistory.Name == "")
            //{
            //    _sErrorMessage = "Please enter ChequeHistory Name";
            //    return false;
            //}

            //if (oChequeHistory.ShortName == null || oChequeHistory.ShortName == "")
            //{
            //    _sErrorMessage = "Please enter Shortname";
            //    return false;
            //}
            //if (oChequeHistory.PrintSetupID == null || oChequeHistory.PrintSetupID == 0)
            //{
            //    _sErrorMessage = "Please Select a Print Setup";
            //    return false;
            //}

            return true;
        }
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM View_ChequeHistory";
            int nChequeID = _bForPrint ? 0 : (Arguments.Split(';')[1].Split('~')[0] == null) ? 0 : (Arguments.Split(';')[1].Split('~')[0] == "") ? 0 : Convert.ToInt32(Arguments.Split(';')[1].Split('~')[0]);
            string sChequeHistoryIDs = _bForPrint ? (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1] : "";

            string sSQL = "";



            #region ChequeID
            if (nChequeID != null)
            {
                if (nChequeID > 0)
                {

                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " ChequeID =" + nChequeID + " ";
                }
            }
            #region ChequeHistoryIDs
            if (sChequeHistoryIDs != null)
            {
                if (sChequeHistoryIDs != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " ChequeHistoryID IN (" + sChequeHistoryIDs + ") ";
                }
            }
            #endregion
            #endregion
            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }
            _sSQL = _sSQL + " ORDER BY ChequeHistoryID DESC";
        }
        #endregion

        #region Actions

        public ActionResult ViewSearchChequeHistorys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ChequeHistory).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oChequeHistorys = new List<ChequeHistory>();

            return View(_oChequeHistorys);
        }
        public ActionResult ViewChequeHistorys(int nid)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ChequeHistory).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oChequeHistorys = new List<ChequeHistory>();
            Cheque oCheque = new Cheque().Get(nid, (int)Session[SessionInfo.currentUserID]);

            _oChequeHistorys = new List<ChequeHistory>();
            if (oCheque.ChequeID > 0) { _oChequeHistorys = ChequeHistory.Gets(oCheque.ChequeID, (int)Session[SessionInfo.currentUserID]); }
            return View(_oChequeHistorys);
        }



        [HttpPost]
        public JsonResult Refresh(ChequeHistory oChequeHistory)
        {
            int nChequeID = (oChequeHistory.ErrorMessage.Split(';')[1].Split('~')[0] == null) ? 0 : (oChequeHistory.ErrorMessage.Split(';')[1].Split('~')[0] == "") ? 0 : Convert.ToInt32(oChequeHistory.ErrorMessage.Split(';')[1].Split('~')[0]);
            Cheque oCheque = new Cheque().Get(nChequeID, (int)Session[SessionInfo.currentUserID]);

            _oChequeHistorys = new List<ChequeHistory>();
            if (oCheque.ChequeID > 0) { _oChequeHistorys = ChequeHistory.Gets(oCheque.ChequeID, (int)Session[SessionInfo.currentUserID]); }
            if (_oChequeHistorys.Count > 0)
            {
                if (_oChequeHistorys[0].ChequeHistoryID > 0)
                {
                    _oChequeHistorys[0].ErrorMessage = "Status History of Cheque No: " + oCheque.ChequeNo + " from Book No: " + oCheque.BookCode;
                }
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChequeHistorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        public ActionResult PrintChequeHistorys(string arguments)
        {
            _bForPrint = true;
            this.MakeSQL(arguments);
            _oChequeHistorys = new List<ChequeHistory>();
            _oChequeHistorys = ChequeHistory.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);



            string Messge = "ChequeHistory List";
            rptChequeHistorys oReport = new rptChequeHistorys();
            byte[] abytes = oReport.PrepareReport(_oChequeHistorys, oCompany, Messge);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintChequeHistorysInXL(string arguments)
        {
            _bForPrint = true;
            this.MakeSQL(arguments);
            //_productsServices = new ProductsServices();
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<ChequeHistoryXL>));

            //We load the data
            List<ChequeHistory> oChequeHistorys = ChequeHistory.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            int nCount = 0; double nTotal = 0;
            ChequeHistoryXL oChequeHistoryXL = new ChequeHistoryXL();
            List<ChequeHistoryXL> oChequeHistoryXLs = new List<ChequeHistoryXL>();
            foreach (ChequeHistory oItem in oChequeHistorys)
            {
                nCount++;
                oChequeHistoryXL = new ChequeHistoryXL();
                oChequeHistoryXL.SLNo = nCount.ToString();
                oChequeHistoryXL.PreviousStatus = oItem.PreviousStatusInString;
                oChequeHistoryXL.CurrentStatus = oItem.CurrentStatusInString;
                oChequeHistoryXL.OperationBy = oItem.OperationByName;
                oChequeHistoryXL.Note = oItem.Note;
                oChequeHistoryXL.ChangeLog = oItem.ChangeLog;
                oChequeHistoryXL.OperationDateTime = oItem.OperationDateTimeInString;
                oChequeHistoryXLs.Add(oChequeHistoryXL);
                //nTotal = nTotal + nCount;
            }



            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oChequeHistoryXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "ChequeHistorys.xls");
        }

    }
        #endregion
}