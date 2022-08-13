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
    public class VoucherBatchController : Controller
    {
        #region Declaration
        VoucherBatch _oVoucherBatch = new VoucherBatch();
        List<VoucherBatch> _oVoucherBatchs = new List<VoucherBatch>();
        string _sErrorMessage = "";
        string _sSQL = "";
        #endregion
        #region Functions
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM View_VoucherBatch";
            string sBatchNO = (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[0];
            //string sName = (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1];
            //string sVoucherBatchIDs = _bForPrint ? (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1] : "";
            string sSQL = "";


            #region BatchNO
            if (sBatchNO != null)
            {
                if (sBatchNO != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " BatchNO LIKE '%" + sBatchNO + "%' ";
                }
            }
            #endregion
            //#region Name
            //if (sName != null)
            //{
            //    if (sName != "")
            //    {
            //        Global.TagSQL(ref sSQL);
            //        sSQL = sSQL + " Name LIKE '%" + sName + "%' ";
            //    }
            //}
            //#endregion
            //#region VoucherBatchIDs
            //if (sVoucherBatchIDs != null)
            //{
            //    if (sVoucherBatchIDs != "")
            //    {
            //        Global.TagSQL(ref sSQL);
            //        sSQL = sSQL + " VoucherBatchID IN (" + sVoucherBatchIDs + ") ";
            //    }
            //}
            //#endregion
            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }
        }
        #endregion

        #region Used Code
        public ActionResult ViewVoucherBatchs( string gfdb, int menuid)
        {
        
            if (gfdb == null)
            {
                this.Session.Remove(SessionInfo.MenuID);
                this.Session.Add(SessionInfo.MenuID, menuid);
                this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VoucherBatch).ToString(), (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));

                _oVoucherBatchs = new List<VoucherBatch>();
                _oVoucherBatchs = VoucherBatch.GetsByCreateBy(((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            return View(_oVoucherBatchs);
        }

        public ActionResult ViewVoucherBatchHistorys(int id)
        {
            List<VoucherBatchHistory> oVoucherBatchHistorys = new List<VoucherBatchHistory>();
            if (id > 0)
            {
                
                oVoucherBatchHistorys = VoucherBatchHistory.GetsByBatchID(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            return View(oVoucherBatchHistorys);
        }
        //[HttpPost]
        public ActionResult ViewVoucherBatch(int id)
        {
            _oVoucherBatch = new VoucherBatch();
            if (id > 0)
            {
                _oVoucherBatch = _oVoucherBatch.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                ViewBag.VBs = VoucherBatch.GetsTransferTo(_oVoucherBatch.VoucherBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oVoucherBatch.Vouchers = Voucher.Gets("SELECT * FROM View_Voucher AS V WHERE V.BatchID=" + _oVoucherBatch.VoucherBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            return View(_oVoucherBatch);        
        }

        [HttpPost]
        public JsonResult BatchSearch(VoucherBatch oVoucherBatch)
        {
            List<VoucherBatch> oVoucherBatchs = new List<VoucherBatch>();
            string sSQL = "SELECT * FROM View_VoucherBatch WHERE CreateBy=" + ((int)this.Session[SessionInfo.currentUserID]).ToString() + "  AND CONVERT(DATE,CONVERT(VARCHAR(12),CreateDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oVoucherBatch.CreateDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oVoucherBatch.RequestDate.ToString("dd MMM yyyy") + "',106)) ORDER BY CreateDate ASC";
            oVoucherBatchs = VoucherBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVoucherBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        


        [HttpPost]
        public JsonResult Gets(VoucherBatch oVoucherBatch)
        {
            List<VoucherBatch> oVoucherBatchs = new List<VoucherBatch>();
            oVoucherBatch.ErrorMessage = oVoucherBatch.ErrorMessage == null ? "" : oVoucherBatch.ErrorMessage;
            this.MakeSQL(oVoucherBatch.ErrorMessage);

            oVoucherBatchs = VoucherBatch.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVoucherBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsRequestTo(User oUser)
        {
            List<User> oUsers = new List<User>();
            _sSQL = "SELECT * FROM View_User WHERE UserID NOT IN(" + ((User)Session[SessionInfo.CurrentUser]).UserID + ",-9) AND UserName like '%" + oUser.ErrorMessage + "%'";

            oUsers = ESimSol.BusinessObjects.User.GetsBySql(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oUsers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get(VoucherBatch oVoucherBatch)
        {
            _oVoucherBatch = new VoucherBatch();
            try
            {
                if (oVoucherBatch.VoucherBatchID > 0)
                {
                    _oVoucherBatch = _oVoucherBatch.Get(oVoucherBatch.VoucherBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oVoucherBatch = new VoucherBatch();
                _oVoucherBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVoucherBatch);
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

        [HttpPost]
        public JsonResult Save(VoucherBatch oVoucherBatch)
        {
            _oVoucherBatch = new VoucherBatch();
            oVoucherBatch.BatchNO = oVoucherBatch.BatchNO == null ? "" : oVoucherBatch.BatchNO;

            try
            {
                _oVoucherBatch = oVoucherBatch;
                _oVoucherBatch = _oVoucherBatch.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oVoucherBatch = new VoucherBatch();
                _oVoucherBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVoucherBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateStatus(VoucherBatch oVoucherBatch)
        {
            _oVoucherBatch = new VoucherBatch();
            oVoucherBatch.BatchNO = oVoucherBatch.BatchNO == null ? "" : oVoucherBatch.BatchNO;

            try
            {
                _oVoucherBatch = oVoucherBatch;
                _oVoucherBatch = _oVoucherBatch.UpdateStatus(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oVoucherBatch = new VoucherBatch();
                _oVoucherBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVoucherBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult VoucherBatchTransfer(VoucherBatch oVoucherBatch)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oVoucherBatch.VoucherBatchTransfer(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Delete(VoucherBatch oVoucherBatch)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oVoucherBatch.Delete(oVoucherBatch.VoucherBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public ActionResult PrintVoucherBatchs(FormCollection DataCollection)
        {
            _oVoucherBatchs = new List<VoucherBatch>();
            _oVoucherBatchs = new JavaScriptSerializer().Deserialize<List<VoucherBatch>>(DataCollection["txtVoucherBatchCollectionList"]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            string Messge = "Voucher Batch List";
            rptVoucherBatchs oReport = new rptVoucherBatchs();
            byte[] abytes = oReport.PrepareReport(_oVoucherBatchs, oCompany, Messge);
            return File(abytes, "application/pdf");

        }

        [HttpPost]
        public ActionResult PrintVoucherBatchHistorys(FormCollection DataCollection)
        {
            List<VoucherBatchHistory> oVoucherBatchHistorys = new List<VoucherBatchHistory>();
            oVoucherBatchHistorys = new JavaScriptSerializer().Deserialize<List<VoucherBatchHistory>>(DataCollection["txtVoucherBatchHistoryCollectionList"]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            string Messge = "History of Batch No: " + oVoucherBatchHistorys[0].BatchNO == null ? "" : oVoucherBatchHistorys[0].BatchNO;
            rptVoucherBatchHistorys oReport = new rptVoucherBatchHistorys();
            byte[] abytes = oReport.PrepareReport(oVoucherBatchHistorys, oCompany, Messge);
            return File(abytes, "application/pdf");

        }
        #endregion




        
        public ActionResult ViewVoucherBatchPicker(string sName, double nts)
        {
            _oVoucherBatchs = new List<VoucherBatch>();
            _oVoucherBatch = new VoucherBatch();
            string sSql = "";
            try
            {
                if (string.IsNullOrEmpty(sName))
                {
                     sSql = "SELECT * FROM VoucherBatch order by Name ";
                }
                else
                {
                     sSql = "SELECT * FROM VoucherBatch WHERE Name LIKE '%" + sName + "%'";
                }
                _oVoucherBatchs = VoucherBatch.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oVoucherBatchs.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oVoucherBatchs = new List<VoucherBatch>();
                _oVoucherBatch.ErrorMessage = ex.Message;
                _oVoucherBatchs.Add(_oVoucherBatch);
            }
            return PartialView(_oVoucherBatchs);
        }

       

        public ActionResult PrintVoucherBatchsInXL()
        {
            //_productsServices = new ProductsServices();
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<VoucherBatchXL>));

            //We load the data
            List<VoucherBatch> oVoucherBatchs = VoucherBatch.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            int nCount = 0; double nTotal = 0;
            VoucherBatchXL oVoucherBatchXL = new VoucherBatchXL();
            List<VoucherBatchXL> oVoucherBatchXLs = new List<VoucherBatchXL>();
            foreach (VoucherBatch oItem in oVoucherBatchs)
            {
                nCount++;
                oVoucherBatchXL = new VoucherBatchXL();
                oVoucherBatchXL.SLNo = nCount.ToString();
                oVoucherBatchXL.BatchNO = oItem.BatchNO;
                oVoucherBatchXL.BatchStatusInString = oItem.BatchStatusInString;
                oVoucherBatchXL.CreateByName = oItem.CreateByName;
                oVoucherBatchXL.RequestToName = oItem.RequestToName;
                oVoucherBatchXLs.Add(oVoucherBatchXL);
                nTotal = nTotal + nCount;
            }           

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oVoucherBatchXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Orders.xls");
        }

       
    }
}
