using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.VR.WSA.WebCam;

public class PhotoCaptureTest : MonoBehaviour {

    PhotoCapture capture = null;
    Texture2D targetTexture = null;
    CameraParameters cameraParameters;
    string folderPath = "";
   
    // Use this for initialization
    void Start () {
        Resolution cameraResolution = cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
            capture = captureObject;
            cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;



        });
    }


    public void TakePicture()
    {
       
        capture.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {

            string filename = string.Format(@"\CapturedImage{0}_n.jpg", Time.time);
            string filePath = folderPath + filename;
            string currentDir = Directory.GetCurrentDirectory();
          
            capture.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
        });
    }
    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("Saved Photo to disk!");
            capture.StopPhotoModeAsync(OnStoppedPhotoMode);
        }
        else
        {
            Debug.Log("Failed to save Photo to disk");
        }
    }
    
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown our photo capture resource
        capture.Dispose();
        
    }
    // Update is called once per frame
    void Update () {
		
	}
}
