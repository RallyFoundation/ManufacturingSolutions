using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrator
{
    public class Result
    {
        public int CreationCount { get; set; }

        public int ModificationCount { get; set; }

        public int DeletionCount { get; set; }

        public int ErrorCount { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            //return base.ToString();

            return String.Format("Creation count: {0}; Modification count: {1}; Deletion count: {2}; Error count: {3}; Message: {4}.", this.CreationCount, this.ModificationCount, this.DeletionCount, this.ErrorCount, this.Message);
        }
    }
}
