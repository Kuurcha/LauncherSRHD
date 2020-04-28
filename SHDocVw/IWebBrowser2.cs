using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SHDocVw
{
	[ComImport]
	[CompilerGenerated]
	[Guid("D30C1661-CDAF-11D0-8A3E-00C04FC9E26E")]
	[DefaultMember("Name")]
	[TypeIdentifier]
	public interface IWebBrowser2 : IWebBrowserApp
	{
		[DispId(0)]
		new string Name
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			[DispId(0)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
		}

		new void _VtblGap1_29();

		void _VtblGap2_17();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[DispId(502)]
		void ExecWB([In] OLECMDID cmdID, [In] OLECMDEXECOPT cmdexecopt, [Optional] [In] [MarshalAs(UnmanagedType.Struct)] ref object pvaIn, [Optional] [In] [Out] [MarshalAs(UnmanagedType.Struct)] ref object pvaOut);
	}
}
