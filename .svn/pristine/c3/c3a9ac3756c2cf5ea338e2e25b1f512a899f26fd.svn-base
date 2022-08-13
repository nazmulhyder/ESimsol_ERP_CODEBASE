using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{

    public class ProformaInvoiceTermsAndConditionService : MarshalByRefObject, IProformaInvoiceTermsAndConditionService
    {
        #region Private functions and declaration
        private ProformaInvoiceTermsAndCondition MapObject(NullHandler oReader)
        {
            ProformaInvoiceTermsAndCondition oProformaInvoiceTermsAndCondition = new ProformaInvoiceTermsAndCondition();
            oProformaInvoiceTermsAndCondition.ProformaInvoiceTermsAndConditionID = oReader.GetInt32("ProformaInvoiceTermsAndConditionID");
            oProformaInvoiceTermsAndCondition.ProformaInvoiceID = oReader.GetInt32("ProformaInvoiceID");
            oProformaInvoiceTermsAndCondition.ProformaInvoiceTermsAndConditionLogID = oReader.GetInt32("ProformaInvoiceTermsAndConditionLogID");
            oProformaInvoiceTermsAndCondition.ProformaInvoiceLogID = oReader.GetInt32("ProformaInvoiceLogID");
            oProformaInvoiceTermsAndCondition.TermsAndCondition = oReader.GetString("TermsAndCondition");
            return oProformaInvoiceTermsAndCondition;
        }

        private ProformaInvoiceTermsAndCondition CreateObject(NullHandler oReader)
        {
            ProformaInvoiceTermsAndCondition oProformaInvoiceTermsAndCondition = new ProformaInvoiceTermsAndCondition();
            oProformaInvoiceTermsAndCondition = MapObject(oReader);
            return oProformaInvoiceTermsAndCondition;
        }

        private List<ProformaInvoiceTermsAndCondition> CreateObjects(IDataReader oReader)
        {
            List<ProformaInvoiceTermsAndCondition> oProformaInvoiceTermsAndCondition = new List<ProformaInvoiceTermsAndCondition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProformaInvoiceTermsAndCondition oItem = CreateObject(oHandler);
                oProformaInvoiceTermsAndCondition.Add(oItem);
            }
            return oProformaInvoiceTermsAndCondition;
        }

        #endregion

        #region Interface implementation
        public ProformaInvoiceTermsAndConditionService() { }

        public ProformaInvoiceTermsAndCondition Save(ProformaInvoiceTermsAndCondition oProformaInvoiceTermsAndCondition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProformaInvoiceTermsAndCondition.ProformaInvoiceTermsAndConditionID <= 0)
                {
                    reader = ProformaInvoiceTermsAndConditionDA.InsertUpdate(tc, oProformaInvoiceTermsAndCondition, EnumDBOperation.Insert,"", nUserId);
                }
                else
                {
                    reader = ProformaInvoiceTermsAndConditionDA.InsertUpdate(tc, oProformaInvoiceTermsAndCondition, EnumDBOperation.Update,"", nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProformaInvoiceTermsAndCondition = new ProformaInvoiceTermsAndCondition();
                    oProformaInvoiceTermsAndCondition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProformaInvoiceTermsAndCondition.ErrorMessage = e.Message;
                #endregion
            }
            return oProformaInvoiceTermsAndCondition;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProformaInvoiceTermsAndCondition oProformaInvoiceTermsAndCondition = new ProformaInvoiceTermsAndCondition();
                oProformaInvoiceTermsAndCondition.ProformaInvoiceTermsAndConditionID = id;
                ProformaInvoiceTermsAndConditionDA.Delete(tc, oProformaInvoiceTermsAndCondition, EnumDBOperation.Delete,"", nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }

        public string ProformaInvoiceTermsAndConditionSave(List<ProformaInvoiceTermsAndCondition> oProformaInvoiceTermsAndConditions, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<ProformaInvoiceTermsAndCondition> _oProformaInvoiceTermsAndConditions = new List<ProformaInvoiceTermsAndCondition>();
            ProformaInvoiceTermsAndCondition oProformaInvoiceTermsAndCondition = new ProformaInvoiceTermsAndCondition();
            oProformaInvoiceTermsAndCondition.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (ProformaInvoiceTermsAndCondition oItem in oProformaInvoiceTermsAndConditions)
                {
                    IDataReader reader;
                    reader = ProformaInvoiceTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, "",nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oProformaInvoiceTermsAndCondition = new ProformaInvoiceTermsAndCondition();
                        oProformaInvoiceTermsAndCondition = CreateObject(oReader);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProformaInvoiceTermsAndCondition.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ProformaInvoiceTermsAndCondition. Because of " + e.Message, e);
                #endregion
            }
            return "Save Successfully !";
        }

        public ProformaInvoiceTermsAndCondition Get(int id, Int64 nUserId)
        {
            ProformaInvoiceTermsAndCondition oAccountHead = new ProformaInvoiceTermsAndCondition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProformaInvoiceTermsAndConditionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ProformaInvoiceTermsAndCondition", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ProformaInvoiceTermsAndCondition> Gets(int id, Int64 nUserID)
        {
            List<ProformaInvoiceTermsAndCondition> oProformaInvoiceTermsAndCondition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceTermsAndConditionDA.Gets(tc, id);
                oProformaInvoiceTermsAndCondition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoiceTermsAndCondition", e);
                #endregion
            }

            return oProformaInvoiceTermsAndCondition;
        }

        public List<ProformaInvoiceTermsAndCondition> GetsPILog(int id, Int64 nUserID)
        {
            List<ProformaInvoiceTermsAndCondition> oProformaInvoiceTermsAndCondition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceTermsAndConditionDA.GetsPILog(tc, id);
                oProformaInvoiceTermsAndCondition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoiceTermsAndCondition", e);
                #endregion
            }

            return oProformaInvoiceTermsAndCondition;
        }
        

        public List<ProformaInvoiceTermsAndCondition> Gets(string sSQL, Int64 nUserID)
        {
            List<ProformaInvoiceTermsAndCondition> oProformaInvoiceTermsAndCondition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceTermsAndConditionDA.Gets(tc, sSQL);
                oProformaInvoiceTermsAndCondition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoiceTermsAndCondition", e);
                #endregion
            }

            return oProformaInvoiceTermsAndCondition;
        }
        #endregion
    }
   
}
