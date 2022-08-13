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
    public class BlockMachineMappingSupervisorService : MarshalByRefObject, IBlockMachineMappingSupervisorService
    {
        #region Private functions and declaration
        private BlockMachineMappingSupervisor MapObject(NullHandler oReader)
        {
            BlockMachineMappingSupervisor oBlockMachineMappingSupervisor = new BlockMachineMappingSupervisor();
            oBlockMachineMappingSupervisor.BMMSID = oReader.GetInt32("BMMSID");
            oBlockMachineMappingSupervisor.BMMID = oReader.GetInt32("BMMID");
            oBlockMachineMappingSupervisor.EmployeeID = oReader.GetInt32("EmployeeID");
            oBlockMachineMappingSupervisor.StartDate = oReader.GetDateTime("StartDate");
            oBlockMachineMappingSupervisor.EndDate = oReader.GetDateTime("EndDate");
            oBlockMachineMappingSupervisor.IsActive = oReader.GetBoolean("IsActive");

            //derive
            oBlockMachineMappingSupervisor.EmployeeName = oReader.GetString("EmployeeName");
            oBlockMachineMappingSupervisor.BlockName = oReader.GetString("BlockName");

            return oBlockMachineMappingSupervisor;

        }

        private BlockMachineMappingSupervisor CreateObject(NullHandler oReader)
        {
            BlockMachineMappingSupervisor oBlockMachineMappingSupervisor = MapObject(oReader);
            return oBlockMachineMappingSupervisor;
        }

        private List<BlockMachineMappingSupervisor> CreateObjects(IDataReader oReader)
        {
            List<BlockMachineMappingSupervisor> oBlockMachineMappingSupervisor = new List<BlockMachineMappingSupervisor>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BlockMachineMappingSupervisor oItem = CreateObject(oHandler);
                oBlockMachineMappingSupervisor.Add(oItem);
            }
            return oBlockMachineMappingSupervisor;
        }

        #endregion

        #region Interface implementation
        public BlockMachineMappingSupervisorService() { }

        public BlockMachineMappingSupervisor IUD(BlockMachineMappingSupervisor oBlockMachineMappingSupervisor, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = BlockMachineMappingSupervisorDA.IUD(tc, oBlockMachineMappingSupervisor, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oBlockMachineMappingSupervisor = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oBlockMachineMappingSupervisor = new BlockMachineMappingSupervisor();
                    oBlockMachineMappingSupervisor.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oBlockMachineMappingSupervisor.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oBlockMachineMappingSupervisor.BMMSID = 0;
                #endregion
            }
            return oBlockMachineMappingSupervisor;
        }


        public BlockMachineMappingSupervisor Get(int nBMMSID, Int64 nUserId)
        {
            BlockMachineMappingSupervisor oBlockMachineMappingSupervisor = new BlockMachineMappingSupervisor();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BlockMachineMappingSupervisorDA.Get(nBMMSID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBlockMachineMappingSupervisor = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get BlockMachineMappingSupervisor", e);
                oBlockMachineMappingSupervisor.ErrorMessage = e.Message;
                #endregion
            }

            return oBlockMachineMappingSupervisor;
        }

        public BlockMachineMappingSupervisor Get(string sSql, Int64 nUserId)
        {
            BlockMachineMappingSupervisor oBlockMachineMappingSupervisor = new BlockMachineMappingSupervisor();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BlockMachineMappingSupervisorDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBlockMachineMappingSupervisor = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get BlockMachineMappingSupervisor", e);
                oBlockMachineMappingSupervisor.ErrorMessage = e.Message;
                #endregion
            }

            return oBlockMachineMappingSupervisor;
        }

        public List<BlockMachineMappingSupervisor> Gets(Int64 nUserID)
        {
            List<BlockMachineMappingSupervisor> oBlockMachineMappingSupervisor = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BlockMachineMappingSupervisorDA.Gets(tc);
                oBlockMachineMappingSupervisor = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_BlockMachineMappingSupervisor", e);
                #endregion
            }
            return oBlockMachineMappingSupervisor;
        }

        public List<BlockMachineMappingSupervisor> Gets(string sSQL, Int64 nUserID)
        {
            List<BlockMachineMappingSupervisor> oBlockMachineMappingSupervisor = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BlockMachineMappingSupervisorDA.Gets(sSQL, tc);
                oBlockMachineMappingSupervisor = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_BlockMachineMappingSupervisor", e);
                #endregion
            }
            return oBlockMachineMappingSupervisor;
        }

        #endregion

    }
}
