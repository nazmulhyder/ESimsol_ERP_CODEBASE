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
    public class IssueFigureController : Controller
    {
        #region Declaration
        IssueFigure _oIssueFigure = new IssueFigure();
        List<IssueFigure> _oIssueFigures = new List<IssueFigure>();
        private string _sSQL = "";
        string _sErrorMessage = "";
        Contractor _oContractor = new Contractor();
        bool _bForPrint = false;
        #endregion
        #region Functions
        private bool ValidateInput(IssueFigure oIssueFigure)
        {
            //if (oIssueFigure.Name == null || oIssueFigure.Name == "")
            //{
            //    _sErrorMessage = "Please enter IssueFigure Name";
            //    return false;
            //}

            //if (oIssueFigure.ShortName == null || oIssueFigure.ShortName == "")
            //{
            //    _sErrorMessage = "Please enter Shortname";
            //    return false;
            //}
            //if (oIssueFigure.PrintSetupID == null || oIssueFigure.PrintSetupID == 0)
            //{
            //    _sErrorMessage = "Please Select a Print Setup";
            //    return false;
            //}

            return true;
        }
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM IssueFigure";
            string sChequeIssueTo = _bForPrint ? "" : (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[0];
            string sIssueFigureIDs = _bForPrint ? (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1] : "";

            string sSQL = "";
            sSQL = _bForPrint ? "" : " WHERE ContractorID= " + _oContractor.ContractorID;

            #region ChequeIssueTo
            if (sChequeIssueTo != null)
            {
                if (sChequeIssueTo != "")
                {
                    if (sChequeIssueTo != "Search by IssueTo Name")
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " ChequeIssueTo LIKE '%" + sChequeIssueTo + "%' ";
                    }
                }
            }
            #endregion
            #region IssueFigureIDs
            if (sIssueFigureIDs != null)
            {
                if (sIssueFigureIDs != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " IssueFigureID IN (" + sIssueFigureIDs + ") ";
                }
            }
            #endregion
            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }
            _sSQL = _sSQL + " ORDER BY ChequeIssueTo ASC";
        }
        #endregion

        #region Actions

        public ActionResult ViewIssueFigures(int id)
        {  
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.IssueFigure).ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID, ((User)Session[SessionInfo.CurrentUser]).UserID));
            _oContractor = new Contractor();
            _oContractor = _oContractor.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oContractor.IssueFigures = new List<IssueFigure>();
            _oContractor.IssueFigures = IssueFigure.Gets(id, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oContractor);
        }

        public ActionResult ViewIssueFigure(int id, int nid) // IssueFigureID
        {
            _oIssueFigure = new IssueFigure();
            if (id > 0)
            {
                _oIssueFigure = _oIssueFigure.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else 
            {
                _oIssueFigure.ContractorID = nid;
                _oIssueFigure.ContractorName = new Contractor().Get(nid, ((User)Session[SessionInfo.CurrentUser]).UserID).Name;
                _oIssueFigure.IsActive = true; 
            }
            return View(_oIssueFigure);
        }
        [HttpPost]
        public JsonResult GetsIssueFigures(Contractor oContractor)
        {

            _oContractor = new Contractor();
            _oContractor = oContractor;
            _oContractor.IssueFigures = IssueFigure.Gets(_oContractor.ContractorID, true, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Refresh(Contractor oContractor)
        {
            _oContractor = oContractor;
            this.MakeSQL(_oContractor.ErrorMessage);
            _oIssueFigures = new List<IssueFigure>();
            _oIssueFigures = IssueFigure.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oIssueFigures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(IssueFigure oIssueFigure)
        {
            _oIssueFigure = new IssueFigure();
            try
            {
                oIssueFigure.ChequeIssueTo = oIssueFigure.ChequeIssueTo == null ? "" : oIssueFigure.ChequeIssueTo;
                oIssueFigure.SecondLineIssueTo = oIssueFigure.SecondLineIssueTo == null ? "" : oIssueFigure.SecondLineIssueTo;
                oIssueFigure.DetailNote = oIssueFigure.DetailNote == null ? "" : oIssueFigure.DetailNote;
                if (!this.ValidateInput(oIssueFigure))
                {
                    throw new Exception(_sErrorMessage);
                }
                _oIssueFigure = oIssueFigure;
                _oIssueFigure = _oIssueFigure.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oIssueFigure = new IssueFigure();
                _oIssueFigure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oIssueFigure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id, double ts)
        {
            string sFeedBackMessage = "";
            try
            {
                IssueFigure oIssueFigure = new IssueFigure();
                sFeedBackMessage = oIssueFigure.Delete(id,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintIssueFigures(string arguments)
        {
            _bForPrint = true;
            this.MakeSQL(arguments);
            _oIssueFigures = new List<IssueFigure>();
            _oIssueFigures = IssueFigure.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);



            string Messge = "IssueFigure List";
            rptIssueFigures oReport = new rptIssueFigures();
            byte[] abytes = oReport.PrepareReport(_oIssueFigures, oCompany, Messge);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintIssueFiguresInXL(string arguments)
        {
            _bForPrint = true;
            this.MakeSQL(arguments);
            //_productsServices = new ProductsServices();
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<IssueFigureXL>));

            //We load the data
            List<IssueFigure> oIssueFigures = IssueFigure.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            int nCount = 0; double nTotal = 0;
            IssueFigureXL oIssueFigureXL = new IssueFigureXL();
            List<IssueFigureXL> oIssueFigureXLs = new List<IssueFigureXL>();
            foreach (IssueFigure oItem in oIssueFigures)
            {
                nCount++;
                oIssueFigureXL = new IssueFigureXL();
                oIssueFigureXL.SLNo = nCount.ToString();
                oIssueFigureXL.ChequeIssueTo = oItem.ChequeIssueTo;
                oIssueFigureXL.SecondLineIssueTo = oItem.SecondLineIssueTo;
            }

            

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oIssueFigureXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "IssueFigures.xls");
        }

    }
        #endregion
}