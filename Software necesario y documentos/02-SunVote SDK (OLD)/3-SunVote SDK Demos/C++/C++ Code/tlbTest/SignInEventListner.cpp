#include "StdAfx.h"
#include "SignInEventListner.h"

KeyStatusEvent SignInEventListner::keyStatusEvent;

SignInEventListner::SignInEventListner(void)
{
}


SignInEventListner::~SignInEventListner(void)
{
}

void  SignInEventListner::init(KeyStatusEvent keyStatusEvent)
{
	SignInEventListner::keyStatusEvent = keyStatusEvent;
	 
}
