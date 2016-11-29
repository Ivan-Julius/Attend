package md59355780db2c8ce99e795bdaa51b8e571;


public class MenuViewHodler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MyAttandance.ViewHolders.MenuViewHodler, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MenuViewHodler.class, __md_methods);
	}


	public MenuViewHodler () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MenuViewHodler.class)
			mono.android.TypeManager.Activate ("MyAttandance.ViewHolders.MenuViewHodler, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
