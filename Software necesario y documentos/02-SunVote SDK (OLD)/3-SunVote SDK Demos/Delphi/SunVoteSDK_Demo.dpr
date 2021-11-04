program SunVoteSDK_Demo;

uses
  Forms,
  uFMain in 'uFMain.pas' {frmMain};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrmMain, frmMain);
  Application.Run;
end.
