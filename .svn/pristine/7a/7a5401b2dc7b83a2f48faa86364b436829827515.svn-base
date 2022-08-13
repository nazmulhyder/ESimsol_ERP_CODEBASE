
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ESimSolFinancial.Controllers;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;

//using System.Web.Security;

namespace ESimSolFinancial
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static System.Timers.Timer timer = new System.Timers.Timer(10 * 60 * 1000);
        public static System.Timers.Timer timeElapsed = new System.Timers.Timer(5 * 60 * 1000);
        //public static System.Timers.Timer timerOneDay = new System.Timers.Timer(24 * 60 * 60 * 1000);


        public static bool _bIsInitialize = false;
        private static List<MailSetUp> _oMailSetUps = new List<MailSetUp>();
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

            timeElapsed.Enabled = true;
            timeElapsed.Elapsed += new System.Timers.ElapsedEventHandler(MailTimer);

            //timerOneDay.Enabled = true;
            //timerOneDay.Elapsed += new System.Timers.ElapsedEventHandler(MailTimerOneDay);

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "User", action = "LogIn", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            string sessionId = Session.SessionID;
        }

        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            var oIssueMails = MailSetUpController.GetMailSetUp(e.SignalTime);

            _oMailSetUps = new List<MailSetUp>();
            #region Mail Check
            /*Daily Mail*/
            var dailyMails = oIssueMails.Where(x => x.MailType == MailReportingType.Daily).ToList();
            if (dailyMails.Any())
            {
                dailyMails.ForEach(x => { x.LastMailTime = e.SignalTime; });
                _oMailSetUps.AddRange(dailyMails);
            }


            /*Weekly Mail*/
            var weeklyMails = oIssueMails.Where(x => x.MailType == MailReportingType.Weekly && (x.LastMailTime == DateTime.MinValue || (e.SignalTime - x.LastMailTime).TotalDays >= 7)).ToList();
            if (weeklyMails.Any())
            {
                weeklyMails.ForEach(x => { x.LastMailTime = e.SignalTime; });
                _oMailSetUps.AddRange(weeklyMails);
            }

            /*Monthly Mail*/
            var monthlyMails = oIssueMails.Where(x => x.MailType == MailReportingType.Monthly && (x.LastMailTime == DateTime.MinValue || (e.SignalTime.Month > x.LastMailTime.Month && e.SignalTime.Year >= x.LastMailTime.Year))).ToList();
            if (monthlyMails.Any())
            {
                monthlyMails.ForEach(x => { x.LastMailTime = e.SignalTime; });
                _oMailSetUps.AddRange(monthlyMails);
            }

            /*Monthly Mail*/
            var yearlyMails = oIssueMails.Where(x => x.MailType == MailReportingType.Yearly && (x.LastMailTime == DateTime.MinValue || e.SignalTime.Year > x.LastMailTime.Year)).ToList();
            if (yearlyMails.Any())
            {
                yearlyMails.ForEach(x => { x.LastMailTime = e.SignalTime; });
                _oMailSetUps.AddRange(yearlyMails);
            }

            #endregion

            List<MailSetUp> oMailSetUps = new List<MailSetUp>();
            oMailSetUps = _oMailSetUps.Where(x => x.LastMailTime.Minute <= e.SignalTime.Minute).ToList();
            if (oMailSetUps.Count() > 0)
            {
                _oMailSetUps.RemoveAll(x => x.LastMailTime.Minute <= e.SignalTime.Minute);
                MailSetUpController.MailReport(oMailSetUps);
                _bIsInitialize = true;
            }

        }

        static void MailTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_bIsInitialize)
            {
                List<MailSetUp> oMailSetUps = new List<MailSetUp>();
                oMailSetUps = _oMailSetUps.Where(x => x.LastMailTime.Minute <= e.SignalTime.Minute).ToList();
                if (oMailSetUps.Count() > 0)
                {
                    _oMailSetUps.RemoveAll(x => x.LastMailTime.Minute <= e.SignalTime.Minute);
                    MailSetUpController.MailReport(oMailSetUps);
                }
                if (_oMailSetUps.Count() <= 0) { _bIsInitialize = false; }
            }
        }
        //static void MailTimerOneDay(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    var oIssueMails = MailSetUpController.GetMailSetUp(e.SignalTime);

        //    _oMailSetUps = new List<MailSetUp>();
        //    #region Mail Check
        //    /*Daily Mail*/
        //    var dailyMails = oIssueMails.Where(x => x.MailType == MailReportingType.Daily).ToList();
        //    if (dailyMails.Any())
        //    {
        //        dailyMails.ForEach(x => { x.LastMailTime = e.SignalTime; });
        //        _oMailSetUps.AddRange(dailyMails);
        //    }
        //    List<MailSetUp> oMailSetUps = new List<MailSetUp>();
        //    oMailSetUps = _oMailSetUps.Where(x => x.LastMailTime.Minute <= e.SignalTime.Minute).ToList();
        //    if (oMailSetUps.Count() > 0)
        //    {
        //        _oMailSetUps.RemoveAll(x => x.LastMailTime.Minute <= e.SignalTime.Minute);
        //        MailSetUpController.MailReport(oMailSetUps);
        //        _bIsInitialize = true;
        //    }
        //    #endregion
        //}
    }
}