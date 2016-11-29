package md5f2f7e9254c347213e2f5ecf704279788;


public class AutoAttendanceService_AttendanceBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MyAttandance.Services.AutoAttendanceService+AttendanceBinder, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AutoAttendanceService_AttendanceBinder.class, __md_methods);
	}


	public AutoAttendanceService_AttendanceBinder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AutoAttendanceService_AttendanceBinder.class)
			mono.android.TypeManager.Activate ("MyAttandance.Services.AutoAttendanceService+AttendanceBinder, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public AutoAttendanceService_AttendanceBinder (md5f2f7e9254c347213e2f5ecf704279788.AutoAttendanceService p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == AutoAttendanceService_AttendanceBinder.class)
			mono.android.TypeManager.Activate ("MyAttandance.Services.AutoAttendanceService+AttendanceBinder, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "MyAttandance.Services.AutoAttendanceService, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}

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
