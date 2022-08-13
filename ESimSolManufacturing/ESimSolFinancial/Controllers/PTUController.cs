using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using ReportManagement;
using iTextSharp.text;
using System.Data;

namespace ESimSolFinancial.Controllers
{
    public class PTUController : Controller
    {
        PTU _oPTU = new PTU();
        List<PTU> _oPTUs = new List<PTU>();
        string _sErrorMessage = "";

        public ActionResult ViewPTUs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPTUs = new List<PTU>();
            return View(_oPTUs);
        }
        public ActionResult ViewPTUDistributions(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPTUs = new List<PTU>();
            return View(_oPTUs);
        }

        public ActionResult ViewPTU_DO(int nID)
        {
             List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            _oPTU = new PTU();
            _oPTU = PTU.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oDyeingOrderDetails = DyeingOrderDetail.GetsBy(_oPTU.LabDipDetailID, _oPTU.OrderID, (int)_oPTU.OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DyeingOrderDetails = oDyeingOrderDetails;

            return View(_oPTU);
        }
        public ActionResult ViewPTUDistribution(int nID)
        {
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<PTUDistribution> oPTUDistributions = new List<PTUDistribution>();
            _oPTU = new PTU();
            _oPTU = PTU.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDyeingOrderDetails = DyeingOrderDetail.GetsBy(_oPTU.LabDipDetailID, _oPTU.OrderID, (int)_oPTU.OrderType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oPTUDistributions = PTUDistribution.Gets(_oPTU.PTUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            ViewBag.DyeingOrderDetails = oDyeingOrderDetails;
            ViewBag.PTUDistributions = oPTUDistributions;

            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = oOrderTypes;
            ViewBag.LabDipTypeObj = EnumObject.jGets(typeof(EnumLabDipType));

            return View(_oPTU);
        }
          
    
        #region Search
        private string MakeSQL(PTU oPTU)
        {

            string sPINo = Convert.ToString(oPTU.Params.Split('~')[0]);
            string sOrderNo = Convert.ToString(oPTU.Params.Split('~')[1]);
            string sSampleOrderNo = Convert.ToString(oPTU.Params.Split('~')[2]);
            //int OrderType = Convert.ToInt32(oPTU.Params.Split('~')[3]);

            string sReturn1 = "SELECT * FROM View_PTUs ";
            string sReturn = "";
         
            if (!String.IsNullOrEmpty(sPINo))
            {
                sPINo = sPINo.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "PINo like '%" + sPINo + "%'";
            }
            if (!String.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType =3 and OrderID in (Select ExportSCID from ExportSC where ExportPIID in (Select ExportPIID from DyeingOrder where DyeingOrder.OrderNo like  '%" + sOrderNo + "%' ) )";
            }
            if (!String.IsNullOrEmpty(sSampleOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType in (2) and OrderID in (Select DyeingOrderID from DyeingOrder where OrderNo like '%" + sSampleOrderNo + "%')";
            }
            sReturn = sReturn1 + sReturn;
           
            return sReturn;
        }
         [HttpPost]
         public JsonResult GetbyNo(PTU oPTU)
         {
             string sSQL = "";
              sSQL = MakeSQL(oPTU);
             _oPTUs = PTU.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(_oPTUs);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }

         [HttpPost]
         public JsonResult GetsDyeingOrderDetail(RouteSheetDO oRouteSheetDO)// For Delivery Order
         {
             string sSQL = "";

             List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
             try
             {
                 
                     sSQL = "Select * from View_DyeingOrderDetailWaitingForRS ";
                     string sReturn = "";
                     if (!String.IsNullOrEmpty(oRouteSheetDO.OrderNo))
                     {
                         oRouteSheetDO.OrderNo = oRouteSheetDO.OrderNo.Trim();
                         Global.TagSQL(ref sReturn);
                         sReturn = sReturn + "OrderNo Like'%" + oRouteSheetDO.OrderNo + "'";
                     }
                     if (oRouteSheetDO.OrderTypeInt >0)
                     {
                         Global.TagSQL(ref sReturn);
                         sReturn = sReturn + "OrderType=" + oRouteSheetDO.OrderTypeInt + "";
                     }
                     if (oRouteSheetDO.LabDipDetailID > 0)
                     {
                         Global.TagSQL(ref sReturn);
                         sReturn = sReturn + "LabDipDetailID=" + oRouteSheetDO.LabDipDetailID + "";
                     }
                     sSQL = sSQL + "" + sReturn;
                     oRouteSheetDOs = RouteSheetDO.GetsDOYetTORS(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

             }
             catch (Exception ex)
             {
                 oRouteSheetDOs = new List<RouteSheetDO>();
             }
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             string sjson = serializer.Serialize(oRouteSheetDOs);
             return Json(sjson, JsonRequestBehavior.AllowGet);
         }
        #endregion

      
 




    }
}
