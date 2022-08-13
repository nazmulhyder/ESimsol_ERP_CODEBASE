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
    #region BlockMachineMapping

    public class BlockMachineMapping : BusinessObject
    {
        public BlockMachineMapping()
        {
            BMMID = 0;
            ProductionProcess = EnumProductionProcess.None;
            DepartmentID = 0;
            BlockName = "";
            IsActive = true;
            ErrorMessage = "";

        }

        #region Properties
        public int BMMID { get; set; }
        public EnumProductionProcess ProductionProcess { get; set; }
        public int DepartmentID { get; set; }
        public string BlockName { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string ActivityStatus { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public string DepartmentName { get; set; }
        public int ProductionProcessInt { get; set; }
        public string ProductionProcessInString
        {
            get
            {
                return ProductionProcess.ToString();
            }
        }
        public List<BlockMachineMappingDetail> BlockMachineMappingDetails { get; set; }
        #endregion

        #region Functions

        public static BlockMachineMapping Get(int id, long nUserID)
        {
            return BlockMachineMapping.Service.Get(id, nUserID);
        }
        public static BlockMachineMapping Get(string sSQL, long nUserID)
        {
            return BlockMachineMapping.Service.Get(sSQL, nUserID);
        }
        public static List<BlockMachineMapping> Gets(long nUserID)
        {
            return BlockMachineMapping.Service.Gets(nUserID);
        }
        public static List<BlockMachineMapping> Gets(string sSQL, long nUserID)
        {
            return BlockMachineMapping.Service.Gets(sSQL, nUserID);
        }
        public BlockMachineMapping IUD(int nDBOperation, long nUserID)
        {
            return BlockMachineMapping.Service.IUD(this, nDBOperation, nUserID);
        }
        public static BlockMachineMapping Activite(int nId, bool Active, long nUserID)
        {
            return BlockMachineMapping.Service.Activite(nId, Active, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBlockMachineMappingService Service
        {
            get { return (IBlockMachineMappingService)Services.Factory.CreateService(typeof(IBlockMachineMappingService)); }
        }
        #endregion
    }
    #endregion

    #region IBlockMachineMapping interface

    public interface IBlockMachineMappingService
    {
        BlockMachineMapping Get(int id, Int64 nUserID);
        BlockMachineMapping Get(string sSQL, Int64 nUserID);
        List<BlockMachineMapping> Gets(Int64 nUserID);
        List<BlockMachineMapping> Gets(string sSQL, Int64 nUserID);
        BlockMachineMapping IUD(BlockMachineMapping oBlockMachineMapping, int nDBOperation, Int64 nUserID);
        BlockMachineMapping Activite(int nId, bool Active, Int64 nUserID);
    }
    #endregion
}
