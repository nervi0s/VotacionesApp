import java.io.IOException;
import com.jacob.activeX.ActiveXComponent;
import com.jacob.com.ComThread;
import com.jacob.com.Dispatch;
import com.jacob.com.DispatchEvents;
import com.jacob.com.Variant;

import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;

import javax.swing.*;



public class mainClass extends JFrame{
	   //create components of interface
	   JPanel pNull= new JPanel();
	   JPanel pConn = new JPanel();  
	   JLabel lBaseID = new JLabel("Base Station ID: " , JLabel.RIGHT); 
	   JTextField tBaseID= new JTextField("1");
	   JButton btnConnection = new JButton("Connect"); 
	   JButton btnDisConnect= new JButton("DisConnect");
	   
	   JPanel pConState= new JPanel();
	   JLabel lConState = new JLabel("BaseOnLine: "); 
	   
	   
	   JPanel pSignIn = new JPanel();  
	   JButton btnStart = new JButton("Start SignIn");  
	   JButton btnStop = new JButton("Stop");  
	   
	   JPanel pSignInState = new JPanel();  
	   JLabel lSignInState = new JLabel("KeyStatus: "); 
	   
	   //Define SunVote Objects
	   ActiveXComponent baseConnection;
	   Dispatch baseConnectObj;
	   ActiveXComponent signIn;
	   Dispatch signInObj;
	   
	public  mainClass() throws HeadlessException {  
		 //Interface layout
		 this.addWindowListener(new  WindowAdapter(){
		    	public void windowClosing(WindowEvent e){System.exit(0);}
		    });
		 
		 this.addWindowListener(new WindowAdapter(){
			 public void windowOpened(WindowEvent e) 
			 { 
				 //Initialize SunVote Objects
				 baseConnection= new ActiveXComponent("SunVote.BaseConnection");
				 signIn= new ActiveXComponent("SunVote.SignIn");
				 baseConnectObj = baseConnection.getObject(); 
				 signInObj= signIn.getObject();
			 }
		 });
		 

		 setSize(450 , 300);  
		 setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);  
		 GridLayout gridLayout = new GridLayout(6, 1, 10, 10); 

		 setLayout(gridLayout);  
		 add(pNull);
		 
		 GridLayout gridLayout0 = new GridLayout(1, 1, 10, 10); 
		 pConn.setLayout(gridLayout0); 
		 pConn.add(lBaseID);  
		 pConn.add(tBaseID);
		 pConn.add(btnConnection);  
		 pConn.add(btnDisConnect);
		 btnConnection.addActionListener(new java.awt.event.ActionListener() 
		 {				
			 public void actionPerformed(java.awt.event.ActionEvent evt) 
			 {					
				 btnConnectionActionPerformed(evt);				
				 }			
			 });
		 
		 btnDisConnect.addActionListener(new java.awt.event.ActionListener() 
		 {				
			 public void actionPerformed(java.awt.event.ActionEvent evt) 
			 {					
				 btnDisConnectActionPerformed(evt);				
				 }			
			 });
		 add(pConn);  
		         
		 FlowLayout flowLayout1 = new FlowLayout(FlowLayout.LEFT); 
		 pConState.setLayout(flowLayout1);  
		 pConState.add(lConState);  
		 add(pConState);   
		 
		  FlowLayout flowLayout2 = new FlowLayout(FlowLayout.CENTER);  
		  pSignIn.setLayout(flowLayout2);   
		  pSignIn.add(btnStart);  
		  pSignIn.add(btnStop);  
		  btnStart.addActionListener(new java.awt.event.ActionListener() 
			 {				
				 public void actionPerformed(java.awt.event.ActionEvent evt) 
				 {					
					 btnStartActionPerformed(evt);				
					 }			
				 });
		  btnStop.addActionListener(new java.awt.event.ActionListener() 
			 {				
				 public void actionPerformed(java.awt.event.ActionEvent evt) 
				 {					
					 btnStopActionPerformed(evt);				
					 }			
				 });	 
		  add(pSignIn);  
		 
	     FlowLayout flowLayout3 = new FlowLayout(FlowLayout.LEFT);   
		 pSignInState.setLayout(flowLayout3);  
		 pSignInState.add(lSignInState);  
		 add(pSignInState);  
		 setVisible(true);  
		 
		 
	}  

	protected void btnDisConnectActionPerformed(ActionEvent evt) {
		// TODO Auto-generated method stub
		Variant rtn= Dispatch.call(baseConnectObj, "Close", new Variant[] {});
	}

	protected void btnStopActionPerformed(ActionEvent evt) {
		// TODO Auto-generated method stub
		Variant rtn= Dispatch.call(signInObj, "Stop", new Variant[] {});
	}

	protected void btnStartActionPerformed(ActionEvent evt) {
		// TODO Auto-generated method stub
		Dispatch.put(signInObj, "StartMode", 1);
        Dispatch.put(signInObj,"BaseConnection", baseConnectObj);
		Dispatch.invoke(signInObj, "Start", Dispatch.Method, new Object[] { },new int[0]);
		DispatchEvents events = new DispatchEvents(signInObj, this);
	}

	protected void btnConnectionActionPerformed(ActionEvent evt) {
		// TODO Auto-generated method stub
	     Dispatch.invoke(baseConnectObj, "Open", Dispatch.Method, new Object[] { 1, this.tBaseID.getText() },new int[1]); 
	     DispatchEvents events = new DispatchEvents(baseConnectObj, this);
	}
	
	//return event when you call Open method
	public void BaseOnLine(Variant[] args) {
		this.lConState.setText("BaseOnLine: BaseID="+args[0]+",BaseState="+args[1]);
    }
	
	//return event when you call Start sign in method
	public void KeyStatus(Variant[] args){
		//
		this.lSignInState.setText("KeyStatus: BaseTag="+args[0]+",KeyID="
				+ args[1]+",ValueType="+args[2]+",KeyValue="+args[3]);
	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		mainClass myClass = new mainClass();  
	}
	

}
