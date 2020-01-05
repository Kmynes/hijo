using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public Image[] imagelist;

    public void PrintOrPutAwayChild(string imagename)
    {
        Image pic = Array.Find(imagelist, imagelist => imagelist.name == imagename);

        if (pic.enabled == true)
        {
            pic.enabled = false;
        }
        else
        {
            pic.enabled = true;
        }

    }

    public void PrintImg(string imagename)
    {
        Image pic = Array.Find(imagelist, imagelist => imagelist.name == imagename);
        pic.enabled = true;
    }
}