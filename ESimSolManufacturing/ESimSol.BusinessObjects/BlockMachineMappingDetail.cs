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
    #region BlockMachineMappingDetail

    public class BlockMachineMappingDetail : BusinessObject
    {
        public BlockMachineMappingDetail()
        {
            BMMDID = 0;
            BMMID = 0;
            MachineNo = "";
            IsActive = true;
            ErrorMessage = "";

        }

        #region Properties
        public int BMMDID { get; set; }
        public int BMMID { get; set; }
        public string MachineNo { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ActivityStatus { get { if (IsActive)return "Active"; else return "Inactive"; } }

        #endregion

        #region Functions

        public static BlockMachineMappingDetail Get(int id, long nUserID)
        {
            return BlockMachineMappingDetail.Service.Get(id, nUserID);
        }

        public static BlockMachineMappingDetail Get(string sSQL, long nUserID)
        {
            return BlockMachineMappingDetail.Service.Get(sSQL, nUserID);
        }

        public static List<BlockMachineMappingDetail> Gets(long nUserID)
        {
            return BlockMachineMappingDetail.Service.Gets(nUserID);
        }

        public static List<BlockMachineMappingDetail> Gets(string sSQL, long nUserID)
        {
            return BlockMachineMappingDetail.Service.Gets(sSQL, nUserID);
        }

        public BlockMachineMappingDetail IUD(int nDBOperation, long nUserID)
        {
            return BlockMachineMappingDetail.Service.IUD(this, nDBOperation, nUserID);
        }

        public static BlockMachineMappingDetail Activite(int nId, bool Active, long nUserID)
        {
            return BlockMachineMappingDetail.Service.Activite(nId, Active, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBlockMachineMappingDetailService Service
        {
            get { return (IBlockMachineMappingDetailService)Services.Factory.CreateService(typeof(IBlockMachineMappingDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IBlockMachineMappingDetail interface

    public interface IBlockMachineMappingDetailService
    {
        BlockMachineMappingDetail Get(int id, Int64 nUserID);
        BlockMachineMappingDetail Get(string sSQL, Int64 nUserID);
        List<BlockMachineMappingDetail> Gets(Int64 nUserID);
        List<BlockMachineMappingDetail> Gets(string sSQL, Int64 nUserID);
        BlockMachineMappingDetail IUD(BlockMachineMappingDetail oBlockMachineMappingDetail, int nDBOperation, Int64 nUserID);
        BlockMachineMappingDetail Activite(int nId, bool Active, Int64 nUserID);

    }
    #endregion
}
