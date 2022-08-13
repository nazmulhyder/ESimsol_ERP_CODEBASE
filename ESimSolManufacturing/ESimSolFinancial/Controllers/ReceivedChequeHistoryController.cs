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
    public class ReceivedChequeHistoryController : Controller
    {
        #region Declaration
        ReceivedChequeHistory _oReceivedChequeHistory = new ReceivedChequeHistory();
        List<ReceivedChequeHistory> _oReceivedChequeHistorys = new List<ReceivedChequeHistory>();
        private string _sSQL = "";
        string _sErrorMessage = "";
        bool _bForPrint = false;
        #endregion
        #region Functions
        private bool ValidateInput(ReceivedChequeHistory oReceivedChequeHistory)
        {
            //if (oReceivedChequeHistory.Name == null || oReceivedChequeHistory.Name == "")
            //{
            //    _sErrorMessage = "Please enter ReceivedChequeHistory Name";
            //    return false;
            //}

            //if (oReceivedChequeHistory.ShortName == null || oReceivedChequeHistory.ShortName == "")
            //{
            //    _sErrorMessage = "Please enter Shortname";
            //    return false;
            //}
            //if (oReceivedChequeHistory.PrintSetupID == null || oReceivedChequeHistory.PrintSetupID == 0)
            //{
            //    _sErrorMessage = "Please Select a Print Setup";
            //    return false;
            //}

            return true;
        }
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM View_ReceivedChequeHistory";
            int nReceivedChequeID = _bForPrint ? 0 : (Arguments.Split(';')[1].Split('~')[0] == null) ? 0 : (Arguments.Split(';')[1].Split('~')[0] == "") ? 0 : Convert.ToInt32(Arguments.Split(';')[1].Split('~')[0]);
            string sReceivedChequeHistoryIDs = _bForPrint ? (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1] : "";

            string sSQL = "";



            #region ReceivedChequeID
            if (nReceivedChequeID != null)
            {
                if (nReceivedChequeID > 0)
                {

                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " ReceivedChequeID =" + nReceivedChequeID + " ";
                }
            }
            #region ReceivedChequeHistoryIDs
            if (sReceivedChequeHistoryIDs != null)
            {
                if (sReceivedChequeHistoryIDs != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " ReceivedChequeHistoryID IN (" + sReceivedChequeHistoryIDs + ") ";
                }
            }
            #endregion
            #endregion
            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }
            _sSQL = _sSQL + " ORDER BY ReceivedChequeHistoryID DESC";
        }
        #endregion

        #region Actions

        public ActionResult ViewSearchReceivedChequeHistorys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ReceivedChequeHistory).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oReceivedChequeHistorys = new List<ReceivedChequeHistory>();

            return View(_oReceivedChequeHistorys);
        }
        public ActionResult ViewReceivedChequeHistorys(int nid)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ReceivedChequeHistory).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oReceivedChequeHistorys = new List<ReceivedChequeHistory>();
            ReceivedCheque oReceivedCheque = new ReceivedCheque().Get(nid, (int)Session[SessionInfo.currentUserID]);

            _oReceivedChequeHistorys = new List<ReceivedChequeHistory>();
            if (oReceivedCheque.ReceivedChequeID > 0) { _oReceivedChequeHistorys = ReceivedChequeHistory.Gets(oReceivedCheque.ReceivedChequeID, (int)Session[SessionInfo.currentUserID]); }
            return View(_oReceivedChequeHistorys);
        }



        [HttpPost]
        public JsonResult Refresh(ReceivedChequeHistory oReceivedChequeHistory)
        {
            int nReceivedChequeID = (oReceivedChequeHistory.ErrorMessage.Split(';')[1].Split('~')[0] == null) ? 0 : (oReceivedChequeHistory.ErrorMessage.Split(';')[1].Split('~')[0] == "") ? 0 : Convert.ToInt32(oReceivedChequeHistory.ErrorMessage.Split(';')[1].Split('~')[0]);
            ReceivedCheque oReceivedCheque = new ReceivedCheque().Get(nReceivedChequeID, (int)Session[SessionInfo.currentUserID]);

            _oReceivedChequeHistorys = new List<ReceivedChequeHistory>();
            if (oReceivedCheque.ReceivedChequeID > 0) { _oReceivedChequeHistorys = ReceivedChequeHistory.Gets(oReceivedCheque.ReceivedChequeID, (int)Session[SessionInfo.currentUserID]); }
            if (_oReceivedChequeHistorys.Count > 0)
            {
                if (_oReceivedChequeHistorys[0].ReceivedChequeHistoryID > 0)
                {
                    _oReceivedChequeHistorys[0].ErrorMessage = "Status History of Received Cheque No: " + oReceivedCheque.ChequeNo + " from Account No: " + oReceivedCheque.AccountNo;
                }
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReceivedChequeHistorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        public ActionResult PrintReceivedChequeHistorys(string arguments)
        {
            _bForPrint = true;
            this.MakeSQL(arguments);
            _oReceivedChequeHistorys = new List<ReceivedChequeHistory>();
            _oReceivedChequeHistorys = ReceivedChequeHistory.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);



            string Messge = "ReceivedChequeHistory List";
            rptReceivedChequeHistorys oReport = new rptReceivedChequeHistorys();
            byte[] abytes = oReport.PrepareReport(_oReceivedChequeHistorys, oCompany, Messge);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintReceivedChequeHistorysInXL(string arguments)
        {
            _bForPrint = true;
            this.MakeSQL(arguments);
            //_productsServices = new ProductsServices();
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<ReceivedChequeHistoryXL>));

            //We load the data
            List<ReceivedChequeHistory> oReceivedChequeHistorys = ReceivedChequeHistory.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);
            int nCount = 0; double nTotal = 0;
            ReceivedChequeHistoryXL oReceivedChequeHistoryXL = new ReceivedChequeHistoryXL();
            List<ReceivedChequeHistoryXL> oReceivedChequeHistoryXLs = new List<ReceivedChequeHistoryXL>();
            foreach (ReceivedChequeHistory oItem in oReceivedChequeHistorys)
            {
                nCount++;
                oReceivedChequeHistoryXL = new ReceivedChequeHistoryXL();
                oReceivedChequeHistoryXL.SLNo = nCount.ToString();
                oReceivedChequeHistoryXL.PreviousStatus = oItem.PreviousStatusInString;
                oReceivedChequeHistoryXL.CurrentStatus = oItem.CurrentStatusInString;
                oReceivedChequeHistoryXL.OperationBy = oItem.OperationByName;
                oReceivedChequeHistoryXL.Note = oItem.Note;
                oReceivedChequeHistoryXL.ChangeLog = oItem.ChangeLog;
                oReceivedChequeHistoryXL.OperationDateTime = oItem.OperationDateTimeInString;
                oReceivedChequeHistoryXLs.Add(oReceivedChequeHistoryXL);
                //nTotal = nTotal + nCount;
            }



            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oReceivedChequeHistoryXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "ReceivedChequeHistorys.xls");
        }

    }
        #endregion
}