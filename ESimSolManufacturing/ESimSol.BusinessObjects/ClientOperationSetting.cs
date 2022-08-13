using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ClientOperationSetting
    
    public class ClientOperationSetting : BusinessObject
    {
        public ClientOperationSetting()
        {
            ClientOperationSettingID = 0;
            OperationType = EnumOperationType.None;
            DataType = 0;
            Value = "";
            OperationTypeInInt = 0;
            DataTypeInInt = 0;
            ErrorMessage = "";
        }
        #region Properties         
        public int ClientOperationSettingID { get; set; }
        public EnumOperationType OperationType { get; set; }
        public EnumDataType DataType { get; set; }
        public string Value { get; set; }
        public int OperationTypeInInt { get; set; }
        public int DataTypeInInt { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public EnumClientOperationValueFormat ClientOperationValueFormat { get; set; }
        public string OperationTypeInString
        {
            get
            {
                return this.OperationType.ToString();
            }
        }
        public string DataTypeInString
        {
            get
            {
                return this.DataType.ToString();
            }
        }
        public string ValueInString
        {
            get
            {
                if (this.DataType == EnumDataType.Enum)
                {
                    return (EnumObject.jGet((EnumClientOperationValueFormat)Convert.ToInt16((this.Value))));
                }
                else if (this.DataType == EnumDataType.Boolean)
                {
                    return (Convert.ToBoolean(Convert.ToUInt32(this.Value))) == true ? "Yes" : "No";
                }
                else if (this.DataType == EnumDataType.Number)
                {
                    return Global.MillionFormatActualDigit(Convert.ToDouble(this.Value));
                }
                else
                {
                    return this.Value;
                }
                //return "Yes";
            }
        }


        public bool Selected { get; set; }
        public List<ClientOperationSetting> ClientOperationSettingList { get; set; }

        #endregion
         
        #region Functions

        public static List<ClientOperationSetting> Gets(long nUserID)
        {
            return ClientOperationSetting.Service.Gets( nUserID);
        }
        public static List<ClientOperationSetting> Gets(string sSQL, long nUserID)
        {
            return ClientOperationSetting.Service.Gets(sSQL, nUserID);
        }
        public ClientOperationSetting Get(int id, long nUserID)
        {
            return ClientOperationSetting.Service.Get(id, nUserID);
        }

        public ClientOperationSetting GetByOperationType(int eOperationTypeid, Int64 nUserID)
        {
            return ClientOperationSetting.Service.GetByOperationType(eOperationTypeid, nUserID);
        }

        public ClientOperationSetting Save(long nUserID)
        {
            return ClientOperationSetting.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return ClientOperationSetting.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IClientOperationSettingService Service
        {
            get { return (IClientOperationSettingService)Services.Factory.CreateService(typeof(IClientOperationSettingService)); }
        }


        #endregion
    }
    #endregion

    #region IClientOperationSetting interface
     
    public interface IClientOperationSettingService
    {
        ClientOperationSetting Get(int id, Int64 nUserID);
        ClientOperationSetting GetByOperationType(int eOperationTypeid, Int64 nUserID);         
        List<ClientOperationSetting> Gets(Int64 nUserID);         
        List<ClientOperationSetting> Gets(string sSQL, Int64 nUserID);         
        string Delete(int id, Int64 nUserID);         
        ClientOperationSetting Save(ClientOperationSetting oClientOperationSetting, Int64 nUserID);
    }
    #endregion
    

}
