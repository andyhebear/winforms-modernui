﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(MetroFrameworkAssembly.Title)]
[assembly: AssemblyDescription(MetroFrameworkAssembly.Description)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(MetroFrameworkAssembly.Company)]
[assembly: AssemblyProduct(MetroFrameworkAssembly.Product)]
[assembly: AssemblyCopyright(MetroFrameworkAssembly.Copyright)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("9559a6f3-8cce-4644-a571-8aeeeb526094")]

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
[assembly: AssemblyVersion(MetroFrameworkAssembly.Version)]
[assembly: AssemblyFileVersion(MetroFrameworkAssembly.Version)]

[assembly: AllowPartiallyTrustedCallers]

//[assembly: InternalsVisibleTo(MetroFramework.AssemblyRef.MetroFrameworkDesignIVT)]
//[assembly: InternalsVisibleTo(MetroFramework.AssemblyRef.MetroFrameworkFontsIVT)]

internal static class MetroFrameworkAssembly
{
    internal const string Title = "MetroFramework.dll";
    internal const string Version = "1.3.0.0";
    internal const string Description = "Metro UI Framework for .NET WinForms";
    internal const string Copyright = "Copyright \x00a9 2014 Rains.  All rights reserved.";
    internal const string Company = "IROBOTQ";
    internal const string Product = "MetroFramework";
}

namespace MetroFramework
{
    internal static class AssemblyRef
    {

        // Design

        internal const string MetroFrameworkDesign_ = "MetroFramework";

        //internal const string MetroFrameworkDesignSN = "MetroFramework.Design, Version=" + MetroFrameworkAssembly.Version
        //                                               + ", Culture=neutral, PublicKeyToken=" + MetroFrameworkKeyToken;
        internal const string MetroFrameworkDesignSN = "MetroFramework";

        //internal const string MetroFrameworkDesignIVT = "MetroFramework.Design, PublicKey=" + MetroFrameworkKeyFull;
        internal const string MetroFrameworkDesignIVT = "MetroFramework";
        // Fonts

        internal const string MetroFrameworkFonts_ = "MetroFramework";

        //internal const string MetroFrameworkFontsSN = "MetroFramework.Fonts, Version=" + MetroFrameworkAssembly.Version
        //                                              + ", Culture=neutral, PublicKeyToken=" + MetroFrameworkKeyToken;
        internal const string MetroFrameworkFontsSN = "MetroFramework";

       // internal const string MetroFrameworkFontsIVT = "MetroFramework.Fonts, PublicKey=" + MetroFrameworkKeyFull;
        internal const string MetroFrameworkFontsIVT = "MetroFramework";

        internal const string MetroFrameworkFontResolver = "MetroFramework.Fonts.FontResolver, " + MetroFrameworkFontsSN;

        // Strong Name Key

        internal const string MetroFrameworkKey = "5f91a84759bf584a";

        internal const string MetroFrameworkKeyFull =
            "00240000048000009400000006020000002400005253413100040000010001004d3b6f2adab21d" +
            "00d59de966f5d7f4d8325296ded578ac35bca529580b534443bb4090600ff1f057136d58f20a22" +
            "5e0d025119aec656e9b6ac5691e12689c0b03d55c8b8822fd84e2acbde80a2d9124009d20f5adf" +
            "05d36cfa95ba164a0d6ab348a9f8e3a00f066f4d32c0b71b5be6d7f86616491f6dd0630e49ec15" +
            "a0c8f9c9";

        internal const string MetroFrameworkKeyToken = "5f91a84759bf584a";
    }
}

//using System.Reflection;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;

//// 有关程序集的常规信息通过下列属性集
//// 控制。更改这些属性值可修改
//// 与程序集关联的信息。
//[assembly: AssemblyTitle("MetroFrameworkUI")]
//[assembly: AssemblyDescription("")]
//[assembly: AssemblyConfiguration("")]
//[assembly: AssemblyCompany("")]
//[assembly: AssemblyProduct("MetroFrameworkUI")]
//[assembly: AssemblyCopyright("Copyright ©  2014")]
//[assembly: AssemblyTrademark("")]
//[assembly: AssemblyCulture("")]

//// 将 ComVisible 设置为 false 使此程序集中的类型
//// 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，
//// 则将该类型上的 ComVisible 属性设置为 true。
//[assembly: ComVisible(false)]

//// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
//[assembly: Guid("815779c4-048a-4bc5-9820-1b75021e809a")]

//// 程序集的版本信息由下面四个值组成:
////
////      主版本
////      次版本 
////      内部版本号
////      修订号
////
//// 可以指定所有这些值，也可以使用“内部版本号”和“修订号”的默认值，
//// 方法是按如下所示使用“*”:
//// [assembly: AssemblyVersion("1.0.*")]
//[assembly: AssemblyVersion("1.0.0.0")]
//[assembly: AssemblyFileVersion("1.0.0.0")]
