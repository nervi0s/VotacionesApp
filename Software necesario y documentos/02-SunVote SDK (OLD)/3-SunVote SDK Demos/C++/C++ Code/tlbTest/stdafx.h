
// stdafx.h : 标准系统包含文件的包含文件，
// 或是经常使用但不常更改的
// 特定于项目的包含文件

#pragma once

#ifndef _SECURE_ATL
#define _SECURE_ATL 1
#endif

#ifndef VC_EXTRALEAN
#define VC_EXTRALEAN            // 从 Windows 头中排除极少使用的资料
#endif

#include "targetver.h"

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS      // 某些 CString 构造函数将是显式的

// 关闭 MFC 对某些常见但经常可放心忽略的警告消息的隐藏
#define _AFX_ALL_WARNINGS

#include <afxwin.h>         // MFC 核心组件和标准组件
#include <afxext.h>         // MFC 扩展


#include <afxdisp.h>        // MFC 自动化类



#ifndef _AFX_NO_OLE_SUPPORT
#include <afxdtctl.h>           // MFC 对 Internet Explorer 4 公共控件的支持
#endif
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>             // MFC 对 Windows 公共控件的支持
#endif // _AFX_NO_AFXCMN_SUPPORT

#include <afxcontrolbars.h>     // 功能区和控件条的 MFC 支持

extern CComModule _Module;//增加
#include <atlcom.h>
#include <atlbase.h>
#include <tchar.h>
#include <stdio.h>


#import "SunVote.dll"

using namespace SunVote;

const IID IID_IBaseConnectionEvents={0x33D406A4,0x5729,0x4B40,{0xA2,0x3C,0x52,0xFE,0xC1,0x23,0x72,0x70}};
const CLSID CID_BaseConnectionObj={0xE7F92645,0x9420,0x45AD,{0xB0,0x70,0x29,0x1C,0xB6,0x01,0x6E,0xA7}};

const IID IID_ISingInEvents={0x9CBC4D22,0x5054,0x4394,{0x80,0x6F,0x6E,0xCE,0x13,0x3C,0x09,0xFF}};
const CLSID CID_SignInObj={0x9C74A8C8,0x0F87,0x40EF,{0x9A,0xD3,0xD3,0x82,0x47,0xD7,0x1F,0x32}};




#ifdef _UNICODE
#if defined _M_IX86
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='x86' publicKeyToken='6595b64144ccf1df' language='*'\"")
#elif defined _M_X64
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='amd64' publicKeyToken='6595b64144ccf1df' language='*'\"")
#else
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")
#endif
#endif





