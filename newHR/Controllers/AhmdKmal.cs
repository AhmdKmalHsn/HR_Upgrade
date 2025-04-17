using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using BioMetrixCore;
using zkemkeeper;
using Controllers;

namespace newHR.Controllers
{
    public class log {
        public int fileNumber { get; set; }
        public DateTime datetime { get; set; }
    }
    public class AhmdKmal
    {
        DeviceManipulator manipulator = new DeviceManipulator();
        public ZkemClient objZkeeper;
        private bool isDeviceConnected = false;
       
        public bool IsDeviceConnected
        {
            get { return isDeviceConnected; }
            set
            {
                isDeviceConnected = value;
                if (isDeviceConnected)
                {
                    //ShowStatusBar("The device is connected !!", true);
                    //btnConnect.Text = "Disconnect";
                    //ToggleControls(true);
                }
                else
                {
                    //ShowStatusBar("The device is diconnected !!", true);
                    objZkeeper.Disconnect();
                    //btnConnect.Text = "Connect";
                    //ToggleControls(false);
                }
            }
        }
        private void RaiseDeviceEvent(object sender, string actionType)
        {
            switch (actionType)
            {
                case UniversalStatic.acx_Disconnect:
                    {
                        //ShowStatusBar("The device is switched off", true);
                        //DisplayEmpty();
                        Console.WriteLine("Connect");
                        //ToggleControls(false);
                        break;
                    }

                default:
                    break;
            }

        }
        public ICollection<MachineInfo> data(string ip)
        {
            
            objZkeeper = new ZkemClient(RaiseDeviceEvent);
           
                if (objZkeeper.Connect_Net(ip, 4370))
                    try
                    {
                        ICollection<MachineInfo> lstMachineInfo = manipulator.GetLogData(objZkeeper, int.Parse("1"));
                        return lstMachineInfo;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
            
            return null;
        }
        public ICollection<MachineInfo> dataBetween(string ip,DateTime F,DateTime T)
        {

            objZkeeper = new ZkemClient(RaiseDeviceEvent);

            if (objZkeeper.Connect_Net(ip, 4370))
                try
                {
                    ICollection<MachineInfo> lstMachineInfo = manipulator.GetLogDataBetween(objZkeeper, int.Parse("1"),F,T,ip);
                    return lstMachineInfo;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            return null;
        }
        
    }
}