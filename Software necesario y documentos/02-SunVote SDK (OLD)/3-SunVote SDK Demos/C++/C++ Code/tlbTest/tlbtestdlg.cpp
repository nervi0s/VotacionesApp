
// tlbTestDlg.cpp : 
//

#include "stdafx.h"
#include "tlbTest.h"
#include "tlbTestDlg.h"
#include "afxdialogex.h"
#include "SignInEventListner.h"
#include "ConnectionEventListener.h"

#include<iostream>
#include<string>
#include<stdio.h>

using namespace std;


#ifdef _DEBUG
#define new DEBUG_NEW
#endif


 

ATL::CComModule _Module;


class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);   



protected:
	DECLARE_MESSAGE_MAP()
};

//SunVote objects
CComPtr<IBaseConnection> pBaseConnection;
CComPtr<ISignIn> pSignIn;

  // use to get callback function
  void onBaseOnLineEvent(long baseID,long baseStates) {
	  CString    strTemp;
	  strTemp.Format(_T("BaseOnLine: The BaseID is %d,BaseState is %d"), baseID,baseStates);
      CtlbTestDlg *dlg=   (CtlbTestDlg*)(AfxGetApp()->GetMainWnd()); //get main dialog
	  dlg->lConnection.SetWindowTextW(strTemp);
}

  // use to get callback function
  void onKeyStatusEvent(BSTR BaseTag, long KeyID, long ValueType,BSTR KeyValue)
  {
	  CString    strTemp;
	  strTemp.Format(_T("SignIn_KeyStatus: The KeyID is %d,KeyValue is %s"), KeyID,KeyValue);
      CtlbTestDlg *dlg=   (CtlbTestDlg*)(AfxGetApp()->GetMainWnd()); 
	  dlg->lSignIn.SetWindowTextW(strTemp);
  }


CAboutDlg::CAboutDlg() : CDialogEx(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()




CtlbTestDlg::CtlbTestDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CtlbTestDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CtlbTestDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_STARTSIGNIN, btnStartSignIn);
	DDX_Control(pDX, IDC_STATICCONNECT, lConnection);
	DDX_Control(pDX, IDC_STATICSIGNIN, lSignIn);
	DDX_Control(pDX, IDC_EDIT2, EdtBaseID);
}

BEGIN_MESSAGE_MAP(CtlbTestDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDOK, &CtlbTestDlg::OnBnClickedOk)

	ON_BN_CLICKED(IDC_STARTSIGNIN, &CtlbTestDlg::OnBnClickedStartsignin)
	ON_BN_CLICKED(IDC_STOP, &CtlbTestDlg::OnBnClickedStop)
	ON_BN_CLICKED(IDCANCEL, &CtlbTestDlg::OnBnClickedCancel)
END_MESSAGE_MAP()




BOOL CtlbTestDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();


	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}


	SetIcon(m_hIcon, TRUE);			
	SetIcon(m_hIcon, FALSE);		

	
	this->EdtBaseID.SetWindowTextW(_T("1"));

    CoInitialize(NULL);
	pBaseConnection.CoCreateInstance(CID_BaseConnectionObj);

 
    IConnectionPointContainer *pCPC;
 
    pBaseConnection->QueryInterface(IID_IConnectionPointContainer,(void **)&pCPC);

 
    IConnectionPoint *pCP;
    pCPC->FindConnectionPoint(IID_IBaseConnectionEvents,&pCP);

 
    pCPC->Release();
     
    

    IUnknown *pSinkUnk;
 
    CConnectionEventListener *pSink = new CConnectionEventListener();

	pSink->init(onBaseOnLineEvent);
    
    pSink->QueryInterface(IID_IUnknown ,(void **)&pSinkUnk);
 
    DWORD dwAdvise;
 
    pCP->Advise(pSinkUnk,&dwAdvise);// make the association with baseconnectioevents

	//

	return TRUE;  

	
}

void CtlbTestDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}


void CtlbTestDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); 

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

HCURSOR CtlbTestDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
	
}

void CtlbTestDlg::OnBnClickedOk()
{


 
 
    this->lConnection.SetWindowTextW(_T(""));
    LONG c = 0;
    CString str;
	this->EdtBaseID.GetWindowTextW(str);
	pBaseConnection->Open(1,_bstr_t(str));
 
    //pCP->Unadvise(dwAdvise); //release association
 
    //pCP->Release();
 
    //pBaseConnection.Release();
 
    //CoUninitialize();




}




void CtlbTestDlg::OnBnClickedStartsignin()
{
	pSignIn.Release();
    CoInitialize(NULL);
	
	HRESULT hr= pSignIn.CoCreateInstance(CID_SignInObj);
	if(hr!=S_OK)
    {
       return ;
    }
 
    IConnectionPointContainer *pCPC;
 
    hr = pSignIn->QueryInterface(IID_IConnectionPointContainer,(void **)&pCPC);
 
    if(!SUCCEEDED(hr))
    {
      return ;
    }
 
    IConnectionPoint *pCP;
    hr = pCPC->FindConnectionPoint(IID_ISingInEvents,&pCP);
 
    if ( !SUCCEEDED(hr) )
    {
       return ;
    }
 
    pCPC->Release();
     
    

    IUnknown *pSinkUnk;
 
    SignInEventListner *pSink = new SignInEventListner();
	pSink->init(onKeyStatusEvent);
    
    hr = pSink->QueryInterface(IID_IUnknown ,(void **)&pSinkUnk);
 
    DWORD dwAdvise;
 
    hr = pCP->Advise(pSinkUnk,&dwAdvise);//make the association with signinevents
 
 
 
    LONG c = 0;
    CString str;
	this->EdtBaseID.GetWindowTextW(str);

	//set properties of signin 
	pSignIn->BaseConnection = pBaseConnection; //must set baseconnection
	
	pSignIn->Mode=0;     // 0 is press "OK" to sign in 
	pSignIn->StartMode=1; // startmode 1: clean and revote
	pSignIn->Start();
 
    //pCP->Unadvise(dwAdvise); //release association
 
    //pCP->Release();
 
    //pBaseConnection.Release();
 
 
 
   // CoUninitialize();

}


void CtlbTestDlg::OnBnClickedStop()
{
	//Stop sign in
	if(pSignIn!=NULL)
	{
		pSignIn->Stop();
	    pSignIn.Release();
	}
}


void CtlbTestDlg::OnBnClickedCancel()
{
	CDialogEx::OnCancel();
}
