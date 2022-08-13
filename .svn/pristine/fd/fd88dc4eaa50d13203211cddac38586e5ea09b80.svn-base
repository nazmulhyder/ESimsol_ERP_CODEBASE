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
using ReportManagement;
using OfficeOpenXml;
using OfficeOpenXml.Style;
namespace ESimSolFinancial.Controllers
{
    public class MarketingAccountController : Controller
    {
        #region Declaration
        MarketingAccount _oMarketingAccount = new MarketingAccount();
        List<MarketingAccount> _oMarketingAccounts = new List<MarketingAccount>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();


        int _nRowIndex = 2;
        ExcelRange _cell;
        ExcelFill _fill;
        OfficeOpenXml.Style.Border _border;
        #endregion


        #region MarketingAccount
        public ActionResult ViewMarketingAccounts(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oMarketingAccounts = new List<MarketingAccount>();
            //if (buid <= 0)
            //{
            //    _oMarketingAccounts = MarketingAccount.Gets((int)Session[SessionInfo.currentUserID]);
            //}
            //else
            //{
            //    _oMarketingAccounts = MarketingAccount.GetsByBU(buid, (int)Session[SessionInfo.currentUserID]);
            //}

            ViewBag.BUID = buid;
            ViewBag.OrderType = EnumObject.jGets(typeof(EnumFabricRequestType));
            return View(_oMarketingAccounts);
        }
        #endregion

        [HttpPost]
        public JsonResult MktSaleTargetSave(MktSaleTarget oMktSaleTarget)
        {
            MktSaleTarget _oMktSaleTarget = new MktSaleTarget();
            try
            {
                _oMktSaleTarget = oMktSaleTarget;
                _oMktSaleTarget = _oMktSaleTarget.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMktSaleTarget = new MktSaleTarget();
                _oMktSaleTarget.ErrorMessage = ex.Message;
            }
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMktSaleTarget);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region MKT Projection
      
        public ActionResult ViewMktProjectionSetup(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.MktProjectionReport).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
            string sSQL = "SELECT * FROM View_MktSaleTarget WHERE MktSaleTargetID >0 Order By MktSaleTargetID";
            _oMktSaleTargets = MktSaleTarget.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.OrderType = EnumObject.jGets(typeof(EnumFabricRequestType));
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            return View(_oMktSaleTargets);
        }
        [HttpPost]
        public JsonResult GetsAllMktPerson(MarketingAccount_BU oMarketingAccount)
        {
            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            try
            {

                string sSQL = "SELECT * FROM View_MarketingAccount WHERE MarketingAccountID >0";
                if (!string.IsNullOrEmpty(oMarketingAccount.Name))
                {
                    sSQL += " AND Name LIKE'%" + oMarketingAccount.Name + "%'";
                }
                //if (oMarketingAccount.BUID > 0)
                //{
                //    sSQL += " AND MarketingAccountID in (Select MarketingAccountID from MarketingAccount_BU where BUID  in (" + oMarketingAccount.BUID + "))";
                //}
                sSQL += " and IsGroup=1   Order by Name";
                oMarketingAccounts = MarketingAccount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oMarketingAccounts = new List<MarketingAccount>();
                oMarketingAccounts.Add(new MarketingAccount() { ErrorMessage = ex.Message });
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            oMarketingAccounts = oMarketingAccounts.OrderBy(x => x.GroupName).ToList();
            string sjson = serializer.Serialize(oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }       
        [HttpPost]
        public JsonResult SearchByMktHead(MktSaleTarget oMktSaleTarget)
        {
           List<MktSaleTarget> _oMktSaleTargets = new List<MktSaleTarget>();
           MktSaleTarget _oMktSaleTarget = new MktSaleTarget();
            try
            {
                string sSQL = "SELECT * FROM View_MktSaleTarget ";
                string sSReturn = "";
                if (oMktSaleTarget.GroupHeadName != "" && oMktSaleTarget.GroupHeadName != null)
                {
                    Global.TagSQL(ref sSReturn);
                    sSReturn += " GroupHeadName LIKE '%" + oMktSaleTarget.GroupHeadName + "%'";
                }
               
                sSQL += sSReturn;
                _oMktSaleTargets = MktSaleTarget.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oMktSaleTarget = new  MktSaleTarget();
                _oMktSaleTarget.ErrorMessage = ex.Message;
                _oMktSaleTargets.Add(_oMktSaleTarget);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMktSaleTargets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
                 
        #endregion

        [HttpGet]
        public JsonResult MktSaleTargetDelete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                MktSaleTarget _MktSaleTarget = new MktSaleTarget();
                sFeedBackMessage = _MktSaleTarget.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewMarketingAccount(int id, double ts)
        {
            _oMarketingAccount = new MarketingAccount();

            if (id > 0)
            {
                _oMarketingAccount = _oMarketingAccount.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oMarketingAccount.MarketingAccount_BUs = MarketingAccount_BU.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oMarketingAccount.MarketingAccount_BUs = new List<MarketingAccount_BU>();
            }

            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oMarketingAccount);
        }
        [HttpPost]
        public JsonResult Save(MarketingAccount oMarketingAccount)
        {
            _oMarketingAccount = new MarketingAccount();
            try
            {
                _oMarketingAccount = oMarketingAccount;
                _oMarketingAccount = _oMarketingAccount.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMarketingAccount.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMarketingAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(MarketingAccount oMarketingAccount)
        {
            string sMessage = "";
            try
            {
                if (oMarketingAccount.MarketingAccountID <= 0) { throw new Exception("Please select a valid MarketingAccount."); }
                sMessage = oMarketingAccount.Delete(oMarketingAccount.MarketingAccountID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CommitActivity(int id, bool IsActive)
        {
            MarketingAccount oMarketingAccount = new MarketingAccount();
            try
            {
                oMarketingAccount = oMarketingAccount.CommitActivity(id, IsActive, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oMarketingAccount.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMarketingAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GroupActivity(int id, bool IsActive)
        {
            MarketingAccount oMarketingAccount = new MarketingAccount();
            _oMarketingAccounts = new List<MarketingAccount>();
            string sSQL = "";  string sMarketingIDs = "";
            try
            {
                oMarketingAccount = oMarketingAccount.Get(id,(int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM MarketingAccount WHERE GroupID = " +id;
                _oMarketingAccounts = MarketingAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                _oMarketingAccounts.Add(oMarketingAccount);
                sMarketingIDs = string.Join(",", _oMarketingAccounts.Select(s => s.MarketingAccountID).Distinct());
                oMarketingAccount = oMarketingAccount.GroupActivity(id,sMarketingIDs, IsActive, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oMarketingAccount.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMarketingAccount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }




        #region MarketingAccount Searching

        [HttpPost]
        public JsonResult GetMarketingAccounts(MarketingAccount oMarketingAccount)
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            string sSQL = GetSearchSQL(oMarketingAccount.Name);
            _oMarketingAccounts = MarketingAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsMarketingAccount(MarketingAccount oMarketingAccount)
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            //string sSQL = GetSearchSQL(oMarketingAccount.Name);
            _oMarketingAccounts = MarketingAccount.Gets("SELECT * FROM View_MarketingAccount WHERE Name Like '%" + oMarketingAccount.Name + "%' AND IsGroup!=1", (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsMarketingAccountByID(MarketingAccount oMarketingAccount)
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            string sSQL = "SELECT * FROM View_MarketingAccount WHERE GroupID=" + oMarketingAccount.MarketingAccountID + " AND IsGroup!=1";
            _oMarketingAccounts = MarketingAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSearchSQL(string sString)
        {
            string sReturn1 = "SELECT * FROM View_MarketingAccount";
            string sReturn = "";
            #region Make String
            if (!String.IsNullOrEmpty(sString))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Name Like '%" + sString + "%' ";
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        [HttpPost]
        public JsonResult MarketingAccountSearchByName(MarketingAccount_BU oMarketingAccount_BU) // Added By Sagor on 24 May 2014 For Enter Event Searching
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            //if(oMarketingAccount_BU.Params!="")
            //{
            //    oMarketingAccount_BU.Name = oMarketingAccount_BU.Params.Split('~')[1].Trim();
            //    oMarketingAccount_BU.BUID = Convert.ToInt32(oMarketingAccount_BU.Params.Split('~')[2]);
            //    if (oMarketingAccount_BU.Name == "@ContractorID") oMarketingAccount_BU.Name = "";
            //}
            try
            {
                if (!String.IsNullOrEmpty(oMarketingAccount_BU.Name))
                {
                    oMarketingAccount_BU.Name = oMarketingAccount_BU.Name.Trim();
                }
                _oMarketingAccounts = MarketingAccount.GetsByName(oMarketingAccount_BU.Name, oMarketingAccount_BU.BUID, (int)Session[SessionInfo.currentUserID]);
                if (_oMarketingAccounts.Count <= 0) { throw new Exception("No information found."); }
            }
            catch (Exception ex)
            {
                _oMarketingAccount = new MarketingAccount();
                _oMarketingAccount.ErrorMessage = ex.Message;
                _oMarketingAccounts = new List<MarketingAccount>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult MarketingAccountSearchByNameAndGroup(MarketingAccount_BU oMarketingAccount_BU) // Added By Sagor on 24 May 2014 For Enter Event Searching
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            try
            {
                if (!String.IsNullOrEmpty(oMarketingAccount_BU.Name))
                {
                    oMarketingAccount_BU.Name = oMarketingAccount_BU.Name.Trim();
                }
                
                _oMarketingAccounts = MarketingAccount.GetsByBUAndGroup( oMarketingAccount_BU.BUID, (int)Session[SessionInfo.currentUserID]);
                if (_oMarketingAccounts.Count > 0)
                {
                    _oMarketingAccounts = MarketingAccount.GetsByNameGroup(oMarketingAccount_BU.Name, oMarketingAccount_BU.BUID, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oMarketingAccounts = MarketingAccount.GetsByName(oMarketingAccount_BU.Name, oMarketingAccount_BU.BUID, (int)Session[SessionInfo.currentUserID]);
                }
                if (_oMarketingAccounts.Count <= 0) { throw new Exception("No information found."); }
            }
            catch (Exception ex)
            {
                _oMarketingAccount = new MarketingAccount();
                _oMarketingAccount.ErrorMessage = ex.Message;
                _oMarketingAccounts = new List<MarketingAccount>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsGroupHead(MarketingAccount_BU oMarketingAccount_BU) // Added By Sagor on 24 May 2014 For Enter Event Searching
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            //if(oMarketingAccount_BU.Params!="")
            //{
            //    oMarketingAccount_BU.Name = oMarketingAccount_BU.Params.Split('~')[1].Trim();
            //    oMarketingAccount_BU.BUID = Convert.ToInt32(oMarketingAccount_BU.Params.Split('~')[2]);
            //    if (oMarketingAccount_BU.Name == "@ContractorID") oMarketingAccount_BU.Name = "";
            //}
            try
            {
                if (!String.IsNullOrEmpty(oMarketingAccount_BU.Name))
                {
                    oMarketingAccount_BU.Name = oMarketingAccount_BU.Name.Trim();
                }
                _oMarketingAccounts = MarketingAccount.GetsGroupHead(oMarketingAccount_BU.Name, oMarketingAccount_BU.BUID, (int)Session[SessionInfo.currentUserID]);
                if (_oMarketingAccounts.Count <= 0) { throw new Exception("No information found."); }
            }
            catch (Exception ex)
            {
                _oMarketingAccount = new MarketingAccount();
                _oMarketingAccount.ErrorMessage = ex.Message;
                _oMarketingAccounts = new List<MarketingAccount>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsGroup(MarketingAccount_BU oMarketingAccount_BU) //
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            try
            {
                string sReturn1 = "Select * from View_MarketingAccount";
                string sReturn = "";
               
                if (!String.IsNullOrEmpty(oMarketingAccount_BU.Name))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Name Like '%" + oMarketingAccount_BU.Name + "%' ";
                }

                if (oMarketingAccount_BU.BUID> 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "MarketingAccountID in (Select MarketingAccountID from MarketingAccount_BU where BUID  in (" + oMarketingAccount_BU.BUID + "))";
                }
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Isnull(GroupID,0)=0 and IsGroup=1";//Activity =1 and

                sReturn1 = sReturn1 + "" + sReturn;
                _oMarketingAccounts = MarketingAccount.Gets(sReturn1, (int)Session[SessionInfo.currentUserID]);
                //if (_oMarketingAccounts.Count <= 0) { throw new Exception("No information found."); }

                _oMarketingAccounts.Add(new MarketingAccount() { Name="Open Group", MarketingAccountID=0});
            }
            catch (Exception ex)
            {
                _oMarketingAccount = new MarketingAccount();
                _oMarketingAccount.ErrorMessage = ex.Message;
                _oMarketingAccounts = new List<MarketingAccount>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsAllGroup(MarketingAccount_BU oMarketingAccount_BU) //
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            try
            {
                _oMarketingAccounts = MarketingAccount.GetsGroup(oMarketingAccount_BU.Name,oMarketingAccount_BU.BUID, (int)Session[SessionInfo.currentUserID]);
                if (_oMarketingAccounts.Count <= 0) { throw new Exception("No information found."); }

                //_oMarketingAccounts.Add(new MarketingAccount() { Name = "Open Group", MarketingAccountID = 0 });
            }
            catch (Exception ex)
            {
                _oMarketingAccount = new MarketingAccount();
                _oMarketingAccount.ErrorMessage = ex.Message;
                _oMarketingAccounts = new List<MarketingAccount>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsMktByUser(MarketingAccount_BU oMarketingAccount_BU) //
        {
            _oMarketingAccounts = new List<MarketingAccount>();
            try
            {
                _oMarketingAccounts = MarketingAccount.GetsByUser(oMarketingAccount_BU.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oMarketingAccounts.Count <= 0) { throw new Exception("No information found."); }

                //_oMarketingAccounts.Add(new MarketingAccount() { Name = "Open Group", MarketingAccountID = 0 });
            }
            catch (Exception ex)
            {
                _oMarketingAccount = new MarketingAccount();
                _oMarketingAccount.ErrorMessage = ex.Message;
                _oMarketingAccounts = new List<MarketingAccount>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsGroupForExportPIReport(MarketingAccount_BU oMarketingAccount_BU) //
        {
            MarketingAccount_BU _oMarketingAccount_BU = new MarketingAccount_BU();
            List<MarketingAccount_BU> _oMarketingAccount_BUs = new List<MarketingAccount_BU>();
            try
            {
                string sReturn1 = "Select * from View_MarketingAccount_BU";
                string sReturn = "";

                if (!String.IsNullOrEmpty(oMarketingAccount_BU.Name))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MarketingAccountName Like '%" + oMarketingAccount_BU.Name + "%' ";
                }

                if (oMarketingAccount_BU.BUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BUID = " + oMarketingAccount_BU.BUID;
                }
                sReturn1 = sReturn1 + "" + sReturn;
                _oMarketingAccount_BUs = MarketingAccount_BU.Gets(sReturn1, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMarketingAccount_BU = new MarketingAccount_BU();
                _oMarketingAccount_BU.ErrorMessage = ex.Message;
                _oMarketingAccount_BUs = new List<MarketingAccount_BU>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMarketingAccount_BUs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
    }
}

              
           
    