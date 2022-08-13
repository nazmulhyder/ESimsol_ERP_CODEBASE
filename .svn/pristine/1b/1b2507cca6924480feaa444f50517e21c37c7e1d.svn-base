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

    public class FabricMachineService : MarshalByRefObject, IFabricMachineService
    {
        #region Private functions and declaration
        private FabricMachine MapObject(NullHandler oReader)
        {
            FabricMachine oFabricMachine = new FabricMachine();
            oFabricMachine.FMID = oReader.GetInt32("FMID");
            oFabricMachine.Name = oReader.GetString("Name");
            oFabricMachine.WeavingProcess = (EnumWeavingProcess)oReader.GetInt32("WeavingProcess");
            oFabricMachine.WeavingProcessInInt = oReader.GetInt32("WeavingProcess");
            oFabricMachine.CCID= oReader.GetInt32("CCID");
            oFabricMachine.Code = oReader.GetString("Code");
            oFabricMachine.Capacity= oReader.GetString("Capacity");
            oFabricMachine.RPM= oReader.GetInt32("RPM");
            oFabricMachine.StandardEfficiency = oReader.GetDouble("StandardEfficiency");
            oFabricMachine.IsActive= oReader.GetBoolean("IsActive");
            oFabricMachine.IsBeam = oReader.GetBoolean("IsBeam");
            oFabricMachine.InActiveDate= oReader.GetDateTime("InActiveDate");
            oFabricMachine.MachineStatus= (EnumMachineStatus)oReader.GetInt32("MachineStatus");
            oFabricMachine.TSUID = oReader.GetInt32("TSUID");
            oFabricMachine.TextileSubUnitName = oReader.GetString("TextileSubUnitName");
            oFabricMachine.ParentMachineTypeID = oReader.GetInt32("ParentMachineTypeID");
            oFabricMachine.ChildMachineTypeID = oReader.GetInt32("ChildMachineTypeID");
            oFabricMachine.ParentMachineTypeName = oReader.GetString("ParentMachineTypeName");
            oFabricMachine.ChildMachineTypeName = oReader.GetString("ChildMachineTypeName");
            oFabricMachine.FabricMachineGroupID = oReader.GetInt32("FabricMachineGroupID");
            oFabricMachine.FabricMachineGroupName = oReader.GetString("FabricMachineGroupName");
            oFabricMachine.IsDirect = oReader.GetBoolean("IsDirect");
            
            return oFabricMachine;
        }

        private FabricMachine CreateObject(NullHandler oReader)
        {
            FabricMachine oFabricMachine = new FabricMachine();
            oFabricMachine = MapObject(oReader);
            return oFabricMachine;
        }

        private List<FabricMachine> CreateObjects(IDataReader oReader)
        {
            List<FabricMachine> oFabricMachine = new List<FabricMachine>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricMachine oItem = CreateObject(oHandler);
                oFabricMachine.Add(oItem);
            }
            return oFabricMachine;
        }

        #endregion

        #region Interface implementation
        public FabricMachineService() { }

        public FabricMachine Save(FabricMachine oFabricMachine, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricMachine.FMID <= 0)
                {
                    reader = FabricMachineDA.InsertUpdate(tc, oFabricMachine, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricMachineDA.InsertUpdate(tc, oFabricMachine, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricMachine = new FabricMachine();
                    oFabricMachine = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricMachine = new FabricMachine();
                oFabricMachine.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricMachine;
        }

        public FabricMachine ActiveInActive(int id, bool bIsActive)
        {
            FabricMachine oAccountHead = new FabricMachine();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                FabricMachineDA.ActiveInActive(tc, id, bIsActive);
                IDataReader reader = FabricMachineDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get FabricMachine", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public FabricMachine LoomMachineRestore(int id)
        {
            FabricMachine oFabricMachine = new FabricMachine();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                //if (id <= 0)
                //{
                    reader = FabricMachineDA.LoomMachineRestore(tc, id);
                //}

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricMachine = new FabricMachine();
                    oFabricMachine = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricMachine", e);
                #endregion
            }

            return oFabricMachine;
        }

        public FabricMachine HoldBeamFinishForLoomProcess(int id)
        {
            FabricMachine oFabricMachine = new FabricMachine();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                //if (id <= 0)
                //{
                reader = FabricMachineDA.HoldBeamFinishForLoomProcess(tc, id);
                //}

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricMachine = new FabricMachine();
                    oFabricMachine = CreateObject(oReader);
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
                throw new ServiceException(e.Message.Split('~')[0]);
                
                #endregion
            }

            return oFabricMachine;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricMachine oFabricMachine = new FabricMachine();
                oFabricMachine.FMID = id;
                DBTableReferenceDA.HasReference(tc, "FabricMachine", id);
                FabricMachineDA.Delete(tc, oFabricMachine, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete FabricMachine. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public FabricMachine Get(int id, Int64 nUserId)
        {
            FabricMachine oAccountHead = new FabricMachine();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricMachineDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get FabricMachine", e);
                #endregion
            }

            return oAccountHead;
        }


        public List<FabricMachine> Gets(Int64 nUserID)
        {
            List<FabricMachine> oFabricMachine = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricMachineDA.Gets(tc);
                oFabricMachine = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricMachine", e);
                #endregion
            }

            return oFabricMachine;
        }
        
        public List<FabricMachine> Gets(int nWeavingProcess, int nMachineStatus, Int64 nUserID)
        {
            List<FabricMachine> oFabricMachine = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricMachineDA.Gets(nWeavingProcess, nMachineStatus,tc);
                oFabricMachine = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricMachine", e);
                #endregion
            }

            return oFabricMachine;
        }
        public List<FabricMachine> Gets(bool bIsBeam, int nWeavingProcess, int nMachineStatus, Int64 nUserID)
        {
            List<FabricMachine> oFabricMachine = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricMachineDA.Gets(bIsBeam, nWeavingProcess, nMachineStatus, tc);
                oFabricMachine = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricMachine", e);
                #endregion
            }

            return oFabricMachine;
        }
        public List<FabricMachine> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricMachine> oFabricMachine = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricMachineDA.Gets(tc, sSQL);
                oFabricMachine = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricMachine", e);
                #endregion
            }

            return oFabricMachine;
        }

        public FabricMachine MakeFree(FabricMachine oFabricMachine, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricMachineDA.InsertUpdate(tc, oFabricMachine, EnumDBOperation.Revise, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricMachine = new FabricMachine();
                    oFabricMachine = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricMachine = new FabricMachine();
                oFabricMachine.ErrorMessage = e.Message;
                #endregion
            }

            return oFabricMachine;
        }

        #endregion
    }   


    

}
