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

namespace ESimSolFinancial.Controllers
{
    public class IntegrationSetupController : Controller
    {

        #region  Declaration
        IntegrationSetup _oIntegrationSetup = new IntegrationSetup();
        List<IntegrationSetup> _oIntegrationSetups = new List<IntegrationSetup>();
        IntegrationSetupDetail _oIntegrationSetupDetail = new IntegrationSetupDetail();
        List<IntegrationSetupDetail> _oIntegrationSetupDetails = new List<IntegrationSetupDetail>();
        DebitCreditSetup _oDebitCreditSetup = new DebitCreditSetup();
        List<DebitCreditSetup> _oDebitCreditSetups = new List<DebitCreditSetup>();
        DataCollectionSetup _oDataCollectionSetup = new DataCollectionSetup();
        List<DataCollectionSetup> _oDataCollectionSetups = new List<DataCollectionSetup>();
        #endregion

        #region Function
        private List<IntegrationSetupDetail> GetSetupDetails(List<IntegrationSetupDetail> oIntegrationSetupDetails, List<DebitCreditSetup> oDebitCreditSetups, List<DataCollectionSetup> oDetailDataCollectionSetups, List<DataCollectionSetup> oDrCrDataCollectionSetups)
        {

            foreach (IntegrationSetupDetail oItem in oIntegrationSetupDetails)
            {
                oItem.DebitCreditSetups = GetDebitCreditSetups(oItem.IntegrationSetupDetailID, oDebitCreditSetups, oDrCrDataCollectionSetups);
                oItem.DataCollectionSetups = GetDataCollectionSetups(oItem.IntegrationSetupDetailID, oDetailDataCollectionSetups); // Set Data Collection for single Integration setup Detail

            }
            return oIntegrationSetupDetails;
        }
        private List<DebitCreditSetup> GetDebitCreditSetups(int id, List<DebitCreditSetup> oDebitCreditSetups, List<DataCollectionSetup> oDrCrDataCollectionSetups)
        {
            _oDebitCreditSetups = new List<DebitCreditSetup>();

            foreach (DebitCreditSetup oItem in oDebitCreditSetups)
            {
                if (oItem.IntegrationSetupDetailID == id)
                {
                    oItem.DataCollectionSetups = GetDataCollectionSetups(oItem.DebitCreditSetupID, oDrCrDataCollectionSetups); // Set Data Collection for single Debit Credit setup
                    _oDebitCreditSetups.Add(oItem);
                }
            }
            return _oDebitCreditSetups;
        }
        private List<DataCollectionSetup> GetDataCollectionSetups(int id, List<DataCollectionSetup> oDataCollectionSetups)
        {
            _oDataCollectionSetups = new List<DataCollectionSetup>();
            foreach (DataCollectionSetup oItem in oDataCollectionSetups)
            {
                if (id == oItem.DataReferenceID)
                {
                    _oDataCollectionSetups.Add(oItem);
                }
            }
            return _oDataCollectionSetups;
        }
        #endregion

        #region Actions
        public ActionResult ViewIntegrationSetups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.IntegrationSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));            

            _oIntegrationSetups = new List<IntegrationSetup>();
            _oIntegrationSetups = IntegrationSetup.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oIntegrationSetups);
        }
        public ActionResult ViewIntegrationSetup(int id)
        {
            //ratin
            _oIntegrationSetup = new IntegrationSetup();
            _oDebitCreditSetups = new List<DebitCreditSetup>();
            _oIntegrationSetupDetails = new List<IntegrationSetupDetail>();
            List<DataCollectionSetup> oDetailDataCollectionSetups = new List<DataCollectionSetup>();
            List<DataCollectionSetup> oDrCrDataCollectionSetups = new List<DataCollectionSetup>();
            if (id > 0)
            {
                _oIntegrationSetup = _oIntegrationSetup.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oIntegrationSetupDetails = IntegrationSetupDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oDebitCreditSetups = DebitCreditSetup.GetsByIntegrationSetup(id, (int)Session[SessionInfo.currentUserID]);
                oDetailDataCollectionSetups = DataCollectionSetup.GetsByIntegrationSetup(id, EnumDataReferenceType.IntegrationDetail, (int)Session[SessionInfo.currentUserID]); // Get for Details
                oDrCrDataCollectionSetups = DataCollectionSetup.GetsByIntegrationSetup(id, EnumDataReferenceType.DebitCreditSetup, (int)Session[SessionInfo.currentUserID]); // Get for DebitCredit
                _oIntegrationSetup.IntegrationSetupDetails = GetSetupDetails(_oIntegrationSetupDetails, _oDebitCreditSetups, oDetailDataCollectionSetups, oDrCrDataCollectionSetups);
            }
            _oIntegrationSetup.IntegrationSetupDetail = new IntegrationSetupDetail();

            ViewBag.VoucherSetups = EnumObject.jGets(typeof(EnumVoucherSetup));
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.SetupTypes = EnumObject.jGets(typeof(EnumSetupType));
            ViewBag.VoucherTypes = VoucherType.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.AccountHeadTypes = EnumObject.jGets(typeof(EnumAccountHeadType));
            ViewBag.CostCenterCategorys = ACCostCenter.Gets(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CostCenterReferenceTypes = EnumObject.jGets(typeof(EnumReferenceType));
            ViewBag.VoucherBillReferenceTypes = EnumObject.jGets(typeof(EnumVoucherBillReferenceType));
            ViewBag.VoucherBillTrTypes = EnumObject.jGets(typeof(EnumVoucherBillTrType));
            ViewBag.VoucherReferenceTypes = EnumObject.jGets(typeof(EnumReferenceType));
            ViewBag.ChequeTypes = EnumObject.jGets(typeof(EnumChequeType));
            ViewBag.VOrderRefTypes = EnumObject.jGets(typeof(EnumVOrderRefType));            
            return View(_oIntegrationSetup);
        }
        public ActionResult ViewSetupSequence(double ts)
        {
            int nSequence = 0;
            _oIntegrationSetups = new List<IntegrationSetup>();
            _oIntegrationSetups = IntegrationSetup.Gets((int)Session[SessionInfo.currentUserID]);
            foreach (IntegrationSetup oItem in _oIntegrationSetups)
            {
                nSequence++;
                oItem.Sequence = nSequence;
            }
            return View(_oIntegrationSetups);
        }
        #endregion

        #region Save
        [HttpPost]
        public JsonResult Save(IntegrationSetup oIntegrationSetup)
        {
            _oIntegrationSetup = new IntegrationSetup();
            try
            {                
                _oIntegrationSetup = oIntegrationSetup;
                _oIntegrationSetup.VoucherSetup = (EnumVoucherSetup)oIntegrationSetup.VoucherSetupInt;
                _oIntegrationSetup.SetupType = (EnumSetupType)oIntegrationSetup.SetupTypeInInt;
                _oIntegrationSetup = _oIntegrationSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oIntegrationSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oIntegrationSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Copy(IntegrationSetup oIntegrationSetup)
        {
            /// Insert a new setup 
            _oIntegrationSetup = new IntegrationSetup();
            try
            {
                _oIntegrationSetup = oIntegrationSetup;
                _oIntegrationSetup.IntegrationSetupID = 0;
                /// Detail 
                foreach (IntegrationSetupDetail oitem in _oIntegrationSetup.IntegrationSetupDetails)
                {
                    oitem.IntegrationSetupDetailID = 0;


                    foreach (DataCollectionSetup oDataCollectionSetup in oitem.DataCollectionSetups)
                    {
                        oDataCollectionSetup.DataCollectionSetupID = 0;
                    }

                    foreach (DebitCreditSetup oDebitCreditSetup in oitem.DebitCreditSetups)
                    {
                        oDebitCreditSetup.DebitCreditSetupID = 0;
                        oDebitCreditSetup.IntegrationSetupDetailID = 0;

                        foreach (DataCollectionSetup oDataCollectionSetup in oDebitCreditSetup.DataCollectionSetups)
                        {
                            oDataCollectionSetup.DataCollectionSetupID = 0;
                        }
                    }


                }
                //  Detail 
                foreach (DataCollectionSetup oitem in _oIntegrationSetup.DataCollectionSetups)
                {

                    oitem.DataCollectionSetupID = 0;
                    oitem.DataReferenceID = 0;
                }
                foreach (DataCollectionSetup oitem in _oIntegrationSetup.DataCollectionSetups)
                {
                    oitem.DataReferenceID = 0;
                    oitem.DataCollectionSetupID = 0;
                    oitem.DataReferenceID = 0;
                }


                _oIntegrationSetup = _oIntegrationSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oIntegrationSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oIntegrationSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateSequence(IntegrationSetup oIntegrationSetup)
        {
            _oIntegrationSetups = new List<IntegrationSetup>();
            try
            {
                _oIntegrationSetups = IntegrationSetup.UpdateSequence(oIntegrationSetup, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {                
                _oIntegrationSetup = new IntegrationSetup();
                _oIntegrationSetups = new List<IntegrationSetup>();
                _oIntegrationSetup.ErrorMessage = ex.Message;                
                _oIntegrationSetups.Add(_oIntegrationSetup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oIntegrationSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sMessage = "";

            try
            {
                IntegrationSetup oIntegrationSetup = new IntegrationSetup();
                sMessage = oIntegrationSetup.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost]
        public JsonResult RefreshProcesses(IntegrationSetup oIntegrationSetup)
        {
            _oIntegrationSetups = new List<IntegrationSetup>();
            string sSQL = "SELECT * FROM View_IntegrationSetup AS HH WHERE HH.BUID = " + oIntegrationSetup.BUID.ToString() + " AND HH.IntegrationSetupID IN (SELECT VPP.IntegrationSetupID FROM VPPermission AS VPP WHERE VPP.UserID = " + ((int)Session[SessionInfo.currentUserID]).ToString() + ")  ORDER BY HH.Sequence ASC";
            _oIntegrationSetups = IntegrationSetup.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            if (_oIntegrationSetups.Count <= 0)
            {
                _oIntegrationSetups = IntegrationSetup.GetsByBU(oIntegrationSetup.BUID, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oIntegrationSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsIntegrationSetup(IntegrationSetup oIntegrationSetup)
        {
            _oIntegrationSetups = new List<IntegrationSetup>();
            if (oIntegrationSetup.BUID > 0)
            {
                _oIntegrationSetups = IntegrationSetup.GetsByBU(oIntegrationSetup.BUID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oIntegrationSetups = IntegrationSetup.Gets((int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oIntegrationSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}