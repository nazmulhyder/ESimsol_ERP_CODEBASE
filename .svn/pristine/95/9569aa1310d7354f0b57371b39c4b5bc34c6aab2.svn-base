using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSolFinancial.Controllers;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
 
using ESimSol.Reports;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;

namespace ESimSolFinancial.Controllers
{

    public class TAPTemplateController : Controller
    {
        #region Declaration
        TAPTemplate _oTAPTemplate = new TAPTemplate();
        List<TAPTemplate> _oTAPTemplates = new List<TAPTemplate>();
        TAPTemplateDetail _oTAPTemplateDetail = new TAPTemplateDetail();
        List<TAPTemplateDetail> _oTAPTemplateDetails = new List<TAPTemplateDetail>();
        TTAPTemplateDetail _oTTAPTemplateDetail = new TTAPTemplateDetail();
        List<TTAPTemplateDetail> _oTTAPTemplateDetails = new List<TTAPTemplateDetail>();
        string _sErrorMessage = "";
        #endregion

        #region Function
        #region Manage Sequence

        //write by Mahaub
        private List<TAPTemplateDetail> ManageSequence (List<TAPTemplateDetail> oTAPTemplateDetails)
        {
            List<TAPTemplateDetail> oNewTAPTemplateDetails = new List<TAPTemplateDetail>();
            List<TAPTemplateDetail> oParentStepList = new List<TAPTemplateDetail>();
            oParentStepList = oTAPTemplateDetails.Where(p => p.OrderStepParentID == 1).OrderBy(x => x.Sequence).ToList();//find parent steps
            
            foreach (TAPTemplateDetail oItem in oParentStepList)
            {
                oNewTAPTemplateDetails.Add(oItem);
                _oTAPTemplateDetails = new List<TAPTemplateDetail>();
                _oTAPTemplateDetails = oTAPTemplateDetails.Where(x => x.OrderStepParentID == oItem.OrderStepID).OrderBy(x => x.Sequence).ToList();//find list for a single parent
                int nChildSteps = _oTAPTemplateDetails.Count;
                foreach (TAPTemplateDetail oChildItem in _oTAPTemplateDetails)
                {
                    oNewTAPTemplateDetails.Add(oChildItem);//push detail sequence
                }
            }

            return oNewTAPTemplateDetails;
        }
        #endregion
        private IEnumerable<TAPTemplateDetail> GetChild(int nOrderStepID)
        {
            List<TAPTemplateDetail> oTAPTemplateDetails = new List<TAPTemplateDetail>();
            foreach (TAPTemplateDetail oItem in _oTAPTemplateDetails)
            {
                if (oItem.OrderStepParentID == nOrderStepID)
                {
                    oTAPTemplateDetails.Add(oItem);
                }
            }
            return oTAPTemplateDetails;
        }
        private void AddTreeNodes(ref TAPTemplateDetail oTAPTemplateDetail)
        {
            IEnumerable<TAPTemplateDetail> oChildNodes;
            oChildNodes = GetChild(oTAPTemplateDetail.OrderStepID);
            oTAPTemplateDetail.ChildNodes = oChildNodes;

            foreach (TAPTemplateDetail oItem in oChildNodes)
            {
                TAPTemplateDetail oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private IEnumerable<TTAPTemplateDetail> GetChildren(int nOrderStepID)
        {
            List<TTAPTemplateDetail> oTTAPTemplateDetails = new List<TTAPTemplateDetail>();
            foreach (TTAPTemplateDetail oItem in _oTTAPTemplateDetails)
            {
                if (oItem.parentid == nOrderStepID)
                {
                    oTTAPTemplateDetails.Add(oItem);
                }
            }
            return oTTAPTemplateDetails;
        }

        private void AddTreeNodes(ref TTAPTemplateDetail oTTAPTemplateDetail)
        {
            IEnumerable<TTAPTemplateDetail> oChildNodes;
            oChildNodes = GetChildren(oTTAPTemplateDetail.id);
            oTTAPTemplateDetail.children = oChildNodes;

            foreach (TTAPTemplateDetail oItem in oChildNodes)
            {
                TTAPTemplateDetail oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private TTAPTemplateDetail GetRoot(int nParentID)
        {
            TTAPTemplateDetail oTTAPTemplateDetail = new TTAPTemplateDetail();
            foreach (TTAPTemplateDetail oItem in _oTTAPTemplateDetails)
            {
                if (oItem.parentid == nParentID)
                {
                    return oItem;
                }
            }
            return _oTTAPTemplateDetail;
        }

        private TTAPTemplateDetail MakeTree(List<TAPTemplateDetail> oTAPTemplateDetails)
        {
            _oTTAPTemplateDetails = new List<TTAPTemplateDetail>();
            //SEt Root
            _oTTAPTemplateDetail = new TTAPTemplateDetail();
            _oTTAPTemplateDetail.id = 1;
            _oTTAPTemplateDetail.parentid = 0;
            _oTTAPTemplateDetail.text = "";
            _oTTAPTemplateDetails.Add(_oTTAPTemplateDetail);

            foreach (TAPTemplateDetail oItem in oTAPTemplateDetails)
            {
                _oTTAPTemplateDetail = new TTAPTemplateDetail();
                _oTTAPTemplateDetail.id = oItem.OrderStepID;//Use for Make Tree
                _oTTAPTemplateDetail.parentid = oItem.OrderStepParentID;
                _oTTAPTemplateDetail.TAPTemplateDetailID = oItem.TAPTemplateDetailID;
                _oTTAPTemplateDetail.text = oItem.OrderStepName;
                 _oTTAPTemplateDetail.Sequence = oItem.Sequence;
                _oTTAPTemplateDetail.Remarks = oItem.Remarks;
                _oTTAPTemplateDetail.BeforeShipment = oItem.BeforeShipment;
                _oTTAPTemplateDetail.OrderStepSequence = oItem.OrderStepSequence;
                _oTTAPTemplateDetail.OrderStepID = oItem.OrderStepID;
                _oTTAPTemplateDetail.TAPTemplateID = oItem.TAPTemplateID;
                _oTTAPTemplateDetails.Add(_oTTAPTemplateDetail);
            }
            _oTTAPTemplateDetail = new TTAPTemplateDetail();
            _oTTAPTemplateDetail = GetRoot(0);
            this.AddTreeNodes(ref _oTTAPTemplateDetail);
            return _oTTAPTemplateDetail;
        }
        #endregion


        #region Actions
        public ActionResult ViewTAPTemplates(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TAPTemplate).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oTAPTemplates = new List<TAPTemplate>();
            _oTAPTemplates = TAPTemplate.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oTAPTemplates);
        }

        public ActionResult ViewTAPTemplate(int id)
        {
            _oTAPTemplate = new TAPTemplate();
            _oTAPTemplateDetails = new List<TAPTemplateDetail>();
            _oTTAPTemplateDetails = new List<TTAPTemplateDetail>();
            if (id > 0)
            {
                _oTAPTemplate = _oTAPTemplate.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oTAPTemplateDetails = TAPTemplateDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oTAPTemplateDetails = ManageSequence(_oTAPTemplateDetails);
            }
            _oTAPTemplate.TTAPTemplateDetail = MakeTree(_oTAPTemplateDetails);
            return View(_oTAPTemplate);
        }

        #region Copy TAP Template
        public ActionResult ViewCopyTAPTemplate(int id)
        {
            _oTAPTemplate = new TAPTemplate();
            _oTAPTemplateDetails = new List<TAPTemplateDetail>();
            _oTTAPTemplateDetails = new List<TTAPTemplateDetail>();
            if (id > 0)
            {
                _oTAPTemplate = _oTAPTemplate.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oTAPTemplateDetails = TAPTemplateDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);               
            }
            _oTAPTemplate.TTAPTemplateDetail = MakeTree(ManageSequence(_oTAPTemplateDetails));
            return View(_oTAPTemplate);
        }
        #endregion

        [HttpPost]
        public JsonResult Save(TAPTemplate oTAPTemplate)
        {
            _oTAPTemplate = new TAPTemplate();
            try
            {
                _oTAPTemplate = oTAPTemplate;
                _oTAPTemplate.CreateBy = (int)Session[SessionInfo.currentUserID];
                _oTAPTemplate.CreateDate = DateTime.Today;
               _oTAPTemplate = _oTAPTemplate.Save((int)Session[SessionInfo.currentUserID]);//Save
               _oTAPTemplateDetails = ManageSequence(_oTAPTemplate.TAPTemplateDetails);//manage sequence
               _oTAPTemplate.TTAPTemplateDetail = MakeTree(_oTAPTemplateDetails);
            }
            catch (Exception ex)
            {
                _oTAPTemplate = new TAPTemplate();
                _oTAPTemplate.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPTemplate);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpDown(TAPTemplateDetail oTAPTemplateDetail)
        {
            _oTAPTemplate = new TAPTemplate();
            try
            {
                _oTAPTemplate = _oTAPTemplate.UpDown(oTAPTemplateDetail,(int)Session[SessionInfo.currentUserID]);//Save
                _oTAPTemplateDetails = ManageSequence(_oTAPTemplate.TAPTemplateDetails);//Manage sequence
                _oTAPTemplate.TTAPTemplateDetail = MakeTree(_oTAPTemplateDetails);
            }
            catch (Exception ex)
            {
                _oTAPTemplate = new TAPTemplate();
                _oTAPTemplate.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPTemplate);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSequenceManagedTAPTemplateDetails(TAPTemplate oTAPTemplate)
        {
            List<TAPTemplateDetail> oTAPTemplateDetails = new List<TAPTemplateDetail>();
            try
            {
                oTAPTemplateDetails = ManageSequence(oTAPTemplate.TAPTemplateDetails);
            }
            catch (Exception ex)
            {
                _oTAPTemplateDetail = new TAPTemplateDetail();
                _oTAPTemplateDetail.ErrorMessage = ex.Message;
                oTAPTemplateDetails.Add(_oTAPTemplateDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTAPTemplateDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                TAPTemplate oTAPTemplate = new TAPTemplate();
                sFeedBackMessage = oTAPTemplate.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        #region Package Template Picker
        public ActionResult TAPTemplatePicker(int nTamplateType)
        {
            _oTAPTemplate = new TAPTemplate();
            _oTAPTemplate.TAPTemplates= TAPTemplate.GetsByTemplateType(nTamplateType, (int)Session[SessionInfo.currentUserID]);
            _oTAPTemplate.TAPTemplateDetails = TAPTemplateDetail.GetsByTampleteType(nTamplateType, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oTAPTemplate);
        }


        [HttpPost]
        public JsonResult GetTAPTemplates(TAPTemplate oTAPTemplate)
        {
            List<TAPTemplateDetail> oTAPTemplateDetails = new List<TAPTemplateDetail>();
            try
            {
                _oTAPTemplate = new TAPTemplate();
                _oTAPTemplate.TAPTemplates = TAPTemplate.GetsByTemplateType(oTAPTemplate.TampleteTypeInInt, (int)Session[SessionInfo.currentUserID]);
                _oTAPTemplate.TAPTemplateDetails = TAPTemplateDetail.GetsByTampleteType(oTAPTemplate.TampleteTypeInInt, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTAPTemplate = new TAPTemplate();
                _oTAPTemplate.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAPTemplate);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion 
        #endregion

        //public ActionResult PrintTAPTemplates(string sParam)
        //{
        //    _oTAPTemplate = new TAPTemplate();
        //    string sSQL = "SELECT * FROM View_TAPTemplate WHERE TAPTemplateID IN (" + sParam + ")";
        //    _oTAPTemplate.TAPTemplates = TAPTemplate.Gets((int)Session[SessionInfo.currentUserID]);
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    _oTAPTemplate.Company = oCompany;

        //    string Messge = "TAPTemplate List";
        //    rptTAPTemplates oReport = new rptTAPTemplates();
        //    byte[] abytes = oReport.PrepareReport(_oTAPTemplate, Messge);
        //    return File(abytes, "application/pdf");

        //}

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