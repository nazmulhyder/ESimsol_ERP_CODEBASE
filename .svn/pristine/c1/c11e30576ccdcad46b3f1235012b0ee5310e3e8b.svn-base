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
    public class BlockMachineMappingService : MarshalByRefObject, IBlockMachineMappingService
    {
        #region Private functions and declaration
        private BlockMachineMapping MapObject(NullHandler oReader)
        {
            BlockMachineMapping oBlockMachineMapping = new BlockMachineMapping();
            oBlockMachineMapping.BMMID = oReader.GetInt32("BMMID");
            oBlockMachineMapping.ProductionProcess = (EnumProductionProcess)oReader.GetInt16("ProductionProcess");
            oBlockMachineMapping.DepartmentID = oReader.GetInt32("DepartmentID");
            oBlockMachineMapping.BlockName = oReader.GetString("BlockName");
            oBlockMachineMapping.IsActive = oReader.GetBoolean("IsActive");

            //derive
            oBlockMachineMapping.DepartmentName = oReader.GetString("DepartmentName");

            return oBlockMachineMapping;

        }

        private BlockMachineMapping CreateObject(NullHandler oReader)
        {
            BlockMachineMapping oBlockMachineMapping = MapObject(oReader);
            return oBlockMachineMapping;
        }

        private List<BlockMachineMapping> CreateObjects(IDataReader oReader)
        {
            List<BlockMachineMapping> oBlockMachineMapping = new List<BlockMachineMapping>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BlockMachineMapping oItem = CreateObject(oHandler);
                oBlockMachineMapping.Add(oItem);
            }
            return oBlockMachineMapping;
        }

        #endregion

        #region Interface implementation
        public BlockMachineMappingService() { }

        public BlockMachineMapping IUD(BlockMachineMapping oBlockMachineMapping, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = BlockMachineMappingDA.IUD(tc, oBlockMachineMapping, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oBlockMachineMapping = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBlockMachineMapping.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oBlockMachineMapping.BMMID = 0;
                #endregion
            }
            return oBlockMachineMapping;
        }


        public BlockMachineMapping Get(int nBMMID, Int64 nUserId)
        {
            BlockMachineMapping oBlockMachineMapping = new BlockMachineMapping();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BlockMachineMappingDA.Get(nBMMID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBlockMachineMapping = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get BlockMachineMapping", e);
                oBlockMachineMapping.ErrorMessage = e.Message;
                #endregion
            }

            return oBlockMachineMapping;
        }

        public BlockMachineMapping Get(string sSql, Int64 nUserId)
        {
            BlockMachineMapping oBlockMachineMapping = new BlockMachineMapping();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BlockMachineMappingDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBlockMachineMapping = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get BlockMachineMapping", e);
                oBlockMachineMapping.ErrorMessage = e.Message;
                #endregion
            }

            return oBlockMachineMapping;
        }

        public List<BlockMachineMapping> Gets(Int64 nUserID)
        {
            List<BlockMachineMapping> oBlockMachineMapping = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BlockMachineMappingDA.Gets(tc);
                oBlockMachineMapping = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_BlockMachineMapping", e);
                #endregion
            }
            return oBlockMachineMapping;
        }

        public List<BlockMachineMapping> Gets(string sSQL, Int64 nUserID)
        {
            List<BlockMachineMapping> oBlockMachineMapping = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BlockMachineMappingDA.Gets(sSQL, tc);
                oBlockMachineMapping = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_BlockMachineMapping", e);
                #endregion
            }
            return oBlockMachineMapping;
        }

        #endregion

        #region Activity
        public BlockMachineMapping Activite(int nBlockMachineMappingID, bool Active, Int64 nUserId)
        {
            BlockMachineMapping oBlockMachineMapping = new BlockMachineMapping();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BlockMachineMappingDA.Activity(nBlockMachineMappingID, Active, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBlockMachineMapping = CreateObject(oReader);
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
                oBlockMachineMapping.ErrorMessage = e.Message;
                #endregion
            }

            return oBlockMachineMapping;
        }


        #endregion
    }
}
