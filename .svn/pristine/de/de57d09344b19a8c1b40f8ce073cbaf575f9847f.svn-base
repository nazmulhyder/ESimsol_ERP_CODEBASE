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
    #region BlockMachineMappingReportSheet

    public class BlockMachineMappingReport : BusinessObject
    {
        public BlockMachineMappingReport()
        {

            StyleNo = "";
            GarmentPart = 0;
            SizeCategoryName = "";
            ColorName = "";
            IssueQty = 0;
            RcvQty = 0;
            MachineNo = "";
            DepartmentName = "";
            BlockName = "";
            SupervisorName = "";
            GPName = "";
            ErrorMessage = "";

        }

        #region Properties
        public string StyleNo { get; set; }
        public string SizeCategoryName { get; set; }
        public string ColorName { get; set; }
        public int IssueQty { get; set; }
        public int RcvQty { get; set; }
        public string MachineNo { get; set; }
        public string DepartmentName { get; set; }
        public string BlockName { get; set; }
        public string SupervisorName { get; set; }
        public int GarmentPart { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public Company Company { get; set; }
        public List<BlockMachineMapping> BlockMachineMappings { get; set; }
        public List<BlockMachineMappingReport> BlockMachineMappingReports { get; set; }

        //public int GarmentPartInt { get; set; }
        //public string GarmentPartInString
        //{
        //    get
        //    {
        //        return GarmentPart.ToString();
        //    }

        //}

        public string GPName { get; set; }
        public string SizeAndColor
        {
            get
            {
                return this.ColorName.ToString() + "[" + this.SizeCategoryName.ToString() + "]";
            }

        }
        #endregion

        #region Functions
        public static List<BlockMachineMappingReport> Gets(string sParams, long nUserID)
        {
            return BlockMachineMappingReport.Service.Gets(sParams, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBlockMachineMappingReportService Service
        {
            get { return (IBlockMachineMappingReportService)Services.Factory.CreateService(typeof(IBlockMachineMappingReportService)); }
        }
        #endregion
    }
    #endregion

    #region IBlockMachineMappingReport interface
    public interface IBlockMachineMappingReportService
    {

        List<BlockMachineMappingReport> Gets(string sParams, Int64 nUserID);


    }
    #endregion
}
