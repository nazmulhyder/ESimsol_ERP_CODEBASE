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
using System.Security;
using OfficeOpenXml;
using OfficeOpenXml.Style;
namespace ESimSolFinancial.Controllers
{
    public class LotBaseTestController :PdfViewController
    {
        #region Lot Base Test
        public ActionResult View_LotBaseTest(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<LotBaseTest> oLotBaseTests = new List<LotBaseTest>();
            string sSQL = "Select * from LotBaseTest";
            oLotBaseTests = LotBaseTest.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(oLotBaseTests);

        }

        [HttpPost]
        public JsonResult Save(LotBaseTest oLotBaseTest)
        {
            try
            {
                if (oLotBaseTest.LotBaseTestID <= 0)
                {
                    oLotBaseTest = oLotBaseTest.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oLotBaseTest = oLotBaseTest.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oLotBaseTest = new LotBaseTest();
                oLotBaseTest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLotBaseTest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(LotBaseTest oLotBaseTest)
        {
            try
            {
                if (oLotBaseTest.LotBaseTestID <= 0) { throw new Exception("Please select an valid item."); }
                oLotBaseTest = oLotBaseTest.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLotBaseTest = new LotBaseTest();
                oLotBaseTest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLotBaseTest.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get(LotBaseTest oLotBaseTest)
        {
            try
            {
                if (oLotBaseTest.LotBaseTestID <= 0)
                    throw new Exception("Select a valid item from list");

                oLotBaseTest = LotBaseTest.Get(oLotBaseTest.LotBaseTestID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLotBaseTest = new LotBaseTest();
                oLotBaseTest.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLotBaseTest);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add LotBaseTestResult
        public ActionResult View_LotBaseTestResults(int buid ,int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<LotBaseTestResult> oLotBaseTests = new List<LotBaseTestResult>();
            List<Lot> oLots = new List<Lot>();
           // string sSQL = "Select top(100)* from View_Lot Where LotNo Like '%t%'";
            oLots = new List<Lot>();// Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Lot = oLots;
            ViewBag.BUID = buid;
            return View(oLotBaseTests);
        }
        public ActionResult View_AddLotBaseTestResults(int nId, double ts ,int nbuid)
        {
            LotBaseTestResult oLotBaseTestResult = new LotBaseTestResult();
            string sSQL = string.Empty;
            Lot oLot = new Lot();
            List<LotBaseTestResult> oLotBaseTestResults = new List<LotBaseTestResult>();
            if (nId > 0)
            {
                oLot = oLot.Get(nId, (int)Session[SessionInfo.currentUserID]);
                sSQL = "Select * from View_LotBaseTestResult Where  LotID IN ( " + nId + " )";
                oLotBaseTestResults = LotBaseTestResult.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.LotBaseTest = LotBaseTest.Gets("Select * from LotBaseTest Where Activity <> 0", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ParentLot = oLot;
            ViewBag.LotBaseTestResults = oLotBaseTestResults;
            return View(oLotBaseTestResult);
        }
        [HttpPost]
        public JsonResult SaveLotBaseTestResult(LotBaseTestResult oLotBaseTestResult)
        {
           
           try
            {
                if (oLotBaseTestResult.LotBaseTestResults.Any())
                {

                    foreach (var oitem in oLotBaseTestResult.LotBaseTestResults)
                    {
                        if(oitem.LotBaseTestResultID>0)
                        {
                            oLotBaseTestResult = oitem.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                        else
                        {
                            oLotBaseTestResult = oitem.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                    }
                }
              

            }
            catch (Exception ex)
            {
                oLotBaseTestResult = new LotBaseTestResult();
                oLotBaseTestResult.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLotBaseTestResult);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintLotBaseTestResult(int nID,double nts,int nbuid )
        {
            List<LotBaseTestResult> oLotBaseTestResults = new List<LotBaseTestResult>();
            Lot oLot = new Lot();
            string sSQL = "";
            oLot = oLot.Get(nID, (int)Session[SessionInfo.currentUserID]);
            sSQL = "Select * from View_LotBaseTestResult Where LotID IN ( " + nID + " )";
            oLotBaseTestResults = LotBaseTestResult.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nbuid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptLotBaseTestResult oReport = new rptLotBaseTestResult();
            byte[] abytes = oReport.PrepareReport(oLot, oLotBaseTestResults, oCompany, oBusinessUnit);
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

        [HttpPost]
        public JsonResult GetsLot(Lot oLot)
        {
            List<Lot> _oLots = new List<Lot>();
            try
            {
                string sSQL = "select top(100)* from View_Lot";
                string sReturn = "";
                string STemp = oLot.Params;
                if (!string.IsNullOrEmpty(STemp))
                {
                    string sLotNo = STemp.Split('~')[0];
                    bool chkRawLot =Convert.ToBoolean( STemp.Split('~')[1]);
                    bool chkFinishLot =Convert.ToBoolean( STemp.Split('~')[2]);
                    int nBUID =Convert.ToInt32( STemp.Split('~')[3]);
                    if (!String.IsNullOrEmpty(sLotNo))
                    {
                        oLot.LotNo = oLot.LotNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " LotNo Like'%" + sLotNo + "%'";
                    }
                    if (nBUID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "BUID=" + nBUID;
                    }
                    if(chkRawLot)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "ParentType IN ( 101 , 103) " ;
                    }
                    if (chkFinishLot)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "ParentType IN (106) ";
                    }

                }
                sSQL = sSQL + "" + sReturn;
                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                //_oLotTraking = new Lot();
                //oRPT_DeliverySummary.ErrorMessage = ex.Message;
                _oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsActiveLotBaseTest()
        {
            List<LotBaseTest> oLotBaseTests = new List<LotBaseTest>();
            try
            {
                string sSQL = "select * from LotBaseTest Where Activity <>0";
                oLotBaseTests =LotBaseTest.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oLotBaseTests = new List<LotBaseTest>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLotBaseTests);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteLotBaseTestResult(LotBaseTestResult oLotBaseTestResult)
        {
            try
            {
                if (oLotBaseTestResult.LotBaseTestResultID <= 0) { throw new Exception("Please select an valid item."); }
                oLotBaseTestResult = oLotBaseTestResult.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLotBaseTestResult = new LotBaseTestResult();
                oLotBaseTestResult.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLotBaseTestResult.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}