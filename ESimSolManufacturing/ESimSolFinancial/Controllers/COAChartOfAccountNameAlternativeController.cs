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

namespace ESimSolFinancial.Controllers
{
    public class COAChartOfAccountNameAlternativeController : Controller
    {
        #region Declaration
        COAChartOfAccountNameAlternative _oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
        List<COAChartOfAccountNameAlternative> _COAChartOfAccountNameAlternatives = new List<COAChartOfAccountNameAlternative>();
        string _sErrorMessage = "";
        #endregion
        #region Functions
        private bool ValidateInput(COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative)
        {
            if (oCOAChartOfAccountNameAlternative.AlternativeAccountHeadID < 0)
            {
                _sErrorMessage = "Nothing to Insert";
                return false;
            }

            if (oCOAChartOfAccountNameAlternative.AccountHeadID < 0)
            {
                _sErrorMessage = "Please Select Alternative Name";
                return false;
            }

            if (oCOAChartOfAccountNameAlternative.Name == "" || oCOAChartOfAccountNameAlternative.Name == null)
            {
                _sErrorMessage = "Please Provide Name";
                return false;
            }

            if (oCOAChartOfAccountNameAlternative.Description == "" || oCOAChartOfAccountNameAlternative.Description == null)
            {
                _sErrorMessage = "Please Provide Use For";
                return false;
            }
            return true;
        }
        #endregion
            
        public ActionResult RefreshList( int ParentID, string sSearch)
        {
            //if (ParentID <= 0)
            //{
            //  //  return ParentID Message Required
            //    return RedirectToAction("RefreshList", "ChartsOfAccount");
            //}
            //COA_ChartsOfAccount oCOA = new COA_ChartsOfAccount();
            //oCOA = oCOA.Get(ParentID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //if (oCOA.IsJVNode != true)
            //{
            //    TempData["message"] = "Select a Ledger Please";
            //    return RedirectToAction("RefreshList", "ChartsOfAccount");                
            //}
            
            //TempData["message"] = "Selected parent account head : " + oCOA.AccountHeadNameCode;
            //_COAChartOfAccountNameAlternatives = new List<COAChartOfAccountNameAlternative>();

            //COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
            //oCOAChartOfAccountNameAlternative.AccountHeadID = ParentID;
            //if (sSearch == null || sSearch == "")
            //{
            //    oCOAChartOfAccountNameAlternative.COAChartOfAccountNameAlternatives = COAChartOfAccountNameAlternative.GetsbyParentID(ParentID, ((User)Session[SessionInfo.CurrentUser]).UserID);            

            //}
            //else
            //{
            //    oCOAChartOfAccountNameAlternative.COAChartOfAccountNameAlternatives = COAChartOfAccountNameAlternative.SearchbyAlternativeName(sSearch, ParentID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //}
            // _COAChartOfAccountNameAlternatives = COAChartOfAccountNameAlternative.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            
            return View();
           
        }
      
        [HttpGet]
        public JsonResult AddAlternativeAccountHead(int AccountHeadID, string Name, string Description)
        {
            COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
            List<COAChartOfAccountNameAlternative> oCOAChartOfAccountNameAlternatives = new List<COAChartOfAccountNameAlternative>();
            try
            {
                oCOAChartOfAccountNameAlternative.AccountHeadID = AccountHeadID;
                oCOAChartOfAccountNameAlternative.Name = Name;
                oCOAChartOfAccountNameAlternative.Description = Description;

                // edit by me

                int id = AccountHeadID;

                if (this.ValidateInput(oCOAChartOfAccountNameAlternative))
                {
                    oCOAChartOfAccountNameAlternative = oCOAChartOfAccountNameAlternative.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oCOAChartOfAccountNameAlternative.ErrorMessage == "" || oCOAChartOfAccountNameAlternative.ErrorMessage == null)
                    {
                        oCOAChartOfAccountNameAlternatives = COAChartOfAccountNameAlternative.GetsByAccountHeadID(oCOAChartOfAccountNameAlternative.AccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        _sErrorMessage = oCOAChartOfAccountNameAlternative.ErrorMessage.Split('!')[0];
                    }
                }
            }
            catch (Exception ex)
            {
                _sErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = "";
            if (_sErrorMessage == "" || _sErrorMessage == null)
            {
                sjson = serializer.Serialize(oCOAChartOfAccountNameAlternatives);
            }
            else
            {
                oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
                oCOAChartOfAccountNameAlternative.ErrorMessage = _sErrorMessage;
                oCOAChartOfAccountNameAlternatives.Add(oCOAChartOfAccountNameAlternative);
                sjson = serializer.Serialize(oCOAChartOfAccountNameAlternatives);
            }
            
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            try
            {
                COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
                sErrorMease = oCOAChartOfAccountNameAlternative.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCOAChartOfAccountNameAlternativeDetails(string keyes)
        {
            COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
            List<COAChartOfAccountNameAlternative> oCOAChartOfAccountNameAlternatives = new List<COAChartOfAccountNameAlternative>();
            oCOAChartOfAccountNameAlternatives = COAChartOfAccountNameAlternative.Getsjason(oCOAChartOfAccountNameAlternative.AlternativeAccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oCOAChartOfAccountNameAlternatives);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetforDocIssue(int DocumentBookID)
        {
            //string sErrorMease = "";
            int id = DocumentBookID;
            List<COAChartOfAccountNameAlternative> oCOAChartOfAccountNameAlternatives = new List<COAChartOfAccountNameAlternative>();
            try
            {
                COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
                string sSQL = "SELECT * FROM COA_ChartOfAccountNameAlternative WHERE AccountHeadID IN (SELECT AccountHeadID FROM COA_ChartsOfAccount WHERE IsDynamic=1 AND IsJVNode =1 AND ReferenceObjectID = " + id + " AND DAHCID IN (SELECT DAHCID FROM COA_DynamicAccountHeadConfigure WHERE TableName = 'BankAccount'))";
                oCOAChartOfAccountNameAlternatives = COAChartOfAccountNameAlternative.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                // oCostCenters = oCostCenter.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                // oCostCenters = oCostCenter.GetsforPOP(oCostCenter.ID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _sErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCOAChartOfAccountNameAlternatives);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //alternativename save for document issue
        [HttpGet]
        public JsonResult SaveAltName(int AccountHeadID, string Name, string Description)
        {
            COAChartOfAccountNameAlternative oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
            List<COAChartOfAccountNameAlternative> oCOAChartOfAccountNameAlternatives = new List<COAChartOfAccountNameAlternative>();
            try
            {
                oCOAChartOfAccountNameAlternative.AccountHeadID = AccountHeadID;
                oCOAChartOfAccountNameAlternative.Name = Name;
                oCOAChartOfAccountNameAlternative.Description = Description;
                int id = AccountHeadID;

                if (this.ValidateInput(oCOAChartOfAccountNameAlternative))
                {
                    oCOAChartOfAccountNameAlternative = oCOAChartOfAccountNameAlternative.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oCOAChartOfAccountNameAlternative.ErrorMessage == "" || oCOAChartOfAccountNameAlternative.ErrorMessage == null)
                    {
                        oCOAChartOfAccountNameAlternatives = COAChartOfAccountNameAlternative.GetsByAccountHeadID(oCOAChartOfAccountNameAlternative.AccountHeadID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        _sErrorMessage = oCOAChartOfAccountNameAlternative.ErrorMessage.Split('!')[0];
                    }
                }
            }
            catch (Exception ex)
            {
                _sErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = "";
            if (_sErrorMessage == "" || _sErrorMessage == null)
            {
                sjson = serializer.Serialize(oCOAChartOfAccountNameAlternatives);
            }
            else
            {
                oCOAChartOfAccountNameAlternative = new COAChartOfAccountNameAlternative();
                oCOAChartOfAccountNameAlternative.ErrorMessage = _sErrorMessage;
                oCOAChartOfAccountNameAlternatives.Add(oCOAChartOfAccountNameAlternative);
                sjson = serializer.Serialize(oCOAChartOfAccountNameAlternatives);
            }

            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


    }
}
