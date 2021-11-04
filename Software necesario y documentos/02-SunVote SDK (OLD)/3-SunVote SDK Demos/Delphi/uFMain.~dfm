object frmMain: TfrmMain
  Left = 285
  Top = 219
  Width = 640
  Height = 460
  BorderIcons = [biSystemMenu, biMinimize]
  Caption = 'SunVoteSDK_Demo'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  Position = poMainFormCenter
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object lblBaseNum: TLabel
    Left = 23
    Top = 24
    Width = 106
    Height = 21
    Alignment = taRightJustify
    AutoSize = False
    Caption = 'Base station ID'
  end
  object lblSignModel: TLabel
    Left = 63
    Top = 60
    Width = 129
    Height = 21
    Alignment = taRightJustify
    AutoSize = False
    Caption = 'Sign-in mode: Key-press '
    WordWrap = True
  end
  object btnStop: TButton
    Left = 376
    Top = 56
    Width = 120
    Height = 25
    Caption = 'Stop signing in'
    TabOrder = 1
    OnClick = btnStopClick
  end
  object edtBaseId: TEdit
    Left = 135
    Top = 20
    Width = 57
    Height = 21
    TabOrder = 2
    Text = '1'
  end
  object btnConnectBase: TButton
    Left = 240
    Top = 16
    Width = 120
    Height = 25
    Caption = 'Connect base station'
    TabOrder = 3
    OnClick = btnConnectBaseClick
  end
  object btnCloseBase: TButton
    Left = 376
    Top = 16
    Width = 120
    Height = 25
    Caption = 'Close connection'
    TabOrder = 4
    OnClick = btnCloseBaseClick
  end
  object btnStart: TButton
    Left = 240
    Top = 56
    Width = 120
    Height = 25
    Caption = 'Start to sign in'
    TabOrder = 0
    OnClick = btnStartClick
  end
  object lstState: TListBox
    Left = 0
    Top = 102
    Width = 624
    Height = 320
    Align = alBottom
    ItemHeight = 13
    TabOrder = 5
  end
  object BaseConnection: TBaseConnection
    AutoConnect = False
    ConnectKind = ckRunningOrNew
    OnBaseOnLine = BaseConnectionBaseOnline
    Left = 14
    Top = 387
  end
  object SignIn: TSignIn
    AutoConnect = False
    ConnectKind = ckRunningOrNew
    OnKeyStatus = SignInKeyStatus
    Left = 47
    Top = 387
  end
end
