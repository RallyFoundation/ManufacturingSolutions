﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataIntegrator
{
    public class Operation
    {
        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Message
        {
            get;
            set;
        }

        public virtual OperationMethod Method
        {
            get;
            set;
        }

        public virtual List<Argument> Arguments
        {
            get;
            set;
        }

        public virtual Operation SubsequentOperation 
        { 
            get; 
            set; 
        }

        public virtual string GetArgumentValue(string ArgumentName)
        {
            if ((this.Arguments != null) && (this.Arguments.Count > 0))
            {
                foreach (Argument argument in this.Arguments)
                {
                    if (argument.Name.ToLower() == ArgumentName.ToLower())
                    {
                        return argument.Value;
                    }
                }
            }

            return null;
        }

        public virtual IDictionary<string, object> GetArguments(string ArgumentCategory) 
        {
            if ((this.Arguments != null) && (this.Arguments.Count > 0))
            {
                IDictionary<string, object> returnValue = new Dictionary<string, object>();

                foreach (Argument argument in this.Arguments)
                {
                    if (argument.Category.ToLower() == ArgumentCategory.ToLower())
                    {
                        if (!String.IsNullOrEmpty(argument.Name))
                        {
                            returnValue.Add(argument.Name, argument.Value);
                        }
                    }
                }

                return returnValue;
            }

            return null;
        }
    }
}
