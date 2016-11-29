package md5184b872a2868c10ace2dfaf96583ecb3;


public class SmootherDrawerToggle
	extends android.support.v7.app.ActionBarDrawerToggle
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDrawerOpened:(Landroid/view/View;)V:GetOnDrawerOpened_Landroid_view_View_Handler\n" +
			"n_onDrawerClosed:(Landroid/view/View;)V:GetOnDrawerClosed_Landroid_view_View_Handler\n" +
			"n_onDrawerStateChanged:(I)V:GetOnDrawerStateChanged_IHandler\n" +
			"";
		mono.android.Runtime.register ("MyAttandance.Helper.SmootherDrawerToggle, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", SmootherDrawerToggle.class, __md_methods);
	}


	public SmootherDrawerToggle (android.app.Activity p0, android.support.v4.widget.DrawerLayout p1, android.support.v7.widget.Toolbar p2, int p3, int p4) throws java.lang.Throwable
	{
		super (p0, p1, p2, p3, p4);
		if (getClass () == SmootherDrawerToggle.class)
			mono.android.TypeManager.Activate ("MyAttandance.Helper.SmootherDrawerToggle, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.App.Activity, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Support.V4.Widget.DrawerLayout, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:Android.Support.V7.Widget.Toolbar, Xamarin.Android.Support.v7.AppCompat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3, p4 });
	}


	public SmootherDrawerToggle (android.app.Activity p0, android.support.v4.widget.DrawerLayout p1, int p2, int p3) throws java.lang.Throwable
	{
		super (p0, p1, p2, p3);
		if (getClass () == SmootherDrawerToggle.class)
			mono.android.TypeManager.Activate ("MyAttandance.Helper.SmootherDrawerToggle, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.App.Activity, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Support.V4.Widget.DrawerLayout, Xamarin.Android.Support.v4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onDrawerOpened (android.view.View p0)
	{
		n_onDrawerOpened (p0);
	}

	private native void n_onDrawerOpened (android.view.View p0);


	public void onDrawerClosed (android.view.View p0)
	{
		n_onDrawerClosed (p0);
	}

	private native void n_onDrawerClosed (android.view.View p0);


	public void onDrawerStateChanged (int p0)
	{
		n_onDrawerStateChanged (p0);
	}

	private native void n_onDrawerStateChanged (int p0);

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
