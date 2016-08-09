package md509574003816bf54dda4c759772ef922f;


public class Camera2BasicFragment_ErrorDialog
	extends android.app.DialogFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreateDialog:(Landroid/os/Bundle;)Landroid/app/Dialog;:GetOnCreateDialog_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Camera.Droid.Renderers.CameraView.Camera2BasicFragment+ErrorDialog, Camera.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Camera2BasicFragment_ErrorDialog.class, __md_methods);
	}


	public Camera2BasicFragment_ErrorDialog () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Camera2BasicFragment_ErrorDialog.class)
			mono.android.TypeManager.Activate ("Camera.Droid.Renderers.CameraView.Camera2BasicFragment+ErrorDialog, Camera.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public android.app.Dialog onCreateDialog (android.os.Bundle p0)
	{
		return n_onCreateDialog (p0);
	}

	private native android.app.Dialog n_onCreateDialog (android.os.Bundle p0);

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
