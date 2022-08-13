using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Net;

namespace ESimSolFinancial.Controllers
{
    public class FabricTransferPackingListController : Controller
    {
        #region Declaration
        FabricTransferPackingList _oFTPL = new FabricTransferPackingList();
        List<FabricTransferPackingList> _oFTPLs = new List<FabricTransferPackingList>();

        FabricTransferPackingListDetail _oFTPLD = new FabricTransferPackingListDetail();
        List<FabricTransferPackingListDetail> _oFTPLDs = new List<FabricTransferPackingListDetail>();
        #endregion

        #region Fabric Transfer Packing List
        public ActionResult View_FabricTransferPackingLists(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFTPLs = new List<FabricTransferPackingList>();
            _oFTPLs = FabricTransferPackingList.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFTPLs);
        }
        public ActionResult View_FabricTransferPackingList(string sFTPListID, string sBtnID, string sMsg)
        {
            int nFTPListID = Convert.ToInt32(sFTPListID);
            if (nFTPListID > 0)
            {
                _oFTPL = _oFTPL.Get(nFTPListID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFTPL.FTPLDetails = FabricTransferPackingListDetail.Gets(nFTPListID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFTPL.FTPLDetails = _oFTPL.FTPLDetails.OrderBy(x => x.Grade).ToList();
            }
            ViewBag.BtnIDHtml = sBtnID;
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(0, EnumModuleName.FabricTransferNote, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            //ViewBag.Stores = WorkingUnit.Gets("FabricTransferPackingList", (int)EnumTriggerParentsType._FabricTransferPackingList, (int)EnumOperationFunctionality._Add, (int)EnumInOutType._Disburse, false, 0, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFTPL);
        }
        [HttpPost]
        public JsonResult SaveFTPL(FabricTransferPackingList oFTPL)
        {
            try
            {
                oFTPL = oFTPL.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFTPL = new FabricTransferPackingList();
                oFTPL.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFTPL);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFTPL(FabricTransferPackingList oFTPL)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFTPL.Delete(oFTPL.FTPListID, oFTPL.FTNID,((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetsFabricSalesContractDetail(FabricTransferPackingList oFTPL)
        {
            List<FabricSalesContractDetail> oFEOs = new List<FabricSalesContractDetail>();
            try
            {

                string sSQL = "SELECT * FROM VIEW_FabricSalesContractDetail FEO WHERE  ISNULL(FEO.Status,0)!=" + (int)EnumPOState.Cancel + " And FEO.FabricSalesContractDetailID IN " //FEO.IsInHouse=1 AND
                              + "(SELECT FB.FEOID FROM FabricBatch FB WHERE FB.FBID IN "
                              + "(SELECT FBQC.FBID FROM FabricBatchQC FBQC WHERE FBQC.FBQCID IN "
                              //+ "(SELECT FBQCD.FBQCID FROM FabricBatchQCDetail FBQCD WHERE FBQCD.ReceiveBy>0 "
                                 + "(SELECT FBQCD.FBQCID FROM FabricBatchQCDetail FBQCD WHERE FBQCD.FBQCID>0 "
                              + "AND FBQCD.FBQCDetailID IN "
                              + "(SELECT LB.ParentID FROM Lot LB WHERE LB.ParentType=" + (int)EnumTriggerParentsType.FabricQCDetail + "/*FBQC*/ "
                              + ((oFTPL.StoreID > 0) ? " AND Balance>0 AND  LB.WorkingUnitID = " + oFTPL.StoreID : " ")
                              + " AND LB.LotID NOT IN (SELECT FTPLD.LotID FROM FabricTransferPackingListDetail FTPLD)))))";

                if (!string.IsNullOrEmpty(oFTPL.FEONo))
                {
                    sSQL = sSQL + " AND ExeNo LIKE '%" + oFTPL.FEONo + "%' ";
                }
                sSQL = sSQL + " ORDER BY ExeNo ";

                oFEOs = FabricSalesContractDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            }
            catch (Exception ex)
            {
                FabricSalesContractDetail oFEO = new FabricSalesContractDetail();
                oFEO.ErrorMessage = ex.Message;
                oFEOs.Add(oFEO);
            }
            var jsonResult = Json(oFEOs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsLot(FabricTransferPackingList oFTPL)
        {
            List<Lot> oLots = new List<Lot>();
            try
            {
                double nQty = 0;
                double.TryParse(oFTPL.Params, out nQty);

                string sSQL = "SELECT * FROM View_Lot WHERE Balance > 0 AND ParentType=" + (int)EnumTriggerParentsType.FabricQCDetail + " AND WorkingUnitID=" + oFTPL.StoreID  ;
                if (oFTPL.FEOID > 0)
                {
                    sSQL += " AND ParentID IN (SELECT FBQCDetailID FROM FabricBatchQCDetail WHERE FBQCID IN (SELECT FBQCID FROM FabricBatchQC WHERE FBID IN (SELECT FBID FROM FabricBatch WHERE FEOID = " + oFTPL.FEOID + ")))";
                }
                if(!string.IsNullOrEmpty(oFTPL.FEONo))
                    sSQL += " AND ParentID IN (SELECT FBQCDetailID FROM FabricBatchQCDetail WHERE FBQCID IN (SELECT FBQCID FROM FabricBatchQC WHERE FBID IN (SELECT FBID FROM View_FabricBatch WHERE FEONo = '" + oFTPL.FEONo + "')))";

                //if (!string.IsNullOrEmpty(oFTPL.LotIDs))
                //{
                //    sSQL = sSQL + " AND LotID NOT IN (" + oFTPL.LotIDs + ")";
                //}
                sSQL = sSQL + " AND LotID NOT IN (SELECT LotID FROM FabricTransferPackingListDetail)";
                sSQL = sSQL + " ORDER BY LotNo";
                oLots = Lot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oLots.Count > 0)
                {
                    List<FabricBatchQCDetail> oFBQCDetails = new List<FabricBatchQCDetail>();
                    sSQL="Select * from View_FabricBatchQCDetail Where FBQCDetailID In ("+string.Join(",", oLots.Select(x=>x.ParentID).Distinct().ToList())+")";
                    oFBQCDetails = FabricBatchQCDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFBQCDetails.Any())
                    {
                        oLots.ForEach(x => {
                            x.LotGrade = (oFBQCDetails.Where(p => p.FBQCDetailID == x.ParentID).Any()) ? oFBQCDetails.Where(p => p.FBQCDetailID == x.ParentID).FirstOrDefault().GradeInString : "";
                            x.WUWrapLot = oFBQCDetails.Where(p => p.FBQCDetailID == x.ParentID).FirstOrDefault().WarpLot ;
                            x.WUWeftLot = oFBQCDetails.Where(p => p.FBQCDetailID == x.ParentID).FirstOrDefault().WeftLot;
                        
                        });
                    }

                    List<FabricTransferPackingListDetail> oFTPDs = new List<FabricTransferPackingListDetail>();
                    sSQL = "SELECT * FROM View_FabricTransferPackingListDetail FTPLD WHERE FTPLD.FTPListID IN (SELECT FTPL.FTPListID FROM FabricTransferPackingList FTPL WHERE FTPL.FEOID=" + oFTPL.FEOID + ")";
                    oFTPDs = FabricTransferPackingListDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFTPDs.Count > 0)
                    {
                        List<FabricTransferPackingListDetail> oTempFTPDs = new List<FabricTransferPackingListDetail>();
                        List<Lot> oTempLots = new List<Lot>();
                        string[] sLotIDs = string.Join(",", oFTPDs.Select(o=>o.LotID).Distinct()).Split(',');
                        foreach(string sLotID in sLotIDs)
                        {
                            int nLotID=Convert.ToInt32(sLotID);
                            oLots.RemoveAll(o => o.LotID == nLotID);
                        }
                    }
                }
                if (nQty > 0 && oLots.Any())
                {
                    oLots = oLots.Where(x => (x.Balance == nQty || x.BalanceInMtr == nQty) ).ToList();
                }

            }
            catch (Exception ex)
            {
                Lot oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                oLots.Add(oLot);
            }
            var jsonResult = Json(oLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsAnotherLot(FabricTransferPackingList oFTPL)
        {
            List<Lot> oLots = new List<Lot>();
            try
            {
                double nQty = 0;
                double.TryParse(oFTPL.Params, out nQty);
                string sSQL = "SELECT * FROM View_Lot WHERE Balance > 0 AND  WorkingUnitID=" + oFTPL.StoreID + "AND LotNo ='" + oFTPL.LotNo + "' AND LotNo  NOT IN ( SELECT LotNo FROM  FabricBatchQCDetail WHERE LotNo IN('" + oFTPL.LotNo + "') )";    
                sSQL = sSQL + " ORDER BY LotNo";
                oLots = Lot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                Lot oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                oLots.Add(oLot);
            }
            var jsonResult = Json(oLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult Print(int nFTPListID, bool bIsInYard, double nts)
        {
            _oFTPL = new FabricTransferPackingList();
            if (nFTPListID > 0)
            {
                _oFTPL = _oFTPL.Get(nFTPListID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "SELECT * FROM View_FabricTransferPackingListDetail WHERE FTPListID = " + nFTPListID;
                _oFTPL.FTPLDetails = FabricTransferPackingListDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else {
                _oFTPL.ErrorMessage = "Invalid Fabric Transfer Packing List";
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            string sMessage = "Packing List";
            rptFabricTransferPackingList oReport = new rptFabricTransferPackingList();
            byte[] abytes = oReport.PrepareReportTwo(_oFTPL, bIsInYard, oCompany, sMessage);
            return File(abytes, "application/pdf");
        }


        [HttpPost]
        public JsonResult GetsUntagPackingList(FabricTransferPackingList oFTPL)
        {
            _oFTPLs = new List<FabricTransferPackingList>();
            try
            {
                string sSQL = "SELECT * FROM View_FabricTransferPackingList WHERE FTNID=0 ";
                if (!string.IsNullOrEmpty(oFTPL.FEONo)) //oFTPL.FEONo used for carrying FEOIDs
                {
                    sSQL = sSQL + " AND FEOID NOT IN ("+oFTPL.FEONo+")";
                }
                sSQL = sSQL + " ORDER BY FEONo";
                _oFTPLs = FabricTransferPackingList.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFTPL = new FabricTransferPackingList();
                _oFTPL.ErrorMessage = ex.Message;
                _oFTPLs.Add(_oFTPL);
            }
            var jsonResult = Json(_oFTPLs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult DeleteFTPLFromNote(FabricTransferPackingList oFTPL)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFTPL.Untag(oFTPL.FTPListID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        #region FabricTransferPackingListDetail
        [HttpPost]
        public JsonResult SaveFTPLDetail(FabricTransferPackingListDetail oFTPLD)
        {
            try
            {
                if (oFTPLD.FTPLDetails.Count > 0)
                {
                    List<FabricTransferPackingListDetail> oFabricTransferPackingListDetails = new List<FabricTransferPackingListDetail>();
                    List<Lot> oLots = new List<Lot>();
                    if (oFTPLD.FTPLDetails[0].IsSaveSingleLot)
                    {
                        string sSQL = "SELECT * FROM View_Lot WHERE LotNo = '" + oFTPLD.FTPLDetails[0].LotNo + "' AND ParentType=" + (int)EnumTriggerParentsType.FabricQCDetail + " AND WorkingUnitID=" + oFTPLD.FTPLDetails[0].StoreID + " AND ParentID IN (SELECT FBQCDetailID FROM FabricBatchQCDetail WHERE FBQCID IN (SELECT FBQCID FROM FabricBatchQC WHERE FBID IN (SELECT FBID FROM FabricBatch WHERE FEOID = " + oFTPLD.FTPLDetails[0].FEOID + ")))";
                        if (!string.IsNullOrEmpty(oFTPLD.LotIDs))
                        {
                            sSQL = sSQL + " AND LotID NOT IN (" + oFTPLD.LotIDs + ") ";
                        }
                        sSQL = sSQL + " ORDER BY LotNo ";
                        oLots = Lot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        if (oLots.Count > 0)
                        {
                            FabricTransferPackingListDetail oTemp = new FabricTransferPackingListDetail();
                            oTemp = oFTPLD.FTPLDetails[0];
                            oTemp.LotID = oLots[0].LotID;
                            oTemp.Qty = oLots[0].Balance;
                            oFTPLD.FTPLDetails = new List<FabricTransferPackingListDetail>();
                            oFTPLD.FTPLDetails.Add(oTemp);
                        }
                        else
                        {
                            oFTPLD = new FabricTransferPackingListDetail();
                            oFTPLD.ErrorMessage = "No lot found or already in list.";
                        }
                    }
                }

                if (string.IsNullOrEmpty(oFTPLD.ErrorMessage))
                {
                    oFTPLD = oFTPLD.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFTPLD = new FabricTransferPackingListDetail();
                oFTPLD.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFTPLD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateFTPLDetail(FabricTransferPackingListDetail oFTPLD)
        {
            try
            {
                    
                oFTPLD = oFTPLD.Update(((User)Session[SessionInfo.CurrentUser]).UserID);
              
            }
            catch (Exception ex)
            {
                oFTPLD = new FabricTransferPackingListDetail();
                oFTPLD.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFTPLD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFTPLDetail(FabricTransferPackingListDetail oFTPLD)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFTPLD.Delete(oFTPLD.FTPLDetailID, oFTPLD.FTPListID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetsFabricTransferPackingListDetail(FabricTransferPackingList oFTPL)
        {
            _oFTPLDs = new List<FabricTransferPackingListDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_FabricTransferPackingListDetail WHERE FTPListID = " + oFTPL.FTPListID;
                _oFTPLDs = FabricTransferPackingListDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFTPLD = new FabricTransferPackingListDetail();
                _oFTPLD.ErrorMessage = ex.Message;
                _oFTPLDs.Add(_oFTPLD);
            }
            var jsonResult = Json(_oFTPLDs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Searching
        [HttpPost]
        public JsonResult GetsAdvSearch(FabricTransferPackingList oFTPL)
        {
            List<FabricTransferPackingList> oFTPLs = new List<FabricTransferPackingList>();
            try
            {
                string sSQL = this.MakeSQLAdvSearch(oFTPL);
                if (!string.IsNullOrEmpty(sSQL))
                {
                    oFTPLs = FabricTransferPackingList.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFTPL = new FabricTransferPackingList();
                    oFTPL.ErrorMessage = "Give a Searching Criteria.";
                    oFTPLs.Add(oFTPL);
                }
            }
            catch (Exception ex)
            {
                oFTPL = new FabricTransferPackingList();
                oFTPL.ErrorMessage = ex.Message;
                oFTPLs.Add(oFTPL);
            }
            var jsonResult = Json(oFTPLs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string MakeSQLAdvSearch(FabricTransferPackingList oFTPL)
        {
            string sSQL = "";

            if (!string.IsNullOrEmpty(oFTPL.Params))
            {
                string sReturn1 = "SELECT * FROM View_FabricTransferPackingList ";
                string sReturn = "";

                string sFEONo = Convert.ToString(oFTPL.Params.Split('~')[0]);
                string sFTNNo = Convert.ToString(oFTPL.Params.Split('~')[1]);

                bool bIsPackingListDate = Convert.ToBoolean(oFTPL.Params.Split('~')[2]);
                DateTime dFromPackingListDate = Convert.ToDateTime(oFTPL.Params.Split('~')[3]);
                DateTime dToPackingListDate = Convert.ToDateTime(oFTPL.Params.Split('~')[4]);

                #region FEO No
                if (!string.IsNullOrEmpty(sFEONo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEONo LIKE '%" + sFEONo + "%' ";
                }
                #endregion

                #region FTN No
                if (!string.IsNullOrEmpty(sFTNNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FTNNo LIKE '%" + sFTNNo + "%' ";
                }
                #endregion


                #region Packing List Date
                if (bIsPackingListDate)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PackingListDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromPackingListDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToPackingListDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                #endregion

                sSQL = sReturn1 + " " + sReturn + " ORDER BY FEONo";
            }
            return sSQL;
        }
        #endregion

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
    }
}