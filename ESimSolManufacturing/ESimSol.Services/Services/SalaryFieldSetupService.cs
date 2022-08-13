using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.Services
{
    public class SalaryFieldSetupService : MarshalByRefObject, ISalaryFieldSetupService
    {
        #region Private functions and declaration
        private SalaryFieldSetup MapObject(NullHandler oReader)
        {
            SalaryFieldSetup oSalaryFieldSetupDetail = new SalaryFieldSetup();
            oSalaryFieldSetupDetail.SalaryFieldSetupID = oReader.GetInt32("SalaryFieldSetupID");
            oSalaryFieldSetupDetail.SetupNo = oReader.GetString("SetupNo");
            oSalaryFieldSetupDetail.SalaryFieldSetupName = oReader.GetString("SalaryFieldSetupName");
            oSalaryFieldSetupDetail.PageOrientation = (EnumPageOrientation)oReader.GetInt32("PageOrientation");
            oSalaryFieldSetupDetail.PageOrientationInt = oReader.GetInt32("PageOrientation");
            oSalaryFieldSetupDetail.Remarks = oReader.GetString("Remarks");
            oSalaryFieldSetupDetail.DBUserID = oReader.GetInt32("DBUserID");
            oSalaryFieldSetupDetail.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oSalaryFieldSetupDetail.UserName = oReader.GetString("UserName");
            oSalaryFieldSetupDetail.LogInID = oReader.GetString("LogInID");
            return oSalaryFieldSetupDetail;
        }

        private List<SalaryFieldSetup> CreateObjects(IDataReader oReader)
        {
            List<SalaryFieldSetup> oSalaryFieldSetups = new List<SalaryFieldSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalaryFieldSetup oType = CreateObject(oHandler);
                oSalaryFieldSetups.Add(oType);
            }
            return oSalaryFieldSetups;
        }

        private SalaryFieldSetup CreateObject(NullHandler oReader)
        {
            SalaryFieldSetup oSalaryFieldSetup = new SalaryFieldSetup();
            oSalaryFieldSetup = MapObject(oReader);
            return oSalaryFieldSetup;
        }
        #endregion

        #region Interface implementation
        public SalaryFieldSetupService() { }

        public List<SalaryFieldSetup> Gets(Int64 nUserID)
        {
            List<SalaryFieldSetup> oSalaryFieldSetup = new List<SalaryFieldSetup>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalaryFieldSetupDA.Gets(tc);
                oSalaryFieldSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalaryFieldSetups", e);
                #endregion
            }

            return oSalaryFieldSetup;
        }

        public SalaryFieldSetup Get(int id, Int64 nUserId)
        {
            SalaryFieldSetup oSalaryFieldSetup = new SalaryFieldSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalaryFieldSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalaryFieldSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SalaryFieldSetup", e);
                #endregion
            }

            return oSalaryFieldSetup;
        }

        public SalaryFieldSetup Save(SalaryFieldSetup oSalaryFieldSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            oSalaryFieldSetup.ErrorMessage = "";

            string sSalaryFieldSetupDetailIDs = "";
            List <SalaryFieldSetupDetail> oSalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();
            SalaryFieldSetupDetail oSalaryFieldSetupDetail = new SalaryFieldSetupDetail();
            oSalaryFieldSetupDetails = oSalaryFieldSetup.SalaryFieldSetupDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSalaryFieldSetup.SalaryFieldSetupID <= 0)
                {
                    reader = SalaryFieldSetupDA.InsertUpdate(tc, oSalaryFieldSetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = SalaryFieldSetupDA.InsertUpdate(tc, oSalaryFieldSetup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalaryFieldSetup = new SalaryFieldSetup();
                    oSalaryFieldSetup = CreateObject(oReader);
                }
                reader.Close();

                #region Salary Field Setup Detail

                foreach (SalaryFieldSetupDetail oItem in oSalaryFieldSetupDetails)
                {
                    IDataReader readerdetail;
                    oItem.SalaryFieldSetupID = oSalaryFieldSetup.SalaryFieldSetupID;
                    if (oItem.SalaryFieldSetupDetailID <= 0)
                    {
                        readerdetail = SalaryFieldSetupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = SalaryFieldSetupDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sSalaryFieldSetupDetailIDs = sSalaryFieldSetupDetailIDs + oReaderDetail.GetString("SalaryFieldSetupDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sSalaryFieldSetupDetailIDs.Length > 0)
                {
                    sSalaryFieldSetupDetailIDs = sSalaryFieldSetupDetailIDs.Remove(sSalaryFieldSetupDetailIDs.Length - 1, 1);
                }
                oSalaryFieldSetupDetail = new SalaryFieldSetupDetail();
                oSalaryFieldSetupDetail.SalaryFieldSetupID = oSalaryFieldSetup.SalaryFieldSetupID;
                SalaryFieldSetupDetailDA.Delete(tc, oSalaryFieldSetupDetail, EnumDBOperation.Delete, nUserID, sSalaryFieldSetupDetailIDs);

                #endregion

                #region Get Salary Field Setup

                reader = SalaryFieldSetupDA.Get(tc, oSalaryFieldSetup.SalaryFieldSetupID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalaryFieldSetup = new SalaryFieldSetup();
                    oSalaryFieldSetup = CreateObject(oReader);
                }
                reader.Close();

                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSalaryFieldSetup = new SalaryFieldSetup();
                oSalaryFieldSetup.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oSalaryFieldSetup;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SalaryFieldSetup oSalaryFieldSetup = new SalaryFieldSetup();
                oSalaryFieldSetup.SalaryFieldSetupID = id;
                SalaryFieldSetupDA.Delete(tc, oSalaryFieldSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }
        #endregion
    }
}
