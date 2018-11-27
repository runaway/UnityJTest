using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;
using System.Threading;

public class SetPlane : MonoBehaviour {

    [DllImport("native-lib")]
    private static extern int DetectObj(string szFileName, byte[] pbyImg, ref uint nWidth, ref uint nHeight);

    Texture2D m_Texture;

    public Material m_Material;
    const int m_iWidth = 1920;// 960; // 854
    const int m_iHeight = 1080;// 540; // 480
    const int m_iChannel = 4; //rgba 4 channel
    const int m_iColorBufferSize = m_iWidth * m_iHeight * m_iChannel;
    byte[] m_abyColorImg = new byte[m_iWidth * m_iHeight * m_iChannel];
    //Color32[] m_Colors = new Color32[m_iWidth * m_iHeight];
    Color[] m_Colors = new Color[m_iWidth * m_iHeight];

    bool m_bInit = false;

    // Use this for initialization
    void Start ()
    {

        InitPlane();


    }
	
	// Update is called once per frame
	void Update ()
    {
        uint nWidth = 0;
        uint nHeight = 0;

        if (true == m_bInit) return;

        if (m_Material != null)
        {
            UnityEngine.Debug.Log("SetPlane: After if (mat != null)");


            {
                UnityEngine.Debug.Log("SetPlane: Before DetectObj");
                DetectObj("/sdcard/Download/image/faces2.jpg", m_abyColorImg, ref nWidth, ref nHeight);
                Debug.LogFormat("SetPlane: After DetectObj {0}, {1}", nWidth, nHeight);
                //nWidth = 640; nHeight = 480;
                m_Material.mainTexture = GenerateColorTextureFromSO(nWidth, nHeight, 4);
                //GetComponent<MeshRenderer>().material.mainTexture = GenerateColorTextureFromSO();
                UnityEngine.Debug.Log("SetPlane: After GenerateColorTextureFromSO");

                m_bInit = true;
            }

            if (null == m_Texture) Debug.Log("SetPlane: null == m_Texture)");
        }

        Thread.Sleep(40);

    }

    void InitPlane()
    {
        m_Material = new Material(Shader.Find("Standard"));

        UnityEngine.Debug.Log("After SetPlane m_Material = new Material");

        m_Texture = new Texture2D(m_iWidth, m_iHeight, TextureFormat.RGBAFloat, false); // ARGB32 BGRA32 RGBA32 RGBAFloat 
        UnityEngine.Debug.Log("After SetPlane m_Texture = new Texture2D");

        m_Material.mainTexture = m_Texture;
        UnityEngine.Debug.Log("After SetPlane m_Material.mainTexture = m_Texture");

        GetComponent<MeshRenderer>().material = m_Material;
    }

    Texture2D GenerateColorTextureFromSO(uint nWidth, uint nHeight, uint nChannel)
    {

        for (int y = 0; y < nHeight; y++)
        {
            for (int x = 0; x < nWidth; x++)
            {

#if false

                m_Colors[y * nWidth + nWidth - x - 1] =
                    new Color32(m_abyColorImg[y * nWidth * 3 + x * 3],
                              m_abyColorImg[y * nWidth * 3 + x * 3 + 1],
                              m_abyColorImg[y * nWidth * 3 + x * 3 + 2],
                              255);
#else
                m_Colors[y * nWidth + nWidth - x - 1] =
                    new Color(m_abyColorImg[y * nWidth * 3 + x * 3 + 2] / 255F,
                              m_abyColorImg[y * nWidth * 3 + x * 3 + 1] / 255F,
                              m_abyColorImg[y * nWidth * 3 + x * 3] / 255F,
                              0.9F);
#endif




            }

        }

        //m_Texture.SetPixels32(0, 0, (int)nWidth, (int)nHeight, m_Colors);
        m_Texture.SetPixels(0, 0, (int)nWidth, (int)nHeight, m_Colors);

        // Apply 使设置生效
        m_Texture.Apply(false, false);

        return m_Texture;
    }

    public void memset(byte[] abyDest, byte ch, int n)
    {
        //foreach (int i in abyDest)
        for (int i = 0; i < n; i++)
        {
            abyDest[i] = ch;
        }
    }

    public void memset32(byte[] abyDest, byte byR, byte byG, byte byB, byte byA, int n)
    {
        //foreach (int i in abyDest)
        for (int i = 0; i < n; i++)
        {
            abyDest[i * 4] = byR;
            abyDest[i * 4 + 1] = byG;
            abyDest[i * 4 + 2] = byB;
            abyDest[i * 4 + 3] = byA;
        }
    }

    public void memcpy(byte[] dest, byte[] src, int n)
    {
        int iWidth = 960;
        int iHeight = 540;
        int iIndex;

        for (int y = 0; y < iHeight; y++)
        {
            for (int x = 0; x < iWidth; x++)
            {
                iIndex = y * iWidth + x;

                dest[iIndex] = src[iIndex];

            }

        }
    }

    public void memcpy(ushort[] dest, ushort[] src, int n)
    {
        int iWidth = 960;
        int iHeight = 540;
        int iIndex;

        for (int y = 0; y < iHeight; y++)
        {
            for (int x = 0; x < iWidth; x++)
            {
                iIndex = y * iWidth + x;

                dest[iIndex] = src[iIndex];

            }

        }
    }

    void DebugLogSerialPort(string strStart, byte[] byData, uint nDataLen)
    {
        string strTemp = strStart;

        for (int i = 0; i < nDataLen; i++)
        {
            strTemp += byData[i].ToString() + " ";
        }

        Debug.Log(strTemp);
    }
}
