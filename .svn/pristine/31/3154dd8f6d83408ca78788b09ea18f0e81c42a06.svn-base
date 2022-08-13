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
    public class DispoHWController : Controller
    {
        #region Declaration

        DispoHW _oDispoHW = new DispoHW();
        List<DispoHW> _oDispoHWs = new List<DispoHW>();
        #endregion

        #region Functions
        #endregion

        #region Actions

        public ActionResult ViewDispoHW(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDispoHWs = new List<DispoHW>();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            return View(_oDispoHWs);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult GetsData(DispoHW objDispoHW)
        {
            List<DispoHW> oDispoHWList = new List<DispoHW>();
            DispoHW oDispoHW = new DispoHW();            
            string sSQL = GetSQL(objDispoHW);
            oDispoHWList = DispoHW.Gets(sSQL, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //var tuple = GenerateDispoHWs(objDispoHW);

            var jSonResult = Json(oDispoHWList, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        private string GetSQL(DispoHW oDispoHW)
        {
            string sReturn = "", sSQL = "";

            #region Order ID
            if (oDispoHW.FEOSID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FEOSID = " + oDispoHW.FEOSID;
            }
            #endregion

            if (!String.IsNullOrEmpty(oDispoHW.ErrorMessage))
            {
                 DateTime dReceivedDateStart =DateTime.Now;
                 DateTime dReceivedDateEnd = DateTime.Now; 
                int nCount = 0;
                string sDispoNo = oDispoHW.ErrorMessage.Split('~')[nCount++];
                int nDispoDate = Convert.ToInt16(oDispoHW.ErrorMessage.Split('~')[nCount++]);
                DateTime dDispoDateStart = Convert.ToDateTime(oDispoHW.ErrorMessage.Split('~')[nCount++]);
                DateTime dDispoDateEnd = Convert.ToDateTime(oDispoHW.ErrorMessage.Split('~')[nCount++]);

                int nReadyDate = Convert.ToInt16(oDispoHW.ErrorMessage.Split('~')[nCount++]);
                DateTime dReadyDateStart = Convert.ToDateTime(oDispoHW.ErrorMessage.Split('~')[nCount++]);
                DateTime dReadyDateEnd = Convert.ToDateTime(oDispoHW.ErrorMessage.Split('~')[nCount++]);

                string sBuyerIDs = oDispoHW.ErrorMessage.Split('~')[nCount++];
                string sCustomerIDs = oDispoHW.ErrorMessage.Split('~')[nCount++];

                int nReceivedDate = Convert.ToInt16(oDispoHW.ErrorMessage.Split('~')[nCount++]);
                if (nReceivedDate > 0)
                {
                     dReceivedDateStart = Convert.ToDateTime(oDispoHW.ErrorMessage.Split('~')[nCount++]);
                     dReceivedDateEnd = Convert.ToDateTime(oDispoHW.ErrorMessage.Split('~')[nCount++]);
                }

                sReturn = "";
                //sSQL = "SELECT * FROM View_DispoHW ";

                #region Dispo No
                if (!string.IsNullOrEmpty(sDispoNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FSCDID IN (SELECT FabricSalescontractDetailID FROM FabricSalescontractDetail WHERE ExeNo LIKE '%" + sDispoNo + "%') ";
                }
                #endregion

                #region Dispo Date
                if (nDispoDate != (int)EnumCompareOperator.None)
                {
                    if (nDispoDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDispoDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDispoDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDispoDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDispoDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDispoDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDispoDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDispoDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDispoDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDispoDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDispoDateEnd.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nDispoDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDispoDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dDispoDateEnd.ToString("dd MMM yyyy") + "', 106))";
                    }
                }
                #endregion
                #region ReadyDate
     
                    if (nReadyDate > 0)
                    {
                        Global.TagSQL(ref sReturn);

                        if (nReadyDate == (int)EnumCompareOperator.EqualTo)
                        {
                            // sReturn = sReturn + " EB.ExportBillID IN (SELECT EBH.ExportBillID FROM View_ExportBillHistory AS EBH WHERE EBH.State= " + oExportBill.StateDateType + " AND EBH.DBServerDateTime = '" + oExportBill.StartDateState.ToString("dd MMM yyyy") + "') ";
                            sReturn = sReturn + " FEOSID IN (Select FEOSID from FabricBeamFinish where ReadyDate >= '" + dReadyDateStart.ToString("dd MMM yyyy") + "' AND ReadyDate<'" + dReadyDateStart.AddDays(1).ToString("dd MMM yyyy") + "') ";

                        }
                        else if (nReadyDate == (int)EnumCompareOperator.NotEqualTo)
                        {
                            sReturn = sReturn + "FEOSID IN (Select FEOSID from FabricBeamFinish where ReadyDate != '" + dReadyDateStart.ToString("dd MMM yyyy") + "') ";
                        }
                        else if (nReadyDate == (int)EnumCompareOperator.GreaterThan)
                        {
                            sReturn = sReturn + " FEOSID IN (Select FEOSID from FabricBeamFinish where ReadyDate > '" + dReadyDateStart.ToString("dd MMM yyyy") + "') ";
                        }
                        else if (nReadyDate == (int)EnumCompareOperator.SmallerThan)
                        {
                            sReturn = sReturn + " FEOSID IN (Select FEOSID from FabricBeamFinish where ReadyDate < '" + dReadyDateStart.ToString("dd MMM yyyy") + "') ";
                        }
                        else if (nReadyDate == (int)EnumCompareOperator.Between)
                        {
                            sReturn = sReturn + " FEOSID IN (Select FEOSID from FabricBeamFinish where ReadyDate >= '" + dReadyDateStart.ToString("dd MMM yyyy") + "' AND  ReadyDate<'" + dReadyDateEnd.ToString("dd MMM yyyy") + "') ";
                        }
                        else if (nReadyDate == (int)EnumCompareOperator.NotBetween)
                        {
                            sReturn = sReturn + " FEOSID IN (Select FEOSID from FabricBeamFinish where ReadyDate NOT BETWEEN '" + dReadyDateStart.ToString("dd MMM yyyy") + "' AND '" + dReadyDateEnd.ToString("dd MMM yyyy") + "') ";
                        }
                    }
             
                #endregion

               #region Received Date
                    if (nReceivedDate > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        if (nReceivedDate == (int)EnumCompareOperator.EqualTo)
                        {
                            //sReturn = sReturn + " FEOSID IN (Select FEOSID from FabricBeamFinish where ReadyDate >= '" + dReceivedDateStart.ToString("dd MMM yyyy") + "' AND ReadyDate<'" + dReceivedDateStart.AddDays(1).ToString("dd MMM yyyy") + "') ";
                            sReturn = sReturn + " FEOSID IN (Select FEOSID from DyeingOrderFabricDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DUHardWinding where ReceiveDate>'" + dReceivedDateStart.ToString("dd MMM yyyy") + "' AND ReceiveDate<'" + dReceivedDateStart.AddDays(1).ToString("dd MMM yyyy") + "')) ";

                        }
                        else if (nReceivedDate == (int)EnumCompareOperator.NotEqualTo)
                        {
                            sReturn = sReturn + " FEOSID IN (Select FEOSID from DyeingOrderFabricDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DUHardWinding where ReceiveDate!='" + dReceivedDateStart.ToString("dd MMM yyyy") + "')) ";
                        }
                        else if (nReceivedDate == (int)EnumCompareOperator.GreaterThan)
                        {
                            sReturn = sReturn + " FEOSID IN (Select FEOSID from DyeingOrderFabricDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DUHardWinding where ReceiveDate>'" + dReceivedDateStart.ToString("dd MMM yyyy") + "')) ";
                        }
                        else if (nReceivedDate == (int)EnumCompareOperator.SmallerThan)
                        {
                            sReturn = sReturn + " FEOSID IN (Select FEOSID from DyeingOrderFabricDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DUHardWinding where ReceiveDate<'" + dReceivedDateStart.ToString("dd MMM yyyy") + "')) ";
                        }
                        else if (nReceivedDate == (int)EnumCompareOperator.Between)
                        {
                            sReturn = sReturn + " FEOSID IN (Select FEOSID from DyeingOrderFabricDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DUHardWinding where ReceiveDate>'" + dReceivedDateStart.ToString("dd MMM yyyy") + "' AND ReceiveDate<'" + dReceivedDateEnd.AddDays(1).ToString("dd MMM yyyy") + "')) ";
                        }
                        else if (nReceivedDate == (int)EnumCompareOperator.NotBetween)
                        {
                            sReturn = sReturn + " FEOSID IN (Select FEOSID from DyeingOrderFabricDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DUHardWinding where ReceiveDate NOT BETWEEN'" + dReceivedDateStart.ToString("dd MMM yyyy") + "' AND '" + dReceivedDateEnd.AddDays(1).ToString("dd MMM yyyy") + "')) ";
                        }
                    }

                    #endregion
                    
                #region Buyer
                if (!string.IsNullOrEmpty(sBuyerIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FSCDID IN (SELECT FabricSalescontractDetailID FROM FabricSalescontractDetail WHERE FabricSalescontractID IN (SELECT FabricSalescontractID FROM FabricSalescontract WHERE BuyerID IN (" + sBuyerIDs + "))) ";
                }
                #endregion

                #region Customer
                if (!string.IsNullOrEmpty(sCustomerIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FSCDID IN (SELECT FabricSalescontractDetailID FROM FabricSalescontractDetail WHERE FabricSalescontractID IN (SELECT FabricSalescontractID FROM FabricSalescontract WHERE ContractorID IN (" + sCustomerIDs + "))) ";
                }
                #endregion

                //sSQL += sWhereCluse;
            }
            return sReturn;
        }

        private Tuple<List<DispoHW>, List<CellRowSpan>> GenerateDispoHWs(DispoHW oDispoHW)
        {
            Tuple<List<DispoHW>, List<CellRowSpan>> tuple = new Tuple<List<DispoHW>, List<CellRowSpan>>(new List<DispoHW>(), new List<CellRowSpan>());
            List<DispoHW> oDHWs = new List<DispoHW>();
            try
            {
                string sSQL = GetSQL(oDispoHW);
                oDHWs = DispoHW.Gets(sSQL, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDHWs = oDHWs.OrderBy(x => x.FEOSID).ToList();

                var oTFDPs = new List<DispoHW>();
                oDHWs.ForEach(x => oTFDPs.Add(x));

                var oCellRowSpans = GenerateSpan(oTFDPs);
                tuple = new Tuple<List<DispoHW>, List<CellRowSpan>>(oDHWs, oCellRowSpans);
            }
            catch (Exception ex)
            {
                tuple = new Tuple<List<DispoHW>, List<CellRowSpan>>(new List<DispoHW>(), new List<CellRowSpan>());
            }

            return tuple;
        }

        private List<CellRowSpan> GenerateSpan(List<DispoHW> oFDPs)
        {
            var oTFDPs = new List<DispoHW>();
            List<CellRowSpan> oCellRowSpans = new List<CellRowSpan>();
            int[,] mergerCell2D = new int[1, 2];
            int[] rowIndex = new int[15];
            int[] rowSpan = new int[15];

            while (oFDPs.Count() > 0)
            {
                oTFDPs = oFDPs.Where(x => x.FEOSID == oFDPs.FirstOrDefault().FEOSID).ToList();
                oFDPs.RemoveAll(x => x.FEOSID == oTFDPs.FirstOrDefault().FEOSID);

                rowIndex[0] = rowIndex[0] + rowSpan[0]; //
                rowSpan[0] = oTFDPs.Count(); //
                oCellRowSpans.Add(MakeSpan.GenerateRowSpan("Span", rowIndex[0], rowSpan[0]));
            }
            return oCellRowSpans;
        }

        public JsonResult GetsFabricBeamFinish(int nFEOSID)
        {
            List<FabricBeamFinish> _oFabricBeamFinishs = new List<FabricBeamFinish>();
            try
            {
                string Ssql = "SELECT * FROM View_FabricBeamFinish WHERE FEOSID=" + nFEOSID + " ";
                _oFabricBeamFinishs = new List<FabricBeamFinish>();
                _oFabricBeamFinishs = FabricBeamFinish.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricBeamFinishs = new List<FabricBeamFinish>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBeamFinishs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region print
        //[HttpPost]
        //public ActionResult SetDispoHWListData(DispoHW oDispoHW)
        //{
        //    this.Session.Remove(SessionInfo.ParamObj);
        //    this.Session.Add(SessionInfo.ParamObj, oDispoHW);

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(Global.SessionParamSetMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult PrintDispoHWs()
        //{
        //    _oDispoHW = new DispoHW();
        //    try
        //    {
        //        _oDispoHW = (DispoHW)Session[SessionInfo.ParamObj];
        //        string sSQL = "SELECT * FROM View_DispoHW WHERE DispoHWID IN (" + _oDispoHW.ErrorMessage + ") Order By DispoHWID";
        //        _oDispoHWs = DispoHW.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oDispoHW = new DispoHW();
        //        _oDispoHWs = new List<DispoHW>();
        //    }
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //_oDispoHW.Company = oCompany;

        //    rptDispoHWs oReport = new rptDispoHWs();
        //    byte[] abytes = oReport.PrepareReport(_oDispoHWs, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        //public ActionResult DispoHWPrintPreview(int id)
        //{
        //    _oDispoHW = new DispoHW();
        //    Company oCompany = new Company();
        //    Contractor oContractor = new Contractor();
        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    if (id > 0)
        //    {
        //        _oDispoHW = _oDispoHW.Get(id, (int)Session[SessionInfo.currentUserID]);
        //        _oDispoHW.DispoHWDetails = DispoHWDetail.Gets(_oDispoHW.DispoHWID, (int)Session[SessionInfo.currentUserID]);
        //        //_oDispoHW.BusinessUnit = oBusinessUnit.Get(_oDispoHW.BUID, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    //else
        //    //{
        //    //    _oDispoHW.BusinessUnit = new BusinessUnit();
        //    //}
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //_oDispoHW.Company = oCompany;
        //    byte[] abytes;
        //    rptDispoHW oReport = new rptDispoHW();
        //    abytes = oReport.PrepareReport(_oDispoHW, oCompany);
        //    return File(abytes, "application/pdf");
        //}
        //public Image GetCompanyLogo(Company oCompany)
        //{
        //    if (oCompany.OrganizationLogo != null)
        //    {
        //        string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
        //        if (System.IO.File.Exists(fileDirectory))
        //        {
        //            System.IO.File.Delete(fileDirectory);
        //        }

        //        MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
        //        System.Drawing.Image img = System.Drawing.Image.FromStream(m);
        //        img.Save(fileDirectory, ImageFormat.Jpeg);
        //        return img;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        #endregion

        #region  Fabric beam finish
        [HttpPost]
        public JsonResult SaveFabricBeamFinish(FabricBeamFinish oFabricBeamFinish)
        {
            FabricBeamFinish _oFabricBeamFinish = new FabricBeamFinish();
            try
            {
                _oFabricBeamFinish = oFabricBeamFinish.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricBeamFinish = new FabricBeamFinish();
                _oFabricBeamFinish.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricBeamFinish);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteFabricBeamFinish(FabricBeamFinish oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricBeamFinish oFabricBeamFinish = new FabricBeamFinish();
                sFeedBackMessage = oFabricBeamFinish.Delete(oJB.FabricBeamFinishID, (int)Session[SessionInfo.currentUserID]);
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
