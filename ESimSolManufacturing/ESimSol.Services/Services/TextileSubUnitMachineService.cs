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
    public class TextileSubUnitMachineService : MarshalByRefObject, ITextileSubUnitMachineService
    {
        #region Private functions and declaration
        private TextileSubUnitMachine MapObject(NullHandler oReader)
        {
            TextileSubUnitMachine oTextileSubUnitMachine = new TextileSubUnitMachine();
            oTextileSubUnitMachine.TSUMachineID = oReader.GetInt32("TSUMachineID");
            oTextileSubUnitMachine.TSUID = oReader.GetInt32("TSUID");
            oTextileSubUnitMachine.FMID = oReader.GetInt32("FMID");
            oTextileSubUnitMachine.ActiveDate = oReader.GetDateTime("ActiveDate");
            oTextileSubUnitMachine.Note = oReader.GetString("Note");
            oTextileSubUnitMachine.InactiveBy = oReader.GetInt32("InactiveBy");
            oTextileSubUnitMachine.InactiveDate = oReader.GetDateTime("InactiveDate");
            oTextileSubUnitMachine.InactiveByName = oReader.GetString("InactiveByName");
            return oTextileSubUnitMachine;
        }

        private TextileSubUnitMachine CreateObject(NullHandler oReader)
        {
            TextileSubUnitMachine oTextileSubUnitMachine = new TextileSubUnitMachine();
            oTextileSubUnitMachine = MapObject(oReader);
            return oTextileSubUnitMachine;
        }

        private List<TextileSubUnitMachine> CreateObjects(IDataReader oReader)
        {
            List<TextileSubUnitMachine> oTextileSubUnitMachine = new List<TextileSubUnitMachine>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TextileSubUnitMachine oItem = CreateObject(oHandler);
                oTextileSubUnitMachine.Add(oItem);
            }
            return oTextileSubUnitMachine;
        }

        #endregion

        #region Interface implementation
        public TextileSubUnitMachineService() { }

        public TextileSubUnitMachine Save(TextileSubUnitMachine oTextileSubUnitMachine, int nDBOperation, Int64 nUserID)
        {
            TextileSubUnit oTextileSubUnit = new TextileSubUnit();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    if (oTextileSubUnitMachine.TSUMachineID <= 0)
                    {
                        reader = TextileSubUnitDA.InsertUpdate(tc, oTextileSubUnitMachine.oTextileSubUnit, nDBOperation, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oTextileSubUnit = new TextileSubUnit();
                            oTextileSubUnit = TextileSubUnitService.CreateObject(oReader);
                        }
                        reader.Close();
                    }
                    oTextileSubUnitMachine.TSUID = (oTextileSubUnitMachine.TSUID > 0) ? oTextileSubUnitMachine.TSUID : oTextileSubUnit.TSUID;
                    reader = TextileSubUnitMachineDA.InsertUpdate(tc, oTextileSubUnitMachine, nDBOperation, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTextileSubUnitMachine = new TextileSubUnitMachine();
                        oTextileSubUnitMachine = CreateObject(oReader);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oTextileSubUnitMachine = new TextileSubUnitMachine();
                oTextileSubUnitMachine.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            oTextileSubUnitMachine.oTextileSubUnit = oTextileSubUnit;
            return oTextileSubUnitMachine;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TextileSubUnitMachine oTextileSubUnitMachine = new TextileSubUnitMachine();
                oTextileSubUnitMachine.TSUMachineID = id;
                TextileSubUnitMachineDA.Delete(tc, oTextileSubUnitMachine, (int)EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete TextileSubUnitMachine. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }


        public List<TextileSubUnitMachine> AssignMachineToTextileUnit(List<TextileSubUnitMachine> oTextileSubUnitMachines, Int64 nUserID)
        {

            List<TextileSubUnitMachine> oTSUMs = new List<TextileSubUnitMachine>();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                NullHandler oReader;
                foreach (TextileSubUnitMachine oItem in oTextileSubUnitMachines)
                {
                    reader = TextileSubUnitMachineDA.InsertUpdate(tc, oItem, (int)EnumDBOperation.Insert, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        var oTSUM= new TextileSubUnitMachine();
                        oTSUM = CreateObject(oReader);
                        if (oTSUM.TSUMachineID > 0)
                            oTSUMs.Add(oTSUM);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oTSUMs = new List<TextileSubUnitMachine>();
                oTSUMs.Add(new TextileSubUnitMachine { ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message });

                #endregion
            }
            return oTSUMs;
        }

        

        public TextileSubUnitMachine Get(int id, Int64 nUserId)
        {
            TextileSubUnitMachine oTextileSubUnitMachine = new TextileSubUnitMachine();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TextileSubUnitMachineDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTextileSubUnitMachine = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get TextileSubUnitMachine", e);
                #endregion
            }

            return oTextileSubUnitMachine;
        }

        public List<TextileSubUnitMachine> Gets(Int64 nUserId)
        {
            List<TextileSubUnitMachine> oTextileSubUnitMachine = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TextileSubUnitMachineDA.Gets(tc);
                oTextileSubUnitMachine = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TextileSubUnitMachine", e);
                #endregion
            }

            return oTextileSubUnitMachine;
        }
        public List<TextileSubUnitMachine> Gets(string sSQL, Int64 nUserId)
        {
            List<TextileSubUnitMachine> oTextileSubUnitMachine = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TextileSubUnitMachineDA.Gets(tc, sSQL);
                oTextileSubUnitMachine = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TextileSubUnitMachine", e);
                #endregion
            }

            return oTextileSubUnitMachine;
        }

        #endregion
    }
}

