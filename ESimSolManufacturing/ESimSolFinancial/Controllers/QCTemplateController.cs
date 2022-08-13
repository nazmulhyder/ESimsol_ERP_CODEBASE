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

    public class QCTemplateController : Controller
    {
        #region Declaration
        QCTemplate _oQCTemplate = new QCTemplate();
        List<QCTemplate> _oQCTemplates = new List<QCTemplate>();
        QCTemplateDetail _oQCTemplateDetail = new QCTemplateDetail();
        List<QCTemplateDetail> _oQCTemplateDetails = new List<QCTemplateDetail>();
        TQCTemplateDetail _oTQCTemplateDetail = new TQCTemplateDetail();
        List<TQCTemplateDetail> _oTQCTemplateDetails = new List<TQCTemplateDetail>();
        
        #endregion

        #region Function
        #region Manage Sequence

        //write by Mahaub
        private List<QCTemplateDetail> ManageSequence (List<QCTemplateDetail> oQCTemplateDetails)
        {
            List<QCTemplateDetail> oNewQCTemplateDetails = new List<QCTemplateDetail>();
            List<QCTemplateDetail> oParentStepList = new List<QCTemplateDetail>();
            oParentStepList = oQCTemplateDetails.Where(p => p.QCStepParentID == 1).OrderBy(x => x.Sequence).ToList();//find parent steps
            
            foreach (QCTemplateDetail oItem in oParentStepList)
            {
                oNewQCTemplateDetails.Add(oItem);
                _oQCTemplateDetails = new List<QCTemplateDetail>();
                _oQCTemplateDetails = oQCTemplateDetails.Where(x => x.QCStepParentID == oItem.QCStepID).OrderBy(x => x.Sequence).ToList();//find list for a single parent
                int nChildSteps = _oQCTemplateDetails.Count;
                foreach (QCTemplateDetail oChildItem in _oQCTemplateDetails)
                {
                    oNewQCTemplateDetails.Add(oChildItem);//push detail sequence
                }
            }

            return oNewQCTemplateDetails;
        }
        #endregion
        private IEnumerable<QCTemplateDetail> GetChild(int nQCStepID)
        {
            List<QCTemplateDetail> oQCTemplateDetails = new List<QCTemplateDetail>();
            foreach (QCTemplateDetail oItem in _oQCTemplateDetails)
            {
                if (oItem.QCStepParentID == nQCStepID)
                {
                    oQCTemplateDetails.Add(oItem);
                }
            }
            return oQCTemplateDetails;
        }
        private void AddTreeNodes(ref QCTemplateDetail oQCTemplateDetail)
        {
            IEnumerable<QCTemplateDetail> oChildNodes;
            oChildNodes = GetChild(oQCTemplateDetail.QCStepID);
            oQCTemplateDetail.ChildNodes = oChildNodes;

            foreach (QCTemplateDetail oItem in oChildNodes)
            {
                QCTemplateDetail oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private IEnumerable<TQCTemplateDetail> GetChildren(int nQCStepID)
        {
            List<TQCTemplateDetail> oTQCTemplateDetails = new List<TQCTemplateDetail>();
            foreach (TQCTemplateDetail oItem in _oTQCTemplateDetails)
            {
                if (oItem.parentid == nQCStepID)
                {
                    oTQCTemplateDetails.Add(oItem);
                }
            }
            return oTQCTemplateDetails;
        }

        private void AddTreeNodes(ref TQCTemplateDetail oTQCTemplateDetail)
        {
            IEnumerable<TQCTemplateDetail> oChildNodes;
            oChildNodes = GetChildren(oTQCTemplateDetail.id);
            oTQCTemplateDetail.children = oChildNodes;

            foreach (TQCTemplateDetail oItem in oChildNodes)
            {
                TQCTemplateDetail oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private TQCTemplateDetail GetRoot(int nParentID)
        {
            TQCTemplateDetail oTQCTemplateDetail = new TQCTemplateDetail();
            foreach (TQCTemplateDetail oItem in _oTQCTemplateDetails)
            {
                if (oItem.parentid == nParentID)
                {
                    return oItem;
                }
            }
            return _oTQCTemplateDetail;
        }

        private TQCTemplateDetail MakeTree(List<QCTemplateDetail> oQCTemplateDetails)
        {
            _oTQCTemplateDetails = new List<TQCTemplateDetail>();
            //SEt Root
            _oTQCTemplateDetail = new TQCTemplateDetail();
            _oTQCTemplateDetail.id = 1;
            _oTQCTemplateDetail.parentid = 0;
            _oTQCTemplateDetail.text = "";
            _oTQCTemplateDetails.Add(_oTQCTemplateDetail);

            foreach (QCTemplateDetail oItem in oQCTemplateDetails)
            {
                _oTQCTemplateDetail = new TQCTemplateDetail();
                _oTQCTemplateDetail.id = oItem.QCStepID;//Use for Make Tree
                _oTQCTemplateDetail.parentid = oItem.QCStepParentID;
                _oTQCTemplateDetail.QCTemplateDetailID = oItem.QCTemplateDetailID;
                _oTQCTemplateDetail.text = oItem.QCStepName;
                 _oTQCTemplateDetail.Sequence = oItem.Sequence;                
                _oTQCTemplateDetail.QCStepSequence = oItem.QCStepSequence;
                _oTQCTemplateDetail.QCStepID = oItem.QCStepID;
                _oTQCTemplateDetail.QCTemplateID = oItem.QCTemplateID;
                _oTQCTemplateDetails.Add(_oTQCTemplateDetail);
            }
            _oTQCTemplateDetail = new TQCTemplateDetail();
            _oTQCTemplateDetail = GetRoot(0);
            this.AddTreeNodes(ref _oTQCTemplateDetail);
            return _oTQCTemplateDetail;
        }
        #endregion


        #region Actions
        public ActionResult ViewQCTemplates(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.QCTemplate).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oQCTemplates = new List<QCTemplate>();
            _oQCTemplates = QCTemplate.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oQCTemplates);
        }

        public ActionResult ViewQCTemplate(int id)
        {
            _oQCTemplate = new QCTemplate();
            _oQCTemplateDetails = new List<QCTemplateDetail>();
            _oTQCTemplateDetails = new List<TQCTemplateDetail>();
            if (id > 0)
            {
                _oQCTemplate = _oQCTemplate.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oQCTemplateDetails = QCTemplateDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oQCTemplateDetails = ManageSequence(_oQCTemplateDetails);
            }
            _oQCTemplate.TQCTemplateDetail = MakeTree(_oQCTemplateDetails);
            return View(_oQCTemplate);
        }

        #region Copy QC Template
        public ActionResult ViewCopyQCTemplate(int id)
        {
            _oQCTemplate = new QCTemplate();
            _oQCTemplateDetails = new List<QCTemplateDetail>();
            _oTQCTemplateDetails = new List<TQCTemplateDetail>();
            if (id > 0)
            {
                _oQCTemplate = _oQCTemplate.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oQCTemplateDetails = QCTemplateDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);               
            }
            _oQCTemplate.TQCTemplateDetail = MakeTree(ManageSequence(_oQCTemplateDetails));
            return View(_oQCTemplate);
        }
        #endregion

        [HttpPost]
        public JsonResult Save(QCTemplate oQCTemplate)
        {
            _oQCTemplate = new QCTemplate();
            try
            {
                _oQCTemplate = oQCTemplate;
                _oQCTemplate.CreateBy = (int)Session[SessionInfo.currentUserID];
                _oQCTemplate.CreateDate = DateTime.Today;
               _oQCTemplate = _oQCTemplate.Save((int)Session[SessionInfo.currentUserID]);//Save
               _oQCTemplateDetails = ManageSequence(_oQCTemplate.QCTemplateDetails);//manage sequence
               _oQCTemplate.TQCTemplateDetail = MakeTree(_oQCTemplateDetails);
            }
            catch (Exception ex)
            {
                _oQCTemplate = new QCTemplate();
                _oQCTemplate.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oQCTemplate);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpDown(QCTemplateDetail oQCTemplateDetail)
        {
            _oQCTemplate = new QCTemplate();
            try
            {
                _oQCTemplate = _oQCTemplate.UpDown(oQCTemplateDetail,(int)Session[SessionInfo.currentUserID]);//Save
                _oQCTemplateDetails = ManageSequence(_oQCTemplate.QCTemplateDetails);//Manage sequence
                _oQCTemplate.TQCTemplateDetail = MakeTree(_oQCTemplateDetails);
            }
            catch (Exception ex)
            {
                _oQCTemplate = new QCTemplate();
                _oQCTemplate.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oQCTemplate);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSequenceManagedQCTemplateDetails(QCTemplate oQCTemplate)
        {
            List<QCTemplateDetail> oQCTemplateDetails = new List<QCTemplateDetail>();
            try
            {
                oQCTemplateDetails = ManageSequence(oQCTemplate.QCTemplateDetails);
            }
            catch (Exception ex)
            {
                _oQCTemplateDetail = new QCTemplateDetail();
                _oQCTemplateDetail.ErrorMessage = ex.Message;
                oQCTemplateDetails.Add(_oQCTemplateDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oQCTemplateDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                QCTemplate oQCTemplate = new QCTemplate();
                sFeedBackMessage = oQCTemplate.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public ActionResult QCTemplatePicker()
        {
            _oQCTemplate = new QCTemplate();
            _oQCTemplate.QCTemplates= QCTemplate.Gets((int)Session[SessionInfo.currentUserID]);
            _oQCTemplate.QCTemplateDetails = QCTemplateDetail.Gets( (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oQCTemplate);
        }

        #endregion 
        #endregion

        //public ActionResult PrintQCTemplates(string sParam)
        //{
        //    _oQCTemplate = new QCTemplate();
        //    string sSQL = "SELECT * FROM View_QCTemplate WHERE QCTemplateID IN (" + sParam + ")";
        //    _oQCTemplate.QCTemplates = QCTemplate.Gets((int)Session[SessionInfo.currentUserID]);
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    _oQCTemplate.Company = oCompany;

        //    string Messge = "QCTemplate List";
        //    rptQCTemplates oReport = new rptQCTemplates();
        //    byte[] abytes = oReport.PrepareReport(_oQCTemplate, Messge);
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