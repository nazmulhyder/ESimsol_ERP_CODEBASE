using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;

using System.Drawing.Imaging;
using System.Web.Script.Serialization;
using ReportManagement;
using iTextSharp.text;
using System.Net.Mail;
using System.IO;
using ESimSol.Reports;



namespace ESimSolFinancial.Controllers
{
    public class UserController : PdfViewController
    {
        #region Declaration
        User _oUser = new User();
        List<User> _oUsers = new List<User>();
        UserWiseContractorConfigure _oUserWiseContractorConfigure = new UserWiseContractorConfigure();
        List<UserWiseContractorConfigure> _oUserWiseContractorConfigures = new List<UserWiseContractorConfigure>();
        UserWiseStyleConfigure _oUserWiseStyleConfigure = new UserWiseStyleConfigure();
        List<UserWiseStyleConfigure> _oUserWiseStyleConfigures = new List<UserWiseStyleConfigure>();
        UserWiseContractorConfigureDetail _oUserWiseContractorConfigureDetail = new UserWiseContractorConfigureDetail();
        List<UserWiseContractorConfigureDetail> _oUserWiseContractorConfigureDetails = new List<UserWiseContractorConfigureDetail>();
        Menu _oMenu = new Menu();
        TMenu _oTMenu = new TMenu();
        List<Menu> _oMenus = new List<Menu>();
        List<TMenu> _oTMenus = new List<TMenu>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        private TMenu GetRoot()
        {
            TMenu oTMenu = new TMenu();
            foreach (TMenu oItem in _oTMenus)
            {
                if (oItem.parentid == 0)
                {
                    return oItem;
                }
            }
            return oTMenu;
        }

        private IEnumerable<TMenu> GetChild(int nid)
        {
            List<TMenu> oTMenus = new List<TMenu>();
            foreach (TMenu oItem in _oTMenus)
            {
                if (oItem.parentid == nid)
                {
                    if ((((User)Session[SessionInfo.CurrentUser]).IsSuperUser) || ((User)Session[SessionInfo.CurrentUser]).IsPermitted(oItem.id))
                    {
                        oTMenus.Add(oItem);
                    }
                }
            }
            return oTMenus;
        }
        private void AddTreeNodes(ref TMenu oTMenu)
        {
            IEnumerable<TMenu> oChildNodes;
            oChildNodes = GetChild(oTMenu.id);
            oTMenu.children = oChildNodes;

            foreach (TMenu oItem in oChildNodes)
            {
                TMenu oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        public TMenu GetUserMenu(EnumAccountHolderType AccountHolderType)
        {
            _oMenus = new List<Menu>();
            _oMenu = new Menu();
            _oTMenu = new TMenu();
            _oTMenus = new List<TMenu>();
            try
            {
                if (Session[SessionInfo.CurrentUserMenu] == null)
                {
                    _oMenus = Menu.Gets((int)EnumApplicationType.WebApplication, (int)Session[SessionInfo.currentUserID]);
                    this.Session.Add(SessionInfo.CurrentUserMenu, (object)_oMenus);
                }
                else
                {
                    _oMenus = (List<Menu>)Session[SessionInfo.CurrentUserMenu];
                }

                foreach (Menu oItem in _oMenus)
                {
                    _oTMenu = new TMenu();
                    _oTMenu.id = oItem.MenuID;
                    _oTMenu.parentid = oItem.ParentID;
                    _oTMenu.text = oItem.MenuName;
                    _oTMenu.IsWithBU = oItem.IsWithBU;
                    _oTMenu.BUID = oItem.BUID;
                    _oTMenu.IsActive = oItem.IsActive;
                    _oTMenu.ModuleName = oItem.ModuleName;
                    _oTMenu.ActivityInString = (oItem.IsActive) ? "Active" : "InActive";
                    if (oItem.ControllerName != "" && oItem.ActionName != "")
                    {
                        _oTMenu.attributes = oItem.ControllerName + "~" + oItem.ActionName;
                    }
                    if (oItem.ControllerName == "aaa" && oItem.ActionName == "aaa")
                    {
                        _oTMenu.state = "closed";
                    }
                    _oTMenus.Add(_oTMenu);
                }
                _oTMenu = new TMenu();
                _oTMenu = GetRoot();
                this.AddTreeNodes(ref _oTMenu);
                return _oTMenu;
            }
            catch (Exception ex)
            {
                _oTMenu = new TMenu();
                return _oTMenu;
            }
        }        
        private bool ValidateInput(User oUser)
        {
            if (oUser.LogInID == null || oUser.LogInID == "")
            {
                _sErrorMessage = "Please enter LogIn-ID";
                return false;
            }
            if (oUser.UserName == null || oUser.UserName == "")
            {
                _sErrorMessage = "Please enter User Name";
                return false;
            }
            if (oUser.Password != oUser.ConfirmPassword)
            {
                _sErrorMessage = "Your Entered password & Confirm password are not same";
                return false;
            }
            if (oUser.EmailAddress == null || oUser.EmailAddress == "")
            {
                _sErrorMessage = "Please enter Email Address";
                return false;
            }
            return true;
        }
        private List<UserWiseContractorConfigureDetail> GetUWCDetails(int nUWCID, List<UserWiseContractorConfigureDetail> oTempUserWiseContractorConfigureDetails)
        {
            List<UserWiseContractorConfigureDetail> oUserWiseContractorConfigureDetails = new List<UserWiseContractorConfigureDetail>();

            foreach (UserWiseContractorConfigureDetail oDetailItem in oTempUserWiseContractorConfigureDetails)
            {
                if (oDetailItem.UserWiseContractorConfigureID == nUWCID)
                {
                    oUserWiseContractorConfigureDetails.Add(oDetailItem);
                }
            }
            return oUserWiseContractorConfigureDetails;
        }
       
        #endregion        
        private string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        public ActionResult Login()
        {
            //check currently any user login            

            if ((User)Session[SessionInfo.CurrentUser] == null || ((User)Session[SessionInfo.CurrentUser]).UserID == 0)
            {
                _oUser = new User();
            }
            else
            {
                _oUser = (User)Session[SessionInfo.CurrentUser];
            }
            //ICS.Base.Client.BOFoundation.ObjectArryay oa = new ICS.Base.Client.BOFoundation.ObjectArryay(_oUser, null);
            return View(_oUser);
        }
        [HttpPost]
        public ActionResult Login(string LoginID, string PassWord,string BrowserName, string IPAddress, string LogInLocation)
        {
            _oUser = new User();
            try
            {
                //IPAddress = GetIPAddress();
                if ((User)Session[SessionInfo.CurrentUser] == null || ((User)Session[SessionInfo.CurrentUser]).UserID == 0)
                {   
                    _oUser = new ESimSol.BusinessObjects.User();
                    _oUser = ESimSol.BusinessObjects.User.EYDLLogin(0, LoginID, PassWord, BrowserName, IPAddress, LogInLocation);                 
                    if (_oUser.UserID > 0 || _oUser.IsSuperUser)
                    {
                        this.Session.Remove(SessionInfo.ParamObj);
                        this.Session.Add(SessionInfo.currentUserID, _oUser.UserID);
                        this.Session.Add(SessionInfo.currentUserName, _oUser.UserName);
                        this.Session.Add(SessionInfo.CurrentUser, _oUser);
                        this.Session.Add(SessionInfo.FinancialUserType, _oUser.FinancialUserTypeInt);
                        this.Session.Add(SessionInfo.EmployeeType, _oUser.EmployeeType);       
                    }
                }
                else
                {
                    _oUser = (User)Session[SessionInfo.CurrentUser];
                }
                if (_oUser != null)
                {                    
                    TempData["message"] = _oUser.LoginMessage;
                    _oUser.Menu = this.GetUserMenu(_oUser.AccountHolderType);
                    this.Session.Remove(SessionInfo.Menu);
                    this.Session.Remove(SessionInfo.ParamObj);
                    this.Session.Add(SessionInfo.Menu, _oUser.Menu);
                    List<DBPermission> oDBPermissions = new List<DBPermission>();
                    oDBPermissions = DBPermission.GetsByUser(_oUser.UserID, _oUser.UserID);
                    if (oDBPermissions.Count > 0)
                    {
                        if (oDBPermissions[0].DashBoardType == EnumDashBoardType.Dyeing_DashBoard || oDBPermissions[0].DashBoardType == EnumDashBoardType.Plastic_DashBoard)
                        {
                            return RedirectToAction("ViewMgtDashBoard", "ManagementDashboard", new { nbuid = 0, sreportdate = DateTime.Today.ToString("dd MMM yyyy"), menuid = 0 });
                        }
                    }
                    else
                    {
                        if (_oUser.EmployeeType == EnumEmployeeDesignationType.Merchandiser)
                        {
                            return RedirectToAction("MerchandiserDashBoard", "Home", new { menuid = 0, EmployeeID = _oUser.EmployeeID, buid = 0 });
                        }
                        else if (_oUser.EmployeeType == EnumEmployeeDesignationType.Management)
                        {
                            return RedirectToAction("ViewManagementDashboard", "Home", new { sessionid = 0 });
                        }
                        else if (_oUser.EmployeeType == EnumEmployeeDesignationType.ManagingDirector || _oUser.EmployeeType == EnumEmployeeDesignationType.Chairman)
                        {
                            return RedirectToAction("ViewFinancialDeshBoard", "CapacityAllocation", new { menuid = 0 });
                        }
                        else
                        {
                            return View(_oUser);
                        }
                    }
                }
            }
            catch (Exception exp)
            {                
                TempData["message"] = exp.Message;
                _oUser.LoginMessage = exp.Message;
            }
            return View(_oUser);
        }
        public ActionResult LogOut()
        {
            if(((User)Session[SessionInfo.CurrentUser])!=null)
                ESimSol.BusinessObjects.User.LogOut(((User)Session[SessionInfo.CurrentUser]).UserID); 
           
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Remove(SessionInfo.Menu);
            this.Session.Remove(SessionInfo.CurrentUser);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Remove(SessionInfo.BaseAddress);
            this.Session.Remove(SessionInfo.CurrentUserMenu);
            this.Session.Remove(SessionInfo.FinancialUserType);
            this.Session.Remove(SessionInfo.BaseCurrencyID);
            this.Session.Remove(SessionInfo.ParamObj);

            return RedirectToAction("LogIn", "User");
        }
        public ActionResult ChangePassword()
        {
            User oUser = (User)Session[SessionInfo.CurrentUser];
            oUser = oUser.Get(oUser.UserID, ((User)Session[SessionInfo.CurrentUser]).UserID);//refresh from db
            oUser.ConfirmPassword = oUser.Password;            
            return View(oUser);
        }       
        [HttpPost]
        public ActionResult ChangePassword(User oUser, FormCollection col)
        {
            try
            {
                string sOldPassword = col["txtOldPassword"];
                string sNewPassword = col["txtNewPassword"];
                string sConfirmPassword = col["txtConfirmPassword"];
                oUser = oUser.Get(oUser.UserID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                if (sOldPassword != Global.Decrypt(oUser.Password))
                {
                    TempData["message"] = "Your entered old password is incorrect!";
                    return View(oUser);
                }

                if (sNewPassword != sConfirmPassword)
                {
                    TempData["message"] = "password miss match!";
                    return View(oUser);
                }
                oUser.Password = sNewPassword;
                oUser.ConfirmPassword = sConfirmPassword;
                if (this.ValidateInput(oUser))
                {
                    oUser.DomainUserName = "";
                    oUser = oUser.ChangePassword(((User)Session[SessionInfo.CurrentUser]).UserID);
                    string sSubject = MailSubject((int)EnumMailPurpose.ChangePassword);
                    string sBodyInformation = MailBody(oUser, (int)EnumMailPurpose.ChangePassword);
                    List<string> EmailTos = new List<string>();
                    EmailTos.Add(oUser.EmailAddress);
                    Global.MailSend(sSubject, sBodyInformation, EmailTos, new List<string>(), new List<Attachment>());
                    if (oUser.UserID > 0 || oUser.UserID == -9)
                    {
                        TempData["message"] = "Password Change Successfully";
                        return RedirectToAction("LogIn", "User");
                    }
                }
                TempData["message"] = _sErrorMessage;
                return View(oUser);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(oUser);
            }
        }
      
        public ActionResult UserPiker()
        {
            _oUsers = new List<User>();
            _oUsers = ESimSol.BusinessObjects.User.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oUsers);
        }

        public ActionResult UserActionLog(int nid, double ts)
        {
            _oUser = new User();
            _oUser = _oUser.Get(_oUser.UserID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oUser);
        }

        public ActionResult View_User(int id, double ts)
        {
            _oUser = new User();
            if (id > 0)
            {
                _oUser = _oUser.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oUser.OwnerID = (int)Session[SessionInfo.currentUserID];
                _oUser.LoggedOn = false;
                _oUser.LoggedOnMachine = "";
                _oUser.CanLogin = true;
                _oUser.DomainUserName = "";
            }            
            ViewBag.Locations = Location.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.AccountHolderTypes = EnumObject.jGets(typeof(EnumAccountHolderType));
            ViewBag.FinancialUserTypes = EnumObject.jGets(typeof(EnumFinancialUserType));
            return View(_oUser);
        }

        public ActionResult RefreshList(int menuid,string w)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oUsers = new List<User>();
            if (w == null || w == "")
            {
                _oUsers = new List<User>();
                //_oUsers = ESimSol.BusinessObjects.User.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _oUser = new User();
                _oUser.UserID = Convert.ToInt32(w);
                _oUser = _oUser.Get(_oUser.UserID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oUser.CanLogin == true)
                {
                    _oUser.CanLogin = false;
                    _oUser = _oUser.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oUser.CanLogin = true;
                    _oUser = _oUser.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                _oUsers = new List<User>();
                //_oUsers = ESimSol.BusinessObjects.User.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oUsers = ESimSol.BusinessObjects.User.GetsByLogInID(w, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.menuid = menuid;
            return View(_oUsers);
        }

        [HttpPost]
        public JsonResult Save(User oUser)
        {
            try
            {
                oUser.RemoveNulls();
                oUser.DomainUserName = "";                
                if (Global.IsValidMail(oUser.EmailAddress))
                {
                    int nType = 0;
                    if (oUser.UserID > 0) { nType = (int)EnumMailPurpose.Update; }
                    else { nType = (int)EnumMailPurpose.NewAccount; }

                    oUser = oUser.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oUser.ErrorMessage=="")
                    {
                        string sSubject = MailSubject(nType);
                        string sBodyInformation = MailBody(oUser, nType);
                        List<string> EmailTos = new List<string>();
                        EmailTos.Add(oUser.EmailAddress);
                        Global.MailSend(sSubject, sBodyInformation, EmailTos, new List<string>(), new List<Attachment>());
                    }
                    
                }
                else
                {
                    throw new Exception("Please enter a valid email address.");
                }

            }
            catch (Exception ex)
            {
                oUser = new User();
                oUser.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUser);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }            
        public ActionResult ViewCopyAuthorization(int id, double ts)
        {
            _oUsers = new List<User>();
            string sSQL = "UserID IN (SELECT UserID FROM AuthorizationRoleMapping WHERE UserID NOT IN(" + id + ",-9))";
            _oUsers = ESimSol.BusinessObjects.User.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oUsers);
        }

        #region Report Study
        public ActionResult PrintUsers()
        {
            List<User> lstUsers = new List<User>();            
            lstUsers = ESimSol.BusinessObjects.User.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            UserList oUserList = new UserList();
            oUserList.AddRange(lstUsers);
            FillImageUrl(oUserList, "report.jpg");
            return this.ViewPdf("User List", "rptUsers", oUserList, PageSize.A4, 40, 40, 40, 40, false);
        }
        


        private void FillImageUrl(UserList oUserList, string imageName)
        {
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            oUserList.ImageUrl = url + "Content/" + imageName;
        }
        #endregion
               

        #region UAP and password
        public ActionResult UserPasswordReset(int nID)
        {
            User oUser = new User();

            oUser = oUser.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oUser.ConfirmPassword = oUser.Password;
            return View(oUser);  
        }
        #endregion

        [HttpPost]
        public JsonResult GetsUserByLocation(User oUser)
        {

            List<User> oUsers = new List<User>();
            string sSQL="Select * from View_User WHere UserID>0 AND LocationID="+oUser.LocationID+"";
            oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUsers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsUserForCopyAuthorization(User oUser)       /* by akram */
        {
            List<User> oUsers = new List<User>();
            try
            {
                string sSQL = "SELECT * FROM View_User WHERE UserID>0 AND UserID !=" + oUser.UserID + " ORDER BY LoginID ASC";
                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oUser = new User();
                _oUser.ErrorMessage = ex.Message;
                oUsers.Add(_oUser);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUsers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region UserActionLog
        [HttpPost]
        public JsonResult GetUserActionLog(DateTime StartDate, DateTime EndDate, int nUserID)
        {
            string sSQL = "SELECT * FROM (SELECT COUNT(*) as [Login] FROM UserActionLog WHERE UserID=" + nUserID +
                " AND Action=1 AND Datetime BETWEEN CONVERT(date,'" + StartDate + "') AND (CONVERT(date,'" + EndDate + "')))a,(SELECT COUNT(*) as Logout FROM UserActionLog WHERE UserID=" + nUserID +
                " AND Action=2 AND Datetime BETWEEN CONVERT(date,'" + StartDate + "') AND (CONVERT(date,'" + EndDate + "')))b,(SELECT COUNT(*) as WrongPass FROM UserActionLog WHERE UserID=" + nUserID +
                " AND Action=3 AND Datetime BETWEEN CONVERT(date,'" + StartDate + "') AND (CONVERT(date,'" + EndDate + "')))c";
            var ActionLogs = ESimSol.BusinessObjects.User.GetUserActionLogs(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(ActionLogs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllUserActionLog(DateTime StartDate, DateTime EndDate)
        {
            string sSQL = "SELECT * FROM (SELECT COUNT(*) as [Login] FROM UserActionLog WHERE Action=1 AND Datetime BETWEEN CONVERT(date,'" + StartDate + "') AND (CONVERT(date,'" + EndDate + "')))a,(SELECT COUNT(*) as Logout FROM UserActionLog WHERE Action=2 AND Datetime BETWEEN CONVERT(date,'" + StartDate + "') AND (CONVERT(date,'" + EndDate + "')))b,(SELECT COUNT(*) as WrongPass FROM UserActionLog WHERE Action=3 AND Datetime BETWEEN CONVERT(date,'" + StartDate + "') AND (CONVERT(date,'" + EndDate + "')))c";
            var ActionLogs = ESimSol.BusinessObjects.User.GetUserActionLogs(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(ActionLogs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets User By Name
        public ActionResult ViewUsers(string sName, double nts)
        {
            _oUsers = new List<User>();
            string sSQL = "";
            if (sName.Trim() != "")
            {
                sSQL = "Select * from View_User Where EmployeeNameCode Like '%" + sName + "%'";
                _oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return PartialView(_oUsers);
        }
        [HttpPost]
        public JsonResult GetUesrs(string sName, double nts)
        {
            _oUsers = new List<User>();
            _oUser = new User();
            try
            {
                string sSQL ="";
                if(sName.Trim()=="")
                {
                    sSQL = "Select * from View_User order by UserName";
                }
                else
                {
                    sSQL = "Select * from View_User Where EmployeeNameCode Like '%" + sName + "%' order by UserName";
                }
                _oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oUsers.Count <= 0)
                {
                    throw new Exception("No information found");
                }
            }
            catch (Exception ex)
            {
                _oUsers = new List<User>();
                _oUser.ErrorMessage = ex.Message;
                _oUsers.Add(_oUser);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oUsers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUesrsByName(User oUser)
        {
            _oUsers = new List<User>();
            _oUser = new User();
            try
            {
                string sSQL = "";
                if (String.IsNullOrEmpty(oUser.UserName))
                {
                    sSQL = "Select * from View_User order by UserName";
                }
                else
                {
                    oUser.UserName= oUser.UserName.Trim();
                    sSQL = "Select * from View_User Where UserName+LogInID like '%" + oUser.UserName + "%' order by UserName";
                }
                _oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oUsers.Count <= 0)
                {
                    throw new Exception("No information found");
                }
            }
            catch (Exception ex)
            {
                _oUsers = new List<User>();
                _oUser.ErrorMessage = ex.Message;
                _oUsers.Add(_oUser);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oUsers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Verify,Updated Account Through Mail

        private string MailSubject(int nType)
        {
            string sSubject = ""; ;
            if (nType == (int)EnumMailPurpose.NewAccount){ sSubject = "Verify Account-ESimSol";}
            else if (nType == (int)EnumMailPurpose.Update){ sSubject = "Information Updated";}
            else if (nType == (int)EnumMailPurpose.ChangePassword) { sSubject = "Password Changed";}
            else if (nType == (int)EnumMailPurpose.RetrivePassword) { sSubject = "Retrive Password"; }
            return sSubject;
        }
        private string MailBody(User oUser, int nType)
        {
            string sInformation = "" ;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, 0);

            if (nType == (int)EnumMailPurpose.NewAccount)
            {
                sInformation = "<h2>Welcome  Mr. " + oUser.UserName + ".......</h2><br> Link:esimsol.com/" + oCompany.BaseAddress+ ",  User Name: " + oUser.LogInID + " <br>Password: " + Global.Decrypt(oUser.Password) + " <br><div style='float:right; Font-size:11px;'> Created at " + DateTime.Now.ToString("dd MMM yyyy hh:mm") + "</div>";
            }
            else if (nType == (int)EnumMailPurpose.Update)
            {
                sInformation = "<h2>Mr. " + oUser.UserName + ".......</h2><br>  Link:esimsol.com/" + oCompany.BaseAddress + ", User Name: " + oUser.LogInID + " <br>Password: " + Global.Decrypt(oUser.Password) + " <br><div style='float:right; Font-size:11px;'> Updated at " + DateTime.Now.ToString("dd MMM yyyy hh:mm") + "</div>";
            }
            else if (nType == (int)EnumMailPurpose.ChangePassword)
            {
                sInformation = "<h2>Mr. " + oUser.UserName + ".......</h2><br> Link:esimsol.com/" + oCompany.BaseAddress + ",  Your New Password: " + Global.Decrypt(oUser.Password) + " <br><div style='float:right; Font-size:11px;'> Changed at " + DateTime.Now.ToString("dd MMM yyyy hh:mm") + "</div>";
            }
            else if (nType == (int)EnumMailPurpose.RetrivePassword)
            {
                sInformation = "<h2>Mr. " + oUser.UserName + ".......</h2><br> Link:esimsol.com/" + oCompany.BaseAddress + ",  Your Current Password: " + Global.Decrypt(oUser.Password) + " <br><div style='float:right; Font-size:11px;'> Retrive at " + DateTime.Now.ToString("dd MMM yyyy hh:mm") + "</div>";
            }
            return sInformation;
        }
       
        #endregion

        #region Forgotten Password Retrive Added By Sagor On 11 Dec 2014
        public ActionResult ForgetPassword()
        {
            User oUser = new User();
            return PartialView(oUser);
        }

        [HttpPost]
        public ActionResult ForgetPassword(User oUser)
        {
            try
            {
                _oUser = new User();
                if (oUser.LogInID==null || oUser.LogInID.Trim() == "" ) { throw new Exception("Please give a valid user name."); }
                _oUser = _oUser.GetByLogInID(oUser.LogInID, 0);
                if (_oUser.UserID > 0 || _oUser.UserID == -9)
                {
                    string sSubject = MailSubject((int)EnumMailPurpose.RetrivePassword);
                    string sBodyInformation = MailBody(_oUser, (int)EnumMailPurpose.RetrivePassword);
                    List<string> EmailTos = new List<string>();
                    EmailTos.Add(_oUser.EmailAddress);
                    if (Global.MailSend(sSubject, sBodyInformation, EmailTos, new List<string>(), new List<Attachment>()))
                    {
                        TempData["message"] = "Information sent to your mail.";
                    }
                    else
                    {
                        throw new Exception("Your email address is not valid.");
                    }
                }
                else
                {
                    throw new Exception("Invalid User.");
                }

            }
            catch (Exception ex)
            {
                oUser = new User();
                TempData["message"] = ex.Message;
            }
            return PartialView(oUser);
        }

        #endregion

        #region User Image
        public ActionResult View_UserImages(int nUserID, double ts)
        {
            List<UserImage> oUserImages = new List<UserImage>();
          
            oUserImages = UserImage.Gets(nUserID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return PartialView(oUserImages);
        }
        public ActionResult View_UserImage(int nUserImageID, string sMsg)
        {
            UserImage oUserImage = new UserImage();
            if (nUserImageID > 0)
            {
                oUserImage = oUserImage.Get(nUserImageID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oUserImage.ErrorMessage = sMsg;
            return PartialView(oUserImage);
        }

        [HttpPost]
        public ActionResult UserImageIU(HttpPostedFileBase file1, UserImage oUserImage)
        {

            string sErrorMessage = "";
            try
            {
                oUserImage.ImageType = (EnumUserImageType)oUserImage.ImageTypeInt;
                #region Photo Image
                if (file1 != null && file1.ContentLength > 0)
                {
                    System.Drawing.Image oPhotoImage = System.Drawing.Image.FromStream(file1.InputStream, true, true);

                    byte[] aPhotoImageInByteArray = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        oPhotoImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        aPhotoImageInByteArray = ms.ToArray();
                    }

                    #region Image Size Validation
                    double nMaxLength = 40 * 1024;
                    if (aPhotoImageInByteArray.Length > nMaxLength)
                    {
                        sErrorMessage = "Youe Photo Image " + aPhotoImageInByteArray.Length + "KB! You can selecte maximum 80KB image";

                    }

                    #endregion
                    oUserImage.ImageFile = aPhotoImageInByteArray;
                }
                //else
                //{
                //    sErrorMessage = "Select all fields!";
                //}
                #endregion


                if (sErrorMessage == "")
                {

                    if (oUserImage.UserImageID <= 0)
                    {
                        oUserImage = oUserImage.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        oUserImage = oUserImage.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    if (oUserImage.UserImageID > 0 && oUserImage.ErrorMessage == "")
                    {
                        sErrorMessage = "Data saved successfuly !";
                    }
                    else
                    {
                        sErrorMessage = oUserImage.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                sErrorMessage = ex.Message;
            }
            return RedirectToAction("View_UserImage", new { nUserImageID = oUserImage.UserImageID, sMsg = sErrorMessage });
        }

        public System.Drawing.Image GetUserImage(int nUserImageID)
        {
            UserImage oUserImage = new UserImage();
            oUserImage = oUserImage.Get(nUserImageID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oUserImage.ImageFile != null)
            {
                MemoryStream m = new MemoryStream(oUserImage.ImageFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult UserImage_Delete(int nUserImageID)
        {
            UserImage oUserImage = new UserImage();
            oUserImage.UserImageID = nUserImageID;
            try
            {
                oUserImage = oUserImage.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oUserImage.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUserImage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region  Attach
        public ActionResult ViewUserImage(int id, string ms, string OperationInfo)
        {
            UserImage oUserImage = new UserImage();
            List<UserImage> oUserImages = new List<UserImage>();
            oUserImages = UserImage.Gets(id,(int)Session[SessionInfo.currentUserID]);
            oUserImage.UserID = id;
            
            oUserImage.UserImages = oUserImages;
            TempData["message"] = ms;
            oUserImage.ErrorMessage = OperationInfo;
            return View(oUserImage);
        }

        [HttpPost]
        public ActionResult UploadAttchment(HttpPostedFileBase file, UserImage oUserImage)
        {
            string sErrorMessage = "", sUserNameOperationInfo = oUserImage.ErrorMessage;
            int nImageType = (int)oUserImage.ImageType;
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    System.Drawing.Image oPhotoImage = System.Drawing.Image.FromStream(file.InputStream, true, true);
                    byte[] aPhotoImageInByteArray = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        oPhotoImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        aPhotoImageInByteArray = ms.ToArray();
                    }
                    #region Image Size Validation
                    double nMaxLength = 40 * 1024;
                    if (aPhotoImageInByteArray.Length > nMaxLength)
                    {
                        sErrorMessage = "Youe Photo Image " + aPhotoImageInByteArray.Length + "KB! You can selecte maximum 80KB image";
                    }
                    #endregion
                    oUserImage.ImageFile = aPhotoImageInByteArray;
                    oUserImage.ImageType = (EnumUserImageType) nImageType;
                    oUserImage = oUserImage.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
            
                else
                {
                    sErrorMessage = "Please select an file!";
                }

            }
            catch (Exception ex)
            {
                sErrorMessage = "";
                sErrorMessage = ex.Message;
            }
            return RedirectToAction("ViewUserImage", new { id = oUserImage.UserID, ms = sErrorMessage, OperationInfo = sUserNameOperationInfo });
        }

        [HttpPost]
        public JsonResult DeleteAttachment(UserImage oUserImage)
        {
            string sErrorMease = "";
            try
            {
                oUserImage = oUserImage.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #endregion
        
        [HttpPost]
        public JsonResult Gets()
        {
            List<User> oUsers = new List<User>();
            _oUsers = ESimSol.BusinessObjects.User.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            var jsonResult = Json(_oUsers, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetsAll(User oUsers)
        {
            _oUsers = ESimSol.BusinessObjects.User.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            var jsonResult = Json(_oUsers, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #region Display Error Message
        public ViewResult MessageHelper(string message)
        {
            TempData["message"] = message;
            return View();
        }
        #endregion

        #region Gets Requested Users
        [HttpPost]
        public JsonResult GetsRequestedUsers(Employee oEmployee)
        {
            User oUser = new User();
            List<User> oUsers = new List<User>();
            try
            {
                string sSQL = "SELECT * FROM View_User WHERE EmployeeID IN (SELECT EmployeeID FROM Employee WHERE EmployeeDesignationType IN(" + oEmployee.DesignationName.Trim() + "))";
                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]);                 
            }
            catch (Exception ex)
            {
                oUser = new User();
                oUser.ErrorMessage = ex.Message;
                oUsers.Add(oUser);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUsers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    #endregion

        #region User Wise Contractor Configure
        public ActionResult ViewUserWiseContractorConfigure(int id, int buid) // nUserID 
        {
            _oUserWiseContractorConfigure = new UserWiseContractorConfigure();
            _oUserWiseContractorConfigures = new List<UserWiseContractorConfigure>();
            _oUserWiseContractorConfigureDetails = new List<UserWiseContractorConfigureDetail>();
            List<Contractor> oContractors = new List<Contractor>();
            List<UserWiseContractorConfigure> oUserWiseContractorConfigures = new List<UserWiseContractorConfigure>();
            string sSQL = "SELECT * FROM UserWiseContractorConfigureDetail WHERE UserWiseContractorConfigureID IN (SELECT UserWiseContractorConfigureID FROM UserWiseContractorConfigure WHERE UserID = " + id + ")";
            if (id > 0)
            {
                _oUserWiseContractorConfigures = UserWiseContractorConfigure.GetsByUser(id, (int)Session[SessionInfo.currentUserID]);
                _oUserWiseContractorConfigureDetails = UserWiseContractorConfigureDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (UserWiseContractorConfigure oItem in _oUserWiseContractorConfigures)
                {
                    List<UserWiseContractorConfigureDetail> oUserWiseContractorConfigureDetails = new List<UserWiseContractorConfigureDetail>();
                    oUserWiseContractorConfigureDetails = GetUWCDetails(oItem.UserWiseContractorConfigureID, _oUserWiseContractorConfigureDetails);
                    oItem.UserWiseContractorConfigureDetails = oUserWiseContractorConfigureDetails;
                    foreach (UserWiseContractorConfigureDetail oDItem in oUserWiseContractorConfigureDetails)
                    {
                        oItem.StyleTypeIDs = oItem.StyleTypeIDs + (int)oDItem.StyleType + ",";
                        oItem.StyleTypeInString = oItem.StyleTypeInString + oDItem.StyleTypeInString + ",";
                    }
                    if (oItem.StyleTypeInString != null && oItem.StyleTypeInString != "")
                    {
                        if (oItem.StyleTypeInString.Length > 0)
                        {
                            oItem.StyleTypeIDs = oItem.StyleTypeIDs.Remove(oItem.StyleTypeIDs.Length - 1, 1);
                            oItem.StyleTypeInString = oItem.StyleTypeInString.Remove(oItem.StyleTypeInString.Length - 1, 1);
                        }
                    }
                    else
                    {
                        //Sweater = 0,Knit = 1,Woven = 2
                        oItem.StyleTypeInString = "Sweater,Knit,Woven";
                        oItem.StyleTypeIDs = "0,1,2";
                        //Add Sweater Type
                        _oUserWiseContractorConfigureDetail = new UserWiseContractorConfigureDetail();
                        _oUserWiseContractorConfigureDetail.StyleType = EnumTSType.Sweater;
                        oItem.UserWiseContractorConfigureDetails.Add(_oUserWiseContractorConfigureDetail);

                        //Add Kint Type
                        _oUserWiseContractorConfigureDetail = new UserWiseContractorConfigureDetail();
                        _oUserWiseContractorConfigureDetail.StyleType = EnumTSType.Knit;
                        oItem.UserWiseContractorConfigureDetails.Add(_oUserWiseContractorConfigureDetail);

                        //Add Woven Type
                        _oUserWiseContractorConfigureDetail = new UserWiseContractorConfigureDetail();
                        _oUserWiseContractorConfigureDetail.StyleType = EnumTSType.Woven;
                        oItem.UserWiseContractorConfigureDetails.Add(_oUserWiseContractorConfigureDetail);
                    }
                }
            }
            if (buid>0)
            {
                oContractors = Contractor.GetsByNamenType("",((int)EnumContractorType.Buyer).ToString(),buid, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oContractors = Contractor.Gets("SELECT * FROM Contractor WHERE  ContractorID IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + id + ") Order by [Name]", (int)Session[SessionInfo.currentUserID]);
            }
            oUserWiseContractorConfigures = new List<UserWiseContractorConfigure>();
            foreach (Contractor oItem in oContractors)
            {
                _oUserWiseContractorConfigure = new UserWiseContractorConfigure();
                _oUserWiseContractorConfigure = GetExistingBuyer(oItem.ContractorID);
                if (_oUserWiseContractorConfigure.UserWiseContractorConfigureID <= 0)
                {
                    _oUserWiseContractorConfigure = new UserWiseContractorConfigure();
                    _oUserWiseContractorConfigure.UserWiseContractorConfigureID = 0;
                    _oUserWiseContractorConfigure.UserID = id;
                    _oUserWiseContractorConfigure.ContractorName = oItem.Name;
                    _oUserWiseContractorConfigure.UserName = "";
                    _oUserWiseContractorConfigure.ContractorID = oItem.ContractorID;
                    //Sweater = 0,Knit = 1,Woven = 2
                    _oUserWiseContractorConfigure.StyleTypeInString = "Sweater,Knit,Woven";
                    _oUserWiseContractorConfigure.StyleTypeIDs = "0,1,2";
                    //Add Sweater Type
                    _oUserWiseContractorConfigureDetail = new UserWiseContractorConfigureDetail();
                    _oUserWiseContractorConfigureDetail.StyleType = EnumTSType.Sweater;
                    _oUserWiseContractorConfigure.UserWiseContractorConfigureDetails.Add(_oUserWiseContractorConfigureDetail);

                    //Add Kint Type
                    _oUserWiseContractorConfigureDetail = new UserWiseContractorConfigureDetail();
                    _oUserWiseContractorConfigureDetail.StyleType = EnumTSType.Knit;
                    _oUserWiseContractorConfigure.UserWiseContractorConfigureDetails.Add(_oUserWiseContractorConfigureDetail);

                    //Add Woven Type
                    _oUserWiseContractorConfigureDetail = new UserWiseContractorConfigureDetail();
                    _oUserWiseContractorConfigureDetail.StyleType = EnumTSType.Woven;
                    _oUserWiseContractorConfigure.UserWiseContractorConfigureDetails.Add(_oUserWiseContractorConfigureDetail);

                }
                oUserWiseContractorConfigures.Add(_oUserWiseContractorConfigure);
            }
            _oUserWiseContractorConfigure = new UserWiseContractorConfigure();
            _oUserWiseContractorConfigure.UserID = id;
            _oUserWiseContractorConfigure.UserWiseContractorConfigures = oUserWiseContractorConfigures;
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oUserWiseContractorConfigure);
        }

        private UserWiseContractorConfigure GetExistingBuyer(int nContractorID)
        {
            UserWiseContractorConfigure oUserWiseContractorConfigure = new UserWiseContractorConfigure();
            foreach (UserWiseContractorConfigure oItem in _oUserWiseContractorConfigures)
            {
                if (nContractorID == oItem.ContractorID)
                {
                    return oItem;
                }
            }
            return oUserWiseContractorConfigure;
        }

        #region Commit User Wise Contractor
        [HttpPost]
        public JsonResult CommitUserWiseContractor(UserWiseContractorConfigure oUserWiseContractorConfigure)
        {
            string sfeedBackMessage = "";
            try
            {
                sfeedBackMessage = oUserWiseContractorConfigure.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oUserWiseContractorConfigure = new UserWiseContractorConfigure();
                _oUserWiseContractorConfigure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sfeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion
        #endregion

        #region User Wise Style Configure
        public ActionResult ViewUserWiseStyleConfigure(int id) // nUserID 
        {
            User oUser = new ESimSol.BusinessObjects.User();
            _oUserWiseStyleConfigure = new UserWiseStyleConfigure();
            if (id > 0)
            {
                _oUserWiseStyleConfigure.UserWiseStyleConfigures = UserWiseStyleConfigure.GetsByUser(id, (int)Session[SessionInfo.currentUserID]);
                oUser = oUser.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            TempData["UserID"] = id;
            TempData["UserName"] = oUser.UserName;
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            _oUserWiseStyleConfigure.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            return View(_oUserWiseStyleConfigure);
        }

        #region Save User Wize Style Configure
        [HttpPost]
        public JsonResult CommitUserWiseStyle(UserWiseStyleConfigure oUserWiseStyleConfigure)
        {
            _oUserWiseStyleConfigure = new UserWiseStyleConfigure();
            try
            {
                _oUserWiseStyleConfigure = oUserWiseStyleConfigure.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oUserWiseStyleConfigure = new UserWiseStyleConfigure();
                _oUserWiseStyleConfigure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oUserWiseStyleConfigure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


    

        #region Delete User wise Style configure

        [HttpGet]
        public JsonResult DeleteUserWiseStyle(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                _oUserWiseStyleConfigure = new UserWiseStyleConfigure();
                sFeedBackMessage = _oUserWiseStyleConfigure.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region User Location Binded 

        [HttpPost] // Active/ Inactive
        public JsonResult ToggleLocationBindded(User oUser)
        {
            try
            {
                if (oUser.UserID <= 0) { throw new Exception("Invalid user."); }
                oUser.IsLocationBindded = !oUser.IsLocationBindded;
                oUser = oUser.ToggleLocationBindded(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oUser = new User();
                oUser.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUser);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost] // Active/ Inactive
        public JsonResult ToggleShowLedgerBalance(User oUser)
        {
            try
            {
                if (oUser.UserID <= 0) { throw new Exception("Invalid user."); }
                oUser.IsShowLedgerBalance = !oUser.IsShowLedgerBalance;
                oUser = oUser.ToggleShowLedgerBalance(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oUser = new User();
                oUser.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUser);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    #endregion

        [HttpGet]
        public JsonResult GetByEmployeeLogInUN(string sLogInUN, bool bIsLogInID, double nts)
        {
            User oUser = new User();
            List<User> oUsers = new List<User>();

            try
            {
                string sSQL = "";
                if (bIsLogInID)
                {
                    sSQL = "SELECT * FROM View_User WHERE LogInID LIKE '%" + sLogInUN + "%' ORDER BY LogInID ASC";
                }
                else
                {
                    sSQL = "SELECT * FROM View_User WHERE UserName LIKE '%" + sLogInUN + "%' ORDER BY LogInID UserName";
                }
                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oUsers = new List<User>();
                oUser = new User();
                oUser.ErrorMessage = ex.Message;
                oUsers.Add(oUser);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUsers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Menu Permission
        public ActionResult ViewMenuPermission(int id)
        {
            User oUser = new User();
            oUser = oUser.Get(id, (int)Session[SessionInfo.currentUserID]);

            return View(oUser);
        }


        [HttpPost]
        public JsonResult GetsUserMenuTree(User oUser)
        {

            User oTempUser = new User();
            oTempUser = oTempUser.Get(oUser.UserID, (int)Session[SessionInfo.currentUserID]);

            Menu _oMenu = new Menu();
            oTempUser.Menu = this.GetUserMenu(EnumAccountHolderType.Own);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oTempUser);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ConfirmMenuPermission(User oUser)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sSuccessfullmessage = "";
            string sjson = "";
            try
            {
                if (oUser.Keys != null && oUser.Keys != "")
                {
                    oUser.Keys = oUser.Keys.Remove(oUser.Keys.Length - 1, 1);
                }
                if (oUser.ConfirmMenuPermission(oUser.UserID, oUser.Keys, (int)EnumApplicationType.WebApplication, (int)Session[SessionInfo.currentUserID]))
                {
                    sSuccessfullmessage = "Data save successfully";
                    sjson = serializer.Serialize((object)sSuccessfullmessage);
                    return Json(sjson, JsonRequestBehavior.AllowGet);
                }
                sjson = serializer.Serialize((object)sSuccessfullmessage);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                sjson = serializer.Serialize((object)ex.Message);
                return Json(sjson, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ledger Balance Display Permission 
        public ActionResult ViewLedgerBalanceDisplayPermission(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oUsers = new List<User>();

            ViewBag.menuid = menuid;
            return View(_oUsers);
        }
    #endregion

        #region DRP Permission
        public ActionResult ViewDRPPermission(int id)
        {
            User oUser = new User();
            oUser = oUser.Get(id, (int)Session[SessionInfo.currentUserID]);
            return View(oUser);
        }
        #endregion


        #region UserActivity==>Active/Inactive
        [HttpPost]
        public JsonResult UpdateCanLogin(User oUser)
        {
            _oUsers = new List<User>();
            _oUser = new User();
            try
            {
                _oUser = oUser.UpdateCanlogin(oUser.CanLogin, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oUser = new User();
                _oUser.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oUser);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost]
        public JsonResult GetByEmployeeType(User oUser) // Added By Sagor on 24 May 2014 For Enter Event Searching
        {
            _oUsers = new List<User>();
            try
            {
                //'" + oUser.ErrorMessage + "'   SELECT * FROM View_User WHERE EmployeeType = 2 
                string sSQL = "SELECT * FROM View_User WHERE EmployeeType = " + oUser.EmployeeID + " AND UserName LIKE '%" + oUser.UserName + "%' AND UserID != -9 ";
                _oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oUsers.Count <= 0) { throw new Exception("No information found."); }
            }
            catch (Exception ex)
            {
                oUser = new User();
                oUser.ErrorMessage = ex.Message;
                _oUsers.Add(oUser);
            }
            var jsonResult = Json(_oUsers, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult PrintUserPermission(int id, int buid)
        {
            _oUser = new User();
            _oUser = _oUser.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oUser.Menu = this.GetUserMenu(_oUser.AccountHolderType);

            List<Menu> oMenus = new List<Menu>();
            oMenus = Menu.Gets("SELECT * FROM View_Menu WHERE MenuID IN (SELECT MenuID FROM UserPermissionFinance WHERE UserID = " + id + ")", (int)Session[SessionInfo.currentUserID]);

            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByUser(_oUser.UserID, (int)Session[SessionInfo.currentUserID]);

            string sPermissions = "";
            List<AuthorizationRoleMapping> oTempAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            int nCount = 0;
            foreach (Menu oItem in oMenus)  //modulename
            {
                sPermissions = "";
                oTempAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
                oTempAuthorizationRoleMappings = oAuthorizationRoleMappings.Where(x => x.ModuleName == oItem.ModuleName).ToList();
                
                foreach (AuthorizationRoleMapping obj in oTempAuthorizationRoleMappings)
                {
                    sPermissions = sPermissions + obj.OperationTypeST + " ,";
                }
                if (!string.IsNullOrEmpty(sPermissions))
                {
                    sPermissions = sPermissions.Remove(sPermissions.Length - 1);
                }
                oItem.PermittedOperation = sPermissions;
                oMenus[nCount].PermittedOperation = sPermissions;
                nCount++;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            Menu oTempMenu = new Menu();
            List<Menu> oTempMenus = new List<Menu>();
            _oTMenus = new List<TMenu>();            
            foreach (Menu oItem in oMenus)
            {
                _oTMenu = new TMenu();
                _oTMenu.id = oItem.MenuID;
                _oTMenu.parentid = oItem.ParentID;
                _oTMenu.text = oItem.MenuName;
                _oTMenu.attributes = oItem.PermittedOperation;
                _oTMenu.MenuLevel = oItem.MenuLevel;
                _oTMenus.Add(_oTMenu);
            }

            _oTMenu = new TMenu();
            _oTMenu = GetRoot();
            this.AddTreeNodes(ref _oTMenu);

            #region Get Store Permission
            List<StorePermission> oStorePermissions = StorePermission.GetsByUser(id, (int)Session[SessionInfo.currentUserID]);
            #endregion

            #region Product Permission
            List<ProductPermission> oProductPermissions = ProductPermission.GetsByUser(id, (int)Session[SessionInfo.currentUserID]); 
            #endregion

            rptUserPermission orptUserPermission = new rptUserPermission();
            byte[] abytes = orptUserPermission.PrepareReport(_oUser, _oTMenu, oCompany, oStorePermissions, oProductPermissions);
            return File(abytes, "application/pdf");
        }

        public System.Drawing.Image GetCompanyLogo(Company oCompany)
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
        public ActionResult PrintUserPermissionAuditReport(int id)
        {
            _oUser = new User();
            _oUser = _oUser.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oUser.Menu = this.GetUserMenu(_oUser.AccountHolderType);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            #region Get Authorization Role
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByUser(id, (int)Session[SessionInfo.currentUserID]);
            #endregion

            rptUserPermissionAuditReport orptUserPermissionAuditReport = new rptUserPermissionAuditReport();
            byte[] abytes = orptUserPermissionAuditReport.PrepareReport(_oUser, oCompany, oAuthorizationRoleMappings);
            return File(abytes, "application/pdf");
        }
        


    }
}