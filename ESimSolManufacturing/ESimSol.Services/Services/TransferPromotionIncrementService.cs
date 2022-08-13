using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.IO;
using System.Diagnostics;

namespace ESimSol.Services.Services
{
    public class TransferPromotionIncrementService : MarshalByRefObject, ITransferPromotionIncrementService
    {
        #region Private functions and declaration
        private TransferPromotionIncrement MapObject(NullHandler oReader)
        {
            TransferPromotionIncrement oTransferPromotionIncrement = new TransferPromotionIncrement();
            oTransferPromotionIncrement.TPIID = oReader.GetInt32("TPIID");
            oTransferPromotionIncrement.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");
            oTransferPromotionIncrement.TPIEmployeeTypeID = oReader.GetInt32("TPIEmployeeTypeID");
            oTransferPromotionIncrement.EmployeeID = oReader.GetInt32("EmployeeID");
            oTransferPromotionIncrement.DesignationID = oReader.GetInt32("DesignationID");
            oTransferPromotionIncrement.DRPID = oReader.GetInt32("DRPID");
            oTransferPromotionIncrement.JoiningDate = oReader.GetDateTime("JoiningDate");
            oTransferPromotionIncrement.ASID = oReader.GetInt32("ASID");
            oTransferPromotionIncrement.SalarySchemeID = oReader.GetInt32("SalarySchemeID");
            oTransferPromotionIncrement.GrossSalary = oReader.GetDouble("GrossSalary");
            oTransferPromotionIncrement.CompGrossSalary = oReader.GetDouble("CompGrossSalary");
            oTransferPromotionIncrement.CompTPIGrossSalary = oReader.GetDouble("CompTPIGrossSalary");
            oTransferPromotionIncrement.IsNoHistory = oReader.GetBoolean("IsNoHistory");
            oTransferPromotionIncrement.IsTransfer = oReader.GetBoolean("IsTransfer");
            oTransferPromotionIncrement.IsPromotion = oReader.GetBoolean("IsPromotion");
            oTransferPromotionIncrement.IsIncrement = oReader.GetBoolean("IsIncrement");
            oTransferPromotionIncrement.IsCashFixed = oReader.GetBoolean("IsCashFixed");
            oTransferPromotionIncrement.TPIDesignationID = oReader.GetInt32("TPIDesignationID");
            oTransferPromotionIncrement.TPIDRPID = oReader.GetInt32("TPIDRPID");
            oTransferPromotionIncrement.TPIASID = oReader.GetInt32("TPIASID");
            oTransferPromotionIncrement.TPISalarySchemeID = oReader.GetInt32("TPISalarySchemeID");
            oTransferPromotionIncrement.TPIShiftID = oReader.GetInt32("TPIShiftID");
            oTransferPromotionIncrement.TPIGrossSalary = oReader.GetDouble("TPIGrossSalary");
            oTransferPromotionIncrement.TPIIsFixedAmount = oReader.GetBoolean("TPIIsFixedAmount");
            oTransferPromotionIncrement.CashAmount = oReader.GetDouble("CashAmount");
            oTransferPromotionIncrement.Note = oReader.GetString("Note");
            oTransferPromotionIncrement.EffectedDate = oReader.GetDateTime("EffectedDate");
            oTransferPromotionIncrement.ActualEffectedDate = oReader.GetDateTime("ActualEffectedDate");
            oTransferPromotionIncrement.RecommendedBy = oReader.GetInt16("RecommendedBy");
            oTransferPromotionIncrement.RecommendedByDate = oReader.GetDateTime("RecommendedByDate");
            oTransferPromotionIncrement.ApproveBy = oReader.GetInt16("ApproveBy");
            oTransferPromotionIncrement.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            //derive
            oTransferPromotionIncrement.EmployeeName = oReader.GetString("EmployeeName");
            oTransferPromotionIncrement.EmployeeCode = oReader.GetString("EmployeeCode");
            oTransferPromotionIncrement.LocationName = oReader.GetString("LocationName");
            oTransferPromotionIncrement.DesignationName = oReader.GetString("DesignationName");
            oTransferPromotionIncrement.DepartmentName = oReader.GetString("DepartmentName");

            oTransferPromotionIncrement.TPILocationName = oReader.GetString("TPILocationName");
            oTransferPromotionIncrement.TPIDepartmentName = oReader.GetString("TPIDepartmentName");
            oTransferPromotionIncrement.TPIDesignationName = oReader.GetString("TPIDesignationName");
            //oTransferPromotionIncrement.DRPName = oReader.GetString("DRPName");
            //oTransferPromotionIncrement.TPIDRPName = oReader.GetString("TPIDRPName");
            oTransferPromotionIncrement.AttendanceSchemeName = oReader.GetString("AttendanceSchemeName");
            oTransferPromotionIncrement.TPIASName = oReader.GetString("TPIASName");
            oTransferPromotionIncrement.SalarySchemeName = oReader.GetString("SalarySchemeName");
            oTransferPromotionIncrement.TPISSName = oReader.GetString("TPISSName");
            oTransferPromotionIncrement.TPIShiftName = oReader.GetString("TPIShiftName");
            oTransferPromotionIncrement.RecommendBYName = oReader.GetString("RecommendBYName");
            oTransferPromotionIncrement.ApprovedBYName = oReader.GetString("ApprovedBYName");
            oTransferPromotionIncrement.EncryptTPIID = Global.Encrypt(oTransferPromotionIncrement.TPIID.ToString());
            oTransferPromotionIncrement.PresentDesignationName = oReader.GetString("PresentDesignationName");

            return oTransferPromotionIncrement;

        }

        private TransferPromotionIncrement CreateObject(NullHandler oReader)
        {
            TransferPromotionIncrement oTransferPromotionIncrement = MapObject(oReader);
            return oTransferPromotionIncrement;
        }

        private List<TransferPromotionIncrement> CreateObjects(IDataReader oReader)
        {
            List<TransferPromotionIncrement> oTransferPromotionIncrement = new List<TransferPromotionIncrement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TransferPromotionIncrement oItem = CreateObject(oHandler);
                oTransferPromotionIncrement.Add(oItem);
            }
            return oTransferPromotionIncrement;
        }

        #endregion

        #region Interface implementation
        public TransferPromotionIncrementService() { }

        public TransferPromotionIncrement IUD(TransferPromotionIncrement oTransferPromotionIncrement, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = TransferPromotionIncrementDA.IUD(tc, oTransferPromotionIncrement, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oTransferPromotionIncrement = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oTransferPromotionIncrement.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oTransferPromotionIncrement;
        }
        public List<TransferPromotionIncrement> IUD_Multiple(string sEmployeeIDs, int nSalaryHeadID, int nPercent, string sMonthIDs, string sYearIDs, string sBUIDs, string sLocationIDs, Int64 nUserID)
        {
            int nIndex = 0;
            int nNewIndex = 1;
            List<TransferPromotionIncrement> oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
            List<TransferPromotionIncrement> oErrorLists = new List<TransferPromotionIncrement>();
            TransferPromotionIncrement oTempTPI = new TransferPromotionIncrement();
            TransferPromotionIncrement oTransferPromotionIncrement = new TransferPromotionIncrement();
            //oTPIs = oTransferPromotionIncrement.TransferPromotionIncrements;//old

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                while (nNewIndex != 0)
                {
                    IDataReader reader = null;
                    reader = TransferPromotionIncrementDA.GetsIncrementByPercent(tc, sEmployeeIDs, nSalaryHeadID, nPercent, sMonthIDs, sYearIDs, nUserID, sBUIDs, sLocationIDs, nIndex);
                    NullHandler oreader = new NullHandler(reader);
                    List<TransferPromotionIncrement> oTPIs = new List<TransferPromotionIncrement>();
                    int nCounter = 0;
                    while (reader.Read())
                    {
                        ++nCounter;
                        TransferPromotionIncrement oTPI = new TransferPromotionIncrement();
                        oTPI.EmployeeID = oreader.GetInt32("EmployeeID");
                        oTPI.EmployeeName = oreader.GetString("Name");
                        oTPI.EmployeeCode = oreader.GetString("Code");
                        oTPI.EmployeeID = oreader.GetInt32("EmployeeID");
                        oTPI.IndexNo = oreader.GetInt32("IndexNo");
                        oTPI.DesignationID = oreader.GetInt32("DesignationID");
                        oTPI.DRPID = oreader.GetInt32("DRPID");
                        oTPI.ASID = oreader.GetInt32("ASID");
                        oTPI.SalarySchemeID = oreader.GetInt32("SalarySchemeID");
                        oTPI.TPISalarySchemeID = oreader.GetInt32("SalarySchemeID");
                        oTPI.GrossSalary = oreader.GetDouble("PreviousGrossAmount");
                        oTPI.TPIGrossSalary = oreader.GetDouble("IncrementedGrossAmount");
                        oTPI.CashAmount = oreader.GetDouble("IncrementedGrossAmount");
                        oTPI.CompTPIGrossSalary = oreader.GetDouble("IncrementedGrossAmount");
                        oTPI.BasicAmount = oreader.GetDouble("PreviousBasicAmount");
                        oTPI.TPIBasicAmount = oreader.GetDouble("IncrementedBasicAmount");
                        oTPI.ActualEffectedDate = oreader.GetDateTime("DOJ");
                        oTPI.SalarySchemeName = oreader.GetString("SalarySchemeName");
                        oTPI.PresentDesignationName = oreader.GetString("PresentDesignationName");
                        oTPIs.Add(oTPI);
                        oTransferPromotionIncrements.Add(oTPI);
                    }
                    reader.Close();

                    foreach (TransferPromotionIncrement oItem in oTPIs)
                    {
                        try
                        {
                            IDataReader readerP;
                            oItem.IsCashFixed = true;
                            readerP = TransferPromotionIncrementDA.UploadXLAsPerScheme(tc, oItem, nUserID);
                            NullHandler oReaderP = new NullHandler(readerP);
                            if (readerP.Read())
                            {
                                oTransferPromotionIncrement = CreateObject(oReaderP);
                            }

                            readerP.Close();
                        }
                        catch (Exception e)
                        {
                            #region Handle Exception
                            if (tc != null)
                                tc.HandleError();
                            oTempTPI.EmployeeCode = oItem.EmployeeCode;
                            oTempTPI.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View  
                            oErrorLists.Add(oTempTPI);
                            #endregion
                        }
                    }
                    nNewIndex = (oTPIs.Count > 0) ? oTPIs[0].IndexNo : 0;
                    nIndex = nNewIndex;
                }
                tc.End();
            }
            catch (Exception e)
            {
                oTempTPI.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View  
                oErrorLists.Add(oTempTPI);
            }


            //foreach (TransferPromotionIncrement oItem in oTPIs)
            //{
            //    try
            //    {
            //        tc = TransactionContext.Begin(true);

            //        //Insert Update Delete Part
            //        IDataReader reader;
            //        oItem.IsCashFixed = true;
            //        reader = TransferPromotionIncrementDA.UploadXLAsPerScheme(tc, oItem, nUserID);
            //        NullHandler oReader = new NullHandler(reader);
            //        if (reader.Read())
            //        {
            //            oTransferPromotionIncrement = CreateObject(oReader);
            //        }
            //        reader.Close();

            //        ////Recommend Part
            //        //IDataReader readerRec = TransferPromotionIncrementDA.Recommend(oTransferPromotionIncrement.TPIID, nUserID, tc);
            //        //NullHandler oReaderRec = new NullHandler(readerRec);
            //        //if (readerRec.Read())
            //        //{
            //        //    oTransferPromotionIncrement = CreateObject(oReaderRec);
            //        //}
            //        //readerRec.Close();

            //        ////Approve Part
            //        //IDataReader readerApp = TransferPromotionIncrementDA.Approve(oTransferPromotionIncrement.TPIID, nUserID, tc);
            //        //NullHandler oReaderApp = new NullHandler(readerApp);
            //        //if (readerApp.Read())
            //        //{
            //        //    oTransferPromotionIncrement = CreateObject(oReaderApp);
            //        //}
            //        //readerApp.Close();

            //        //Effect Part
            //        //IDataReader readerEff = TransferPromotionIncrementDA.Effect(tc, oTransferPromotionIncrement, nUserID);
            //        //NullHandler oReaderEff = new NullHandler(readerEff);
            //        //if (readerEff.Read())
            //        //{

            //        //    oTransferPromotionIncrement = CreateObject(oReaderEff);
            //        //}
            //        //readerEff.Close();


            //        tc.End();
            //    }
            //    catch (Exception e)
            //    {
            //        #region Handle Exception
            //        if (tc != null)
            //            tc.HandleError();
            //        oTempTPI.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
            //        #endregion
            //    }
            //}
            return oErrorLists;
        }

        public TransferPromotionIncrement AttScheme(TransferPromotionIncrement oTransferPromotionIncrement, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = TransferPromotionIncrementDA.AttScheme(tc, oTransferPromotionIncrement, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oTransferPromotionIncrement = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oTransferPromotionIncrement.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oTransferPromotionIncrement;
        }

        public TransferPromotionIncrement IUDQuick(TransferPromotionIncrement oTransferPromotionIncrement, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = TransferPromotionIncrementDA.IUDQuick(tc, oTransferPromotionIncrement, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oTransferPromotionIncrement = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oTransferPromotionIncrement.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oTransferPromotionIncrement;
        }


        public TransferPromotionIncrement Get(int nTPIID, Int64 nUserId)
        {
            TransferPromotionIncrement oTransferPromotionIncrement = new TransferPromotionIncrement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TransferPromotionIncrementDA.Get(nTPIID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTransferPromotionIncrement = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get TransferPromotionIncrement", e);
                oTransferPromotionIncrement.ErrorMessage = e.Message;
                #endregion
            }

            return oTransferPromotionIncrement;
        }
        public List<TransferPromotionIncrement> Gets(Int64 nUserID)
        {
            List<TransferPromotionIncrement> oTransferPromotionIncrement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TransferPromotionIncrementDA.Gets(tc);
                oTransferPromotionIncrement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TransferPromotionIncrement", e);
                #endregion
            }
            return oTransferPromotionIncrement;
        }



        public List<TransferPromotionIncrement> GetsAutoTPI(Int64 nUserID)
        {
            List<TransferPromotionIncrement> oTransferPromotionIncrement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TransferPromotionIncrementDA.GetsAutoTPI(tc);
                oTransferPromotionIncrement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TransferPromotionIncrement", e);
                #endregion
            }
            return oTransferPromotionIncrement;
        }

        public List<TransferPromotionIncrement> GetsIncrementByPercent(string sEmployeeIDs, int nSalaryHeadID, int nPercent, string sMonthIDs, string sYearIDs, string sBUIDs, string sLocationIDs, Int64 nUserID)
        {
            int nIndex = 0;
            int nNewIndex = 1;
            List<TransferPromotionIncrement> oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
            TransferPromotionIncrement oTransferPromotionIncrement = new TransferPromotionIncrement();
            //oTPIs = oTransferPromotionIncrement.TransferPromotionIncrements;//old

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                while (nNewIndex != 0)
                {
                    IDataReader reader = null;
                    reader = TransferPromotionIncrementDA.GetsIncrementByPercent(tc, sEmployeeIDs, nSalaryHeadID, nPercent, sMonthIDs, sYearIDs, nUserID, sBUIDs, sLocationIDs, nIndex);
                    NullHandler oreader = new NullHandler(reader);
                    int nCounter = 0;
                    List<TransferPromotionIncrement> oTPIs = new List<TransferPromotionIncrement>();
                    while (reader.Read())
                    {
                        ++nCounter;
                        TransferPromotionIncrement oTPI = new TransferPromotionIncrement();
                        oTPI.EmployeeID = oreader.GetInt32("EmployeeID");
                        oTPI.EmployeeName = oreader.GetString("Name");
                        oTPI.EmployeeCode = oreader.GetString("Code");
                        oTPI.EmployeeID = oreader.GetInt32("EmployeeID");
                        oTPI.IndexNo = oreader.GetInt32("IndexNo");
                        oTPI.DesignationID = oreader.GetInt32("DesignationID");
                        oTPI.DRPID = oreader.GetInt32("DRPID");
                        oTPI.ASID = oreader.GetInt32("ASID");
                        oTPI.SalarySchemeID = oreader.GetInt32("SalarySchemeID");
                        oTPI.TPISalarySchemeID = oreader.GetInt32("SalarySchemeID");
                        oTPI.GrossSalary = oreader.GetDouble("PreviousGrossAmount");
                        oTPI.TPIGrossSalary = oreader.GetDouble("IncrementedGrossAmount");
                        oTPI.CashAmount = oreader.GetDouble("IncrementedGrossAmount");
                        oTPI.CompTPIGrossSalary = oreader.GetDouble("IncrementedGrossAmount");
                        oTPI.BasicAmount = oreader.GetDouble("PreviousBasicAmount");
                        oTPI.TPIBasicAmount = oreader.GetDouble("IncrementedBasicAmount");
                        oTPI.ActualEffectedDate = oreader.GetDateTime("DOJ");
                        oTPI.SalarySchemeName = oreader.GetString("SalarySchemeName");
                        oTPI.LocationName = oreader.GetString("LocationName");
                        oTPI.BUName = oreader.GetString("BUName");
                        oTPIs.Add(oTPI);
                        oTransferPromotionIncrements.Add(oTPI);
                    }
                    List<TransferPromotionIncrement> oTTPIs = new List<TransferPromotionIncrement>();
                    reader.Close();
                    nNewIndex = (oTPIs.Count > 0) ? oTPIs[0].IndexNo:0;
                    nIndex = nNewIndex;
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TransferPromotionIncrement", e);
                #endregion
            }

            return oTransferPromotionIncrements;
        }
        public List<TransferPromotionIncrement> Gets(string sSQL, Int64 nUserID)
        {
            List<TransferPromotionIncrement> oTransferPromotionIncrement = new List<TransferPromotionIncrement>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TransferPromotionIncrementDA.Gets(sSQL, tc);
                oTransferPromotionIncrement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TransferPromotionIncrement", e);
                #endregion
            }
            return oTransferPromotionIncrement;
        }

        #region RecommendAndApprove
        public TransferPromotionIncrement Recommend(int nTPIID, Int64 nUserId)
        {
            TransferPromotionIncrement oTransferPromotionIncrement = new TransferPromotionIncrement();
            TransactionContext tc = null;
            try
            {
                if (nUserId < 0)
                {
                    throw new Exception("SuperUser Is Not Allowed To Recommend !");
                }
                tc = TransactionContext.Begin();
                IDataReader reader = TransferPromotionIncrementDA.Recommend(nTPIID, nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTransferPromotionIncrement = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {

                oTransferPromotionIncrement.ErrorMessage = e.Message;

            }

            return oTransferPromotionIncrement;
        }

        public TransferPromotionIncrement Approve(int nTPIID, Int64 nUserId)
        {
            TransferPromotionIncrement oTransferPromotionIncrement = new TransferPromotionIncrement();
            TransactionContext tc = null;
            try
            {
                if (nUserId < 0)
                {
                    throw new Exception("SuperUser Is Not Allowed To Approve !");
                }
                tc = TransactionContext.Begin();
                IDataReader reader = TransferPromotionIncrementDA.Approve(nTPIID, nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTransferPromotionIncrement = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                oTransferPromotionIncrement.ErrorMessage = e.Message;
            }

            return oTransferPromotionIncrement;
        }

        public List<TransferPromotionIncrement> MultipleApprove(TransferPromotionIncrement oTransferPromotionIncrement, Int64 nUserId)
        {
            List<TransferPromotionIncrement> oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
            TransactionContext tc = null;
            try
            {
                if (nUserId < 0)
                {
                    throw new Exception("SuperUser Is Not Allowed To Approve !");
                }

                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TransferPromotionIncrementDA.MultipleApprove(oTransferPromotionIncrement,nUserId,tc);
                oTransferPromotionIncrements = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                oTransferPromotionIncrement = new TransferPromotionIncrement();
                oTransferPromotionIncrement.ErrorMessage = e.Message;
                oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
                oTransferPromotionIncrements.Add(oTransferPromotionIncrement);
            }

            return oTransferPromotionIncrements;
        }

        #endregion

        public TransferPromotionIncrement Effect(TransferPromotionIncrement oTransferPromotionIncrement, Int64 nUserID)
        {

            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            oEmployeeSalaryStructureDetails = oTransferPromotionIncrement.EmployeeSalaryStructureDetails;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = TransferPromotionIncrementDA.Effect(tc, oTransferPromotionIncrement, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oTransferPromotionIncrement = CreateObject(oReader);
                }
                reader.Close();
                string sEmployeeSalaryStructureSQL = " SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID = " + oTransferPromotionIncrement.EmployeeID;
                int ESSID = TransferPromotionIncrementDA.GetEmployeeSalaryStructureID(sEmployeeSalaryStructureSQL, tc);
                if (oEmployeeSalaryStructureDetails != null)
                {                    
                    foreach (EmployeeSalaryStructureDetail oItem in oEmployeeSalaryStructureDetails)
                    {
                        oItem.ESSID = ESSID;
                        IDataReader ESSDreader;
                        ESSDreader = EmployeeSalaryStructureDetailDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert, "");                        
                        ESSDreader.Close();
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oTransferPromotionIncrement.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oTransferPromotionIncrement;
        }



        #region UploadXL

        public List<TransferPromotionIncrement> UploadXLAsPerScheme(List<TransferPromotionIncrement> oTPIs, Int64 nUserID)
        {
            TransferPromotionIncrement oTransferPromotionIncrement = new TransferPromotionIncrement();
            TransferPromotionIncrement oTempTPI = new TransferPromotionIncrement();
            List<TransferPromotionIncrement> oTempTPIs = new List<TransferPromotionIncrement>();
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail = new EmployeeSalaryStructureDetail();
            TransactionContext tc = null;
            
            foreach (TransferPromotionIncrement oItem in oTPIs)
            {
                try
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;

                    //readerSaveTPI = TransferPromotionIncrementDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert);
                    //NullHandler oReaderSaveTPI = new NullHandler(readerSaveTPI);
                    //if (readerSaveTPI.Read())
                    //{

                    //    oTransferPromotionIncrement = CreateObject(oReaderSaveTPI);
                    //}
                    //readerSaveTPI.Close();


                    //readerApproveTPI = TransferPromotionIncrementDA.Approve(oTransferPromotionIncrement.TPIID, nUserID, tc);
                    //NullHandler oReaderApproveTPI = new NullHandler(readerApproveTPI);
                    //if (readerApproveTPI.Read())
                    //{

                    //    oTransferPromotionIncrement = CreateObject(oReaderApproveTPI);
                    //}
                    //readerApproveTPI.Close();

                    oItem.TPIID = oTransferPromotionIncrement.TPIID;
                    reader = TransferPromotionIncrementDA.UploadXLAsPerScheme(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {

                        oTransferPromotionIncrement = CreateObject(oReader);
                    }
                    reader.Close();


                    //readerSaveDetail = TransferPromotionIncrementDA.UploadXLAsPerScheme(tc, oItem, nUserID);
                    //NullHandler oReaderSaveDetail = new NullHandler(readerSaveDetail);
                    //if (readerSaveDetail.Read())
                    //{

                    //    oTransferPromotionIncrement = CreateObject(oReader);
                    //}
                    //readerSaveDetail.Close();

                    tc.End();
                }
                catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    oItem.ErrorMessage = e.Message.Contains("!") ? e.Message.Split('!')[0] : e.Message;
                    oTempTPIs.Add(oItem);
                    #endregion
                }
            }
            return oTempTPIs;
        }


        public List<TransferPromotionIncrement> UploadXL(List<TransferPromotionIncrement> oTPIs, Int64 nUserID)
        {
            TransferPromotionIncrement oTempTPI = new TransferPromotionIncrement();
            List<TransferPromotionIncrement> oTempTPIs = new List<TransferPromotionIncrement>();
            TransactionContext tc = null;
            try
            {
                int nCount = 0;
                foreach (TransferPromotionIncrement oItem in oTPIs)
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    oTempTPI = new TransferPromotionIncrement();
                    reader = TransferPromotionIncrementDA.UploadXL(tc, oItem, nUserID);
                    if (nCount < 100)
                    {
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oTempTPI = CreateObject(oReader);
                        }
                        if (oTempTPI.TPIID > 0)
                        {
                            oTempTPIs.Add(oTempTPI);
                            nCount++;
                        }
                    }
                    reader.Close();
                    tc.End();
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new ServiceException(e.Message.Split('!')[0]);
                #endregion
            }
            return oTempTPIs;
        }
        public List<TransferPromotionIncrement> UploadXLTP(List<TransferPromotionIncrement> oTPIList, Int64 nUserID)
        {
            TransferPromotionIncrement oTempTPI = new TransferPromotionIncrement();
            List<TransferPromotionIncrement> oTPIs = new List<TransferPromotionIncrement>();
            List<TransferPromotionIncrement> oTempTPIs = new List<TransferPromotionIncrement>();
            List<TransferPromotionIncrement> oTempList = new List<TransferPromotionIncrement>();
            TransactionContext tc = null;
            foreach (TransferPromotionIncrement oItem in oTPIList)
            {
                try
                {
                    tc = TransactionContext.Begin(true);

                    oTempTPIs = new List<TransferPromotionIncrement>();
                    IDataReader reader = null;
                    reader = TransferPromotionIncrementDA.UploadXLTP(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    //if (reader.RecordsAffected <= 0)
                    //{
                    //    oTempList.Add(oItem);
                    //}
                    if (reader.Read())
                    {
                        oTempTPI = CreateObject(oReader);
                    }

                    reader.Close();
                    tc.End();
                }
                catch (Exception e)
                {
                    if (tc != null)
                        tc.HandleError();

                    oItem.ErrorMessage = e.Message.Contains("!") ? e.Message.Split('!')[0] : e.Message;
                    oTempList.Add(oItem);
                }
            }
            return oTempList;
        }
        #endregion UploadXL

        #endregion

    }
}
