#include "StdAfx.h"
#include "ConnectionEventListener.h"

BaseOnLineEvent CConnectionEventListener::baseOnLineEvent;

CConnectionEventListener::CConnectionEventListener(void)
{
	m_dwRefCount =0;
}




CConnectionEventListener::~CConnectionEventListener(void)
{
}

void  CConnectionEventListener::init(BaseOnLineEvent baseOnLineEvent)
{
	CConnectionEventListener::baseOnLineEvent = baseOnLineEvent;
	 
}
