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
    public class SalaryCorrectionService : MarshalByRefObject, ISalaryCorrectionService
    {
        #region Private functions and declaration
        private SalaryCorrection MapObject(NullHandler oReader)
        {
            SalaryCorrection oSalaryCorrection = new SalaryCorrection();
            oSalaryCorrection.Name = oReader.GetString("Name");
            oSalaryCorrection.Code = oReader.GetString("Code");
            oSalaryCorrection.BUName = oReader.GetString("BUName");
            oSalaryCorrection.DepartmentName = oReader.GetString("DepartmentName");
            oSalaryCorrection.DesignationName = oReader.GetString("DesignationName");
            oSalaryCorrection.LocationName = oReader.GetString("LocationName");
            oSalaryCorrection.Reason = oReader.GetString("Reason");
            oSalaryCorrection.EmployeeID = oReader.GetInt32("EmployeeID");
            oSalaryCorrection.BUID = oReader.GetInt32("BUID");
            oSalaryCorrection.IndexNo = oReader.GetInt32("IndexNo");
            oSalaryCorrection.LocationID = oReader.GetInt32("LocationID");
            oSalaryCorrection.DesignationID = oReader.GetInt32("DesignationID");
            oSalaryCorrection.DepartmentID = oReader.GetInt32("DepartmentID");
            return oSalaryCorrection;

        }

        private SalaryCorrection CreateObject(NullHandler oReader)
        {
            SalaryCorrection oSalaryCorrection = MapObject(oReader);
            return oSalaryCorrection;
        }

        private List<SalaryCorrection> CreateObjects(IDataReader oReader)
        {
            List<SalaryCorrection> oSalaryCorrection = new List<SalaryCorrection>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalaryCorrection oItem = CreateObject(oHandler);
                oSalaryCorrection.Add(oItem);
            }
            return oSalaryCorrection;
        }


        #endregion

        #region Interface implementation
        public SalaryCorrectionService() { }
        public SalaryCorrection Get(string sSQL, Int64 nUserId)
        {
            SalaryCorrection oSalaryCorrection = new SalaryCorrection();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalaryCorrectionDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalaryCorrection = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                //oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oSalaryCorrection;
        }

        public List<SalaryCorrection> Gets(string sSQL, Int64 nUserID)
        {
            List<SalaryCorrection> oSalaryCorrection = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalaryCorrectionDA.Gets(sSQL, tc);
                oSalaryCorrection = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oSalaryCorrection;
        }


        public List<SalaryCorrection> GetsReason(string sBU, string sLocationID, int nMonthID, int nYear, int nRowLength, int nLoadRecords, string sEmployeeIDs, bool bIsCallFromExcel, Int64 nUserID)
        {
            int nIndex = 0;
            int nNewIndex = 1;
            List<SalaryCorrection> oSalaryCorrections = new List<SalaryCorrection>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                //while (nNewIndex != 0)
                //{
                    IDataReader reader = null;
                    reader = SalaryCorrectionDA.GetsReason(sBU, sLocationID, nMonthID, nYear, nRowLength, nLoadRecords, sEmployeeIDs, bIsCallFromExcel, tc);
                    NullHandler oreader = new NullHandler(reader);
                    int nCounter = 0;
                    List<SalaryCorrection> oSCs = new List<SalaryCorrection>();
                    while (reader.Read())
                    {
                        SalaryCorrection oSC = new SalaryCorrection();
                        oSC.EmployeeID = oreader.GetInt32("EmployeeID");
                        oSC.Name = oreader.GetString("Name");
                        oSC.Code = oreader.GetString("Code");
                        oSC.DepartmentName = oreader.GetString("DepartmentName");
                        oSC.DesignationName = oreader.GetString("DesignationName");
                        oSC.LocationName = oreader.GetString("LocationName");
                        oSC.BUName = oreader.GetString("BUName");
                        oSC.BUID = oreader.GetInt32("BUID");
                        oSC.LocationID = oreader.GetInt32("LocationID");
                        oSC.DepartmentID = oreader.GetInt32("DepartmentID");
                        oSC.DesignationID = oreader.GetInt32("DesignationID");
                        oSC.Reason = oreader.GetString("Reason");
                        oSC.IndexNo = oreader.GetInt32("IndexNo");

                        oSCs.Add(oSC);
                        oSalaryCorrections.Add(oSC);
                    }
                    reader.Close();
                    //nNewIndex = (oSCs.Count > 0) ? oSCs[0].IndexNo : 0;
                    //nIndex = nNewIndex;
                //}
                tc.End();


                //tc = TransactionContext.Begin();

                //IDataReader reader = null;
                //reader = SalaryCorrectionDA.GetsReason(sBU, sLocationID, nMonthID, nYear, tc);
                //oSalaryCorrection = CreateObjects(reader);
                //reader.Close();
                //tc.End();
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
            return oSalaryCorrections;
        }

        
        #endregion
    }
}

