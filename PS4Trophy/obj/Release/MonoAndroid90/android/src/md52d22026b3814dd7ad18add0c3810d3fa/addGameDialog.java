package md52d22026b3814dd7ad18add0c3810d3fa;


public class addGameDialog
	extends android.app.Dialog
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("PS4Trophy.addGameDialog, PS4Trophy", addGameDialog.class, __md_methods);
	}


	public addGameDialog (android.content.Context p0)
	{
		super (p0);
		if (getClass () == addGameDialog.class)
			mono.android.TypeManager.Activate ("PS4Trophy.addGameDialog, PS4Trophy", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
