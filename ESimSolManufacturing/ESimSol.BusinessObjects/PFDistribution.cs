using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Globalization;


namespace ESimSol.BusinessObjects
{
    public class PFDistribution : BusinessObject
    {
        public PFDistribution()
        {
            PFPDID = 0;
            PETID = 0;
            EmployeeID = 0;
            PFMemberContribution = 0;
            PFAllMemberContribution = 0;
            ProfitAmount = 0;
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            ErrorMessage = "";
        }

        #region Properties

        public int PFPDID { get; set; }
        public int PETID { get; set; }
        public int EmployeeID { get; set; }
        public double PFMemberContribution { get; set; }
        public double PFAllMemberContribution { get; set; }
        public double ProfitAmount { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string ErrorMessage { get; set; }

        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public double  SelfContribution { get; set; }
        public double AllMemberContribution { get; set; }
        public double ProfitAmountX { get; set; }

        #endregion



        #region Functions

        public static PFDistribution Distribute(int EnumBDOperation,  int nPFMCID, Int64 nUserID)
        {
            return PFDistribution.Service.Distribute(EnumBDOperation, nPFMCID, nUserID);
        }
        public static List<PFDistribution> Distributes(int EnumBDOperation, int nPFMCID, Int64 nUserID)
        {
            return PFDistribution.Service.Distributes(EnumBDOperation, nPFMCID, nUserID);
        }
        public static List<PFDistribution> Gets(string sSql, Int64 nUserID)
        {
            return PFDistribution.Service.Gets(sSql, nUserID);
        }

        public static PFDistribution Rollback(int EnumBDOperation, int nPFMCID, Int64 nUserID)
        {
            return PFDistribution.Service.Rollback(EnumBDOperation, nPFMCID, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IPFDistributionService Service
        {
            get { return (IPFDistributionService)Services.Factory.CreateService(typeof(IPFDistributionService)); }
        }
        #endregion
    }

    #region IPFDistribution interface

    public interface IPFDistributionService
    {
        PFDistribution Distribute(int EnumBDOperation, int nPFMCID, Int64 nUserID);
        PFDistribution Rollback(int EnumBDOperation, int nPFMCID, Int64 nUserID);
        List<PFDistribution> Distributes(int EnumBDOperation, int nPFMCID, Int64 nUserID);
        List<PFDistribution> Gets(string sSql, Int64 nUserID);

    }
    #endregion
    }

