using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class BlockMachineMappingDetailService : MarshalByRefObject, IBlockMachineMappingDetailService
    {
        #region Private functions and declaration
        private BlockMachineMappingDetail MapObject(NullHandler oReader)
        {
            BlockMachineMappingDetail oBlockMachineMappingDetail = new BlockMachineMappingDetail();
            oBlockMachineMappingDetail.BMMDID = oReader.GetInt32("BMMDID");
            oBlockMachineMappingDetail.BMMID = oReader.GetInt32("BMMID");
            oBlockMachineMappingDetail.MachineNo = oReader.GetString("MachineNo");
            oBlockMachineMappingDetail.IsActive = oReader.GetBoolean("IsActive");

            return oBlockMachineMappingDetail;

        }

        private BlockMachineMappingDetail CreateObject(NullHandler oReader)
        {
            BlockMachineMappingDetail oBlockMachineMappingDetail = MapObject(oReader);
            return oBlockMachineMappingDetail;
        }

        private List<BlockMachineMappingDetail> CreateObjects(IDataReader oReader)
        {
            List<BlockMachineMappingDetail> oBlockMachineMappingDetail = new List<BlockMachineMappingDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BlockMachineMappingDetail oItem = CreateObject(oHandler);
                oBlockMachineMappingDetail.Add(oItem);
            }
            return oBlockMachineMappingDetail;
        }

        #endregion

        #region Interface implementation
        public BlockMachineMappingDetailService() { }

        public BlockMachineMappingDetail IUD(BlockMachineMappingDetail oBlockMachineMappingDetail, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = BlockMachineMappingDetailDA.IUD(tc, oBlockMachineMappingDetail, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oBlockMachineMappingDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBlockMachineMappingDetail.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oBlockMachineMappingDetail.BMMDID = 0;
                #endregion
            }
            return oBlockMachineMappingDetail;
        }


        public BlockMachineMappingDetail Get(int nBMMDID, Int64 nUserId)
        {
            BlockMachineMappingDetail oBlockMachineMappingDetail = new BlockMachineMappingDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BlockMachineMappingDetailDA.Get(nBMMDID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBlockMachineMappingDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get BlockMachineMappingDetail", e);
                oBlockMachineMappingDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oBlockMachineMappingDetail;
        }

        public BlockMachineMappingDetail Get(string sSql, Int64 nUserId)
        {
            BlockMachineMappingDetail oBlockMachineMappingDetail = new BlockMachineMappingDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BlockMachineMappingDetailDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBlockMachineMappingDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get BlockMachineMappingDetail", e);
                oBlockMachineMappingDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oBlockMachineMappingDetail;
        }

        public List<BlockMachineMappingDetail> Gets(Int64 nUserID)
        {
            List<BlockMachineMappingDetail> oBlockMachineMappingDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BlockMachineMappingDetailDA.Gets(tc);
                oBlockMachineMappingDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_BlockMachineMappingDetail", e);
                #endregion
            }
            return oBlockMachineMappingDetail;
        }

        public List<BlockMachineMappingDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<BlockMachineMappingDetail> oBlockMachineMappingDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BlockMachineMappingDetailDA.Gets(sSQL, tc);
                oBlockMachineMappingDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_BlockMachineMappingDetail", e);
                #endregion
            }
            return oBlockMachineMappingDetail;
        }

        #region Activity
        public BlockMachineMappingDetail Activite(int nBlockMachineMappingDetailID, bool Active, Int64 nUserId)
        {
            BlockMachineMappingDetail oBlockMachineMappingDetail = new BlockMachineMappingDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BlockMachineMappingDetailDA.Activity(nBlockMachineMappingDetailID, Active, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBlockMachineMappingDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                oBlockMachineMappingDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oBlockMachineMappingDetail;
        }


        #endregion

        #endregion

    }
}
