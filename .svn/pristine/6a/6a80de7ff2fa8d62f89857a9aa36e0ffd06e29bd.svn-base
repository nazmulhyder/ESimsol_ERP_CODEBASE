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
    public class TechnicalSheetSizeService : MarshalByRefObject, ITechnicalSheetSizeService
    {
        #region Private functions and declaration
        private TechnicalSheetSize MapObject(NullHandler oReader)
        {
            TechnicalSheetSize oTechnicalSheetSize = new TechnicalSheetSize();            
            oTechnicalSheetSize.TechnicalSheetSizeID = oReader.GetInt32("TechnicalSheetSizeID");
            oTechnicalSheetSize.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oTechnicalSheetSize.SizeCategoryID = oReader.GetInt32("SizeCategoryID");
            oTechnicalSheetSize.Note = oReader.GetString("Note");
            oTechnicalSheetSize.SizeCategoryName = oReader.GetString("SizeCategoryName");
            oTechnicalSheetSize.Sequence = oReader.GetInt32("Sequence");
            oTechnicalSheetSize.QtyInPercent = oReader.GetDouble("QtyInPercent");
            return oTechnicalSheetSize;
        }

        private TechnicalSheetSize CreateObject(NullHandler oReader)
        {
            TechnicalSheetSize oTechnicalSheetSize = new TechnicalSheetSize();
            oTechnicalSheetSize = MapObject(oReader);
            return oTechnicalSheetSize;
        }

        private List<TechnicalSheetSize> CreateObjects(IDataReader oReader)
        {
            List<TechnicalSheetSize> oTechnicalSheetSize = new List<TechnicalSheetSize>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TechnicalSheetSize oItem = CreateObject(oHandler);
                oTechnicalSheetSize.Add(oItem);
            }
            return oTechnicalSheetSize;
        }

        #endregion

        #region Interface implementation
        public TechnicalSheetSizeService() { }

        public TechnicalSheetSize Save(TechnicalSheetSize oTechnicalSheetSize, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTechnicalSheetSize.TechnicalSheetSizeID <= 0)
                {
                    reader = TechnicalSheetSizeDA.InsertUpdate(tc, oTechnicalSheetSize, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = TechnicalSheetSizeDA.InsertUpdate(tc, oTechnicalSheetSize, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTechnicalSheetSize = new TechnicalSheetSize();
                    oTechnicalSheetSize = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save TechnicalSheetSize. Because of " + e.Message, e);
                #endregion
            }
            return oTechnicalSheetSize;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TechnicalSheetSize oTechnicalSheetSize = new TechnicalSheetSize();
                oTechnicalSheetSize.TechnicalSheetSizeID = id;
                TechnicalSheetSizeDA.Delete(tc, oTechnicalSheetSize, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete TechnicalSheetSize. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public TechnicalSheetSize Get(int id, Int64 nUserId)
        {
            TechnicalSheetSize oAccountHead = new TechnicalSheetSize();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TechnicalSheetSizeDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get TechnicalSheetSize", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<TechnicalSheetSize> Gets(Int64 nUserID)
        {
            List<TechnicalSheetSize> oTechnicalSheetSize = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TechnicalSheetSizeDA.Gets(tc);
                oTechnicalSheetSize = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheetSize", e);
                #endregion
            }

            return oTechnicalSheetSize;
        }

        public List<TechnicalSheetSize> Gets(int nTSID, Int64 nUserID)
        {
            List<TechnicalSheetSize> oTechnicalSheetSize = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TechnicalSheetSizeDA.Gets(tc, nTSID);
                oTechnicalSheetSize = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheetSize", e);
                #endregion
            }

            return oTechnicalSheetSize;
        }
        #endregion
    }
}
