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
using System.Dynamic;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class WYRequisitionController : Controller
    {
        #region Declaration
        string _sReportDateRange = "";
        WYRequisition _oWYRequisition = new WYRequisition();
        List<WYRequisition> _oWYRequisitions = new List<WYRequisition>();
        FabricExecutionOrderYarnReceive _oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
        List<FabricExecutionOrderYarnReceive> _FabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
        #endregion

        #region Actions
        public ActionResult ViewWYRequisitions(int buid,  int menuid, int nLayoutType)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.WYRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
           
            _oWYRequisitions = new List<WYRequisition>();
            _oWYRequisitions = WYRequisition.Gets("SELECT * FROM View_WYRequisition WHERE ISNULL(DisburseBy,0)=0 "
                                                    + (nLayoutType>0? " AND WYarnType= " + nLayoutType : "") 
                                                    + " AND BUID = " + buid, 
                                                    (int)Session[SessionInfo.currentUserID]);

            ViewBag.WarpWeftTypes = EnumObject.jGets(typeof(EnumWarpWeft)).Where(x => x.id == (int)EnumWarpWeft.Warp || x.id == (int)EnumWarpWeft.Weft).ToList();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.WYarnTypes = EnumObject.jGets(typeof(EnumWYarnType));
            ViewBag.LayoutType = nLayoutType;
            return View(_oWYRequisitions);
        }

        public ActionResult ViewWYRequisition(int id, int buid, int nLayoutType)
        {
            _oWYRequisition = new WYRequisition();
            BusinessUnit oDyeingBusinessUnit = new BusinessUnit();

            List<FabricExecutionOrderYarnReceive> oFEOYRs = new List<FabricExecutionOrderYarnReceive>();

            if (id > 0)
            {
                _oWYRequisition = _oWYRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oWYRequisition.FEOYSList = FabricExecutionOrderYarnReceive.Gets(id, (int)Session[SessionInfo.currentUserID]);

                if (_oWYRequisition.FEOYSList.Count() > 0)
                {
                    string sSQL = "SELECT FSCDID, MAX(BeamCount)+1 AS BeamCount,MAX(RequiredWarpLength) AS RequiredWarpLength, SUM(TFLength) AS TFLength, SUM(ReceiveQty) AS ReceiveQty "
                                + "FROM [View_FabricExecutionOrderYarnReceive] WHERE FSCDID IN (" + string.Join(",", _oWYRequisition.FEOYSList.Select(x=>x.FSCDID)) + ") AND WYRequisitionID < " + id + " GROUP BY FSCDID";
                    oFEOYRs = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
            }
            else
            {
                _oWYRequisition.WYarnTypeInt = nLayoutType;
                _oWYRequisition.RequisitionTypeInt = (int)EnumInOutType.Disburse;
            }

            oDyeingBusinessUnit = oDyeingBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, (int)Session[SessionInfo.currentUserID]);
            
            ViewBag.LayoutType = nLayoutType;
            //ViewBag.WarpWeftTypes = EnumObject.jGets(typeof(EnumWarpWeft));
            ViewBag.WYarnTypes = EnumObject.jGets(typeof(EnumWYarnType));

            ViewBag.WarpWeftTypes = EnumObject.jGets(typeof(EnumWarpWeft)).Where(x => x.id == (int)EnumWarpWeft.Warp || x.id == (int)EnumWarpWeft.Weft).ToList();
            //UPDATE StorePermission SET ModuleName=180  WHERE ModuleName IN (181, 182)
            //EnumModuleName StoreModule = (nLayoutType == 1) ? EnumModuleName.WYRequisition_Dyed : EnumModuleName.WYRequisition_Yarn;

            //ViewBag.IssueStores = WorkingUnit.GetsPermittedStore(oDyeingBusinessUnit.BusinessUnitID, EnumModuleName.WYRequisition, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            //ViewBag.ReceiveStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.WYRequisition, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);

            //ViewBag.IssueStores   = WorkingUnit.Gets("SELECT * FROM View_WorkingUnit VW WHERE VW.WorkingUnitID IN (SELECT WorkingUnitID FROM WYStoreMapping WHERE WYStoreType=" + (int)EnumStoreType.IssueStore + (nLayoutType > 0 ? " AND WYarnType=" + nLayoutType : "" ) + ") ", (int)Session[SessionInfo.currentUserID]);
            //ViewBag.ReceiveStores = WorkingUnit.Gets("SELECT * FROM View_WorkingUnit VW WHERE VW.WorkingUnitID IN (SELECT WorkingUnitID FROM WYStoreMapping WHERE WYStoreType=" + (int)EnumStoreType.ReceiveStore + (nLayoutType > 0 ? " AND WYarnType=" + nLayoutType : "" ) + ") ", (int)Session[SessionInfo.currentUserID]);

            ViewBag.FEOYRs = oFEOYRs;
            ViewBag.IssueStores   = WYStoreMapping.Gets("SELECT * FROM View_WYStoreMapping WHERE WYStoreType=" + (int)EnumStoreType.IssueStore + (nLayoutType > 0 ? " AND WYarnType=" + nLayoutType : "" ) , (int)Session[SessionInfo.currentUserID]);
            ViewBag.ReceiveStores = WYStoreMapping.Gets("SELECT * FROM View_WYStoreMapping WHERE WYStoreType=" + (int)EnumStoreType.ReceiveStore + (nLayoutType > 0 ? " AND WYarnType=" + nLayoutType : ""), (int)Session[SessionInfo.currentUserID]);

            return View(_oWYRequisition);
        }

        public ActionResult View_WYRequisitions(int buid, int menuid, int nLayoutType)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.WYRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oWYRequisitions = new List<WYRequisition>();
            _oWYRequisitions = WYRequisition.Gets("SELECT * FROM View_WYRequisition WHERE ISNULL(DisburseBy,0)=0 "
                                                    + (nLayoutType > 0 ? " AND WYarnType= " + nLayoutType : "")
                                                    + " AND BUID = " + buid,
                                                    (int)Session[SessionInfo.currentUserID]);

            if (_oWYRequisitions.Count > 0)
            {
                _oWYRequisitions = GetsDispoNos(_oWYRequisitions);
            }

            ViewBag.WarpWeftTypes = EnumObject.jGets(typeof(EnumWarpWeft)).Where(x => x.id == (int)EnumWarpWeft.Warp || x.id == (int)EnumWarpWeft.Weft).ToList();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.WYarnTypes = EnumObject.jGets(typeof(EnumWYarnType));
            ViewBag.LayoutType = nLayoutType;
            ViewBag.BUID = buid;
            ViewBag.IssueStores = WYStoreMapping.Gets("SELECT * FROM View_WYStoreMapping WHERE WYStoreType=" + (int)EnumStoreType.IssueStore + (nLayoutType > 0 ? " AND WYarnType=" + nLayoutType : ""), (int)Session[SessionInfo.currentUserID]);
            ViewBag.ReceiveStores = WYStoreMapping.Gets("SELECT * FROM View_WYStoreMapping WHERE WYStoreType=" + (int)EnumStoreType.ReceiveStore + (nLayoutType > 0 ? " AND WYarnType=" + nLayoutType : ""), (int)Session[SessionInfo.currentUserID]);
            
            return View(_oWYRequisitions);
        }
        public ActionResult View_WYRequisition(int id, int buid, int nLayoutType)
        {
            _oWYRequisition = new WYRequisition();
            BusinessUnit oDyeingBusinessUnit = new BusinessUnit();
            List<FabricExecutionOrderYarnReceive> oFEOYRs = new List<FabricExecutionOrderYarnReceive>();
            if (id > 0)
            {
                _oWYRequisition = _oWYRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oWYRequisition.FEOYSList = FabricExecutionOrderYarnReceive.Gets(id, (int)Session[SessionInfo.currentUserID]);
                if (_oWYRequisition.FEOYSList.Count() > 0)
                {
                    string sSQL = "SELECT FSCDID, MAX(BeamCount)+1 AS BeamCount,MAX(RequiredWarpLength) AS RequiredWarpLength, SUM(TFLength) AS TFLength, SUM(ReceiveQty) AS ReceiveQty "
                                + "FROM [View_FabricExecutionOrderYarnReceive] WHERE FSCDID IN (" + string.Join(",", _oWYRequisition.FEOYSList.Select(x => x.FSCDID)) + ") AND WYRequisitionID < " + id + " GROUP BY FSCDID";
                    oFEOYRs = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
            }
            else
            {
                _oWYRequisition.WYarnTypeInt = nLayoutType;
                _oWYRequisition.RequisitionTypeInt = (int)EnumInOutType.Receive;
            }

            oDyeingBusinessUnit = oDyeingBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, (int)Session[SessionInfo.currentUserID]);

            ViewBag.LayoutType = nLayoutType;
            ViewBag.WYarnTypes = EnumObject.jGets(typeof(EnumWYarnType));

            ViewBag.WarpWeftTypes = EnumObject.jGets(typeof(EnumWarpWeft)).Where(x => x.id == (int)EnumWarpWeft.Warp || x.id == (int)EnumWarpWeft.Weft).ToList();
            

            ViewBag.FEOYRs = oFEOYRs;
            ViewBag.IssueStores = WYStoreMapping.Gets("SELECT * FROM View_WYStoreMapping WHERE WYStoreType=" + (int)EnumStoreType.IssueStore + (nLayoutType > 0 ? " AND WYarnType=" + nLayoutType : ""), (int)Session[SessionInfo.currentUserID]);
            ViewBag.ReceiveStores = WYStoreMapping.Gets("SELECT * FROM View_WYStoreMapping WHERE WYStoreType=" + (int)EnumStoreType.ReceiveStore + (nLayoutType > 0 ? " AND WYarnType=" + nLayoutType : ""), (int)Session[SessionInfo.currentUserID]);
            ViewBag.LayoutType = nLayoutType;
            ViewBag.BUID = buid;
            return View(_oWYRequisition);
        }

        [HttpPost]
        public JsonResult Save(WYRequisition oWYRequisition)
        {
            _oWYRequisition = new WYRequisition();
            try
            {
                _oWYRequisition = oWYRequisition;
                _oWYRequisition.IssueDate = Convert.ToDateTime(_oWYRequisition.IssueDate.ToString("dd MMM yyyy"));
                _oWYRequisition.IssueDate = _oWYRequisition.IssueDate.AddHours(DateTime.Now.Hour);
                _oWYRequisition.IssueDate = _oWYRequisition.IssueDate.AddMinutes(DateTime.Now.Minute);
                _oWYRequisition.IssueDate = _oWYRequisition.IssueDate.AddSeconds(DateTime.Now.Second);
                _oWYRequisition.WarpWeftType = _oWYRequisition.FEOYSList[0].WarpWeftType;   //For Time Consuming
                _oWYRequisition = _oWYRequisition.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWYRequisition = new WYRequisition();
                _oWYRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWYRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approve(WYRequisition oWYRequisition)
        {
            _oWYRequisition = new WYRequisition();
            try
            {
                _oWYRequisition = oWYRequisition;
                _oWYRequisition = _oWYRequisition.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWYRequisition = new WYRequisition();
                _oWYRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWYRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UndoApprove(WYRequisition oWYRequisition)
        {
            _oWYRequisition = new WYRequisition();
            try
            {
                _oWYRequisition = oWYRequisition;
                _oWYRequisition = _oWYRequisition.UndoApprove(EnumDBOperation.UnApproval, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWYRequisition = new WYRequisition();
                _oWYRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWYRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Disburse(WYRequisition oWYRequisition)
        {
            _oWYRequisition = new WYRequisition();
            try
            {
                _oWYRequisition = oWYRequisition;
                _oWYRequisition = _oWYRequisition.Disburse((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWYRequisition = new WYRequisition();
                _oWYRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWYRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoDisburse(WYRequisition oWYRequisition)
        {
            _oWYRequisition = new WYRequisition();
            try
            {
                _oWYRequisition = oWYRequisition;
                _oWYRequisition = _oWYRequisition.UndoApprove(EnumDBOperation.Undo, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWYRequisition = new WYRequisition();
                _oWYRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWYRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Receive(WYRequisition oWYRequisition)
        {
            _oWYRequisition = new WYRequisition();
            try
            {
                _oWYRequisition = oWYRequisition;
                _oWYRequisition = _oWYRequisition.Receive((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWYRequisition = new WYRequisition();
                _oWYRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWYRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReceiveFEOY(FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive)
        {
            FabricExecutionOrderYarnReceive _oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            try
            {
                _oFabricExecutionOrderYarnReceive = oFabricExecutionOrderYarnReceive.ReceiveFEOY((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
                _oFabricExecutionOrderYarnReceive.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricExecutionOrderYarnReceive);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDispoList(DyeingOrderFabricDetail oDOFD)
        {
            List<DyeingOrderFabricDetail> oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
            try
            {
                //string sSQL = "SELECT top 10 * FROM View_FabricSalesContractDetail AS FSCD WHERE FSCD.FabricSalesContractDetailID IN ( SELECT DOF.FSCDetailID FROM DyeingOrderFabric AS DOF WHERE DOF.DyeingOrderID IN (SELECT DOD.DyeingOrderID FROM DyeingOrderDetail AS DOD WHERE DOD.DyeingOrderDetailID IN (SELECT DUL.DODID FROM View_DULotDistribution AS DUL WHERE  DUL.WorkingUnitID = "+oFabricSalesContractDetail.WorkingUnitID+"))) AND FSCD.ExeNo LIKE '%"+oFabricSalesContractDetail.ExeNo+"%' ";
                //string sSQL = "SELECT top 50* FROM View_FabricSalesContractDetail AS FSCD WHERE ISNULL(FSCD.ExeNo,'')!='' and FSCD.ExeNo like '%" + oFabricSalesContractDetail.ExeNo+ "%' and  FabricSalesContractDetailID in (Select FSCDetailID from  DyeingOrderFabric where DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where DyeingOrderDetailID in (Select DODID from DULotDistribution where isnull(Qty,0)>0 ))) Order by ExeNo DESc";
                string sSQL = "SELECT TOP 50* FROM View_DyeingOrderFabricDetail AS DOFD WHERE ISNULL(DOFD.ExeNo,'')!='' AND DOFD.ExeNo LIKE '%" + oDOFD.ExeNo + "%' "
                            + (oDOFD.WarpWeftTypeInt > 0 ? " AND WarpWeftType="+oDOFD.WarpWeftTypeInt : "")
                            + " ORDER BY ExeNo DESC";
                oDyeingOrderFabricDetails = DyeingOrderFabricDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDyeingOrderFabricDetails.Add(new DyeingOrderFabricDetail() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderFabricDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDispoList_Gray(FabricSalesContractDetail oFabricSalesContractDetail)
        {
            List<FabricSalesContractDetail> oFSCDs = new List<FabricSalesContractDetail>();
            List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
            List<FabricExecutionOrderSpecificationDetail> _oFEOSDs = new List<FabricExecutionOrderSpecificationDetail>();
            try
            {
                //string sSQL = "SELECT top 10 * FROM View_FabricSalesContractDetail AS FSCD WHERE FSCD.FabricSalesContractDetailID IN ( SELECT DOF.FSCDetailID FROM DyeingOrderFabric AS DOF WHERE DOF.DyeingOrderID IN (SELECT DOD.DyeingOrderID FROM DyeingOrderDetail AS DOD WHERE DOD.DyeingOrderDetailID IN (SELECT DUL.DODID FROM View_DULotDistribution AS DUL WHERE  DUL.WorkingUnitID = "+oFabricSalesContractDetail.WorkingUnitID+"))) AND FSCD.ExeNo LIKE '%"+oFabricSalesContractDetail.ExeNo+"%' ";
                //string sSQL = "SELECT top 50 * FROM View_FabricExecutionOrderSpecificationDetail WHERE FEOSID IN (SELECT FEO.FEOSID FROM View_FabricExecutionOrderSpecification AS FEO WHERE ISNULL(FEO.ExeNo,'')!='' AND FEO.ExeNo LIKE '%" + oFabricSalesContractDetail.ExeNo + "%' )"; // and  FabricSalesContractDetailID in (Select FSCDetailID from  DyeingOrderFabric where DyeingOrderID in (Select DyeingOrderID from DyeingOrderDetail where DyeingOrderDetailID in (Select DODID from DULotDistribution where isnull(Qty,0)>0 ))) Order by ExeNo DESc";

                string sSQL = "SELECT top 100 * FROM View_FabricSalesContractDetail FSCD WHERE ISNULL(FSCD.ExeNoFull,'')!='' AND FSCD.ExeNoFull LIKE '%" + oFabricSalesContractDetail.ExeNo + "%' and FSCD.FabricSalesContractDetailID in (select FSCDID from FabricExecutionOrderSpecification where isnull(ApproveBy,0)<>0) order by FabricSalesContractDetailID DESC";
                oFabricSalesContractDetails = FabricSalesContractDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                //string sSQL = "SELECT top 50 * FROM View_FabricExecutionOrderSpecificationDetail WHERE FEOSID IN (SELECT FEO.FEOSID FROM View_FabricExecutionOrderSpecification AS FEO WHERE ISNULL(FEO.ExeNo,'')!='' AND FEO.ExeNo LIKE '%" + oFabricSalesContractDetail.ExeNo + "%' )";
                //_oFEOSDs = FabricExecutionOrderSpecificationDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                
                //_oFEOSDs.ForEach(oFEOD => oFabricSalesContractDetails.Add( new FabricSalesContractDetail{
                //    ExeNo = oFEOD.ExeNo,
                //    SCNoFull = oFEOD.SCNoFull,
                //    ColorInfo=oFEOD.ColorName,
                //    ProductName = oFEOD.ProductName,
                //    Qty = oFEOD.Qty,
                //    Amount = oFEOD.Value,
                //    FabricSalesContractDetailID = oFEOD.FSCDID,
                //    FabricWeaveName=(oFEOD.IsWarp)?"Warp":"Weft"
                //}));
            }
            catch (Exception ex)
            {
                _oWYRequisition = new WYRequisition();
                _oWYRequisition.ErrorMessage = ex.Message;
                _oWYRequisitions.Add(_oWYRequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSalesContractDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Gets Lot
        [HttpPost]
        public JsonResult GetsLot_Yarn(Lot oLot)
        {
            List<Lot> oLots = new List<Lot>();
            string sLotID = "";
            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            try
            {

                oFabricLotAssigns = FabricLotAssign.Gets("Select * from View_FabricLotAssign where FEOSDID in (" + oLot .ModelReferenceID+ ")", (int)Session[SessionInfo.currentUserID]);

                oLot.LotNo = (!string.IsNullOrEmpty(oLot.LotNo)) ? oLot.LotNo.Trim() : "";

                string sSQL = "Select  TOP 100 * from View_Lot Where LotID<>0 and isnull(Balance,0)>0 ";

                //if (!string.IsNullOrEmpty(oLot.LotNo))
                //    sSQL = sSQL + " And LotNo Like '%" + oLot.LotNo + "%'";
                //if (oLot.ProductID > 0)
                //    sSQL = sSQL + " And ProductID=" + oLot.ProductID;
                if (oLot.WorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + oLot.WorkingUnitID;

                sLotID = string.Join(",", oFabricLotAssigns.Select(x => x.LotID).Distinct().ToList());
                if (!string.IsNullOrEmpty(sLotID))
                    sSQL = sSQL + " And  LotID in (" + sLotID + ") ";
                if (string.IsNullOrEmpty(sLotID))
                {
                    throw new Exception("Lot Not Found!!");
                }

                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oLots.ForEach(x =>
                {
                    if (oFabricLotAssigns.FirstOrDefault() != null && oFabricLotAssigns.FirstOrDefault().ProductID > 0 && oFabricLotAssigns.Where(b => (b.LotID == x.LotID)).Count() > 0)
                    {
                        x.OrderRecapNo = oFabricLotAssigns.Where(p => (p.LotID == x.LotID) && p.ProductID > 0).FirstOrDefault().ExeNo;
                        x.StockValue = oFabricLotAssigns.Where(p => (p.LotID == x.LotID) && p.ProductID > 0).FirstOrDefault().Balance;
                        // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                    }
                });
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message; oLots.Add(oLot);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsLot_Dyes(Lot oLot)
        {
            List<Lot> oLots = new List<Lot>();
            List<DUHardWinding> oDUHardWindings = new List<DUHardWinding>();
            string sLotID = "";
            try
            {
                oDUHardWindings = DUHardWinding.Gets("Select * from View_DUHardWinding Where DyeingOrderDetailID=" + oLot.ParentID, (int)Session[SessionInfo.currentUserID]);

                oLot.LotNo = (!string.IsNullOrEmpty(oLot.LotNo)) ? oLot.LotNo.Trim() : "";

                string sSQL = "Select  TOP 100 * from View_Lot Where LotID<>0 and isnull(Balance,0)>0 ";

                //if (!string.IsNullOrEmpty(oLot.LotNo))
                //    sSQL = sSQL + " And LotNo Like '%" + oLot.LotNo + "%'";
                if (oLot.ProductID > 0)
                    sSQL = sSQL + " And ProductID=" + oLot.ProductID;
                if (oLot.WorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + oLot.WorkingUnitID;
                //if (oLot.BUID > 0)
                //    sSQL = sSQL + " And BUID=" + oLot.BUID;
                sLotID = string.Join(",", oDUHardWindings.Select(x => x.LotID).Distinct().ToList());
                if (!string.IsNullOrEmpty(sLotID))
                    sSQL = sSQL + " And  LotID in (" + sLotID + ") ";
                if (string.IsNullOrEmpty(sLotID))
                {
                    throw new Exception("Lot Not Found!!");
                }
                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oLots.ForEach(x =>
                {
                    if (oDUHardWindings.FirstOrDefault() != null && oDUHardWindings.FirstOrDefault().ProductID > 0 && oDUHardWindings.Where(b => (b.LotID == x.LotID)).Count() > 0)
                    {
                        x.OrderRecapNo = oDUHardWindings.Where(p => (p.LotID == x.LotID ) && p.ProductID > 0).FirstOrDefault().DyeingOrderNo;
                        x.StockValue = oDUHardWindings.Where(p => (p.LotID == x.LotID ) && p.ProductID > 0).FirstOrDefault().Balance;
                        // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                    }
                });

               
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message; oLots.Add(oLot);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets Details Info
        [HttpPost]
        public JsonResult GetFEYRs(WYRequisition oWYRequisition) // GRAY YARN 
        {
            List<FabricExecutionOrderYarnReceive> oFEOYRs = new List<FabricExecutionOrderYarnReceive>();
            try
            {
                string sSQL = "SELECT FSCDID, MAX(BeamCount)+1 AS BeamCount,MAX(RequiredWarpLength) AS RequiredWarpLength, SUM(TFLength) AS TFLength, SUM(ReceiveQty) AS ReceiveQty "
                            + "FROM [View_FabricExecutionOrderYarnReceive] WHERE FSCDID IN (" + oWYRequisition.Params + ") "
                            + (oWYRequisition.WYRequisitionID > 0 ? ("AND WYRequisitionID < " + oWYRequisition.WYRequisitionID) : "" ) + " GROUP BY FSCDID";
                oFEOYRs = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFEOYRs.Add(new FabricExecutionOrderYarnReceive() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOYRs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetYarnlList(DyeingOrderFabricDetail oDOFD) // GRAY YARN 
        {
            List<FabricExecutionOrderYarnReceive> oFEOYRs = new List<FabricExecutionOrderYarnReceive>();
             string sSQL="";
            try
            {

                int nWUID_Issue = Convert.ToInt32(oDOFD.Params.Split('~')[0]);
                int nYarnType = Convert.ToInt32(oDOFD.Params.Split('~')[1]);
                int nReqType = Convert.ToInt32(oDOFD.Params.Split('~')[2]);
                if (nYarnType == (int)EnumWYarnType.Gray && nReqType==(int)EnumInOutType.Receive)
                {
                    sSQL =
                               "   SELECT TOP 100"
                               + "	    DOFD.FSCDetailID AS FSCDID, "
                               + "	    DOFD.DispoNo AS DispoNo, "
                               + "	    DOFD.DispoNo AS ExeNo, "
                               + "	    DOFD.DyeingOrderID, "
                               + "	    DOFD.DyeingOrderDetailID, "
                               + "	    DOFD.FEOSDID, "
                               + "	    DOFD.ProductID,"
                               + "	    DOFD.ProductName,"
                               + "	    DOFD.ColorName,"
                               + "	    DOFD.WarpWeftType,"
                               + "	    FEOS.RequiredWarpLength,"
                               + "	    FLA.LotID AS IssueLotID,"
                                + "	    FLA.WorkingunitID as WUID,"
                               + "	    FLA.LotNo,"
                               + "	    FLA.BalanceLot as LotBalance,"
                               + "(Select SUM(ReceiveQty) from FabricExecutionOrderYarnReceive where FEOSDID=DOFD.FEOSDID) as BalanceQty,"
                               + "	    DOFD.Qty AS OrderQty"
                                //+ "	    FLA.Qty AS ReqQty"
                               + "  FROM View_DyeingOrderFabricDetail AS DOFD"
                               + " LEFT JOIN FabricExecutionOrderSpecification AS FEOS  On DOFD.FEOSID=FEOS.FEOSID"
                               + "  LEFT JOIN View_FabricLotAssign AS FLA ON DOFD.FEOSDID = FLA.FEOSDID WHERE isnull(FLA.LotID,0)>0 and ISNULL(DOFD.ExeNo,'')!='' "

                               + (!string.IsNullOrEmpty(oDOFD.ExeNo) ? " AND DOFD.ExeNo LIKE '%" + oDOFD.ExeNo + "%'" : "")
                               + (oDOFD.WarpWeftTypeInt > 0 ? " AND DOFD.WarpWeftType =" + oDOFD.WarpWeftTypeInt : "")
                               + "  ORDER BY DispoNo";
                }
                else if ((nYarnType == (int)EnumWYarnType.LeftOver || nYarnType == (int)EnumWYarnType.Gray) && nReqType == (int)EnumInOutType.Disburse)
                {
                    sSQL = "Select top(100) 0 as FEOYID,DOD.Code+''+RIGHT(FabricSalesContractDetail.ExeNo, LEN(FabricSalesContractDetail.ExeNo)- 1)   AS DispoNo"
	+ " ,FabricSalesContractDetail.ExeNo  AS ExeNo"
	+ " ,FEOYRecv.FSCDID"
	+ " ,FEOYRecv.WarpWeftType"
	+ " ,FEOYRecv.ReqQty"
    + " ,WUReqd.IssueStoreID as WUID"
    //+ " ,FEOYRecv.ReceiveQty"
	+ " ,FEOYRecv.BagQty"
	+ " ,FEOYRecv.DyeingOrderDetailID"
	+ " ,FEOYRecv.FEOSDID"
	+ " ,FEOYRecv.Remarks"
	+ " ,FEOYRecv.Dia"
	+ " ,FEOYRecv.TFLength"
	+ " ,FEOYRecv.BeamNo"
	+ " ,FEOYRecv.NumberOfCone"
	+ " ,FEOSD.ColorName"
	+ " ,FEOSD.FEOSID"
	+ " ,(FEOS.RepeatSection * FEOSD.EndsCount) AS ReqCones"
	+ " ,Product.ProductName"
	+ " ,Product.ProductCode"
	+ " ,FEOSD.ProductID"
	+ " ,FEOYRecv.DestinationLotID as IssueLotID"
	+ " ,DestinationLot.LotNo AS LotNo"
    + " ,DestinationLot.Balance AS LotBalance"
    + " ,FEOYRecv.ReceiveQty AS ReceiveQty"
     //+ " ,FEOYRecv.ReceiveQty AS BalanceQty"
     + " ,FEOYRecv.ReceiveQty AS OrderQty"
	+ " ,FEOS.RequiredWarpLength"
    + " from FabricExecutionOrderYarnReceive FEOYRecv"
    + " Left Join WYRequisition as WUReqd   on FEOYRecv.WYRequisitionID=WUReqd.WYRequisitionID"
    + " Left Join Lot as DestinationLot  on FEOYRecv.DestinationLotID=DestinationLot.LotID"
    + " Left Join DyeingOrderFabricDetail DOD  On DOD.FEOSDID=FEOYRecv.FEOSDID"
    + " Left Join FabricExecutionOrderSpecificationDetail FEOSD  On FEOSD.FEOSDID=FEOYRecv.FEOSDID"
    + " Left Join FabricExecutionOrderSpecification as FEOS  On FEOSD.FEOSID=FEOS.FEOSID"
    + " Left Join FabricSalesContractDetail  On FEOS.FSCDID=FabricSalesContractDetail.FabricSalesContractDetailID"
    + " Left Join Product  On FEOSD.ProductID=Product.ProductID"
    + " where DestinationLot.Balance>0 and DestinationLot.WorkingUnitID=" + nWUID_Issue
    + (!string.IsNullOrEmpty(oDOFD.ExeNo) ? " AND FabricSalesContractDetail.ExeNo LIKE '%" + oDOFD.ExeNo + "%'" : "")
                                             + (oDOFD.WarpWeftTypeInt > 0 ? " AND DOD.WarpWeftType =" + oDOFD.WarpWeftTypeInt : "")
                                             + "  ORDER BY ExeNo";

                }

                oFEOYRs = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                //oFEOYRs=oFEOYRs.ForEach()
                if (nYarnType == (int)EnumWYarnType.Gray)
                {
                    oFEOYRs.ForEach(o => o.ReceiveQty = (o.OrderQty - o.BalanceQty) < 0 ? 0 : o.OrderQty - o.BalanceQty);
                    oFEOYRs.ForEach(o => o.ReqQty = (o.OrderQty - o.BalanceQty) < 0 ? 0 : o.OrderQty - o.BalanceQty);
                }


            }
            catch (Exception ex)
            {
                oFEOYRs.Add(new FabricExecutionOrderYarnReceive(){ErrorMessage = ex.Message});
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOYRs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetDyed_YarnlList(DyeingOrderFabricDetail oDyeingOrderFabricDetail)
        {
            List<FabricExecutionOrderYarnReceive> oFEOYRs = new List<FabricExecutionOrderYarnReceive>();
            FabricExecutionOrderYarnReceive oTempFEOYR = new FabricExecutionOrderYarnReceive();
            try
            {

                string sSQL = "";
                int nWUID_Issue = Convert.ToInt32(oDyeingOrderFabricDetail.Params.Split('~')[0]);
                int nYarnType = Convert.ToInt32(oDyeingOrderFabricDetail.Params.Split('~')[1]);
                int nReqType = Convert.ToInt32(oDyeingOrderFabricDetail.Params.Split('~')[2]);
                bool bAllOrder = Convert.ToBoolean(oDyeingOrderFabricDetail.Params.Split('~')[3]);
                

                if (nReqType == (int)EnumInOutType.Disburse)
                {
                    sSQL = "Select top(100) 0 as FEOYID, DOD.Code+''+RIGHT(FabricSalesContractDetail.ExeNo, LEN(FabricSalesContractDetail.ExeNo)- 1)   AS DispoNo"
    + " ,FabricSalesContractDetail.ExeNo  AS ExeNo"
    + " ,FEOYRecv.FSCDID"
    + " ,FEOYRecv.WarpWeftType"
    + " ,max(FEOYRecv.ReqQty) as ReqQty "
    + " ,WUReqd.IssueStoreID as WUID"
                        //+ " ,FEOYRecv.ReceiveQty"
    + " ,max(FEOYRecv.BagQty) as BagQty"
    + " ,FEOYRecv.DyeingOrderDetailID"
    + " ,FEOYRecv.FEOSDID"
    + " ,max(FEOYRecv.TFLength) as TFLength "
    + " ,FEOSD.ColorName"
    + " ,FEOSD.FEOSID"
    //+ " ,(FEOS.RepeatSection * FEOSD.EndsCount) AS ReqCones"
    + " ,Product.ProductName"
    + " ,Product.ProductCode"
    + " ,FEOSD.ProductID"
    + " ,FEOYRecv.DestinationLotID as IssueLotID"
    + " ,DestinationLot.LotNo AS LotNo"
    + " ,DestinationLot.Balance AS LotBalance"
    + " ,max(FEOYRecv.ReceiveQty) as ReceiveQty "
    + " ,max(FEOYRecv.ReceiveQty) as OrderQty "
    + " ,max(FEOS.RequiredWarpLength) as RequiredWarpLength "
    + " from FabricExecutionOrderYarnReceive FEOYRecv"
    + " Left Join WYRequisition as WUReqd   on FEOYRecv.WYRequisitionID=WUReqd.WYRequisitionID"
    + " Left Join Lot as DestinationLot  on FEOYRecv.DestinationLotID=DestinationLot.LotID"
    + " Left Join DyeingOrderFabricDetail DOD  On DOD.FEOSDID=FEOYRecv.FEOSDID"
    + " Left Join FabricExecutionOrderSpecificationDetail FEOSD  On FEOSD.FEOSDID=FEOYRecv.FEOSDID"
    + " Left Join FabricExecutionOrderSpecification as FEOS  On FEOSD.FEOSID=FEOS.FEOSID"
    + " Left Join FabricSalesContractDetail  On FEOS.FSCDID=FabricSalesContractDetail.FabricSalesContractDetailID"
    + " Left Join Product  On FEOSD.ProductID=Product.ProductID"
    + " where DestinationLot.Balance>0 and DestinationLot.WorkingUnitID=" + nWUID_Issue
    + (!string.IsNullOrEmpty(oDyeingOrderFabricDetail.ExeNo) ? " AND FabricSalesContractDetail.ExeNo LIKE '%" + oDyeingOrderFabricDetail.ExeNo + "%'" : "")
                                             + (oDyeingOrderFabricDetail.WarpWeftTypeInt > 0 ? " AND DOD.WarpWeftType =" + oDyeingOrderFabricDetail.WarpWeftTypeInt : "")
                                             + " group by DOD.Code,FabricSalesContractDetail.ExeNo  ,FEOYRecv.FSCDID ,FEOYRecv.WarpWeftType  ,WUReqd.IssueStoreID  ,FEOYRecv.DyeingOrderDetailID ,FEOYRecv.FEOSDID  , FEOSD.ColorName ,FEOSD.FEOSID ,Product.ProductName ,Product.ProductCode ,FEOSD.ProductID ,FEOYRecv.DestinationLotID  ,DestinationLot.LotNo  ,DestinationLot.Balance  ORDER BY ExeNo";
                    oFEOYRs = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                   
                }
                else
                {

                    sSQL =
                               "   SELECT TOP 100 "
                               + "	    DOFD.FSCDetailID AS FSCDID, "
                               + "	    DOFD.ExeNo AS DispoNo, "
                               + "	    DOFD.DyeingOrderID, "
                               + "	    DOFD.DyeingOrderDetailID, "
                               + "	    DOFD.FEOSDID, "
                               + "	    DOFD.ProductID,"
                               + "	    DOFD.ProductName,"
                               + "	    DOFD.ColorName,"
                               + "	    DOFD.WarpWeftType,"
                               + "	    FEOS.RequiredWarpLength,"
                               + "	    (FEOS.RepeatSection * FEOSD.EndsCount) AS ReqCones,"
                               + "     (Select sum(DD.LengthFinish) from FabricBeamFinish  as DD where DD.FEOSID=DOFD.FEOSID  ) as BeamFinish,"
                               + "     (SELECT SUM(TFlength) FROM (SELECT FSCDID, MAX(TFlength) AS TFlength FROM [FabricExecutionOrderYarnReceive] WHERE  WarpWeftType=1 and FSCDID  = DOFD.FSCDetailID GROUP BY FSCDID, WYRequisitionID) A GROUP BY FSCDID) AS IssuedLength, "
                        //+ "     ((SELECT SUM(TFlength) FROM (SELECT FSCDID, MAX(TFlength) AS TFlength FROM [FabricExecutionOrderYarnReceive] WHERE FSCDID  = DOFD.FSCDetailID GROUP BY FSCDID, WYRequisitionID) A GROUP BY FSCDID)) AS TFlength, "

                               + "	    HW.LotID AS IssueLotID,"
                               + "	    HW.LotNo,"
                        //+ "	    HW.Balance as BalanceQty,"
                               + " 	HW.Balance as LotBalance,"
                                  + "(Select SUM(ReceiveQty) from FabricExecutionOrderYarnReceive where  FEOSDID=DOFD.FEOSDID and WYRequisitionID in (select WYRequisitionid from WYRequisition where WYRequisition.RequisitionType =101 and WYarnType in (1,2))) as BalanceQty,"
                                + "	    DOFD.Qty AS OrderQty"
                               + " FROM View_DyeingOrderFabricDetail AS DOFD"
                               + " LEFT JOIN FabricExecutionOrderSpecification AS FEOS  On DOFD.FEOSID=FEOS.FEOSID"
                               + " LEFT JOIN FabricExecutionOrderSpecificationDetail FEOSD  On FEOSD.FEOSDID=DOFD.FEOSDID"
                               + " LEFT JOIN View_DUHardWinding AS HW ON DOFD.DyeingOrderDetailID = HW.DyeingOrderDetailID WHERE   ISNULL(DOFD.ExeNo,'')!='' "
                               + ((!bAllOrder) ? " and isnull(HW.Balance,0)>0 " : "")
                               + (!string.IsNullOrEmpty(oDyeingOrderFabricDetail.ExeNo) ? " AND DOFD.ExeNo LIKE '%" + oDyeingOrderFabricDetail.ExeNo + "%'" : "")
                               + (oDyeingOrderFabricDetail.WarpWeftTypeInt > 0 ? " AND DOFD.WarpWeftType =" + oDyeingOrderFabricDetail.WarpWeftTypeInt : "")
                               + (oDyeingOrderFabricDetail.WarpWeftTypeInt == (int)EnumWarpWeft.Warp && bAllOrder == false ? " AND FEOS.FEOSID in ( Select FEOSID from FabricBeamFinish where FabricBeamFinish.IsFinish in (0,1))" : "")
                               + "  ORDER BY DispoNo";
                    oFEOYRs = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
              
            
                oFEOYRs.ForEach(o => o.TFLength = (o.BeamFinish - o.IssuedLength) < 0 ? 0 : o.BeamFinish - o.IssuedLength);
                oFEOYRs.ForEach(o => o.ReceiveQty = (o.OrderQty - o.BalanceQty) < 0 ? 0 : o.OrderQty - o.BalanceQty);
                oFEOYRs.ForEach(o => o.ReqQty = o.ReceiveQty);
            }
            catch (Exception ex)
            {
                oFEOYRs.Add(new FabricExecutionOrderYarnReceive() { ErrorMessage = ex.Message });
            }
          
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOYRs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsLot(FabricExecutionOrderYarnReceive oFEOYR)
        {
            List<Lot> oLots = new List<Lot>();
            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            List<DULotDistribution> oDULotDistributions = new List<DULotDistribution>();
            string sLotID = "";
            string sParentLotID = "";
            string sSQL = "";
            try
            {

                int nYarnType = Convert.ToInt32(oFEOYR.ErrorMessage.Split('~')[0]);
                int nReqType = Convert.ToInt32(oFEOYR.ErrorMessage.Split('~')[1]);
 
                oFEOYR.LotNo = (!string.IsNullOrEmpty(oFEOYR.LotNo)) ? oFEOYR.LotNo.Trim() : "";
            
                if (nYarnType == (int)EnumWYarnType.Gray)
                {
                    sSQL = "Select * from View_FabricLotAssign where FEOSDID in (Select FEOSDID from DyeingOrderFabricDetail where FEOSDID=" + oFEOYR.FEOSDID + " )";
                    //if (oLotParent.ProductID > 0)
                    //    sSQL = sSQL + " And ProductID=" + oLotParent.ProductID;
                    oFabricLotAssigns = FabricLotAssign.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    sLotID = string.Join(",", oFabricLotAssigns.Select(x => x.LotID).Distinct().ToList());
                    sParentLotID = string.Join(",", oFabricLotAssigns.Where(b => b.ParentLotID > 0).Select(x => x.ParentLotID).Distinct().ToList());

                    if (!string.IsNullOrEmpty(sParentLotID))
                    {
                        sLotID = sLotID + "," + sParentLotID;
                    }

                    if (oFabricLotAssigns.Count <= 0)
                    {
                        throw new Exception("Lot yet not assign with this order!!");
                    }
                }
                if (nYarnType == (int)EnumWYarnType.DyedYarn)
                {
                    sSQL = "select * from DULotDistribution where DODID in (Select DyeingOrderDetailID from DyeingOrderFabricDetail where FEOSDID =" + oFEOYR.FEOSDID + ")";
                    oDULotDistributions = DULotDistribution.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    sLotID = string.Join(",", oDULotDistributions.Select(x => x.LotID).Distinct().ToList());
                }


                sSQL = "Select * from View_Lot Where Balance>0.1 and WorkingUnitID in (SELECT WorkingUnitID FROM View_WYStoreMapping WHERE WYStoreType=" + (int)EnumStoreType.IssueStore + (nYarnType > 0 ? " AND WYarnType=" + nYarnType : "") + ")";

                if (oFEOYR.WUID > 0)
                {
                    sSQL = sSQL + " And WorkingUnitID=" + oFEOYR.WUID;
                }
                //else
                //{
                //    sSQL = sSQL + " And WorkingUnitID in (SELECT WorkingUnitID FROM View_WYStoreMapping WHERE WYStoreType=1 AND WYarnType=" + (int)nYarnType+ ")";
                //}
              
                //if (oDUOrderSetup.IsOpenRawLot == true)
                //{
                //    if (!string.IsNullOrEmpty(oFEOYR.LotNo))
                //        sSQL = sSQL + " And LotNo Like '%" + oFEOYR.LotNo + "%'";

                //    if (oFEOYR.ProductID > 0)
                //        sSQL = sSQL + " And ProductID=" + oFEOYR.ProductID;

                //}
                //else
                //{
                if (!string.IsNullOrEmpty(oFEOYR.LotNo))
                    sSQL = sSQL + " And LotNo Like '%" + oFEOYR.LotNo + "%'";
                     if (oFEOYR.ProductID > 0)
                        sSQL = sSQL + " And ProductID=" + oFEOYR.ProductID;
                    if (!string.IsNullOrEmpty(sLotID))
                        sSQL = sSQL + " And  (LotID in (" + sLotID + ") or ParentLotID in (" + sLotID + "))";

                    //if (string.IsNullOrEmpty(sLotID))
                    //{
                    //    throw new Exception("Lot yet not assign with this order!!");
                    //}
                
                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (nYarnType == (int)EnumWYarnType.Gray)
                {
                    oLots.ForEach(x =>
                    {
                        if (oFabricLotAssigns.FirstOrDefault() != null && oFabricLotAssigns.FirstOrDefault().ProductID > 0 && oFabricLotAssigns.Where(b => (b.LotID == x.LotID || b.ParentLotID == x.ParentLotID || b.LotID == x.ParentLotID)).Count() > 0)
                        {
                            x.OrderRecapNo = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID || p.ParentLotID == x.ParentLotID) && p.ProductID > 0).FirstOrDefault().ExeNo;
                            x.StockValue = oFabricLotAssigns.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.ParentLotID == x.LotID) && p.ProductID > 0).Sum(a => a.Qty);
                            // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                        }
                    });
                }
                else
                {
                    oLots.ForEach(x =>
                    {
                        if (oDULotDistributions.FirstOrDefault() != null && oDULotDistributions.FirstOrDefault().LotID > 0 && oDULotDistributions.Where(b => (b.LotID == x.LotID )).Count() > 0)
                        {
                            x.OrderRecapNo = oDULotDistributions.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.LotID > 0).FirstOrDefault().OrderNo;
                            x.StockValue = oDULotDistributions.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID || p.LotID == x.ParentLotID) && p.LotID > 0).FirstOrDefault().Qty;
                            // x.Qt = oLotParents.Where(p => (p.LotID == x.LotID || p.LotID == x.ParentLotID) && p.DyeingOrderID > 0).FirstOrDefault().DyeingOrderNo;
                        }
                    });
                }
               
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLots.Add(new Lot() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpPost]
        public JsonResult Delete(WYRequisition oWYRequisition)
        {
            try
            {
                if (oWYRequisition.WYRequisitionID <= 0) { throw new Exception("Please select an valid item."); }
                oWYRequisition.ErrorMessage = oWYRequisition.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oWYRequisition = new WYRequisition();
                oWYRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWYRequisition.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        #endregion

        #region ADV SEARCH

        [HttpPost]
        public JsonResult AdvSearch(FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive)
        {
            List<WYRequisition> oWYRequisitions = new List<WYRequisition>();
            WYRequisition _oWYRequisition = new WYRequisition();
            string sSQL = MakeSQL(oFabricExecutionOrderYarnReceive);
            if (sSQL == "Error")
            {
                _oWYRequisition = new WYRequisition();
                _oWYRequisition.ErrorMessage = "Please select a searching critaria.";
                oWYRequisitions = new List<WYRequisition>();
            }
            else
            {
                oWYRequisitions = new List<WYRequisition>();
                oWYRequisitions = WYRequisition.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }


            if (oWYRequisitions.Count > 0)
            {
                oWYRequisitions = GetsDispoNos(oWYRequisitions);
            }

            var jsonResult = Json(oWYRequisitions, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private List<WYRequisition> GetsDispoNos(List<WYRequisition> oWYRequisitions)
        {
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            string sWYRequisitionIDs = string.Join(",", oWYRequisitions.Select(x => x.WYRequisitionID).ToList());
            if (!string.IsNullOrEmpty(sWYRequisitionIDs))
            {
                sWYRequisitionIDs = "Select * from View_FabricExecutionOrderYarnReceive Where WYRequisitionID<>0 and WYRequisitionID in (" + sWYRequisitionIDs + ")";
                oFabricExecutionOrderYarnReceives = ESimSol.BusinessObjects.FabricExecutionOrderYarnReceive.Gets(sWYRequisitionIDs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oWYRequisitions.ForEach(x =>
                {

                    if (oFabricExecutionOrderYarnReceives.FirstOrDefault() != null && oFabricExecutionOrderYarnReceives.FirstOrDefault().WYRequisitionID > 0 && oFabricExecutionOrderYarnReceives.Where(b => (b.WYRequisitionID == x.WYRequisitionID)).Count() > 0)
                    {
                        x.DispoNo = string.Join(",", oFabricExecutionOrderYarnReceives.Where(p => (p.WYRequisitionID == x.WYRequisitionID) && p.WYRequisitionID > 0).Select(m => m.DispoNo).Distinct().ToList());
                    }

                });
            }

            return oWYRequisitions;
        }

        private string MakeSQL(FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive)
        {
            int nDateCriteria_QuotationDate = 0, nRcvDate = 0, nStatus = 0, nDisburseDate = 0, RequsitionType = 0, IssueStore = 0, ReceiveStore = 0;
            string sReqNo = "";
            DateTime dStart_QuotationDate = DateTime.Today, dEnd_QuotationDate = DateTime.Today, dStart_RcvDate= DateTime.Today, dEnd_RcvDate= DateTime.Today,
                dStart_DisburseDate = DateTime.Today, dEnd_DisburseDate = DateTime.Today;

            if ( !String.IsNullOrEmpty(oFabricExecutionOrderYarnReceive.SearchStringDate))
            {
                nDateCriteria_QuotationDate = Convert.ToInt32(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[0]);
                dStart_QuotationDate = Convert.ToDateTime(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[1]);
                dEnd_QuotationDate = Convert.ToDateTime(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[2]);
                nRcvDate = Convert.ToInt32(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[3]);
                dStart_RcvDate = Convert.ToDateTime(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[4]);
                dEnd_RcvDate = Convert.ToDateTime(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[5]);
                nStatus = Convert.ToInt32(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[6]);
                sReqNo = oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[7];

                nDisburseDate = Convert.ToInt32(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[8]);
                dStart_DisburseDate = Convert.ToDateTime(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[9]);
                dEnd_DisburseDate = Convert.ToDateTime(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[10]);

                RequsitionType = Convert.ToInt32(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[11]);
                IssueStore = Convert.ToInt32(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[12]);
                ReceiveStore = Convert.ToInt32(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[13]);
            }

            //SELECT IssueDate FROM View_WYRequisition WHERE WYRequisitionID iN (SELECT WYRequisitionID FROM FabricExecutionOrderYarnReceive)
            string sReturn1 = "SELECT * FROM View_WYRequisition AS EB";
            string sReturn = "";

            #region DispoNo
            
            if (!string.IsNullOrEmpty(oFabricExecutionOrderYarnReceive.DispoNo))
            {
                Global.TagSQL(ref sReturn);
               // sReturn = sReturn + " EB.WYRequisitionID iN (SELECT WYRequisitionID FROM View_FabricExecutionOrderYarnReceive WHERE DispoNo LIKE '%" + oFabricExecutionOrderYarnReceive.DispoNo + "%')";
                sReturn = sReturn + " EB.WYRequisitionID iN (SELECT WYRequisitionID FROM FabricExecutionOrderYarnReceive as FEOY WHERE FEOY.FSCDID in (SELECT HH.FabricSalesContractDetailID FROM FabricSalesContractDetail as HH WHERE ExeNo LIKE '%"+ oFabricExecutionOrderYarnReceive.DispoNo +"%'))";
            }
            #endregion

            #region Req No

            if (!string.IsNullOrEmpty(sReqNo))
            {
                Global.TagSQL(ref sReturn);
                // sReturn = sReturn + " EB.WYRequisitionID iN (SELECT WYRequisitionID FROM View_FabricExecutionOrderYarnReceive WHERE DispoNo LIKE '%" + oFabricExecutionOrderYarnReceive.DispoNo + "%')";
                sReturn = sReturn + " EB.RequisitionNo LIKE '%" + sReqNo + "%'";
            }
            #endregion

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " EB.IssueDate", nDateCriteria_QuotationDate, dStart_QuotationDate, dEnd_QuotationDate);
            #endregion

            #region WYarnType
            if ((int)oFabricExecutionOrderYarnReceive.WYarnType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.WYarnType ="+(int)oFabricExecutionOrderYarnReceive.WYarnType;
            }
            #endregion

            #region WarpWeftType
            if ((int)oFabricExecutionOrderYarnReceive.WarpWeftType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.WYRequisitionID iN (SELECT WYRequisitionID FROM FabricExecutionOrderYarnReceive WHERE WarpWeftType = " + (int)oFabricExecutionOrderYarnReceive.WarpWeftType + ")";
            }
            #endregion

            #region Rcv SEARCH
            //DateObject.CompareDateQuery(ref sReturn, " ReceiveDate", nRcvDate, dStart_RcvDate, dEnd_RcvDate);
            #region Rcv Date
            if (nRcvDate != (int)EnumCompareOperator.None)
            {
                if (nRcvDate == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WYRequisitionID IN (SELECT WYRequisitionID FROM View_WYRequisitionDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_RcvDate.ToString("dd MMM yyyy") + "', 106)) )";
                }
                else if (nRcvDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WYRequisitionID IN (SELECT WYRequisitionID FROM View_WYRequisitionDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_RcvDate.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nRcvDate == (int)EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WYRequisitionID IN (SELECT WYRequisitionID FROM View_WYRequisitionDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_RcvDate.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nRcvDate == (int)EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WYRequisitionID IN (SELECT WYRequisitionID FROM View_WYRequisitionDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_RcvDate.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nRcvDate == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WYRequisitionID IN (SELECT WYRequisitionID FROM View_WYRequisitionDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_RcvDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dEnd_RcvDate.ToString("dd MMM yyyy") + "', 106)))";
                }
                else if (nRcvDate == (int)EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WYRequisitionID IN (SELECT WYRequisitionID FROM View_WYRequisitionDetail WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dStart_RcvDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dEnd_RcvDate.ToString("dd MMM yyyy") + "', 106)))";
                }
            }
            #endregion
            #endregion

            #region Buyer
            if (!string.IsNullOrEmpty(oFabricExecutionOrderYarnReceive.BuyerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " WYRequisitionID IN (SELECT WYRequisitionID FROM View_WYRequisitionDetail WHERE BuyerID IN (" + oFabricExecutionOrderYarnReceive.BuyerName + "))";
            }
            #endregion

            #region Status
            if (nStatus > 0)
            {
                if (nStatus == 1)//yet approve
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ISNULL(EB.ApprovedBy,0)=0";
                }
                else if (nStatus == 2)//Wait For Receive
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ISNULL(EB.DisburseBy,0)!=0 AND ISNULL(EB.ReceivedBy,0)=0 ";
                }
                else if (nStatus == 3)//Wait For Disburse
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ISNULL(EB.ApprovedBy,0)!=0 AND ISNULL(EB.DisburseBy,0)=0 ";
                }
            }
            #endregion

            #region RequsitionType
            if (RequsitionType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.RequisitionType = " + RequsitionType;
            }
            #endregion

            #region IssueStore
            if (IssueStore > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.IssueStoreID = " + IssueStore;
            }
            #endregion

            #region ReceiveStore
            if (ReceiveStore > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ReceiveStoreID = " + ReceiveStore;
            }
            #endregion

            #region Disburse DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " EB.DisburseDate", nDisburseDate, dStart_DisburseDate, dEnd_DisburseDate);
            #endregion

            #region Lot No
            if (!string.IsNullOrEmpty(oFabricExecutionOrderYarnReceive.LotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.WYRequisitionID iN (SELECT WYRequisitionID FROM View_WYRequisitionDetail WHERE LotNo LIKE '%" + oFabricExecutionOrderYarnReceive.LotNo + "%' )";
            }
            #endregion

            #region Product IDs
            if (!string.IsNullOrEmpty(oFabricExecutionOrderYarnReceive.ProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.WYRequisitionID iN (SELECT WYRequisitionID FROM View_WYRequisitionDetail WHERE ProductID IN (" + oFabricExecutionOrderYarnReceive.ProductName + ") )";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region PDF
        private string MakeSQL_Report(FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive)
        {
            int nDateCriteria_QuotationDate = 0,
                nStatus = 0; ;

            DateTime dStart_QuotationDate = DateTime.Today,
                     dEnd_QuotationDate = DateTime.Today;

            if (!String.IsNullOrEmpty(oFabricExecutionOrderYarnReceive.SearchStringDate) && oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~').Count() == 3)
            {
                nDateCriteria_QuotationDate = Convert.ToInt32(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[0]);
                dStart_QuotationDate = Convert.ToDateTime(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[1]);
                dEnd_QuotationDate = Convert.ToDateTime(oFabricExecutionOrderYarnReceive.SearchStringDate.Split('~')[2]);

                if (nDateCriteria_QuotationDate == (int)EnumCompareOperator.EqualTo) _sReportDateRange ="Date : " + dStart_QuotationDate.ToString("dd MMM yyyy");
                if (nDateCriteria_QuotationDate == (int)EnumCompareOperator.Between) _sReportDateRange = "Date (Between) : " + dStart_QuotationDate.ToString("dd MMM yyyy") + " - TO - " + dEnd_QuotationDate.ToString("dd MMM yyyy");
             // if (nDateCriteria_QuotationDate == (int)EnumCompareOperator.NotBetween) _sReportDateRange = "Date (Not Between) : " + dStart_QuotationDate.ToString("dd MMM yyyy") + " To " + dEnd_QuotationDate.ToString("dd MMM yyyy");
            //  if (nDateCriteria_QuotationDate == (int)EnumCompareOperator.GreaterThan) _sReportDateRange = "Date (Greater Than) : " + dStart_QuotationDate.ToString("dd MMM yyyy") + " To " + dEnd_QuotationDate.ToString("dd MMM yyyy");
            }

            //SELECT IssueDate FROM View_WYRequisition WHERE WYRequisitionID iN (SELECT WYRequisitionID FROM FabricExecutionOrderYarnReceive)
            string sReturn1 = "SELECT WYRequisitionID FROM View_WYRequisition AS EB";
            string sReturn = "";

            #region DispoNo

            if (!string.IsNullOrEmpty(oFabricExecutionOrderYarnReceive.DispoNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.WYRequisitionID iN (SELECT WYRequisitionID FROM View_FabricExecutionOrderYarnReceive WHERE DispoNo LIKE '%" + oFabricExecutionOrderYarnReceive.DispoNo + "%')";
            }
            #endregion

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " EB.IssueDate", nDateCriteria_QuotationDate, dStart_QuotationDate, dEnd_QuotationDate);
            #endregion

            #region WYarnType
            if ((int)oFabricExecutionOrderYarnReceive.WYarnType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.WYarnType =" + (int)oFabricExecutionOrderYarnReceive.WYarnType;
            }
            #endregion

            sReturn = sReturn1 + sReturn;

            if (oFabricExecutionOrderYarnReceive.WYRequisitionID > 0)
                sReturn = oFabricExecutionOrderYarnReceive.WYRequisitionID.ToString();

            sReturn = "SELECT * FROM View_FabricExecutionOrderYarnReceive WHERE WYRequisitionID IN (" + sReturn + ")";

            return sReturn;
        }
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricExecutionOrderYarnReceive);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Print
        public ActionResult WYRequisitionPreview(int nWYRequisitionID, int nBUID)
        {
            WYRequisition oWYRequisition = new WYRequisition();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<DyeingOrderFabricDetail> oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            #region Print Setup
            if (nWYRequisitionID > 0)
            {
                _oWYRequisition = oWYRequisition.Get(nWYRequisitionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                _oWYRequisition.FEOYSList = FabricExecutionOrderYarnReceive.Gets(nWYRequisitionID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oDyeingOrderFabricDetails = DyeingOrderFabricDetail.Gets("SELECT * FROM View_DyeingOrderFabricDetail  as DD   where DD.FEOSID IN (" + string.Join(",", _oWYRequisition.FEOYSList.Select(x => x.FEOSID)) + ") ", (int)Session[SessionInfo.currentUserID]);
                if (oDyeingOrderFabricDetails.Count > 0)
                {
                    string sSQL = "Select * from View_FabricExecutionOrderYarnReceive where FSCDID>0 and FSCDID IN (" + string.Join(",", oDyeingOrderFabricDetails.Select(x => x.FSCDetailID)) + ")";
                    oFabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }

            }
            #endregion

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);

            rptWYRequisition oReport = new rptWYRequisition();
            byte[] abytes = oReport.PrepareReport(_oWYRequisition, oCompany, oBusinessUnit, oDyeingOrderFabricDetails, oFabricExecutionOrderYarnReceives);
            return File(abytes, "application/pdf");
        }
        #endregion


        public ActionResult PrintWYRequisitions(int buid)
        {
            _oWYRequisition = new WYRequisition();
            FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            _FabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            rptErrorMessage oReport = new rptErrorMessage();

            oFabricExecutionOrderYarnReceive = (FabricExecutionOrderYarnReceive)Session[SessionInfo.ParamObj];
            string sSQL = MakeSQL_Report(oFabricExecutionOrderYarnReceive);

            if (string.IsNullOrEmpty(sSQL))
            {
                byte[] abytes_error = oReport.PrepareReport("Sorry, No Data Found!!");
                return File(abytes_error, "application/pdf");
            }

            if (sSQL != "Error")
            {
                if (oFabricExecutionOrderYarnReceive.WYRequisitionID > 0)
                    _oWYRequisition = _oWYRequisition.Get(oFabricExecutionOrderYarnReceive.WYRequisitionID, (int)Session[SessionInfo.currentUserID]);

                _FabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }

            string sHeader = "Weaving Requisition Report";

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompanys.First();
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (_FabricExecutionOrderYarnReceives.Count == 0)
            {
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!!");
                return File(abytes, "application/pdf");
            }
            else
            {
                List<SelectedField> oSelectedFields = new List<SelectedField>();
                SelectedField oSelectedField = new SelectedField("", "#SL", 15, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("DispoNo", "Order No", 25, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("BuyerName", "Customer", 40, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ProductName", "Count", 60, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);

                oSelectedField = new SelectedField("ColorName", "Colour", 40, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("LotNo", "Batch", 30, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);

                oSelectedField = new SelectedField("ReqCones", "Need Pcs", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("NumeberOfCone", "T/F Pcs (c)", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);

                oSelectedField = new SelectedField("ReceiveQty", "Issue Kg", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("UnitName", "Kg", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Dia", "Dia", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("BagQty", "Bag", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
               
                oSelectedField = new SelectedField("TFLength", "T/F Mtr", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("TFLengthLB", "T/F LB Mtr", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("BeamNo", "Beam No", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Remarks", "Remarks", 37, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);

                rptWYRequisitionBeam oReport_WY = new rptWYRequisitionBeam();
                //oReport_WY.SLNo = "S/L";
                //oReport_WY.SpanTotal = 6;//ColSpanForTotal
                //oReport_WY.PrintSignatureList = true;
                //oReport_WY.FooterRowHeight = 40;
                //oReport_WY.DateRange = _sReportDateRange;
                
                //oReport_WY.SignatureList = new string[] { "Requisition By", "Received By", "Issue By", "Approved By" };
                //oReport_WY.DataList = new string[] {"","","",""};

                _oWYRequisition.FEOYSList = _FabricExecutionOrderYarnReceives;
                byte[] abytes = oReport_WY.PrepareReport(_oWYRequisition, oSelectedFields, oCompany, oBusinessUnit, _sReportDateRange);
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PrintWYRequisitions_WeftYarn(int buid)
        {
            _oWYRequisition = new WYRequisition();
            FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            _FabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            rptErrorMessage oReport = new rptErrorMessage();

            oFabricExecutionOrderYarnReceive = (FabricExecutionOrderYarnReceive)Session[SessionInfo.ParamObj];
            string sSQL = MakeSQL_Report(oFabricExecutionOrderYarnReceive);

            if (string.IsNullOrEmpty(sSQL))
            {
                byte[] abytes_error = oReport.PrepareReport("Sorry, No Data Found!!");
                return File(abytes_error, "application/pdf");
            }

            if (sSQL != "Error")
            {
                _FabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }

            string sHeader = "Weft Yarn Requisition Report";

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompanys.First();
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (_FabricExecutionOrderYarnReceives.Count == 0)
            {
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!!");
                return File(abytes, "application/pdf");
            }
            else
            {
                List<SelectedField> oSelectedFields = new List<SelectedField>();
                SelectedField oSelectedField = new SelectedField("DispoNo", "Dispo No", 30, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("BuyerName", "Customer", 60, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ProductName", "Count", 90, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("LotNo", "Batch", 30, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ColorName", "Colour", 30, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ReqQty", "Req Kg", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("BagQty", "Bag", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ReceiveQty", "Issue Kg", 30, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Remarks", "Remarks", 37, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);

                rptDynamicReport oReport_WY = new rptDynamicReport(595, 842);
                oReport_WY.SLNo = "S/L";
                oReport_WY.SpanTotal = 5;//ColSpanForTotal
                //oReport_WY.PrintESimSolFooter = true;
                oReport_WY.PrintSignatureList = true;

                oReport_WY.DateRange = _sReportDateRange;
                oReport_WY.SignatureList = new string[] { "Requisition By", "Received By", "Issue By", "Approved By" };
                oReport_WY.DataList = new string[] { "", "", "", "" };
                byte[] abytes = oReport_WY.PrepareReport(_FabricExecutionOrderYarnReceives.Cast<object>().ToList(), oBusinessUnit, oCompany, sHeader, oSelectedFields);
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


        public void WYRequisitionListExcel(bool isDyed)
        {
            List<WYRequisition> oWYRequisitions = new List<WYRequisition>();
            WYRequisition oWYRequisition = new WYRequisition();
            FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            List<FabricExecutionOrderYarnReceive> oWYRequisitionDetails = new List<FabricExecutionOrderYarnReceive>();
            try
            {
                oFabricExecutionOrderYarnReceive = (FabricExecutionOrderYarnReceive)Session[SessionInfo.ParamObj];
                oWYRequisitions = new List<WYRequisition>();
                string sSQL = MakeSQL(oFabricExecutionOrderYarnReceive);
                oWYRequisitions = WYRequisition.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oWYRequisitionDetails = FabricExecutionOrderYarnReceive.Gets("SELECT * FROM View_FabricExecutionOrderYarnReceive WHERE WYRequisitionID IN (" + string.Join(",", oWYRequisitions.Select(x => x.WYRequisitionID)) + ")", (int)Session[SessionInfo.currentUserID]);
            }
            catch
            {
                oWYRequisitions = new List<WYRequisition>();
            }

            if (oWYRequisitions.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oWYRequisition.BUID > 0)
                {
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    oBusinessUnit = oBusinessUnit.Get(oWYRequisition.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                }

                if (!isDyed)
                {
                    #region Header
                    List<TableHeader> table_header = new List<TableHeader>();

                    table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Requisition No", Width = 15f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Issue Date", Width = 28f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Issue store", Width = 35f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Yarn. Type", Width = 15f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Receive Store", Width = 35f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Approved By", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Disburse By", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Receive By", Width = 20f, IsRotate = false });

                    table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Buyer", Width = 28f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Warp/Weft", Width = 12f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Yarn Count", Width = 35f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Color", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Beam No", Width = 10f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Qty", Width = 12f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Remarks", Width = 20f, IsRotate = false });
                    #endregion
                    #region Export Excel
                    int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                    ExcelRange cell; ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Requisition List");

                        foreach (TableHeader listItem in table_header)
                        {
                            sheet.Column(nStartCol++).Width = listItem.Width;
                        }

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        #endregion
                        nRowIndex++;
                        #region Address & Date
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        #endregion
                        nRowIndex++;
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                        cell.Value = "Requisition List"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex += 2;
                        #region Column Header
                        nStartCol = 2;
                        foreach (TableHeader listItem in table_header)
                        {
                            cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        #endregion

                        #region Data

                        nRowIndex++;

                        int nCount = 0;
                        foreach (var oItem in oWYRequisitions)
                        {
                            nStartCol = 2;
                            List<FabricExecutionOrderYarnReceive> oTempDetails = new List<FabricExecutionOrderYarnReceive>();
                            oTempDetails = oWYRequisitionDetails.Where(x => x.WYRequisitionID == oItem.WYRequisitionID).ToList();
                            int rowCount = (oTempDetails.Count() - 1);
                            if (rowCount <= 0) rowCount = 0;
                            ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                            #region DATA
                            ExcelTool.FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.RequisitionNo, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.IssueDateTimeSt, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.IssueStoreName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.WYarnTypeStr, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.ReceiveStoreName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.ApprovedByName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.DisburseByName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.ReceivedByName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);

                            #endregion

                            #region Detail
                            if (oTempDetails.Count > 0)
                            {
                                ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                                foreach (FabricExecutionOrderYarnReceive oItemDetail in oTempDetails)
                                {
                                    nStartCol = 11;
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.DispoNo, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.WarpWeftTypeSt, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.ColorName, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.LotNo, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.BeamNo, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.ReceiveQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.Remarks, false, ExcelHorizontalAlignment.Left, false, false);

                                    nRowIndex++;
                                }
                            }
                            else
                            {
                                nStartCol = 11;
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);

                                nRowIndex++;
                            }
                            #endregion

                        }

                        #region Total
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Total", nRowIndex, nRowIndex, nStartCol, nStartCol += 15, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, oWYRequisitionDetails.Sum(x => x.ReceiveQty).ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, " ", false, ExcelHorizontalAlignment.Left, false, false);
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        #endregion

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Requisition_List.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                else
                {
                    #region Header
                    List<TableHeader> table_header = new List<TableHeader>();

                    table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Requisition No", Width = 15f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Issue Date", Width = 28f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Issue store", Width = 35f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Yarn. Type", Width = 15f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Receive Store", Width = 35f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Approved By", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Disburse By", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Receive By", Width = 20f, IsRotate = false });

                    table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Buyer", Width = 28f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Warp/Weft", Width = 12f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Yarn Count", Width = 35f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Color", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Batch No", Width = 15f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Beam No", Width = 10f, IsRotate = false });

                    table_header.Add(new TableHeader { Header = "Need Pcs", Width = 15f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "T/F Pcs", Width = 10f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Dia", Width = 12f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Bag Qty", Width = 12f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "TF Length", Width = 12f, IsRotate = false });

                    table_header.Add(new TableHeader { Header = "Qty", Width = 12f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Remarks", Width = 20f, IsRotate = false });
                    #endregion
                    #region Export Excel
                    int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                    ExcelRange cell; ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Requisition List");

                        foreach (TableHeader listItem in table_header)
                        {
                            sheet.Column(nStartCol++).Width = listItem.Width;
                        }

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        #endregion
                        nRowIndex++;
                        #region Address & Date
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        #endregion
                        nRowIndex++;
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                        cell.Value = "Requisition List"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex += 2;
                        #region Column Header
                        nStartCol = 2;
                        foreach (TableHeader listItem in table_header)
                        {
                            cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        #endregion

                        #region Data

                        nRowIndex++;

                        int nCount = 0;
                        foreach (var oItem in oWYRequisitions)
                        {
                            nStartCol = 2;
                            List<FabricExecutionOrderYarnReceive> oTempDetails = new List<FabricExecutionOrderYarnReceive>();
                            oTempDetails = oWYRequisitionDetails.Where(x => x.WYRequisitionID == oItem.WYRequisitionID).ToList();
                            int rowCount = (oTempDetails.Count() - 1);
                            if (rowCount <= 0) rowCount = 0;
                            ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                            #region DATA
                            ExcelTool.FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.RequisitionNo, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.IssueDateTimeSt, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.IssueStoreName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.WYarnTypeStr, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.ReceiveStoreName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.ApprovedByName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.DisburseByName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);
                            ExcelTool.FillCellMerge(ref sheet, oItem.ReceivedByName, nRowIndex, nRowIndex + rowCount, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, false);

                            #endregion

                            #region Detail
                            if (oTempDetails.Count > 0)
                            {
                                ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                                foreach (FabricExecutionOrderYarnReceive oItemDetail in oTempDetails)
                                {
                                    nStartCol = 11;
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.DispoNo, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.WarpWeftTypeSt, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.ColorName, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.LotNo, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.BeamNo, false, ExcelHorizontalAlignment.Left, false, false);

                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.ReqCones.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.NumberOfCone.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.Dia, false, ExcelHorizontalAlignment.Left, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.BagQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.TFLength.ToString(), true, ExcelHorizontalAlignment.Right, false, false);

                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.ReceiveQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItemDetail.Remarks, false, ExcelHorizontalAlignment.Left, false, false);

                                    nRowIndex++;
                                }
                            }
                            else
                            {
                                nStartCol = 11;
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);
                                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, " ", false, ExcelHorizontalAlignment.Left, false, false);

                                nRowIndex++;
                            }
                            #endregion

                        }

                        #region Total
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Total", nRowIndex, nRowIndex, nStartCol, nStartCol += 19, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";

                        var oWYRs = oWYRequisitionDetails.GroupBy(x => new { x.WYRequisitionID, x.FEOSID }, (key, grp) =>
                                   new
                                   {
                                       WYRequisitionID = key.WYRequisitionID,
                                       FEOSID = key.FEOSID,
                                       TFLength = grp.Select(x => x.TFLength).FirstOrDefault(),
                                   }).ToList();


                        ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, oWYRs.Sum(x => x.TFLength).ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, oWYRequisitionDetails.Sum(x => x.ReceiveQty).ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, " ", false, ExcelHorizontalAlignment.Left, false, false);
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);
                        #endregion

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Requisition_List.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                
            }

        }

        #endregion

        #region print
        [HttpPost]
        public ActionResult SetFabricExecutionOrderYarnReceiveData(FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricExecutionOrderYarnReceive);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region FabricExecutionOrderYarnReceive
        public ActionResult ViewLeftOverList(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.WYRequisition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            oFabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets("SELECT * FROM [View_WYRequisitionDetail] WHERE ISNULL(ReceiveBy,0) = 0 AND ISNULL(DisburseBy,0) != 0 AND WYarnType = " + (int)EnumWYarnType.LeftOver + " ORDER BY FEOYID", (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.buid = buid;
            return View(oFabricExecutionOrderYarnReceives);
        }

        [HttpPost]
        public JsonResult SaveDetail(List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives)
        {
            _oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            List<FabricExecutionOrderYarnReceive> _oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            try
            {
                _oFabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.SaveDetail(oFabricExecutionOrderYarnReceives, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
                _oFabricExecutionOrderYarnReceive.ErrorMessage = ex.Message;
                _oFabricExecutionOrderYarnReceives.Add(_oFabricExecutionOrderYarnReceive);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricExecutionOrderYarnReceives);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsData(FabricExecutionOrderYarnReceive objFabricExecutionOrderYarnReceive)
        {
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceiveList = new List<FabricExecutionOrderYarnReceive>();
            string sSQL = GetSQL(objFabricExecutionOrderYarnReceive);
            oFabricExecutionOrderYarnReceiveList = FabricExecutionOrderYarnReceive.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            var jSonResult = Json(oFabricExecutionOrderYarnReceiveList, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        private string GetSQL(FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive)
        {
            string sReturn = "", sSQL = "";

            if (!String.IsNullOrEmpty(oFabricExecutionOrderYarnReceive.ErrorMessage))
            {
                int nCount = 0;
                string sDispoNo = oFabricExecutionOrderYarnReceive.ErrorMessage.Split('~')[nCount++];
                int nIssueDate = Convert.ToInt16(oFabricExecutionOrderYarnReceive.ErrorMessage.Split('~')[nCount++]);
                DateTime dIssueDateStart = Convert.ToDateTime(oFabricExecutionOrderYarnReceive.ErrorMessage.Split('~')[nCount++]);
                DateTime dIssueDateEnd = Convert.ToDateTime(oFabricExecutionOrderYarnReceive.ErrorMessage.Split('~')[nCount++]);
                int nReceivedDate = Convert.ToInt16(oFabricExecutionOrderYarnReceive.ErrorMessage.Split('~')[nCount++]);
                DateTime dReceivedDateStart = Convert.ToDateTime(oFabricExecutionOrderYarnReceive.ErrorMessage.Split('~')[nCount++]);
                DateTime dReceivedDateEnd = Convert.ToDateTime(oFabricExecutionOrderYarnReceive.ErrorMessage.Split('~')[nCount++]);
                bool bWaitingForReceive = Convert.ToBoolean(oFabricExecutionOrderYarnReceive.ErrorMessage.Split('~')[nCount++]);
                string sReqNo = oFabricExecutionOrderYarnReceive.ErrorMessage.Split('~')[nCount++];
                string sBatchNo = oFabricExecutionOrderYarnReceive.ErrorMessage.Split('~')[nCount++];

                sReturn = " ";
                sSQL = "SELECT * FROM [View_WYRequisitionDetail] WHERE WYarnType = 4 AND ISNULL(DisburseBy,0) != 0 ";

                #region Dispo No
                if (!string.IsNullOrEmpty(sDispoNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DispoNo LIKE '%" + sDispoNo + "%' ";
                }
                #endregion

                #region Batch No
                if (!string.IsNullOrEmpty(sBatchNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LotNo LIKE '%" + sBatchNo + "%' ";
                }
                #endregion

                #region Issue Date
                DateObject.CompareDateQuery(ref sReturn, "IssueDate", nIssueDate, dIssueDateStart, dIssueDateEnd);
                #endregion

                #region Rcv Date
                DateObject.CompareDateQuery(ref sReturn, "ReceiveDate", nReceivedDate, dReceivedDateStart, dReceivedDateEnd);
                #endregion

                #region Waiting For Receive
                if (bWaitingForReceive)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ISNULL(ReceiveBy,0) = 0 ";
                }
                #endregion

                #region Req No
                if (!string.IsNullOrEmpty(sReqNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " WYRequisitionID IN (SELECT WYRequisitionID FROM WYRequisition WHERE RequisitionNo LIKE '%" + sReqNo + "%') ";
                }
                #endregion

                //sSQL += sWhereCluse;
            }
            return sSQL + sReturn; ;
        }

        [HttpPost]
        public JsonResult UpdateData(FabricExecutionOrderYarnReceive oFabricExecutionOrderYarnReceive)
        {
            _oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            try
            {
                _oFabricExecutionOrderYarnReceive = oFabricExecutionOrderYarnReceive.UpdateObj((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
                _oFabricExecutionOrderYarnReceive.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricExecutionOrderYarnReceive);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintFabricExecutionOrderYarnReceiveList()
        {
            _oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            List<FabricExecutionOrderYarnReceive> _oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            _oWYRequisitions = new List<WYRequisition>();
            try
            {
                _oFabricExecutionOrderYarnReceive = (FabricExecutionOrderYarnReceive)Session[SessionInfo.ParamObj];
                _oFabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets("SELECT * FROM View_WYRequisitionDetail WHERE FEOYID IN (" + _oFabricExecutionOrderYarnReceive.ErrorMessage + ") Order By WYRequisitionID", (int)Session[SessionInfo.currentUserID]);
                _oWYRequisitions = WYRequisition.Gets("SELECT * FROM View_WYRequisition WHERE WYRequisitionID IN (SELECT WYRequisitionID FROM FabricExecutionOrderYarnReceive WHERE FEOYID IN (" + _oFabricExecutionOrderYarnReceive.ErrorMessage + ")) Order By WYRequisitionID", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
                _oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
                _oWYRequisitions = new List<WYRequisition>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oFabricExecutionOrderYarnReceive.BUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oFabricExecutionOrderYarnReceive.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }
            //bool bIsRateView = false;
            //List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            //oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricExecutionOrderYarnReceive).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            //oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            //if (oAuthorizationRoleMapping.Count > 0)
            //{
            //    bIsRateView = true;
            //}

            rptFabricExecutionOrderYarnReceives oReport = new rptFabricExecutionOrderYarnReceives();
            byte[] abytes = oReport.PrepareReport(_oWYRequisitions, _oFabricExecutionOrderYarnReceives, oCompany);
            return File(abytes, "application/pdf");
        }

        public void ExcelFabricExecutionOrderYarnReceiveList()
        {
            _oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
            List<FabricExecutionOrderYarnReceive> _oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            _oWYRequisitions = new List<WYRequisition>();
            try
            {
                _oFabricExecutionOrderYarnReceive = (FabricExecutionOrderYarnReceive)Session[SessionInfo.ParamObj];
                _oFabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets("SELECT * FROM View_WYRequisitionDetail WHERE FEOYID IN (" + _oFabricExecutionOrderYarnReceive.ErrorMessage + ") Order By WYRequisitionID", (int)Session[SessionInfo.currentUserID]);
                _oWYRequisitions = WYRequisition.Gets("SELECT * FROM View_WYRequisition WHERE WYRequisitionID IN (SELECT WYRequisitionID FROM FabricExecutionOrderYarnReceive WHERE FEOYID IN (" + _oFabricExecutionOrderYarnReceive.ErrorMessage + ")) Order By WYRequisitionID", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricExecutionOrderYarnReceive = new FabricExecutionOrderYarnReceive();
                _oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
                _oWYRequisitions = new List<WYRequisition>();
            }

            if (_oWYRequisitions.Count > 0)
            {
                Company _oCompany = new Company();
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFabricExecutionOrderYarnReceive.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(_oFabricExecutionOrderYarnReceive.BUID, (int)Session[SessionInfo.currentUserID]);
                    _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
                }

                //bool bIsRateView = false;
                //List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
                //oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricExecutionOrderYarnReceive).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

                //oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
                //if (oAuthorizationRoleMapping.Count > 0)
                //{
                //    bIsRateView = true;
                //}

                int nStartCol = 2, nTotalCol = 0;

                #region full excel
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Left Over Details");
                    sheet.Name = "Left Over Details";
                    sheet.Column(nStartCol++).Width = 5; //SL
                    sheet.Column(nStartCol++).Width = 15; //Requisition no
                    sheet.Column(nStartCol++).Width = 12; //Date
                    sheet.Column(nStartCol++).Width = 15; //Dipo
                    sheet.Column(nStartCol++).Width = 25; //Buyer
                    sheet.Column(nStartCol++).Width = 35; //Product
                    sheet.Column(nStartCol++).Width = 20; //Lot
                    sheet.Column(nStartCol++).Width = 20; //Color
                    sheet.Column(nStartCol++).Width = 10; //qty
                    //if (bIsRateView)
                    //{
                    //    sheet.Column(nStartCol++).Width = 12; //U. Price
                    //    sheet.Column(nStartCol++).Width = 20; //Note
                    //}

                    nTotalCol = nStartCol;
                    nStartCol = 2;

                    #region Report Header
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = "Left Over Details"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Requisition No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Dispo No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Product"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Lot No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Color"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //if (bIsRateView)
                    //{
                    //    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "U. Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //}

                    rowIndex = rowIndex + 1;
                    #endregion

                    #region data
                    int nCount = 0;
                    foreach (WYRequisition oItem in _oWYRequisitions)
                    {
                        List<FabricExecutionOrderYarnReceive> oTempFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
                        oTempFabricExecutionOrderYarnReceives = _oFabricExecutionOrderYarnReceives.Where(x => x.WYRequisitionID == oItem.WYRequisitionID).ToList();
                        int rowCount = (oTempFabricExecutionOrderYarnReceives.Count() - 1);
                        if (rowCount <= 0) rowCount = 0;
                        nStartCol = 2;

                        #region main object
                        nCount++;
                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = nCount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.RequisitionNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.IssueDateSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        #endregion

                        #region Detail
                        if (oTempFabricExecutionOrderYarnReceives.Count > 0)
                        {
                            foreach (FabricExecutionOrderYarnReceive oItemDetail in oTempFabricExecutionOrderYarnReceives)
                            {
                                nStartCol = 5;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.DispoNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.ProductName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.LotNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.ColorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.ReceiveQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                                rowIndex++;
                            }
                        }
                        else
                        {
                            nStartCol = 5;
                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex++;
                        }
                        #endregion

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, 9]; cell.Merge = true; cell.Value = "Grand Total "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = _oFabricExecutionOrderYarnReceives.Sum(x => x.ReceiveQty); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //if (bIsRateView)
                    //{
                    //    cell = sheet.Cells[rowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //    cell = sheet.Cells[rowIndex, 12]; cell.Value = _oFabricExecutionOrderYarnReceives.Select(x => (x.Qty * x.UnitPrice)).Sum(); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //}
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Left_Over_Details.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }

            }
                #endregion

        }

        #endregion
    }
}