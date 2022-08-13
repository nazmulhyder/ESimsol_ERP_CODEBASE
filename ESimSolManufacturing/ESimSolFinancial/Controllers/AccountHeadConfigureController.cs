using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class AccountHeadConfigureController : Controller
    {
        #region Declaration
        List<AccountHeadConfigureController> _oAccountHeadConfigures = new List<AccountHeadConfigureController>();
        AccountHeadConfigureController _oAccountHeadConfigure = new AccountHeadConfigureController();
        #endregion
    }

}