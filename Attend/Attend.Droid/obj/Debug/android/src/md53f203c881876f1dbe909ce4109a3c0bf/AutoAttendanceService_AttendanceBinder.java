package md53f203c881876f1dbe909ce4109a3c0bf;


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
		mono.android.Runtime.register ("Attend.Droid.Services.AutoAttendanceService+AttendanceBinder, Attend.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AutoAttendanceService_AttendanceBinder.class, __md_methods);
	}


	public AutoAttendanceService_AttendanceBinder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AutoAttendanceService_AttendanceBinder.class)
			mono.android.TypeManager.Activate ("Attend.Droid.Services.AutoAttendanceService+AttendanceBinder, Attend.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public AutoAttendanceService_AttendanceBinder (md53f203c881876f1dbe909ce4109a3c0bf.AutoAttendanceService p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == AutoAttendanceService_AttendanceBinder.class)
			mono.android.TypeManager.Activate ("Attend.Droid.Services.AutoAttendanceService+AttendanceBinder, Attend.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Attend.Droid.Services.AutoAttendanceService, Attend.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
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
