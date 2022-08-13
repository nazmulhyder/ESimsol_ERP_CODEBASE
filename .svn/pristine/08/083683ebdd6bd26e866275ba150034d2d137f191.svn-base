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
    public class RPTSalarySheetDetailService : MarshalByRefObject, IRPTSalarySheetDetailService
    {
        #region Private functions and declaration
        private RPTSalarySheetDetail MapObject(NullHandler oReader)
        {
            RPTSalarySheetDetail oEmployeeSalaryDetail = new RPTSalarySheetDetail();
            oEmployeeSalaryDetail.ESDID = oReader.GetInt32("ESDSalarylID");
            oEmployeeSalaryDetail.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
            oEmployeeSalaryDetail.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oEmployeeSalaryDetail.Amount = oReader.GetDouble("Amount");
            oEmployeeSalaryDetail.CompAmount = oReader.GetDouble("CompAmount");
            oEmployeeSalaryDetail.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oEmployeeSalaryDetail.SalaryHeadType = oReader.GetInt16("SalaryHeadType");
            oEmployeeSalaryDetail.SalaryHeadSequence = oReader.GetInt16("SalaryHeadSequence");
            return oEmployeeSalaryDetail;
        }

        private RPTSalarySheetDetail CreateObject(NullHandler oReader)
        {
            RPTSalarySheetDetail oEmployeeSalaryDetail = MapObject(oReader);
            return oEmployeeSalaryDetail;
        }

        private List<RPTSalarySheetDetail> CreateObjects(IDataReader oReader)
        {
            List<RPTSalarySheetDetail> oEmployeeSalaryDetail = new List<RPTSalarySheetDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RPTSalarySheetDetail oItem = CreateObject(oHandler);
                oEmployeeSalaryDetail.Add(oItem);
            }
            return oEmployeeSalaryDetail;
        }

        #endregion

        #region Interface implementation
        public RPTSalarySheetDetailService() { }

        public RPTSalarySheetDetail Get(int nESDID, Int64 nUserId)
        {
            RPTSalarySheetDetail oEmployeeSalaryDetail = new RPTSalarySheetDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RPTSalarySheetDetailDA.Get(nESDID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryDetail = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeSalaryDetail", e);
                oEmployeeSalaryDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalaryDetail;
        }

        public RPTSalarySheetDetail Get(string sSql, Int64 nUserId)
        {
            RPTSalarySheetDetail oEmployeeSalaryDetail = new RPTSalarySheetDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RPTSalarySheetDetailDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSalaryDetail = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeSalaryDetail", e);
                oEmployeeSalaryDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSalaryDetail;
        }

        public List<RPTSalarySheetDetail> Gets(Int64 nUserID)
        {
            List<RPTSalarySheetDetail> oEmployeeSalaryDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RPTSalarySheetDetailDA.Gets(tc);
                oEmployeeSalaryDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeSalaryDetail", e);
                #endregion
            }
            return oEmployeeSalaryDetail;
        }

        public List<RPTSalarySheetDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<RPTSalarySheetDetail> oEmployeeSalaryDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RPTSalarySheetDetailDA.Gets(sSQL, tc);
                oEmployeeSalaryDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeSalaryDetail", e);
                #endregion
            }
            return oEmployeeSalaryDetail;
        }

        public List<RPTSalarySheetDetail> GetEmployeesSalaryDetail(string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sBlockIDs, string sGroupIDs, string sEmpIDs, int nMonthID, int nYear, bool bNewJoin, int IsOutSheet, double nStartSalaryRange, double nEndSalaryRange, bool IsCompliance)
        {
            List<RPTSalarySheetDetail> oEmployeeSalaryDetails = new List<RPTSalarySheetDetail>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RPTSalarySheetDetailDA.GetEmployeesSalaryDetail(tc, sBU, sLocationID, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, sBlockIDs, sGroupIDs, sEmpIDs, nMonthID, nYear, bNewJoin, IsOutSheet, nStartSalaryRange, nEndSalaryRange, IsCompliance);

                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {

                    RPTSalarySheetDetail oESS = new RPTSalarySheetDetail();
                    oESS.EmployeeSalaryID = oreader.GetInt32("EmployeeSalaryID");
                    oESS.SalaryHeadID = oreader.GetInt32("SalaryHeadID");
                    oESS.SalaryHeadType = oreader.GetInt32("SType");
                    oESS.Amount = oreader.GetDouble("Amount");
                    oESS.SalaryHeadName = oreader.GetString("SalaryHeadName");

                    oEmployeeSalaryDetails.Add(oESS);
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
                throw new ServiceException("Failed to Get EmployeeSalaryDetail", e);
                #endregion
            }
            return oEmployeeSalaryDetails;
        }

        #endregion

    }
}
