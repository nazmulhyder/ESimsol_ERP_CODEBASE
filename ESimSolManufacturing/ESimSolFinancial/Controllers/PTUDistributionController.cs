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
    public class PTUDistributionController : Controller
    {
        #region Declaration
        PTUDistribution _oPTUDistribution = new PTUDistribution();
        List<PTUDistribution> _oPTUDistributions = new List<PTUDistribution>();
        string _sErrorMessage = "";
        #endregion

        #region PTUDistribution

        [HttpPost]
        public JsonResult PTUToPTU_Transfer(PTUDistribution oPTUDistribution)
        {
            try
            {
                _oPTUDistribution = oPTUDistribution.PTUToPTU_Transfer(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oPTUDistribution = new PTUDistribution();
                _oPTUDistribution.ErrorMessage = "Invalid entry Order No";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPTUDistribution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


         [HttpPost]
        public JsonResult Gets(PTUDistribution oPTUDistribution)
          {
             
           
              _oPTUDistributions = new List<PTUDistribution>();
              _oPTUDistribution = new PTUDistribution();
              try
              {
                  string sSQL = "SELECT * FROM View_PTUDistribution";
                  string sReturn = "";
                  if (!String.IsNullOrEmpty(oPTUDistribution.LotNo))
                  {
                      oPTUDistribution.LotNo = oPTUDistribution.LotNo.Trim();
                      Global.TagSQL(ref sReturn);
                      sReturn = sReturn + "LotNo like '%" + oPTUDistribution.LotNo + "%'";
                  }
                  if (oPTUDistribution.PTUID>0)
                  {
                     
                      Global.TagSQL(ref sReturn);
                      sReturn = sReturn + "PTUID=" + oPTUDistribution.PTUID;
                  }
                  if (oPTUDistribution.LotID > 0)
                  {

                      Global.TagSQL(ref sReturn);
                      sReturn = sReturn + "LotID=" + oPTUDistribution.LotID;
                  }
                  sSQL = sSQL + "" + sReturn;
                  _oPTUDistributions = PTUDistribution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
               

              }
              catch (Exception ex)
              {
                  _oPTUDistributions = new List<PTUDistribution>();
              }
              JavaScriptSerializer serializer = new JavaScriptSerializer();
              string sjson = serializer.Serialize(_oPTUDistributions);
              return Json(sjson, JsonRequestBehavior.AllowGet);
          }
        
        #endregion

        #region Search
       
        #endregion
        #region GetCompanyLogo
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