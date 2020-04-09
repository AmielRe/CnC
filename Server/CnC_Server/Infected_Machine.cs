using System;
using System.Collections.Generic;
using System.Text;

namespace Server.CnC_Server
{
    /// <summary>
    /// This class represent the Infected_Machine (The bot).
    /// It contains the most current data about the machine, the bot ip and the listening port.
    /// </summary>
    public class Infected_Machine
    {
        private readonly static List<string> validDataNames = new List<string> { "ram", "processes" };

        public readonly string ip;
        public readonly int port;
        private Dictionary<string, double> data;

        public Infected_Machine(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            data = new Dictionary<string, double>();
        }

        /// <summary>
        /// Set a new data about the machine that was retrieved from execute option
        /// </summary>
        /// <param name="nameOfValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public bool SetNewValue(string nameOfValue, double newValue)
        {
            // Modify the name of the value to lower case
            nameOfValue = nameOfValue.ToLower();

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
            // Modify the name of the value to lower case
            nameOfValue = nameOfValue.ToLower();

            // Try to get value and if it doesn't exist then return NaN (Not A Number)
            return data.ContainsKey(nameOfValue) ? data[nameOfValue] : double.NaN;
        }
    }
}
