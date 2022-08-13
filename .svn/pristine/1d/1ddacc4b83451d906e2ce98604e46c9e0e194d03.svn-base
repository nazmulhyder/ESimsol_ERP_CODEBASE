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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Dynamic;

namespace ESimSolFinancial.Controllers
{
    public class FabricPlanningController : Controller
    {
        #region Declaration
        FabricSCReport _oFabricSCReport = new FabricSCReport();
        FabricPlanning _oFabricPlanning = new FabricPlanning();
        List<FabricPlanning> _oFabricPlannings = new List<FabricPlanning>();
        //List<FabricPlanCount> _oFabricPlanCounts = new List<FabricPlanCount>();
        #endregion

        #region Planning
        public ActionResult ViewFabricPlannings(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFabricPlannings = new List<FabricPlanning>();
            _oFabricPlannings = FabricPlanning.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            return View(_oFabricPlannings);
        }

        public ActionResult ViewFabricPlanning(int id, int nFSCDID, double ts)
        {
            FabricSCReport oFabricSCReport = new FabricSCReport();
            List<dynamic> oDynamics = new List<dynamic>();

            _oFabricPlannings = new List<FabricPlanning>();
            if (nFSCDID > 0)
            {
                oFabricSCReport = oFabricSCReport.Get(nFSCDID, (int)Session[SessionInfo.currentUserID]);
                _oFabricPlannings = FabricPlanning.Gets("SELECT * FROM View_FabricPlanning WHERE FabricID =" + id + " Order By IsWarp DESC", (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oFabricPlannings = FabricPlanning.Gets("SELECT * FROM View_FabricPlanning WHERE FabricID =" + id + " Order By  IsWarp DESC", (int)Session[SessionInfo.currentUserID]);
            }
            if (_oFabricPlannings.Count > 0)
            {
                if (_oFabricPlannings.FirstOrDefault() != null && _oFabricPlannings.FirstOrDefault().FabricPlanningID > 0 && _oFabricPlannings.Where(x => x.ComboNo > 0).Count() > 0)
                {
                    List<FabricPlanning> oFabricPlannings = new List<FabricPlanning>();
                    _oFabricPlannings.ForEach((item) => { oFabricPlannings.Add(item); });
                    _oFabricPlannings = this.ComboFabricPlannings(_oFabricPlannings);
                    _oFabricPlannings[0].CellRowSpans = this.RowMerge(oFabricPlannings);
                }

                //int nCount = 0;
                //foreach (var oItem in _oFabricPlanCounts)
                //{
                //    dynamic obj = new ExpandoObject();
                //    var expobj = obj as IDictionary<string, object>;
                //    expobj.Add("Count" + (++nCount), dtFrom.ToString("dd MMM yyyy"));
                //    grpColumn.ForEach(x =>
                //    {
                //        expobj.Add(x.Column, oResult.Where(p => p.LotNo == x.LotNo && p.ProductName == x.ProductName).Sum(p => p.NetWeight));
                //    });
                //    oDynamics.Add(expobj);
                //    dtFrom = dtFrom.AddDays(1);
                //}
            }

            ViewBag.FabricID = id;
            ViewBag.FabricSCReport = oFabricSCReport;

            return View(_oFabricPlannings);
        }

        [HttpPost]
        public JsonResult SavePlanning(FabricPlanning oFabricPlanning)
        {
            _oFabricPlanning = new FabricPlanning();
            try
            {
                _oFabricPlanning = oFabricPlanning;
                _oFabricPlanning = _oFabricPlanning.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricPlanning = new FabricPlanning();
                _oFabricPlanning.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricPlanning);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePlanning(FabricPlanning oFabricPlanning)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricPlanning.Delete(oFabricPlanning.FabricPlanningID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintFabricPlanning(int nFSCDID, int buid)
        {
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            _oFabricSCReport = _oFabricSCReport.Get(nFSCDID, (int)Session[SessionInfo.currentUserID]);

            _oFabricPlannings = FabricPlanning.Gets("SELECT * FROM View_FabricPlanning WHERE FabricID =" + _oFabricSCReport.FabricID + " Order By IsWarp DESC", (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oFabricPlannings.Any())
            {
                rptFabricPlanning oReport = new rptFabricPlanning();
                byte[] abytes = oReport.PrepareReport(_oFabricPlannings, _oFabricSCReport, oCompany, oBusinessUnit, "");
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("Nothing To Print");
                return File(abytes, "application/pdf");
            }
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

        public ActionResult ViewFabricPlanningDetail(int nFabricPlanningID)
        {
            List<FabricPlanningDetail> oFabricPlanningDetails = new List<FabricPlanningDetail>();
            //oFabricPlanningDetails = FabricPlanningDetail.Gets((int)Session[SessionInfo.currentUserID]);
            return View(oFabricPlanningDetails);
        }

        #endregion

        #region Fabric Plan Count
        
            


             //         $scope.gridOptions.columnDefs=$scope.GetColumns(results.Item1);
             //                             var data=[];
             //                             angular.forEach(results.Item2, function(objs,index){
             //                                 var dynamicObj={};
             //                                 angular.forEach(objs, function(item,position){
             //                                     dynamicObj[item.Key]=item.Value;
             //                                 });
             //                                 data.push(dynamicObj);
             //                             });
             //                             $scope.gridOptions.data=data;
        #endregion


        #region Merger Row

        private List<CellRowSpan> RowMerge(List<FabricPlanning> oFabricPlannings)
        {

            List<CellRowSpan> oCellRowSpans = new List<CellRowSpan>();
            int[] rowIndex = new int[1];
            int[] rowSpan = new int[1];

            List<FabricPlanning> oTWGLDDetails = new List<FabricPlanning>();
            List<FabricPlanning> oLDDetails = new List<FabricPlanning>();
            List<FabricPlanning> oTempLDDetails = new List<FabricPlanning>();

            oTWGLDDetails = oFabricPlannings.Where(x => x.ComboNo > 0).ToList();
            oLDDetails = oFabricPlannings.Where(x => x.ComboNo == 0).ToList();

            while (oFabricPlannings.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oFabricPlannings.FirstOrDefault().FabricPlanningID == oTWGLDDetails.FirstOrDefault().FabricPlanningID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.ComboNo == oTWGLDDetails.FirstOrDefault().ComboNo).ToList();

                    oFabricPlannings.RemoveAll(x => x.ComboNo == oTempLDDetails.FirstOrDefault().ComboNo);
                    oTWGLDDetails.RemoveAll(x => x.ComboNo == oTempLDDetails.FirstOrDefault().ComboNo);

                }
                else if (oLDDetails.FirstOrDefault() != null && oFabricPlannings.FirstOrDefault().FabricPlanningID == oLDDetails.FirstOrDefault().FabricPlanningID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.FabricPlanningID == oLDDetails.FirstOrDefault().FabricPlanningID).ToList();

                    oFabricPlannings.RemoveAll(x => x.FabricPlanningID == oTempLDDetails.FirstOrDefault().FabricPlanningID);
                    oLDDetails.RemoveAll(x => x.FabricPlanningID == oTempLDDetails.FirstOrDefault().FabricPlanningID);
                }

                rowIndex[0] = rowIndex[0] + rowSpan[0];
                rowSpan[0] = oTempLDDetails.Count();
                oCellRowSpans.Add(MakeSpan.GenerateRowSpan("ComboNo", rowIndex[0], rowSpan[0]));
            }
            return oCellRowSpans;
        }
        private List<FabricPlanning> ComboFabricPlannings(List<FabricPlanning> oFabricPlannings)
        {
            List<FabricPlanning> oTwistedLDDetails = new List<FabricPlanning>();
            List<FabricPlanning> oTWGLDDetails = new List<FabricPlanning>();
            List<FabricPlanning> oLDDetails = new List<FabricPlanning>();
            List<FabricPlanning> oTempLDDetails = new List<FabricPlanning>();

            oTWGLDDetails = oFabricPlannings.Where(x => x.ComboNo > 0).ToList();
            oLDDetails = oFabricPlannings.Where(x => x.ComboNo == 0).ToList();

            while (oFabricPlannings.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oFabricPlannings.FirstOrDefault().FabricPlanningID == oTWGLDDetails.FirstOrDefault().FabricPlanningID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.ComboNo == oTWGLDDetails.FirstOrDefault().ComboNo).ToList();
                    oFabricPlannings.RemoveAll(x => x.ComboNo == oTempLDDetails.FirstOrDefault().ComboNo);
                    oTWGLDDetails.RemoveAll(x => x.ComboNo == oTempLDDetails.FirstOrDefault().ComboNo);

                }
                else if (oLDDetails.FirstOrDefault() != null && oFabricPlannings.FirstOrDefault().FabricPlanningID == oLDDetails.FirstOrDefault().FabricPlanningID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.FabricPlanningID == oLDDetails.FirstOrDefault().FabricPlanningID).ToList();

                    oFabricPlannings.RemoveAll(x => x.FabricPlanningID == oTempLDDetails.FirstOrDefault().FabricPlanningID);
                    oLDDetails.RemoveAll(x => x.FabricPlanningID == oTempLDDetails.FirstOrDefault().FabricPlanningID);
                }
                oTwistedLDDetails.AddRange(oTempLDDetails);
            }
            return oTwistedLDDetails;
        }
        [HttpPost]
        public JsonResult MakeCombo(FabricPlanning oFabricPlanning)
        {
            FabricPlanning oFabricPlanningRet = new FabricPlanning();
            List<FabricPlanning> oFabricPlannings = new List<FabricPlanning>();
            try
            {
                string sFabricPlanningID = string.IsNullOrEmpty(oFabricPlanning.ErrorMessage) ? "" : oFabricPlanning.ErrorMessage;
                if (sFabricPlanningID == "")
                    throw new Exception("No items found to make Group.");
                if (oFabricPlanning.FabricID <= 0)
                    throw new Exception("No valid Order found.");

                oFabricPlannings = FabricPlanning.MakeCombo(sFabricPlanningID, oFabricPlanning.FabricID, oFabricPlanning.ComboNo, (int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricPlannings.FirstOrDefault() != null && oFabricPlannings.FirstOrDefault().FabricPlanningID > 0 && oFabricPlannings.Where(x => x.ComboNo > 0).Count() > 0)
                {
                    List<FabricPlanning> oTempFabricPlannings = new List<FabricPlanning>();
                    oFabricPlannings.ForEach((item) => { oTempFabricPlannings.Add(item); });
                    oFabricPlannings = this.ComboFabricPlannings(oFabricPlannings);
                    oFabricPlannings[0].CellRowSpans = this.RowMerge(oTempFabricPlannings);
                }
                oFabricPlanningRet.FabricPlannings = oFabricPlannings;

            }
            catch (Exception ex)
            {
                oFabricPlanningRet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPlanningRet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveComboGroup(FabricPlanning oFabricPlanning)
        {
            FabricPlanning oFabricPlanningRet = new FabricPlanning();
            List<FabricPlanning> oFabricPlannings = new List<FabricPlanning>();
            try
            {
                string sFabricPlanningID = string.IsNullOrEmpty(oFabricPlanning.ErrorMessage) ? "" : oFabricPlanning.ErrorMessage;
                if (sFabricPlanningID == "")
                    throw new Exception("No items found to make twisted.");
                if (oFabricPlanning.FabricID <= 0)
                    throw new Exception("No valid labdip found.");

                oFabricPlannings = FabricPlanning.MakeCombo(sFabricPlanningID, oFabricPlanning.FabricID, oFabricPlanning.ComboNo, (int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oFabricPlannings.FirstOrDefault() != null && oFabricPlannings.FirstOrDefault().FabricPlanningID > 0 && oFabricPlannings.Where(x => x.ComboNo > 0).Count() > 0)
                {
                    List<FabricPlanning> oTempFabricPlannings = new List<FabricPlanning>();
                    oFabricPlannings.ForEach((item) => { oTempFabricPlannings.Add(item); });
                    oFabricPlannings = this.ComboFabricPlannings(oFabricPlannings);
                    oFabricPlannings[0].CellRowSpans = this.RowMerge(oTempFabricPlannings);
                }
                oFabricPlanningRet.FabricPlannings = oFabricPlannings;

            }
            catch (Exception ex)
            {
                oFabricPlanningRet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPlanningRet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
