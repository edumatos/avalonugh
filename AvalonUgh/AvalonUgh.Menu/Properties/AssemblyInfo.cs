﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ScriptCoreLib;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("AvalonUgh.Menu")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("AvalonUgh.Menu")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2007")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("91a9e2f5-2152-4ebf-8b11-d814dfb83a78")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly:
	Script,
	ScriptTypeFilter(ScriptType.ActionScript, typeof(AvalonUgh.Menu.Shared.MenuCanvas)),
	ScriptTypeFilter(ScriptType.ActionScript, typeof(AvalonUgh.Menu.ActionScript.MenuFlash)),
	ScriptTypeFilter(ScriptType.JavaScript, typeof(AvalonUgh.Menu.Shared.MenuCanvas)),
	ScriptTypeFilter(ScriptType.JavaScript, typeof(AvalonUgh.Menu.JavaScript.MenuDocument)),
]