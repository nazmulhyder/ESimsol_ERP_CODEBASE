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
    public class DUDashboardProductionController :PdfViewController
    {
        #region Declaration
        string _sDateRange = "";
        List<DUDashboardProduction> _oDUDashboardProductions = new List<DUDashboardProduction>();
        #endregion
        public ActionResult View_DU_Dashboard_Production(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<Location> oLocations = new List<Location>();
            List<DUDashboardProduction> oDUDashboardProductions = new List<DUDashboardProduction>();
            oLocations = Location.Gets("select * from Location where LocationID IN(Select Distinct LocationID from RouteSheet)", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Locations = oLocations;
            ViewBag.BUID = buid;
            return View(oDUDashboardProductions);
        }
        public ActionResult View_DU_Dashboard_Production_DetailView(int buid, int ViewType , int portionID ,string searchStr)
        {
            string sSQL = MakeSQL_View(buid, ViewType, portionID, searchStr);
            _oDUDashboardProductions = DUDashboardProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            DUDashboardProduction oDUDashboardProduction = new DUDashboardProduction();
            oDUDashboardProduction.Count = _oDUDashboardProductions.Sum(x => x.Count);
            oDUDashboardProduction.Qty = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty),2);
            oDUDashboardProduction.Qty_Dryer = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_Dryer),2);
            oDUDashboardProduction.Qty_Fresh = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_Fresh),2);
            oDUDashboardProduction.Qty_Hydro = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_Hydro),2);
            oDUDashboardProduction.Qty_Machine = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_Machine),2);
            oDUDashboardProduction.Qty_Out =Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_Out),2);
            oDUDashboardProduction.Qty_QCD = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_QCD),2);
            oDUDashboardProduction.Qty_UnManage =Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_UnManage),2);
            oDUDashboardProduction.Qty_WForStore =Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_WForStore));
            oDUDashboardProduction.Qty_WQC = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_WQC));
            oDUDashboardProduction.StockInHand =Math.Round(_oDUDashboardProductions.Sum(x => x.StockInHand));
            oDUDashboardProduction.Qty_DC = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_DC),2);
            oDUDashboardProduction.Name = "Total :";
            _oDUDashboardProductions.Add(oDUDashboardProduction);

            ViewBag.BUID = buid;
            ViewBag.ViewType = ViewType;
            ViewBag.SearchStr = searchStr;
            ViewBag.TrackID = 1;
            return View(_oDUDashboardProductions);
        }
        public ActionResult View_DU_Dashboard_Production_DetailViewRS(int buid, int ViewType, int portionID, string searchStr)
        {
            string sSQL = MakeSQL_View_RS(buid, ViewType, portionID, searchStr);
            _oDUDashboardProductions = DUDashboardProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            DUDashboardProduction oDUDashboardProduction = new DUDashboardProduction();
            oDUDashboardProduction.Count = _oDUDashboardProductions.Sum(x => x.Count);
            oDUDashboardProduction.Qty = _oDUDashboardProductions.Sum(x => x.Qty);
            oDUDashboardProduction.Qty_Dryer = _oDUDashboardProductions.Sum(x => x.Qty_Dryer);
            oDUDashboardProduction.Qty_Fresh = _oDUDashboardProductions.Sum(x => x.Qty_Fresh);
            oDUDashboardProduction.Qty_Hydro = _oDUDashboardProductions.Sum(x => x.Qty_Hydro);
            oDUDashboardProduction.Qty_Machine = _oDUDashboardProductions.Sum(x => x.Qty_Machine);
            oDUDashboardProduction.Qty_Out = _oDUDashboardProductions.Sum(x => x.Qty_Out);
            oDUDashboardProduction.Qty_QCD = _oDUDashboardProductions.Sum(x => x.Qty_QCD);
            oDUDashboardProduction.Qty_UnManage = _oDUDashboardProductions.Sum(x => x.Qty_UnManage);
            oDUDashboardProduction.Qty_WForStore = _oDUDashboardProductions.Sum(x => x.Qty_WForStore);
            oDUDashboardProduction.Qty_WQC = _oDUDashboardProductions.Sum(x => x.Qty_WQC);
            oDUDashboardProduction.StockInHand = _oDUDashboardProductions.Sum(x => x.StockInHand);
            oDUDashboardProduction.Name = "Total :";
            oDUDashboardProduction.RouteSheetDate = DateTime.MinValue;
            _oDUDashboardProductions.Add(oDUDashboardProduction);

            ViewBag.BUID = buid;
            ViewBag.ViewType = ViewType;
            ViewBag.SearchStr = searchStr;
            ViewBag.TrackID = 1;
            return View(_oDUDashboardProductions);
        }
        
        [HttpPost]
        public JsonResult DUDashBoardProduction(string SearchStr)
        {
            List<Location> oLocations = new List<Location>();
            DUDashboardProduction oDUDashboardProduction = new DUDashboardProduction()
            {
                IsDate = Convert.ToBoolean(SearchStr.Split('~')[0]),
                StartDate = Convert.ToDateTime(SearchStr.Split('~')[1]),
                EndDate = Convert.ToDateTime(SearchStr.Split('~')[2]),
                LocationID = Convert.ToInt32(SearchStr.Split('~')[3])
            };
            DUDashboardProduction oDUDB_Daily = new DUDashboardProduction()
            {
                IsDate = Convert.ToBoolean(SearchStr.Split('~')[0]),
                StartDate = Convert.ToDateTime(SearchStr.Split('~')[1]),
                EndDate =Convert.ToDateTime(SearchStr.Split('~')[2]),
                LocationID = Convert.ToInt32(SearchStr.Split('~')[3])
            };
            var tuple = new Tuple<DUDashboardProduction, DUDashboardProduction>(new DUDashboardProduction(), new DUDashboardProduction());
          
            try
            {
                _oDUDashboardProductions= DUDashboardProduction.Gets(oDUDashboardProduction, (int)Session[SessionInfo.currentUserID]);
                 oDUDashboardProduction = new DUDashboardProduction();
                oDUDashboardProduction.Count = _oDUDashboardProductions.Sum(x => x.Count);
                oDUDashboardProduction.Qty = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty),2);
                oDUDashboardProduction.Qty_Dryer =Math.Round( _oDUDashboardProductions.Sum(x => x.Qty_Dryer),2);
                oDUDashboardProduction.Qty_Fresh = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_Fresh),2);
                oDUDashboardProduction.Qty_Hydro =Math.Round( _oDUDashboardProductions.Sum(x => x.Qty_Hydro),2);
                oDUDashboardProduction.Qty_Machine =Math.Round( _oDUDashboardProductions.Sum(x => x.Qty_Machine),2);
                oDUDashboardProduction.Qty_Out = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_Out),2);
                oDUDashboardProduction.Qty_QCD = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_QCD),2);
                oDUDashboardProduction.Qty_UnManage = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_UnManage),2);
                oDUDashboardProduction.Qty_WForStore = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_WForStore),2);
                oDUDashboardProduction.Qty_WQC = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_WQC),2);
                oDUDashboardProduction.StockInHand =Math.Round(_oDUDashboardProductions.Sum(x => x.StockInHand),2);
                oDUDashboardProduction.Qty_DC = Math.Round(_oDUDashboardProductions.Sum(x => x.Qty_DC), 2);
                oDUDashboardProduction.Name = "Total :";

                //oDUDashboardProduction = DUDashboardProduction.Get(oDUDashboardProduction, (int)Session[SessionInfo.currentUserID]);

                //if (!oDUDB_Daily.IsDate)
                //{
                //     oDUDB_Daily.EndDate = oDUDB_Daily.StartDate = DateTime.Now;
                   
                //}
                if (oDUDB_Daily.LocationID <= 0)
                {
                    oLocations = Location.Gets("select * from Location where LocationID IN(Select Distinct LocationID from RouteSheet)", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDUDB_Daily.Name = string.Join(",", oLocations.Select(x => x.LocationID).ToList());
                    if(String.IsNullOrEmpty(oDUDB_Daily.Name))
                    {
                        oDUDB_Daily.Name = "0";
                    }
                }
                else { oDUDB_Daily.Name = oDUDB_Daily.LocationID.ToString(); }

                DUDashboardProduction oDUDDP_Data = new DUDashboardProduction();
                //oDUDDP_Data = DUDashboardProduction.Get(oDUDB_Daily, (int)Session[SessionInfo.currentUserID]);
                _oDUDashboardProductions = DUDashboardProduction.Gets_Daily(oDUDB_Daily, (int)Session[SessionInfo.currentUserID]);
                oDUDDP_Data = new DUDashboardProduction();
                oDUDDP_Data.Count = _oDUDashboardProductions.Sum(x => x.Count);
                oDUDDP_Data.Qty = _oDUDashboardProductions.Sum(x => x.Qty);
              
                oDUDDP_Data.Qty_Dryer = _oDUDashboardProductions.Sum(x => x.Qty_Dryer);
                oDUDDP_Data.Qty_Fresh = _oDUDashboardProductions.Sum(x => x.Qty_Fresh);
                oDUDDP_Data.Qty_Hydro = _oDUDashboardProductions.Sum(x => x.Qty_Hydro);
                oDUDDP_Data.Qty_Machine = _oDUDashboardProductions.Sum(x => x.Qty_Machine);
                oDUDDP_Data.Qty_Out = _oDUDashboardProductions.Sum(x => x.Qty_Out);
                oDUDDP_Data.Qty_QCD = _oDUDashboardProductions.Sum(x => x.Qty_QCD);
                oDUDDP_Data.Qty_UnManage = _oDUDashboardProductions.Sum(x => x.Qty_UnManage);
                oDUDDP_Data.Qty_WForStore = _oDUDashboardProductions.Sum(x => x.Qty_WForStore);
                oDUDDP_Data.Qty_WQC = _oDUDashboardProductions.Sum(x => x.Qty_WQC);
                oDUDDP_Data.StockInHand = _oDUDashboardProductions.Sum(x => x.StockInHand);
                oDUDDP_Data.Qty_Cancel = _oDUDashboardProductions.Sum(x => x.Qty_Cancel);
                oDUDDP_Data.Qty_Recycle = _oDUDashboardProductions.Sum(x => x.Qty_Recycle);
                oDUDDP_Data.Qty_Wastage = _oDUDashboardProductions.Sum(x => x.Qty_Wastage);
                oDUDDP_Data.Qty_Loss = _oDUDashboardProductions.Sum(x => x.Qty_Loss);
                oDUDDP_Data.Qty_Gain = _oDUDashboardProductions.Sum(x => x.Qty_Gain);
                oDUDDP_Data.Qty_Manage = _oDUDashboardProductions.Sum(x => x.Qty_Manage);
                oDUDDP_Data.Qty_DC = _oDUDashboardProductions.Sum(x => x.Qty_DC);
                oDUDDP_Data.Qty_Received = _oDUDashboardProductions.Sum(x => x.Qty_Received);
                oDUDDP_Data.Qty_Gain = Math.Abs(oDUDDP_Data.Qty_Gain);
                oDUDDP_Data.Qty_Loss = Math.Abs(oDUDDP_Data.Qty_Loss);
                oDUDDP_Data.Name = "Total :";
                oDUDDP_Data.StartDate = oDUDB_Daily.StartDate; oDUDDP_Data.EndDate = oDUDB_Daily.EndDate;
                tuple = new Tuple<DUDashboardProduction, DUDashboardProduction>(oDUDashboardProduction, oDUDDP_Data);
            }
            catch (Exception ex)
            {
                tuple = new Tuple<DUDashboardProduction, DUDashboardProduction>(new DUDashboardProduction(), new DUDashboardProduction());
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(tuple);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL_View(int buid,int ViewType, int portionID, string SearchStr)
        {
            bool IsDate = false;
            DateTime dStratDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;
            int nLocationID = 0;
            string sRSStatus = "";
            string sYarnType = "";
            sYarnType = ((int)EnumYarnType.Recycle).ToString() + "," + ((int)EnumYarnType.Wastage).ToString();

            if (portionID > 0 && portionID == 13)// Qty_Recycle Current Status
            {
                sYarnType = ((int)EnumYarnType.Recycle).ToString() ;
            }
            if (portionID > 0 && portionID == 13)// Wastage Current Status
            {
                sYarnType = ((int)EnumYarnType.Wastage).ToString();
            }

            if (!string.IsNullOrEmpty(SearchStr))
            {
                IsDate = Convert.ToBoolean(SearchStr.Split('~')[0]);
                dStratDate = Convert.ToDateTime(SearchStr.Split('~')[1]);
                dEndDate = Convert.ToDateTime(SearchStr.Split('~')[2]);
                nLocationID = Convert.ToInt32(SearchStr.Split('~')[3]);
            }
            string sReturn1 = "";
            string sReturn = "";
            if (ViewType == 1) ///ProductName
            {
                sReturn1 = "Select DOD.ProductID as ID,Product.ProductName as Name, count(Distinct(RS.RouteSheetID)) as [Count],  sum(RSDO.Qty_RS) as Qty ,sum(RSDO.Qty_QC) as Qty_Fresh,SUM(RSDO.RecycleQty+RSDO.WastageQty ) as Qty_UnManage ,sum(RSDO.QtyDC-RSDO.QtyRC) as Qty_DC ,0 as StockInHand  from RouteSheetDO as RSDO left join RouteSheet as RS on RSDO.RouteSheetID=RS.RouteSheetID left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=RSDO.DyeingOrderDetailID left join Product as Product on Product.ProductID=DOD.ProductID";
                //sReturn1 = "Select ID,dd.Name,Count(*)as [Count],Sum(dd.Qty) as Qty, SUM(dd.Qty_Fresh) as Qty_Fresh,Sum(Qty_UnManage)as Qty_UnManage ,Sum(StockInHand) as StockInHand,Sum(Qty_DC) as Qty_DC from (Select Qty, ProductID_Raw as ID ,ProductName_Raw as Name,isnull((SELECT SUM (Qty) FROM RSInQcDetail where RSInQCSetupID in (Select RSInQCSetupID from RSInQCSetup where YarnType in (1,4)) and RouteSheetID=TT.RouteSheetID),0) Qty_Fresh,isnull((SELECT SUM (Qty) FROM RSInQcDetail where RSInQCSetupID in (Select RSInQCSetupID from RSInQCSetup where YarnType  in (" + sYarnType + ")) and RouteSheetID=TT.RouteSheetID),0) Qty_UnManage,(Select SUM(Qty) from DULotDistribution where LotID in (Select LotID from Lot where Balance>0.01 and ParentID=TT.RouteSheetID and ParentType=106 )) as StockInHand,(Select SUM(Qty) from DUDeliveryChallanDetail  where DUDeliveryChallanID in (select DUDeliveryChallanID from DUDeliveryChallan where DUDeliveryChallan.IsDelivered=1) and LotID in (Select LotID from Lot where  ParentID=TT.RouteSheetID and ParentType=106 )) as Qty_DC from View_RouteSheet as TT";
            }
            else if (ViewType == 2)///ContractorName
            {
                //sReturn1 = "Select ID,dd.Name,Count(*)as [Count],Sum(dd.Qty) as Qty, SUM(dd.Qty_Fresh) as Qty_Fresh,Sum(Qty_UnManage)as Qty_UnManage ,Sum(StockInHand) as StockInHand                       from (Select Qty, ContractorID as ID ,ContractorName as Name,isnull((SELECT SUM (Qty) FROM RSInQcDetail where RSInQCSetupID in (Select RSInQCSetupID from RSInQCSetup where YarnType in (1,4)) and RouteSheetID=TT.RouteSheetID),0) Qty_Fresh,isnull((SELECT SUM (Qty) FROM RSInQcDetail where isnull(RSInQcDetail.ManagedLotID,0)=0 and RouteSheetID=TT.RouteSheetID),0) Qty_UnManage,isnull((Select SUM(Balance)  from view_Lot where WorkingUnitID in (Select WorkingUnitID from WorkingUnit where BUID=1 and UnitType in (3,4)) and ParentType=106 and ParentID=TT.RouteSheetID),0) as StockInHand from View_RouteSheet as TT";
                sReturn1 = " Select DO.ContractorID as ID,Contractor.Name as Name, count(Distinct(RS.RouteSheetID)) as [Count],  sum(RSDO.Qty_RS) as Qty ,sum(RSDO.Qty_QC) as Qty_Fresh,SUM(RSDO.RecycleQty+RSDO.WastageQty ) as Qty_UnManage ,sum(RSDO.QtyDC-RSDO.QtyRC) as Qty_DC ,0 as StockInHand  from RouteSheetDO as RSDO left join RouteSheet as RS on RSDO.RouteSheetID=RS.RouteSheetID left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=RSDO.DyeingOrderDetailID    left join DyeingOrder as DO on DO.DyeingOrderID=DOD.DyeingOrderID 	 left join Contractor as Contractor on Contractor.ContractorID=DO.ContractorID";
            }
             else if(ViewType == 3)
            {
                sReturn1 = "   Select RS.MachineID as ID,Machine.Code+'('+Machine.Name+')' as Name, count(Distinct(RS.RouteSheetID)) as [Count],  sum(RSDO.Qty_RS) as Qty ,sum(RSDO.Qty_QC) as Qty_Fresh,SUM(RSDO.RecycleQty+RSDO.WastageQty ) as Qty_UnManage ,sum(RSDO.QtyDC-RSDO.QtyRC) as Qty_DC ,0 as StockInHand  from RouteSheetDO as RSDO left join RouteSheet as RS on RSDO.RouteSheetID=RS.RouteSheetID left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=RSDO.DyeingOrderDetailID	left join Machine as Machine on Machine.MachineID=RS.MachineID ";
               // sReturn1 = "Select ID,dd.Name,Count(*)as [Count],Sum(dd.Qty) as Qty, SUM(dd.Qty_Fresh) as Qty_Fresh,Sum(Qty_UnManage)as Qty_UnManage ,Sum(StockInHand) as StockInHand,Sum(Qty_DC) as Qty_DC from (Select Qty, MachineID as ID ,MachineName as Name,isnull((SELECT SUM (Qty) FROM RSInQcDetail where RSInQCSetupID in (Select RSInQCSetupID from RSInQCSetup where YarnType in (1,4)) and RouteSheetID=TT.RouteSheetID),0) Qty_Fresh,isnull((SELECT SUM (Qty) FROM RSInQcDetail where RSInQCSetupID in (Select RSInQCSetupID from RSInQCSetup where YarnType  in (" + sYarnType + ")) and RouteSheetID=TT.RouteSheetID),0) Qty_UnManage,(Select SUM(Qty) from DULotDistribution where LotID in (Select LotID from Lot where Balance>0.01 and ParentID=TT.RouteSheetID and ParentType=106 )) as StockInHand,(Select SUM(Qty) from DUDeliveryChallanDetail  where DUDeliveryChallanID in (select DUDeliveryChallanID from DUDeliveryChallan where DUDeliveryChallan.IsDelivered=1) and LotID in (Select LotID from Lot where  ParentID=TT.RouteSheetID and ParentType=106 )) as Qty_DC from View_RouteSheet as TT";
            }
             else if (ViewType == 4)
            {
                sReturn1 = " Select Lot.WorkingUnitID as ID,WU.OperationUnitName  as Name, count(Distinct(RS.RouteSheetID)) as [Count],  sum(RSDO.Qty_RS) as Qty ,sum(RSDO.Qty_QC) as Qty_Fresh,SUM(RSDO.RecycleQty+RSDO.WastageQty ) as Qty_UnManage ,sum(RSDO.QtyDC-RSDO.QtyRC) as Qty_DC ,0 as StockInHand  from RouteSheetDO as RSDO left join RouteSheet as RS on RSDO.RouteSheetID=RS.RouteSheetID left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=RSDO.DyeingOrderDetailID left join RSRawLot as RSRawLot on RSRawLot.RouteSheetID=RS.RouteSheetID 	left join Lot as Lot on Lot.LotID=RSRawLot.LotID 	left join View_WorkingUnit as WU on WU.WorkingUnitID=Lot.WorkingUnitID ";
                //sReturn1 = "Select ID,dd.Name,Count(*)as [Count],Sum(dd.Qty) as Qty, SUM(dd.Qty_Fresh) as Qty_Fresh,Sum(Qty_UnManage)as Qty_UnManage ,Sum(StockInHand) as StockInHand,Sum(Qty_DC) as Qty_DC from (Select Qty, WorkingUnitID as ID ,OperationUnitName as Name,isnull((SELECT SUM (Qty) FROM RSInQcDetail where RSInQCSetupID in (Select RSInQCSetupID from RSInQCSetup where YarnType in (1,4)) and RouteSheetID=TT.RouteSheetID),0) Qty_Fresh,isnull((SELECT SUM (Qty) FROM RSInQcDetail where RSInQCSetupID in (Select RSInQCSetupID from RSInQCSetup where YarnType  in (" + sYarnType + ")) and RouteSheetID=TT.RouteSheetID),0) Qty_UnManage,(Select SUM(Qty) from DULotDistribution where LotID in (Select LotID from Lot where Balance>0.01 and ParentID=TT.RouteSheetID and ParentType=106 )) as StockInHand,(Select SUM(Qty) from DUDeliveryChallanDetail  where DUDeliveryChallanID in (select DUDeliveryChallanID from DUDeliveryChallan where DUDeliveryChallan.IsDelivered=1) and LotID in (Select LotID from Lot where  ParentID=TT.RouteSheetID and ParentType=106 )) as Qty_DC from View_RouteSheet as TT";
            }
            else
            {
                //sReturn1 = "Select ID,dd.Name,Count(*)as [Count],Sum(dd.Qty) as Qty, SUM(dd.Qty_Fresh) as Qty_Fresh,Sum(Qty_UnManage)as Qty_UnManage ,Sum(StockInHand) as StockInHand                       from (Select Qty, ProductID_Raw as ID ,ProductName_Raw as Name,isnull((SELECT SUM (Qty) FROM RSInQcDetail where RSInQCSetupID in (Select RSInQCSetupID from RSInQCSetup where YarnType in (1,4)) and RouteSheetID=TT.RouteSheetID),0) Qty_Fresh,isnull((SELECT SUM (Qty) FROM RSInQcDetail where isnull(RSInQcDetail.ManagedLotID,0)=0 and RouteSheetID=TT.RouteSheetID),0) Qty_UnManage,isnull((Select SUM(Balance)  from view_Lot where WorkingUnitID in (Select WorkingUnitID from WorkingUnit where BUID=1 and UnitType in (3,4)) and ParentType=106 and ParentID=TT.RouteSheetID),0) as StockInHand from View_RouteSheet as TT";
                sReturn1 = "Select DOD.ProductID as ID,Product.ProductName as Name, count(Distinct(RS.RouteSheetID)) as [Count],  sum(RSDO.Qty_RS) as Qty ,sum(RSDO.Qty_QC) as Qty_Fresh,SUM(RSDO.RecycleQty+RSDO.WastageQty ) as Qty_UnManage ,sum(RSDO.QtyDC-RSDO.QtyRC) as Qty_DC ,0 as StockInHand  from RouteSheetDO as RSDO left join RouteSheet as RS on RSDO.RouteSheetID=RS.RouteSheetID left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=RSDO.DyeingOrderDetailID left join Product as Product on Product.ProductID=DOD.ProductID";
            }

        
            #region nLocationID
            if (nLocationID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RS.LocationID in(" + nLocationID + ")";
            }
            #endregion
            #region portionID Id

            if (portionID == 1 && IsDate == false)// raw Out
            {
                sRSStatus = ((int)EnumRSState.YarnOut).ToString();
            }
            else if (portionID == 2)// Qty_Machine
            {
                sRSStatus = ((int)EnumRSState.LoadedInDyeMachine).ToString() + "," + ((int)EnumRSState.UnloadedFromDyeMachine).ToString();
            }
            else if (portionID == 3)// Qty_Hydro
            {
                sRSStatus = ((int)EnumRSState.LoadedInHydro).ToString() + "," + ((int)EnumRSState.UnloadedFromHydro).ToString();
            }
            else if (portionID == 4)// Qty_Dryer
            {
                sRSStatus = ((int)EnumRSState.LoadedInDrier).ToString() + "," + ((int)EnumRSState.UnLoadedFromDrier).ToString();
            }
            else if (portionID == 5)// Qty_WQC
            {
                sRSStatus = ((int)EnumRSState.InPackageing).ToString() + "," + ((int)EnumRSState.InSubFinishingstore_Partially).ToString() + "," + ((int)EnumRSState.InHW_Sub_Store).ToString() + "," + ((int)EnumRSState.InDelivery).ToString();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RS.RouteSheetID not in (Select RouteSheetID from RouteSheetHistory where CurrentStatus=" + (int)EnumRSState.QC_Done + " )";
   
            }
            else if (portionID == 6)// Qty_QCD
            {
                sRSStatus = ((int)EnumRSState.QC_Done).ToString() + "," + ((int)EnumRSState.InPackageing).ToString() + "," + ((int)EnumRSState.InSubFinishingstore_Partially).ToString() + "," + ((int)EnumRSState.InHW_Sub_Store).ToString() + "," + ((int)EnumRSState.InDelivery).ToString();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RS.RouteSheetID in (Select RouteSheetID from RouteSheetHistory where CurrentStatus=" + (int)EnumRSState.QC_Done + " )";
   
            }
            else if (portionID == 7)// Unmanaged
            {
                sRSStatus = ((int)EnumRSState.InPackageing).ToString() + "," + ((int)EnumRSState.QC_Done).ToString() + "," + ((int)EnumRSState.InSubFinishingstore_Partially).ToString() + "," + ((int)EnumRSState.InHW_Sub_Store).ToString() + "," + ((int)EnumRSState.InDelivery).ToString();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RS.RouteSheetID in (SELECT RouteSheetID FROM RSInQcDetail where isnull(RSInQcDetail.ManagedLotID,0)=0 and  RSInQCSetupID  in (Select RSInQCSetupID from RSInQCSetup where YarnType not in (" + (int)EnumYarnType.FreshDyedYarn + "," + (int)EnumYarnType.DyedYarnOne + "," + (int)EnumYarnType.DyedYarnTwo + "," + (int)EnumYarnType.DyedYarnThree + ")))";
            }
            else if (portionID == 8)// Qty_WForStore
            {
                sRSStatus = ((int)EnumRSState.InSubFinishingstore_Partially).ToString() + "," + ((int)EnumRSState.InHW_Sub_Store).ToString() + "," + ((int)EnumRSState.InDelivery).ToString() + "," + ((int)EnumRSState.QC_Done).ToString() + "," + ((int)EnumRSState.Finished).ToString();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RS.RouteSheetID in (SELECT RSInQcDetail.RouteSheetID FROM RSInQcDetail where ManagedLotID in (Select Lot.LotID from Lot where Balance>0.02 and ParentType=106) and   RSInQCSetupID in (Select RSInQCSetupID from RSInQCSetup where YarnType in (" + (int)EnumYarnType.FreshDyedYarn + "," + (int)EnumYarnType.DyedYarnOne + "," + (int)EnumYarnType.DyedYarnTwo + "," + (int)EnumYarnType.DyedYarnThree + ")))";
            }

            if (portionID > 0 && portionID < 9)// raw Out Current Status
            {
                if (!string.IsNullOrEmpty(sRSStatus))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "RS.RSState in(" + sRSStatus + ")";
                }
            }
            #endregion
            #region Issue Date
            if (portionID >=9 && IsDate == false)// raw Out
            {
                IsDate = true;
                dStratDate = DateTime.Today;
                dEndDate = DateTime.Today;
            }

            if (IsDate)
            {
                Global.TagSQL(ref sReturn);
                if (portionID > 0 && portionID <= 9)// raw Out Current Status
                {
                    sReturn = sReturn + " RS.RouteSheetID in (Select RSH.RouteSheetID from RouteSheetHistory as RSH where CurrentStatus in (" + ((int)EnumRSState.YarnOut).ToString() + ") and  CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStratDate.ToString("dd MMM yyyy 08:00:00") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy 08:00:00") + "',106))) ";
                }
                if (portionID > 0 && portionID == 11)// raw Out Current Status
                {
                    sReturn = sReturn + " RS.RouteSheetID in (Select RSH.RouteSheetID from RouteSheetHistory as RSH where CurrentStatus in (" + ((int)EnumRSState.LotReturn).ToString() + ") and  CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStratDate.ToString("dd MMM yyyy 08:00:00") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy 08:00:00") + "',106))) ";
                }
                if (portionID > 0 && portionID ==12)// raw Out Current Status
                {
                    sReturn = sReturn + " RS.RouteSheetID in (Select RSH.RouteSheetID from RouteSheetHistory as RSH where CurrentStatus in (" + ((int)EnumRSState.QC_Done).ToString() + ") and  CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStratDate.ToString("dd MMM yyyy 08:00:00") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy 08:00:00") + "',106))) ";
                }
                if (portionID > 0 && portionID == 13)// Qty_Recycle Current Status
                {
                    sReturn = sReturn + " RS.RouteSheetID in (Select RSH.RouteSheetID from RouteSheetHistory as RSH where CurrentStatus in (" + ((int)EnumRSState.QC_Done).ToString() + ") and  CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStratDate.ToString("dd MMM yyyy 08:00:00") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy 08:00:00") + "',106))) and RouteSheetID in ( select RouteSheetID from RSInQcDetail where RSInQcDetail.RSInQCSetupID in (Select RSInQCSetup.RSInQCSetupID from RSInQCSetup where RSInQCSetup.YarnType=2 )) ";
                }
                if (portionID > 0 && portionID == 14)// Qty_Wastage
                {
                    sReturn = sReturn + " RS.RouteSheetID in (Select RSH.RouteSheetID from RouteSheetHistory as RSH where CurrentStatus in (" + ((int)EnumRSState.QC_Done).ToString() + ") and  CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStratDate.ToString("dd MMM yyyy 08:00:00") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy 08:00:00") + "',106))) and RouteSheetID in ( select RouteSheetID from RSInQcDetail where RSInQcDetail.RSInQCSetupID in (Select RSInQCSetup.RSInQCSetupID from RSInQCSetup where RSInQCSetup.YarnType=3 )) ";
                }
            }
            
            #endregion
            string sSQL = "";
            if (ViewType == 1)
            {
                sSQL = sReturn1 + " " + sReturn + " Group by DOD.ProductID ,Product.ProductName";
            }
            else if (ViewType == 2)
            {
                sSQL = sReturn1 + " " + sReturn + " Group by DO.ContractorID ,Contractor.Name";
            }
            else if (ViewType == 3)
            {
                sSQL = sReturn1 + " " + sReturn + " Group by RS.MachineID ,Machine.Code,Machine.Name";
            }
            else
            {
                sSQL = sReturn1 + " " + sReturn + ") as dd Group by ID,dd.Name";
            }
            return sSQL;
        }
        private string MakeSQL_View_RS(int id, int ViewType, int portionID, string SearchStr)
        {
            bool IsDate = false;
            DateTime dStratDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;
            int nLocationID = 0;
            int nID = 0;
            string sRSStatus = "";
            if (!string.IsNullOrEmpty(SearchStr))
            {
                IsDate = Convert.ToBoolean(SearchStr.Split('~')[0]);
                dStratDate = Convert.ToDateTime(SearchStr.Split('~')[1]);
                dEndDate = Convert.ToDateTime(SearchStr.Split('~')[2]);
                nLocationID = Convert.ToInt32(SearchStr.Split('~')[3]);
                nID = Convert.ToInt32(SearchStr.Split('~')[4]);
            }
            _sDateRange = "Date: " + dStratDate.ToString("dd MMM yyyy") + (IsDate ? " To " + dEndDate.ToString("dd MMM yyyy") : "");

            string sReturn1 = "";
            string sReturn = "";

            //sReturn1 = "Select RouteSheetNo, (NoCode+''+OrderNo) as OrderNo,RouteSheetDate , Qty,isnull((SELECT SUM (Qty) FROM RSInQcDetail where RSInQCSetupID in (Select RSInQCSetupID from RSInQCSetup where YarnType in (1,4)) and RouteSheetID=TT.RouteSheetID),0) Qty_Fresh,isnull((SELECT SUM (Qty) FROM RSInQcDetail where isnull(RSInQcDetail.ManagedLotID,0)=0 and RouteSheetID=TT.RouteSheetID),0) Qty_UnManage,isnull((Select SUM(Balance)  from view_Lot where WorkingUnitID in (Select WorkingUnitID from WorkingUnit where BUID=1 and UnitType in (3,4)) and ParentType=106 and ParentID=TT.RouteSheetID),0) as StockInHand from View_RouteSheet as TT";
            sReturn1 = " Select RouteSheetNo, (OrderNo) as OrderNo,RouteSheetDate ,RSDO.Qty_RS as Qty ,RSDO.Qty_QC as Qty_Fresh,RSDO.RecycleQty+RSDO.WastageQty  as Qty_UnManage ,(RSDO.QtyDC-RSDO.QtyRC) as Qty_DC ,0 as StockInHand  from RouteSheetDO as RSDO left join RouteSheet as RS on RSDO.RouteSheetID=RS.RouteSheetID left join DyeingOrderDetail as DOD on DOD.DyeingOrderDetailID=RSDO.DyeingOrderDetailID  left join DyeingOrder as DO on DO.DyeingOrderID=DOD.DyeingOrderID  ";
           
            #region View Type
            if (ViewType == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DOD.ProductID in(" + nID + ")";
            }
            else if (ViewType == 2)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DO.ContractorID in(" + nID + ")";
            }
            else if (ViewType == 3)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RS.MachineID in(" + nID + ")";
            }
            else if (ViewType == 4)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "WorkingUnitID in(" + nID + ")";
            }
            else
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DOD.ProductID in(" + nID + ")";
            }
            #endregion

            #region nLocationID
            if (nLocationID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "LocationID in(" + nLocationID + ")";
            }
            #endregion
            #region portionID Id

            if (portionID == 1)// raw Out
            {
                sRSStatus = ((int)EnumRSState.YarnOut).ToString();
            }
            else if (portionID == 2)// Qty_Machine
            {
                sRSStatus = ((int)EnumRSState.LoadedInDyeMachine).ToString() + "," + ((int)EnumRSState.UnloadedFromDyeMachine).ToString();
            }
            else if (portionID == 3)// Qty_Hydro
            {
                sRSStatus = ((int)EnumRSState.LoadedInHydro).ToString() + "," + ((int)EnumRSState.UnloadedFromHydro).ToString();
            }
            else if (portionID == 4)// Qty_Dryer
            {
                sRSStatus = ((int)EnumRSState.UnLoadedFromDrier).ToString() + "," + ((int)EnumRSState.UnLoadedFromDrier).ToString();
            }
            else if (portionID == 5)// Qty_WQC
            {
                sRSStatus = ((int)EnumRSState.InPackageing).ToString();
            }
            else if (portionID == 6)// Qty_QCD
            {
                sRSStatus = ((int)EnumRSState.QC_Done).ToString();
            }
            else if (portionID == 7)// Qty_Hydro
            {
                sRSStatus = ((int)EnumRSState.InPackageing).ToString() + "," + ((int)EnumRSState.QC_Done).ToString() + "," + ((int)EnumRSState.InSubFinishingstore_Partially).ToString() + "," + ((int)EnumRSState.InHW_Sub_Store).ToString() + "," + ((int)EnumRSState.InDelivery).ToString();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RouteSheetID in (SELECT RouteSheetID FROM RSInQcDetail where isnull(RSInQcDetail.ManagedLotID,0)=0 and  RSInQCSetupID  in (Select RSInQCSetupID from RSInQCSetup where YarnType not in (" + (int)EnumYarnType.FreshDyedYarn + "," + (int)EnumYarnType.DyedYarnOne + "," + (int)EnumYarnType.DyedYarnTwo + "," + (int)EnumYarnType.DyedYarnThree + ")))";
            }
            else if (portionID == 8)// Qty_WForStore
            {
                sRSStatus = ((int)EnumRSState.InSubFinishingstore_Partially).ToString() + "," + ((int)EnumRSState.InHW_Sub_Store).ToString() + "," + ((int)EnumRSState.InDelivery).ToString() + "," + ((int)EnumRSState.QC_Done).ToString() + "," + ((int)EnumRSState.Finished).ToString();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RouteSheetID in (SELECT RSInQcDetail.RouteSheetID FROM RSInQcDetail where ManagedLotID in (Select Lot.LotID from Lot where Balance>0.02 and ParentType=106) and   RSInQCSetupID in (Select RSInQCSetupID from RSInQCSetup where YarnType in (" + (int)EnumYarnType.FreshDyedYarn + "," + (int)EnumYarnType.DyedYarnOne + "," + (int)EnumYarnType.DyedYarnTwo + "," + (int)EnumYarnType.DyedYarnThree + ")))";
            }

            if (portionID > 0 && portionID < 9)// raw Out
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "RS.RSState in(" + sRSStatus + ")";
            }
            #endregion
            #region Issue Date
         
            if (IsDate)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RouteSheetID in (Select RSH.RouteSheetID from RouteSheetHistory as RSH where CurrentStatus in (" + ((int)EnumRSState.YarnOut).ToString() + ") and  CONVERT(DATE,CONVERT(VARCHAR(12),EventTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStratDate.ToString("dd MMM yyyy 08:00:00") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy 08:00:00") + "',106))) ";

            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + "";
            return sSQL;
        }
        public ActionResult Print_DetailRS(int buid, int ViewType, int portionID, string searchStr, string sName)
        {
            string sSQL = MakeSQL_View_RS(buid, ViewType, portionID, searchStr);
            _oDUDashboardProductions = DUDashboardProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            string sHeader = "Production Details (" + (string.IsNullOrEmpty(sName) ? "" : sName+",  " ) + _sDateRange + ")";

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompanys.First();
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (_oDUDashboardProductions.Count == 0)
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!!");
                return File(abytes, "application/pdf");
            }
            else
            {
                List<SelectedField> oSelectedFields = new List<SelectedField>();
                SelectedField oSelectedField = new SelectedField("RouteSheetDateSt", "Date", 47, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("RouteSheetNo", "Batch No", 47, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("OrderNo", "Order No", 47, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Qty_Out", "Qty Out", 47, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Qty_Fresh", "Qty Fresh", 47, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Qty_UnManage", "Qty UnManage", 47, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("StockInHand", "Stock In Hand", 47, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);

                rptDynamicReport oReport = new rptDynamicReport(595,842);
                oReport.SpanTotal = 4;//ColSpanForTotal
                byte[] abytes = oReport.PrepareReport(_oDUDashboardProductions.Cast<object>().ToList(), oBusinessUnit, oCompany, sHeader, oSelectedFields);
                return File(abytes, "application/pdf");
            }
        }
        public void PrintXL_DetailRS(int buid, int ViewType, int portionID, string searchStr)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            string _sHeaderName = "";
           
            if (!string.IsNullOrEmpty(searchStr))
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                _sHeaderName = "Unit: " + oBusinessUnit.Name;

                try
                {
                    string sSQL = MakeSQL_View_RS(buid, ViewType, portionID, searchStr);
                    _oDUDashboardProductions = DUDashboardProduction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                catch (Exception ex)
                {
                    _oDUDashboardProductions = new List<DUDashboardProduction>();
                    DUDashboardProduction oDUDashboardProduction = new DUDashboardProduction();
                    oDUDashboardProduction.ErrorMessage = ex.Message;
                    _oDUDashboardProductions.Add(oDUDashboardProduction); 
                }
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            #region Print XL

            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 9, nColumn = 1, nCount = 0, nImportLCID = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("DDBProductionDetails");
                sheet.Name = "Production Details";

                sheet.Column(++nColumn).Width = 10; //SL No
                sheet.Column(++nColumn).Width = 25; //LC Date
                sheet.Column(++nColumn).Width = 20; //Batch No
                sheet.Column(++nColumn).Width = 20; //Order No
                sheet.Column(++nColumn).Width = 15; //Qty
                sheet.Column(++nColumn).Width = 15; //QtyFr
                sheet.Column(++nColumn).Width = 15; //QtyUnM
                sheet.Column(++nColumn).Width = 15; //SIH
                //sheet.Column(++nColumn).Width = 15; //LC Val
                //sheet.Column(++nColumn).Width = 15; //InvoiceQty

                //nCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol + 4]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 5, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Production Details"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol + 4]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol + 5, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _sDateRange; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion


                #region Report Data

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region
                nColumn = 1;
                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Batch No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty Fresh"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty UnManage"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Stock In Hand"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
                nRowIndex++;
                #endregion

                #region Data

                foreach (DUDashboardProduction oItem in _oDUDashboardProductions)
                {
                    nColumn = 1;

                    nCount++;
                    //int nRowSpan = _oDUDashboardProductions.Where(PIR => PIR.ImportLCID == oItem.ImportLCID).ToList().Count - 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.RouteSheetDateSt.ToString(); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.RouteSheetNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_Out; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_Fresh; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_UnManage; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.StockInHand; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEndRow = nRowIndex;
                    nRowIndex++;
                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 4]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                double nValue = _oDUDashboardProductions.Select(c => c.Qty_Out).Sum();
                cell = sheet.Cells[nRowIndex, nEndCol - 3]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "# #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oDUDashboardProductions.Select(c => c.Qty_Fresh).Sum();
                cell = sheet.Cells[nRowIndex, nEndCol - 2]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "# #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oDUDashboardProductions.Select(c => c.Qty_UnManage).Sum();
                cell = sheet.Cells[nRowIndex, nEndCol-1]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "# #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oDUDashboardProductions.Select(c => c.StockInHand).Sum();
                cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "# #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Dashboard_Production_Details.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
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
    
    }//End
}