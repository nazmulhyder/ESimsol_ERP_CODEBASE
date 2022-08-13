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
    public class PerformanceIncentiveService : MarshalByRefObject, IPerformanceIncentiveService
    {
        #region Private functions and declaration
        private PerformanceIncentive MapObject(NullHandler oReader)
        {
            PerformanceIncentive oPerformanceIncentive = new PerformanceIncentive();

            oPerformanceIncentive.PIID = oReader.GetInt32("PIID");
            oPerformanceIncentive.Code = oReader.GetString("Code");
            oPerformanceIncentive.Name = oReader.GetString("Name");
            oPerformanceIncentive.Description = oReader.GetString("Description");
            oPerformanceIncentive.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oPerformanceIncentive.ApproveBy = oReader.GetInt32("ApproveBy");
            oPerformanceIncentive.ApproveDate = oReader.GetDateTime("ApproveDate");
            oPerformanceIncentive.InactiveBy = oReader.GetInt32("InactiveBy");
            oPerformanceIncentive.InactiveDate = oReader.GetDateTime("InactiveDate");
            oPerformanceIncentive.EncryptPIID = Global.Encrypt(oPerformanceIncentive.PIID.ToString());
            oPerformanceIncentive.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oPerformanceIncentive.InactiveByName = oReader.GetString("InactiveByName");
            oPerformanceIncentive.ApproveByName = oReader.GetString("ApproveByName");

            return oPerformanceIncentive;

        }

        private PerformanceIncentive CreateObject(NullHandler oReader)
        {
            PerformanceIncentive oPerformanceIncentive = MapObject(oReader);
            return oPerformanceIncentive;
        }

        private List<PerformanceIncentive> CreateObjects(IDataReader oReader)
        {
            List<PerformanceIncentive> oPerformanceIncentives = new List<PerformanceIncentive>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PerformanceIncentive oItem = CreateObject(oHandler);
                oPerformanceIncentives.Add(oItem);
            }
            return oPerformanceIncentives;
        }

        #endregion

        #region Interface implementation
        public PerformanceIncentiveService() { }

        public PerformanceIncentive IUD(PerformanceIncentive oPerformanceIncentive, int nDBOperation, Int64 nUserID)
        {

            List<PerformanceIncentiveSlab> oPerformanceIncentiveSlabs = new List<PerformanceIncentiveSlab>();
            oPerformanceIncentiveSlabs = oPerformanceIncentive.PerformanceIncentiveSlabs;
            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PerformanceIncentiveDA.IUD(tc, oPerformanceIncentive, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oPerformanceIncentive = CreateObject(oReader);
                }
                reader.Close();
                //#region PerformanceIncentiveSlabPart
                //if (nDBOperation != 3)
                //{
                //    if (oPerformanceIncentive.PIID > 0)
                //    {
                //        foreach (PerformanceIncentiveSlab oItem in oPerformanceIncentiveSlabs)
                //        {
                //            IDataReader readerDetail;
                //            oItem.PIID = oPerformanceIncentive.PIID;
                //            if (oItem.PISlabID <= 0)
                //            {
                //                readerDetail = PerformanceIncentiveSlabDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert);
                //            }
                //            else
                //            {
                //                readerDetail = PerformanceIncentiveSlabDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Update);

                //            }
                //            NullHandler oReaderDetail = new NullHandler(readerDetail);
                //            readerDetail.Close();
                //        }
                //    }

                //}
                //#endregion PerformanceIncentiveSlabPart

                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oPerformanceIncentive.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPerformanceIncentive.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oPerformanceIncentive.PIID = 0;
                #endregion
            }
            return oPerformanceIncentive;
        }


        public PerformanceIncentive Get(int nPIID, Int64 nUserId)
        {
            PerformanceIncentive oPerformanceIncentive = new PerformanceIncentive();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PerformanceIncentiveDA.Get(nPIID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentive = CreateObject(oReader);
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

                oPerformanceIncentive.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentive;
        }

        public PerformanceIncentive Get(string sSQL, Int64 nUserId)
        {
            PerformanceIncentive oPerformanceIncentive = new PerformanceIncentive();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PerformanceIncentiveDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentive = CreateObject(oReader);
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

                oPerformanceIncentive.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentive;
        }

        public List<PerformanceIncentive> Gets(Int64 nUserID)
        {
            List<PerformanceIncentive> oPerformanceIncentive = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PerformanceIncentiveDA.Gets(tc);
                oPerformanceIncentive = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PerformanceIncentive", e);
                #endregion
            }
            return oPerformanceIncentive;
        }

        public List<PerformanceIncentive> Gets(string sSQL, Int64 nUserID)
        {
            List<PerformanceIncentive> oPerformanceIncentive = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PerformanceIncentiveDA.Gets(sSQL, tc);
                oPerformanceIncentive = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PerformanceIncentive", e);
                #endregion
            }
            return oPerformanceIncentive;
        }

        #region Activity
        public PerformanceIncentive InActive(int nPIID, Int64 nUserId)
        {
            PerformanceIncentive oPerformanceIncentive = new PerformanceIncentive();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PerformanceIncentiveDA.InActive(nPIID, nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentive = CreateObject(oReader);
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
                oPerformanceIncentive.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentive;
        }


        #endregion

        #region Approve
        public PerformanceIncentive Approve(int nPIID, Int64 nUserId)
        {
            PerformanceIncentive oPerformanceIncentive = new PerformanceIncentive();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PerformanceIncentiveDA.Approve(nPIID, nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentive = CreateObject(oReader);
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
                oPerformanceIncentive.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentive;
        }


        #endregion

        #region Approve
        public PerformanceIncentive PerformanceIncentive_Transfer(int PreviousPIID, int PresentPIID, Int64 nUserId)
        {
            PerformanceIncentive oPerformanceIncentive = new PerformanceIncentive();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PerformanceIncentiveDA.PerformanceIncentive_Transfer(PreviousPIID, PresentPIID, nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentive = CreateObject(oReader);
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
                oPerformanceIncentive.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentive;
        }
        #endregion
        #endregion


    }
}
