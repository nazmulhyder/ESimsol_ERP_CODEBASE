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
    public class OrderRecapYarnService : MarshalByRefObject, IOrderRecapYarnService
    {
        #region Private functions and declaration
        private OrderRecapYarn MapObject(NullHandler oReader)
        {
            OrderRecapYarn oOrderRecapYarn = new OrderRecapYarn();
            oOrderRecapYarn.OrderRecapYarnID = oReader.GetInt32("OrderRecapYarnID");
            oOrderRecapYarn.RefObjectID = oReader.GetInt32("RefObjectID");
            oOrderRecapYarn.OrderRecapYarnLogID = oReader.GetInt32("OrderRecapYarnLogID");
            oOrderRecapYarn.RefObjectLogID = oReader.GetInt32("RefObjectLogID");
            oOrderRecapYarn.YarnType = (EnumRecapYarnType)oReader.GetInt32("YarnType");
            oOrderRecapYarn.RefType =(EnumRecapRefType)oReader.GetInt32("RefType");

            oOrderRecapYarn.YarnID = oReader.GetInt32("YarnID");
            oOrderRecapYarn.YarnPly = oReader.GetString("YarnPly");
            oOrderRecapYarn.YarnCode = oReader.GetString("YarnCode");
            oOrderRecapYarn.YarnName = oReader.GetString("YarnName");
            oOrderRecapYarn.Note = oReader.GetString("Note");
            
            return oOrderRecapYarn;
        }

        private OrderRecapYarn CreateObject(NullHandler oReader)
        {
            OrderRecapYarn oOrderRecapYarn = new OrderRecapYarn();
            oOrderRecapYarn = MapObject(oReader);
            return oOrderRecapYarn;
        }

        private List<OrderRecapYarn> CreateObjects(IDataReader oReader)
        {
            List<OrderRecapYarn> oOrderRecapYarn = new List<OrderRecapYarn>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                OrderRecapYarn oItem = CreateObject(oHandler);
                oOrderRecapYarn.Add(oItem);
            }
            return oOrderRecapYarn;
        }

        #endregion

        #region Interface implementation
        public OrderRecapYarnService() { }

        public OrderRecapYarn Save(OrderRecapYarn oOrderRecapYarn, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oOrderRecapYarn.OrderRecapYarnID <= 0)
                {
                    reader = OrderRecapYarnDA.InsertUpdate(tc, oOrderRecapYarn, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    reader = OrderRecapYarnDA.InsertUpdate(tc, oOrderRecapYarn, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecapYarn = new OrderRecapYarn();
                    oOrderRecapYarn = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save OrderRecapYarn. Because of " + e.Message, e);
                #endregion
            }
            return oOrderRecapYarn;
        }

        public string Delete(int id, Int64 nUserId, string sOrderRecapYarnIDs)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                OrderRecapYarn oOrderRecapYarn = new OrderRecapYarn();
                oOrderRecapYarn.OrderRecapYarnID = id;
                OrderRecapYarnDA.Delete(tc, oOrderRecapYarn, EnumDBOperation.Delete, nUserId, sOrderRecapYarnIDs);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete OrderRecapYarn. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public OrderRecapYarn Get(int id, Int64 nUserId)
        {
            OrderRecapYarn oAccountHead = new OrderRecapYarn();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = OrderRecapYarnDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get OrderRecapYarn", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<OrderRecapYarn> Gets(Int64 nUserID)
        {
            List<OrderRecapYarn> oOrderRecapYarn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderRecapYarnDA.Gets(tc);
                oOrderRecapYarn = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderRecapYarn", e);
                #endregion
            }

            return oOrderRecapYarn;
        }

        public List<OrderRecapYarn> Gets(string sSQL, Int64 nUserID)
        {
            List<OrderRecapYarn> oOrderRecapYarn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderRecapYarnDA.Gets(tc, sSQL);
                oOrderRecapYarn = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderRecapYarn", e);
                #endregion
            }

            return oOrderRecapYarn;
        }

        public List<OrderRecapYarn> Gets(int nRefID, int nRefType, Int64 nUserID)
        {
            List<OrderRecapYarn> oOrderRecapYarn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderRecapYarnDA.Gets(tc, nRefID, nRefType);
                oOrderRecapYarn = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderRecapYarn", e);
                #endregion
            }

            return oOrderRecapYarn;
        }

        public List<OrderRecapYarn> GetsByLog(int id, Int64 nUserID)
        {
            List<OrderRecapYarn> oOrderRecapYarn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderRecapYarnDA.GetsByLog(tc, id);
                oOrderRecapYarn = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderRecapYarn", e);
                #endregion
            }

            return oOrderRecapYarn;
        }      
        #endregion
    }
}
