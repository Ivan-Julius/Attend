package md5f2f7e9254c347213e2f5ecf704279788;


public class AutoAttendanceService
	extends android.app.Service
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onBind:(Landroid/content/Intent;)Landroid/os/IBinder;:GetOnBind_Landroid_content_Intent_Handler\n" +
			"n_onStartCommand:(Landroid/content/Intent;II)I:GetOnStartCommand_Landroid_content_Intent_IIHandler\n" +
			"n_stopService:(Landroid/content/Intent;)Z:GetStopService_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("MyAttandance.Services.AutoAttendanceService, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AutoAttendanceService.class, __md_methods);
	}


	public AutoAttendanceService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AutoAttendanceService.class)
			mono.android.TypeManager.Activate ("MyAttandance.Services.AutoAttendanceService, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public android.os.IBinder onBind (android.content.Intent p0)
	{
		return n_onBind (p0);
	}

	private native android.os.IBinder n_onBind (android.content.Intent p0);


	public int onStartCommand (android.content.Intent p0, int p1, int p2)
	{
		return n_onStartCommand (p0, p1, p2);
	}

	private native int n_onStartCommand (android.content.Intent p0, int p1, int p2);


	public boolean stopService (android.content.Intent p0)
	{
		return n_stopService (p0);
	}

	private native boolean n_stopService (android.content.Intent p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
