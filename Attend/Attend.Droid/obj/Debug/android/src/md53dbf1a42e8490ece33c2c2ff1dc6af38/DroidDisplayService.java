package md53dbf1a42e8490ece33c2c2ff1dc6af38;


public class DroidDisplayService
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DevExpress.Mobile.DataGrid.Android.DroidDisplayService, DevExpress.Mobile.Grid.Android.v16.1, Version=16.1.2.0, Culture=neutral, PublicKeyToken=null", DroidDisplayService.class, __md_methods);
	}


	public DroidDisplayService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DroidDisplayService.class)
			mono.android.TypeManager.Activate ("DevExpress.Mobile.DataGrid.Android.DroidDisplayService, DevExpress.Mobile.Grid.Android.v16.1, Version=16.1.2.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
