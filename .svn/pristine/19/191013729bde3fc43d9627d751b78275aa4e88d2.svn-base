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
    public class FabricPlanController : Controller
    {
        #region Declaration
        FabricSCReport _oFabricExecutionOrder = new FabricSCReport();
        FabricPlan _oFabricPlan = new FabricPlan();
        List<FabricPlan> _oFabricPlans = new List<FabricPlan>();
        List<FabricPlanDetail> _oFabricPlanDetails = new List<FabricPlanDetail>();
        #endregion

        #region Planning
        public ActionResult ViewFabricPlans(int id, int nType)//fabricid, Reftype:fabric
        {
            string sSQL = "";
            Fabric oFabric = new Fabric();
            FabricSCReport oFabricExecutionOrder = new FabricSCReport();
            List<FabricPlanOrder> oFabricPlanOrders = new List<FabricPlanOrder>();
            //  oFabricPlanOrders = FabricPlanOrder.Gets(id, nType, (int)Session[SessionInfo.currentUserID]);
            ViewBag.RefType = nType;
            ViewBag.RefID = id;
            ViewBag.WarpWeftTypes = EnumObject.jGets(typeof(EnumWarpWeft));
            ViewBag.FabricPlanRefTypes = EnumObject.jGets(typeof(EnumFabricPlanRefType));
            oFabricPlanOrders = FabricPlanOrder.Gets(id, nType, (int)Session[SessionInfo.currentUserID]);

            if (nType == (int)EnumFabricPlanRefType.Dispo)
            {
                oFabricExecutionOrder = oFabricExecutionOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            else if (nType == (int)EnumFabricPlanRefType.Fabric)
            {
                oFabric = oFabric.Get(id, (int)Session[SessionInfo.currentUserID]);
                oFabricExecutionOrder.FabricID = oFabric.FabricID;
                oFabricExecutionOrder.FabricNo = oFabric.FabricNo;
                oFabricExecutionOrder.FabricWeave = oFabric.FabricWeave;
                oFabricExecutionOrder.BuyerID = oFabric.BuyerID;
                oFabricExecutionOrder.BuyerName = oFabric.BuyerName;
                oFabricExecutionOrder.Construction = oFabric.Construction;
            }
            oFabricPlanOrders.ForEach(o => o.RefNo = oFabricExecutionOrder.FabricNo);
            ViewBag.FabricSCReport = oFabricExecutionOrder;
            sSQL = "SELECT * FROM FabricProcess WHERE ProcessType=" + (int)EnumFabricProcess.Weave;
            ViewBag.FabricWeaves = FabricProcess.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            // ViewBag.FabricPlans = FabricPlan.Gets("SELECT * FROM View_FabricPlan WHERE RefID =" + id + " AND RefType = " + (int)EnumFabricPlanRefType.Fabric+ " Order By WarpWeftType,SLNo ", (int)Session[SessionInfo.currentUserID]);
            return View(oFabricPlanOrders);
        }
        public ActionResult ViewFabricPlan(int id, int nType, double ts)//PlanOrderID, warpWeftType
        {
            FabricSCReport oFabricExecutionOrder = new FabricSCReport();
            Fabric oFabric = new Fabric();
            List<FabricPlanDetail> _oFabricPlanDetails = new List<FabricPlanDetail>();
            FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder();

            _oFabricPlans = new List<FabricPlan>();
            string sSQL = "";
            oFabricPlanOrder = FabricPlanOrder.Gets("Select * from  View_FabricPlanOrder where RefID =" + id + " and RefType=" + nType, (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
            if (nType == (int)EnumFabricPlanRefType.Dispo)
            {
                oFabricExecutionOrder = oFabricExecutionOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oFabric = oFabric.Get(id, (int)Session[SessionInfo.currentUserID]);
                oFabricExecutionOrder.FabricID = oFabric.FabricID;
                oFabricExecutionOrder.FabricNo = oFabric.FabricNo;
                oFabricExecutionOrder.FabricWeave = oFabric.FabricWeave;
                oFabricExecutionOrder.BuyerID = oFabric.BuyerID;
                oFabricExecutionOrder.BuyerName = oFabric.BuyerName;
                oFabricExecutionOrder.Construction = oFabric.Construction;
            }
            if (oFabricPlanOrder != null && oFabricPlanOrder.FabricPlanOrderID > 0)
            {

                _oFabricPlans = FabricPlan.Gets("SELECT * FROM View_FabricPlan WHERE FabricPlanOrderID =" + oFabricPlanOrder.FabricPlanOrderID + " Order By WarpWeftType,SLNo ", (int)Session[SessionInfo.currentUserID]);
                if (_oFabricPlans.Count > 0)
                {
                    _oFabricPlanDetails = FabricPlanDetail.Gets("SELECT * FROM View_FabricPlanDetail where FabricPlanID in (" + string.Join(",", _oFabricPlans.Select(x => x.FabricPlanID).Distinct().ToList()) + ")", (int)Session[SessionInfo.currentUserID]);
                    _oFabricPlans.ForEach(x =>
                    {
                        if (_oFabricPlanDetails.FirstOrDefault() != null && _oFabricPlanDetails.Where(b => b.FabricPlanID == x.FabricPlanID).Count() > 0)
                        {
                            x.FabricPlanDetails = _oFabricPlanDetails.Where(p => p.FabricPlanID == x.FabricPlanID && p.FabricPlanID > 0).ToList();
                        }
                    });

                    if (_oFabricPlans.FirstOrDefault() != null && _oFabricPlans.FirstOrDefault().FabricPlanID > 0 && _oFabricPlans.Where(x => x.TwistedGroup > 0).Count() > 0)
                    {
                        List<FabricPlan> oFabricPlans = new List<FabricPlan>();
                        _oFabricPlans.ForEach((item) => { oFabricPlans.Add(item); });
                        _oFabricPlans = this.ComboFabricPlans(_oFabricPlans);
                        _oFabricPlans[0].CellRowSpans = this.RowMerge(oFabricPlans);
                    }

                }
            }
            else
            {
                oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.ColumnCount = 10;
                oFabricPlanOrder.Weave = oFabricExecutionOrder.FabricWeave;
                oFabricPlanOrder.RefID = id;
                oFabricPlanOrder.RefType = (EnumFabricPlanRefType)nType;
            }

            sSQL = "SELECT * FROM FabricProcess WHERE ProcessType=" + (int)EnumFabricProcess.Weave;
            ViewBag.FabricWeaves = FabricProcess.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.WarpWeftTypes = EnumObject.jGets(typeof(EnumWarpWeft));
            ViewBag.FabricPlanRefTypes = EnumObject.jGets(typeof(EnumFabricPlanRefType));
            ViewBag.FabricID = oFabricExecutionOrder.FabricID;
            ViewBag.FabricPlanOrder = oFabricPlanOrder;
            ViewBag.FabricSCReport = oFabricExecutionOrder;
            return View(_oFabricPlans);
        }




        [HttpPost]
        public JsonResult LoadFabricPlanInfo(FabricPlanOrder oFabricPlanOrder)//PlanOrderID, fabricid, warpWeftType
        {
            FabricSCReport oFabricExecutionOrder = new FabricSCReport();
            FabricPlan oFabricPlan = new FabricPlan();
            int nTempFabricID = oFabricPlanOrder.FabricID;
            int nWarpWeftType = (int)oFabricPlanOrder.WarpWeftType;

            Fabric oFabric = new Fabric();
            List<FabricPlanDetail> _oFabricPlanDetails = new List<FabricPlanDetail>();
            _oFabricPlans = new List<FabricPlan>();
            string sSQL = "";
            oFabricPlanOrder = oFabricPlanOrder.Get(oFabricPlanOrder.FabricPlanOrderID, (int)Session[SessionInfo.currentUserID]);
            if (oFabricPlanOrder.FabricPlanOrderID == 0)//for New entry
            {
                oFabric = oFabric.Get(nTempFabricID, (int)Session[SessionInfo.currentUserID]);
                oFabricExecutionOrder.FabricID = oFabric.FabricID;
                oFabricExecutionOrder.FabricNo = oFabric.FabricNo;
                oFabricExecutionOrder.FabricWeave = oFabric.FabricWeave;
                oFabricExecutionOrder.BuyerID = oFabric.BuyerID;
                oFabricExecutionOrder.BuyerName = oFabric.BuyerName;
                oFabricExecutionOrder.Construction = oFabric.Construction;
            }
            if (oFabricPlanOrder != null && oFabricPlanOrder.FabricPlanOrderID > 0)
            {

                _oFabricPlans = FabricPlan.Gets("SELECT * FROM View_FabricPlan WHERE FabricPlanOrderID =" + oFabricPlanOrder.FabricPlanOrderID + " AND WarpWeftType = " + nWarpWeftType + " Order By SLNo ", (int)Session[SessionInfo.currentUserID]);
                if (_oFabricPlans.Count > 0)
                {
                    _oFabricPlanDetails = FabricPlanDetail.Gets("SELECT * FROM View_FabricPlanDetail where FabricPlanID in (" + string.Join(",", _oFabricPlans.Select(x => x.FabricPlanID).Distinct().ToList()) + ")", (int)Session[SessionInfo.currentUserID]);
                    _oFabricPlans.ForEach(x =>
                    {
                        if (_oFabricPlanDetails.FirstOrDefault() != null && _oFabricPlanDetails.Where(b => b.FabricPlanID == x.FabricPlanID).Count() > 0)
                        {
                            x.FabricPlanDetails = _oFabricPlanDetails.Where(p => p.FabricPlanID == x.FabricPlanID && p.FabricPlanID > 0).ToList();
                        }
                    });

                    if (_oFabricPlans.FirstOrDefault() != null && _oFabricPlans.FirstOrDefault().FabricPlanID > 0 && _oFabricPlans.Where(x => x.TwistedGroup > 0).Count() > 0)
                    {
                        List<FabricPlan> oFabricPlans = new List<FabricPlan>();
                        _oFabricPlans.ForEach((item) => { oFabricPlans.Add(item); });
                        _oFabricPlans = this.ComboFabricPlans(_oFabricPlans);
                        _oFabricPlans[0].CellRowSpans = this.RowMerge(oFabricPlans);
                    }

                }
                oFabricPlanOrder.FabricPlans = _oFabricPlans;
            }
            else
            {
                oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.ColumnCount = 10;
                oFabricPlanOrder.Weave = oFabricExecutionOrder.FabricWeave;
                oFabricPlanOrder.RefID = oFabricExecutionOrder.FabricID;
                oFabricPlanOrder.RefType = EnumFabricPlanRefType.Fabric;
            }


            oFabricPlanOrder.FabricID = oFabricExecutionOrder.FabricID;
            //  oFabricPlanOrder.FabricPlanOrder = oFabricPlanOrder;
            //oFabricPlanOrder.FabricExecutionOrder = oFabricExecutionOrder;


            var jsonResult = Json(oFabricPlanOrder, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oFabricPlanOrder);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SavePlanning(FabricPlan oFabricPlan)
        {
            _oFabricPlan = new FabricPlan();
            try
            {
                _oFabricPlan = oFabricPlan;
                _oFabricPlan = _oFabricPlan.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricPlan = new FabricPlan();
                _oFabricPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePlanning(List<FabricPlan> oFabricPlansDel)
        {
            _oFabricPlan = new FabricPlan();
            _oFabricPlans = new List<FabricPlan>();
            FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder();
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = _oFabricPlan.Delete(oFabricPlansDel, (int)Session[SessionInfo.currentUserID]);

                if (oFabricPlansDel.Count > 0)
                {
                    oFabricPlanOrder = oFabricPlanOrder.Get(oFabricPlansDel[0].FabricPlanOrderID, (int)Session[SessionInfo.currentUserID]);
                }
                if (oFabricPlanOrder.FabricPlanOrderID > 0)
                {
                    _oFabricPlans = FabricPlan.Gets("SELECT * FROM View_FabricPlan WHERE FabricPlanOrderID =" + oFabricPlanOrder.FabricPlanOrderID + " Order By WarpWeftType,SLNo ", (int)Session[SessionInfo.currentUserID]);
                    oFabricPlanOrder.FabricPlanRepeats = FabricPlanRepeat.Gets("SELECT * FROM View_FabricPlanRepeat WHERE  FabricPlanOrderID = " + oFabricPlanOrder.FabricPlanOrderID + " ORDER BY FabricPlanRepeatID", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oFabricPlans.Count > 0)
                    {
                        _oFabricPlanDetails = FabricPlanDetail.Gets("SELECT * FROM View_FabricPlanDetail where FabricPlanID in (" + string.Join(",", _oFabricPlans.Select(x => x.FabricPlanID).Distinct().ToList()) + ")", (int)Session[SessionInfo.currentUserID]);
                        _oFabricPlans.ForEach(x =>
                        {
                            if (_oFabricPlanDetails.FirstOrDefault() != null && _oFabricPlanDetails.Where(b => b.FabricPlanID == x.FabricPlanID).Count() > 0)
                            {
                                x.FabricPlanDetails = _oFabricPlanDetails.Where(p => p.FabricPlanID == x.FabricPlanID && p.FabricPlanID > 0).ToList();
                            }
                        });

                        if (_oFabricPlans.FirstOrDefault() != null && _oFabricPlans.FirstOrDefault().FabricPlanID > 0 && _oFabricPlans.Where(x => x.TwistedGroup > 0).Count() > 0)
                        {
                            List<FabricPlan> oFabricPlans = new List<FabricPlan>();
                            _oFabricPlans.ForEach((item) => { oFabricPlans.Add(item); });
                            _oFabricPlans = this.ComboFabricPlans(_oFabricPlans);
                            _oFabricPlans[0].CellRowSpans = this.RowMerge(oFabricPlans);
                        }
                    }

                    oFabricPlanOrder.FabricPlans = _oFabricPlans;
                    //  oFabricPlanOrder.FabricPlanRepeats = oFabricPlanRepeats;

                }

            }
            catch (Exception ex)
            {
                oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.ErrorMessage = ex.Message;

            }
            var jsonResult = Json(oFabricPlanOrder, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult UpdateYarn(List<FabricPlan> oFabricPlans)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = _oFabricPlan.UpdateYarn(oFabricPlans, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult DeleteAll(FabricPlanOrder oFabricPlanOrder)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricPlanOrder.Delete(oFabricPlanOrder.FabricPlanOrderID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult UpdateLDDetailID(FabricPlan oFabricPlan)
        {
            _oFabricPlan = new FabricPlan();
            try
            {
                _oFabricPlan = oFabricPlan.UpdateLDDetailID((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricPlan = new FabricPlan();
                _oFabricPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CopyFabricWefts(FabricPlanOrder oFabricPlanOrderTemp)
        {
            _oFabricPlan = new FabricPlan();
            _oFabricPlans = new List<FabricPlan>();
            FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder();
            try
            {
                oFabricPlanOrder = oFabricPlanOrderTemp.CopyFabricPlans((int)Session[SessionInfo.currentUserID]);
                //   _oFabricPlans = oFabricPlan.CopyFabricPlans((int)Session[SessionInfo.currentUserID]);


                // oFabricPlanOrder = oFabricPlanOrder.Get(_oFabricPlans[0].FabricPlanOrderID, (int)Session[SessionInfo.currentUserID]);

                if (oFabricPlanOrder.FabricPlanOrderID > 0)
                {
                    _oFabricPlans = FabricPlan.Gets("SELECT * FROM View_FabricPlan WHERE WarpWeftType=" + (int)EnumWarpWeft.Weft + " and FabricPlanOrderID =" + oFabricPlanOrder.FabricPlanOrderID + " Order By WarpWeftType,SLNo ", (int)Session[SessionInfo.currentUserID]);
                    oFabricPlanOrder.FabricPlanRepeats = FabricPlanRepeat.Gets("SELECT * FROM View_FabricPlanRepeat WHERE WarpWeftType=" + (int)EnumWarpWeft.Weft + " and FabricPlanOrderID = " + oFabricPlanOrder.FabricPlanOrderID + " ORDER BY FabricPlanRepeatID", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oFabricPlans.Count > 0)
                    {
                        _oFabricPlanDetails = FabricPlanDetail.Gets("SELECT * FROM View_FabricPlanDetail where FabricPlanID in (" + string.Join(",", _oFabricPlans.Select(x => x.FabricPlanID).Distinct().ToList()) + ")", (int)Session[SessionInfo.currentUserID]);
                    }
                    _oFabricPlans.ForEach(x =>
                    {
                        if (_oFabricPlanDetails.FirstOrDefault() != null && _oFabricPlanDetails.Where(b => b.FabricPlanID == x.FabricPlanID).Count() > 0)
                        {
                            x.FabricPlanDetails = _oFabricPlanDetails.Where(p => p.FabricPlanID == x.FabricPlanID && p.FabricPlanID > 0).ToList();
                        }
                    });

                    if (_oFabricPlans.FirstOrDefault() != null && _oFabricPlans.FirstOrDefault().FabricPlanID > 0 && _oFabricPlans.Where(x => x.TwistedGroup > 0).Count() > 0)
                    {
                        List<FabricPlan> oFabricPlans = new List<FabricPlan>();
                        _oFabricPlans.ForEach((item) => { oFabricPlans.Add(item); });
                        _oFabricPlans = this.ComboFabricPlans(_oFabricPlans);
                        _oFabricPlans[0].CellRowSpans = this.RowMerge(oFabricPlans);
                    }

                    oFabricPlanOrder.FabricPlans = _oFabricPlans;
                    //  oFabricPlanOrder.FabricPlanRepeats = oFabricPlanRepeats;

                }



            }
            catch (Exception ex)
            {
                oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.ErrorMessage = ex.Message;

            }
            var jsonResult = Json(oFabricPlanOrder, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult CopyFabricPlans(FabricPlanOrder oFabricPlanOrderTemp)
        {
            _oFabricPlan = new FabricPlan();
            _oFabricPlans = new List<FabricPlan>();
            FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder();
            try
            {
                if (oFabricPlanOrderTemp.FabricPlanOrderIDFrom <= 0)
                {
                    oFabricPlanOrder = FabricPlanOrder.Gets("Select top(1)* from  View_FabricPlanOrder where RefID =" + oFabricPlanOrderTemp.FabricID + " and RefType=" + (int)EnumFabricPlanRefType.Fabric, (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
                    oFabricPlanOrderTemp.FabricPlanOrderIDFrom = oFabricPlanOrder.FabricPlanOrderID;
                }
                //oFabricPlan.FabricPlanOrderID = oFabricPlanOrder.FabricPlanOrderID;
                oFabricPlanOrder = oFabricPlanOrderTemp.CopyFabricPlans((int)Session[SessionInfo.currentUserID]);
                //if (_oFabricPlans.Count > 0)
                //{
                //    oFabricPlanOrder = oFabricPlanOrder.Get(_oFabricPlans[0].FabricPlanOrderID, (int)Session[SessionInfo.currentUserID]);

                //    if (oFabricPlanOrder.FabricPlanOrderID > 0)
                //    {
                //        //_oFabricPlans = FabricPlan.Gets("SELECT * FROM View_FabricPlan WHERE FabricPlanOrderID =" + oFabricPlanOrder.FabricPlanOrderID + " Order By WarpWeftType,SLNo ", (int)Session[SessionInfo.currentUserID]);
                //        oFabricPlanOrder.FabricPlanRepeats = FabricPlanRepeat.Gets("SELECT * FROM View_FabricPlanRepeat WHERE  FabricPlanOrderID = " + oFabricPlanOrder.FabricPlanOrderID + " ORDER BY FabricPlanRepeatID", ((User)Session[SessionInfo.CurrentUser]).UserID);
                //        _oFabricPlanDetails = FabricPlanDetail.Gets("SELECT * FROM View_FabricPlanDetail where FabricPlanID in (" + string.Join(",", _oFabricPlans.Select(x => x.FabricPlanID).Distinct().ToList()) + ")", (int)Session[SessionInfo.currentUserID]);
                //        _oFabricPlans.ForEach(x =>
                //        {
                //            if (_oFabricPlanDetails.FirstOrDefault() != null && _oFabricPlanDetails.Where(b => b.FabricPlanID == x.FabricPlanID).Count() > 0)
                //            {
                //                x.FabricPlanDetails = _oFabricPlanDetails.Where(p => p.FabricPlanID == x.FabricPlanID && p.FabricPlanID > 0).ToList();
                //            }
                //        });

                //        if (_oFabricPlans.FirstOrDefault() != null && _oFabricPlans.FirstOrDefault().FabricPlanID > 0 && _oFabricPlans.Where(x => x.TwistedGroup > 0).Count() > 0)
                //        {
                //            List<FabricPlan> oFabricPlans = new List<FabricPlan>();
                //            _oFabricPlans.ForEach((item) => { oFabricPlans.Add(item); });
                //            _oFabricPlans = this.ComboFabricPlans(_oFabricPlans);
                //            _oFabricPlans[0].CellRowSpans = this.RowMerge(oFabricPlans);
                //        }

                //        oFabricPlanOrder.FabricPlans = _oFabricPlans;
                //      //  oFabricPlanOrder.FabricPlanRepeats = oFabricPlanRepeats;

                //    }

                //}

            }
            catch (Exception ex)
            {
                oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.ErrorMessage = ex.Message;

            }
            var jsonResult = Json(oFabricPlanOrder, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oFabricPlanOrder);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult DeleteAll(FabricPlan oFabricPlan)
        //{
        //    string sFeedBackMessage = "";
        //    FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder(); 
        //    try
        //    {
        //        sFeedBackMessage = oFabricPlanOrder.Delete(oFabricPlan.FabricPlanOrderID, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        sFeedBackMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(sFeedBackMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult SaveAll(List<FabricPlanDetail> oFabricPlanDetails)
        {
            FabricPlanDetail oFabricPlanDetail = new FabricPlanDetail();
            _oFabricPlanDetails = new List<FabricPlanDetail>();
            try
            {
                _oFabricPlanDetails = FabricPlanDetail.SaveAll(oFabricPlanDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFabricPlanDetail = new FabricPlanDetail();
                oFabricPlanDetail.ErrorMessage = ex.Message;
                _oFabricPlanDetails = new List<FabricPlanDetail>();
                _oFabricPlanDetails.Add(oFabricPlanDetail);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricPlanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save(FabricPlanOrder oFabricPlanOrder)
        {
            FabricPlanOrder oFabricPlanDetail = new FabricPlanOrder();
            List<FabricPlanRepeat> oFabricPlanRepeats = new List<FabricPlanRepeat>();
            _oFabricPlanDetails = new List<FabricPlanDetail>();
            int nTempWarpWeftType = (int)oFabricPlanOrder.WarpWeftType;
            try
            {
                oFabricPlanOrder = oFabricPlanOrder.Save(((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oFabricPlanOrder.FabricPlanOrderID > 0)
                {
                    _oFabricPlans = FabricPlan.Gets("SELECT * FROM View_FabricPlan WHERE FabricPlanOrderID =" + oFabricPlanOrder.FabricPlanOrderID + " AND  WarpWeftType =" + nTempWarpWeftType + "  Order By SLNo ", (int)Session[SessionInfo.currentUserID]);
                    oFabricPlanRepeats = FabricPlanRepeat.Gets("SELECT * FROM View_FabricPlanRepeat WHERE  FabricPlanOrderID = " + oFabricPlanOrder.FabricPlanOrderID + " AND  WarpWeftType =" + nTempWarpWeftType + "  ORDER BY FabricPlanRepeatID", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oFabricPlanDetails = FabricPlanDetail.Gets("SELECT * FROM View_FabricPlanDetail where FabricPlanID in (" + string.Join(",", _oFabricPlans.Select(x => x.FabricPlanID).Distinct().ToList()) + ")", (int)Session[SessionInfo.currentUserID]);
                    _oFabricPlans.ForEach(x =>
                    {
                        if (_oFabricPlanDetails.FirstOrDefault() != null && _oFabricPlanDetails.Where(b => b.FabricPlanID == x.FabricPlanID).Count() > 0)
                        {
                            x.FabricPlanDetails = _oFabricPlanDetails.Where(p => p.FabricPlanID == x.FabricPlanID && p.FabricPlanID > 0).ToList();
                        }
                    });

                    if (_oFabricPlans.FirstOrDefault() != null && _oFabricPlans.FirstOrDefault().FabricPlanID > 0 && _oFabricPlans.Where(x => x.TwistedGroup > 0).Count() > 0)
                    {
                        List<FabricPlan> oFabricPlans = new List<FabricPlan>();
                        _oFabricPlans.ForEach((item) => { oFabricPlans.Add(item); });
                        _oFabricPlans = this.ComboFabricPlans(_oFabricPlans);
                        _oFabricPlans[0].CellRowSpans = this.RowMerge(oFabricPlans);
                    }

                    oFabricPlanOrder.FabricPlans = _oFabricPlans;
                    oFabricPlanOrder.FabricPlanRepeats = oFabricPlanRepeats;
                    oFabricPlanOrder.FabricPlanDetails = new List<FabricPlanDetail>();

                }

            }
            catch (Exception ex)
            {
                oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.ErrorMessage = ex.Message;
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oFabricPlanOrder);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(oFabricPlanOrder, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult PrintFabricPlan(int nID, int buid)
        {
            List<FabricPlanRepeat> oFabricPlanRepeats = new List<FabricPlanRepeat>();
            FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder();
            Fabric oFabric = new Fabric();
            oFabricPlanOrder = FabricPlanOrder.Gets("SELECT * FROM View_FabricPlanOrder WHERE FabricPlanOrderID =" + nID, (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
            if (oFabricPlanOrder.RefType == EnumFabricPlanRefType.Dispo)
            {
                _oFabricExecutionOrder = _oFabricExecutionOrder.Get(oFabricPlanOrder.RefID, (int)Session[SessionInfo.currentUserID]);
            }
            if (oFabricPlanOrder.RefType == EnumFabricPlanRefType.Fabric)
            {
                oFabric = oFabric.Get(oFabricPlanOrder.RefID, (int)Session[SessionInfo.currentUserID]);
                _oFabricExecutionOrder.ExeNo = "";
                _oFabricExecutionOrder.FabricNo = oFabric.FabricNo;
                _oFabricExecutionOrder.FabricWeave = oFabric.FabricWeave;
                _oFabricExecutionOrder.BuyerName = oFabric.BuyerName;
                _oFabricExecutionOrder.FabricWidth = oFabric.FabricWidth;
                oFabricPlanOrder.FabricDesignName = oFabric.FabricDesignName;
                _oFabricExecutionOrder.Construction = oFabric.Construction;
                _oFabricExecutionOrder.StyleNo = oFabric.StyleNo;

            }

            _oFabricPlans = FabricPlan.Gets("SELECT * FROM View_FabricPlan WHERE  FabricPlanOrderID =" + oFabricPlanOrder.FabricPlanOrderID, (int)Session[SessionInfo.currentUserID]);
            _oFabricPlanDetails = FabricPlanDetail.Gets("SELECT * FROM View_FabricPlanDetail where FabricPlanID in (" + string.Join(",", _oFabricPlans.Select(x => x.FabricPlanID).Distinct().ToList()) + ")", (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT * FROM View_FabricPlanRepeat WHERE  FabricPlanOrderID = " + oFabricPlanOrder.FabricPlanOrderID + " ORDER BY FabricPlanRepeatID";
            oFabricPlanRepeats = FabricPlanRepeat.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oFabricPlans.Any())
            {
                rptFabricPlan oReport = new rptFabricPlan();
                byte[] abytes = oReport.PrepareReport(oFabricPlanOrder, _oFabricPlans, _oFabricPlanDetails, oFabricPlanRepeats, _oFabricExecutionOrder, oCompany, oBusinessUnit, "");
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("Nothing To Print");
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintFabricPlan_V1(int nID, int buid)
        {
            List<FabricPlanRepeat> oFabricPlanRepeats = new List<FabricPlanRepeat>();
            FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder();
            Fabric oFabric = new Fabric();
            oFabricPlanOrder = FabricPlanOrder.Gets("SELECT * FROM View_FabricPlanOrder WHERE FabricPlanOrderID =" + nID, (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
            if (oFabricPlanOrder.RefType == EnumFabricPlanRefType.Dispo)
            {
                _oFabricExecutionOrder = _oFabricExecutionOrder.Get(oFabricPlanOrder.RefID, (int)Session[SessionInfo.currentUserID]);
            }
            if (oFabricPlanOrder.RefType == EnumFabricPlanRefType.Fabric)
            {
                oFabric = oFabric.Get(oFabricPlanOrder.RefID, (int)Session[SessionInfo.currentUserID]);
                _oFabricExecutionOrder.ExeNo = "";
                _oFabricExecutionOrder.FabricNo = oFabric.FabricNo;
                _oFabricExecutionOrder.FabricWeave = oFabric.FabricWeave;
                _oFabricExecutionOrder.FabricWeaveName = oFabric.FabricWeaveName;
                _oFabricExecutionOrder.BuyerName = oFabric.BuyerName;
                _oFabricExecutionOrder.FabricWidth = oFabric.FabricWidth;
                _oFabricExecutionOrder.Construction = oFabric.Construction;
                _oFabricExecutionOrder.StyleNo = oFabric.StyleNo;
                oFabricPlanOrder.FabricDesignName = oFabric.FabricDesignName;


            }

            _oFabricPlans = FabricPlan.Gets("SELECT * FROM View_FabricPlan WHERE  FabricPlanOrderID =" + oFabricPlanOrder.FabricPlanOrderID, (int)Session[SessionInfo.currentUserID]);
            _oFabricPlanDetails = FabricPlanDetail.Gets("SELECT * FROM View_FabricPlanDetail where FabricPlanID in (" + string.Join(",", _oFabricPlans.Select(x => x.FabricPlanID).Distinct().ToList()) + ")", (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT * FROM View_FabricPlanRepeat WHERE  FabricPlanOrderID = " + oFabricPlanOrder.FabricPlanOrderID + " ORDER BY FabricPlanRepeatID";
            oFabricPlanRepeats = FabricPlanRepeat.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oFabricPlans.Any())
            {
                rptFabricPlan oReport = new rptFabricPlan();
                byte[] abytes = oReport.PrepareReport_V1(oFabricPlanOrder, _oFabricPlans, _oFabricPlanDetails, oFabricPlanRepeats, _oFabricExecutionOrder, oCompany, oBusinessUnit, "");
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

        public ActionResult ViewFabricPlanDetail(int nFabricPlanID)
        {
            List<FabricPlanDetail> oFabricPlanDetails = new List<FabricPlanDetail>();
            //oFabricPlanDetails = FabricPlanDetail.Gets((int)Session[SessionInfo.currentUserID]);
            return View(oFabricPlanDetails);
        }

        #endregion
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();

            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.FabricPattern, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.FabricPattern, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveSequence(List<FabricPlan> oFabricPlans)
        {
            try
            {
                if (oFabricPlans.Count > 0)
                {
                    oFabricPlans = FabricPlan.SaveSequence(oFabricPlans, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oFabricPlan = new FabricPlan();
                _oFabricPlan.ErrorMessage = ex.Message;
                oFabricPlans.Add(_oFabricPlan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Search
        [HttpPost]
        public JsonResult GetsRefNo(FabricPlanOrder oFabricPlanOrder)
        {
            List<FabricPlanOrder> oFabricPlanOrders = new List<FabricPlanOrder>();

            string sSql = "";

            try
            {
                if (oFabricPlanOrder.RefType == EnumFabricPlanRefType.Fabric)
                {
                    sSql = "select top(50) FPO.FabricPlanOrderID,FPO.RefID,FPO.RefType,FPO.ColumnCount,fabric.FabricNo as RefNo"
                    + " from FabricPlanOrder as FPO left join fabric on fabric.FabricID= FPO.RefID where RefType=" + (int)oFabricPlanOrder.RefType;
                    if (!string.IsNullOrEmpty(oFabricPlanOrder.RefNo))
                    {
                        sSql = sSql + " and fabric.FabricNo like '%" + oFabricPlanOrder.RefNo + "%'";
                    }
                }
                if (oFabricPlanOrder.RefType == EnumFabricPlanRefType.Dispo)
                {
                    sSql = "select top(50) FPO.FabricPlanOrderID,FPO.RefID,FPO.RefType,FPO.ColumnCount,FEO.FEONo as RefNo from FabricPlanOrder as FPO"
                    + " left join FabricExecutionOrder as FEO on FEO.FEOID= FPO.RefID where RefType=" + (int)oFabricPlanOrder.RefType;
                    if (string.IsNullOrEmpty(oFabricPlanOrder.RefNo))
                    {
                        sSql = sSql + " and FEO.FEONo like '%" + oFabricPlanOrder.RefNo + "%'";
                    }
                }
                sSql = sSql + "  order by FPO.DBServerDateTime desc";
                oFabricPlanOrders = FabricPlanOrder.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.ErrorMessage = ex.Message;
                oFabricPlanOrders.Add(oFabricPlanOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPlanOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsPlan(FabricPlanOrder oFabricPlanOrder)
        {
            List<FabricPlanRepeat> oFabricPlanRepeats = new List<FabricPlanRepeat>();
            try
            {
                oFabricPlanOrder = FabricPlanOrder.Gets("SELECT * FROM View_FabricPlanOrder WHERE FabricPlanOrderID =" + oFabricPlanOrder.FabricPlanOrderID, (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
                _oFabricPlans = FabricPlan.Gets("SELECT * FROM View_FabricPlan WHERE FabricPlanOrderID =" + oFabricPlanOrder.FabricPlanOrderID + " Order By WarpWeftType,SLNo ", (int)Session[SessionInfo.currentUserID]);
                oFabricPlanRepeats = FabricPlanRepeat.Gets("SELECT * FROM View_FabricPlanRepeat WHERE  FabricPlanOrderID = " + oFabricPlanOrder.FabricPlanOrderID + " ORDER BY FabricPlanRepeatID", ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFabricPlans.Count > 0)
                {
                    _oFabricPlanDetails = FabricPlanDetail.Gets("SELECT * FROM View_FabricPlanDetail where FabricPlanID in (" + string.Join(",", _oFabricPlans.Select(x => x.FabricPlanID).Distinct().ToList()) + ")", (int)Session[SessionInfo.currentUserID]);
                    _oFabricPlans.ForEach(x =>
                    {
                        if (_oFabricPlanDetails.FirstOrDefault() != null && _oFabricPlanDetails.Where(b => b.FabricPlanID == x.FabricPlanID).Count() > 0)
                        {
                            x.FabricPlanDetails = _oFabricPlanDetails.Where(p => p.FabricPlanID == x.FabricPlanID && p.FabricPlanID > 0).ToList();
                        }
                    });

                    if (_oFabricPlans.FirstOrDefault() != null && _oFabricPlans.FirstOrDefault().FabricPlanID > 0 && _oFabricPlans.Where(x => x.TwistedGroup > 0).Count() > 0)
                    {
                        List<FabricPlan> oFabricPlans = new List<FabricPlan>();
                        _oFabricPlans.ForEach((item) => { oFabricPlans.Add(item); });
                        _oFabricPlans = this.ComboFabricPlans(_oFabricPlans);
                        _oFabricPlans[0].CellRowSpans = this.RowMerge(oFabricPlans);
                    }

                    oFabricPlanOrder.FabricPlans = _oFabricPlans;
                    oFabricPlanOrder.FabricPlanRepeats = oFabricPlanRepeats;
                    oFabricPlanOrder.FabricPlanOrderID = 0; //It make zero for Create new Pattern after save
                }
            }
            catch (Exception ex)
            {
                oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.ErrorMessage = ex.Message;

            }
            var jsonResult = Json(oFabricPlanOrder, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oFabricPlanOrder);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Merger Row
        private List<CellRowSpan> RowMerge(List<FabricPlan> oFabricPlans)
        {

            List<CellRowSpan> oCellRowSpans = new List<CellRowSpan>();
            int[] rowIndex = new int[1];
            int[] rowSpan = new int[1];

            List<FabricPlan> oTWGLDDetails = new List<FabricPlan>();
            List<FabricPlan> oLDDetails = new List<FabricPlan>();
            List<FabricPlan> oTempLDDetails = new List<FabricPlan>();

            oTWGLDDetails = oFabricPlans.Where(x => x.TwistedGroup > 0).ToList();
            oLDDetails = oFabricPlans.Where(x => x.TwistedGroup == 0).ToList();

            while (oFabricPlans.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oFabricPlans.FirstOrDefault().FabricPlanID == oTWGLDDetails.FirstOrDefault().FabricPlanID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.TwistedGroup == oTWGLDDetails.FirstOrDefault().TwistedGroup).ToList();

                    oFabricPlans.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);
                    oTWGLDDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);

                }
                else if (oLDDetails.FirstOrDefault() != null && oFabricPlans.FirstOrDefault().FabricPlanID == oLDDetails.FirstOrDefault().FabricPlanID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.FabricPlanID == oLDDetails.FirstOrDefault().FabricPlanID).ToList();

                    oFabricPlans.RemoveAll(x => x.FabricPlanID == oTempLDDetails.FirstOrDefault().FabricPlanID);
                    oLDDetails.RemoveAll(x => x.FabricPlanID == oTempLDDetails.FirstOrDefault().FabricPlanID);
                }

                rowIndex[0] = rowIndex[0] + rowSpan[0];
                rowSpan[0] = oTempLDDetails.Count();
                oCellRowSpans.Add(MakeSpan.GenerateRowSpan("TwistedGroup", rowIndex[0], rowSpan[0]));
            }
            return oCellRowSpans;
        }
        private List<FabricPlan> ComboFabricPlans(List<FabricPlan> oFabricPlans)
        {
            List<FabricPlan> oTwistedLDDetails = new List<FabricPlan>();
            List<FabricPlan> oTWGLDDetails = new List<FabricPlan>();
            List<FabricPlan> oLDDetails = new List<FabricPlan>();
            List<FabricPlan> oTempLDDetails = new List<FabricPlan>();

            oTWGLDDetails = oFabricPlans.Where(x => x.TwistedGroup > 0).ToList();
            oLDDetails = oFabricPlans.Where(x => x.TwistedGroup == 0).ToList();

            while (oFabricPlans.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oFabricPlans.FirstOrDefault().FabricPlanID == oTWGLDDetails.FirstOrDefault().FabricPlanID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.TwistedGroup == oTWGLDDetails.FirstOrDefault().TwistedGroup).ToList();
                    oFabricPlans.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);
                    oTWGLDDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);

                }
                else if (oLDDetails.FirstOrDefault() != null && oFabricPlans.FirstOrDefault().FabricPlanID == oLDDetails.FirstOrDefault().FabricPlanID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.FabricPlanID == oLDDetails.FirstOrDefault().FabricPlanID).ToList();

                    oFabricPlans.RemoveAll(x => x.FabricPlanID == oTempLDDetails.FirstOrDefault().FabricPlanID);
                    oLDDetails.RemoveAll(x => x.FabricPlanID == oTempLDDetails.FirstOrDefault().FabricPlanID);
                }
                oTwistedLDDetails.AddRange(oTempLDDetails);
            }
            return oTwistedLDDetails;
        }
        [HttpPost]
        public JsonResult MakeCombo(FabricPlan oFabricPlan)
        {
            FabricPlan oFabricPlanRet = new FabricPlan();
            List<FabricPlan> oFabricPlans = new List<FabricPlan>();
            try
            {
                string sFabricPlanID = string.IsNullOrEmpty(oFabricPlan.ErrorMessage) ? "" : oFabricPlan.ErrorMessage;
                if (sFabricPlanID == "")
                    throw new Exception("No items found to make Group.");
                if (oFabricPlan.FabricPlanOrderID <= 0)
                    throw new Exception("No valid Order found.");

                oFabricPlans = FabricPlan.MakeCombo(sFabricPlanID, oFabricPlan.FabricPlanOrderID, oFabricPlan.TwistedGroup, (int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricPlans = oFabricPlans.Where(x => x.WarpWeftType == oFabricPlan.WarpWeftType).OrderBy(p => p.SLNo).ToList();
                if (oFabricPlans.FirstOrDefault() != null && oFabricPlans.FirstOrDefault().FabricPlanID > 0 && oFabricPlans.Where(x => x.TwistedGroup > 0).Count() > 0)
                {
                    List<FabricPlan> oTempFabricPlans = new List<FabricPlan>();
                    oFabricPlans.ForEach((item) => { oTempFabricPlans.Add(item); });
                    oFabricPlans = this.ComboFabricPlans(oFabricPlans);
                    oFabricPlans[0].CellRowSpans = this.RowMerge(oTempFabricPlans);
                }
                oFabricPlanRet.FabricPlans = oFabricPlans;

            }
            catch (Exception ex)
            {
                oFabricPlanRet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPlanRet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveComboGroup(FabricPlan oFabricPlan)
        {
            FabricPlan oFabricPlanRet = new FabricPlan();
            List<FabricPlan> oFabricPlans = new List<FabricPlan>();
            try
            {
                string sFabricPlanID = string.IsNullOrEmpty(oFabricPlan.ErrorMessage) ? "" : oFabricPlan.ErrorMessage;
                if (sFabricPlanID == "")
                    throw new Exception("No items found to make twisted.");
                if (oFabricPlan.FabricPlanOrderID <= 0)
                    throw new Exception("No valid labdip found.");

                oFabricPlans = FabricPlan.MakeCombo(sFabricPlanID, oFabricPlan.FabricPlanOrderID, oFabricPlan.TwistedGroup, (int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oFabricPlans = oFabricPlans.Where(x => x.WarpWeftType == oFabricPlan.WarpWeftType).OrderBy(p => p.SLNo).ToList();

                if (oFabricPlans.FirstOrDefault() != null && oFabricPlans.FirstOrDefault().FabricPlanID > 0 && oFabricPlans.Where(x => x.TwistedGroup > 0).Count() > 0)
                {
                    List<FabricPlan> oTempFabricPlans = new List<FabricPlan>();
                    oFabricPlans.ForEach((item) => { oTempFabricPlans.Add(item); });
                    oFabricPlans = this.ComboFabricPlans(oFabricPlans);
                    oFabricPlans[0].CellRowSpans = this.RowMerge(oTempFabricPlans);
                }
                oFabricPlanRet.FabricPlans = oFabricPlans;

            }
            catch (Exception ex)
            {
                oFabricPlanRet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricPlanRet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFabricPlanRepeat(FabricPlanRepeat oFabricPlanRepeat)
        {
            List<FabricPlanRepeat> oFabricPlanRepeats = new List<FabricPlanRepeat>();
            FabricPlanRepeat _oFabricPlanRepeat = new FabricPlanRepeat();
            try
            {
                string sSQL = "SELECT * FROM View_FabricPlanRepeat WHERE  FabricPlanOrderID = " + oFabricPlanRepeat.FabricPlanOrderID + " ORDER BY FabricPlanRepeatID";
                oFabricPlanRepeats = FabricPlanRepeat.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricPlanRepeats = new List<FabricPlanRepeat>();
                _oFabricPlanRepeat.ErrorMessage = ex.Message;

            }
            var jsonResult = Json(oFabricPlanRepeats, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult FabricPlanRepeatSave(FabricPlanRepeat oFabricPlanRepeat)
        {
            FabricPlanRepeat _oFabricPlanRepeat = new FabricPlanRepeat();
            try
            {
                _oFabricPlanRepeat = oFabricPlanRepeat;
                _oFabricPlanRepeat = _oFabricPlanRepeat.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricPlanRepeat = new FabricPlanRepeat();
                _oFabricPlanRepeat.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricPlanRepeat);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult FabricPlanRepeatDelete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricPlanRepeat _FabricPlanRepeat = new FabricPlanRepeat();
                sFeedBackMessage = _FabricPlanRepeat.Delete(id, (int)Session[SessionInfo.currentUserID]);

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
    }
}
