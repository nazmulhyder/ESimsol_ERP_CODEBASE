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
    public class FabricAvailableStockController : Controller
    {
        #region Declaration

        FabricAvailableStock _oFabricAvailableStock = new FabricAvailableStock();
        List<FabricAvailableStock> _oFabricAvailableStocks = new List<FabricAvailableStock>();
        #endregion

        #region Actions
        public ActionResult ViewFabricAvailableStock(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricAvailableStock).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oFabricAvailableStocks = new List<FabricAvailableStock>();
            List<WorkingUnit> oWorkingUnit_Issue = new List<WorkingUnit>();
            List<WorkingUnit> oWorkingUnit_Received = new List<WorkingUnit>();

            #region Issue Stores
            oWorkingUnit_Issue = new List<WorkingUnit>();
            oWorkingUnit_Issue = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricAvailableStock, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            #region Received Stores
            oWorkingUnit_Received = new List<WorkingUnit>();
            oWorkingUnit_Received = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricAvailableStock, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.FabricTransferRequisitionSlips = FabricTransferRequisitionSlip.Gets("SELECT TOP 20 * FROM View_FabricTransferRequisitionSlip ORDER BY DBServerDateTime DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);

            //ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = oFabricOrderSetups.Where(o => o.IsLocal == false).ToList();
            ViewBag.BUID = buid;
            ViewBag.WorkingUnit_Issue = oWorkingUnit_Issue;
            ViewBag.WorkingUnit_Received = oWorkingUnit_Received;
            return View(_oFabricAvailableStocks);
        }
        #endregion

        #region Functions
        [HttpPost]
        public JsonResult GetsAvalnDelivery(FabricAvailableStock oFabricAvailableStock)
        {
            _oFabricAvailableStocks = new List<FabricAvailableStock>();
            try
            {
                string sSQL = MakeSQL(oFabricAvailableStock);
                _oFabricAvailableStocks = FabricAvailableStock.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricAvailableStock = new FabricAvailableStock();
                //oDUDeliverySummary.ErrorMessage = ex.Message;
                _oFabricAvailableStocks = new List<FabricAvailableStock>();
            }
            var jsonResult = Json(_oFabricAvailableStocks, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(FabricAvailableStock oFabricAvailableStock)
        {
            string sParams = oFabricAvailableStock.ErrorMessage;
            int nWorkingUnitID = 0;
            int nOrderType = 0;
            string sOrderNo = "";
            string sDispoNo = "";
            string sProductIDs = "";
            int nBUID = 0;

            if (!string.IsNullOrEmpty(sParams))
            {
                nWorkingUnitID = Convert.ToInt32(sParams.Split('~')[0]);
                nOrderType = Convert.ToInt32(sParams.Split('~')[1]);
                sDispoNo = Convert.ToString(sParams.Split('~')[2]);
                sOrderNo = Convert.ToString(sParams.Split('~')[3]);
                nBUID = Convert.ToInt32(sParams.Split('~')[4]);
            }

            string sReturn1 = " ";
            string sReturn = " ";
            #region BU
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Lot.BUID=" + nBUID;
            }
            #endregion
            #region sIssueStoreIDs
            if (nWorkingUnitID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Lot.WorkingUnitID=" + nWorkingUnitID;
            }
            #endregion
            #region nOrderType
            if (nOrderType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Lot.ParentID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE OrderType = " + nOrderType + "))";
            }
            #endregion
            #region sOrderNo
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                //sReturn = sReturn + " DODID in (Select DOD.DyeingOrderDetailID  from DyeingOrderDetail as DOD where DOD.DyeingOrderID in (Select DyeingOrderID from DyeingOrder where OrderNo like '%" + sOrderNo + "%' ))";
                sReturn = sReturn + " Lot.ParentID IN (SELECT FabricSalesContractDetailID FROM View_FabricSalesContractDetail WHERE SCNoFull LIKE '%" + sOrderNo + "%')";
            }
            #endregion
            #region Dispo No
            if (!string.IsNullOrEmpty(sDispoNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Lot.ParentID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE ExeNo LIKE '%" + sDispoNo + "%')";
            }
            #endregion

            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Lot.ProductID in (" + sProductIDs + "))";
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + "";
            return sSQL;
        }

        [HttpPost]
        public JsonResult SendToRequsition(List<FabricAvailableStock> oFabricAvailableStocks)
        {
            List<FabricTransferableLot> oFabricTransferableLots_Ret = new List<FabricTransferableLot>();
            List<FabricTransferableLot> oFabricTransferableLots = new List<FabricTransferableLot>();
            FabricTransferableLot oFabricTransferableLot = new FabricTransferableLot();

            foreach (FabricAvailableStock oItem in oFabricAvailableStocks)
            {
                oFabricTransferableLot = new FabricTransferableLot();
                oFabricTransferableLot.FabricTransferableLotID = 0;
                oFabricTransferableLot.WorkingUnitID = oItem.WUID;
                oFabricTransferableLot.LotID = oItem.LotID;
                oFabricTransferableLot.Qty = oItem.LotBalance;
                oFabricTransferableLot.RollQty = oItem.RollQty;
                oFabricTransferableLots.Add(oFabricTransferableLot);
            }

            try
            {
                oFabricTransferableLots_Ret = FabricTransferableLot.SendToRequsition(oFabricTransferableLots, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricTransferableLots_Ret = new List<FabricTransferableLot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricTransferableLots_Ret);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FabricTransferableLot oFabricTransferableLot)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricTransferableLot.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SearchTransferableLot(FabricTransferableLot oTransferableLot)
        {
            List<FabricTransferableLot> oTransferableLots_Ret = new List<FabricTransferableLot>();
            try
            {
                oTransferableLots_Ret = FabricTransferableLot.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oTransferableLots_Ret = new List<FabricTransferableLot>();

            }
            var jsonResult = Json(oTransferableLots_Ret, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult TransferToStore(FabricTransferableLot oFabricTransferableLot)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricTransferableLot.TransferToStore(((User)Session[SessionInfo.CurrentUser]).UserID);
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

        #region Search Requisition Slip
        [HttpPost]
        public JsonResult SearchRequisitionSlips(FabricTransferRequisitionSlip oFabricTransferRequisitionSlip)
        {
            List<FabricTransferRequisitionSlip> oFabricTransferRequisitionSlips = new List<FabricTransferRequisitionSlip>();
            try
            {
                if (!string.IsNullOrEmpty(oFabricTransferRequisitionSlip.ErrorMessage))
                {
                    DateTime dStartDate = Convert.ToDateTime(oFabricTransferRequisitionSlip.ErrorMessage.Split('~')[0]);
                    DateTime dEndDate = Convert.ToDateTime(oFabricTransferRequisitionSlip.ErrorMessage.Split('~')[1]);
                    string sSQL = "SELECT * FROM View_FabricTransferRequisitionSlip WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IssueDateTime,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dEndDate.ToString("dd MMM yyyy") + "', 106))";
                    oFabricTransferRequisitionSlips = FabricTransferRequisitionSlip.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFabricTransferRequisitionSlips = new List<FabricTransferRequisitionSlip>();
                oFabricTransferRequisitionSlip = new FabricTransferRequisitionSlip();
                oFabricTransferRequisitionSlip.ErrorMessage = ex.Message;
                oFabricTransferRequisitionSlips.Add(oFabricTransferRequisitionSlip);
            }
            var jsonResult = Json(oFabricTransferRequisitionSlips, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Pdf
        public ActionResult PrintTransferRequisitionSlip(int nFabricTRSID, int nBUID)
        {
            FabricTransferRequisitionSlip oFabricTransferRequisitionSlip = new FabricTransferRequisitionSlip();
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            try
            {
                if (nFabricTRSID > 0)
                {
                    oFabricTransferRequisitionSlip = oFabricTransferRequisitionSlip.Get(nFabricTRSID, (int)Session[SessionInfo.currentUserID]);
                    oFabricTransferRequisitionSlip.FabricTransferRequisitionSlipDetails = FabricTransferRequisitionSlipDetail.Gets("SELECT * FROM View_FabricTransferRequisitionSlipDetail WHERE FabricTRSID=" + oFabricTransferRequisitionSlip.FabricTRSID, (int)Session[SessionInfo.currentUserID]);
                }
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                if (nBUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                byte[] abytes;
                rptFabricTransferRequisitionSlip oReport = new rptFabricTransferRequisitionSlip();
                abytes = oReport.PrepareReport(oFabricTransferRequisitionSlip, oCompany);
                return File(abytes, "application/pdf");
            }
            catch
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
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
        #endregion

    }

}
