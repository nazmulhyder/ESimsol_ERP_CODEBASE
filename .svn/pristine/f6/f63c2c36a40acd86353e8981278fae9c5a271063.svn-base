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
    public class ORBarCodeService : MarshalByRefObject, IORBarCodeService
    {
        #region Private functions and declaration
        private ORBarCode MapObject(NullHandler oReader)
        {
            ORBarCode oORBarCode = new ORBarCode();
            oORBarCode.ORBarCodeID = oReader.GetInt32("ORBarCodeID");
            oORBarCode.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oORBarCode.ColorID = oReader.GetInt32("ColorID");
            oORBarCode.SizeID = oReader.GetInt32("SizeID");
            oORBarCode.BarCode = oReader.GetString("BarCode");
            oORBarCode.ColorName = oReader.GetString("ColorName");
            oORBarCode.SizeName = oReader.GetString("SizeName");
            oORBarCode.ORBarCodeLogID = oReader.GetInt32("ORBarCodeLogID");
            oORBarCode.OrderRecapLogID = oReader.GetInt32("OrderRecapLogID");
            return oORBarCode;
        }

        private ORBarCode CreateObject(NullHandler oReader)
        {
            ORBarCode oORBarCode = new ORBarCode();
            oORBarCode = MapObject(oReader);
            return oORBarCode;
        }

        private List<ORBarCode> CreateObjects(IDataReader oReader)
        {
            List<ORBarCode> oORBarCode = new List<ORBarCode>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ORBarCode oItem = CreateObject(oHandler);
                oORBarCode.Add(oItem);
            }
            return oORBarCode;
        }

        #endregion

        #region Interface implementation
        public ORBarCodeService() { }

        public ORBarCode Save(ORBarCode oORBarCode, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ORBarCode> _oORBarCodes = new List<ORBarCode>();
            oORBarCode.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ORBarCodeDA.InsertUpdate(tc, oORBarCode, EnumDBOperation.Update, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oORBarCode = new ORBarCode();
                    oORBarCode = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oORBarCode.ErrorMessage = e.Message;
                #endregion
            }
            return oORBarCode;
        }

        public ORBarCode Get(int nORBarCodeID, Int64 nUserId)
        {
            ORBarCode oAccountHead = new ORBarCode();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ORBarCodeDA.Get(tc, nORBarCodeID);
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
                throw new ServiceException("Failed to Get ORBarCode", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ORBarCode> Gets(int nOrderRecapID, Int64 nUserID)
        {
            List<ORBarCode> oORBarCode = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ORBarCodeDA.Gets(nOrderRecapID, tc);
                oORBarCode = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ORBarCode", e);
                #endregion
            }

            return oORBarCode;
        }

        public List<ORBarCode> GetsByLog(int id, Int64 nUserId)
        {
            List<ORBarCode> oORBarCodes = new List<ORBarCode>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ORBarCodeDA.GetsByLog(tc, id);
                oORBarCodes = CreateObjects(reader);
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

            return oORBarCodes;
        }

        public List<ORBarCode> Gets(string sSQL, Int64 nUserID)
        {
            List<ORBarCode> oORBarCode = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ORBarCodeDA.Gets(tc, sSQL);
                oORBarCode = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ORBarCode", e);
                #endregion
            }
            return oORBarCode;
        }
        #endregion
    }   
}
