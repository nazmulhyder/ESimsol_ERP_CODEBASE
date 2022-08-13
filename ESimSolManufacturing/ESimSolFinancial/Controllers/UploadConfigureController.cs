using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class UploadConfigureController : Controller
    {
        #region Declaration
        UploadConfigure _oUploadConfigure = new UploadConfigure();
        List<UploadConfigure> _oUploadConfigures = new List<UploadConfigure>();
        string _sErrorMessage = "";
        #endregion

        #region Actions
        public ActionResult View_UploadConfigure(int nUploadType)
        {
            _oUploadConfigures = new List<UploadConfigure>();
            UploadConfigure oUploadConfigure = new UploadConfigure();

            _oUploadConfigures = MakeList(nUploadType);

            ViewBag.SelectedOptions = _oUploadConfigure.Get(nUploadType, (int)Session[SessionInfo.currentUserID]);

            return View(_oUploadConfigures);
        }
        #endregion

        private List<UploadConfigure> MakeList(int nUploadType)
        {
            List<UploadConfigure> oUploadConfigures = new List<UploadConfigure>();
            UploadConfigure oUploadConfigure = new UploadConfigure();
            Dictionary<string, string> sTableCaptions = new Dictionary<string, string>();

            if (nUploadType == 1)
            {
                sTableCaptions.Add("Code", "Code");
                sTableCaptions.Add("Name", "Name");
                sTableCaptions.Add("FathersName", "FatherName");
                sTableCaptions.Add("HusbandName", "HusbandName");
                sTableCaptions.Add("MothersName", "MotherName");
                sTableCaptions.Add("Date of Birth", "DateOfBirth");
                sTableCaptions.Add("Gender", "Gender");
                sTableCaptions.Add("ContactNo", "ContactNo");
                sTableCaptions.Add("Email", "Email");
                sTableCaptions.Add("BloodGroup", "BloodGroup");
                sTableCaptions.Add("NationalID", "NationalID");
                sTableCaptions.Add("PresentAddress", "PresentAddress");
                sTableCaptions.Add("PermanentAddress", "PermanentAddress");
                sTableCaptions.Add("Village", "Village");
                sTableCaptions.Add("PostOffice", "PostOffice");
                sTableCaptions.Add("Thana", "Thana");
                sTableCaptions.Add("District", "District");
                sTableCaptions.Add("PostCode", "PostCode");
                sTableCaptions.Add("Marital Status", "MaritalStatus");
                //sTableCaptions.Add("Address", "PresentAddress");
                sTableCaptions.Add("Date of join", "DateOfJoin");
                sTableCaptions.Add("Date of Confirmation", "DateOfConfirmation");
                sTableCaptions.Add("EmployeeType", "EmployeeType");
                sTableCaptions.Add("Bank Code", "BankCode");
                sTableCaptions.Add("AccNo", "AccountNo");
                sTableCaptions.Add("Bank Amount", "BankAmount");
                sTableCaptions.Add("Cash Amount", "CashAmount");
                sTableCaptions.Add("Proximity Card No", "CardNo");
                sTableCaptions.Add("Block", "Block");
                sTableCaptions.Add("Group", "Group");
                sTableCaptions.Add("TIN", "TIN");
                sTableCaptions.Add("ETIN", "ETIN");

                //sTableCaptions.Add("Employee Category", "Category");

                //sTableCaptions.Add("Reporting Person", "ReportingPersonCode");
                #region Bangla
                sTableCaptions.Add("Code Bangla", "Code_Bangla");
                sTableCaptions.Add("Name In Bangla", "Name_InBangla");
                sTableCaptions.Add("Father Name Bangla", "Father_NameBangla");
                sTableCaptions.Add("Mother Name Bangla", "Mother_NameBangla");
                sTableCaptions.Add("Nationality Bangla", "Nationality_Bangla");
                sTableCaptions.Add("National ID Bangla", "National_IDBangla");
                sTableCaptions.Add("Blood Group Bangla", "Blood_GroupBangla");
                sTableCaptions.Add("Height Bangla", "Height_Bangla");
                sTableCaptions.Add("Weight Bangla", "Weight_Bangla");
                sTableCaptions.Add("District Bangla", "District_BanglaName");
                sTableCaptions.Add("Thana Bangla", "Thana_BanglaName");
                sTableCaptions.Add("Post Office Bangla", "Post_OfficeBangla");
                sTableCaptions.Add("Village Bangla", "Village_Name_Bangla");
                sTableCaptions.Add("Present Address Bangla", "Present_AddressBangla");
                sTableCaptions.Add("Perm District Bangla", "Perm_District_Bangla");
                sTableCaptions.Add("Perm Thana Bangla", "Perm_Thana_Bangla");
                sTableCaptions.Add("Perm Post Office Bangla", "Perm_Post_Office_Bangla");
                sTableCaptions.Add("Perm Village Bangla", "Perm_Village_Bangla");
                sTableCaptions.Add("Permanent Address Bangla", "Permanent_AddressBangla");
                sTableCaptions.Add("Religion Bangla", "Religion_Bangla");
                sTableCaptions.Add("Marital Status Bangla", "Marital_Status_Bangla");
                sTableCaptions.Add("Nominee Bangla", "Nominee_Bangla");
                sTableCaptions.Add("Authentication Bangla", "Authentication_Bangla");
                #endregion
            }
            foreach (var oItem in sTableCaptions)
            {
                oUploadConfigure = new UploadConfigure();
                oUploadConfigure.TableCaption = oItem.Key;
                oUploadConfigure.FieldName = oItem.Value;
                oUploadConfigures.Add(oUploadConfigure);
            }

            return oUploadConfigures;
        }

        [HttpPost]
        public JsonResult Save(UploadConfigure oUploadConfigure)
        {
            _oUploadConfigure = new UploadConfigure();
            try
            {
                _oUploadConfigure = oUploadConfigure;
                _oUploadConfigure.UploadType = (EnumUploadType)oUploadConfigure.UploadTypeInInt;
                _oUploadConfigure.UserID = ((User)Session[SessionInfo.CurrentUser]).UserID;
                _oUploadConfigure = _oUploadConfigure.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oUploadConfigure = new UploadConfigure();
                _oUploadConfigure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oUploadConfigure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //GetUploadConfigure

        [HttpPost]
        public JsonResult GetUploadConfigure(UploadConfigure oUploadConfigure)
        {
            _oUploadConfigure = new UploadConfigure();
            try
            {
                #region common Get
                _oUploadConfigure = _oUploadConfigure.Get(oUploadConfigure.UploadTypeInInt, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #endregion
            }
            catch (Exception ex)
            {
                _oUploadConfigure = new UploadConfigure();
                _oUploadConfigure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oUploadConfigure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                UploadConfigure oUploadConfigure = new UploadConfigure();
                sFeedBackMessage = oUploadConfigure.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
}
