unit uFMain;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, OleServer, SunVote_TLB;

type
  TfrmMain = class(TForm)
    lblBaseNum: TLabel;
    lblSignModel: TLabel;
    btnStop: TButton;
    edtBaseId: TEdit;
    btnConnectBase: TButton;
    btnCloseBase: TButton;
    btnStart: TButton;
    BaseConnection: TBaseConnection;
    SignIn: TSignIn;
    lstState: TListBox;
    procedure btnConnectBaseClick(Sender: TObject);
    procedure btnCloseBaseClick(Sender: TObject);
    procedure btnStartClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure btnStopClick(Sender: TObject);
    procedure BaseConnectionBaseOnline(ASender: TObject; BaseID,
      BaseState: Integer);
    procedure SignInKeyStatus(ASender: TObject; const BaseTag: WideString;
      KeyID, ValueType: Integer; const KeyValue: WideString);
  private
    { Private declarations }
    Procedure ShowMsg(Msg: String); //Show message
  public
    { Public declarations }
  end;

var
  frmMain: TfrmMain;

implementation

{$R *.dfm}

procedure TfrmMain.FormCreate(Sender: TObject);
begin
   //Set the connection properties of sign-in objects.
  signIn.BaseConnection := baseConnection.DefaultInterface;
end;

procedure TfrmMain.btnConnectBaseClick(Sender: TObject);
begin
  BaseConnection.Open(1, edtBaseId.Text); //Connect base station by usb ports.
end;

procedure TfrmMain.btnCloseBaseClick(Sender: TObject);
begin
  BaseConnection.Close; //Close the connection of base station
end;

procedure TfrmMain.btnStartClick(Sender: TObject);
var
  lState:string;
begin
  signIn.Mode := 0; //Enable the key-press sign-in by pressing a single key
  signIn.StartMode := 1; //Restart
  lState:=signIn.Start;
  ShowMsg('signIn.Start:' + lState);
end;

procedure TfrmMain.btnStopClick(Sender: TObject);
var
  lState:string;
begin
  lState:=signIn.Stop;  //Stop the key-press sign-in
  ShowMsg('signIn.Stop:' + lState);
end;

//******************************************************************************
//Functions: The events for connecting objects of base station, return the base
//           sation ID number and connecting state.
//Parameters: BaseID:Base station ID
//            BaseState:connecting state of base station
//Returned: No
//******************************************************************************
procedure TfrmMain.BaseConnectionBaseOnline(ASender: TObject; BaseID,
  BaseState: Integer);
Var
  s: String;
Begin
  s := 'BaseOnline: BaseId=' + IntToStr(BaseID) + ',BaseState =' + IntToStr(BaseState);

  Case BaseState Of
    0:
      s := s + '  Connect Base Failure or BaseConnection Close!';
    1:
      s := s + '  Connect Base Success!';
    -1:
      s := s + '  Can Not Support The ConnectType !';
    -2:
      s := s + '  Can not find Base!';
    -3:
      s := s + '  Port Error!';
    -4:
      s := s + '  The Baseconnection has been closed!';
  Else
  End;

  ShowMsg(s);
end;

//******************************************************************************
//  Functions:  Show message
//  parameters:  Msg: message
//  Returned:    No
//******************************************************************************
procedure TfrmMain.ShowMsg(Msg: String);
begin
  Msg := FormatDateTime('hh:mm:ss.zzz', now) + ' ' + Msg;
  lstState.Items.Insert(0, Msg);
end;

//******************************************************************************
//Functions: The sign-in event of sign-in objects, return the sign-in datas of keypads
//parameters:
//            BaseTag:Base ID
//            KeyID:Keypad ID number
//            SignInType:Types of sign-in
//            KeyValue??Values of keys
//Returned: No
//******************************************************************************
procedure TfrmMain.SignInKeyStatus(ASender: TObject;
  const BaseTag: WideString; KeyID, ValueType: Integer;
  const KeyValue: WideString);
Var
  lMsg: String;
Begin
  lMsg := 'SignIn_KeyStatus:BaseTag=' + BaseTag +
    ',KeyID=' + IntToStr(KeyID) +
    ',ValueType=' + IntToStr(ValueType) +
    ',KeyValue=' + KeyValue;

  ShowMsg(lMsg);
end;

end.
