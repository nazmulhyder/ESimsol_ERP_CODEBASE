using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSolFinancial.Controllers;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
 
using ESimSol.Reports;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;

namespace ESimSolFinancial.Controllers
{

    public class PlanAnalysisController : Controller
    {

        #region Declartion
        PlanAnalysis _oPlanAnalysis = new PlanAnalysis();
        List<PlanAnalysis> _oPlanAnalysises = new List<PlanAnalysis>();
        List<ObjectWithProperties> _oObjectWithProperties = new List<ObjectWithProperties>();

        #endregion

        #region Method
        public double FindGapInPercent(List<PlanAnalysis> oPlanAnalysis)
        {
            foreach(PlanAnalysis oItem in oPlanAnalysis)
            {
                if(oItem.PlanDate == DateTime.Today) //find gap for  Current Date
                {
                    double total = ((double)oItem.TotalExecutionStepQty * 100) / (double)(oItem.TotalPlanQty);
                    return Math.Round(100-total,2);
                }
            }
            if (oPlanAnalysis[oPlanAnalysis.Count - 1].PlanDate < DateTime.Today)//if current date will be greater than today then calculate total:total
            {
                double total = ((double)oPlanAnalysis[oPlanAnalysis.Count - 1].TotalExecutionStepQty * 100) / (double)(oPlanAnalysis[oPlanAnalysis.Count - 1].TotalPlanQty);
                return Math.Round(100-total, 2);
            }

            return 0;
        }
        public DateTime GetApproximateFinishDate(List<PlanAnalysis> oPlanAnalysis)
        {
            int nDay = 0;
            if(oPlanAnalysis[0].ShortQty>0)
            {
            foreach (PlanAnalysis oItem in oPlanAnalysis)
            {
                nDay++;
                if (oItem.PlanDate == DateTime.Today)
                {
                    double nAvgProductionPerDay = ((double)oItem.TotalExecutionStepQty) / (double)(nDay);//calculate avgProduction per day
                    int nExtraDay = Convert.ToInt32(oPlanAnalysis[0].ShortQty / nAvgProductionPerDay);//find Extra days                    
                    return oPlanAnalysis[oPlanAnalysis.Count-1].PlanDate.AddDays(nExtraDay); // add extra days with plan last date
                }
            }
            }
            return DateTime.Today;
        }
        public List<PlanAnalysis> GetRequiredPerDayTotalQty(List<PlanAnalysis> oPlanAnalysis)
        {
            if((oPlanAnalysis[oPlanAnalysis.Count-1].PlanDate.Subtract(oPlanAnalysis[0].MaxExecutionDate).TotalDays)>0)
            {
                                                                                        //calculate remaining Qty                                       /           Calculte Execution Days = required per day total
                oPlanAnalysis[0].RequiredPerDayTotalQty =  (oPlanAnalysis[oPlanAnalysis.Count-1].TotalPlanQty - oPlanAnalysis[0].TotalQtyOfMaxExecutionDate)/(oPlanAnalysis[oPlanAnalysis.Count-1].PlanDate.Subtract(DateTime.Today).TotalDays);
            }
            else
            {
                oPlanAnalysis[0].RequiredPerDayTotalQty = oPlanAnalysis[0].ShortQty;
            }
            return oPlanAnalysis;
        }
        #endregion

        #region Plan Analysis View
        public ActionResult ViewPlanAnalysis(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oPlanAnalysis = new PlanAnalysis();
         //   _oObjectWithProperties = MakedynamicObjet();
            ViewBag.StatusList = EnumObject.jGets(typeof(EnumGUProductionOrderStatus));
            string sSql = "SELECT * FROM View_User WHERE UserID IN (SELECT Distinct ApprovedBy FROM View_CostSheet)";
            ViewBag.Employees = Employee.Gets(EnumEmployeeDesignationType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Users = ESimSol.BusinessObjects.User.GetsBySql(sSql, (int)Session[SessionInfo.currentUserID]);
            return View(_oPlanAnalysis);
        }
        #endregion

        #region HTTP GET GetProductionStep
        [HttpGet]
        public JsonResult GetProductionStep(int id) // id is Production Order id
        {
            List<ProductionStep> oProductionSteps = new List<ProductionStep>();
            try
            {
                //string sSQL = "SELECT * FROM ProductionStep WHERE ProductionStepID IN (SELECT ProductionStepID FROM View_ProductionTracingUnitDetail WHERE ProductionTracingUnitID IN (SELECT ProductionTracingUnitID FROM ProductionTracingUnit WHERE GUProductionOrderID = " + id + "))";
                //Change for Faruk vai order
                string sSQL = "SELECT * FROM ProductionStep WHERE ProductionStepID IN (SELECT ProductionStepID from GUProductionProcedure WHERE GUProductionOrderID = "+id+")";
                oProductionSteps = ProductionStep.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                ProductionStep oProductionStep = new ProductionStep();
                oProductionStep.ErrorMessage = ex.Message;
                oProductionSteps.Add(oProductionStep);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionSteps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region HTTP GET GetPlanAnalysis
        [HttpGet]
        public JsonResult GetPlanAnalysis(int id, int StepID) // id is Production Order id
        {
            _oPlanAnalysises = new List<PlanAnalysis>();
            try
            {
                _oPlanAnalysises = PlanAnalysis.Gets(StepID, id, (int)Session[SessionInfo.currentUserID]);
                if (_oPlanAnalysises.Count > 0)
                {
                    _oPlanAnalysises[0].GapInPercent = FindGapInPercent(_oPlanAnalysises);
                    if(_oPlanAnalysises[0].GapInPercent<=0) //if not get gap the set 0 & 0
                    {
                        _oPlanAnalysises[0].GapInPercent = 0;
                        _oPlanAnalysises[0].ShortQty = 0;
                    }
                    else
                    {                        
                        _oPlanAnalysises[0].ShortQty =(_oPlanAnalysises[_oPlanAnalysises.Count-1].TotalPlanQty * (_oPlanAnalysises[0].GapInPercent/100));
                    }
                    _oPlanAnalysises[0].ApproximateFinishDate = GetApproximateFinishDate(_oPlanAnalysises);
                    _oPlanAnalysises = GetRequiredPerDayTotalQty(_oPlanAnalysises);
                } 
            }
            catch (Exception ex)
            {
                _oPlanAnalysis = new PlanAnalysis();
                _oPlanAnalysis.ErrorMessage = ex.Message;
                _oPlanAnalysises.Add(_oPlanAnalysis);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPlanAnalysises);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Print Plan Execution Ratio
        public ActionResult PrintPlanExecutionRatio(int id, int StepID, string StepName)
        {
            GUProductionOrder oGUProductionOrder = new GUProductionOrder();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            oGUProductionOrder = oGUProductionOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            oGUProductionOrder.TechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(oGUProductionOrder.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            oGUProductionOrder.PlanAnalysises = PlanAnalysis.Gets(StepID, id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oGUProductionOrder.Company = oCompany;
            rptPlanExecutionRatio oReport = new rptPlanExecutionRatio();
            byte[] abytes = oReport.PrepareReport(oGUProductionOrder,  StepName);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region
        private List<ObjectWithProperties> MakedynamicObjet()
        {
            var obj1 = new ObjectWithProperties();
            obj1["test"] = 100;
            var obj2 = new ObjectWithProperties();
            obj2["test"] = 200;
            var obj3 = new ObjectWithProperties();
            obj3["test"] = 150;
            var objects = new List<ObjectWithProperties>(new ObjectWithProperties[] { obj1, obj2, obj3 });

            // filtering:
            Console.WriteLine("Filtering:");
            var filtered = from obj in objects
                           where (int)obj["test"] >= 150
                           select obj;
            foreach (var obj in filtered)
            {
                Console.WriteLine(obj["test"]);
            }
            _oObjectWithProperties = objects;
            //var x = _oObjectWithProperties[0].GetType().GetProperty("test");
            //int y = x.GetValue();
            // sorting:
            //Console.WriteLine("Sorting:");
            //Comparer<int> c = new Comparer<int>("test");
            //objects.Sort(c);
            //foreach (var obj in objects)
            //{
            //    Console.WriteLine(obj["test"]);
            //}

            return _oObjectWithProperties;
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

        //AdvanceSearch
    }

    class ObjectWithProperties
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();

        public object this[string name]
        {
            get
            {
                if (properties.ContainsKey(name))
                {
                    return properties[name];
                }
                return null;
            }
            set
            {
                properties[name] = value;
            }
        }

    }
}