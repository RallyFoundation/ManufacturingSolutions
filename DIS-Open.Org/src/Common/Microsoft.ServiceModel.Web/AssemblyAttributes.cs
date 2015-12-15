//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyCopyright("\x00a9 Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyDescription("Microsoft WCF REST Service Development API")]
[assembly: AssemblyProduct("Microsoft (R) WCF REST Starter Kit CodePlex Preview 2")]
[assembly: AssemblyTitle("Microsoft.ServiceModel.Web.dll")]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Revision
//      Build Number
//
// You can specify all the value or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion("1.0.0.0")]

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]

[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityCritical(SecurityCriticalScope.Explicit)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]

