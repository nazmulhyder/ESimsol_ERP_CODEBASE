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
    public class ORAssortmentService : MarshalByRefObject, IORAssortmentService
    {
        #region Private functions and declaration
        private ORAssortment MapObject(NullHandler oReader)
        {
            ORAssortment oORAssortment = new ORAssortment();
            oORAssortment.ORAssortmentID = oReader.GetInt32("ORAssortmentID");
            oORAssortment.OrderRecapID = oReader.GetInt32("OrderRecapID");            
            oORAssortment.ColorID = oReader.GetInt32("ColorID");
            oORAssortment.SizeID = oReader.GetInt32("SizeID");
            oORAssortment.Qty = oReader.GetDouble("Qty");
            oORAssortment.ColorName = oReader.GetString("ColorName");
            oORAssortment.SizeName = oReader.GetString("SizeName");
            oORAssortment.ORAssortmentLogID = oReader.GetInt32("ORAssortmentLogID");
            oORAssortment.OrderRecapLogID = oReader.GetInt32("OrderRecapLogID");
            return oORAssortment;
        }

        private ORAssortment CreateObject(NullHandler oReader)
        {
            ORAssortment oORAssortment = new ORAssortment();
            oORAssortment = MapObject(oReader);
            return oORAssortment;
        }

        private List<ORAssortment> CreateObjects(IDataReader oReader)
        {
            List<ORAssortment> oORAssortment = new List<ORAssortment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ORAssortment oItem = CreateObject(oHandler);
                oORAssortment.Add(oItem);
            }
            return oORAssortment;
        }

        #endregion

        #region Interface implementation
        public ORAssortmentService() { }

        public ORAssortment Save(ORAssortment oORAssortment, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ORAssortment> _oORAssortments = new List<ORAssortment>();
            oORAssortment.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ORAssortmentDA.InsertUpdate(tc, oORAssortment, EnumDBOperation.Update, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oORAssortment = new ORAssortment();
                    oORAssortment = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oORAssortment.ErrorMessage = e.Message;
                #endregion
            }
            return oORAssortment;
        }

        public ORAssortment Get(int nORAssortmentID, Int64 nUserId)
        {
            ORAssortment oAccountHead = new ORAssortment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ORAssortmentDA.Get(tc, nORAssortmentID);
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
                throw new ServiceException("Failed to Get ORAssortment", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ORAssortment> Gets(int nOrderRecapID, Int64 nUserID)
        {
            List<ORAssortment> oORAssortment = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ORAssortmentDA.Gets(nOrderRecapID, tc);
                oORAssortment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ORAssortment", e);
                #endregion
            }

            return oORAssortment;
        }

        public List<ORAssortment> GetsByLog(int id, Int64 nUserId)
        {
            List<ORAssortment> oORAssortments = new List<ORAssortment> ();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ORAssortmentDA.GetsByLog(tc, id);
                oORAssortments = CreateObjects(reader);
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

            return oORAssortments;
        }

        public List<ORAssortment> Gets(string sSQL, Int64 nUserID)
        {
            List<ORAssortment> oORAssortment = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ORAssortmentDA.Gets(tc, sSQL);
                oORAssortment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ORAssortment", e);
                #endregion
            }
            return oORAssortment;
        }
        #endregion
    }
}
