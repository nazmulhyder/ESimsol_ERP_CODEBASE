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
    public class DUDyeingTypeService : MarshalByRefObject, IDUDyeingTypeService
    {
        #region Private functions and declaration
        private static DUDyeingType MapObject(NullHandler oReader)
        {
            DUDyeingType oDUDyeingType = new DUDyeingType();
            oDUDyeingType.DUDyeingTypeID = oReader.GetInt32("DUDyeingTypeID");
            oDUDyeingType.DyeingType = (EumDyeingType)oReader.GetInt16("DyeingType");
            oDUDyeingType.DyeingTypeInt = oReader.GetInt16("DyeingType");
            oDUDyeingType.Name = oReader.GetString("Name");
            oDUDyeingType.Capacity = oReader.GetDouble("Capacity");
            oDUDyeingType.Activity = oReader.GetBoolean("Activity");
            return oDUDyeingType;
        }

        public static DUDyeingType CreateObject(NullHandler oReader)
        {
            DUDyeingType oDUDyeingType = MapObject(oReader);
            return oDUDyeingType;
        }

        private List<DUDyeingType> CreateObjects(IDataReader oReader)
        {
            List<DUDyeingType> oDUDyeingTypes = new List<DUDyeingType>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDyeingType oItem = CreateObject(oHandler);
                oDUDyeingTypes.Add(oItem);
            }
            return oDUDyeingTypes;
        }

        #endregion
        #region Interface implementation
        public DUDyeingTypeService() { }

        public DUDyeingType IUD(DUDyeingType oDUDyeingType, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval || nDBOperation == (int)EnumDBOperation.Revise)
                {
                    reader = DUDyeingTypeDA.IUD(tc, oDUDyeingType, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDUDyeingType = new DUDyeingType();
                        oDUDyeingType = CreateObject(oReader);
                    }
                    reader.Close();

                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = DUDyeingTypeDA.IUD(tc, oDUDyeingType, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oDUDyeingType.ErrorMessage = Global.DeleteMessage;
                }
                else
                {
                    throw new Exception("Invalid Operation In Service");
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oDUDyeingType = new DUDyeingType();
                oDUDyeingType.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oDUDyeingType;
        }

        public DUDyeingType Get(int nDUDyeingTypeID, Int64 nUserId)
        {
            DUDyeingType oDUDyeingType = new DUDyeingType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDyeingTypeDA.Get(tc, nDUDyeingTypeID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDyeingType = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oDUDyeingType = new DUDyeingType();
                oDUDyeingType.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oDUDyeingType;
        }

        public List<DUDyeingType> Gets(string sSQL, Int64 nUserID)
        {
            List<DUDyeingType> oDUDyeingTypes = new List<DUDyeingType>();
            DUDyeingType oDUDyeingType = new DUDyeingType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDyeingTypeDA.Gets(tc, sSQL);
                oDUDyeingTypes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oDUDyeingType.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oDUDyeingTypes.Add(oDUDyeingType);
                #endregion
            }

            return oDUDyeingTypes;
        }
        public List<DUDyeingType> GetsActivity(bool bActivity, Int64 nUserID)
        {
            List<DUDyeingType> oDUDyeingTypes = new List<DUDyeingType>();
            DUDyeingType oDUDyeingType = new DUDyeingType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDyeingTypeDA.GetsActivity(tc, bActivity);
                oDUDyeingTypes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oDUDyeingType.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oDUDyeingTypes.Add(oDUDyeingType);
                #endregion
            }

            return oDUDyeingTypes;
        }



        #endregion
    }
}
