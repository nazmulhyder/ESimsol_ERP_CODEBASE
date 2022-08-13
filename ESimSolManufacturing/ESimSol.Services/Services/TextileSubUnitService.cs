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
    public class TextileSubUnitService : MarshalByRefObject, ITextileSubUnitService
    {
        #region Private functions and declaration
        private static TextileSubUnit MapObject(NullHandler oReader)
        {
            TextileSubUnit oTextileSubUnit = new TextileSubUnit();
            oTextileSubUnit.TSUID = oReader.GetInt32("TSUID");
            oTextileSubUnit.TextileUnit = (EnumTextileUnit)oReader.GetInt16("TextileUnit");
            oTextileSubUnit.Name = oReader.GetString("Name");
            oTextileSubUnit.Note = oReader.GetString("Note");
            oTextileSubUnit.InactiveBy = oReader.GetInt32("InactiveBy");
            oTextileSubUnit.InactiveDate = oReader.GetDateTime("InactiveDate");
            oTextileSubUnit.InactiveByName = oReader.GetString("InactiveByName");
            return oTextileSubUnit;
        }

        public static  TextileSubUnit CreateObject(NullHandler oReader)
        {
            TextileSubUnit oTextileSubUnit = new TextileSubUnit();
            oTextileSubUnit = MapObject(oReader);
            return oTextileSubUnit;
        }

        private List<TextileSubUnit> CreateObjects(IDataReader oReader)
        {
            List<TextileSubUnit> oTextileSubUnit = new List<TextileSubUnit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TextileSubUnit oItem = CreateObject(oHandler);
                oTextileSubUnit.Add(oItem);
            }
            return oTextileSubUnit;
        }

        #endregion

        #region Interface implementation
        public TextileSubUnitService() { }

        public TextileSubUnit Save(TextileSubUnit oTextileSubUnit,int nDBOperation,  Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTextileSubUnit.TSUID <= 0)
                {
                    reader = TextileSubUnitDA.InsertUpdate(tc, oTextileSubUnit, (int)EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = TextileSubUnitDA.InsertUpdate(tc, oTextileSubUnit, (int)EnumDBOperation.Approval, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTextileSubUnit = new TextileSubUnit();
                    oTextileSubUnit = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oTextileSubUnit.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Product. Because of " + e.Message, e);
                #endregion
            }
            return oTextileSubUnit;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TextileSubUnit oTextileSubUnit = new TextileSubUnit();
                oTextileSubUnit.TSUID = id;
                TextileSubUnitDA.Delete(tc, oTextileSubUnit, (int)EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete TextileSubUnit. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public TextileSubUnit Get(int id, Int64 nUserId)
        {
            TextileSubUnit oTextileSubUnit = new TextileSubUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TextileSubUnitDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTextileSubUnit = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get TextileSubUnit", e);
                #endregion
            }

            return oTextileSubUnit;
        }

        public List<TextileSubUnit> Gets(Int64 nUserId)
        {
            List<TextileSubUnit> oTextileSubUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TextileSubUnitDA.Gets(tc);
                oTextileSubUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TextileSubUnit", e);
                #endregion
            }

            return oTextileSubUnit;
        }
        public List<TextileSubUnit> Gets(string sSQL, Int64 nUserId)
        {
            List<TextileSubUnit> oTextileSubUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TextileSubUnitDA.Gets(tc, sSQL);
                oTextileSubUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TextileSubUnit", e);
                #endregion
            }

            return oTextileSubUnit;
        }

        #endregion
    }
}
