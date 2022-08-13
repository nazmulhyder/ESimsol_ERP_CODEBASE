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
    public class TechnicalSheetColorService : MarshalByRefObject, ITechnicalSheetColorService
    {
        #region Private functions and declaration
        private TechnicalSheetColor MapObject(NullHandler oReader)
        {
            TechnicalSheetColor oTechnicalSheetColor = new TechnicalSheetColor();
            oTechnicalSheetColor.TechnicalSheetColorID = oReader.GetInt32("TechnicalSheetColorID");
            oTechnicalSheetColor.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oTechnicalSheetColor.ColorCategoryID = oReader.GetInt32("ColorCategoryID");
            oTechnicalSheetColor.IsSelected = oReader.GetBoolean("IsSelected");
            oTechnicalSheetColor.Note = oReader.GetString("Note");
            oTechnicalSheetColor.ColorName = oReader.GetString("ColorName");
            oTechnicalSheetColor.PantonNo = oReader.GetString("PantonNo");
            oTechnicalSheetColor.Sequence = oReader.GetInt32("Sequence");
            
            return oTechnicalSheetColor;
        }

        private TechnicalSheetColor CreateObject(NullHandler oReader)
        {
            TechnicalSheetColor oTechnicalSheetColor = new TechnicalSheetColor();
            oTechnicalSheetColor = MapObject(oReader);
            return oTechnicalSheetColor;
        }

        private List<TechnicalSheetColor> CreateObjects(IDataReader oReader)
        {
            List<TechnicalSheetColor> oTechnicalSheetColor = new List<TechnicalSheetColor>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TechnicalSheetColor oItem = CreateObject(oHandler);
                oTechnicalSheetColor.Add(oItem);
            }
            return oTechnicalSheetColor;
        }

        #endregion

        #region Interface implementation
        public TechnicalSheetColorService() { }

        public TechnicalSheetColor Save(TechnicalSheetColor oTechnicalSheetColor, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTechnicalSheetColor.TechnicalSheetColorID <= 0)
                {
                    reader = TechnicalSheetColorDA.InsertUpdate(tc, oTechnicalSheetColor, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = TechnicalSheetColorDA.InsertUpdate(tc, oTechnicalSheetColor, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTechnicalSheetColor = new TechnicalSheetColor();
                    oTechnicalSheetColor = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save TechnicalSheetColor. Because of " + e.Message, e);
                #endregion
            }
            return oTechnicalSheetColor;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TechnicalSheetColor oTechnicalSheetColor = new TechnicalSheetColor();
                oTechnicalSheetColor.TechnicalSheetColorID = id;
                TechnicalSheetColorDA.Delete(tc, oTechnicalSheetColor, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete TechnicalSheetColor. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public TechnicalSheetColor Get(int id, Int64 nUserId)
        {
            TechnicalSheetColor oAccountHead = new TechnicalSheetColor();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TechnicalSheetColorDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get TechnicalSheetColor", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<TechnicalSheetColor> Gets(Int64 nUserID)
        {
            List<TechnicalSheetColor> oTechnicalSheetColor = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TechnicalSheetColorDA.Gets(tc);
                oTechnicalSheetColor = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheetColor", e);
                #endregion
            }

            return oTechnicalSheetColor;
        }

        public List<TechnicalSheetColor> Gets(int id, Int64 nUserID)
        {
            List<TechnicalSheetColor> oTechnicalSheetColor = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TechnicalSheetColorDA.Gets(tc, id);
                oTechnicalSheetColor = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheetColor", e);
                #endregion
            }

            return oTechnicalSheetColor;
        }
        #endregion
    }
}
