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

    public class QCStepController : Controller
    {
        #region Declaration
        QCStep _oQCStep = new QCStep();
        List<QCStep> _oQCSteps = new List<QCStep>();
        TQCStep _oTQCStep = new TQCStep();
        List<TQCStep> _oTQCSteps = new List<TQCStep>();
        string _sErrorMessage = "";
        #endregion

        #region Function 
        private IEnumerable<QCStep> GetChild(int nQCStepID)
        {
            List<QCStep> oQCSteps = new List<QCStep>();
            foreach (QCStep oItem in _oQCSteps)
            {
                if (oItem.ParentID == nQCStepID)
                {
                    oQCSteps.Add(oItem);
                }
            }
            return oQCSteps;
        }
        private void AddTreeNodes(ref QCStep oQCStep)
        {
            IEnumerable<QCStep> oChildNodes;
            oChildNodes = GetChild(oQCStep.QCStepID);
            oQCStep.ChildNodes = oChildNodes;

            foreach (QCStep oItem in oChildNodes)
            {
                QCStep oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private IEnumerable<TQCStep> GetChildren(int nAccountHeadID)
        {
            List<TQCStep> oTQCSteps = new List<TQCStep>();
            foreach (TQCStep oItem in _oTQCSteps)
            {
                if (oItem.parentid == nAccountHeadID)
                {
                    oTQCSteps.Add(oItem);
                }
            }
            return oTQCSteps;
        }

        private void AddTreeNodes(ref TQCStep oTQCStep)
        {
            IEnumerable<TQCStep> oChildNodes;
            oChildNodes = GetChildren(oTQCStep.id);
            oTQCStep.children = oChildNodes;

            foreach (TQCStep oItem in oChildNodes)
            {
                TQCStep oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private TQCStep GetRoot(int nParentID)
        {
            TQCStep oTQCStep = new TQCStep();
            foreach (TQCStep oItem in _oTQCSteps)
            {
                if (oItem.parentid == nParentID)
                {
                    return oItem;
                }
            }
            return _oTQCStep;
        }

        #endregion

        #region Actions

        public ActionResult ViewQCSteps(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.QCTemplate).ToString() + "," + ((int)EnumModuleName.HIASetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oQCSteps = new List<QCStep>();      
            _oQCStep = new QCStep();
            _oTQCStep = new TQCStep();
            _oTQCSteps = new List<TQCStep>();
            try
            {
                _oQCSteps = QCStep.Gets((int)Session[SessionInfo.currentUserID]);
                foreach (QCStep oItem in _oQCSteps)
                {

                    _oTQCStep = new TQCStep();
                    _oTQCStep.id = oItem.QCStepID;
                    _oTQCStep.parentid = oItem.ParentID;
                    _oTQCStep.text = oItem.QCStepName;
                    _oTQCStep.QCDataTypeInString = oItem.QCDataTypeInString;
                    _oTQCStep.ProductionStepID = oItem.ProductionStepID;
                    _oTQCStep.Sequence = oItem.Sequence;
                    _oTQCStep.ProductionStepName = oItem.ProductionStepName;
                    _oTQCSteps.Add(_oTQCStep);
                }
                _oTQCStep = new TQCStep();
                _oTQCStep = GetRoot(0);
                this.AddTreeNodes(ref _oTQCStep);                
                return View(_oTQCStep);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oTQCStep);
            }

        }
      

        public ActionResult ViewQCStep(int id)
        {
            _oQCStep = new QCStep();
            if (id > 0)
            {
                _oQCStep = _oQCStep.Get(id, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "SELECT * FROM View_QCStep WHERE ParentID = "+_oQCStep.QCStepID+" Order By Sequence";
                _oQCStep.ChildQCSteps = QCStep.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oQCStep.ChildQCSteps = new List<QCStep>();
            }
            ViewBag.ProductionSteps = ProductionStep.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oQCStep);
        }

      
        [HttpPost]
        public JsonResult Save(QCStep oQCStep)
        {
            _oQCStep = new QCStep();
            try
            {
                _oQCStep = oQCStep;
                _oQCStep = _oQCStep.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oQCStep = new QCStep();
                _oQCStep.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oQCStep);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                QCStep oQCStep = new QCStep();
                sFeedBackMessage = oQCStep.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region QC Step Picker
        public ActionResult QCStepPicker(double ts)
        {
            _oQCSteps = new List<QCStep>();
            string sSQL = "SELECT * FROM QCStep WHERE ParentID = 1 QC By QCStepID";
            _oQCSteps = QCStep.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            return PartialView(_oQCSteps);
        }
        [HttpPost]
        public JsonResult GetQCSteps(QCStep oQCStep)
        {
            _oQCSteps = new List<QCStep>();
            if(oQCStep.Note==null || oQCStep.Note=="")
            {
                _oQCSteps = QCStep.Gets((int)Session[SessionInfo.currentUserID]);
            }else{
                _oQCSteps = QCStep.Gets(oQCStep.Note, (int)Session[SessionInfo.currentUserID]);
            }
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oQCSteps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion


    }
    
 
}