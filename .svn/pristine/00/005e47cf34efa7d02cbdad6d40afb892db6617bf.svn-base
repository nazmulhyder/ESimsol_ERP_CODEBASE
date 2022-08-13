using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class OperationUnitService : MarshalByRefObject, IOperationUnitService
    {
        #region Private functions and declaration
        private OperationUnit MapObject(NullHandler oReader)
        {
            OperationUnit oOperationUnit = new OperationUnit();
            oOperationUnit.OperationUnitID = oReader.GetInt32("OperationUnitID");
            oOperationUnit.OperationUnitName = oReader.GetString("OperationUnitName");
            oOperationUnit.ShortName = oReader.GetString("ShortName");
            oOperationUnit.Description = oReader.GetString("Description");
            oOperationUnit.IsStore = oReader.GetBoolean("IsStore");
            oOperationUnit.ContainingProduct = oReader.GetInt32("ContainingProduct");
            return oOperationUnit;
        }

        private OperationUnit CreateObject(NullHandler oReader)
        {
            OperationUnit oOperationUnit = new OperationUnit();
            oOperationUnit = MapObject(oReader);
            return oOperationUnit;
        }

        private List<OperationUnit> CreateObjects(IDataReader oReader)
        {
            List<OperationUnit> oOperationUnit = new List<OperationUnit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                OperationUnit oItem = CreateObject(oHandler);
                oOperationUnit.Add(oItem);
            }
            return oOperationUnit;
        }

        #endregion

        #region Interface implementation
        public OperationUnitService() { }

        public OperationUnit Save(OperationUnit oOperationUnit, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oOperationUnit.OperationUnitID <= 0)
                {
                    reader = OperationUnitDA.InsertUpdate(tc, oOperationUnit, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = OperationUnitDA.InsertUpdate(tc, oOperationUnit, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOperationUnit = new OperationUnit();
                    oOperationUnit = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oOperationUnit = new OperationUnit();
                oOperationUnit.ErrorMessage = e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save OperationUnit. Because of " + e.Message, e);
                #endregion
            }
            return oOperationUnit;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                OperationUnit oOperationUnit = new OperationUnit();
                oOperationUnit.OperationUnitID = id;                
                OperationUnitDA.Delete(tc, oOperationUnit, EnumDBOperation.Delete, nUserId);
                tc.End();
                return "Delete sucessfully";
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete OperationUnit. Because of " + e.Message, e);
                #endregion
            }

        }

        public OperationUnit Get(int id, int nUserId)
        {
            OperationUnit oAccountHead = new OperationUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = OperationUnitDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get OperationUnit", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<OperationUnit> Gets(int nUserId)
        {
            List<OperationUnit> oOperationUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = OperationUnitDA.Gets(tc);
                oOperationUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OperationUnit", e);
                #endregion
            }

            return oOperationUnit;
        }

        public List<OperationUnit> Gets(string sSQL, int nUserId)
        {
            List<OperationUnit> oOperationUnits = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OperationUnitDA.Gets(tc, sSQL);
                oOperationUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OperationUnit", e);
                #endregion
            }

            return oOperationUnits;
        }
        #endregion
    }
}