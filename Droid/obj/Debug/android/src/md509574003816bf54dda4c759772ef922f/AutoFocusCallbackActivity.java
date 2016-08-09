package md509574003816bf54dda4c759772ef922f;


public class AutoFocusCallbackActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer,
		android.hardware.Camera.AutoFocusCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onAutoFocus:(ZLandroid/hardware/Camera;)V:GetOnAutoFocus_ZLandroid_hardware_Camera_Handler:Android.Hardware.Camera/IAutoFocusCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Camera.Droid.Renderers.CameraView.AutoFocusCallbackActivity, Camera.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AutoFocusCallbackActivity.class, __md_methods);
	}


	public AutoFocusCallbackActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AutoFocusCallbackActivity.class)
			mono.android.TypeManager.Activate ("Camera.Droid.Renderers.CameraView.AutoFocusCallbackActivity, Camera.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onAutoFocus (boolean p0, android.hardware.Camera p1)
	{
		n_onAutoFocus (p0, p1);
	}

	private native void n_onAutoFocus (boolean p0, android.hardware.Camera p1);

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
