package md5f2f7e9254c347213e2f5ecf704279788;


public class AppLocationService_LocalBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MyAttandance.Services.AppLocationService+LocalBinder, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AppLocationService_LocalBinder.class, __md_methods);
	}


	public AppLocationService_LocalBinder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AppLocationService_LocalBinder.class)
			mono.android.TypeManager.Activate ("MyAttandance.Services.AppLocationService+LocalBinder, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public AppLocationService_LocalBinder (md5f2f7e9254c347213e2f5ecf704279788.AppLocationService p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == AppLocationService_LocalBinder.class)
			mono.android.TypeManager.Activate ("MyAttandance.Services.AppLocationService+LocalBinder, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "MyAttandance.Services.AppLocationService, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
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
