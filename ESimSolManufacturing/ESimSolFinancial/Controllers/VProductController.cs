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
using System.Web;
using ICS.Core.Utility;
using System.Data;
using System.Data.OleDb;

namespace ESimSolFinancial.Controllers
{
    public class VProductController : Controller
    {
        #region Declaration
        VProduct _oVProduct = new VProduct();
        List<VProduct> _oVProducts = new List<VProduct>();
        string _sSQL = "";
        #endregion

        #region Functions
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM View_VProduct";
            string sProductCode= (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[0];
            string sProductName = (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1];
            string sSQL = "";


            #region ProductCode
            if (sProductCode != null)
            {
                if (sProductCode != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " ProductCode LIKE '%" + sProductCode + "%' ";
                }
            }
            #endregion
            #region ProductName
            if (sProductName != null)
            {
                if (sProductName != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " ProductName LIKE '%" + sProductName + "%' ";
                }
            }
            #endregion



            if (sSQL != "")
            {
                _sSQL = _sSQL + sSQL;
            }
        }
        #endregion

        #region Actions
        public ActionResult ViewVProducts(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VProduct).ToString(), (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));
            _oVProducts = new List<VProduct>();
            _oVProducts = VProduct.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oVProducts);
        }        

        public ActionResult ViewVProduct(int id)
        {
            _oVProduct = new VProduct();
            if (id > 0)
            {
             _oVProduct = _oVProduct.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oVProduct);
        }        

        [HttpPost]
        public JsonResult Save(VProduct oVProduct)
        {
            _oVProduct = new VProduct();
            oVProduct.ProductName = oVProduct.ProductName == null ? "" : oVProduct.ProductName;
            oVProduct.ShortName = oVProduct.ShortName == null ? "" : oVProduct.ShortName;
            oVProduct.BrandName = oVProduct.BrandName == null ? "" : oVProduct.BrandName;
            oVProduct.Remarks = oVProduct.Remarks == null ? "" : oVProduct.Remarks;
          
            try
            {
                _oVProduct = oVProduct;
                _oVProduct = _oVProduct.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oVProduct = new VProduct();
                _oVProduct.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVProduct);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(VProduct oVProduct)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oVProduct.Delete(oVProduct.VProductID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Gets(VProduct oVProduct)
        {
            _oVProduct = new VProduct();
            _oVProduct = oVProduct;
            List<VProduct> oVProducts = new List<VProduct>();
            this.MakeSQL(oVProduct.ErrorMessage);
            oVProducts = VProduct.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByCodeOrName(VProduct oVProduct)
        {
            List<VProduct> oVProducts = new List<VProduct>();
            if (!String.IsNullOrEmpty(oVProduct.NameCode))
            {
                oVProduct.NameCode = oVProduct.NameCode.Trim();
                oVProducts = VProduct.GetsByCodeOrName(oVProduct, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oVProducts = VProduct.Gets( (int)Session[SessionInfo.currentUserID]);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVProducts);
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

        #region Import From Excel
        private List<VProduct> GetVProductsFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<VProduct> oVProducts = new List<VProduct>();
            VProduct oVProduct = new VProduct();
            if (PostedFile.ContentLength > 0)
            {
                fileExtension = System.IO.Path.GetExtension(PostedFile.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    fileDirectory = Server.MapPath("~/Content/") + PostedFile.FileName;
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                    PostedFile.SaveAs(fileDirectory);
                    string excelConnectionString = string.Empty;
                    //connection String for xls file format.
                    //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                    ////excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }
                    excelConnection.Close();
                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}]", "Products$");
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    oRows = ds.Tables[0].Rows;

                    for (int i = 0; i < oRows.Count; i++)
                    {
                        oVProduct = new VProduct();
                        oVProduct.ProductName = Convert.ToString(oRows[i]["ProductName"] == DBNull.Value ? "" : oRows[i]["ProductName"]);                       
                        oVProduct.ShortName = Convert.ToString(oRows[i]["ShortName"] == DBNull.Value ? "" : oRows[i]["ShortName"]);
                        oVProduct.BrandName = Convert.ToString(oRows[i]["BrandName"] == DBNull.Value ? "" : oRows[i]["BrandName"]);
                        oVProduct.Remarks = Convert.ToString(oRows[i]["Remarks"] == DBNull.Value ? "" : oRows[i]["Remarks"]);                       
                        oVProducts.Add(oVProduct);
                    }
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                }
                else
                {
                    throw new Exception("File not supported");
                }
            }
            return oVProducts;
        }
        
        public ActionResult ImportFromExcel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportFromExcel(HttpPostedFileBase fileVProducts)
        {
            List<VProduct> oVProducts = new List<VProduct>();
            VProduct oVProduct = new VProduct();

            try
            {
                if (fileVProducts == null) { throw new Exception("File not Found"); }
                oVProducts = this.GetVProductsFromExcel(fileVProducts);
                foreach (VProduct oItem in oVProducts)
                {
                    oVProduct = oItem.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                return View();
            }
            return RedirectToAction("ViewVProducts", "VProduct", new { menuid = (int)Session[SessionInfo.MenuID] });
        }

        public ActionResult DownloadFormat(int ift)
        {
            ImportFormat oImportFormat = new ImportFormat();
            try
            {
                oImportFormat = ImportFormat.GetByType((EnumImportFormatType)ift, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oImportFormat.AttatchFile != null)
                {
                    var file = File(oImportFormat.AttatchFile, oImportFormat.FileType);
                    file.FileDownloadName = oImportFormat.AttatchmentName;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oImportFormat.AttatchmentName);
            }
        }
        #endregion
    }
}
