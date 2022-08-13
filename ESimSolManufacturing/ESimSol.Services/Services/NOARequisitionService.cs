using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.Services.Services
{
    public class NOARequisitionService : MarshalByRefObject, INOARequisitionService
    {
        #region Private functions and declaration
        public static NOARequisition MapObject(NullHandler oReader)
        {
            NOARequisition oNOARequisition = new NOARequisition();
            oNOARequisition.NOARequisitionLogID = oReader.GetInt32("NOARequisitionLogID");
            oNOARequisition.NOALogID = oReader.GetInt32("NOALogID");
            oNOARequisition.NOARequisitionID = oReader.GetInt32("NOARequisitionID");
            oNOARequisition.NOAID = oReader.GetInt32("NOAID");
            oNOARequisition.PRID = oReader.GetInt32("PRID");
            oNOARequisition.PrepareByName = oReader.GetString("PrepareByName");
            oNOARequisition.PRNo = oReader.GetString("PRNo");
            oNOARequisition.PRDate = oReader.GetDateTime("PRDate");
            oNOARequisition.RequirementDate = oReader.GetDateTime("RequirementDate");
            oNOARequisition.RequisitionByName = oReader.GetString("RequisitionByName");
            oNOARequisition.ApprovedByName = oReader.GetString("ApprovedByName");
            oNOARequisition.Note = oReader.GetString("Note");

            return oNOARequisition;
        }
        public static NOARequisition CreateObject(NullHandler oReader)
        {
            NOARequisition oNOARequisition = new NOARequisition();
            oNOARequisition = MapObject(oReader);
            return oNOARequisition;
        }
        public static List<NOARequisition> CreateObjects(IDataReader oReader)
        {
            List<NOARequisition> oNOARequisition = new List<NOARequisition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                NOARequisition oItem = CreateObject(oHandler);
                oNOARequisition.Add(oItem);
            }
            return oNOARequisition;
        }
        #endregion

        #region Interface implementation
        public List<NOARequisition> Save(NOARequisition oNOARequisition, int nUserID)
        {
            TransactionContext tc = null;
            List<NOARequisition> oNOARequisitions = new List<NOARequisition>();
            oNOARequisitions = oNOARequisition.NOARequisitions;
            int nNOAID = oNOARequisitions[0].NOAID;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach(NOARequisition oItem in oNOARequisitions)
                {
                    IDataReader reader;
                    NullHandler oReader;
                    reader = NOARequisitionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oNOARequisition = new NOARequisition();
                        oNOARequisition = CreateObject(oReader);
                    }
                    reader.Close();
                }
                IDataReader readerNOA;
                readerNOA = NOARequisitionDA.Gets("SELECT * FROM View_NOARequisition WHERE NOAID = "+nNOAID, tc);
                oNOARequisitions = CreateObjects(readerNOA);
                readerNOA.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oNOARequisition = new NOARequisition();
                oNOARequisition.ErrorMessage = e.Message;
                #endregion
            }


            return oNOARequisitions;
        }

        public NOARequisition Get(int id, int nUserId)
        {
            NOARequisition oAccountHead = new NOARequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = NOARequisitionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                #endregion
            }

            return oAccountHead;
        }

        public string Delete(int nNOARequisitionID, int nUserId)
        {
            TransactionContext tc = null;
            List<NOARequisition> oNOARequisitions = new List<NOARequisition>();

            try
            {
                tc = TransactionContext.Begin(true);
                NOARequisition oNOARequisition = new NOARequisition();
                oNOARequisition.NOARequisitionID = nNOARequisitionID;
                NOARequisitionDA.Delete(tc, oNOARequisition, EnumDBOperation.Delete, nUserId, "");
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
            return "Succefully Deleted";
        }

        public List<NOARequisition> Gets(int nUserId)
        {
            List<NOARequisition> oNOARequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOARequisitionDA.Gets(tc);
                oNOARequisition = CreateObjects(reader);
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

            return oNOARequisition;
        }
        
        public List<NOARequisition> Gets(string sSQL, int nUserId)
        {
            List<NOARequisition> oNOARequisitions = new List<NOARequisition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOARequisitionDA.Gets(sSQL, tc);
                oNOARequisitions = CreateObjects(reader);
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

            return oNOARequisitions;
        }
        public List<NOARequisition> Gets(long nNOADetailId, int nUserId)
        {
            List<NOARequisition> oNOARequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOARequisitionDA.Gets(tc, nNOADetailId);
                oNOARequisition = CreateObjects(reader);
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

            return oNOARequisition;
        }
        #endregion
    }   
}
