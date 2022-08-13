using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region FnOrderExecute
    public class FnOrderExecute : BusinessObject
    {
        public FnOrderExecute()
        {
            FnOrderExecuteID = 0;
            FSCDID = 0;
            FNLabdipDetailID = 0;
            ShadeID = 0;
            IssueDate = DateTime.Now;
            FinishWidth = 0;
            NoOfFrame = 0;
            DyeType = "";
            Qty = 0;
            Percentage = 0;
            GreigeWidth = 0;
            GL = 0;
            Note = "";
            QtyOrder = 0;
            FabricSource = "";
            PrepareByName = "";
            ErrorMessage = "";
            
        }

        #region Property
        public int FnOrderExecuteID { get; set; }
        public int FSCDID { get; set; }
        public int FNLabdipDetailID { get; set; }
        public int ShadeID { get; set; }
        public DateTime IssueDate { get; set; }
        public double FinishWidth { get; set; }
        public double NoOfFrame { get; set; }
        public string DyeType { get; set; }
        public double Qty { get; set; }
        public double Percentage { get; set; }
        public double GreigeWidth { get; set; }
        public double GL { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string LDNo { get; set; }
        public double QtyOrder { get; set; }
        public string FabricSource { get; set; }
        public string PrepareByName { get; set; }
        public string IssueDateInString
        {
            get
            {
                return IssueDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<FnOrderExecute> Gets(long nUserID)
        {
            return FnOrderExecute.Service.Gets(nUserID);
        }
        public static List<FnOrderExecute> Gets(string sSQL, long nUserID)
        {
            return FnOrderExecute.Service.Gets(sSQL, nUserID);
        }
        public FnOrderExecute Get(int id, long nUserID)
        {
            return FnOrderExecute.Service.Get(id, nUserID);
        }
        public FnOrderExecute Save(long nUserID)
        {
            return FnOrderExecute.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FnOrderExecute.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFnOrderExecuteService Service
        {
            get { return (IFnOrderExecuteService)Services.Factory.CreateService(typeof(IFnOrderExecuteService)); }
        }
        #endregion
    }
    #endregion

    #region IFnOrderExecute interface
    public interface IFnOrderExecuteService
    {
        FnOrderExecute Get(int id, Int64 nUserID);
        List<FnOrderExecute> Gets(Int64 nUserID);
        List<FnOrderExecute> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FnOrderExecute Save(FnOrderExecute oFnOrderExecute, Int64 nUserID);
    }
    #endregion
}
