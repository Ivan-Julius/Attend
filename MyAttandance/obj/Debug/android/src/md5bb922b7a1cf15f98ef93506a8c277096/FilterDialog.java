package md5bb922b7a1cf15f98ef93506a8c277096;


public class FilterDialog
	extends android.app.AlertDialog
	implements
		mono.android.IGCUserPeer,
		android.content.DialogInterface.OnShowListener,
		android.app.DatePickerDialog.OnDateSetListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onShow:(Landroid/content/DialogInterface;)V:GetOnShow_Landroid_content_DialogInterface_Handler:Android.Content.IDialogInterfaceOnShowListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onDateSet:(Landroid/widget/DatePicker;III)V:GetOnDateSet_Landroid_widget_DatePicker_IIIHandler:Android.App.DatePickerDialog/IOnDateSetListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("MyAttandance.Dialogs.FilterDialog, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FilterDialog.class, __md_methods);
	}


	public FilterDialog (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == FilterDialog.class)
			mono.android.TypeManager.Activate ("MyAttandance.Dialogs.FilterDialog, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public FilterDialog (android.content.Context p0, boolean p1, android.content.DialogInterface.OnCancelListener p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == FilterDialog.class)
			mono.android.TypeManager.Activate ("MyAttandance.Dialogs.FilterDialog, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Boolean, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.Content.IDialogInterfaceOnCancelListener, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public FilterDialog (android.content.Context p0, int p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == FilterDialog.class)
			mono.android.TypeManager.Activate ("MyAttandance.Dialogs.FilterDialog, MyAttandance, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1 });
	}


	public void onShow (android.content.DialogInterface p0)
	{
		n_onShow (p0);
	}

	private native void n_onShow (android.content.DialogInterface p0);


	public void onDateSet (android.widget.DatePicker p0, int p1, int p2, int p3)
	{
		n_onDateSet (p0, p1, p2, p3);
	}

	private native void n_onDateSet (android.widget.DatePicker p0, int p1, int p2, int p3);

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
