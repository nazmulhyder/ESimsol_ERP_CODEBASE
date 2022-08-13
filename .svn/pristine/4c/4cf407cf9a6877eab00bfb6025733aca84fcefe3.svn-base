using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using ICS.Core.Utility;
using iTextSharp.text;

namespace ESimSolFinancial.Controllers
{
    public class ACCostCenterController : Controller
    {
        #region Declaration
        List<ACCostCenter> _oACCostCenters = new List<ACCostCenter>();
        ACCostCenter _oACCostCenter = new ACCostCenter();
        TACCostCenter _oTACCostCenter = new TACCostCenter();
        List<TACCostCenter> _oTACCostCentres = new List<TACCostCenter>();
        #endregion

        #region Functions
        private TACCostCenter GetRoot(int nParentID)
        {
            TACCostCenter oTACCostCentre = new TACCostCenter();
            foreach (TACCostCenter oItem in _oTACCostCentres)
            {
                if (oItem.parentid == nParentID)
                {
                    return oItem;
                }
            }
            return oTACCostCentre;
        }

        private void AddTreeNodes(ref TACCostCenter oTACCostCentre)
        {
            List<TACCostCenter> oChildNodes=new List<TACCostCenter>();
            oChildNodes = GetChildren(oTACCostCentre.id);
            oTACCostCentre.children = oChildNodes;

            foreach (TACCostCenter oItem in oChildNodes)
            {
                TACCostCenter oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }

        private List<TACCostCenter> GetChildren(int nAccountHeadID)
        {
            List<TACCostCenter> oTACCostCentres = new List<TACCostCenter>();
            foreach (TACCostCenter oItem in _oTACCostCentres)
            {
                if (oItem.parentid == nAccountHeadID)
                {
                    oTACCostCentres.Add(oItem);
                }
            }
            return oTACCostCentres;
        }

        private List<TACCostCenter> DefineState(List<TACCostCenter> oTACCostCenters)
        {
            foreach (TACCostCenter oItem in oTACCostCenters)
            {
                if (oItem.children.Count > 0)
                {
                    oItem.state = "closed";
                }
                else
                {
                    oItem.state = "open";
                }
            }
            return oTACCostCenters;
        }

        #endregion

        #region AC CostCenter
        public ActionResult ViewCostCenters(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oACCostCenters = new List<ACCostCenter>();
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ACCostCenter).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oACCostCenter.ParentID = 1;            
            _oACCostCenters = ACCostCenter.Gets(_oACCostCenter.ParentID, (int)Session[SessionInfo.currentUserID]);

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oACCostCenters);
        }

        public ActionResult ViewCostCenter(int id, int cct, double ts)
        {            
            ACCostCenter oACCostCenter = new ACCostCenter();
            if (id > 0)
            {
                oACCostCenter = oACCostCenter.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                ACCostCenter oACCParent = new ACCostCenter();
                oACCParent = oACCParent.Get(cct, (int)Session[SessionInfo.currentUserID]);
                oACCostCenter.ParentID = oACCParent.ACCostCenterID;                
            }
            return PartialView(oACCostCenter);
        }

        [HttpPost]
        public JsonResult GetCostCenter(ACCostCenter oSubledger)
        {
            ACCostCenter oACCostCenter = new ACCostCenter();
            try
            {
                if (oSubledger.ACCostCenterID > 0)
                {
                    oACCostCenter = oACCostCenter.Get(oSubledger.ACCostCenterID, (int)Session[SessionInfo.currentUserID]);                    
                    oACCostCenter.BUWiseSubLedgers = BUWiseSubLedger.GetsByCC(oACCostCenter.ACCostCenterID, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    ACCostCenter oACCParent = new ACCostCenter();
                    oACCParent = oACCParent.Get(oSubledger.ParentID, (int)Session[SessionInfo.currentUserID]);
                    oACCostCenter.ParentID = oACCParent.ACCostCenterID;
                    oACCostCenter.CategoryName = oACCParent.Name;
                }

                List<SubledgerRefConfig> oSubledgerRefConfigs = new List<SubledgerRefConfig>();
                List<AccountHeadConfigure> oAccountHeadConfigures = new List<AccountHeadConfigure>();                
                if (oACCostCenter.ParentID == 1)
                {
                    oAccountHeadConfigures = AccountHeadConfigure.Gets("SELECT * FROM View_AccountHeadConfigure WHERE ReferenceObjectType = " + (int)EnumVoucherExplanationType.CostCenter + " AND ReferenceObjectID = " + oACCostCenter.ACCostCenterID, (int)Session[SessionInfo.currentUserID]);
                    oACCostCenter.AccountHeadConfigures = oAccountHeadConfigures;
                }
                else
                {
                    oSubledgerRefConfigs = SubledgerRefConfig.Gets(oACCostCenter.ACCostCenterID, (int)Session[SessionInfo.currentUserID]);
                    oAccountHeadConfigures = AccountHeadConfigure.Gets("SELECT * FROM View_AccountHeadConfigure WHERE ReferenceObjectType = " + (int)EnumVoucherExplanationType.CostCenter + " AND ReferenceObjectID = " + oACCostCenter.ParentID, (int)Session[SessionInfo.currentUserID]);
                    foreach (AccountHeadConfigure oItem in oAccountHeadConfigures)
                    {
                        oItem.IsBillRefApply = this.GetSubledgerRefValue(oSubledgerRefConfigs, oItem.AccountHeadID, true);
                        oItem.IsOrderRefApply = this.GetSubledgerRefValue(oSubledgerRefConfigs, oItem.AccountHeadID, false);
                    }
                    oACCostCenter.AccountHeadConfigures = oAccountHeadConfigures;
                }
                
            }
            catch (Exception ex)
            {
                oACCostCenter = new ACCostCenter();
                oACCostCenter.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oACCostCenter);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private bool GetSubledgerRefValue(List<SubledgerRefConfig> oSubledgerRefConfigs, int nAccountHeadID, bool bIsBill)
        {
            foreach (SubledgerRefConfig oItem in oSubledgerRefConfigs)
            {
                if (oItem.AccountHeadID == nAccountHeadID)
                {
                    if (bIsBill)
                    {
                        return oItem.IsBillRefApply;
                    }
                    else
                    {
                        return oItem.IsOrderRefApply;
                    }
                }
            }
            return false;
        }



        [HttpPost]
        public JsonResult Save(ACCostCenter oACCostCenter)
        {
            _oACCostCenter = new ACCostCenter();
            try
            {
                oACCostCenter.Name = oACCostCenter.Name == null ? "" : oACCostCenter.Name;
                oACCostCenter.Code = oACCostCenter.Code == null ? "" : oACCostCenter.Code;
                oACCostCenter.Description = oACCostCenter.Description == null ? "" : oACCostCenter.Description;
                oACCostCenter.SubledgerRefConfigs = (oACCostCenter.IsBillRefApply == false) ? oACCostCenter.SubledgerRefConfigs = new List<SubledgerRefConfig>() : oACCostCenter.SubledgerRefConfigs;
                oACCostCenter.BUWiseSubLedgers = (oACCostCenter.IsOrderRefApply == false) ? oACCostCenter.BUWiseSubLedgers = new List<BUWiseSubLedger>() : oACCostCenter.BUWiseSubLedgers;
                _oACCostCenter = oACCostCenter;                
                _oACCostCenter = _oACCostCenter.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oACCostCenter = new ACCostCenter();
                _oACCostCenter.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oACCostCenter);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                ACCostCenter oACCostCenter = new ACCostCenter();

                sFeedBackMessage = oACCostCenter.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetCategoryWiseCC(ACCostCenter oACCostCenter)
        {            
            _oACCostCenters = new List<ACCostCenter>();
            try
            {
                string sSQL = "SELECT * FROM View_ACCostCenter AS HH WHERE HH.ParentID=" + oACCostCenter.ParentID.ToString();
                if (oACCostCenter.BUID > 0)
                {
                    sSQL = sSQL + " AND HH.ACCostCenterID IN (SELECT MM.SubledgerID FROM BUWiseSubLedger AS MM WHERE MM.BusinessUnitID=" + oACCostCenter.BUID.ToString() + ")";
                }
                sSQL = sSQL + " ORDER BY HH.Name";

                _oACCostCenters = ACCostCenter.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oACCostCenter = new ACCostCenter();
                _oACCostCenter.ErrorMessage = ex.Message;
                _oACCostCenters.Add(_oACCostCenter);
            }

            var jsonResult = Json(_oACCostCenters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oACCostCenters);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsCostCenter(ACCostCenter oACCostCenter)
        {
            _oACCostCenters = new List<ACCostCenter>();            
            if (oACCostCenter.Name == null) oACCostCenter.Name = "";
            try
            {
                _oACCostCenters = ACCostCenter.GetsByConfigure(oACCostCenter.AccountHeadID, oACCostCenter.Name, oACCostCenter.BUID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oACCostCenter = new ACCostCenter();
                _oACCostCenter.ErrorMessage = ex.Message;
                _oACCostCenters.Add(_oACCostCenter);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oACCostCenters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Sub Ledger Picker
        public ActionResult ViewACCostCenterPicker(int id, string sTempCCName , double ts)
        {
            _oACCostCenters = new List<ACCostCenter>();
            //if (id > 0)
            //{             
                _oACCostCenters = ACCostCenter.GetsByConfigure(id, sTempCCName, 0, (int)Session[SessionInfo.currentUserID]);
            //}
            return PartialView(_oACCostCenters);
        }
        [HttpPost]
        public JsonResult GetACCostCenters(ACCostCenter oACCostCenter)
        {
            _oACCostCenters = new List<ACCostCenter>();
            //if (id > 0)
            //{             
            _oACCostCenters = ACCostCenter.GetsByConfigure(oACCostCenter.AccountHeadID, oACCostCenter.CategoryName, 0,  (int)Session[SessionInfo.currentUserID]);
            //}
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oACCostCenters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetACCostCenters_ForAccountEffect(ACCostCenter oACCostCenter)
        {
            _oACCostCenters = new List<ACCostCenter>();
            string sSQL = "SELECT * FROM View_ACCostCenter WHERE ParentID!=1";
            if(!string.IsNullOrEmpty(oACCostCenter.Name))
            {
                sSQL += " AND Name Like '%"+oACCostCenter.Name+"%'";
            }
            _oACCostCenters = ACCostCenter.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            var jSonResult = Json(_oACCostCenters, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        #endregion

        #region Extra Code for testing
        [HttpPost]
        public JsonResult GetsCostCenterTest(ACCostCenter oACCostCenter)
        {
            _oACCostCenters = new List<ACCostCenter>();
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            if (oACCostCenter.Name == null) oACCostCenter.Name = "";
            try
            {
                string sSQL = "SELECT * FROM View_ACCostCenter WHERE Name LIKE ('%" + oACCostCenter.Name + "%')";
                _oACCostCenters = ACCostCenter.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oACCostCenter = new ACCostCenter();
                _oACCostCenter.ErrorMessage = ex.Message;
                _oACCostCenters.Add(_oACCostCenter);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oACCostCenters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult GetsByCodeOrName(ACCostCenter oACCostCenter)
        {
            VoucherType oVoucherType = new VoucherType();
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            
            oACCostCenters = ACCostCenter.GetsByCodeOrName(oACCostCenter, (int)Session[SessionInfo.currentUserID], oACCostCenter.BUID);
            oVoucherType = oVoucherType.Get(oACCostCenter.VoucherTypeID, (int)Session[SessionInfo.currentUserID]);

            foreach (ACCostCenter oItem in oACCostCenters)
            {
                oItem.IsPaymentCheque = oVoucherType.IsPaymentCheque;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oACCostCenters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByName(ACCostCenter oACCostCenter)
        {
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            string sSQL = "SELECT * FROM View_ACCostCenter WHERE ACCostCenterID!=1 AND NameCode LIKE '%" + oACCostCenter.NameCode + "%' ORDER BY Name";
            oACCostCenters = ACCostCenter.Gets( sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oACCostCenters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsByNameForLandingCost(ACCostCenter oACCostCenter)
        {
            //Here 403 means Landing Cost Category
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            string sSQL = "SELECT * FROM View_ACCostCenter WHERE ParentID =403 AND NameCode LIKE '%" + oACCostCenter.NameCode + "%' ORDER BY Name";
            oACCostCenters = ACCostCenter.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oACCostCenters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult GetsByCode(ACCostCenter oACCostCenter)
        {
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            oACCostCenters = ACCostCenter.GetsByCode(oACCostCenter, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oACCostCenters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsACCostCentersForOrderReference(ACCostCenter oACCostCenter)
        {
            //Here 403 means Landing Cost Category
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>();
            string sSQL = "SELECT * FROM View_ACCostCenter AS HH WHERE HH.ParentID NOT IN (0,1)  AND HH.ACCostCenterID IN (SELECT MM.SubLedgerID FROM BUWiseSubLedger AS MM WHERE MM.BusinessUnitID=" + oACCostCenter.BUID+ ")";
            //VOrderRefType
            if (oACCostCenter.ReferenceTypeInt == (int)EnumVOrderRefType.ExportPI || oACCostCenter.ReferenceTypeInt == (int)EnumVOrderRefType.ExportLC)
            {
                sSQL += " AND HH.ReferenceType = "+(int)EnumReferenceType.Customer;
            }
            else if (oACCostCenter.ReferenceTypeInt == (int)EnumVOrderRefType.ImportPI || oACCostCenter.ReferenceTypeInt == (int)EnumVOrderRefType.ImportLC)
            {
                sSQL += " AND HH.ReferenceType = " + (int)EnumReferenceType.Vendor;
            }

            if(!string.IsNullOrEmpty(oACCostCenter.Name))
            {
                sSQL += " AND HH.Name LIKE '%" + oACCostCenter.Name+"%'";
            }
            oACCostCenters = ACCostCenter.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
         
            var jsonResult = Json(oACCostCenters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Print 
        public ActionResult PrintSubledger(int nSLCID)
        {
            List<ACCostCenter> oSubLedgerCategorys = new List<ACCostCenter>();
            List<ACCostCenter> oSubLedgers = new List<ACCostCenter>();
            string sSQL = "";

            sSQL = "SELECT * FROM View_ACCostCenter AS HH WHERE HH.ACCostCenterID = " + nSLCID.ToString() + " ORDER BY HH.ParentID, HH.Name ASC";
            if (nSLCID == 1)
            {
                sSQL = "SELECT * FROM View_ACCostCenter AS HH WHERE HH.ParentID=" + nSLCID.ToString() + " ORDER BY HH.ParentID, HH.Name ASC";
            }
            oSubLedgerCategorys = ACCostCenter.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);


            sSQL = "SELECT * FROM View_ACCostCenter AS HH WHERE HH.ParentID=" + nSLCID.ToString() + " ORDER BY HH.ParentID, HH.Name ASC";
            if (nSLCID == 1)
            {
                sSQL = "SELECT * FROM View_ACCostCenter AS HH WHERE HH.ParentID IN (SELECT MM.ACCostCenterID FROM ACCostCenter AS MM WHERE MM.ParentID=" + nSLCID.ToString() + ") ORDER BY HH.ParentID, HH.Name ASC";
            }
            oSubLedgers = ACCostCenter.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptSubLedgers oReport = new rptSubLedgers();
            byte[] abytes = oReport.PrepareReport(oSubLedgerCategorys, oSubLedgers, oCompany);
            return File(abytes, "application/pdf");

        }

        public System.Drawing.Image GetCompanyLogo(Company oCompany)
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
        #endregion
    }
}