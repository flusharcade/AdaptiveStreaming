package md509574003816bf54dda4c759772ef922f;


public class Camera2BasicFragment_ImageAvailableListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.media.ImageReader.OnImageAvailableListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onImageAvailable:(Landroid/media/ImageReader;)V:GetOnImageAvailable_Landroid_media_ImageReader_Handler:Android.Media.ImageReader/IOnImageAvailableListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Camera.Droid.Renderers.CameraView.Camera2BasicFragment+ImageAvailableListener, Camera.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Camera2BasicFragment_ImageAvailableListener.class, __md_methods);
	}


	public Camera2BasicFragment_ImageAvailableListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Camera2BasicFragment_ImageAvailableListener.class)
			mono.android.TypeManager.Activate ("Camera.Droid.Renderers.CameraView.Camera2BasicFragment+ImageAvailableListener, Camera.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onImageAvailable (android.media.ImageReader p0)
	{
		n_onImageAvailable (p0);
	}

	private native void n_onImageAvailable (android.media.ImageReader p0);

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
