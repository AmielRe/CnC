using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AnalysisNameAttribute : Attribute
    {
        readonly string _analysisName;

        // Set the analysis name
        public AnalysisNameAttribute(string analysisName)
        {
            this._analysisName = analysisName;
        }

        // Return the analysis name
        public string AnalysisName
        {
            get { return this._analysisName; }
        }
    }
}
