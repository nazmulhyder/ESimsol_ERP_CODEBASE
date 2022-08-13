using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class KnittingYarnReturnController : Controller
    {
        #region Declaration 

        KnittingYarnReturn _oKnittingYarnReturn = new KnittingYarnReturn();
        List<KnittingYarnReturn> _oKnittingYarnReturns = new List<KnittingYarnReturn>();
        KnittingYarnReturnDetail _oKnittingYarnReturnDetail = new KnittingYarnReturnDetail();
        List<KnittingYarnReturnDetail> _oKnittingYarnReturnDetails = new List<KnittingYarnReturnDetail>();
        #endregion

        #region Actions

        public ActionResult ViewKnittingYarnReturnChallans(int nKntOrderID)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.KnittingYarnReturn).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            KnittingOrder oKnittingOrder = new KnittingOrder();
            _oKnittingYarnReturns = new List<KnittingYarnReturn>();
            oKnittingOrder = oKnittingOrder.Get(nKntOrderID, (int)Session[SessionInfo.currentUserID]);
            //oKnittingOrder.KnittingYarns = KnittingYarn.Gets("select * from View_KnittingYarn WHERE KnittingOrderID=" + nKntOrderID, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_KnittingYarnChallanDetail WHERE KnittingYarnChallanID IN (SELECT KnittingYarnChallanID FROM KnittingYarnChallan WHERE KnittingOrderID =" + nKntOrderID + " AND ISNULL(ApprovedBy,0)!=0)";
            List<KnittingYarnChallanDetail>  oKnittingYarnChallanDetails = KnittingYarnChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.KnittingYarnChallanDetails = oKnittingYarnChallanDetails;
            _oKnittingYarnReturns = KnittingYarnReturn.Gets("select * from View_KnittingYarnReturn WHERE KnittingOrderID=" + nKntOrderID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.KnittingOrder = oKnittingOrder;
            ViewBag.Units = MeasurementUnit.Gets("SELECT * FROM MeasurementUnit ORDER BY UnitType ASC", (int)Session[SessionInfo.currentUserID]);
            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            //oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(oKnittingOrder.BUID, EnumModuleName.KnittingYarnChallan, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));
            sSQL = "SELECT * FROM View_WorkingUnit AS WU WHERE WU.WorkingUnitID IN (SELECT IssueStoreID FROM KnittingYarnChallanDetail AS KYCD)";
            oWorkingUnits.AddRange(WorkingUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.Stores = oWorkingUnits;
           // ViewBag.Stores = WorkingUnit.Gets("SELECT * FROM View_WorkingUnit ORDER BY WorkingUnitID", (int)Session[SessionInfo.currentUserID]);
            return View(_oKnittingYarnReturns);
        }
        [HttpPost]
        public JsonResult GetsKnittingYarnChallanDetail(KnittingYarnChallanDetail oKnittingYarnChallanDetail)
        {  
            List<KnittingYarnChallanDetail> oKnittingYarnChallanDetails = new List<KnittingYarnChallanDetail>();
            try
            {
                if (oKnittingYarnChallanDetail.YarnID > 0)
                {
                    string sSQL = "SELECT * FROM KnittingYarnChallanDetail WHERE KnittingYarnChallanID IN (SELECT KnittingYarnChallanID FROM KnittingYarnChallan WHERE KnittingOrderID =" +Convert.ToInt32(oKnittingYarnChallanDetail.Params)+ " AND ISNULL(ApproveBy,0)=0)";
                    oKnittingYarnChallanDetails = KnittingYarnChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 }

            }
            catch (Exception ex)
            {
                oKnittingYarnChallanDetail = new KnittingYarnChallanDetail();
                oKnittingYarnChallanDetail.ErrorMessage = ex.Message;
                oKnittingYarnChallanDetails = new List<KnittingYarnChallanDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingYarnChallanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetKnittingYarnReturn(KnittingYarnReturn oKnittingYarnReturn)
        {
            _oKnittingYarnReturn = new KnittingYarnReturn();
            _oKnittingYarnReturnDetails = new List<KnittingYarnReturnDetail>();

            try
            {
                _oKnittingYarnReturn = _oKnittingYarnReturn.Get(oKnittingYarnReturn.KnittingYarnReturnID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSql = "SELECT * FROM View_KnittingYarnReturnDetail AS HH WHERE HH.KnittingYarnReturnID = " + _oKnittingYarnReturn.KnittingYarnReturnID;
                _oKnittingYarnReturnDetails = KnittingYarnReturnDetail.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oKnittingYarnReturn.KnittingYarnReturnDetails = _oKnittingYarnReturnDetails;
            }
            catch (Exception ex)
            {
                _oKnittingYarnReturn = new KnittingYarnReturn();
                _oKnittingYarnReturn.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnittingYarnReturn);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        //[HttpPost]
        //public JsonResult GetKnittingFabricReceive(KnittingYarnReturn oKnittingFabricReceive)
        //{
        //    _oKnittingYarnReturn = new KnittingYarnReturn();
        //    _oKnittingYarnReturnDetails = new List<KnittingYarnReturnDetail>();

        //    try
        //    {
        //        _oKnittingYarnReturn = _oKnittingYarnReturn.Get(oKnittingFabricReceive.KnittingFabricReceiveID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        string sSql = "SELECT * FROM View_KnittingFabricReceiveDetail AS HH WHERE HH.KnittingFabricReceiveID = " + _oKnittingYarnReturn.KnittingFabricReceiveID;
        //        _oKnittingYarnReturnDetails = KnittingYarnReturnDetail.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        _oKnittingYarnReturn.KnittingFabricReceiveDetails = _oKnittingYarnReturnDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        _oKnittingYarnReturn = new KnittingYarnReturn();
        //        _oKnittingYarnReturn.ErrorMessage = ex.Message;

        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oKnittingYarnReturn);
        //    var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
        //    jSonResult.MaxJsonLength = int.MaxValue;
        //    return jSonResult;
        //}
        //[HttpPost]
        //public JsonResult GetLotByFabric(Lot oLot)
        //{
        //    string sSQL = "", sTempSql = "";
        //    List<Lot> oLots = new List<Lot>();

        //    try
        //    {
        //        sSQL = "SELECT * FROM View_Lot ";
        //        if (oLot.WorkingUnitID != 0)
        //        {
        //            Global.TagSQL(ref sTempSql);
        //            sTempSql += " WorkingUnitID =" + oLot.WorkingUnitID;
        //        }
        //        if (!string.IsNullOrEmpty(oLot.LotNo))
        //        {
        //            Global.TagSQL(ref sTempSql);
        //            sTempSql += "  LotNo LIKE '%" + oLot.LotNo + "%'";
        //        }
        //        if (oLot.ProductID != 0)
        //        {
        //            Global.TagSQL(ref sTempSql);
        //            sTempSql += " ProductID =" + oLot.ProductID;
        //        }
        //        if (oLot.BUID != 0)
        //        {
        //            Global.TagSQL(ref sTempSql);
        //            sTempSql += " BUID =" + oLot.BUID;
        //        }

        //        sTempSql += "  AND Balance >0";
        //        sSQL += sTempSql;
        //        oLots = Lot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    }
        //    catch (Exception ex)
        //    {
        //        oLots = new List<Lot>();
        //        oLot = new Lot();
        //        oLot.ErrorMessage = ex.Message;
        //        oLots.Add(oLot);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oLots);
        //    var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
        //    jSonResult.MaxJsonLength = int.MaxValue;
        //    return jSonResult;
        //}

        [HttpPost]
        public JsonResult Save(KnittingYarnReturn oKnittingYarnReturn)
        {
           
            try
            {
                oKnittingYarnReturn = oKnittingYarnReturn.Save((int)Session[SessionInfo.currentUserID]);
                oKnittingYarnReturn.KnittingYarnReturnDetails = KnittingYarnReturnDetail.Gets("SELECT * FROM View_KnittingYarnReturnDetail WHERE KnittingYarnReturnID= " + oKnittingYarnReturn.KnittingYarnReturnID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oKnittingYarnReturn = new KnittingYarnReturn();
                oKnittingYarnReturn.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingYarnReturn);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ApproveChallan(KnittingYarnReturn oKnittingYarnReturn)
        {
       
            try
            {
                oKnittingYarnReturn = oKnittingYarnReturn.Approve((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oKnittingYarnReturn = new KnittingYarnReturn();
                oKnittingYarnReturn.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingYarnReturn);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(KnittingYarnReturn oKnittingFabricReceive)
        {
            string sFeedBackMessage = "";
            try
            {

                sFeedBackMessage = oKnittingFabricReceive.Delete(oKnittingFabricReceive.KnittingYarnReturnID, (int)Session[SessionInfo.currentUserID]);
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

        #region Functions
        #endregion

        #region Get

      
        #endregion

        #region print

        public ActionResult KnittingFabricReceivePrintPreview(int id)
        {
            BusinessUnit oBU = new BusinessUnit();
            _oKnittingYarnReturn = new KnittingYarnReturn();
            if (id > 0)
            {
                _oKnittingYarnReturn = _oKnittingYarnReturn.Get(id, (int)Session[SessionInfo.currentUserID]);
                string sSql = "SELECT * FROM View_KnittingYarnReturnDetail WHERE KnittingYarnReturnID = " + _oKnittingYarnReturn.KnittingYarnReturnID;
                _oKnittingYarnReturn.KnittingYarnReturnDetails = KnittingYarnReturnDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }

            //List<ApprovalHead> oApprovalHead = new List<ApprovalHead>();
            //string ssql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.KnittingYarnReturn + " AND BUID = " + _oKnittingYarnReturn.BUID + "  Order By Sequence";
            //oApprovalHead = ApprovalHead.Gets(ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //List<ApprovalHistory> oApprovalHistorys = new List<ApprovalHistory>();
            //string Sql = "SELECT * FROM View_ApprovalHistory WHERE ModuleID = " + (int)EnumModuleName.KnittingYarnReturn + " AND BUID = " + _oKnittingYarnReturn.BUID + "  AND  ObjectRefID = " + id + " Order BY ApprovalSequence";
            //oApprovalHistorys = ApprovalHistory.Gets(Sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.KnittingYarnReturn, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oKnittingYarnReturn.BUID > 0)
            {
                oBU = oBU.Get(_oKnittingYarnReturn.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            byte[] abytes;
            rptKnittingYarnReturn oReport = new rptKnittingYarnReturn();
            abytes = oReport.PrepareReport(_oKnittingYarnReturn, oCompany, oBU, oSignatureSetups);//oApprovalHead, oApprovalHistorys
            return File(abytes, "application/pdf");
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

        #endregion
    }
}