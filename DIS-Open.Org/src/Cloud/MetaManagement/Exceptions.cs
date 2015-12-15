using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DISConfigurationCloud.MetaManagement
{
    public class ConfigurationCountLimitExceededException : Exception
    {
        public ConfigurationCountLimitExceededException(string message) : base(message)
        {
        }
    }

    public class DuplicatedConfigurationTypeException : Exception 
    {
        public DuplicatedConfigurationTypeException(string message) : base(message) 
        {
        }
    }

    public class DuplicatedDatabaseException : Exception 
    {
        public DuplicatedDatabaseException(string message) : base(message) 
        {
        }
    }

    public class InvalidConnectionStringException : Exception 
    {
        public InvalidConnectionStringException(string message) : base(message) 
        {
        }
    }
}
