﻿/// ---------------------------------------------------
/// date ： 2015/01/25 
/// brief ： ローカルでキャプチャーを保存
/// author ： Yamada Masamistu
/// ---------------------------------------------------
/// 
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#if UNITY_METRO && !UNITY_EDITOR
using LegacySystem.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using System;
#else
using System.IO;
#endif

public class CaptureLocalSaveController : MonoBehaviour {

    CaptureController Capture = null;

    Button ClickButton = null;

    int ID = 0;

	// Use this for initialization
	void Start () {
        if (!ClickButton)
        {
            ClickButton = GetComponent<Button>();
            ClickButton.onClick.AddListener(Save);
        }
            
	}

	// Update is called once per frame
	void Update () {
	
	}

    void Save()
    {
        if (ModeManager.IsResetMode) return;

        if (!Capture)
        {
            Capture = GetComponent<CaptureController>();
        }

        StartCoroutine("WaitSave");
    }

    /// <summary>
    /// 保存する。
    /// </summary>
    IEnumerator WaitSave()
    {
        yield return new WaitForSeconds(1.0f);

        ID++;

        var bytes = Capture.Texture.EncodeToJPG();

#if UNITY_METRO && !UNITY_EDITOR
        LibForWinRT.WriteSharePicture("PicGather", ID + ".jpg", bytes);
#else
        var folderPath = Application.persistentDataPath + "/Share/";
        var filePath = string.Format("{0}{1}", folderPath, ID + ".jpg");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        File.WriteAllBytes(filePath, bytes);
#endif
    }
}
