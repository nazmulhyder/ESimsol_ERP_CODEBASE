using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{

    public class POTandCClauseService : MarshalByRefObject, IPOTandCClauseService
    {
        #region Private functions and declaration
        private POTandCClause MapObject(NullHandler oReader)
        {
            POTandCClause oPOTandCClause = new POTandCClause();
            oPOTandCClause.POTandCClauseID = oReader.GetInt32("POTandCClauseID");
            oPOTandCClause.POID = oReader.GetInt32("POID");
            oPOTandCClause.ClauseType = oReader.GetInt32("ClauseType");
            
            //oPOTandCClause.POTandCClauseLogID = oReader.GetInt32("POTandCClauseLogID");
            //oPOTandCClause.ProformaInvoiceLogID = oReader.GetInt32("ProformaInvoiceLogID");
            oPOTandCClause.TermsAndCondition = oReader.GetString("TermsAndCondition");
            return oPOTandCClause;
        }

        private POTandCClause CreateObject(NullHandler oReader)
        {
            POTandCClause oPOTandCClause = new POTandCClause();
            oPOTandCClause = MapObject(oReader);
            return oPOTandCClause;
        }

        private List<POTandCClause> CreateObjects(IDataReader oReader)
        {
            List<POTandCClause> oPOTandCClause = new List<POTandCClause>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                POTandCClause oItem = CreateObject(oHandler);
                oPOTandCClause.Add(oItem);
            }
            return oPOTandCClause;
        }

        #endregion

        #region Interface implementation
        public POTandCClauseService() { }

        public POTandCClause Save(POTandCClause oPOTandCClause, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPOTandCClause.POTandCClauseID <= 0)
                {
                    reader = POTandCClauseDA.InsertUpdate(tc, oPOTandCClause, EnumDBOperation.Insert, nUserId,"");
                }
                else
                {
                    reader = POTandCClauseDA.InsertUpdate(tc, oPOTandCClause, EnumDBOperation.Update, nUserId,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPOTandCClause = new POTandCClause();
                    oPOTandCClause = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPOTandCClause.ErrorMessage = e.Message;
                #endregion
            }
            return oPOTandCClause;
        }

        public string Delete(POTandCClause oPOTandCClause, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                POTandCClauseDA.Delete(tc, oPOTandCClause, EnumDBOperation.Delete, nUserId, "");
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
            return Global.DeleteMessage;
        }

        public string POTandCClauseSave(List<POTandCClause> oPOTandCClauses, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<POTandCClause> _oPOTandCClauses = new List<POTandCClause>();
            POTandCClause oPOTandCClause = new POTandCClause();
            oPOTandCClause.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (POTandCClause oItem in oPOTandCClauses)
                {
                    IDataReader reader;

                    if (oItem.POTandCClauseID <= 0)
                    {
                        reader = POTandCClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                    }
                    else
                    {
                        reader = POTandCClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                    }


                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPOTandCClause = new POTandCClause();
                        oPOTandCClause = CreateObject(oReader);
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

                oPOTandCClause.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save POTandCClause. Because of " + e.Message, e);
                #endregion
            }
            return "Save Successfully !";
        }

        public POTandCClause Get(int id, Int64 nUserId)
        {
            POTandCClause oAccountHead = new POTandCClause();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = POTandCClauseDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get POTandCClause", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<POTandCClause> Gets(int nPOID, Int64 nUserID)
        {
            List<POTandCClause> oPOTandCClause = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = POTandCClauseDA.Gets(tc, nPOID);
                oPOTandCClause = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get POTandCClause", e);
                #endregion
            }

            return oPOTandCClause;
        }

        public List<POTandCClause> GetsPOLog(int id, Int64 nUserID)
        {
            List<POTandCClause> oPOTandCClause = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = POTandCClauseDA.GetsPILog(tc, id);
                oPOTandCClause = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get POTandCClause", e);
                #endregion
            }

            return oPOTandCClause;
        }


        public List<POTandCClause> Gets(string sSQL, Int64 nUserID)
        {
            List<POTandCClause> oPOTandCClause = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = POTandCClauseDA.Gets(tc, sSQL);
                oPOTandCClause = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get POTandCClause", e);
                #endregion
            }

            return oPOTandCClause;
        }
        #endregion
    }

}
