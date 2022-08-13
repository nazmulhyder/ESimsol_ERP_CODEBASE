using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class CompanyController : Controller
    {
        #region Declaration
        Company _oCompany = new Company();
        List<Company> _oCompanys = new List<Company>();
        #endregion
        public ActionResult ViewCompanys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oCompanys = new List<Company>();
            _oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oCompanys);
        }
        public ActionResult ViewCompany(string sid, string sMsg)
        {
            _oCompany = new Company();
            int nID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            if (nID > 0)
            {
                _oCompany = _oCompany.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oCompany.OrganizationLogo != null)
                {
                    _oCompany.ByteInString = "data:image/Jpeg;base64," + Convert.ToBase64String(_oCompany.OrganizationLogo);
                }
                else
                {
                    _oCompany.ByteInString = "";
                }

                if (_oCompany.AuthorizedSignature != null)
                {
                    _oCompany.ByteInString_Signature = "data:image/Jpeg;base64," + Convert.ToBase64String(_oCompany.AuthorizedSignature);
                }
                else
                {
                    _oCompany.ByteInString_Signature = "";
                }
                if (_oCompany.OrganizationTitle != null)
                {
                    _oCompany.ByteInString_CompanyTitle = "data:image/Jpeg;base64," + Convert.ToBase64String(_oCompany.OrganizationTitle);
                }
                else
                {
                    _oCompany.ByteInString_CompanyTitle = "";
                }
            }            
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            if (sMsg != "N/A")
            {
                _oCompany.ErrorMessage = sMsg;
            }
            return View(_oCompany);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                Company oCompany = new Company();
                sFeedBackMessage = oCompany.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Gets()
        {
            List<Company> oCompanys = new List<Company>();
            Company oCompany = new Company();
            oCompany.Name = "-- Select Company --";
            oCompanys.Add(oCompany);
            oCompanys.AddRange(Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oCompanys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public Image GetCompanyLogo(int id)
        {
            #region From DB
            Company oCompany = new Company();
            oCompany = oCompany.GetCompanyLogo(id, 0);
            this.Session.Add(SessionInfo.CurrentCompanyID, id);
            this.Session.Add(SessionInfo.BaseCurrencyID, oCompany.BaseCurrencyID);

            this.Session.Add(SessionInfo.BaseAddress, "/" + oCompany.BaseAddress);
            this.Session.Add(SessionInfo.BaseAddress, ""); //off with hosting/

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
        public Image GetCompanyTitle(int id)
        {            
            if ((User)Session[SessionInfo.CurrentUser]!=null)
            {
                #region From DB
                Company oCompany = new Company();
                oCompany = oCompany.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oCompany.OrganizationTitle != null)
                {
                    MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
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
            else
            {
                #region From Foldr File
                //string spath = this.ControllerContext.HttpContext.Server.MapPath("../../Content/Images/Companylogo.jpg");//RT
                string spath = this.ControllerContext.HttpContext.Server.MapPath("../../Content/Images/ICS_LOGO.png");
                //string spath = this.ControllerContext.HttpContext.Server.MapPath("../../Content/Images/AKIJ_LOGO.png");
                using (FileStream fileStream = System.IO.File.OpenRead(spath))
                {
                    MemoryStream memStream = new MemoryStream();
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                    System.Drawing.Image img = System.Drawing.Image.FromStream(memStream);
                    img.Save(Response.OutputStream, ImageFormat.Jpeg);
                    return img;
                }
                #endregion
            }
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
        public Image GetBackgroundLogo(int id)
        {

            #region From Foldr File
            string spath = this.ControllerContext.HttpContext.Server.MapPath("../../Content/Images/BGTheme.jpg");
            using (FileStream fileStream = System.IO.File.OpenRead(spath))
            {
                MemoryStream memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                System.Drawing.Image img = System.Drawing.Image.FromStream(memStream);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            #endregion
        }

        #region Save Company Information by XML
        [HttpPost]
        public JsonResult SaveCompanyInformation(double nts)
        {
            string sMessage = "";
            Company oCompany = new Company();
            try
            {
                oCompany.CompanyID = Convert.ToInt32(Request.Headers["CompanyID"]);
                oCompany.Name = Request.Headers["Name"].Trim();
                oCompany.Address = Request.Headers["Address"].Trim();
                oCompany.FactoryAddress = Request.Headers["FactoryAddress"].Trim();
                oCompany.Phone = Request.Headers["Phone"].Trim();
                oCompany.Email = Request.Headers["Email"].Trim();
                oCompany.WebAddress = Request.Headers["WebAddress"].Trim();
                oCompany.VatRegNo = Request.Headers["VatRegNo"].Trim();
                oCompany.PostalCode = Request.Headers["PostalCode"].Trim();
                oCompany.Country = Request.Headers["Country"].Trim();
                oCompany.Note = Request.Headers["Note"].Trim();
                oCompany.NameInBangla = Request.Headers["NameInBangla"].Trim();
                if (Request.Headers["AddressInBangla"] != null)
                {
                    oCompany.AddressInBangla = Request.Headers["AddressInBangla"].Trim();
                }
                oCompany.BaseCurrencyID = Convert.ToInt32(Request.Headers["BaseCurrencyID"]);
                oCompany.ParentID = Convert.ToInt32(Request.Headers["ParentID"]);

                oCompany.IsRemoveLogo = Convert.ToBoolean(Request.Headers["IsRemoveLogo"]);
                oCompany.IsRemoveSignature = Convert.ToBoolean(Request.Headers["IsRemoveSignature"]);
                oCompany.IsRemoveTitle = Convert.ToBoolean(Request.Headers["IsRemoveTitle"]);

                byte[] data;
                #region File
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = null;
                    HttpPostedFileBase file_Signature = null;
                    HttpPostedFileBase file_CompanyTitle = null;
                    if (Request.Files.Count == 3)
                    {
                        file = Request.Files[0];
                        file_Signature = Request.Files[1];
                        file_CompanyTitle = Request.Files[2];
                    }
                    else
                    {
                        if (Convert.ToBoolean(Request.Headers["IsOrganizationLogo"]) == true)
                        {
                            file = Request.Files[0];
                        }
                        else if (Convert.ToBoolean(Request.Headers["IsAuthorizedSignature"]) == true)
                        {
                            file_Signature = Request.Files[0];
                        }
                        else if (Convert.ToBoolean(Request.Headers["IsCompanyTitle"]) == true)
                        {
                            file_CompanyTitle = Request.Files[0];
                        }
                    }
                    if (file != null)
                    {
                        using (Stream inputStream = file.InputStream)
                        {
                            MemoryStream memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }
                            data = memoryStream.ToArray();
                            double nMaxLength = 100 * 1024;
                            if (data.Length > nMaxLength)
                            {
                                throw new Exception("Youe Photo Image " + data.Length / 1024 + "KB! You can selecte maximum 40KB image");
                            }
                        }
                        oCompany.OrganizationLogo = data;
                    }
                    if (file_CompanyTitle != null)
                    {
                        data = null;
                        using (Stream inputStream = file_CompanyTitle.InputStream)
                        {
                            MemoryStream memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }
                            data = memoryStream.ToArray();
                            double nMaxLength = 100 * 1024;
                            if (data.Length > nMaxLength)
                            {
                                throw new Exception("Youe Photo Image " + data.Length / 1024 + "KB! You can selecte maximum 40KB image");
                            }
                        }
                        oCompany.OrganizationTitle = data;
                    }
                    if (file_Signature != null)
                    {

                        data = null;
                        using (Stream inputStream = file_Signature.InputStream)
                        {
                            MemoryStream memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }
                            data = memoryStream.ToArray();
                            double nMaxLength = 100 * 1024;
                            if (data.Length > nMaxLength)
                            {
                                throw new Exception("Youe Photo Image " + data.Length / 1024 + "KB! You can selecte maximum 40KB image");
                            }
                        }
                        oCompany.AuthorizedSignature = data;
                    }
                }
                #endregion
                oCompany = oCompany.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oCompany.CompanyID > 0 && (oCompany.ErrorMessage == null || oCompany.ErrorMessage == ""))
                {
                    sMessage = "Save Successfully" + "~" + oCompany.CompanyID + "~" + oCompany.Name + "~" + oCompany.Address + "~" + oCompany.Phone + "~" +
                               oCompany.Email + "~" + oCompany.WebAddress + "~" + oCompany.Note + "~" + oCompany.BaseCurrencyID + "~" + oCompany.ParentID + "~" + oCompany.EncryptCompanyID;
                }
                else
                {
                    if ((oCompany.ErrorMessage == null || oCompany.ErrorMessage == "")) { throw new Exception("Unable to Upload."); }
                    else { throw new Exception(oCompany.ErrorMessage); }

                }
            }
            catch (Exception ex)
            {
                oCompany = new Company();
                oCompany.ErrorMessage = ex.Message;
                sMessage = oCompany.ErrorMessage + "~" + oCompany.CompanyID + "~" + oCompany.Name + "~" + oCompany.Address + "~" + oCompany.Phone + "~" +
                               oCompany.Email + "~" + oCompany.WebAddress + "~" + oCompany.Note + "~" + oCompany.BaseCurrencyID + "~" + oCompany.ParentID + "~" + oCompany.EncryptCompanyID;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
