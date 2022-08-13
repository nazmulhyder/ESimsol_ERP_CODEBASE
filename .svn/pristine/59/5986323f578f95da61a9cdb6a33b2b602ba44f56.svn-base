using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class ChequeSetupController : Controller
    {
        #region Declaration
        ChequeSetup _oChequeSetup = new ChequeSetup();
        List<ChequeSetup> _oChequeSetups = new List<ChequeSetup>();
        private string _sSQL = "";
        string _sErrorMessage = "";
        #endregion
        #region Functions
        private bool ValidateInput(ChequeSetup oChequeSetup)
        {
            if (oChequeSetup.ChequeSetupName == null || oChequeSetup.ChequeSetupName == "")
            {
                _sErrorMessage = "Please enter ChequeSetup Name";
                return false;
            }
            Double L = oChequeSetup.Length;
            Double W = oChequeSetup.Width;
            if (L == null || L == 0 || W == null || W == 0 || L < W)
            {
                _sErrorMessage = "Ivalid Leaf size";
                return false;
            }
            if (oChequeSetup.PaymentMethodX == null || oChequeSetup.PaymentMethodX == 0 || oChequeSetup.PaymentMethodX >=L-0.5)
            {
                _sErrorMessage = "invalid PaymentMethod Position";
                return false;
            }
            if (oChequeSetup.paymentMethodY == null || oChequeSetup.paymentMethodY == 0 || oChequeSetup.paymentMethodY >= W - 0.5)
            {
                _sErrorMessage = "invalid PaymentMethod Position";
                return false;
            }
            if (oChequeSetup.DateX == null || oChequeSetup.DateX == 0 || oChequeSetup.DateX >= L - 0.5)
            {
                _sErrorMessage = "invalid Date Position";
                return false;
            }
            if (oChequeSetup.DateY == null || oChequeSetup.DateY == 0 || oChequeSetup.DateY >= W - 0.5)
            {
                _sErrorMessage = "invalid Date Position";
                return false;
            }
            if (oChequeSetup.PayToX == null || oChequeSetup.PayToX == 0 || oChequeSetup.PayToX >= L - 0.5)
            {
                _sErrorMessage = "invalid PayTo Position";
                return false;
            }
            if (oChequeSetup.PayToY == null || oChequeSetup.PayToY == 0 || oChequeSetup.PayToY >= W - 0.5)
            {
                _sErrorMessage = "invalid PayTo Position";
                return false;
            }
            if (oChequeSetup.AmountWordX == null || oChequeSetup.AmountWordX == 0 || oChequeSetup.AmountWordX >= L - 0.5)
            {
                _sErrorMessage = "invalid AmountWord Position";
                return false;
            }
            if (oChequeSetup.AmountWordY == null || oChequeSetup.AmountWordY == 0 || oChequeSetup.AmountWordY >= W - 0.5)
            {
                _sErrorMessage = "invalid AmountWord Position";
                return false;
            }
            if (oChequeSetup.AmountX == null || oChequeSetup.AmountX == 0 || oChequeSetup.AmountX >= L - 0.5)
            {
                _sErrorMessage = "invalid Amount Position";
                return false;
            }
            if (oChequeSetup.AmountY == null || oChequeSetup.AmountY == 0 || oChequeSetup.AmountY >= W - 0.5)
            {
                _sErrorMessage = "invalid Amount Position";
                return false;
            }

            return true;
        }
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM ChequeSetup";
            string sChequeSetupName = Arguments.Split(';')[1].Split('~')[0];

            string sSQL = "";


            #region ChequeSetupName
            if (sChequeSetupName != null)
            {
                if (sChequeSetupName != "")
                {
                    if (sChequeSetupName != "Search by ChequeSetup Name")
                    {
                        Global.TagSQL(ref sSQL);
                        sSQL = sSQL + " ChequeSetupName LIKE '%" + sChequeSetupName + "%' ";
                    }
                }
            }
            #endregion
            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }
        }
        #endregion

        #region Actions

        public ActionResult ViewChequeSetups(int menuid)
        {

            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ChequeSetup).ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID, ((User)Session[SessionInfo.CurrentUser]).UserID));
            _oChequeSetups = new List<ChequeSetup>();
            _oChequeSetups = ChequeSetup.GetsWithoutImage(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oChequeSetups);
        }

        public ActionResult ViewChequeSetup(int nid, string sMsg,bool bcopy,bool bsub) // ChequeSetupID
        {
            _oChequeSetup = new ChequeSetup();
            if (nid > 0)
            {
                _oChequeSetup = _oChequeSetup.Get(nid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (bcopy) { _oChequeSetup.ChequeSetupID = 0; _oChequeSetup.ChequeSetupName = ""; }
                if (bsub) { _oChequeSetup.IsSubmitted = true; }
            }
            
            if (sMsg != "N/A")
            {
                _oChequeSetup.ErrorMessage = sMsg;
            }
            
            return PartialView(_oChequeSetup);
        }
        public ActionResult ViewLetterSetupForTest(int menuid) // ChequeSetupID
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oChequeSetup = new ChequeSetup();

            List<string> fixedList = new List<string>();
            fixedList.Add("Name");
            fixedList.Add("Designation");
            fixedList.Add("Grade");
            fixedList.Add("Card No");
            fixedList.Add("Fathers Name");
            ViewBag.FixedList = fixedList;

            return View(_oChequeSetup);
        }
        

        [HttpPost]
        public JsonResult Refresh(ChequeSetup oChequeSetup)
        {
            this.MakeSQL(oChequeSetup.ErrorMessage);
            _oChequeSetups = new List<ChequeSetup>();
            _oChequeSetups = ChequeSetup.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChequeSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Save(HttpPostedFileBase file, ChequeSetup oChequeSetup)
        {
            _oChequeSetup = new ChequeSetup();
            try
            {
                oChequeSetup.ChequeSetupName = oChequeSetup.ChequeSetupName == null ? "" : oChequeSetup.ChequeSetupName;
                oChequeSetup.DateFormat = oChequeSetup.DateFormat == null ? "" : oChequeSetup.DateFormat;
                if (!this.ValidateInput(oChequeSetup))
                {
                    throw new Exception(_sErrorMessage);
                }
                _oChequeSetup = oChequeSetup;

                MemoryStream oMemoryStream = new MemoryStream();
                if (file != null && file.ContentLength > 0)
                {
                    Image oImage = Image.FromStream(file.InputStream, true, true);
                    oImage.Save(oMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    _oChequeSetup.ChequeImageInByte = oMemoryStream.GetBuffer();
                }
                
                _oChequeSetup = _oChequeSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oChequeSetup = new ChequeSetup();
                _oChequeSetup.ErrorMessage = ex.Message;
            }

            return RedirectToAction("ViewChequeSetup", new { nid = _oChequeSetup.ChequeSetupID, sMsg = "N/A", bcopy = false, bsub = true });
        }

        [HttpGet]
        public JsonResult Delete(int id, double ts)
        {
            string sFeedBackMessage = "";
            try
            {
                ChequeSetup oChequeSetup = new ChequeSetup();
                sFeedBackMessage = oChequeSetup.Delete(id,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public Image GetDBChequeImage(int id)
        {
            #region From DB
            ChequeSetup oChequeSetup = new ChequeSetup();
            oChequeSetup = oChequeSetup.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oChequeSetup.ChequeImageInByte != null)
            {
                MemoryStream m = new MemoryStream(oChequeSetup.ChequeImageInByte);
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

    }
        #endregion
}