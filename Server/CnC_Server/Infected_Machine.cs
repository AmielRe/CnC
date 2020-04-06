using System;
using System.Collections.Generic;
using System.Text;

namespace Server.CnC_Server
{
    public class Infected_Machine
    {
        private readonly static List<string> validDataNames = new List<string> { "ram", "processes" };

        private readonly string ip;
        private readonly int port;
        private Dictionary<string, double> data;

        public Infected_Machine(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            data = new Dictionary<string, double>();
        }

        public bool SetNewValue(string nameOfValue, double newValue)
        {
            // Check if the value to set is valid
            if (validDataNames.Contains(nameOfValue))
            {
                // Set the new value / update existing value
                data[nameOfValue] = newValue;
                return true;
            }

            return false;
        }

        public double GetValueByName(string nameOfValue)
        {
            // Try to get value and if it doesn't exist then return NaN (Not A Number)
            return data.ContainsKey(nameOfValue) ? data[nameOfValue] : double.NaN;
        }
    }
}
