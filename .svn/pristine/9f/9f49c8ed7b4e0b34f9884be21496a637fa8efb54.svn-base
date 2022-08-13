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
    public class PartyWiseBankController : Controller
    {
        #region Declaration
        PartyWiseBank _oPartyWiseBank = new PartyWiseBank();
        List<PartyWiseBank> _oPartyWiseBanks = new List<PartyWiseBank>();
        private string _sSQL = "";
        string _sErrorMessage = "";
        Contractor _oContractor = new Contractor();
        #endregion
        #region Functions
      
        #endregion

        #region Actions
        public ActionResult ViewPartyWiseBanks(int id)
        {  
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oContractor = new Contractor();
            _oContractor = _oContractor.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oContractor.PartyWiseBanks = new List<PartyWiseBank>();
            _oContractor.PartyWiseBanks = PartyWiseBank.GetsByContractor(id,((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oContractor);
        }
        public ActionResult ViewPartyWiseBank(int id, int nid) // PartyWiseBankID
        {
            _oPartyWiseBank = new PartyWiseBank();
            if (id > 0)
            {
                _oPartyWiseBank = _oPartyWiseBank.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else 
            {
                _oPartyWiseBank.ContractorID = nid;
                _oPartyWiseBank.ContractorName = new Contractor().Get(nid, ((User)Session[SessionInfo.CurrentUser]).UserID).Name;
            }
            return View(_oPartyWiseBank);
        }
        [HttpPost]
        public JsonResult GetsPartyWiseBanks(Contractor oContractor)
        {

            _oContractor = new Contractor();
            _oContractor = oContractor;
            _oContractor.PartyWiseBanks = PartyWiseBank.GetsByContractor(_oContractor.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oContractor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult Save(PartyWiseBank oPartyWiseBank)
        {
            _oPartyWiseBank = new PartyWiseBank();
            try
            {
                _oPartyWiseBank = oPartyWiseBank.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPartyWiseBank = new PartyWiseBank();
                _oPartyWiseBank.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPartyWiseBank);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id, double ts)
        {
            string sFeedBackMessage = "";
            try
            {
                PartyWiseBank oPartyWiseBank = new PartyWiseBank();
                sFeedBackMessage = oPartyWiseBank.Delete(id,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
        #endregion
}