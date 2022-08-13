using System;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Drawing;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ReportManagement;

namespace ESimSolFinancial.Controllers
{
    public class EmployeeCodeController : PdfViewController
    {
        #region Declaration
        EmployeeCode _oEmployeeCode = new EmployeeCode();
        List<EmployeeCode> _oEmployeeCodes = new List<EmployeeCode>();
        
        #endregion
        public ActionResult View_EmployeeCodes(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployeeCodes = new List<EmployeeCode>();
            _oEmployeeCodes = EmployeeCode.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oEmployeeCodes);
        }

        public ActionResult View_EmployeeCode(string sid, string sMsg)
        {
            int nEmpCodeID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            _oEmployeeCode = new EmployeeCode();
            try
            {
                if (nEmpCodeID > 0)
                {
                    _oEmployeeCode = EmployeeCode.Get(nEmpCodeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    string sSql = "SELECT * FROM EmployeeCodeDetail WHERE EmployeeCodeID=" + nEmpCodeID;
                    _oEmployeeCode.EmployeeCodeDetails = EmployeeCodeDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oEmployeeCode = new EmployeeCode();
                _oEmployeeCode.ErrorMessage = ex.Message;
            }
            if (sMsg != "N/A")
            {
                _oEmployeeCode.ErrorMessage = sMsg;
            }
            return View(_oEmployeeCode);
        }

        [HttpPost]
        public JsonResult EmployeeCode_Save(EmployeeCode oEmployeeCode)
        {

            try
            {

                //oEmployeeCode.CompanyID = (int)Session[SessionInfo.CurrentCompanyID]; //oEmployeeCode.CompanyID;
                oEmployeeCode.EmployeeCodeDetailsInString = GetEmployeeCodeDetailsInString(oEmployeeCode.EmployeeCodeDetails);
                if (oEmployeeCode.EmployeeCodeID > 0)
                {
                    oEmployeeCode = oEmployeeCode.IUD((int)EnumDBOperation.Update,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oEmployeeCode = oEmployeeCode.IUD((int)EnumDBOperation.Insert,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oEmployeeCode = new EmployeeCode();
                oEmployeeCode.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeCode);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetEmployeeCodeDetailsInString(List<EmployeeCodeDetail> oEmployeeCodeDetails)
        {
            string sEmployeeCodesInString = "";
            if (oEmployeeCodeDetails != null)
            {
                foreach (EmployeeCodeDetail oItem in oEmployeeCodeDetails)
                {
                    sEmployeeCodesInString = sEmployeeCodesInString + oItem.ECDID.ToString() + "," + oItem.EmployeeCodeID.ToString() + "," + (oItem.ECDTypeInInt).ToString() + "," + oItem.Value.ToString() + "," + oItem.Length.ToString() + "," + (oItem.RestartInInt).ToString() + "," + oItem.Sequence.ToString() + "~";
                }
                if (sEmployeeCodesInString.Length > 0)
                {
                    sEmployeeCodesInString = sEmployeeCodesInString.Remove(sEmployeeCodesInString.Length - 1, 1);
                }
            }
            return sEmployeeCodesInString;
        }

        [HttpPost]
        public JsonResult EmployeeCode_Delete(EmployeeCode oEmployeeCode)
        {
            try
            {
                oEmployeeCode.EmployeeCodeDetailsInString = "";
                oEmployeeCode = oEmployeeCode.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oEmployeeCode = new EmployeeCode();
                oEmployeeCode.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeCode.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
}