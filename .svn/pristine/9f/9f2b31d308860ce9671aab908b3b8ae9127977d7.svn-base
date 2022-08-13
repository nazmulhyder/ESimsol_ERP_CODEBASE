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


    public class ConsumptionUnitService : MarshalByRefObject, IConsumptionUnitService
    {
        #region Private functions and declaration
        private ConsumptionUnit MapObject(NullHandler oReader)
        {
            ConsumptionUnit oConsumptionUnit = new ConsumptionUnit();
            oConsumptionUnit.ConsumptionUnitID = oReader.GetInt32("ConsumptionUnitID");
            oConsumptionUnit.Name = oReader.GetString("Name");
            oConsumptionUnit.Note = oReader.GetString("Note");
            oConsumptionUnit.ParentConsumptionUnitID = oReader.GetInt32("ParentConsumptionUnitID");
            oConsumptionUnit.CUSequence = oReader.GetInt32("CUSequence");
            return oConsumptionUnit;
        }

        private ConsumptionUnit CreateObject(NullHandler oReader)
        {
            ConsumptionUnit oConsumptionUnit = new ConsumptionUnit();
            oConsumptionUnit = MapObject(oReader);
            return oConsumptionUnit;
        }

        private List<ConsumptionUnit> CreateObjects(IDataReader oReader)
        {
            List<ConsumptionUnit> oConsumptionUnit = new List<ConsumptionUnit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ConsumptionUnit oItem = CreateObject(oHandler);
                oConsumptionUnit.Add(oItem);
            }
            return oConsumptionUnit;
        }

        #endregion

        #region Interface implementation
        public ConsumptionUnitService() { }

        public ConsumptionUnit Save(ConsumptionUnit oConsumptionUnit, Int64 nUserID)
        {

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = oConsumptionUnit.BusinessUnits;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oConsumptionUnit.ConsumptionUnitID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ConsumptionUnit, EnumRoleOperationType.Add);
                    reader = ConsumptionUnitDA.InsertUpdate(tc, oConsumptionUnit, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ConsumptionUnit, EnumRoleOperationType.Edit);
                    reader = ConsumptionUnitDA.InsertUpdate(tc, oConsumptionUnit, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oConsumptionUnit = new ConsumptionUnit();
                    oConsumptionUnit = CreateObject(oReader);
                }
                reader.Close();
                #region
                if (oBusinessUnits != null)
                {
                    BUWiseConsumptionUnit oBUWiseConsumptionUnit = new BUWiseConsumptionUnit();
                    oBUWiseConsumptionUnit.ConsumptionUnitID = oConsumptionUnit.ConsumptionUnitID;
                    BUWiseConsumptionUnitDA.Delete(tc, oBUWiseConsumptionUnit, EnumDBOperation.Delete, nUserID);
                    foreach (BusinessUnit oItem in oBusinessUnits)
                    {
                        IDataReader readerBUConsumptionUnit;
                        oBUWiseConsumptionUnit = new BUWiseConsumptionUnit();
                        oBUWiseConsumptionUnit.ConsumptionUnitID = oConsumptionUnit.ConsumptionUnitID;
                        oBUWiseConsumptionUnit.BUID = oItem.BusinessUnitID;
                        readerBUConsumptionUnit = BUWiseConsumptionUnitDA.InsertUpdate(tc, oBUWiseConsumptionUnit, EnumDBOperation.Insert, nUserID);
                        NullHandler oReaderTNC = new NullHandler(readerBUConsumptionUnit);
                        readerBUConsumptionUnit.Close();
                    }
                }
                #endregion
          
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oConsumptionUnit = new ConsumptionUnit();
                oConsumptionUnit.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oConsumptionUnit;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ConsumptionUnit oConsumptionUnit = new ConsumptionUnit();
                oConsumptionUnit.ConsumptionUnitID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ConsumptionUnit, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ConsumptionUnit", id);
                ConsumptionUnitDA.Delete(tc, oConsumptionUnit, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ConsumptionUnit. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }



        public string ChangeGroup(string sSql, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = ConsumptionUnitDA.ChangeGroup(tc, sSql);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ConsumptionUnit. Because of " + e.Message, e);
                #endregion
            }
            return "Change Group successfully";
        }



        public string RefreshSequence(ConsumptionUnit oConsumptionUnit, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oConsumptionUnit.ConsumptionUnits.Count > 0)
                {
                    foreach (ConsumptionUnit oItem in oConsumptionUnit.ConsumptionUnits)
                    {
                        if (oItem.ConsumptionUnitID > 0 && oItem.CUSequence > 0)
                        {
                            ConsumptionUnitDA.UpdateSequence(tc, oItem);
                        }
                    }
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                
                #endregion
            }
            return "Change Sequence successfully";
        }



        public ConsumptionUnit Get(int id, Int64 nUserId)
        {
            ConsumptionUnit oAccountHead = new ConsumptionUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ConsumptionUnitDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ConsumptionUnit", e);
                #endregion
            }

            return oAccountHead;
        }


        public List<ConsumptionUnit> Gets(Int64 nUserID)
        {
            List<ConsumptionUnit> oConsumptionUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ConsumptionUnitDA.Gets(tc);
                oConsumptionUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ConsumptionUnit", e);
                #endregion
            }

            return oConsumptionUnit;
        }

        public List<ConsumptionUnit> Gets(string sSQL, Int64 nUserID)
        {
            List<ConsumptionUnit> oConsumptionUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ConsumptionUnitDA.Gets(tc, sSQL);
                oConsumptionUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ConsumptionUnit", e);
                #endregion
            }

            return oConsumptionUnit;
        }

        #endregion
    }   

    
}
