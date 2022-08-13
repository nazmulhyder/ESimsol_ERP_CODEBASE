using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region PLineConfigure

    public class PLineConfigure : BusinessObject
    {
        public PLineConfigure()
        {
            PLineConfigureID = 0;
            ProductionUnitID = 0;
            LineNo = "";
            Remarks = "";
            RefName = "";
            PUShortName = "";
            RefShortName = "";//BU Name
            PUName = "";
            MachineQty = 0;
            RefID = 0;
            ApproxPlanStartDate = DateTime.Today;
            ErrorMessage = "";
        }

        #region Properties
        public int PLineConfigureID { get; set; }
        public int ProductionUnitID { get; set; }
        public string LineNo { get; set; }
        public string RefName { get; set; }
        public string Remarks { get; set; }
        public string RefShortName { get; set; }
        public string PUShortName { get; set; }
        public string PUName { get; set; }             
        public int MachineQty { get;set; }
        public int RefID { get; set; }
        public DateTime ApproxPlanStartDate { get; set; }
        public string ErrorMessage { get; set; }

        #region Derived Property
        public DateTime PlanStartDate
        {
            get 
            {
                if (this.ApproxPlanStartDate <= DateTime.Today)
                {
                    return DateTime.Today.AddDays(1);
                }
                else
                {
                    return this.ApproxPlanStartDate;
                }
            }
        }
        public string PlanStartDateSt
        {
            get
            {
                return this.PlanStartDate.ToString("dd MMM yyyy");
            }
        }
        public string PULineNo
        {
            get
            {
                return "[" + this.PUShortName + "] " + this.LineNo + ", M Qty(" + this.MachineQty.ToString("#,###") + ") Free @ " + this.PlanStartDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #endregion

        #region Functions
        public static List<PLineConfigure> Gets(long nUserID)
        {
            return PLineConfigure.Service.Gets(nUserID);
        }
        public static List<PLineConfigure> Gets(string sSQL, long nUserID)
        {
            return PLineConfigure.Service.Gets(sSQL, nUserID);
        }
        public static List<PLineConfigure> Gets(int nPUnitID, long nUserID)
        {
            return PLineConfigure.Service.Gets(nPUnitID, nUserID);
        }
        public PLineConfigure Get(int id, long nUserID)
        {
            return PLineConfigure.Service.Get(id, nUserID);
        }
     
        public PLineConfigure Save(long nUserID)
        {
            return PLineConfigure.Service.Save(this, nUserID);
        }

        public string Delete(long nUserID)
        {
            return PLineConfigure.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPLineConfigureService Service
        {
            get { return (IPLineConfigureService)Services.Factory.CreateService(typeof(IPLineConfigureService)); }
        }
        #endregion
    }
    #endregion


    #region IPLineConfigure interface

    public interface IPLineConfigureService
    {
        PLineConfigure Get(int id, Int64 nUserID);
        List<PLineConfigure> Gets(string sSQL, long nUserID);
        List<PLineConfigure> Gets(int nPUnitID, long nUserID);
        List<PLineConfigure> Gets(Int64 nUserID);
        string Delete(PLineConfigure oPLineConfigure, Int64 nUserID);
        PLineConfigure Save(PLineConfigure oPLineConfigure, Int64 nUserID);
    }
    #endregion
}
