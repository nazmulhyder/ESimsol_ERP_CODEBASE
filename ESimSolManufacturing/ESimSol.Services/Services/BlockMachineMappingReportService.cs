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
    public class BlockMachineMappingReportService : MarshalByRefObject, IBlockMachineMappingReportService
    {
        #region Private functions and declaration
        private BlockMachineMappingReport MapObject(NullHandler oReader)
        {
            BlockMachineMappingReport oBlockMachineMappingReport = new BlockMachineMappingReport();
            oBlockMachineMappingReport.StyleNo = oReader.GetString("StyleNo");
            oBlockMachineMappingReport.GarmentPart = oReader.GetInt32("GarmentPart");
            oBlockMachineMappingReport.SizeCategoryName = oReader.GetString("SizeCategoryName");
            oBlockMachineMappingReport.ColorName = oReader.GetString("ColorName");
            oBlockMachineMappingReport.IssueQty = oReader.GetInt32("IssueQty");
            oBlockMachineMappingReport.RcvQty = oReader.GetInt32("RcvQty");
            oBlockMachineMappingReport.MachineNo = oReader.GetString("MachineNo");
            oBlockMachineMappingReport.DepartmentName = oReader.GetString("DepartmentName");
            oBlockMachineMappingReport.BlockName = oReader.GetString("BlockName");
            oBlockMachineMappingReport.SupervisorName = oReader.GetString("SupervisorName");
            oBlockMachineMappingReport.GPName = oReader.GetString("GPName");
            return oBlockMachineMappingReport;

        }

        private BlockMachineMappingReport CreateObject(NullHandler oReader)
        {
            BlockMachineMappingReport oBlockMachineMappingReport = MapObject(oReader);
            return oBlockMachineMappingReport;
        }

        private List<BlockMachineMappingReport> CreateObjects(IDataReader oReader)
        {
            List<BlockMachineMappingReport> oBlockMachineMappingReport = new List<BlockMachineMappingReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BlockMachineMappingReport oItem = CreateObject(oHandler);
                oBlockMachineMappingReport.Add(oItem);
            }
            return oBlockMachineMappingReport;
        }

        #endregion

        #region Interface implementation
        public BlockMachineMappingReportService() { }


        public List<BlockMachineMappingReport> Gets(string sParams, Int64 nUserId)
        {
            List<BlockMachineMappingReport> oBlockMachineMappingReports = new List<BlockMachineMappingReport>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BlockMachineMappingReportDA.Gets(tc, sParams);
                oBlockMachineMappingReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message.Split('!')[0]);
                #endregion
            }
            return oBlockMachineMappingReports;
        }

        #endregion

    }
}
