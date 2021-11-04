VERSION 5.00
Begin VB.Form frmMain 
   Caption         =   "SunVoteSDK_Demo"
   ClientHeight    =   6900
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   9600
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   460
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   640
   StartUpPosition =   2  'ÆÁÄ»ÖÐÐÄ
   Begin VB.ListBox lstState 
      Height          =   5280
      Left            =   60
      TabIndex        =   6
      Top             =   1515
      Width           =   9495
   End
   Begin VB.CommandButton cmdStop 
      Caption         =   "Stop signing in"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   5640
      TabIndex        =   5
      Top             =   840
      Width           =   1800
   End
   Begin VB.CommandButton cmdStart 
      Caption         =   "Start to sign in"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3600
      TabIndex        =   4
      Top             =   840
      Width           =   1800
   End
   Begin VB.CommandButton cmdCloseBase 
      Caption         =   "Close connection"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   5640
      TabIndex        =   2
      Top             =   240
      Width           =   1800
   End
   Begin VB.CommandButton cmdConnectBase 
      Caption         =   "Connect base station"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3600
      TabIndex        =   1
      Top             =   240
      Width           =   1800
   End
   Begin VB.TextBox txtBaseId 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   2130
      TabIndex        =   0
      Text            =   "1"
      Top             =   375
      Width           =   855
   End
   Begin VB.Label lblSignModel 
      AutoSize        =   -1  'True
      Caption         =   "Sign-in mode: Key-press "
      Height          =   180
      Left            =   840
      TabIndex        =   7
      Top             =   945
      Width           =   2160
   End
   Begin VB.Label lblBaseNum 
      AutoSize        =   -1  'True
      Caption         =   "Base station ID"
      Height          =   180
      Left            =   690
      TabIndex        =   3
      Top             =   450
      Width           =   1350
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Private Declare Sub GetSystemTime Lib "kernel32" (lpSystemTime As SYSTEMTIME)

Private WithEvents BaseConnection As SunVote.BaseConnection 'Base station conneciton object statement
Attribute BaseConnection.VB_VarHelpID = -1

Private WithEvents SignIn As SunVote.SignIn 'Application business object statement
Attribute SignIn.VB_VarHelpID = -1

Dim ms As String

Private Type SYSTEMTIME
    wYear As Integer
    wMonth As Integer
    wDayOfWeek As Integer
    wDay As Integer
    wHour As Integer
    wMinute As Integer
    wSecond As Integer
    wMilliseconds As Integer
End Type

Private Sub cmdCloseBase_Click()
    BaseConnection.Close  'Close the connection of base station
End Sub

Private Sub cmdConnectBase_Click()
    BaseConnection.Open 1, txtBaseId.Text 'Connect base station by usb ports.
End Sub

Private Sub cmdStart_Click()
    Dim lState As String

    SignIn.Mode = 0  'Enable the key-press sign-in by pressing a single key
    SignIn.StartMode = 1 '/Restart
    lState = SignIn.Start
    ShowMsg "signIn.Start:" + lState
End Sub

Private Sub cmdStop_Click()
    Dim lState As String

    lState = SignIn.Stop 'Stop the key-press sign-in
    ShowMsg "signIn.Stop:" + lState
End Sub

Private Sub Form_Load()
    Set BaseConnection = New SunVote.BaseConnection 'Creat the base staiton object
    Set SignIn = New SunVote.SignIn 'Creat the business object: this sample is for sign-in
    
    SignIn.BaseConnection = BaseConnection 'Set the connection properties of sign-in objects.
End Sub

'******************************************************************************
'  Functions:  Show message
'  parameters:  Msg: message
'  Returned:    No
'******************************************************************************
Private Sub ShowMsg(ByVal Msg As String)
    Dim st As SYSTEMTIME
    Dim stime As String
    
    GetSystemTime st
    stime = Format(Now, "hh:mm:ss") & "." & Right("000" & st.wMilliseconds, 3)

    Msg = stime & " " & Msg
    lstState.AddItem Msg, 0
End Sub

'******************************************************************************
'Functions: The sign-in event of sign-in objects, return the sign-in datas of keypads
'parameters:BaseTag:Base ID
'            KeyID:Keypad ID number
'            SignInType:Types of sign-in
'            KeyValue£ºValues of keys
'Returned: No
'******************************************************************************
Private Sub SignIn_KeyStatus(ByVal BaseTag As String, ByVal KeyID As Long, ByVal ValueType As Long, ByVal KeyValue As String)
    Dim lMsg As String

    lMsg = "SignIn_KeyStatus:BaseTag=" & BaseTag & ",KeyID=" & KeyID & _
           ",ValueType=" & ValueType & ",KeyValue=" & KeyValue

    ShowMsg lMsg
End Sub


'******************************************************************************
'Functions: The events for connecting objects of base station, return the base
'           sation ID number and connecting state.
'Parameters: BaseID:Base station ID
'            BaseState:connecting state of base station
'Returned: No
'******************************************************************************


Private Sub BaseConnection_BaseOnline(ByVal BaseID As Long, ByVal BaseState As Long)
    Dim s As String
  
  On Error GoTo Error
  s = "BaseOnline: BaseId=" & BaseID & ",BaseState=" & BaseState
  Select Case BaseState
     Case 0
      s = s + "  Connect Base Failure or BaseConnection Close!"
    Case 1
      s = s + "  Connect Base Success!"
    Case -1
      s = s + "  Can Not Support The ConnectType !"
    Case -2
      s = s + "  Can not find Base!"
    Case -3
      s = s + "  Port Error!"
    Case -4
      s = s + "  The Baseconnection has been closed!"
  End Select
  
  ms = s
  
  ShowMsg s
 Exit Sub

Error:
    MsgBox Err.Description, vbCritical
End Sub

