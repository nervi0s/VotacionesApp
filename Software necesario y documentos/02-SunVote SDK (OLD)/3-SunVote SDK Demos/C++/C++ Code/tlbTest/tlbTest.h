
// tlbTest.h : PROJECT_NAME application header file
//

#pragma once

#ifndef __AFXWIN_H__
	#error "Before include this file contains the PCH stdafx.h" to generate the file
#endif

#include "resource.h"		


// CtlbTestApp:
// About the realization of the class£¬please reference tlbTest.cpp
//

class CtlbTestApp : public CWinApp
{
public:
	CtlbTestApp();

// rewriten
public:
	virtual BOOL InitInstance();
	
// realization

	DECLARE_MESSAGE_MAP()
};

extern CtlbTestApp theApp;