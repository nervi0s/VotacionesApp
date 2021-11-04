
// tlbTestDlg.h : Í·ÎÄ¼þ
//

#pragma once
#include "afxwin.h"



// CtlbTestDlg dialog
class CtlbTestDlg : public CDialogEx
{
// Construct
public:
	CtlbTestDlg(CWnd* pParent = NULL);	// 

	enum { IDD = IDD_TLBTEST_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 
	

// 
protected:
	HICON m_hIcon;

	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	afx_msg void OnEnChangeEdit1();
	CButton btnStartSignIn;
	CStatic lConnection;
	CStatic lSignIn;
	CEdit EdtBaseID;
	afx_msg void OnBnClickedStartsignin();
	afx_msg void OnBnClickedStop();
	afx_msg void OnBnClickedCancel();
};
