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
    public class PaymentTermController : Controller
    {
        #region Declaration
        PaymentTerm _oPaymentTerm = new PaymentTerm();
        List<PaymentTerm> _oPaymentTerms = new List<PaymentTerm>();

        TLocation _oTLocation = new TLocation();
        List<TLocation> _oTLocations = new List<TLocation>();
        List<Location> _oLocations = new List<Location>();

      
        string _sErrorMessage = "";
        string _sSQL = "";
        #endregion
        

        #region Used Code
        public ActionResult ViewPaymentTerms(int buid, int menuid)
        {   
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PaymentTerm).ToString(), (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));
            _oPaymentTerms = new List<PaymentTerm>();
            _oPaymentTerms = PaymentTerm.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oPaymentTerms);
        }
        
        public ActionResult ViewPaymentTerm(int id)
        {
            _oPaymentTerm = new PaymentTerm();
            if (id > 0)
            {
             _oPaymentTerm = _oPaymentTerm.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.POPaymentTypeObj = EnumObject.jGets(typeof(EnumPOPaymentType));
            ViewBag.DayApplyTypes = EnumObject.jGets(typeof(EnumDayApplyType));;
            ViewBag.DisplayParts = EnumObject.jGets(typeof(EnumDisplayPart));
            ViewBag.PaymentTermTypes = EnumObject.jGets(typeof(EnumPaymentTermType));   
            return View(_oPaymentTerm);
        }
     

        [HttpPost]
        public JsonResult Get(PaymentTerm oPaymentTerm)
        {
            _oPaymentTerm = new PaymentTerm();
            try
            {
                if (oPaymentTerm.PaymentTermID > 0)
                {
                    _oPaymentTerm = _oPaymentTerm.Get(oPaymentTerm.PaymentTermID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oPaymentTerm = new PaymentTerm();
                _oPaymentTerm.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPaymentTerm);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

      
        public Image GetDBCompanyLogo(int id)
        {
            #region From DB
            Company oCompany = new Company();
            oCompany = oCompany.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
            #endregion
        }


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

        #endregion

        public ActionResult ViewPaymentTermPicker(string sName, double nts)
        {
            _oPaymentTerms = new List<PaymentTerm>();
            _oPaymentTerm = new PaymentTerm();
            string sSql = "";
            try
            {
                if (string.IsNullOrEmpty(sName))
                {
                     sSql = "SELECT * FROM PaymentTerm order by Name ";
                }
                else
                {
                     sSql = "SELECT * FROM PaymentTerm WHERE Name LIKE '%" + sName + "%'";
                }
               // _oPaymentTerms = PaymentTerm.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oPaymentTerms.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oPaymentTerms = new List<PaymentTerm>();
                _oPaymentTerm.ErrorMessage = ex.Message;
                _oPaymentTerms.Add(_oPaymentTerm);
            }
            return PartialView(_oPaymentTerms);
        }

        [HttpPost]
        public JsonResult Save(PaymentTerm oPaymentTerm)
        {
            _oPaymentTerm = new PaymentTerm();
            //oPaymentTerm.Name = oPaymentTerm.Name == null ? "" : oPaymentTerm.Name;
          
            try
            {
                _oPaymentTerm = oPaymentTerm;
                _oPaymentTerm = _oPaymentTerm.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPaymentTerm = new PaymentTerm();
                _oPaymentTerm.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPaymentTerm);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(PaymentTerm oPaymentTerm)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oPaymentTerm.Delete(oPaymentTerm.PaymentTermID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetPaymentTerms()
        {
            List<PaymentTerm> oPaymentTerms = new List<PaymentTerm>();
            PaymentTerm oPaymentTerm = new PaymentTerm();
            //oPaymentTerm.Name = "-- Select PaymentTerm --";
            oPaymentTerms.Add(oPaymentTerm);
            oPaymentTerms.AddRange(PaymentTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID));
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oPaymentTerms);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPaymentTermsForInvoice(PaymentTerm oPaymentTerm)
        {            
             _sSQL = "Select * from PaymentTerm Where BUID = "+oPaymentTerm.BUID;
            if(oPaymentTerm.TermText!="" && oPaymentTerm.TermText!=null)
            {
                _sSQL += " AND TermText LIKE '%" + oPaymentTerm.TermText + "%'";
            }
           
            List<PaymentTerm> oPaymentTerms = new List<PaymentTerm>();
            oPaymentTerms = PaymentTerm.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oPaymentTerms);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        private string MakeSQL(PaymentTerm oPaymentTerm)
        {

            string sReturn1 = "SELECT * FROM PaymentTerm";
            string sReturn = "";

            #region 

            if (oPaymentTerm.Percentage>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Percentage =" + oPaymentTerm.Percentage.ToString() + "";
            }
            #endregion

            #region TermText
            //if (!String.IsNullOrEmpty(oPaymentTerm.TermText))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "TermText like'%" + oPaymentTerm.TermText + "%'";
            //}
            #endregion
            #region DayApplyType
            if (oPaymentTerm.DayApplyTypeint>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DayApplyType=" + oPaymentTerm.DayApplyTypeint + "";
            }
            #endregion
            #region DayApplyType
            if (oPaymentTerm.Days > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "Days=" + oPaymentTerm.Days;
            }
            #endregion

            #region DateDisplayPartint
            if (oPaymentTerm.DateDisplayPart > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DateDisplayPart=" + oPaymentTerm.DateDisplayPartint + "";
            }
            #endregion
            #region DateDisplayPartint
            if (oPaymentTerm.PaymentTermTypeInt > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "PaymentTermType=" + oPaymentTerm.PaymentTermTypeInt + "";
            }
            #endregion
            #region TermText
            //if (!String.IsNullOrEmpty(oPaymentTerm.DateText))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "DateText like'%" + oPaymentTerm.DateText + "%'";
            //}
            #endregion
           
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
     
        [HttpPost]
        public JsonResult SearchPaymentTerm(PaymentTerm oPaymentTerm)
        {
            string _sSQL = "";
            List<PaymentTerm> oPaymentTerms = new List<PaymentTerm>();
            oPaymentTerm.ErrorMessage = oPaymentTerm.ErrorMessage == null ? "" : oPaymentTerm.ErrorMessage;
            _sSQL=this.MakeSQL(oPaymentTerm);

            oPaymentTerms = PaymentTerm.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oPaymentTerms);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
}
