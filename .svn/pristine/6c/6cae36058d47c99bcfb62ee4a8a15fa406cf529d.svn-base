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
    public class DUColorComboService : MarshalByRefObject, IDUColorComboService
    {
        #region Private functions and declaration
        private DUColorCombo MapObject(NullHandler oReader)
        {
            DUColorCombo oDUColorCombo = new DUColorCombo();
            oDUColorCombo.DUColorComboID = oReader.GetInt32("DUColorComboID");
            oDUColorCombo.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUColorCombo.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDUColorCombo.ComboID = (EnumNumericOrder)oReader.GetInt32("ComboID");
            oDUColorCombo.SLNo = oReader.GetInt32("SLNo");
            oDUColorCombo.ColorName = oReader.GetString("ColorName");
            return oDUColorCombo;
        }

        private DUColorCombo CreateObject(NullHandler oReader)
        {
            DUColorCombo oDUColorCombo = new DUColorCombo();
            oDUColorCombo = MapObject(oReader);
            return oDUColorCombo;
        }

        private List<DUColorCombo> CreateObjects(IDataReader oReader)
        {
            List<DUColorCombo> oDUColorCombo = new List<DUColorCombo>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUColorCombo oItem = CreateObject(oHandler);
                oDUColorCombo.Add(oItem);
            }
            return oDUColorCombo;
        }

        #endregion

        #region Interface implementation
        public DUColorComboService() { }


        public List<DUColorCombo> Save(List<DUColorCombo> oDUColorCombos, Int64 nUserID)
        {
            DUColorCombo oDUColorCombo = new DUColorCombo();
            List<DUColorCombo> oDUColorCombos_Return = new List<DUColorCombo>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                //DUColorComboDA.Delete(tc, oItem, EnumDBOperation.Insert, nUserID);
                IDataReader reader;
                foreach (DUColorCombo oItem in oDUColorCombos)
                {

                    if (oItem.DUColorComboID <= 0)
                    {
                        reader = DUColorComboDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = DUColorComboDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDUColorCombo = new DUColorCombo();
                        oDUColorCombo = CreateObject(oReader);
                        oDUColorCombos_Return.Add(oDUColorCombo);
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

                oDUColorCombo = new DUColorCombo();
                oDUColorCombo.ErrorMessage = e.Message.Split('~')[0];
                oDUColorCombos_Return.Add(oDUColorCombo);

                #endregion
            }
            return oDUColorCombos_Return;
        }
        public string Delete(DUColorCombo oDUColorCombo, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
               
                DUColorComboDA.Delete(tc, oDUColorCombo, EnumDBOperation.Delete, nUserId);
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
            return Global.DeleteMessage;
        }

        public DUColorCombo Get(int id, Int64 nUserId)
        {
            DUColorCombo oDUColorCombo = new DUColorCombo();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DUColorComboDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUColorCombo = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DUColorCombo", e);
                #endregion
            }
            return oDUColorCombo;
        }
        public List<DUColorCombo> Gets(int nDODID, long nUserID)
        {
            List<DUColorCombo> oDUColorCombos = new List<DUColorCombo>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUColorComboDA.Gets(tc, nDODID);
                oDUColorCombos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUColorCombo", e);
                #endregion
            }
            return oDUColorCombos;
        }
        public List<DUColorCombo> Gets(int nDODID, int nComboID, long nUserID)
        {
            List<DUColorCombo> oDUColorCombos = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUColorComboDA.Gets(tc,  nDODID,  nComboID,  nUserID);
                oDUColorCombos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUColorCombo", e);
                #endregion
            }
            return oDUColorCombos;
        }
        #endregion
    }
}