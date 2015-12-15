//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.Behaviors
{
   public class EnumHelper
    {

       public static string GetFieldDecription(Type enumType, object enumValue)
       {
           if (enumValue == null)
               return string.Empty;
           if (enumType == null)
               return string.Empty;
           try
           {
               DescriptionAttribute descriptionAttribute;
               FieldInfo fieldInfo = enumType.GetField(enumValue.ToString());
               descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
               return descriptionAttribute.Description;
           }
           catch(Exception ex)
           {
               throw ex;
           }
       }
    }
}
