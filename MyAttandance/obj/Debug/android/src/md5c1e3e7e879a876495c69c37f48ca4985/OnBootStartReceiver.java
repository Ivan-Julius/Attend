package md5c1e3e7e879a876495c69c37f48ca4985;


public class OnBootStartReceiver
	extends android.support.v4.content.WakefulBroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("MyAttandance.Receivers.OnBootStartReceiver, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", OnBootStartReceiver.class, __md_methods);
	}


	public OnBootStartReceiver () throws java.lang.Throwable
	{
		super ();
		if (getClass () == OnBootStartReceiver.class)
			mono.android.TypeManager.Activate ("MyAttandance.Receivers.OnBootStartReceiver, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

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
