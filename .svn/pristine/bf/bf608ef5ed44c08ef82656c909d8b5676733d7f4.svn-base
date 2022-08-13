using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ApprovalHeadPersonService : MarshalByRefObject, IApprovalHeadPersonService
    {
        #region Private functions and declaration
        private ApprovalHeadPerson MapObject(NullHandler oReader)
        {
            ApprovalHeadPerson oApprovalHeadPerson = new ApprovalHeadPerson();
            oApprovalHeadPerson.ApprovalHeadPersonID = oReader.GetInt32("ApprovalHeadPersonID");
            oApprovalHeadPerson.ApprovalHeadID = oReader.GetInt32("ApprovalHeadID");
            oApprovalHeadPerson.UserID = oReader.GetInt32("UserID");
            oApprovalHeadPerson.UserName = oReader.GetString("UserName");
            oApprovalHeadPerson.EmployeeName = oReader.GetString("EmployeeName");
            oApprovalHeadPerson.Note = oReader.GetString("Note");
            oApprovalHeadPerson.IsActive = oReader.GetBoolean("IsActive");
            return oApprovalHeadPerson;

        }

        private ApprovalHeadPerson CreateObject(NullHandler oReader)
        {
            ApprovalHeadPerson oApprovalHeadPerson = MapObject(oReader);
            return oApprovalHeadPerson;
        }

        private List<ApprovalHeadPerson> CreateObjects(IDataReader oReader)
        {
            List<ApprovalHeadPerson> oApprovalHeadPerson = new List<ApprovalHeadPerson>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ApprovalHeadPerson oItem = CreateObject(oHandler);
                oApprovalHeadPerson.Add(oItem);
            }
            return oApprovalHeadPerson;
        }


        #endregion

        #region Interface implementation
        public ApprovalHeadPersonService() { }

        public ApprovalHead IUD(ApprovalHead oApprovalHead, int nDBOperation, Int64 nUserID)
        {
            List<ApprovalHeadPerson> oApprovalHeadPersons = new List<ApprovalHeadPerson>();
            oApprovalHeadPersons = oApprovalHead.ApprovalHeadPersons;
            TransactionContext tc = null;
            
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {

                    if (oApprovalHeadPersons.Any())
                    {
                        foreach (ApprovalHeadPerson oItem in oApprovalHeadPersons)
                        {
                            IDataReader reader;
                            reader = ApprovalHeadPersonDA.IUD(tc, oItem, nUserID, (int)EnumDBOperation.Insert);

                            NullHandler oReaderDetail = new NullHandler(reader);
                            if (reader.Read())
                            {
                                ApprovalHeadPerson oApprovalHeadPerson = new ApprovalHeadPerson();
                                oApprovalHeadPerson = CreateObject(oReaderDetail);
                            }
                            reader.Close();
                        }
                    }
                }
                IDataReader newreader = null;
                newreader = ApprovalHeadPersonDA.Gets(oApprovalHead.ApprovalHeadID, tc);
                oApprovalHead.ApprovalHeadPersons = CreateObjects(newreader);
                newreader.Close();
                //else if (nDBOperation == (int)EnumDBOperation.Delete)
                //{
                //    ApprovalHeadPerson oApprovalHeadPerson = new ApprovalHeadPerson();
                //    oApprovalHeadPerson.ApprovalHeadPersonID
                //    readerDetails = ApprovalHeadPersonDA.IUD(tc, oApprovalHeadPerson, nUserID, nDBOperation);
                //    NullHandler oReader = new NullHandler(readerDetails);
                //    readerDetails.Close();
                //    oApprovalHeadPerson.ErrorMessage = Global.DeleteMessage;
                //}

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                 oApprovalHead = new ApprovalHead();
                oApprovalHead.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }

            return oApprovalHead;
        }


        public String Delete(ApprovalHeadPerson oApprovalHeadPerson, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ApprovalHeadPersonDA.Delete(tc, oApprovalHeadPerson, nUserID, (int)EnumDBOperation.Delete);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }


        public ApprovalHeadPerson Activate(ApprovalHeadPerson oApprovalHeadPerson, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ApprovalHeadPersonDA.Activate(tc, oApprovalHeadPerson);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oApprovalHeadPerson = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oApprovalHeadPerson = new ApprovalHeadPerson();
                oApprovalHeadPerson.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oApprovalHeadPerson;
        }


        public ApprovalHeadPerson Get(string sSQL, Int64 nUserId)
        {
            ApprovalHeadPerson oApprovalHeadPerson = new ApprovalHeadPerson();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ApprovalHeadPersonDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oApprovalHeadPerson = CreateObject(oReader);
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

            return oApprovalHeadPerson;
        }

        public List<ApprovalHeadPerson> Gets(string sSQL, Int64 nUserID)
        {
            List<ApprovalHeadPerson> oApprovalHeadPerson = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ApprovalHeadPersonDA.Gets(sSQL, tc);
                oApprovalHeadPerson = CreateObjects(reader);
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
            return oApprovalHeadPerson;
        }
        #endregion
    }
}


