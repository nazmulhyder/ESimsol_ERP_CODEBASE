using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;


namespace ESimSolFinancial.Controllers
{
    public class DUDeliveryStockController : Controller
    {
        #region Declaration
        DUDeliveryStock _oDUDeliveryStock = new DUDeliveryStock();
        List<DUDeliveryStock> _oDUDeliveryStocks = new List<DUDeliveryStock>();
        List<DUDeliverySummary> _oDUDeliverySummarys = new List<DUDeliverySummary>();
        #endregion

        #region Actions
        #region SubFinishing To Delivery Store
        public ActionResult ViewDUDeliveryStocks(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDUDeliveryStocks = new List<DUDeliveryStock>();
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'InvoicePurchase'", (int)Session[SessionInfo.currentUserID], (Guid)Session[SessionInfo.wcfSessionID]));
          //  _oDUDeliveryStocks = DUDeliveryStock.Gets(1,8,"",(Guid)Session[SessionInfo.wcfSessionID]);
            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            #region Received Stores
            List<WorkingUnit> oWorkingUnit_Received = new List<WorkingUnit>();
            oWorkingUnit_Received = new List<WorkingUnit>();
            oWorkingUnit_Received = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DUDeliveryChallan, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.WorkingUnit_Received = oWorkingUnit_Received;
            #endregion
            return View(_oDUDeliveryStocks);
        }

        [HttpPost]
        public JsonResult SearchByOrderType(DUDeliveryStock oDUDeliveryStock)
        {
            _oDUDeliveryStocks = new List<DUDeliveryStock>();
            try
            {
                _oDUDeliveryStocks = DUDeliveryStock.Gets(Convert.ToInt32(oDUDeliveryStock.OrderType), oDUDeliveryStock.WorkingUnitID, "", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryStock = new DUDeliveryStock();
                //oDUDeliverySummary.ErrorMessage = ex.Message;
                _oDUDeliveryStocks = new List<DUDeliveryStock>();
            }
            var jsonResult = Json(_oDUDeliveryStocks, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult SendToRequsitionToDelivery(DUDeliveryStock oDUDeliveryStock)
        {
            string sReturn = "";
            List<DUDeliveryStock> oDUDeliveryStocks_Ret = new List<DUDeliveryStock>();
            try
            {
                sReturn = oDUDeliveryStock.SendToRequsitionToDelivery(oDUDeliveryStock, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDUDeliveryStocks_Ret = new List<DUDeliveryStock>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sReturn);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Aval To Delivery Store
        public ActionResult ViewDUDeliveryStocksAvalToDelivery(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDUDeliveryStocks = new List<DUDeliveryStock>();
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TransferRequisitionSlipDyed).ToString() + "," + ((int)EnumModuleName.Adjustment).ToString() + "," + ((int)EnumModuleName.CommercialEncashment).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            //  _oDUDeliveryStocks = DUDeliveryStock.Gets(1,8,"",(Guid)Session[SessionInfo.wcfSessionID]);
            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.UserID = ((User)Session[SessionInfo.CurrentUser]).UserID;
            ViewBag.BUID = buid;
            return View(_oDUDeliveryStocks);
        }
        public ActionResult ViewDUDeliveryStocksDeliveryToRecycle(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TransferRequisitionSlipDyed).ToString() + "," + ((int)EnumModuleName.Adjustment).ToString() + "," + ((int)EnumModuleName.CommercialEncashment).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            

            _oDUDeliveryStocks = new List<DUDeliveryStock>();
            List<WorkingUnit> oWorkingUnit_Issue = new List<WorkingUnit>();
            List<WorkingUnit> oWorkingUnit_Received = new List<WorkingUnit>();

            #region Issue Stores
            oWorkingUnit_Issue = new List<WorkingUnit>();
            oWorkingUnit_Issue = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.TransferRequisitionSlipDyed, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            #endregion
            #region Received Stores
            oWorkingUnit_Received = new List<WorkingUnit>();
            oWorkingUnit_Received = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.TransferRequisitionSlipDyed, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.WorkingUnit_Issue = oWorkingUnit_Issue;
            ViewBag.WorkingUnit_Received = oWorkingUnit_Received;
            return View(_oDUDeliveryStocks);
        }
        private string MakeSQL(DUDeliveryStock oDUDeliveryStock)
        {
            string sParams = oDUDeliveryStock.ErrorMessage;
            
            int nWorkingUnitID =0;
            int nOrderType = 0;
            string sPINo = "";
            string sOrderNo = "";
            string sLotNo = "";
            string sProductIDs = "";
            int nBUID = 0;
            //List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            //string sIssueStoreIDs = "";
            //oWorkingUnits = new List<WorkingUnit>();
            //oWorkingUnits = WorkingUnit.GetsPermittedStore(nBUID, EnumModuleName.DUDeliveryChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            //sIssueStoreIDs = string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID).ToList());
            //if (string.IsNullOrEmpty(sIssueStoreIDs))
            //    sIssueStoreIDs = "0";
            if (!string.IsNullOrEmpty(sParams))
            {
                string sTemp = "";
                nWorkingUnitID = Convert.ToInt32(sParams.Split('~')[0]);
                nOrderType = Convert.ToInt32(sParams.Split('~')[1]);
                sLotNo = Convert.ToString(sParams.Split('~')[2]);
                sOrderNo = Convert.ToString(sParams.Split('~')[3]);
                //sProductIDs = Convert.ToString(sParams.Split('~')[5]);
                //sPINo = Convert.ToString(sParams.Split('~')[3]);
                nBUID = Convert.ToInt32(sParams.Split('~')[4]);
            }


            string sReturn1 = " ";
            string sReturn = " ";
            #region BU
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "BUID=" + nBUID;
            }
            #endregion
            #region sIssueStoreIDs
            if (nWorkingUnitID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "WorkingUnitID ="+nWorkingUnitID;
            }
            #endregion
            #region nOrderType
            if (nOrderType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DODID in (Select DOD.DyeingOrderDetailID  from DyeingOrderDetail as DOD where DOD.DyeingOrderID in (Select DyeingOrderID from DyeingOrder where DyeingOrderType=" + nOrderType + "))";
            }
            #endregion
            #region sOrderNo
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DODID in (Select DOD.DyeingOrderDetailID  from DyeingOrderDetail as DOD where DOD.DyeingOrderID in (Select DyeingOrderID from DyeingOrder where OrderNo like '%" + sOrderNo + "%' ))";
            }
            #endregion
            #region Lot No
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "LotNo Like '%" + sLotNo + "%'";
            }
            #endregion

            //#region Contractor
            //if (!String.IsNullOrEmpty(_oDUDeliveryChallan.ContractorName))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " DC.ContractorID in(" + _oDUDeliveryChallan.ContractorName + ")";
            //}
            //#endregion
       
            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in (" + sProductIDs + "))";
            }
            #endregion

         
          

            //#region P/I  No
            //if (!string.IsNullOrEmpty(sPINo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " OrderType=3 and  DC.DUDeliveryChallanID in ( Select DCD.DUDeliveryChallanID from DUDeliveryChallanDetail as DCD where OrderID in  (Select ExportSC.ExportSCID from ExportSC where ExportPIID in (Select ExportPI.ExportPIID from ExportPI where PINO Like  '%" + sPINo + "%')))";
            //}
            //#endregion
           
            string sSQL = sReturn1 + " " + sReturn + "";
            return sSQL;
        }
        [HttpPost]
        public JsonResult GetsAvalnDelivery(DUDeliveryStock oDUDeliveryStock)
        {
            _oDUDeliveryStocks = new List<DUDeliveryStock>();
            try
            {
               string sSQL = MakeSQL(oDUDeliveryStock);
               _oDUDeliveryStocks = DUDeliveryStock.GetsAvalnDelivery(Convert.ToInt32(oDUDeliveryStock.OrderType), oDUDeliveryStock.WorkingUnitID, sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryStock = new DUDeliveryStock();
                //oDUDeliverySummary.ErrorMessage = ex.Message;
                _oDUDeliveryStocks = new List<DUDeliveryStock>();
            }
            var jsonResult = Json(_oDUDeliveryStocks, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SendToRequsition(List<DUDeliveryStock> oDUDeliveryStocks)
        {
            List<TransferableLot> oTransferableLots_Ret = new List<TransferableLot>();
            List<TransferableLot> oTransferableLots = new List<TransferableLot>();
            TransferableLot oTransferableLot = new TransferableLot();

            foreach (DUDeliveryStock oItem in oDUDeliveryStocks)
            {
                oTransferableLot = new TransferableLot();
                oTransferableLot.TransferableLotID = 0;
                oTransferableLot.WorkingUnitID = oItem.WorkingUnitID;
                oTransferableLot.LotID = oItem.LotID;
                oTransferableLot.Qty = oItem.Qty_Tr;
                oTransferableLots.Add(oTransferableLot);
            }

            try
            {
                oTransferableLots_Ret = TransferableLot.SendToRequsition(oTransferableLots, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oTransferableLots_Ret = new List<TransferableLot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTransferableLots_Ret);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchTransferableLot(TransferableLot oTransferableLot)
        {
            List<TransferableLot> oTransferableLots_Ret = new List<TransferableLot>();
            try
            {
                oTransferableLots_Ret = TransferableLot.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oTransferableLots_Ret = new List<TransferableLot>();

            }
            var jsonResult = Json(oTransferableLots_Ret, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult Delete(TransferableLot oTransferableLot)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oTransferableLot.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult TransferToStore(TransferableLot oTransferableLot)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oTransferableLot.TransferToStore(((User)Session[SessionInfo.CurrentUser]).UserID);
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

        #region
        [HttpPost]
        public JsonResult DelivertLotAdjustment(TransferableLot oTransferableLot)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oTransferableLot.LotAdjustment(((User)Session[SessionInfo.CurrentUser]).UserID);
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

        #region Searching

     

       

       
        #endregion

        #region report Daily Delivery Summary
        public ActionResult ViewDUDeliverySummary(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<DUDeliverySummary> oDUDeliverySummarys = new List<DUDeliverySummary>();
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'InvoicePurchase'", (int)Session[SessionInfo.currentUserID], (Guid)Session[SessionInfo.wcfSessionID]));
            // _oDUDeliverySummarys = DUDeliverySummary.Gets("", (Guid)Session[SessionInfo.wcfSessionID]);
            return View(oDUDeliverySummarys);
        }
        private string TotalRS(DUDeliverySummary oDUDeliverySummary)
        {
            string sTemp = "";
            foreach (DUDeliverySummary oItem in _oDUDeliverySummarys)
            {
                if (oDUDeliverySummary.RSID == oItem.RSID)
                {
                    if (sTemp.Length > 0)
                    {
                        sTemp = sTemp + "," + oItem.ChallanNo;
                    }
                    else
                    {
                        sTemp = oItem.ChallanNo;
                    }
                  
                }

            }

            return sTemp;
        }
        [HttpPost]
        public JsonResult SearchByDate(DUDeliverySummary oDUDeliverySummary)
        {
            int nRSID = 0;
            //DUDeliverySummary oDUDeliverySummary = new DUDeliverySummary();
            _oDUDeliverySummarys = new List<DUDeliverySummary>();
            List<DUDeliverySummary> oDUDeliverySummarys = new List<DUDeliverySummary>();
            try
            {
                _oDUDeliverySummarys = DUDeliverySummary.Gets(oDUDeliverySummary.StartDate, oDUDeliverySummary.EndDate.AddDays(1), oDUDeliverySummary.OrderType, oDUDeliverySummary.ReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oDUDeliverySummary.ReportType == 1)
                {
                    foreach (DUDeliverySummary oItem in _oDUDeliverySummarys)
                    {
                        if (nRSID != oItem.RSID)
                        {
                            oDUDeliverySummary = new DUDeliverySummary();
                            oDUDeliverySummary = oItem;
                            oDUDeliverySummary.ChallanNo = TotalRS(oDUDeliverySummary);
                            oDUDeliverySummarys.Add(oDUDeliverySummary);
                        }
                        nRSID = nRSID = oItem.RSID;
                    }

                }
                else
                {
                    oDUDeliverySummarys = _oDUDeliverySummarys;
                }
            }
            catch (Exception ex)
            {
                oDUDeliverySummary = new DUDeliverySummary();
                //oDUDeliverySummary.ErrorMessage = ex.Message;
                oDUDeliverySummarys = new List<DUDeliverySummary>();
            }
            var jsonResult = Json(oDUDeliverySummarys, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        #endregion 

    }
}
