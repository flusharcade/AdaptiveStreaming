package md509574003816bf54dda4c759772ef922f;


public class Camera2BasicFragment_MyDialogOnClickListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.content.DialogInterface.OnClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/content/DialogInterface;I)V:GetOnClick_Landroid_content_DialogInterface_IHandler:Android.Content.IDialogInterfaceOnClickListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Camera.Droid.Renderers.CameraView.Camera2BasicFragment+MyDialogOnClickListener, Camera.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Camera2BasicFragment_MyDialogOnClickListener.class, __md_methods);
	}


	public Camera2BasicFragment_MyDialogOnClickListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Camera2BasicFragment_MyDialogOnClickListener.class)
			mono.android.TypeManager.Activate ("Camera.Droid.Renderers.CameraView.Camera2BasicFragment+MyDialogOnClickListener, Camera.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public Camera2BasicFragment_MyDialogOnClickListener (md509574003816bf54dda4c759772ef922f.Camera2BasicFragment_ErrorDialog p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == Camera2BasicFragment_MyDialogOnClickListener.class)
			mono.android.TypeManager.Activate ("Camera.Droid.Renderers.CameraView.Camera2BasicFragment+MyDialogOnClickListener, Camera.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Camera.Droid.Renderers.CameraView.Camera2BasicFragment+ErrorDialog, Camera.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onClick (android.content.DialogInterface p0, int p1)
	{
		n_onClick (p0, p1);
	}

	private native void n_onClick (android.content.DialogInterface p0, int p1);

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
