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
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net.Mail;

namespace ESimSolFinancial.Controllers
{
    public class FabricSCReportController : Controller
    {
        #region Declaration
        FabricSCReport _oFabricSCReport = new FabricSCReport();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<FabricSCReport> _oFabricSCReports = new List<FabricSCReport>();
        string _sDateRange = "";
        #endregion

        public ActionResult View_FabricSCReports(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));

            ViewBag.OrderTypes = oFabricOrderSetups;// EnumObject.jGets(typeof(EnumFabricRequestType));
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus));
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricPO).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            ViewBag.FabricPOSetup = oFabricPOSetup;
            _oFabricSCReport.BUID = buid;
            var tuple = GenerateFabricSCReports(_oFabricSCReport);
            _oFabricSCReports = tuple.Item1;
            ViewBag.CellRowSpans = tuple.Item2;
            return View(_oFabricSCReports);
        }
        public ActionResult View_FabricSCReports_DC(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType));
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus));
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricPO).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            ViewBag.FabricPOSetup = oFabricPOSetup;
            var tuple = GenerateFabricSCReports(_oFabricSCReport);
            _oFabricSCReports = tuple.Item1;
            ViewBag.CellRowSpans = tuple.Item2;
            return View(_oFabricSCReports);
        }
        public ActionResult View_FabricSCReports_Order(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricPO).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType));
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus));

            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            return View(_oFabricSCReports);
        }
        public ActionResult ViewFabric(int id, int buid)
        {
            Fabric oFabric = new Fabric();
            FabricRnD oFabricRnD = new FabricRnD();

            if (id > 0)
            {
                _oFabricSCReport = _oFabricSCReport.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
         
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            List<Product> oProducts = new List<Product>();
            oProducts = Product.GetsPermittedProduct(buid, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.CompareOperators = Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.SwatchTypes = EnumObject.jGets(typeof(EnumSwatchType)); //SwatchTypesObj.Gets();            
            //ViewBag.MktPersons = oMarketingAccounts;
            ViewBag.Products = oProducts;
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.BUID = buid;
            ViewBag.CurrentUserID = ((User)Session[SessionInfo.CurrentUser]).UserID;
            ViewBag.FabricOrderSetups = oFabricOrderSetups;

            List<LightSource> oLSs = new List<LightSource>();
            oLSs = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabric = oFabric.Get(_oFabricSCReport.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabricRnD = oFabricRnD.GetBy(_oFabricSCReport.FabricSalesContractDetailID,_oFabricSCReport.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Fabric = oFabric;
            ViewBag.FabricRnD = oFabricRnD;
            ViewBag.ShadeTypes = EnumObject.jGets(typeof(EnumFabricRndShade));
            //ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oFabricSCReport);
        }

        public ActionResult ViewFabricMail(string ids, int buid)
        {
            Fabric oFabric = new Fabric();
            FabricRnD oFabricRnD = new FabricRnD();
            MailSetUp oMailSetUp = new MailSetUp();

            MarketingAccount oMarketingAccount = new MarketingAccount();
            List<MarketingAccount> oMarketingGroups = new List<MarketingAccount>();

            _oFabricSCReports = new List<FabricSCReport>();
            List<FabricRnD> oFabricRnDs = new List<FabricRnD>();

            List<User> oUsers = new List<User>();
            List<string> oMailCollections = new List<string>();

            if (ids.Length > 0)
            {
                try
                {
                    _oFabricSCReports = FabricSCReport.Gets("SELECT * FROM View_FabricSalesContractReport WHERE FabricSalesContractDetailID IN (" + ids + ")", (int)Session[SessionInfo.currentUserID]);
                    oFabricRnDs = FabricRnD.Gets("SELECT * FROM View_FabricRnD WHERE FSCDID IN ("+ids+")", (int)Session[SessionInfo.currentUserID]);

                    oMailSetUp = MailSetUp.GetByModule((int)EnumModuleName.FabricSalesContract, (int)Session[SessionInfo.currentUserID]);
                    oMailSetUp.CCMails = MailAssignedPerson.Gets("SELECT * FROM MailAssignedPerson WHERE MSID=" + oMailSetUp.MSID, (int)Session[SessionInfo.currentUserID]);

                    oMarketingAccount = oMarketingAccount.Get(_oFabricSCReports.Select(x=>x.MktAccountID).FirstOrDefault(), (int)Session[SessionInfo.currentUserID]);
                    oMarketingGroups = MarketingAccount.Gets("SELECT * FROM MarketingAccount WHERE GroupID IN (SELECT GroupID FROM MarketingAccount WHERE MarketingAccountID = " + _oFabricSCReports.Select(x => x.MktAccountID).FirstOrDefault() + ") AND ISNULL(IsGroup,0)=0", (int)Session[SessionInfo.currentUserID]);

                    oUsers = ESimSol.BusinessObjects.User.GetsBySql("SELECT * FROM Users WHERE Users.UserID IN (" + string.Join(",", _oFabricSCReports.Select(x => x.PreapeByID).Distinct()) + ")", (int)Session[SessionInfo.currentUserID]);
                    
                    #region MAIL SETUP
                    
                    var oMailTo = oMailSetUp.CCMails.Where(x => x.IsCCMail == false).FirstOrDefault();
                    var oMailCCs = oMailSetUp.CCMails.Where(x => x.IsCCMail).ToList();

                    #region MAIL TO
                    List<string> emialTos = new List<string>();
                    emialTos.Add(oMailTo.MailTo);

                    var oUSs = oUsers.Where(x => !string.IsNullOrEmpty(x.EmailAddress) && x.EmailAddress.Contains("@"));
                    if (oUSs != null)
                    {
                        emialTos.AddRange(oUSs.Select(x => x.EmailAddress).Distinct());
                    }

                    emialTos.AddRange(oUSs.Select(x => x.EmailAddress).Distinct());
                    //emialTos.AddRange( oMarketingGroups.Where(x => !string.IsNullOrEmpty(x.Email) && x.Email.Contains("@")).Select(x=>x.Email) );
                    #endregion

                    #region MAIL TO CC
                    List<string> ccMailTos = new List<string>();
                    ccMailTos.AddRange(oMailCCs.Select(x => x.MailTo));

                    var oMKTPs = oMarketingGroups.Where(x => (!string.IsNullOrEmpty(x.Email) && x.Email.Contains("@")) && x.IsGroupHead);
                    if(oMKTPs!=null)
                    {
                        ccMailTos.AddRange(oMKTPs.Select(x => x.Email).Distinct());
                    }
                    #endregion

                    string subject = "PO Sent From Lab :" + String.Join(", ",_oFabricSCReports.Select(x=>x.SCNoFull));

                    /*
                        string bodyInfo = "<div class='col-md-12'>";
                        bodyInfo += "<div style='padding-top:10px;font-size:13px'>Dear Sir, </div> <br>";
                        bodyInfo += "<div style='padding-top:10px;font-size:13px'>Please see the analysis report of Customer: "
                                 +  "<label style='font-weight:bold; font-size:13px'>" + _oFabricSCReports.Select(x => x.BuyerName).FirstOrDefault() + " (" + _oFabricSCReports.Select(x => x.SCNoFull).FirstOrDefault() + ") </label> </div> ";
                    */

                    string bodyInfo = "<div class='col-md-12'> <br><br>";
                    bodyInfo += "<div style='font-size:13px'>Dear Sir, </div> <br>";
                    bodyInfo += "<div style='font-size:13px'>Please see the analysis report of Customer: "
                             + "<label style='font-weight:bold; font-size:13px'>" + _oFabricSCReports.Select(x => x.BuyerName).FirstOrDefault() + " (" + _oFabricSCReports.Select(x => x.SCNoFull).FirstOrDefault() + ") </label> </div> ";
                    string prvMails = "";

                    foreach(var oItem in _oFabricSCReports)
                    {
                        var oRND = oFabricRnDs.Where(x=>x.FSCDID == oItem.FabricSalesContractDetailID).FirstOrDefault();

                        if(oRND==null)oRND = new FabricRnD();

                        if (oRND.ForwardBy != 0) prvMails += oItem.SCNoFull + " [" + oItem.FabricNo + "]  was Sent by " + oRND.ForwardByName + " [" + oRND.ForwardDate.ToString("dd MMM yyyy") + "]" + Environment.NewLine;

                        oRND.ProductNameWarp = "• Actual Composition: " + oRND.ProductNameWarp + (string.IsNullOrEmpty(oRND.ProductNameWeft) ? "" : ", " + oRND.ProductNameWeft);
                        //oRND.ProductNameWarpSuggest = "Suggested Composition: " + oRND.ProductNameWarpSuggest + (string.IsNullOrEmpty(oRND.ProductNameWeftSuggest) ? "" : ", " + oRND.ProductNameWeftSuggest);
                        oRND.ProductWarpRnd_Suggest = "Suggested Composition: " + oRND.ProductWarpRnd_Suggest;
                        oRND.FabricWeaveName = "Actual Weave: " + oRND.FabricWeaveName;
                        oRND.FabricWeaveNameSuggest = "Suggested Weave: " + oRND.FabricWeaveNameSuggest;
                        oRND.WarpCount = "Actual Construction: " + oRND.WarpCount + "x" + oRND.WeftCount + "/" + oRND.EPI + "x" + oRND.PPI;
                        oRND.ConstructionSuggest = "Suggested Construction: " + oRND.ConstructionSuggest;

                        bodyInfo += string.Format("<div class='col-md-12'>"
                                                + "<br><br><div > {0} </div>" //style='padding-top:30px;'
                                                + "<div style='padding-left:10px 0px 0px 25px; font-weight:bold'> {1} </div> "
                                                //+ "<div style='padding-top:20px;'></div>"
                                                + "<div style='padding-left:25px;'> {2} </div> "
                                                + "<div style='padding-left:25px;'> {3} </div> "
                                                + "<div style='padding-left:25px;'> {4} </div> "
                                                + "<div style='padding-left:25px;'> {5} </div> "
                                                + "<div style='padding-left:25px; font-weight:bold; background:yellow'> {6} </div> "
                                                //+"<div style='padding-top:10px;'>{6} </div> ",
                                                + "</div>",
                                  
                                                oItem.FabricNo,      //oItem.SCNoFull + " [" + oItem.FabricNo + "] ",
                                                oRND.ProductNameWarp,
                                                oRND.ProductWarpRnd_Suggest,
                                                oRND.FabricWeaveName,
                                                oRND.FabricWeaveNameSuggest, 
                                                oRND.WarpCount,
                                                oRND.ConstructionSuggest
                                                );
                    }
                    #endregion

                    oMailCollections.Add(string.Join(",  ", emialTos) + ",  " + ((User)Session[SessionInfo.CurrentUser]).EmailAddress);
                    oMailCollections.Add(string.Join(",  ", ccMailTos));
                    oMailCollections.Add(oMailSetUp.Subject);
                    oMailCollections.Add(bodyInfo);
                    oMailCollections.Add(ids);
                    oMailCollections.Add(prvMails);

                    //Global.MailSend(subject, bodyInfo, emialTos, new List<string>(), new List<Attachment>());
                }
                catch (Exception ex) { }
                
                ViewBag.MailCollections = oMailCollections;
            }

            return View(_oFabricSCReport);
        }

        [HttpPost]
        public JsonResult SendMailToMarketingPO(MailSetUp oMailSetUp) 
        {
            List<string> cMail = new List<string>();
            _oFabricSCReport = new FabricSCReport();
            _oFabricSCReports= new List<FabricSCReport> ();

            int nCount = 0;
            //int nOrderDateCompare = Convert.ToString(oFabricSCReport.Params.Split('~')[nCount++]);
            string sTemp = "";
            try
            {
                //sTemp = Convert.ToString(oMailSetUp.ToMail.Split('~')[nCount++]);
                //List<string> oMailTo = (!string.IsNullOrEmpty(sTemp) ? sTemp.Split(',').ToList() : new List<string>());
                //sTemp = Convert.ToString(oFabricSalesContractDetail.ErrorMessage.Split('~')[nCount++]);
                //List<string> oMailCC = (!string.IsNullOrEmpty(sTemp) ? sTemp.Split(',').ToList() : new List<string>());
                //string oMailSub = Convert.ToString(oFabricSalesContractDetail.ErrorMessage.Split('~')[nCount++]);
                //string oMailBody = Convert.ToString(oFabricSalesContractDetail.ErrorMessage.Split('~')[nCount++]);
                //string sFSCDIDs = Convert.ToString(oFabricSalesContractDetail.ErrorMessage.Split('~')[nCount++]);
                //bool isSuccess = Global.MailSend(oMailSub, oMailBody, oMailTo, oMailCC, new List<Attachment>());

                sTemp = oMailSetUp.ReportName.Trim();
                List<string> oMailTo = (!string.IsNullOrEmpty(sTemp) ? sTemp.Split(',').ToList() : new List<string>());
                sTemp = oMailSetUp.FunctionName;
                List<string> oMailCC = (!string.IsNullOrEmpty(sTemp) ? sTemp.Split(',').ToList() : new List<string>());
                string oMailSub = oMailSetUp.Subject.Trim();
                string oMailBody = oMailSetUp.ControllerName.Trim();
                string sFSCDIDs = oMailSetUp.Params;
                bool isSuccess = Global.MailSend(oMailSub, oMailBody, oMailTo, oMailCC, new List<Attachment>());

                if(isSuccess)
                {
                    _oFabricSCReport = new FabricSCReport() { Params=sFSCDIDs };
                    _oFabricSCReports = FabricSCReport.UpdateMail(_oFabricSCReport, (int)Session[SessionInfo.currentUserID]);
                }
                else throw new Exception("Failed To Send.");
            }
            catch (Exception ex)
            {
                _oFabricSCReports= new List<FabricSCReport> ();
                _oFabricSCReports.Add(new FabricSCReport() { ErrorMessage=ex.Message });
            }
            var jsonResult = Json(_oFabricSCReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult AdvanchSearch(FabricSCReport oFabricSCReport)
        {

            _oFabricSCReports = new List<FabricSCReport>();
            //try
            //{
            //    //string sSQL = MakeSQL(oFabricSCReport);
            //_oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            var tuple = GenerateFabricSCReports(oFabricSCReport);
            //}
            //catch (Exception ex)
            //{
            //    _oFabricSCReports = new List<FabricSCReport>();
            //    _oFabricSCReport.ErrorMessage = ex.Message;

            //}
            var jsonResult = Json(tuple, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            //_oFabricSCReports = new List<FabricSCReport>();
            //try
            //{
            //    string sSQL = MakeSQL(oFabricSCReport);
            //    _oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //}
            //catch (Exception ex)
            //{
            //    _oFabricSCReports = new List<FabricSCReport>();
            //    _oFabricSCReport.ErrorMessage = ex.Message;

            //}
            //var jsonResult = Json(_oFabricSCReports, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
        }

        [HttpPost]
        public JsonResult GetOrderByDispo(FabricSCReport oFabricSCReport)
        {
            _oFabricSCReports = new List<FabricSCReport>();
            string _sSQL = "";
            try
            {
                if (oFabricSCReport.ErrorMessage != "")
                {
                    _sSQL = "SELECT top(100)* FROM View_FabricSalesContractReport WHERE  ISNULL(ApproveBy,0)<>0 AND  OrderType In (" + (int)EnumFabricRequestType.Sample + "," + (int)EnumFabricRequestType.Bulk+","+(int)EnumFabricRequestType.SampleFOC+","+(int)EnumFabricRequestType.Local_Bulk+","+(int)EnumFabricRequestType.Local_Sample+","+(int)EnumFabricRequestType.Buffer+") AND ExeNo Like '%" + oFabricSCReport.ErrorMessage + "%' AND FabricSalesContractDetailID<>" + oFabricSCReport.FabricSalesContractDetailID + " order by SCDate DESC";
                }
                else
                {
                    _sSQL = "SELECT  top(100)* FROM View_FabricSalesContractReport WHERE  ISNULL(ApproveBy,0)<>0 AND  OrderType In ("+ (int)EnumFabricRequestType.Sample + "," + (int)EnumFabricRequestType.Bulk+","+(int)EnumFabricRequestType.SampleFOC+","+(int)EnumFabricRequestType.Local_Bulk+","+(int)EnumFabricRequestType.Local_Sample+","+(int)EnumFabricRequestType.Buffer+") AND FabricSalesContractDetailID<>" + oFabricSCReport.FabricSalesContractDetailID + " order by SCDate DESC";
                }
               
                _oFabricSCReports = FabricSCReport.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricSCReports = new List<FabricSCReport>();
                _oFabricSCReport.ErrorMessage = ex.Message;

            }
            var jsonResult = Json(_oFabricSCReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private Tuple<List<FabricSCReport>, List<CellRowSpan>> GenerateFabricSCReports(FabricSCReport oFabricSCReport)
        {
            Tuple<List<FabricSCReport>, List<CellRowSpan>> tuple = new Tuple<List<FabricSCReport>, List<CellRowSpan>>(new List<FabricSCReport>(), new List<CellRowSpan>());
            List<FabricSCReport> oFDPs = new List<FabricSCReport>();
            try
            {
                string sSQL = MakeSQL(oFabricSCReport);
                oFDPs = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFDPs = oFDPs.OrderBy(x => x.FabricSalesContractID).ToList();

                var oTFDPs = new List<FabricSCReport>();
                oFDPs.ForEach(x => oTFDPs.Add(x));

                var oCellRowSpans = GenerateSpan(oTFDPs);
                tuple = new Tuple<List<FabricSCReport>, List<CellRowSpan>>(oFDPs, oCellRowSpans);
            }
            catch (Exception ex)
            {
                tuple = new Tuple<List<FabricSCReport>, List<CellRowSpan>>(new List<FabricSCReport>(), new List<CellRowSpan>());
            }

            return tuple;
        }

        private List<CellRowSpan> GenerateSpan(List<FabricSCReport> oFDPs)
        {
            var oTFDPs = new List<FabricSCReport>();
            List<CellRowSpan> oCellRowSpans = new List<CellRowSpan>();
            int[,] mergerCell2D = new int[1, 2];
            int[] rowIndex = new int[15];
            int[] rowSpan = new int[15];

            while (oFDPs.Count() > 0)
            {
                oTFDPs = oFDPs.Where(x => x.SCNoFull == oFDPs.FirstOrDefault().SCNoFull).ToList();
                oFDPs.RemoveAll(x => x.SCNoFull == oTFDPs.FirstOrDefault().SCNoFull);

                rowIndex[0] = rowIndex[0] + rowSpan[0]; //
                rowSpan[0] = oTFDPs.Count(); //
                oCellRowSpans.Add(MakeSpan.GenerateRowSpan("Span", rowIndex[0], rowSpan[0]));
            }
            return oCellRowSpans;
        }
        [HttpPost]
        public JsonResult GetsBySCNoExeNo(FabricSCReport oFabricSCReport)
        {
            _oFabricSCReports = new List<FabricSCReport>();
            string sReturn1 = "SELECT * FROM View_FabricSalesContractReport";
            string sReturn = "";

            #region F SC NO
            if (!string.IsNullOrEmpty(oFabricSCReport.SCNoFull))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "SCNo LIKE '%" + oFabricSCReport.SCNoFull + "%' ";
            }
            #endregion

            #region ExeNo
            if (!string.IsNullOrEmpty(oFabricSCReport.ExeNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FabricSalesContractDetailID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE ExeNo LIKE '%" + oFabricSCReport.ExeNo + "%')";
            }
            #endregion
            
            #region sFabricID
            if (!string.IsNullOrEmpty(oFabricSCReport.FabricNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FabricNo like '%" + oFabricSCReport.FabricNo + "%'";
            }
            #endregion

            #region Export LC NO
            if (oFabricSCReport.OrderType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType=" + oFabricSCReport.OrderType;
            }
            #endregion

            string sSQL = sReturn1 + sReturn + " order by SCDate DESC, Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) ASC";
        
            _oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var jsonResult = Json(_oFabricSCReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetbySCNoMKT(FabricSCReport oFabricSCReport)
        {

            _oFabricSCReports = new List<FabricSCReport>();
            string sReturn1 = "SELECT * FROM View_FabricSalesContractReport";
            string sReturn = "";

            #region F SC NO
            if (!string.IsNullOrEmpty(oFabricSCReport.SCNoFull))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "SCNo LIKE '%" + oFabricSCReport.SCNoFull + "%' ";
            }
            #endregion
               
            #region sFabricID
            if (!string.IsNullOrEmpty(oFabricSCReport.FabricNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FabricNo like '%" + oFabricSCReport.FabricNo + "%'";
            }
            #endregion

            #region Export LC NO
            if (oFabricSCReport.OrderType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "OrderType=" + oFabricSCReport.OrderType;
            }
            #endregion

            List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(oFabricSCReport.BUID, (int)Session[SessionInfo.currentUserID]);
            if (oMarketingAccounts.Count > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MktAccountID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
            }
            string sSQL = sReturn1 + sReturn + " order by SCDate DESC, Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) ASC";
         
            _oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var jsonResult = Json(_oFabricSCReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult AdvanchSearchTwo(FabricSCReport oFabricSCReport)
        {
            _oFabricSCReports = new List<FabricSCReport>();
            try
            {
                string sSQL = MakeSQL(oFabricSCReport);
                _oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricSCReports = new List<FabricSCReport>();
                _oFabricSCReport.ErrorMessage = ex.Message;

            }
            var jsonResult = Json(_oFabricSCReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetFabricExpDelivery(FabricSCReport oFabricSCReport)
        {
             FabricExpDelivery _oFabricExpDelivery = new FabricExpDelivery();
            List<FabricExpDelivery> _oFabricExpDeliverys = new List<FabricExpDelivery>();
            String sSQL = ""; 
            try
            {
                sSQL = "SELECT * FROM View_FabricExpDelivery WHERE FSCDID = " + oFabricSCReport.FabricSalesContractDetailID;
                _oFabricExpDeliverys = FabricExpDelivery.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricExpDelivery = new FabricExpDelivery();
                _oFabricExpDelivery.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricExpDeliverys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FabricExpDeliverySave(FabricExpDelivery oFabricExpDelivery)
        {
            FabricExpDelivery _oFabricExpDelivery = new FabricExpDelivery();
            try
            {
                _oFabricExpDelivery = oFabricExpDelivery;
                _oFabricExpDelivery = _oFabricExpDelivery.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricExpDelivery = new FabricExpDelivery();
                _oFabricExpDelivery.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricExpDelivery);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult FabricExpDeliveryDelete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricExpDelivery oFabricExpDelivery = new FabricExpDelivery();
                sFeedBackMessage = oFabricExpDelivery.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRollForTransfer(FNOrderFabricReceive oFNOrderFabricReceive)
        {
            List<FNOrderFabricReceive> oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
            FNOrderFabricReceive _oFNOrderFabricReceive = new FNOrderFabricReceive();
            try
            {
                string sSQL = "SELECT * FROM View_FNOrderFabricReceive WHERE  ReceiveBy<>0 AND FSCDID = " + oFNOrderFabricReceive.FSCDID + " ORDER BY FNOrderFabricReceiveID";
                oFNOrderFabricReceives = FNOrderFabricReceive.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
                oFNOrderFabricReceive.ErrorMessage = ex.Message;

            }
            var jsonResult = Json(oFNOrderFabricReceives, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public JsonResult ReloadData(FNOrderFabricReceive oFNOrderFabricReceive)
        {
            List<FNOrderFabricReceive> oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
            FNOrderFabricReceive _oFNOrderFabricReceive = new FNOrderFabricReceive();
            try
            {
                string sSQL = "SELECT * FROM View_FNOrderFabricReceive WHERE FSCDID = " + oFNOrderFabricReceive.FSCDID + " ORDER BY FNOrderFabricReceiveID";
                oFNOrderFabricReceives = FNOrderFabricReceive.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
                oFNOrderFabricReceive.ErrorMessage = ex.Message;

            }
            var jsonResult = Json(oFNOrderFabricReceives, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        private string MakeSQL(FabricSCReport oFabricSCReport)
        {
            int nBUID = 0;
            string sTemp = oFabricSCReport.ErrorMessage;
            nBUID = oFabricSCReport.BUID;
            string sReturn1 = "";
            string sReturn = "";
            if (!string.IsNullOrEmpty(sTemp) && sTemp!=null)
            {
                 sReturn1 = "SELECT * FROM View_FabricSalesContractReport";

                #region Set Values

                string sExportPINo = Convert.ToString(sTemp.Split('~')[0]);
                string sFabricID = Convert.ToString(sTemp.Split('~')[1]);
                string sContractorIDs = Convert.ToString(sTemp.Split('~')[2]);
                string sBuyerIDs = Convert.ToString(sTemp.Split('~')[3]);
                string sMAccountIDs = Convert.ToString(sTemp.Split('~')[4]);

                int nCboSCDate = Convert.ToInt32(sTemp.Split('~')[5]);
                DateTime dFromSCDate = DateTime.Now;
                DateTime dToSCDate = DateTime.Now;
                if (nCboSCDate > 0)
                {
                    dFromSCDate = Convert.ToDateTime(sTemp.Split('~')[6]);
                    dToSCDate = Convert.ToDateTime(sTemp.Split('~')[7]);
                }

                int ncboApproveDate = Convert.ToInt32(sTemp.Split('~')[8]);
                DateTime dFromApproveDate = DateTime.Now;
                DateTime dToApproveDate = DateTime.Now;
                if (ncboApproveDate > 0)
                {
                    dFromApproveDate = Convert.ToDateTime(sTemp.Split('~')[9]);
                    dToApproveDate = Convert.ToDateTime(sTemp.Split('~')[10]);
                }

                int ncboProductionType = 0;
                if (sTemp.Split('~').Length > 11)
                    Int32.TryParse(sTemp.Split('~')[11], out ncboProductionType);

                int ncboPrinted = 0;
                if (sTemp.Split('~').Length > 12)
                    Int32.TryParse(sTemp.Split('~')[12], out ncboPrinted);
                string sOrderTypeIDs = string.Empty;
                if (sTemp.Split('~').Length > 13)
                    sOrderTypeIDs = sTemp.Split('~')[13];

                string sCurrentStatus = string.Empty;
                if (sTemp.Split('~').Length > 14)
                    sCurrentStatus = sTemp.Split('~')[14];
              
                if (sTemp.Split('~').Length > 15)
                    Int32.TryParse(sTemp.Split('~')[15], out nBUID);
                oFabricSCReport.SCNoFull = string.Empty;
                if (sTemp.Split('~').Length > 16)
                    oFabricSCReport.SCNoFull = sTemp.Split('~')[16];
                //int ncboProductionType = Convert.ToInt32(sTemp.Split('~')[11]);
                //int ncboPrinted = Convert.ToInt32(sTemp.Split('~')[12]);
                //string sOrderTypeIDs = Convert.ToString(sTemp.Split('~')[13]);
                //string sCurrentStatus = Convert.ToString(sTemp.Split('~')[14]);
                //int nBUID = Convert.ToInt32(sTemp.Split('~')[15]);
                //oFabricSCReport.SCNoFull = sTemp.Split('~')[16];

                int nType = 0;
                if (sTemp.Split('~').Length > 17)
                    Int32.TryParse(sTemp.Split('~')[17], out nType);

                string sHLNo = string.Empty;
                if (sTemp.Split('~').Length > 18)
                    sHLNo = sTemp.Split('~')[18];

                bool IsReceived = false;
                if (sTemp.Split('~').Length > 19)
                    bool.TryParse(sTemp.Split('~')[19], out IsReceived);

                bool IsSubmitHO = false;
                if (sTemp.Split('~').Length > 20)
                    bool.TryParse(sTemp.Split('~')[20], out IsSubmitHO);

                bool IsWatingForLab = false;
                if (sTemp.Split('~').Length > 21)
                    bool.TryParse(sTemp.Split('~')[21], out IsWatingForLab);

                bool IsYetToSubmitHO = false;
                if (sTemp.Split('~').Length > 22)
                    bool.TryParse(sTemp.Split('~')[22], out IsYetToSubmitHO);

                string sLabStatus = string.Empty;
                if (sTemp.Split('~').Length > 23)
                    sLabStatus = sTemp.Split('~')[23];

                int ncboReceivedDate = 0;
                if (sTemp.Split('~').Length > 24)
                    Int32.TryParse(sTemp.Split('~')[24], out ncboReceivedDate);
              
                DateTime dFromReceivedDate = DateTime.Now;
                DateTime dToReceivedDate = DateTime.Now;
                if (ncboReceivedDate > 0)
                {
                    dFromReceivedDate = Convert.ToDateTime(sTemp.Split('~')[25]);
                    dToReceivedDate = Convert.ToDateTime(sTemp.Split('~')[26]);
                }
                int ncboExcDate = 0;
                if (sTemp.Split('~').Length > 27)
                    Int32.TryParse(sTemp.Split('~')[27], out ncboExcDate);

                DateTime dFromExcDate = DateTime.Now;
                DateTime dToExcDate = DateTime.Now;
                if (ncboExcDate > 0)
                {
                    dFromExcDate = Convert.ToDateTime(sTemp.Split('~')[28]);
                    dToExcDate = Convert.ToDateTime(sTemp.Split('~')[29]);
                }

                int ncboLabStatus = 0;
                DateTime dFromLabDate = DateTime.Now;
                DateTime dToLabDate = DateTime.Now;
                int ncboLabDate = 0;
                if (sTemp.Split('~').Length > 30) 
                {
                    if (!string.IsNullOrEmpty(sTemp.Split('~')[30]))
                    {
                        ncboLabStatus = Convert.ToInt32(sTemp.Split('~')[30]);
                        ncboLabDate = Convert.ToInt32(sTemp.Split('~')[31]);
                        if (ncboLabDate > 0)
                        {
                            dFromLabDate = Convert.ToDateTime(sTemp.Split('~')[32]);
                            dToLabDate = Convert.ToDateTime(sTemp.Split('~')[33]);
                        }
                    }
                }
                string sMAGroupIDs = string.Empty;
                if (sTemp.Split('~').Length > 34)
                    sMAGroupIDs = sTemp.Split('~')[34];
               
                #endregion

                #region Make Query

                #region F SC NO
                if (!string.IsNullOrEmpty(oFabricSCReport.SCNoFull))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SCNoFull LIKE '%" + oFabricSCReport.SCNoFull + "%' ";
                }
                #endregion

                #region Export PI NO
                if (!string.IsNullOrEmpty(sExportPINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "PINo like '%" + sExportPINo + "%'";
                }
                #endregion

                #region sFabricID
                if (!string.IsNullOrEmpty(sFabricID))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FabricNo like '%" + sFabricID + "%'";
                }
                #endregion

                #region ContractorID
                if (!String.IsNullOrEmpty(sContractorIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID in( " + sContractorIDs + ") ";
                }
                #endregion

                #region BBuyerID
                if (!String.IsNullOrEmpty(sBuyerIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerID in( " + sBuyerIDs + ") ";
                }
                #endregion

                #region sMAccountIDs
                if (!String.IsNullOrEmpty(sMAccountIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID in(" + sMAccountIDs + ") ";
                }
                #endregion
                #region sMKTGroupIDs
                if (!String.IsNullOrEmpty(sMAGroupIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "MktAccountID  in(Select MarketingAccountID  from View_MarketingAccount where GroupID >0 and GroupID in (" + sMAGroupIDs + ")) ";
                }
                #endregion

                #region F SC Date
                if (nCboSCDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboSCDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion

                #region Approval Date
                if (ncboApproveDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboApproveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                #region ReceivedDate
                if (ncboReceivedDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboReceivedDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboReceivedDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),FabricReceiveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceivedDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceivedDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                #region ncboExcDate
                if (ncboExcDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboExcDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106)))";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExcDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    else if (ncboExcDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where  CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromExcDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToExcDate.ToString("dd MMM yyyy") + "',106)))";
                    }
                }
                #endregion

                #region ncboProductionType
                if (ncboProductionType > 0)
                {

                    if (ncboProductionType == 1)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IsInHouse =1";
                    }
                    else if (ncboProductionType == 2)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IsInHouse =0";
                    }

                }
                #endregion

                #region nIsPrinting
                if (ncboPrinted > 0)
                {
                    if (ncboPrinted == 1)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " IsPrint =1";
                    }
                    else if (ncboPrinted == 2)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "IsPrint =0";
                    }
                }
                #endregion
                #region Order Type Wise
                if (!string.IsNullOrEmpty(sOrderTypeIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderType IN (" + sOrderTypeIDs + ") ";
                }
                #endregion

                #region Custom Type R&D Menu Wise
                //nType => 1; // Analysis
                //nType => 2; // H/L PO
                //nType => 3; // Bulk 

                if (nType > 0 && nType <= 3)
                {
                    int[] OrderTypes = this.GetOrderTypes(nType);

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ISNULL(ApproveBy,0)<>0 AND  OrderType In (" + string.Join(",", OrderTypes) + ")";
                }

                #endregion


                #region Status Wise
                if (!string.IsNullOrEmpty(sCurrentStatus))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Currentstatus IN (" + sCurrentStatus + ") ";
                }
                #endregion

                #region HL NO
                if (!string.IsNullOrEmpty(sHLNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ExeNo Like '%" + sHLNo + "%'";
                }
                #endregion
                #region Received
                if (IsReceived)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ISNULL(FabricReceiveBy,0)!=0";
                }
                #endregion

                #region Submit To HO
                if (IsSubmitHO)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SubmissionDate != '' AND SubmissionDate IS NOT NULL ";
                }
                #endregion

                #region IsWatingForLab
                if (IsWatingForLab)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FabricID not in (Select FabricID from Labdip where isnull(FabricID,0)>0)";
                }
                #endregion

                #region Yet Submit To HO
                if (IsYetToSubmitHO)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SCDetailType=" +(int) EnumSCDetailType.ExtraOrder;
                }
                #endregion

                #region sLabStatus
                if (!string.IsNullOrEmpty(sLabStatus))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LabStatus in (" + sLabStatus + ")";
                }

                #endregion

                #region FSC Lab History
                //--SELECT * FROM FabricSalesContractDetail AS FSCDetail WHERE LabStatus=4 AND FabricSalesContractDetailID IN ((Select Top(1)Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.FSCDetailID = FSCDetail.FabricSalesContractDetailID AND Lab.CurrentStatus=4 ORDER BY DBServerDateTime DESC ))
                if (ncboLabDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboLabDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " FabricSalesContractDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus="+ncboLabStatus+" AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboLabDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + "  FabricSalesContractDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboLabDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + "  FabricSalesContractDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboLabDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + "  FabricSalesContractDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboLabDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + "  FabricSalesContractDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLabDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                    else if (ncboLabDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + "  FabricSalesContractDetailID IN (Select Lab.FSCDetailID from FSCDetailLab as Lab WHERE Lab.CurrentStatus=" + ncboLabStatus + " AND CONVERT(DATE,CONVERT(VARCHAR(12),Lab.OperationDate ,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromLabDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToLabDate.ToString("dd MMM yyyy") + "',106)) )";
                    }
                }
                #endregion

                List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(nBUID, (int)Session[SessionInfo.currentUserID]);
                if (oMarketingAccounts.Count > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
                }
                #endregion

            }
            else
            {
                sReturn1 = "SELECT top(150)* FROM View_FabricSalesContractReport where ordertype in (" + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.CAD + "," + (int)EnumFabricRequestType.Color + "," + (int)EnumFabricRequestType.Labdip + "," + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.YarnSkein + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") ";

                List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(nBUID, (int)Session[SessionInfo.currentUserID]);
                if (oMarketingAccounts.Count > 0)
                {
                    Global.TagSQL(ref sReturn1);
                    sReturn1 = sReturn1 + " MktAccountID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
                }

                //_oFabricSCReports = FabricSCReport.Gets("Select top(150)* from View_FabricSalesContractReport where ordertype in (" + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.CAD + "," + (int)EnumFabricRequestType.Color + "," + (int)EnumFabricRequestType.Labdip + "," + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.YarnSkein + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

          //  sReturn = sReturn1 + sReturn + " order by OrderType, Convert(int, dbo.udf_GetNumeric (ExeNo)) ASC";
            sReturn = sReturn1 + sReturn + " order by SCDate DESC, Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) ASC";
            
            return sReturn;
        }

        #region Print XL
        public void Print_ReportXL(string sTempString, int BUID)
        {

            _oFabricSCReports = new List<FabricSCReport>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            oFabricSCReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oFabricSCReport);
            _oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            bool bRateView = false;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FabricDeliverySchedule> oFebricDeliverySchedules = new List<FabricDeliverySchedule>();
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();

            oAuthorizationRoleMappings=   AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricPO).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            foreach (AuthorizationRoleMapping oitem in oAuthorizationRoleMappings)
            {
                if(oitem. OperationType == EnumRoleOperationType.RateView)
                {
                    bRateView = true;
                    break;
                }
            }

            string sQuery = "SELECT * FROM View_FabricDeliverySchedule WHERE FabricSalesContractID IN (" + string.Join(",", _oFabricSCReports.Select(x => x.FabricSalesContractID)) + ")";
            oFebricDeliverySchedules = FabricDeliverySchedule.Gets(sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
            int nSL = 0;
            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 31;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                int nColumn = 3;
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("PO Report");
                sheet.Name = "PO Report";
                sheet.Column(nColumn).Width = 22;
                sheet.Column(++nColumn).Width = 30;
                sheet.Column(++nColumn).Width = 35;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 30;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 30;
                sheet.Column(++nColumn).Width = 20;

                //   nEndCol = 31;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "PO Register "; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region
                int nColumnIndex = 0;
                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "PO No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "PO Issue Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Buying House"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Issue To"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = " MKT Team"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Concern Person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Approve By"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Approve Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 10]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 11]; cell.Value = "PI Date"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 12]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 13]; cell.Value = "L/C Status"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "MKT Ref"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "PO Qty(Y)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Qty PI(Y)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Yet To PI(Y)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Dispo No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Buyer Ref."; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Construction"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Composition"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Fabric type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Weave"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Size"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Finish Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = " Style No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Width"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Fabric Design Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = " Order Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "PP Sample Delivery Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = " Dispo No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex++;
                #endregion
                string sTemp = ""; int nFabricSalesContractID = -999;
                #region Data

                _oFabricSCReports = _oFabricSCReports.OrderBy(x => x.FabricSalesContractID).ToList();
                foreach (FabricSCReport oItem in _oFabricSCReports)
                {
                    nColumnIndex = 0;
                    int nRowSpan = 0;
                    if (nFabricSalesContractID != oItem.FabricSalesContractID)
                    {
                        nSL++;
                        nRowSpan = _oFabricSCReports.Where(x => x.FabricSalesContractID == oItem.FabricSalesContractID).Count()-1;
                        
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan,nColumnIndex]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#####"; 
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan,nColumnIndex]; cell.Value = "" + oItem.SCNoFull; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#####";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan,nColumnIndex]; cell.Value = oItem.SCDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#####";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan,nColumnIndex]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#####";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan,nColumnIndex]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#####";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan,nColumnIndex]; cell.Value = oItem.MKTGroup; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#####";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan,nColumnIndex]; cell.Value = (oItem.MKTPName); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan,nColumnIndex]; cell.Value = oItem.ApproveByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan,nColumnIndex]; cell.Value = oItem.ApprovedDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nColumnIndex = 9;
                    //cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.PIDateSt; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.LCStatusSt; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.FabricNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (bRateView)
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    else
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                   
                    if (bRateView)
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    else {
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    
                    if (nFabricSalesContractID != oItem.FabricSalesContractID)
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nColumnIndex = 14;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.Qty_PI; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.Qty_PIBal; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.ExeNo; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.HLReference; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.BuyerReference; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    //nColumnIndex, nRowIndex + nRowSpan, nColumnIndex
                    //if (nFabricSalesContractID != oItem.FabricSalesContractID)
                    //{
                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.ProcessTypeName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //Fabric type
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.FabricWeaveName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.Size; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = oItem.FinishTypeName; cell.Style.Font.Bold = false;
                        //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        //try { cell.Merge = true; }
                        //catch (Exception e) { }
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    //}
                    //nColumnIndex = 23;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.FinishTypeName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.ColorInfo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.FabricWidth; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.FabricDesignName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (nFabricSalesContractID != oItem.FabricSalesContractID)
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = oItem.OrderTypeSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        string PP_DeliveyInfo = string.Join(",\n", oFebricDeliverySchedules.Where(x => x.FabricSalesContractID == oItem.FabricSalesContractID).Select(x => new { CustomString = x.Name + ": " + x.DeliveryDateSt }).ToList().Select(x => x.CustomString).ToList());
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = PP_DeliveyInfo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nColumnIndex = 30;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem.ExeNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nFabricSalesContractID= oItem.FabricSalesContractID;
                    nEndRow = nRowIndex;
                    nRowIndex++;
                }
                #endregion

                //#region Total
                //cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 7]; cell.Value = nQty; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 9]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 14]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nRowIndex = nRowIndex + 1;
                //#endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PORegister.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion
        }
        public void Print_ReportXL_DC(string sTempString, int BUID)
        {
            _oFabricSCReports = new List<FabricSCReport>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            oFabricSCReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oFabricSCReport);
            _oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            bool bRateView = false;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FabricDeliverySchedule> oFebricDeliverySchedules = new List<FabricDeliverySchedule>();
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            List<FabricDeliveryOrderDetail> oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();

            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricPO).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            foreach (AuthorizationRoleMapping oitem in oAuthorizationRoleMappings)
            {
                if (oitem.OperationType == EnumRoleOperationType.RateView)
                {
                    bRateView = true;
                    break;
                }
            }

            if (_oFabricSCReports.Any())
            {
                string sQuery = "SELECT * FROM View_FabricDeliverySchedule WHERE FabricSalesContractID IN (" + string.Join(",", _oFabricSCReports.Select(x => x.FabricSalesContractID)) + ")";
                oFebricDeliverySchedules = FabricDeliverySchedule.Gets(sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricDeliveryOrderDetails = FabricDeliveryOrderDetail.Gets("SELECT * FROM View_FabricDeliveryOrderDetail AS DD WHERE "
                                        + " DD.FEOID IN (" + string.Join(",", _oFabricSCReports.Select(x => x.FabricSalesContractDetailID)) + ")"
                                        + " AND DD.FDOID IN (SELECT FabricDeliveryOrder.FDOID from FabricDeliveryOrder WHERE Isnull(FabricDeliveryOrder.DOStatus,0)<>8 )  ", ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricDeliveryChallanDetails = FabricDeliveryChallanDetail.Gets("SELECT * FROM View_FabricDeliveryChallanDetail AS DD WHERE "
                                        + " DD.FDODID IN (" + string.Join(",", oFabricDeliveryOrderDetails.Select(x => x.FDODID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            int nSL = 0;

            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 31;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                int nColumn = 2;
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("PO Report");
                sheet.Name = "PO Report";
                
                sheet.Column(nColumn).Width = 15;
                sheet.Column(++nColumn).Width = 30;
                sheet.Column(++nColumn).Width = 35;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;

                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 30;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                sheet.Column(++nColumn).Width = 20;
                
                //sheet.Column(++nColumn).Width = 20;

                nEndCol = nColumn;
                //sheet.Column(++nColumn).Width = 20;
                //sheet.Column(++nColumn).Width = 30;
                //sheet.Column(++nColumn).Width = 20;

                //   nEndCol = 28;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "PO Register"; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region HEADER
                //SL No	PO No	PO Approve Date	Buying House	Issue To	 *
                int nColumnIndex = 1;
                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "PO No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "PO Approve Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Buying House"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Issue To"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = " MKT Team"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //MKT Ref	PO Qty(Y)	Unit Price	Amount	PI No	Qty PI(Y)	

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "MKT Ref"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "PO Qty(Y)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Qty PI(Y)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Yet To PI(Y)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //Yet To PI(Y)	Construction	Composition	Color	 Order Type	PP Sample Delivery Date	 Dispo No	

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Buyer Ref."; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Construction"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Composition"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = " Order Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "PP Sample Delivery Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = " Dispo No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //Final Insp. Qty	Send to Store	D.C Date	D.C Qty	Chaaln No								

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Final Insp. Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Send to Store"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "D.C Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "D.C Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = "Challan No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                #endregion

                nRowIndex++;

                int nFabricSalesContractDetailID = -999;

                #region Data

                //_oFabricSCReports = _oFabricSCReports.OrderBy(x => x.FabricSalesContractID).ToList();
                //oFabricDeliveryOrderDetails = oFabricDeliveryOrderDetails.OrderBy(x => x.FEOID).ToList();
                
                oFabricDeliveryChallanDetails = oFabricDeliveryChallanDetails.OrderBy(x => x.FDODID).ToList();

                foreach (FabricDeliveryChallanDetail oItem_DC in oFabricDeliveryChallanDetails)
                {
                    nColumnIndex = 1;
                    int nRowSpan = 0;

                    var oItem_DO = oFabricDeliveryOrderDetails.Where(x => x.FDODID == oItem_DC.FDODID).FirstOrDefault();
                    var oFSCReport = _oFabricSCReports.Where(x => x.FabricSalesContractDetailID == oItem_DO.FEOID).FirstOrDefault();

                    if (nFabricSalesContractDetailID != oItem_DO.FEOID)
                    {
                        nSL++;
                        nRowSpan = oFabricDeliveryChallanDetails.Where(x => x.FDODID == oItem_DO.FDODID).Count() - 1;

                        //SL No	PO No	PO Approve Date	Buying House	Issue To								

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#####";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = "" + oFSCReport.SCNoFull; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#####";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = oFSCReport.SCDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#####";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = oFSCReport.BuyerName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#####";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = oFSCReport.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#####";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        // MKT Team	MKT Ref	PO Qty(Y)	Unit Price	Amount	PI No	
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = oFSCReport.MKTGroup; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#####";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = (oFSCReport.MKTPName); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }
                    nColumnIndex = 7;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem_DO.FabricNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem_DO.Qty_P0; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem_DO.UnitPrice; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   
                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oFSCReport.Amount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (nFabricSalesContractDetailID != oItem_DO.FEOID)
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = oItem_DO.PINo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nColumnIndex = 12;

                    //Qty PI(Y)	Yet To PI(Y)	Construction	Composition	

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem_DO.Qty_PI; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (nFabricSalesContractDetailID != oItem_DO.FEOID)
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = oFSCReport.Qty_PIBal; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                       
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = oFSCReport.BuyerReference; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nColumnIndex = 15;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem_DC.Construction; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (nFabricSalesContractDetailID != oItem_DO.FEOID)
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = oItem_DC.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nColumnIndex = 17;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem_DC.ColorInfo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Color	 Order Type	PP Sample Delivery Date	 Dispo No	Final Insp. Qty	Send to Store	D.C Date	D.C Qty	Chaaln No	

                    if (nFabricSalesContractDetailID != oItem_DO.FEOID)
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = oItem_DO.OrderTypeSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        string PP_DeliveyInfo = string.Join(",\n", oFebricDeliverySchedules.Where(x => x.FabricSalesContractID == oItem_DO.FEOID).Select(x => new { CustomString = x.Name + ": " + x.DeliveryDateSt }).ToList().Select(x => x.CustomString).ToList());
                        cell = sheet.Cells[nRowIndex, ++nColumnIndex, nRowIndex + nRowSpan, nColumnIndex]; cell.Value = PP_DeliveyInfo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        try { cell.Merge = true; }
                        catch (Exception e) { }
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nColumnIndex = 20;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem_DC.ExeNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Final Insp. Qty	Send to Store	D.C Date	D.C Qty	Chaaln No	
                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem_DC.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value =""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem_DC.ChallanDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem_DC.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumnIndex]; cell.Value = oItem_DC.ChallanNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nFabricSalesContractDetailID = oItem_DO.FEOID;
                    nEndRow = nRowIndex;
                    nRowIndex++;
                }
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PORegister.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        public void Print_ReportXL_Order(string sTempString, int BUID)
        {

            _oFabricSCReports = new List<FabricSCReport>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            oFabricSCReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oFabricSCReport);
            _oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            int nCount = 0;
            double nQty = 0;
            double nQty_DO = 0;
            double nQty_DC = 0;
            double nStockInHand = 0;
            double nAmount = 0;


            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 14;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("PO Report");
                sheet.Name = "PO Report";
                sheet.Column(3).Width = 22;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 20;
                //sheet.Column(12).Width = 20;
                //sheet.Column(13).Width = 20;
                //sheet.Column(14).Width = 20;
                //sheet.Column(15).Width = 20;
                //sheet.Column(16).Width = 20;
                //sheet.Column(17).Width = 20;
                //sheet.Column(18).Width = 20;
                //sheet.Column(19).Width = 20;
                //sheet.Column(20).Width = 20;
                //sheet.Column(21).Width = 20;
                //sheet.Column(22).Width = 20;
                //sheet.Column(23).Width = 20;
                //sheet.Column(24).Width = 20;
                //sheet.Column(25).Width = 20;
                //sheet.Column(26).Width = 20;
                //sheet.Column(27).Width = 20;
                //sheet.Column(28).Width = 20;
                //sheet.Column(29).Width = 20;


                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Order Management "; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region
                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "PO No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "PO Issue Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Exe No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Buying House"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Issue To"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = " MKT Team"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Concern Person"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "PI Date"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "L/C Status"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "MKT Ref"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Qty(Y)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Price"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Stock In Hand(Y)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "DO Qty(Y)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Yet To DO(Y)"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "DC Qty(Y)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Balance Qty(Y)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;




                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Construction"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Composition"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Fabric type"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Weave"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Finish Type"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Width"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Fabric Design Name"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nCount++;
                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = " Order Type"; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#####";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

             

                nRowIndex++;
                #endregion
              
                #region Data
                foreach (FabricSCReport oItem in _oFabricSCReports)
                {
                    nCount = 0;
                    nSL++;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "" + oItem.SCNoFull; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.SCDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ExeNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.MKTGroup; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#####";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = (oItem.MKTPName); cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    //nCount++;

                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.PIDateSt; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.LCStatusSt; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.FabricNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.StockInHand; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.Qty_DO; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.Qty_DOBal; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.Qty_DC; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.Qty_DCBal; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.FinishTypeName; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.FabricWeaveName; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.FinishTypeName; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ColorInfo; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.FabricWidth; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.FabricDesignName; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nCount++;
                    //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.OrderTypeSt; cell.Style.Font.Bold = false;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nQty = nQty + oItem.Qty;
                    nQty_DO = nQty_DO+oItem.Qty_DO;
                    nQty_DC = nQty_DC + oItem.Qty_DC;
                    nStockInHand = nStockInHand+oItem.StockInHand;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion
                nCount=0;
                nCount++;
                #region Total
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = nQty; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = nQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = nStockInHand; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = nQty_DO; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = nQty_DO; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = nQty_DC; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = Math.Round(nQty - nQty_DC,2); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PORegister.xlsx");
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

        #endregion

        #region Fabric Report From PO
        [HttpPost]
        public JsonResult GetSalesContactDetail(FabricSCReport oFabricSCReport)
        {

            string sSQL = "SELECT * FROM View_FabricSalesContractReport Where FabricSalesContractDetailID =" + oFabricSCReport.FabricSalesContractDetailID;
            var result = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (result.Any() && result.First().FabricSalesContractDetailID > 0)
                oFabricSCReport = result.First();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSCReport);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private int[] GetOrderTypes(int type)
        {
            int[] OrderTypes = new int[] { };
            if (type == 1)// Analysis PO
                OrderTypes = new int[]
                { 
                        (int)EnumFabricRequestType.Analysis, 
                        (int)EnumFabricRequestType.CAD,
                        (int)EnumFabricRequestType.Color,
                        (int)EnumFabricRequestType.Labdip,
                        (int)EnumFabricRequestType.YarnSkein
                };
            else if (type == 2) //HL
                OrderTypes = new int[]
                { 
                        (int)EnumFabricRequestType.HandLoom
                };
            else if (type == 3) //Bulk, Sample
                OrderTypes = new int[]
                { 
                        (int)EnumFabricRequestType.Bulk,
                        (int)EnumFabricRequestType.Sample,
                        (int)EnumFabricRequestType.SampleFOC,
                        (int)EnumFabricRequestType.Local_Bulk,
                        (int)EnumFabricRequestType.Local_Sample,
                        (int)EnumFabricRequestType.Buffer
                };
            else if (type == 4) //Bulk, Sample,HO
                OrderTypes = new int[]
                {        (int)EnumFabricRequestType.HandLoom,
                        (int)EnumFabricRequestType.Bulk,
                        (int)EnumFabricRequestType.Sample,
                        (int)EnumFabricRequestType.SampleFOC,
                        (int)EnumFabricRequestType.Local_Bulk,
                        (int)EnumFabricRequestType.Local_Sample
                };
            else if (type == 5) //Bulk, Sample,HO
                OrderTypes = new int[]
                {        (int)EnumFabricRequestType.Analysis, 
                        (int)EnumFabricRequestType.CAD
                };
            return OrderTypes;
        }
        public ActionResult View_FabricSCRnDHL(int buid, int menuid)
        {
            return RedirectToAction("View_FabricSCReceive", new { buid = buid, menuid = menuid, type = 1 });
        }
        public ActionResult View_FabricSCHandLoom(int buid, int menuid)
        {
            return RedirectToAction("View_FabricSCReceive", new { buid = buid, menuid = menuid, type = 2 });
        }
        public ActionResult View_FabricSCBulk(int buid, int menuid)
        {
            return RedirectToAction("View_FabricSCReceive", new { buid = buid, menuid = menuid, type = 3 });
        }
        public ActionResult View_FabricSCReceive(int buid, int menuid, int type)
        {

            int[] OrderTypes = this.GetOrderTypes(type);
            int[] FabricPOStatus = new int[] { (int)EnumFabricPOStatus.None, (int)EnumFabricPOStatus.Initialized, (int)EnumFabricPOStatus.RequestForApprove };

            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);


            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.LCTerms = LCTerm.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.PaymentInstructions = EnumObject.jGets(typeof(EnumPaymentInstruction));
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)).Where(x => OrderTypes.Contains(x.id)).ToList();
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus)).Where(x => !FabricPOStatus.Contains(x.id)).ToList();
            ViewBag.LabdipStatus = EnumObject.jGets(typeof(EnumLabdipOrderStatus)).Where(x => OrderTypes.Contains(x.id)).ToList();


            var oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);



            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            ViewBag.Type = type;

            var tuple = GenerateFabricSCReports(_oFabricSCReport);
            _oFabricSCReports = tuple.Item1;
            ViewBag.CellRowSpans = tuple.Item2;

            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricPO).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            return View(_oFabricSCReports);
        }
        [HttpPost]
        public JsonResult CreateLab(FabricSCReport oFabricSCReport)
        {
            _oFabricSCReport = new FabricSCReport();
            _oFabricSCReports = new List<FabricSCReport>();
            LabDip oLabDip = new LabDip();

            try
            {
                oLabDip = LabDip.GetByFSDMap(oFabricSCReport.Params, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oLabDip.LabDipID > 0) 
                {
                    _oFabricSCReport.LabDipID = oLabDip.LabDipID;
                }
                else _oFabricSCReport = oFabricSCReport.CreateLab(EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);  
            }
            catch (Exception ex)
            {
                oFabricSCReport.ErrorMessage = ex.Message;
                _oFabricSCReports = new List<FabricSCReport>();
                _oFabricSCReports.Add(_oFabricSCReport);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSCReport);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LabOperation(FabricSCReport oFabricSCReport)
        {
            _oFabricSCReport = new FabricSCReport();
            _oFabricSCReports = new List<FabricSCReport>();
            try
            {
                if ((int)oFabricSCReport.LabStatus == (int)EnumFabricLabStatus.InLab) /*4*/
                {
                    _oFabricSCReport = oFabricSCReport.OperationLab(EnumDBOperation.Request, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if ((int)oFabricSCReport.LabStatus == (int)EnumFabricLabStatus.LabdipDone) /*5*/
                {
                    _oFabricSCReport = oFabricSCReport.OperationLab(EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if ((int)oFabricSCReport.LabStatus == (int)EnumFabricLabStatus.Undo) /*17 -- undo forward*/
                {
                    oFabricSCReport.LabStatus = EnumFabricLabStatus.None;
                    _oFabricSCReport = oFabricSCReport.OperationLab(EnumDBOperation.Undo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if ((int)oFabricSCReport.LabStatus == (int)EnumFabricLabStatus.ReLab) /*16*/
                {
                    oFabricSCReport.LabStatus = EnumFabricLabStatus.InLab;
                    _oFabricSCReport = oFabricSCReport.OperationLab(EnumDBOperation.Revise, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if ((int)oFabricSCReport.LabStatus == (int)EnumFabricLabStatus.Submitted) /*8*/
                {
                    oFabricSCReport.LabStatus = EnumFabricLabStatus.Submitted;
                    _oFabricSCReport = oFabricSCReport.OperationLab(EnumDBOperation.Delivered, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFabricSCReport.ErrorMessage = ex.Message;
                _oFabricSCReports = new List<FabricSCReport>();
                _oFabricSCReports.Add(_oFabricSCReport);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSCReport);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsLabDip_byFSCD(FabricSCReport oFSCR)
        {
            List<LabDip> oLabDips = new List<LabDip>();
            try
            {
                if (oFSCR.FabricSalesContractDetailID> 0)
                {
                    oLabDips = LabDip.Gets("SELECT * FROM View_Labdip_FSCD WHERE FSCDetailID= "+oFSCR.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oLabDips = new List<LabDip>();
                oLabDips.Add(new LabDip() { ErrorMessage=ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDips);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult GetsLabDipDetail(LabDipDetail oLabDipDetail)
        {
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            try
            {
                string sSQL = "SELECT TOP 200 * FROM  View_LabdipDetail ";
                string sReturn = "WHERE ISNULL(LDNo,'')+ISNULL(ColorNo,'')!=''";

                if(oLabDipDetail.LabDipDetailID>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LabdipID IN (SELECT FF.LabdipID From FSCLabMapping AS FF WHERE FF.FSCDetailID=" + oLabDipDetail.LabDipDetailID + ")";
                }
                if (!String.IsNullOrEmpty(oLabDipDetail.LabdipNo))
                {
                    oLabDipDetail.LabdipNo = oLabDipDetail.LabdipNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LabdipNo Like'%" + oLabDipDetail.LabdipNo + "%'";
                }
                if (!String.IsNullOrEmpty(oLabDipDetail.ColorNo))
                {
                    oLabDipDetail.LabdipNo = oLabDipDetail.ColorNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ISNULL(LDNo,'')+ISNULL(ColorNo,'') Like'%" + oLabDipDetail.ColorNo + "%'";
                }

                //Global.TagSQL(ref sReturn);
                //sReturn = sReturn + "LabdipID in (Select LabdipID from Labdip where Labdip.OrderStatus in (5,6,7,8,9))";

                sSQL = sSQL + "" + sReturn;
                oLabDipDetails = LabDipDetail.Gets(sSQL + " ORDER BY OrderDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipDetails = new List<LabDipDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Received(FabricSCReport oFabricSCReport)
        {
            _oFabricSCReports = new List<FabricSCReport>();
            try
            {
                _oFabricSCReports = FabricSCReport.Gets("SELECT * FROM View_FabricSalesContractReport where  FabricSalesContractID in( " + oFabricSCReport.FabricSalesContractID + ")  order by SCDate DESC, Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) ASC", ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFabricSCReports = FabricSCReport.Received(_oFabricSCReports, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oFabricSCReports = FabricSCReport.Gets("SELECT * FROM View_FabricSalesContractReport where  FabricSalesContractID in( " + oFabricSCReport.FabricSalesContractID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFabricSCReport.ErrorMessage = ex.Message;
                _oFabricSCReports = new List<FabricSCReport>();
                _oFabricSCReports.Add(oFabricSCReport);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSCReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveExcNo(FabricSCReport oFabricSCReport)
        {
         
            try
            {
               _oFabricSCReport = oFabricSCReport.SaveExcNo( ((User)Session[SessionInfo.CurrentUser]).UserID);
              
            }
            catch (Exception ex)
            {
                oFabricSCReport.ErrorMessage = ex.Message;
                _oFabricSCReports = new List<FabricSCReport>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSCReport);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult View_FabricSCRnD(int Type, int buid, int menuid)
        {
          //  int type = 1; //1 For RnD//
            int[] OrderTypes = this.GetOrderTypes(Type);
            int[] FabricPOStatus = new int[] { (int)EnumFabricPOStatus.None, (int)EnumFabricPOStatus.Approved, (int)EnumFabricPOStatus.Received };
            List<Product> oProducts = new List<Product>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //_oFabricSCReports = FabricSCReport.Gets("Select top(150)* from View_FabricSalesContractReport where ordertype in (" + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.CAD + "," + (int)EnumFabricRequestType.Color + "," + (int)EnumFabricRequestType.Labdip + "," + (int)EnumFabricRequestType.Analysis + "," + (int)EnumFabricRequestType.YarnSkein + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sIDs = string.Join(",", OrderTypes.ToList());
            if (string.IsNullOrEmpty(sIDs)) {sIDs="0";}
            _oFabricSCReports = FabricSCReport.Gets("Select top(150)* from View_FabricSalesContractReport where ordertype in (" + sIDs + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") order by SCDate DESC ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oProducts = Product.GetsPermittedProduct(buid, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);


            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)).Where(x => OrderTypes.Contains(x.id)).ToList();
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus)).Where(x => FabricPOStatus.Contains(x.id)).ToList();
            ViewBag.LabdipStatus = EnumObject.jGets(typeof(EnumLabdipOrderStatus));

            var oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Products = oProducts;
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            ViewBag.Type = Type;
            return View(_oFabricSCReports);
        }
        public ActionResult View_FabricSCRnDHLO(int buid, int menuid)
        {
            int type = 2; //1 For HandLoom//
            int[] OrderTypes = this.GetOrderTypes(type);
            int[] FabricPOStatus = new int[] { (int)EnumFabricPOStatus.None, (int)EnumFabricPOStatus.Approved, (int)EnumFabricPOStatus.Received };
            List<Product> oProducts = new List<Product>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oFabricSCReports = FabricSCReport.Gets("Select top(150)* from View_FabricSalesContractReport where ordertype in (" + (int)EnumFabricRequestType.HandLoom + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ")  order by SCDate DESC,SLNo ASC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oProducts = Product.GetsPermittedProduct(buid, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);


            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)).Where(x => OrderTypes.Contains(x.id)).ToList();
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus)).Where(x => FabricPOStatus.Contains(x.id)).ToList();
            ViewBag.LabdipStatus = EnumObject.jGets(typeof(EnumLabdipOrderStatus));

            var oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Products = oProducts;
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            ViewBag.Type = type;
            return View(_oFabricSCReports);
        }
        public ActionResult View_FabricSCRnDHLO_InLab(int buid, int menuid)
        {
            int type = 4; //1 For HandLoom//
            int[] OrderTypes = this.GetOrderTypes(type);
            int[] FabricPOStatus = new int[] { (int)EnumFabricPOStatus.None, (int)EnumFabricPOStatus.Approved, (int)EnumFabricPOStatus.Received };
            List<Product> oProducts = new List<Product>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oFabricSCReports = FabricSCReport.Gets("Select top(150)* from View_FabricSalesContractReport where   LabStatus in (" + (int)EnumLabdipOrderStatus.InLab+ ") ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oProducts = Product.GetsPermittedProduct(buid, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);


            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.cboEnumWarpWeft = EnumObject.jGets(typeof(EnumWarpWeft));
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)).Where(x => OrderTypes.Contains(x.id)).ToList();
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus)).Where(x => FabricPOStatus.Contains(x.id)).ToList();
            ViewBag.LabdipStatus = EnumObject.jGets(typeof(EnumLabdipOrderStatus));

            var oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Products = oProducts;
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            ViewBag.Type = type;
            return View(_oFabricSCReports);
        }
        public ActionResult View_FabricSCRnDBulk(int buid, int menuid)
        {
            int type = 3; //1 For RnD//
            int[] OrderTypes = this.GetOrderTypes(type);
            int[] FabricPOStatus = new int[] { (int)EnumFabricPOStatus.None, (int)EnumFabricPOStatus.Approved, (int)EnumFabricPOStatus.Received };
            List<Product> oProducts = new List<Product>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oFabricSCReports = FabricSCReport.Gets("Select top(50)* from View_FabricSalesContractReport where  isnull(SCDetailType,0) not in (" + (int)EnumSCDetailType.AddCharge + "," + (int)EnumSCDetailType.DeductCharge + ") and  ordertype in (" + (int)EnumFabricRequestType.Buffer + "," + (int)EnumFabricRequestType.Bulk + "," + (int)EnumFabricRequestType.Sample + "," + (int)EnumFabricRequestType.SampleFOC + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oProducts = Product.GetsPermittedProduct(buid, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumFabricRequestType)).Where(x => OrderTypes.Contains(x.id)).ToList();
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus)).Where(x => FabricPOStatus.Contains(x.id)).ToList();
            ViewBag.LabdipStatus = EnumObject.jGets(typeof(EnumLabdipOrderStatus));

            var oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Products = oProducts;
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            ViewBag.Type = type;
            return View(_oFabricSCReports);
        }
        public ActionResult View_FabricSCRnDBulk_Finish(int buid, int menuid)
        {
            int type = 3; //1 For RnD//
            //int[] OrderTypes = this.GetOrderTypes(type);
            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>(); 
            int[] FabricPOStatus = new int[] { (int)EnumFabricPOStatus.None, (int)EnumFabricPOStatus.Approved, (int)EnumFabricPOStatus.Received };
            List<Product> oProducts = new List<Product>();
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            string sOrderTypeIDs = string.Join(",", oFabricOrderSetups.Where(o=>o.IsApplyPO==true).Select(x => (int)x.FabricOrderType).ToList());

            if (string.IsNullOrEmpty(sOrderTypeIDs))
                sOrderTypeIDs = "0";

            _oFabricSCReports = FabricSCReport.Gets("Select top(150)* from View_FabricSalesContractReport where isnull(SCDetailType,0) not in (" + (int)EnumSCDetailType.AddCharge + "," + (int)EnumSCDetailType.DeductCharge + ") and ordertype in (" + sOrderTypeIDs + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") order by SCDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oProducts = Product.GetsPermittedProduct(buid, EnumModuleName.Fabric, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);

       

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.OrderTypes = oFabricOrderSetups.Where(o => o.IsApplyPO == true).ToList();// EnumObject.jGets(typeof(EnumFabricRequestType)).Where(x => OrderTypes.Contains(x.id)).ToList();
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus)).Where(x => FabricPOStatus.Contains(x.id)).ToList();
            ViewBag.LabdipStatus = EnumObject.jGets(typeof(EnumLabdipOrderStatus));

            var oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriorityLevels = Enum.GetValues(typeof(EnumPriorityLevel)).Cast<EnumPriorityLevel>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.FinishTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Finish).ToList();
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.FabricDesigns = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.FabricDesign).ToList();
            ViewBag.LightSources = LightSource.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Products = oProducts;
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FabricPOSetup = oFabricPOSetup;
            ViewBag.BUID = buid;
            ViewBag.BusinessUnit = oBusinessUnit;
            ViewBag.Type = type;
            return View(_oFabricSCReports);
        }
        #endregion

        #region FN Execution Order Receive
        private string GetSQLFabricDeliveryChallanForReceive()
        {
            //string sSQL = "SELECT * FROM View_FabricDeliveryChallan WHERE DisburseBy>0 AND FDOID IN " +
            //    " (SELECT FDOID FROM FabricDeliveryOrderDetail WHERE FEOID IN (SELECT FEOID FROM FNExecutionOrder Where ApproveByID<>0)) " +
            //    " AND FDCID IN (SELECT FDCID FROM FabricDeliveryChallanDetail WHERE FDCDID NOT IN (SELECT FDCDID FROM FNOrderFabricReceive)) ";

            string sSQL = "SELECT * FROM View_FabricDeliveryChallan WHERE DisburseBy>0 And FDCID IN " +
                        " (SELECT DISTINCT(FDCID) FROM FabricDeliveryChallanDetail Where FDODID IN " +
                        " (SELECT FDODID FROM FabricDeliveryOrderDetail WHERE FEOID IN " +
                        " (SELECT FEOID FROM FNExecutionOrder Where ApproveByID<>0))" +
                        " And FDCDID Not In (Select FDCDID from FNOrderFabricReceive Where FDCDID>0))";

            return sSQL;
        }
        //public ActionResult ViewFNExecutionOrderReceives(int menuid)
        //{
        //    this.Session.Remove(SessionInfo.MenuID);
        //    this.Session.Add(SessionInfo.MenuID, menuid);
        //    _oFNExecutionOrders = new List<FNExecutionOrder>();

        //    string sSQL = "Select * from View_FNExecutionOrder Where ApproveByID=0 Order by FNExOID DESC";
        //    _oFNExecutionOrders = FNExecutionOrder.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

        //    int[] arrFPT = { 0, 1, 2 };
        //    //ViewBag.FabricProcessTypes = Enum.GetValues(typeof(EnumFabricProcessType)).Cast<EnumFabricProcessType>().Where(x => arrFPT.Contains((int)x)).Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
        //    sSQL = "SELECT * FROM FabricProcess WHERE ProcessType=" + (int)EnumFabricProcess.Process;
        //    ViewBag.FabricProcessTypes = FabricProcess.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    return View(_oFNExecutionOrders);
        //}
        public ActionResult ViewFNOrderFabricReceive(int nId, int nFabricID, int buid)
        {
            _oFabricSCReports = new List<FabricSCReport>();
            List<FNOrderFabricReceive> oFNExeOFRs = new List<FNOrderFabricReceive>();
            string sSQL = "";
            if (nId > 0)
            {
                _oFabricSCReport = _oFabricSCReport.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oFabricSCReport.FabricSalesContractDetailID > 0)
                {
                    sSQL = "Select * from View_FNOrderFabricReceive Where  FSCDID=" + _oFabricSCReport.FabricSalesContractDetailID + "";
                    oFNExeOFRs = FNOrderFabricReceive.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }

            ViewBag.FNExeOFRs = oFNExeOFRs;
            ViewBag.FSCDID = nId;

            #region Received Stores
            List<WorkingUnit> oReceivedStores = new List<WorkingUnit>();
            oReceivedStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricSalesContract, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.WUs = oReceivedStores; 
            return View(_oFabricSCReport);
        }
      
        [HttpPost]
        public JsonResult SaveFNExeOFR(FNOrderFabricReceive oFNEOFR)
        {
            try
            {
                FabricSCReport oFEO = new FabricSCReport();
                if (oFNEOFR.FEOID > 0)
                {
                    oFEO = oFEO.Get(oFNEOFR.FEOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //oFNEOFR.FabricID = oFEO.FabricID;
                }


                if (oFNEOFR.FNOrderFabricReceiveID <= 0)
                {
                    oFNEOFR = oFNEOFR.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFNEOFR = oFNEOFR.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNEOFR = new FNOrderFabricReceive();
                oFNEOFR.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNEOFR);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FNOrderTransferSave(FNOrderFabricReceive oFNOrderFabricReceive)
        {
            FNOrderFabricReceive _oFNOrderFabricReceive = new FNOrderFabricReceive();
            List<FNOrderFabricReceive> _oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
            List<FNOrderFabricTransfer> _oFNOrderFabricTransfers = new List<FNOrderFabricTransfer>();
            FNOrderFabricTransfer oFNOrderFabricTransfer = new FNOrderFabricTransfer();
            try
            {
                foreach (var oItem in oFNOrderFabricReceive.oFNOrderFabricReceives)
                {
                    oFNOrderFabricTransfer = new FNOrderFabricTransfer();
                    oFNOrderFabricTransfer.FNOrderFabricTransferID = 0;
                    oFNOrderFabricTransfer.FNOrderFabricReceiveID_From = oItem.FNOrderFabricReceiveID_from;
                    oFNOrderFabricTransfer.FNOrderFabricReceiveID_To = 0;
                    oFNOrderFabricTransfer.FSCDID_From = oItem.FSCDID_from;
                    oFNOrderFabricTransfer.FSCDID_To = oItem.FSCDID_To;
                    oFNOrderFabricTransfer.Qty = oItem.Qty_from;
                    _oFNOrderFabricTransfers.Add(oFNOrderFabricTransfer);
                }

                //_oFNOrderFabricReceives = _oFNOrderFabricReceive.SaveList(oFNOrderFabricReceive.oFNOrderFabricReceives, (int)Session[SessionInfo.currentUserID]);
                _oFNOrderFabricTransfers = oFNOrderFabricTransfer.SaveList(_oFNOrderFabricTransfers, (int)Session[SessionInfo.currentUserID]);
                _oFNOrderFabricReceives = FNOrderFabricReceive.Gets("SELECT * FROM View_FNOrderFabricReceive WHERE ReceiveBy<>0 AND FSCDID = " + oFNOrderFabricReceive.FSCDID + " ORDER BY FNOrderFabricReceiveID", (int)Session[SessionInfo.currentUserID]);
            
            }
            catch (Exception ex)
            {
                _oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
                _oFNOrderFabricReceive = new FNOrderFabricReceive();
                _oFNOrderFabricReceive.ErrorMessage = ex.Message;
                _oFNOrderFabricReceives.Add(_oFNOrderFabricReceive);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNOrderFabricReceives);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult FNOrderReturnSave(FNOrderFabricReceive oFNOrderFabricReceive)
        {
            FNOrderFabricReceive _oFNOrderFabricReceive = new FNOrderFabricReceive();
            List<FNOrderFabricReceive> _oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
            List<FNOrderFabricTransfer> _oFNOrderFabricTransfers = new List<FNOrderFabricTransfer>();
            FNOrderFabricTransfer oFNOrderFabricTransfer = new FNOrderFabricTransfer();
            try
            {
                foreach (var oItem in oFNOrderFabricReceive.oFNOrderFabricReceives)
                {
                    oFNOrderFabricTransfer = new FNOrderFabricTransfer();
                    oFNOrderFabricTransfer.FNOrderFabricTransferID = 0;
                    oFNOrderFabricTransfer.FNOrderFabricReceiveID_From = oItem.FNOrderFabricReceiveID_from;
                    oFNOrderFabricTransfer.FNOrderFabricReceiveID_To = 0;
                    oFNOrderFabricTransfer.FSCDID_From = oItem.FSCDID_from;
                    oFNOrderFabricTransfer.Qty = oItem.Qty_from;
                    _oFNOrderFabricTransfers.Add(oFNOrderFabricTransfer);
                }

                _oFNOrderFabricTransfers = oFNOrderFabricTransfer.ReturnFabrics(_oFNOrderFabricTransfers, (int)Session[SessionInfo.currentUserID]);
                _oFNOrderFabricReceives = FNOrderFabricReceive.Gets("SELECT * FROM View_FNOrderFabricReceive WHERE ReceiveBy<>0 AND FSCDID = " + oFNOrderFabricReceive.FSCDID + " ORDER BY FNOrderFabricReceiveID", (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
                _oFNOrderFabricReceive = new FNOrderFabricReceive();
                _oFNOrderFabricReceive.ErrorMessage = ex.Message;
                _oFNOrderFabricReceives.Add(_oFNOrderFabricReceive);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNOrderFabricReceives);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFNExeOFR(FNOrderFabricReceive oFNEOFR)
        {
            try
            {
                if (oFNEOFR.FNOrderFabricReceiveID <= 0) { throw new Exception("Please select a valid item from list."); }
                oFNEOFR = oFNEOFR.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNEOFR = new FNOrderFabricReceive();
                oFNEOFR.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNEOFR.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReceiveFNExeOFR(FNOrderFabricReceive oFNEOFR)
        {
            FNOrderFabricReceive _oFNEOFR = new FNOrderFabricReceive();
            List<FNOrderFabricReceive> oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
            try
            {
                oFNOrderFabricReceives = FNOrderFabricReceive.Gets("SELECT * FROM View_FNOrderFabricReceive WHERE FNOrderFabricReceiveID IN(" + oFNEOFR.ErrorMessage + ") ORDER BY FNOrderFabricReceiveID", ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFNOrderFabricReceives = _oFNEOFR.Receive(oFNOrderFabricReceives, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFNOrderFabricReceives = FNOrderFabricReceive.Gets("SELECT * FROM View_FNOrderFabricReceive WHERE FSCDID = " + oFNEOFR.FSCDID + " ORDER BY FNOrderFabricReceiveID", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNEOFR = new FNOrderFabricReceive();
                oFNEOFR.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNOrderFabricReceives);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public string MakeSQLFNOFRStatement(FNOrderFabricReceive oFNOrderFabricReceive)
        {
            return "";
        }

        #endregion

        #region Reports
        public void Print_ReportRnDXL(string sTempString, int BUID)
        {

            _oFabricSCReports = new List<FabricSCReport>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            oFabricSCReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oFabricSCReport);
            _oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            bool bRateView = false;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Export Excel
            int nSL = 0, nRowIndex = 2, colIndex = 2, nMinColumn=2, nMaxColumn = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("PO Report");
                sheet.Name = "PO Report";

                #region Coloums

                string[] columnHead = new string[] {   "SL No", "PO Issue Date","Receive Date", "Buyer", "PO No", "MKT Ref", "Dispo No", "PO Qty(Y)","Construction","Weave","Finish","Approve Ref", "Buyer Ref.","Composition","Fabric type", " Style No", "Color","Width", "Fabric Design Name", " Order Type","Delivery Date", "Delivery Date","Mkt Team","Concern Person" };
                int[] colWidth = new int[] { 10, 12, 12, 18, 15, 18, 15, 12, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 18, 15, 20, 15, 18, 18};

                for (int i = 0; i < colWidth.Length; i++)
                {
                    sheet.Column(colIndex).Width = colWidth[i];
                    colIndex++;
                }
                nMaxColumn = colIndex-1;

                #endregion
                
                #region Report Header
                cell = sheet.Cells[nRowIndex, nMinColumn, nRowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nMinColumn, nRowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nMinColumn, nRowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = "PO Register "; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nMinColumn, nRowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Column
                colIndex = nMinColumn;
                for (int i = 0; i < columnHead.Length; i++)
                {
                    cell = sheet.Cells[nRowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex++;
                }
                nRowIndex++;
                #endregion

                #region Report Data

               
                foreach (FabricSCReport oItem in _oFabricSCReports)
                {
                    colIndex = nMinColumn;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.SCDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.FabricReceiveDateStr; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = "" + oItem.SCNoFull; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.FabricNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.ExeNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.FabricWeaveName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.FinishTypeName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.ApproveByName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.BuyerReference; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.ProcessTypeName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.ColorInfo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.FabricWidth; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.FabricDesignName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.OrderTypeSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.DeliveryDate_PPSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = (oItem.OrderType == (int)EnumFabricRequestType.Bulk) ? oItem.DeliveryDate_FullSt : oItem.DeliveryDate_PPSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.MKTGroup; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;

                }

                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PORegisterRnD.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }
        public void Print_ReportRnDXL_Finish(string sTempString, int BUID)
        {

            _oFabricSCReports = new List<FabricSCReport>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            oFabricSCReport.ErrorMessage = sTempString;
            //string sSQL = MakeSQL(oFabricSCReport);
            //_oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (sTempString != null || sTempString != "null")
            {
                string sSQL = MakeSQL(oFabricSCReport);
                _oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _oFabricSCReports = FabricSCReport.Gets("Select top(150)* from View_FabricSalesContractReport where ordertype in (" + (int)EnumFabricRequestType.Buffer + "," + (int)EnumFabricRequestType.Bulk + "," + (int)EnumFabricRequestType.Sample + "," + (int)EnumFabricRequestType.SampleFOC + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            bool bRateView = false;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Export Excel
            int nSL = 0, nRowIndex = 2, colIndex = 2, nMinColumn = 2, nMaxColumn = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("PO Report");
                sheet.Name = "PO Report";



                #region Coloums
                string[] columnHead = new string[] 
                { 
                    "SL No","PO Issue Date", "Receive Date", "Dispo No",  "PO No", "PI No", "Mkt Team","Buyer Name", "Garments Name","Construction",  "Composition", 
                    "Fabric Code", "Fabric type","Wave Type", "Finish Type","Lab Dip","Color", "Color Qty", "Concern Person","MKT Ref", "Style No","Width","Production Status"
                   
                };
                int[] colWidth = new int[] 
                { 
                     8,15,15,18, 16,18, 18, 30, 30,30, 30,25,15,18,18,15,15,18,20,30,15,15,18
                };


                for (int i = 0; i < colWidth.Length; i++)
                {
                    sheet.Column(colIndex).Width = colWidth[i];
                    colIndex++;
                }
                nMaxColumn = colIndex - 1;

                #endregion



                #region Report Header
                cell = sheet.Cells[nRowIndex, nMinColumn, nRowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nMinColumn, nRowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nMinColumn, nRowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = "PO Register "; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nMinColumn, nRowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Column
                colIndex = nMinColumn;
                for (int i = 0; i < columnHead.Length; i++)
                {
                    cell = sheet.Cells[nRowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex++;
                }
                nRowIndex++;
                #endregion

                #region Report Data


                foreach (FabricSCReport oItem in _oFabricSCReports)
                {
                    colIndex = nMinColumn;
                    nSL++;
                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.SCDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.FabricReceiveDateStr; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.ExeNo; cell.Style.Font.Bold = false;//oItem.HLReference
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = "" + oItem.SCNoFull; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.MKTGroup; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.ProcessTypeName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.FabricWeaveName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.FinishTypeName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.ColorInfo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.BuyerReference; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                   
                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = oItem.FabricWidth; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                   
                    nRowIndex++;

                }

                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PORegisterRnD.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }
        public ActionResult Print_ReportRnDPDF_Finish(string sTempString, int BUID)
        {

            _oFabricSCReports = new List<FabricSCReport>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            oFabricSCReport.ErrorMessage = sTempString;
            //string sSQL = MakeSQL(oFabricSCReport);
            //_oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (sTempString != null || !sTempString.Contains("null"))
            {
                string sSQL = MakeSQL(oFabricSCReport);
                _oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _oFabricSCReports = FabricSCReport.Gets("Select top(150)* from View_FabricSalesContractReport where ordertype in (" + (int)EnumFabricRequestType.Buffer + "," + (int)EnumFabricRequestType.Bulk + "," + (int)EnumFabricRequestType.Sample + "," + (int)EnumFabricRequestType.SampleFOC + " ) and Currentstatus in (" + (int)EnumFabricPOStatus.Approved + ") ", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            //List<Company> oCompanys = new List<Company>();
            //oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);


            rptReportRnD oReport = new rptReportRnD();
            byte[] abytes = oReport.PrepareReport(oCompany, oBusinessUnit, _oFabricSCReports);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintHandLoomSwatch(int nId, int nBUID, double nts)
        {
            FabricSCReport oFSR = new FabricSCReport();

            string sQuery = "Select * from View_FabricSalesContractReport Where FabricSalesContractDetailID=" + nId + "";
            var results = FabricSCReport.Gets(sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (results.Any() && results.First().FabricSalesContractDetailID > 0)
                oFSR = results.First();


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptHandLoomSwatchInfo oReport = new rptHandLoomSwatchInfo();
            byte[] abytes = oReport.PrepareReport(oFSR, oCompany, oBU);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintHandLoomDelivery(int nId, int nBUID, double nts)
        {
            FabricSCReport oFSR = new FabricSCReport();

            string sQuery = "Select * from View_FabricSalesContractReport Where FabricSalesContractDetailID=" + nId + "";
            var results = FabricSCReport.Gets(sQuery, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (results.Any() && results.First().FabricSalesContractDetailID > 0)
                oFSR = results.First();


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptHandLoomDeliveryFormat oReport = new rptHandLoomDeliveryFormat();
            byte[] abytes = oReport.PrepareReport(oFSR, oBU, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintFabricAnalysis(int nId, int nFabricID, int buid)
        {
            Fabric oFabric = new Fabric();
            FabricRnD oFabricRnD = new FabricRnD();
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            oFabric = oFabric.Get(nFabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (buid > 0) oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            string sSQL = "";
            FabricSCReport oFSR = new FabricSCReport();
            if (nId > 0)
            {
                oFSR = oFSR.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                sSQL = "Select * from View_FabricSalesContractReport Where FabricID=" + oFabric.FabricID + " and isnull(OrderType,0) in (" + ((int)EnumFabricRequestType.Analysis).ToString() + "," + ((int)EnumFabricRequestType.CAD).ToString() + ","  +((int)EnumFabricRequestType.HandLoom).ToString() + ")";
                _oFabricSCReports = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oFabricSCReports.Any()) { oFSR = _oFabricSCReports[0];}
            }
            if (oFSR.FabricSalesContractDetailID<=0)
            {
                oFSR.FabricNo = oFabric.FabricNo;
                oFSR.FinishType = oFabric.FinishType;
                oFSR.MktAccountID = oFabric.MKTPersonID;
                oFSR.FabricWeave = oFabric.FabricWeave;
                oFSR.FabricWeaveName = oFabric.FabricWeaveName;
                oFSR.FabricWidth = oFabric.FabricWidth;
                oFSR.FinishTypeName = oFabric.FinishTypeName;
                oFSR.FabricDesignName = oFabric.FabricDesignName;
                oFSR.BuyerName = oFabric.BuyerName;
                oFSR.StyleNo = oFabric.StyleNo;
                oFSR.BuyerReference = oFabric.BuyerReference;
            }

            oFabricRnD = oFabricRnD.GetBy(oFSR.FabricSalesContractDetailID, oFabric.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //rptFabricAnalysis oReport = new rptFabricAnalysis();
            //byte[] abytes = oReport.PrepareReport(oFabric, oFSR,oFabricRnD, oFabricPOSetup, oCompany, oBusinessUnit);
            rptFabricAnalysisFormatTwo oReport = new rptFabricAnalysisFormatTwo();
            byte[] abytes = oReport.PrepareReport(oFabric, oFSR, oFabricRnD, oFabricPOSetup, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintFNEOFabricReceiveStatement(string sParam,double nts)
        {
            FNOrderFabricReceive oFNOrderFabricReceive = new FNOrderFabricReceive();
            List<FNOrderFabricReceive> oFNOrderFabricReceives = new List<FNOrderFabricReceive>();
            FNOrderFabricTransfer oFNOrderFabricTransfer = new FNOrderFabricTransfer();
            List<FNOrderFabricTransfer> oInQtys = new List<FNOrderFabricTransfer>();
            List<FNOrderFabricTransfer> oOutQtys = new List<FNOrderFabricTransfer>();
            List<FNOrderFabricTransfer> oReturnQtys = new List<FNOrderFabricTransfer>();
            List<FNOrderFabricTransfer> oConQtys = new List<FNOrderFabricTransfer>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            string sFNOrderFabricReceiveIDs = Convert.ToString(sParam.Split('~')[0]);
            //oFNOrderFabricReceive.FSCDID = Convert.ToInt32(sParam.Split('~')[1]);          

            string sSQL = "";
            try
            {
                //sSQL = "SELECT * FROM View_FNOrderFabricTransfer WHERE FNOrderFabricReceiveID_To IN(" + oFNOrderFabricReceive.FNOrderFabricReceiveID + ") AND FNOrderFabricReceiveID_From NOT IN("+oFNOrderFabricReceive.FNOrderFabricReceiveID+")";
                //oInQtys = FNOrderFabricTransfer.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //sSQL = "SELECT * FROM View_FNOrderFabricTransfer WHERE FNOrderFabricReceiveID_From IN(" + oFNOrderFabricReceive.FNOrderFabricReceiveID + ") AND FNOrderFabricReceiveID_To NOT IN(" + oFNOrderFabricReceive.FNOrderFabricReceiveID + ")";
                //oOutQtys = FNOrderFabricTransfer.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //sSQL = "SELECT * FROM View_FNOrderFabricTransfer WHERE FNOrderFabricReceiveID_From IN(" + oFNOrderFabricReceive.FNOrderFabricReceiveID + ") AND FNOrderFabricReceiveID_To IN("+oFNOrderFabricReceive.FNOrderFabricReceiveID+")";
                //oReturnQtys = FNOrderFabricTransfer.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sSQL = "SELECT * FROM View_FNOrderFabricReceive WHERE FNOrderFabricReceiveID IN(" + sFNOrderFabricReceiveIDs + ") ORDER BY FNOrderFabricReceiveID";
                oFNOrderFabricReceives = FNOrderFabricReceive.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSCReport = FabricSCReport.Gets("SELECT * FROM View_FabricSalesContractReport WHERE FabricSalesContractID = (SELECT FabricSalesContractID FROM FabricSalesContractDetail WHERE FabricSalesContractDetailID=" + oFNOrderFabricReceives[0].FSCDID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID).FirstOrDefault();
            }
            catch(Exception e)
            {
                oFNOrderFabricReceive = new FNOrderFabricReceive();
                oFNOrderFabricReceive.ErrorMessage = e.Message;
                oFNOrderFabricReceives.Add(oFNOrderFabricReceive);
            }

            if (oFNOrderFabricReceives.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBusinessUnit = new BusinessUnit();
                rptFNOrderFabricReceiveStatement oReport = new rptFNOrderFabricReceiveStatement();

                byte[] abytes = oReport.PrepareReport(oFNOrderFabricReceives, oInQtys, oOutQtys, oReturnQtys, oConQtys, oCompany, "",oFabricSCReport);

                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }

        }
        #endregion
    }
}
