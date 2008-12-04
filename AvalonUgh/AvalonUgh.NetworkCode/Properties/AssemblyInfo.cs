using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ScriptCoreLib;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("AvalonUgh.NetworkCode")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AvalonUgh.NetworkCode")]
[assembly: AssemblyCopyright("Copyright ©  2008")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d0846e36-bddf-4f32-8489-9f0945c1d7ab")]

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
	ScriptTypeFilter(ScriptType.ActionScript, typeof(global::AvalonUgh.NetworkCode.Shared.Communication)),
	ScriptTypeFilter(ScriptType.JavaScript, typeof(global::AvalonUgh.NetworkCode.Shared.Communication)),
	ScriptTypeFilter(ScriptType.CSharp2, typeof(global::AvalonUgh.NetworkCode.Shared.Communication)),

	ScriptTypeFilter(ScriptType.CSharp2, typeof(global::AvalonUgh.NetworkCode.Server.NonobaGame)),

	ScriptTypeFilter(ScriptType.ActionScript, typeof(global::AvalonUgh.NetworkCode.Client.ActionScript.NonobaClient)),
	ScriptTypeFilter(ScriptType.ActionScript, typeof(global::AvalonUgh.NetworkCode.Client.Shared.NetworkClient)),
	ScriptTypeFilter(ScriptType.JavaScript, typeof(global::AvalonUgh.NetworkCode.Client.Shared.NetworkClient)),

	//ScriptTypeFilter(ScriptType.ActionScript, "Mahjong.NetworkCode.ClientSide.Shared"),
	//ScriptTypeFilter(ScriptType.ActionScript, "Mahjong.NetworkCode.ClientSide.ActionScript"),
	//ScriptTypeFilter(ScriptType.JavaScript, "Mahjong.NetworkCode.ClientSide.Shared"),
	//ScriptTypeFilter(ScriptType.CSharp2, "Mahjong.NetworkCode.ServerSide")
]
