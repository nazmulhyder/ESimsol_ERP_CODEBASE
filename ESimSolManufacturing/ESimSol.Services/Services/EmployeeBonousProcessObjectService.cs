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
    public class EmployeeBonousProcessObjectService : MarshalByRefObject, IEmployeeBonousProcessObjectService
    {
        #region Private functions and declaration
        private EmployeeBonusProcessObject MapObject(NullHandler oReader)
        {
            EmployeeBonusProcessObject oEmployeeBonousProcessObject = new EmployeeBonusProcessObject();
            oEmployeeBonousProcessObject.EBPObjectID = oReader.GetInt32("EBPObjectID");
            oEmployeeBonousProcessObject.EBPID = oReader.GetInt32("EBPID");
            oEmployeeBonousProcessObject.PPMObject = oReader.GetInt32("PPMObject");
            oEmployeeBonousProcessObject.ObjectID = oReader.GetInt32("ObjectID");
            return oEmployeeBonousProcessObject;

        }

        private EmployeeBonusProcessObject CreateObject(NullHandler oReader)
        {
            EmployeeBonusProcessObject oEmployeeBonousProcessObject = MapObject(oReader);
            return oEmployeeBonousProcessObject;
        }

        private List<EmployeeBonusProcessObject> CreateObjects(IDataReader oReader)
        {
            List<EmployeeBonusProcessObject> oEmployeeBonousProcessObject = new List<EmployeeBonusProcessObject>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeBonusProcessObject oItem = CreateObject(oHandler);
                oEmployeeBonousProcessObject.Add(oItem);
            }
            return oEmployeeBonousProcessObject;
        }


        #endregion

        #region Interface implementation
        public EmployeeBonousProcessObjectService() { }
        public EmployeeBonusProcessObject IUD(EmployeeBonusProcessObject oEmployeeBonousProcessObject, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    reader = EmployeeBonusProcessObjectDA.IUD(tc, oEmployeeBonousProcessObject, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oEmployeeBonousProcessObject = new EmployeeBonusProcessObject();
                        oEmployeeBonousProcessObject = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = EmployeeBonusProcessObjectDA.IUD(tc, oEmployeeBonousProcessObject, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oEmployeeBonousProcessObject.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oEmployeeBonousProcessObject = new EmployeeBonusProcessObject();
                oEmployeeBonousProcessObject.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oEmployeeBonousProcessObject;
        }


        public EmployeeBonusProcessObject Get(string sSQL, Int64 nUserId)
        {
            EmployeeBonusProcessObject oEmployeeBonousProcessObject = new EmployeeBonusProcessObject();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeBonusProcessObjectDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeBonousProcessObject = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                //oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeBonousProcessObject;
        }

        public List<EmployeeBonusProcessObject> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeBonusProcessObject> oEmployeeBonousProcessObject = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeBonusProcessObjectDA.Gets(sSQL, tc);
                oEmployeeBonousProcessObject = CreateObjects(reader);
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
            return oEmployeeBonousProcessObject;
        }
        #endregion
    }
}



