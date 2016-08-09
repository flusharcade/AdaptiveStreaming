package md509574003816bf54dda4c759772ef922f;


public class CameraCaptureStateListener
	extends android.hardware.camera2.CameraCaptureSession.StateCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onConfigureFailed:(Landroid/hardware/camera2/CameraCaptureSession;)V:GetOnConfigureFailed_Landroid_hardware_camera2_CameraCaptureSession_Handler\n" +
			"n_onConfigured:(Landroid/hardware/camera2/CameraCaptureSession;)V:GetOnConfigured_Landroid_hardware_camera2_CameraCaptureSession_Handler\n" +
			"";
		mono.android.Runtime.register ("Camera.Droid.Renderers.CameraView.CameraCaptureStateListener, Camera.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CameraCaptureStateListener.class, __md_methods);
	}


	public CameraCaptureStateListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CameraCaptureStateListener.class)
			mono.android.TypeManager.Activate ("Camera.Droid.Renderers.CameraView.CameraCaptureStateListener, Camera.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onConfigureFailed (android.hardware.camera2.CameraCaptureSession p0)
	{
		n_onConfigureFailed (p0);
	}

	private native void n_onConfigureFailed (android.hardware.camera2.CameraCaptureSession p0);


	public void onConfigured (android.hardware.camera2.CameraCaptureSession p0)
	{
		n_onConfigured (p0);
	}

	private native void n_onConfigured (android.hardware.camera2.CameraCaptureSession p0);

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
