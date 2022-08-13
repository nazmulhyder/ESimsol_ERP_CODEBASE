using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;



namespace ESimSol.BusinessObjects
{
    #region BlockMachineMappingSupervisor

    public class BlockMachineMappingSupervisor : BusinessObject
    {
        public BlockMachineMappingSupervisor()
        {
            BMMSID = 0;
            BMMID = 0;
            EmployeeID = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            IsActive = true;
            ErrorMessage = "";
            BlockName = "";

        }

        #region Properties
        public int BMMSID { get; set; }
        public int BMMID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string EmployeeName { get; set; }
        public string BlockName { get; set; }
        public string ActivityStatus { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public string StartDateInString
        {
            get { return StartDate.ToString("dd MMM yyyy"); }
        }
        public string EndDateInString
        {
            get
            {
                if (this.IsActive == false)
                    return EndDate.ToString("dd MMM yyyy");
                else return "";
            }
        }
        #endregion

        #region Functions
        public static BlockMachineMappingSupervisor Get(int id, long nUserID)
        {
            return BlockMachineMappingSupervisor.Service.Get(id, nUserID);
        }
        public static BlockMachineMappingSupervisor Get(string sSQL, long nUserID)
        {
            return BlockMachineMappingSupervisor.Service.Get(sSQL, nUserID);
        }
        public static List<BlockMachineMappingSupervisor> Gets(long nUserID)
        {
            return BlockMachineMappingSupervisor.Service.Gets(nUserID);
        }
        public static List<BlockMachineMappingSupervisor> Gets(string sSQL, long nUserID)
        {
            return BlockMachineMappingSupervisor.Service.Gets(sSQL, nUserID);

        }
        public BlockMachineMappingSupervisor IUD(int nDBOperation, long nUserID)
        {
            return BlockMachineMappingSupervisor.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IBlockMachineMappingSupervisorService Service
        {
            get { return (IBlockMachineMappingSupervisorService)Services.Factory.CreateService(typeof(IBlockMachineMappingSupervisorService)); }
        }

        #endregion
    }
    #endregion

    #region IBlockMachineMappingSupervisor interface
    public interface IBlockMachineMappingSupervisorService
    {
        BlockMachineMappingSupervisor Get(int id, Int64 nUserID);
        BlockMachineMappingSupervisor Get(string sSQL, Int64 nUserID);
        List<BlockMachineMappingSupervisor> Gets(Int64 nUserID);
        List<BlockMachineMappingSupervisor> Gets(string sSQL, Int64 nUserID);
        BlockMachineMappingSupervisor IUD(BlockMachineMappingSupervisor oBlockMachineMappingSupervisor, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
