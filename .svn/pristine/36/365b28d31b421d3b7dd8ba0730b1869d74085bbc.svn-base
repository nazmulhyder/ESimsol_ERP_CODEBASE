using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class BlockMachineMappingController : Controller
    {
        #region Declaration
        BlockMachineMapping _oBlockMachineMapping;
        private List<BlockMachineMapping> _oBlockMachineMappings;
        #endregion

        #region Views

        public ActionResult View_BlockMachineMappings(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oBlockMachineMappings = new List<BlockMachineMapping>();
            return View(_oBlockMachineMappings);
        }

        public ActionResult View_BlockMachineMapping(int nId, double ts)
        {
            _oBlockMachineMapping = new BlockMachineMapping();

            if (nId > 0)
            {
                _oBlockMachineMapping = BlockMachineMapping.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                string sDetailSql = "SELECT * FROM BlockMachineMappingDetail WHERE BMMID=" + nId;
                _oBlockMachineMapping.BlockMachineMappingDetails = BlockMachineMappingDetail.Gets(sDetailSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }

            return PartialView(_oBlockMachineMapping);
        }

        #endregion

        #region IUD
        [HttpPost]
        public JsonResult BlockMachineMapping_IU(BlockMachineMapping oBlockMachineMapping)
        {

            _oBlockMachineMapping = new BlockMachineMapping();
            try
            {
                _oBlockMachineMapping = oBlockMachineMapping;
                _oBlockMachineMapping.ProductionProcess = (EnumProductionProcess)oBlockMachineMapping.ProductionProcessInt;


                if (_oBlockMachineMapping.BMMID > 0)
                {
                    _oBlockMachineMapping = _oBlockMachineMapping.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oBlockMachineMapping = _oBlockMachineMapping.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oBlockMachineMapping = new BlockMachineMapping();
                _oBlockMachineMapping.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBlockMachineMapping);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BlockMachineMapping_Delete(int nId, double ts)//nId=BMMID
        {
            _oBlockMachineMapping = new BlockMachineMapping();
            try
            {

                _oBlockMachineMapping.BMMID = nId;
                _oBlockMachineMapping = _oBlockMachineMapping.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oBlockMachineMapping = new BlockMachineMapping();
                _oBlockMachineMapping.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBlockMachineMapping.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BlockMachineMappingDetail_IU(BlockMachineMappingDetail oBlockMachineMappingDetail)
        {

            try
            {
                if (oBlockMachineMappingDetail.BMMDID > 0)
                {
                    oBlockMachineMappingDetail = oBlockMachineMappingDetail.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oBlockMachineMappingDetail = oBlockMachineMappingDetail.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oBlockMachineMappingDetail = new BlockMachineMappingDetail();
                oBlockMachineMappingDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBlockMachineMappingDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BlockMachineMappingDetail_Delete(int nId, double ts)//nId=BMMDID
        {
            BlockMachineMappingDetail oBlockMachineMappingDetail = new BlockMachineMappingDetail();
            try
            {

                oBlockMachineMappingDetail.BMMDID = nId;
                oBlockMachineMappingDetail = oBlockMachineMappingDetail.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oBlockMachineMappingDetail = new BlockMachineMappingDetail();
                oBlockMachineMappingDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBlockMachineMappingDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Search
        [HttpPost]
        public JsonResult BlockMachineMapping_Search(BlockMachineMapping oBlockMachineMapping)
        {

            string sSQL = "";
            sSQL = "SELECT * FROM View_BlockMachineMapping WHERE BMMID <>0";
            if (oBlockMachineMapping.ProductionProcessInt != 0)
            {
                sSQL = sSQL + "AND ProductionProcess = " + oBlockMachineMapping.ProductionProcessInt;
            }
            if (oBlockMachineMapping.DepartmentID != 0)
            {
                sSQL = sSQL + "AND DepartmentID = " + oBlockMachineMapping.DepartmentID;
            }
            if (oBlockMachineMapping.BlockName != "" && oBlockMachineMapping.BlockName != null)
            {
                sSQL = sSQL + " AND BlockName = '" + oBlockMachineMapping.BlockName + "'";
            }
            //if (oBlockMachineMapping.MachineNo != "" && oBlockMachineMapping.MachineNo != null)// property has been changed . search requires modification from js .
            //{
            //    sSQL = sSQL + "AND MachineNo = '" + oBlockMachineMapping.MachineNo + "'";           
            //}
            _oBlockMachineMappings = Search(sSQL);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBlockMachineMappings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public List<BlockMachineMapping> Search(string sSQL)
        {
            _oBlockMachineMappings = new List<BlockMachineMapping>();
            try
            {
                _oBlockMachineMappings = BlockMachineMapping.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oBlockMachineMappings.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }

            }
            catch (Exception ex)
            {
                _oBlockMachineMapping = new BlockMachineMapping();
                _oBlockMachineMappings = new List<BlockMachineMapping>();
                _oBlockMachineMapping.ErrorMessage = ex.Message;
                _oBlockMachineMappings.Add(_oBlockMachineMapping);
            }
            return _oBlockMachineMappings;
        }

        #endregion

        #region Activity
        [HttpPost]
        public JsonResult BlockMachineMapping_Activity(BlockMachineMapping oBlockMachineMapping)
        {
            _oBlockMachineMapping = new BlockMachineMapping();
            try
            {

                _oBlockMachineMapping = oBlockMachineMapping;
                _oBlockMachineMapping = BlockMachineMapping.Activite(_oBlockMachineMapping.BMMID, _oBlockMachineMapping.IsActive, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oBlockMachineMapping = new BlockMachineMapping();
                _oBlockMachineMapping.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBlockMachineMapping);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BlockMachineMappingDetail_Activity(BlockMachineMappingDetail oBlockMachineMappingDetail)
        {
            try
            {
                oBlockMachineMappingDetail = BlockMachineMappingDetail.Activite(oBlockMachineMappingDetail.BMMDID, oBlockMachineMappingDetail.IsActive, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oBlockMachineMappingDetail = new BlockMachineMappingDetail();
                oBlockMachineMappingDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBlockMachineMappingDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region supervisor Assign
        public ActionResult View_SupervisorAssign(int nId, double ts)//BMMID
        {
            List<BlockMachineMappingSupervisor> oBlockMachineMappingSupervisors = new List<BlockMachineMappingSupervisor>();

            if (nId > 0)
            {
                string sDetailSql = "SELECT * FROM View_BlockMachineMappingSupervisor WHERE BMMID=" + nId;
                oBlockMachineMappingSupervisors = BlockMachineMappingSupervisor.Gets(sDetailSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }

            return PartialView(oBlockMachineMappingSupervisors);
        }

        [HttpPost]
        public JsonResult BlockMachineMappingSupervisor_IU(BlockMachineMappingSupervisor oBlockMachineMappingSupervisor)
        {
            try
            {
                if (oBlockMachineMappingSupervisor.BMMSID > 0)
                {
                    oBlockMachineMappingSupervisor = oBlockMachineMappingSupervisor.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oBlockMachineMappingSupervisor = oBlockMachineMappingSupervisor.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oBlockMachineMappingSupervisor = new BlockMachineMappingSupervisor();
                oBlockMachineMappingSupervisor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBlockMachineMappingSupervisor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult BlockMachineMappingSupervisorDelete(BlockMachineMappingSupervisor oBlockMachineMappingSupervisor)
        {
            try
            {
                oBlockMachineMappingSupervisor = oBlockMachineMappingSupervisor.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oBlockMachineMappingSupervisor = new BlockMachineMappingSupervisor();
                oBlockMachineMappingSupervisor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBlockMachineMappingSupervisor.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region oBlockMachineMappingSupervisor_Inactive
        [HttpPost]
        public JsonResult BlockMachineMappingSupervisor_Inactive(BlockMachineMappingSupervisor oBlockMachineMappingSupervisor)
        {
            try
            {
                if (oBlockMachineMappingSupervisor.BMMSID<0) { throw new Exception("Please select a valid block machine mapping supervisor."); }
                else if (oBlockMachineMappingSupervisor.StartDate > oBlockMachineMappingSupervisor.EndDate){ throw new Exception("Start date never greater than End Date.");}
                //for inactivation
                oBlockMachineMappingSupervisor=oBlockMachineMappingSupervisor.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oBlockMachineMappingSupervisor = new BlockMachineMappingSupervisor();
                oBlockMachineMappingSupervisor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBlockMachineMappingSupervisor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion oBlockMachineMappingSupervisor_Inactive
        #endregion supervisor Assign

        #region Block Macine Mapping Report
        public ActionResult View_BlockMachineMappingReports(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<BlockMachineMappingReport> oBlockMachineMappingReports = new List<BlockMachineMappingReport>();
            //BlockMachineMappingReport oBlockMachineMappingReport = new BlockMachineMappingReport();
            //oBlockMachineMappingReports.Add(oBlockMachineMappingReport);
            //oBlockMachineMappingReports[0].BlockMachineMappings = BlockMachineMapping.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(oBlockMachineMappingReports);
        }
        public ActionResult BlockPikerByName(string sBlockName, double ts)
        {
            _oBlockMachineMappings = new List<BlockMachineMapping>();
            _oBlockMachineMapping = new BlockMachineMapping();
            try
            {
                string sSql = "SELECT * FROM View_BlockMachineMapping WHERE BMMID != 0";

                if (sBlockName != "")
                {
                    sSql += " AND BlockName LIKE '%" + sBlockName + "%'";
                }
                _oBlockMachineMappings = BlockMachineMapping.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (_oBlockMachineMappings.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oBlockMachineMappings = new List<BlockMachineMapping>();
                _oBlockMachineMapping.ErrorMessage = ex.Message;
                _oBlockMachineMappings.Add(_oBlockMachineMapping);
            }
            return PartialView(_oBlockMachineMappings);
        }

        [HttpPost]
        public JsonResult BlockMachineMappingReport_Search(string sParams)
        {
            List<BlockMachineMappingReport> oBlockMachineMappingReports = new List<BlockMachineMappingReport>();
            BlockMachineMappingReport oBlockMachineMappingReport = new BlockMachineMappingReport();
            List<BlockMachineMappingReport> oNewBMMRs = new List<BlockMachineMappingReport>();
            try
            {
                oBlockMachineMappingReports = BlockMachineMappingReport.Gets(sParams, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oBlockMachineMappingReports.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
                else
                {
                    var Lists =
                    (from BMMR in oBlockMachineMappingReports
                     group BMMR
                     by new
                     {
                         BMMR.StyleNo,
                         BMMR.GarmentPart,
                         BMMR.GPName,
                         BMMR.ColorName,
                         BMMR.SizeCategoryName,
                         BMMR.DepartmentName,
                         BMMR.SupervisorName,
                         BMMR.BlockName

                     } into newBMMR

                     select new
                     {
                         StyleNo = newBMMR.Key.StyleNo,
                         GarmentPart = newBMMR.Key.GarmentPart,
                         GPName = newBMMR.Key.GPName,
                         ColorName = newBMMR.Key.ColorName,
                         SizeCategoryName = newBMMR.Key.SizeCategoryName,
                         IssueQty = newBMMR.Sum(x => x.IssueQty),
                         RcvQty = newBMMR.Sum(x => x.RcvQty),
                         DepartmentName = newBMMR.Key.DepartmentName,
                         BlockName = newBMMR.Key.BlockName,
                         SupervisorName = newBMMR.Key.SupervisorName,

                     }).ToList();

                    if (Lists.Count > 0)
                    {
                        foreach (var oItem in Lists)
                        {
                            oBlockMachineMappingReport = new BlockMachineMappingReport();
                            oBlockMachineMappingReport.StyleNo = oItem.StyleNo;
                            oBlockMachineMappingReport.GarmentPart = oItem.GarmentPart;
                            oBlockMachineMappingReport.GPName = oItem.GPName;
                            oBlockMachineMappingReport.ColorName = oItem.ColorName;
                            oBlockMachineMappingReport.SizeCategoryName = oItem.SizeCategoryName;
                            oBlockMachineMappingReport.IssueQty = oItem.IssueQty;
                            oBlockMachineMappingReport.RcvQty = oItem.RcvQty;
                            oBlockMachineMappingReport.DepartmentName = oItem.DepartmentName;
                            oBlockMachineMappingReport.BlockName = oItem.BlockName;
                            oBlockMachineMappingReport.SupervisorName = oItem.SupervisorName;

                            oNewBMMRs.Add(oBlockMachineMappingReport);

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                oBlockMachineMappingReport = new BlockMachineMappingReport();
                oNewBMMRs = new List<BlockMachineMappingReport>();
                oBlockMachineMappingReport.ErrorMessage = ex.Message;
                oNewBMMRs.Add(oBlockMachineMappingReport);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oNewBMMRs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintBlockMachine(string sParams)
        {

            BlockMachineMappingReport oBlockMachineMappingReport = new BlockMachineMappingReport();
            List<BlockMachineMappingReport> oBlockMachineMappingReports = new List<BlockMachineMappingReport>();
            List<BlockMachineMappingReport> oNewBMMRs = new List<BlockMachineMappingReport>();
            oBlockMachineMappingReports = BlockMachineMappingReport.Gets(sParams, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            var Lists =
                   (from BMMR in oBlockMachineMappingReports
                    group BMMR
                    by new
                    {
                        BMMR.StyleNo,
                        BMMR.GarmentPart,
                        BMMR.GPName,
                        BMMR.ColorName,
                        BMMR.SizeCategoryName,
                        BMMR.DepartmentName,
                        BMMR.SupervisorName,
                        BMMR.BlockName

                    } into newBMMR

                    select new
                    {
                        StyleNo = newBMMR.Key.StyleNo,
                        GarmentPart = newBMMR.Key.GarmentPart,
                        GPName = newBMMR.Key.GPName,
                        ColorName = newBMMR.Key.ColorName,
                        SizeCategoryName = newBMMR.Key.SizeCategoryName,
                        IssueQty = newBMMR.Sum(x => x.IssueQty),
                        RcvQty = newBMMR.Sum(x => x.RcvQty),
                        DepartmentName = newBMMR.Key.DepartmentName,
                        BlockName = newBMMR.Key.BlockName,
                        SupervisorName = newBMMR.Key.SupervisorName,

                    }).ToList();

            if (Lists.Count > 0)
            {
                foreach (var oItem in Lists)
                {
                    oBlockMachineMappingReport = new BlockMachineMappingReport();
                    oBlockMachineMappingReport.StyleNo = oItem.StyleNo;
                    oBlockMachineMappingReport.GarmentPart = oItem.GarmentPart;
                    oBlockMachineMappingReport.GPName = oItem.GPName;
                    oBlockMachineMappingReport.ColorName = oItem.ColorName;
                    oBlockMachineMappingReport.SizeCategoryName = oItem.SizeCategoryName;
                    oBlockMachineMappingReport.IssueQty = oItem.IssueQty;
                    oBlockMachineMappingReport.RcvQty = oItem.RcvQty;
                    oBlockMachineMappingReport.DepartmentName = oItem.DepartmentName;
                    oBlockMachineMappingReport.BlockName = oItem.BlockName;
                    oBlockMachineMappingReport.SupervisorName = oItem.SupervisorName;

                    oNewBMMRs.Add(oBlockMachineMappingReport);

                }

            }

            oBlockMachineMappingReport.BlockMachineMappingReports = oNewBMMRs;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oBlockMachineMappingReport.Company = oCompanys.First();
            oBlockMachineMappingReport.ErrorMessage = sParams.Split('~')[0] + "," + sParams.Split('~')[1];

            rptBlockMacineWiseReport oReport = new rptBlockMacineWiseReport();
            byte[] abytes = oReport.PrepareReport(oBlockMachineMappingReport);
            return File(abytes, "application/pdf");

        }

        #endregion

    }
}
