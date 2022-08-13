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
    public class DUDeliveryOrderDCController : Controller
    {
        #region Declaration
        DUDeliveryOrderDC _oDUDeliveryOrderDC = new DUDeliveryOrderDC();
        List<DUDeliveryOrderDC> _oDUDeliveryOrderDCs = new List<DUDeliveryOrderDC>();
       
        string _sErrorMessage = "";
        #endregion

        #region DUDeliveryOrderDC
        public ActionResult ViewDUDeliveryOrderDCs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDUDeliveryOrderDCs = new List<DUDeliveryOrderDC>();
            _oDUDeliveryOrderDCs = DUDeliveryOrderDC.Gets("Select Top(500)* from View_DUDeliveryOrderDC where  Qty-0.5>Isnull(Qty_DC,0) and isnull(ApproveBy,0)!=0 order by DODate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oDUDeliveryOrderDCs);
        }

        public ActionResult ViewDUDeliveryChallanDO(int nId, double ts)
        {
             _oDUDeliveryOrderDC = new DUDeliveryOrderDC();
             _oDUDeliveryOrderDC = _oDUDeliveryOrderDC.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

            DUDeliveryChallan oDUDeliveryChallan = new DUDeliveryChallan();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            string sOrderNo = "";
            DyeingOrder oDyeingOrder = new DyeingOrder();
            if (nId > 0)
            {
                oDUDeliveryChallan = DUDeliveryChallan.GetbyDO(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDUDeliveryChallan.DUDeliveryChallanID > 0)
                {
                    oDUDeliveryChallan.DUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    foreach (DUDeliveryChallanDetail oItem in oDUDeliveryChallan.DUDeliveryChallanDetails)
                    {
                        if (sOrderNo != oItem.OrderNo)
                        {
                            oDUDeliveryChallan.OrderNos = oDUDeliveryChallan.OrderNos = oItem.OrderNo;
                        }
                        sOrderNo = oItem.OrderNo;
                    }
                }
                else
                {
                    oDUDeliveryChallan.ContractorID = _oDUDeliveryOrderDC.ContractorID;
                    oDUDeliveryChallan.ContractorName = _oDUDeliveryOrderDC.DeliveryToName;
                    oDUDeliveryChallan.OrderNos = _oDUDeliveryOrderDC.OrderNo;
                    oDUDeliveryChallan.OrderType = _oDUDeliveryOrderDC.OrderType;
                    oDUDeliveryChallan.DONos = _oDUDeliveryOrderDC.DONo;
                    oDUDeliveryChallan.LCNo = _oDUDeliveryOrderDC.ExportLCNo;
                    oDUDeliveryChallan.DOQty = _oDUDeliveryOrderDC.Qty;

                }


            }
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;
            ViewBag.WorkingUnits = WorkingUnit.Gets("SELECT * FROM View_WorkingUnit where WorkingUnitID=8", ((User)Session[SessionInfo.CurrentUser]).UserID);
            // Pls Change It 


            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oDUDeliveryOrderDC.DUDeliveryChallan = oDUDeliveryChallan;
            return View(_oDUDeliveryOrderDC);
        }
      

        #region Search
        [HttpPost]
        public JsonResult AdvSearch(DUDeliveryOrderDC oDUDeliveryOrderDC)
        {
            _oDUDeliveryOrderDCs = new List<DUDeliveryOrderDC>();
            try
            {
                string sSQL = MakeSQL(oDUDeliveryOrderDC);
                _oDUDeliveryOrderDCs = DUDeliveryOrderDC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryOrderDCs = new List<DUDeliveryOrderDC>();
                //_oDUDeliveryOrderDC.ErrorMessage = ex.Message;
                //_oDUDeliveryOrderDCs.Add(_oDUDeliveryOrderDC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrderDCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(DUDeliveryOrderDC oDUDeliveryOrderDC)
        {
            string sParams = oDUDeliveryOrderDC.Note;

            int nCboDODate = 0;
            DateTime dFromDODate = DateTime.Today;
            DateTime dToDODate = DateTime.Today;
            int nCboInvoiceDate = 0;
            DateTime dFromInvoiceDate = DateTime.Today;
            DateTime dToInvoiceDate = DateTime.Today;
            int nCboDeliveryDate = 0;
            DateTime dFromDeliveryDate = DateTime.Today;
            DateTime dToDeliveryDate = DateTime.Today;

            int nCboMkPerson = 0;
            int nPaymentType = 0;
            int nOrderType = 0;

            string sProductIDs = "";

            string sPINo = "";
            string sInvoiceNo = "";
            string sPONo = "";
            string sRefNo = "";

            bool bYetNotSendLabReq = false;
            bool bYetNotSendPro = false;
            int nOrderTypeFixec = 0;
            int nBUID = 0;



            if (!string.IsNullOrEmpty(sParams))
            {
                string sTemp = "";
                _oDUDeliveryOrderDC.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                _oDUDeliveryOrderDC.DeliveryToName = Convert.ToString(sParams.Split('~')[1]);

                nCboDODate = Convert.ToInt32(sParams.Split('~')[2]);
                dFromDODate = Convert.ToDateTime(sParams.Split('~')[3]);
                dToDODate = Convert.ToDateTime(sParams.Split('~')[4]);
                //nCboInvoiceDate = Convert.ToInt32(sParams.Split('~')[5]);
                //dFromInvoiceDate = Convert.ToDateTime(sParams.Split('~')[6]);
                //dToInvoiceDate = Convert.ToDateTime(sParams.Split('~')[7]);
                nCboDeliveryDate = Convert.ToInt32(sParams.Split('~')[8]);
                sTemp = Convert.ToString(sParams.Split('~')[9]);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    dFromDeliveryDate = Convert.ToDateTime(sParams.Split('~')[9]);
                    dToDeliveryDate = Convert.ToDateTime(sParams.Split('~')[10]);
                }

                nCboMkPerson = Convert.ToInt32(sParams.Split('~')[11]);
                nPaymentType = Convert.ToInt32(sParams.Split('~')[12]);
                nOrderType = Convert.ToInt32(sParams.Split('~')[13]);

                sPINo = Convert.ToString(sParams.Split('~')[14]);
                sInvoiceNo = Convert.ToString(sParams.Split('~')[15]);
                sPONo = Convert.ToString(sParams.Split('~')[16]);
                sRefNo = Convert.ToString(sParams.Split('~')[17]);

                sProductIDs = Convert.ToString(sParams.Split('~')[18]);

                bYetNotSendLabReq = Convert.ToBoolean(sParams.Split('~')[19]);
                bYetNotSendPro = Convert.ToBoolean(sParams.Split('~')[20]);

                nOrderTypeFixec = Convert.ToInt16(sParams.Split('~')[21]);


                nBUID = Convert.ToInt32(sParams.Split('~')[22]);
            }


            string sReturn1 = "SELECT * FROM View_DUDeliveryOrderDC AS DO ";
            string sReturn = "";

            #region Contractor
            if (!String.IsNullOrEmpty(_oDUDeliveryOrderDC.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DO.ContractorID in(" + _oDUDeliveryOrderDC.ContractorName + ")";
            }
            #endregion

            #region Buyer Id
            if (!String.IsNullOrEmpty(_oDUDeliveryOrderDC.DeliveryToName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DO.ContractorID in(" + _oDUDeliveryOrderDC.DeliveryToName + ")";
            }
            #endregion

            #region DODate Date
            if (nCboDODate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboDODate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDODate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDODate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDODate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDODate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDODate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.DODate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDODate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Delivery Date
            if (nCboDeliveryDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

                if (nCboDeliveryDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + "DUDeliveryOrderDCID in (Select DUDeliveryOrderDCID from DUDeliveryOrderDCDetail where  CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106)) )";
                }

                else if (nCboDeliveryDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + "DUDeliveryOrderDCID in (Select DUDeliveryOrderDCID from DUDeliveryOrderDCDetail where CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + "DUDeliveryOrderDCID in (Select DUDeliveryOrderDCID from DUDeliveryOrderDCDetail where CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }

                else if (nCboDeliveryDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + "DUDeliveryOrderDCID in (Select DUDeliveryOrderDCID from DUDeliveryOrderDCDetail where CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "DUDeliveryOrderDCID in (Select DUDeliveryOrderDCID from DUDeliveryOrderDCDetail where  CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + "DUDeliveryOrderDCID in (Select DUDeliveryOrderDCID from DUDeliveryOrderDCDetail where  CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }

            }
            #endregion

            #region nOrderTypeFixec
            /// This Searching Critria for Different Window Bulk and Sample
            if (nOrderTypeFixec > 0)
            {
                if (nOrderTypeFixec == (int)EnumOrderType.BulkOrder)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DO.OrderType in (3)";
                }
                if (nOrderTypeFixec == (int)EnumOrderType.SampleOrder)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DO.OrderType in (2)";
                }
            }
            #endregion

            //#region nPayment Type
            //if (nPaymentType > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " DO.PaymentType = " + nPaymentType;
            //}
            //#endregion
            //#region Mkt. Person
            //if (nCboMkPerson > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " DO.MKTEmpID = " + nCboMkPerson;
            //}
            //#endregion
            //#region Order Type
            //if (nOrderType > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " DO.DUDeliveryOrderDCType = " + nOrderType;
            //}
        #endregion



            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DUDeliveryOrderDCID in (Select DOD.DUDeliveryOrderDCID from DUDeliveryOrderDCDetail as DOD where ProductID in (" + sProductIDs + "))";
            }
            #endregion

            //#region Po No
            //if (!string.IsNullOrEmpty(sPONo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "(OrderType=3 and OrderID in  (Select ExportSC.ExportSCID from ExportSC where ExportPIID in (Select DyeingOrder.ExportPIID from DyeingOrder where OrderNo Like  '%" + sPONo + "%'))) or (OrderType=2 and OrderID in (Select DyeingOrder.ExportPIID from DyeingOrder where OrderNo Like  '%" + sPONo + "%'))";
            //}
            //#endregion
            #region Po No
            if (!string.IsNullOrEmpty(sPONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType=" + (int)EnumOrderType.SampleOrder + " and OrderID  in (Select DyeingOrder.DyeingOrderID from DyeingOrder where OrderNo Like  '%" + sPONo + "%')";
                // sReturn = sReturn + "(OrderType=" + (int)EnumOrderType.SampleOrder + " and OrderID  in (Select DyeingOrder.DyeingOrderID from DyeingOrder where OrderNo Like  '%" + sPONo + "%'))) or (OrderType=2 and OrderID in (Select DyeingOrder.ExportPIID from DyeingOrder where OrderNo Like  '%" + sPONo + "%'))";
            }
            #endregion


            #region Invoice No
            if (!string.IsNullOrEmpty(sInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType=2 and OrderID in (Select DyeingOrder.DyeingOrderID from DyeingOrder where DyeingOrder.SampleInvoiceID in (Select sampleInvoice.SampleInvoiceID from sampleInvoice where sampleInvoiceNo like '%0%' ))";
            }
            #endregion

            #region P/I  No
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType=3 and OrderID in  (Select ExportSC.ExportSCID from ExportSC where ExportPIID in (Select ExportPI.ExportPIID from ExportPI where PINO Like '%" + sPINo + "%'))";
            }
            #endregion

            #region Business Unit
            //if (nBUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " DO.BUID = " + nBUID;
            //}
            #endregion
            string sSQL = sReturn1 + " " + sReturn + " order by OrderNo";
            return sSQL;
        }
        [HttpPost]
        public JsonResult GetbyNo(DUDeliveryOrderDC oDUDeliveryOrderDC)
        {
            _oDUDeliveryOrderDCs = new List<DUDeliveryOrderDC>();

            try
            {
                string sSQL = "SELECT * FROM View_DUDeliveryOrderDC ";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oDUDeliveryOrderDC.DONo))
                {
                    oDUDeliveryOrderDC.DONo = oDUDeliveryOrderDC.DONo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DONo like '%" + oDUDeliveryOrderDC.DONo + "%'";
                }
                if (!String.IsNullOrEmpty(oDUDeliveryOrderDC.OrderNo))
                {
                    oDUDeliveryOrderDC.OrderNo = oDUDeliveryOrderDC.OrderNo.Trim();
                    Global.TagSQL(ref sReturn, "OrderNo", oDUDeliveryOrderDC.OrderNo);
                }

                if (oDUDeliveryOrderDC.OrderType > 0)
                {

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderType=" + oDUDeliveryOrderDC.OrderType;
                }

                sSQL = sSQL + "" + sReturn;
                _oDUDeliveryOrderDCs = DUDeliveryOrderDC.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryOrderDCs = new List<DUDeliveryOrderDC>();
                //_oDUDeliveryOrderDC.ErrorMessage = ex.Message;
                //_oDUDeliveryOrderDCs.Add(_oDUDeliveryOrderDC);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrderDCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Logo No
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