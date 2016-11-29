package md5a836cfc6d4ae14a083f69c16997ae5c7;


public class LocationLog
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MyAttandance.Entities.LocationLog, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LocationLog.class, __md_methods);
	}


	public LocationLog () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LocationLog.class)
			mono.android.TypeManager.Activate ("MyAttandance.Entities.LocationLog, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
