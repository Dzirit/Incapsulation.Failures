using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
    public enum FailureType
    {
        unexpectedShutdown,
        nonResponding,
        hardwareFailures,
        connectionProblems
    }
    public class Device
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
    public class Failure
    {
        public int DeviceId { get; set; }
        public FailureType Type { get; set; }
        public DateTime FailureDate { get; set; }
        public static bool IsFailureSerious(FailureType failureType)
        {
            if ((int)failureType % 2 == 0) return true;
            return false;
        }
    }
    public class Common
    {
        
        public static bool Earlier(DateTime failureDate, DateTime curDate)
        {
            if (failureDate < curDate) return true;
            return false;
        }
    }

    public class ReportMaker
    {
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
        int day,
            int month,
            int year,
            int[] failureTypes,
            int[] deviceId,
            object[][] times,
            List<Dictionary<string, object>> devices)
        {

            var curDate = new DateTime(year, month, day);
            Failure[] failures = new Failure[failureTypes.Length];
            Device[] devices1 = new Device[devices.Count];

            for (int i=0;i<failureTypes.Length;i++)
            {
                failures[i] = new Failure();
                failures[i].DeviceId = deviceId[i];
                failures[i].FailureDate = new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0]);
                failures[i].Type = (FailureType)failureTypes[i];
                devices1[i] = new Device();
                devices1[i].Id = (int)devices[i]["DeviceId"];
                devices1[i].Name = (string)devices[i]["Name"];
            }

                return FindDevicesFailedBeforeDate(curDate,failures,devices1);
        }
        public static List<string> FindDevicesFailedBeforeDate(
           DateTime curDate,
           Failure[] failures,
           Device[] devices)
        {

            var problematicDevices = new HashSet<int>();
            for (int i = 0; i < failures.Length; i++)
                if (Failure.IsFailureSerious(failures[i].Type) && Common.Earlier(failures[i].FailureDate, curDate))
                    problematicDevices.Add(failures[i].DeviceId);

            var result = new List<string>();
            foreach (var device in devices)
                if (problematicDevices.Contains(device.Id))
                    result.Add(device.Name);

            return result;
        }


    }
}
