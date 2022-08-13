using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region Print
    
    public class Print : BusinessObject
    {
        #region  Constructor
        public Print()
        {
            PrintID=0;
            ReportCode="";
            ReportName="";
            ObjectID=0;
            NumberOfPrint=0;
            DBServerDateTime = DateTime.Now;
            ErrorMessage = "";
        }
        #endregion

        #region Properties

        
        public int PrintID { get; set; }
        
        public string ReportCode { get; set; }
        
        public string ReportName { get; set; }
        
        public int ObjectID { get; set; }
        
        public int NumberOfPrint { get; set; }
        
        public DateTime DBServerDateTime { get; set; }
        
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public string DateInString
        {
            get
            {
                if (DBServerDateTime == null || DBServerDateTime == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return DBServerDateTime.ToString("dd MMM yyyy");
                }
            }
        }

        #endregion

        #region Functions

        public Print IU(long nUserID)
        {
            return Print.Service.IU(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPrintService Service
        {
            get { return (IPrintService)Services.Factory.CreateService(typeof(IPrintService)); }
        }
        #endregion
    }
    #endregion



    #region IPrint interface
    
    public interface IPrintService
    {
        
        Print IU(Print oPrint, long nUserID);
    }
    #endregion
}