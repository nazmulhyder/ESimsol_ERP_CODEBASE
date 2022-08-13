using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;


using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class MachineService : MarshalByRefObject, IMachineService
    {
        #region Private functions and declaration
        private Machine MapObject(NullHandler oReader)
        {
            Machine oMachine = new Machine();
            oMachine.MachineID = oReader.GetInt32("MachineID");
            oMachine.MachineTypeID = oReader.GetInt32("MachineTypeID");
            oMachine.SequenceNo = oReader.GetInt32("SequenceNo");
            oMachine.BUID = oReader.GetInt32("BUID");
            oMachine.LocationID = oReader.GetInt32("LocationID");
            oMachine.Activity = oReader.GetBoolean("Activity");
            oMachine.LocationType = (EnumLocationType)oReader.GetInt32("LocationType");
            oMachine.Capacity = oReader.GetDouble("Capacity");
            oMachine.Name = oReader.GetString("Name");
            oMachine.Capacity2 = oReader.GetString("Capacity2");
            oMachine.Code = oReader.GetString("Code");
            oMachine.Note = oReader.GetString("Note");
            oMachine.LocCode = oReader.GetString("LocCode");
            oMachine.LocationName = oReader.GetString("LocationName");
            oMachine.BUnit = oReader.GetString("BUnit");
            oMachine.MachineTypeName = oReader.GetString("MachineTypeName");
            return oMachine;
        }

        private Machine CreateObject(NullHandler oReader)
        {
            Machine oMachine = new Machine();
            oMachine = MapObject(oReader);
            return oMachine;
        }

        private List<Machine> CreateObjects(IDataReader oReader)
        {
            List<Machine> oMachines = new List<Machine>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Machine oItem = CreateObject(oHandler);
                oMachines.Add(oItem);
            }
            return oMachines;
        }

        #endregion

        #region Interface implementation
        public MachineService() { }


        public Machine Save(Machine oMachine, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region Machine
                IDataReader reader;
                if (oMachine.MachineID <= 0)
                {
                    reader = MachineDA.InsertUpdate(tc, oMachine, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = MachineDA.InsertUpdate(tc, oMachine, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMachine = new Machine();
                    oMachine = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oMachine = new Machine();
                oMachine.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oMachine;
        }
        public List<Machine> Update(List<Machine> oMachines, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Delivery Order Detail Part
                foreach (Machine oItem in oMachines)
                {
                    if (oItem.MachineID > 0)
                    {
                        //IDataReader readerdetail; readerdetail =
                         MachineDA.UpdateSequence(tc, oItem, nUserID);
                        //NullHandler oReaderDetail = new NullHandler(readerdetail);
                        //readerdetail.Close();
                    }
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oMachines = new List<Machine>();
                    Machine oMachine = new Machine();
                    oMachine.ErrorMessage = e.Message.Split('!')[0]; oMachines.Add(oMachine);
                }
                #endregion
            }
            return oMachines;
        }

        public String Delete(Machine oMachine, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MachineDA.Delete(tc, oMachine, EnumDBOperation.Delete, nUserID);
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
        public Machine Get(int id, Int64 nUserId)
        {
            Machine oMachine = new Machine();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MachineDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMachine = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oMachine;
        }

        public List<Machine> Gets(string sSQL, Int64 nUserID)
        {
            List<Machine> oMachine = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MachineDA.Gets(sSQL, tc);
                oMachine = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Machine", e);
                #endregion
            }
            return oMachine;
        }
        public Machine GetByType(int nOrderType, Int64 nUserId)
        {
            Machine oMachine = new Machine();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MachineDA.GetByType(tc, nOrderType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMachine = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oMachine;
        }
     

        public List<Machine> Gets(Int64 nUserId)
        {
            List<Machine> oMachines = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MachineDA.Gets(tc);
                oMachines = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oMachines;
        }
        public List<Machine> GetsActive( Int64 nUserId)
        {
            List<Machine> oMachines = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MachineDA.GetsActive(tc);
                oMachines = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oMachines;
        }
        public List<Machine> GetsBy(int nBUID,Int64 nUserId)
        {
            List<Machine> oMachines = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MachineDA.GetsBy(tc, nBUID);
                oMachines = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oMachines;
        }
        public List<Machine> GetsByModule(int nBUID, string sModuleIDs, Int64 nUserId)
        {
            List<Machine> oMachines = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MachineDA.GetsByModule(tc,  nBUID, sModuleIDs);
                oMachines = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oMachines;
        }

        public Machine Activate(Machine oMachine, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MachineDA.Activate(tc, oMachine);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMachine = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oMachine = new Machine();
                oMachine.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oMachine;
        }
    

        #endregion
    }
}