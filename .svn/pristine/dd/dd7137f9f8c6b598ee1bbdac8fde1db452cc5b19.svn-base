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
    public class NOAQuotationService : MarshalByRefObject, INOAQuotationService
    {
        #region Private functions and declaration
        public static NOAQuotation MapObject(NullHandler oReader)
        {
            NOAQuotation oNOAQuotation = new NOAQuotation();
            oNOAQuotation.NOAQuotationLogID = oReader.GetInt32("NOAQuotationLogID");
            oNOAQuotation.NOADetailLogID = oReader.GetInt32("NOADetailLogID");
            oNOAQuotation.NOAQuotationID = oReader.GetInt32("NOAQuotationID");
            oNOAQuotation.NOAID = oReader.GetInt32("NOAID");
            oNOAQuotation.PQID = oReader.GetInt32("PQID");
            oNOAQuotation.NOADetailID = oReader.GetInt32("NOADetailID");
            oNOAQuotation.PQDetailID = oReader.GetInt32("PQDetailID");
            oNOAQuotation.SupplierID = oReader.GetInt32("SupplierID");
            oNOAQuotation.UnitPrice = oReader.GetDouble("UnitPrice");
            oNOAQuotation.SupplierName = oReader.GetString("SupplierName");
            oNOAQuotation.ShortName = oReader.GetString("ShortName");
            oNOAQuotation.PQNo = oReader.GetString("PurchaseQuotationNo");
            oNOAQuotation.DiscountInAmount = oReader.GetDouble("DiscountInAmount");
            oNOAQuotation.DiscountInpercent = oReader.GetDouble("DiscountInpercent");
            oNOAQuotation.VatInPercent = oReader.GetDouble("VatInPercent");
            oNOAQuotation.VatInAmount = oReader.GetDouble("VatInAmount");
            oNOAQuotation.TransportCostInPercent = oReader.GetDouble("TransportCostInPercent");
            oNOAQuotation.TransportCostInAmount = oReader.GetDouble("TransportCostInAmount");
            return oNOAQuotation;
        }
        public static NOAQuotation CreateObject(NullHandler oReader)
        {
            NOAQuotation oNOAQuotation = new NOAQuotation();
            oNOAQuotation = MapObject(oReader);
            return oNOAQuotation;
        }
        public static List<NOAQuotation> CreateObjects(IDataReader oReader)
        {
            List<NOAQuotation> oNOAQuotation = new List<NOAQuotation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                NOAQuotation oItem = CreateObject(oHandler);
                oNOAQuotation.Add(oItem);
            }
            return oNOAQuotation;
        }
        #endregion

        #region Interface implementation
        public List<NOAQuotation> Save(NOAQuotation oNOAQuotation, int nUserID)
        {
            TransactionContext tc = null;
            List<NOAQuotation> oNOAQuotations = new List<NOAQuotation>();
            oNOAQuotations = oNOAQuotation.NOAQuotations;
            int nNOADetailID = oNOAQuotations[0].NOADetailID;
            string sDetailIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                foreach(NOAQuotation oItem in oNOAQuotations)
                {
                    IDataReader reader;
                    NullHandler oReader;
                    if (oItem.NOAQuotationID <= 0)
                    {
                        reader = NOAQuotationDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                    }
                    else
                    {
                        reader = NOAQuotationDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        sDetailIDs = sDetailIDs + oReader.GetString("NOAQuotationID") + ",";   
                    }
                    reader.Close();
                }
                if (sDetailIDs.Length > 0)
                {
                    sDetailIDs = sDetailIDs.Remove(sDetailIDs.Length - 1, 1);
                }
                oNOAQuotation = new NOAQuotation();
                oNOAQuotation.NOADetailID = nNOADetailID;
                NOAQuotationDA.Delete(tc, oNOAQuotation, EnumDBOperation.Delete, nUserID, sDetailIDs);

                IDataReader readerNOA;
                readerNOA = NOAQuotationDA.Gets("SELECT * FROM View_NOAQuotation WHERE NOADetailID = "+nNOADetailID, tc);
                oNOAQuotations = CreateObjects(readerNOA);
                readerNOA.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oNOAQuotation = new NOAQuotation();
                oNOAQuotation.ErrorMessage = e.Message;
                #endregion
            }


            return oNOAQuotations;
        }

        public NOAQuotation Get(int id, int nUserId)
        {
            NOAQuotation oAccountHead = new NOAQuotation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = NOAQuotationDA.Get(tc, id);
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

        public string Delete(int nNOAQuotationID, int nUserId)
        {
            TransactionContext tc = null;
            List<NOAQuotation> oNOAQuotations = new List<NOAQuotation>();

            try
            {
                tc = TransactionContext.Begin(true);
                NOAQuotation oNOAQuotation = new NOAQuotation();
                oNOAQuotation.NOAQuotationID = nNOAQuotationID;
                NOAQuotationDA.Delete(tc, oNOAQuotation, EnumDBOperation.Delete, nUserId, "");
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

        public List<NOAQuotation> Gets(int nUserId)
        {
            List<NOAQuotation> oNOAQuotation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOAQuotationDA.Gets(tc);
                oNOAQuotation = CreateObjects(reader);
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

            return oNOAQuotation;
        }

        public List<NOAQuotation> Gets(string sSQL, int nUserId)
        {
            List<NOAQuotation> oNOAQuotations = new List<NOAQuotation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOAQuotationDA.Gets(sSQL, tc);
                oNOAQuotations = CreateObjects(reader);
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

            return oNOAQuotations;
        }
        public List<NOAQuotation> GetsByLog(string sSQL, int nUserId)
        {
            List<NOAQuotation> oNOAQuotations = new List<NOAQuotation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOAQuotationDA.GetsByLog(sSQL, tc);
                oNOAQuotations = CreateObjects(reader);
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

            return oNOAQuotations;
        }
        public List<NOAQuotation> Gets(long nNOADetailId, int nUserId)
        {
            List<NOAQuotation> oNOAQuotation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOAQuotationDA.Gets(tc, nNOADetailId);
                oNOAQuotation = CreateObjects(reader);
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

            return oNOAQuotation;
        }
        #endregion
    }   
}
