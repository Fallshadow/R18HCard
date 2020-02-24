using System.Collections.Generic;
using UnityEngine;

namespace act.utility
{
    public static class CameraUtility
    {
        private static Texture2D screenShot = null;

        // NOTE: Need to call after yield return new WaitForEndOfFrame();
        public static Texture2D TakeShot(int width, int height)
        {
            if (screenShot != null)
            {
                Object.Destroy(screenShot);
            }

            screenShot = new Texture2D(width, height);
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenShot.Apply();

            return screenShot;
        }

        public static Texture2D TakeShot(Camera cam, int width, int height)
        {
            if (screenShot != null)
            {
                Object.Destroy(screenShot);
            }

            var lastActiveRt = RenderTexture.active;
            var lastTargetTex = cam.targetTexture;
            var rt = new RenderTexture(width, height, 24);
            RenderTexture.active = rt;
            cam.targetTexture = rt;
            cam.Render();

            screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenShot.Apply();

            RenderTexture.active = lastActiveRt;
            cam.targetTexture = lastTargetTex;
            Object.Destroy(rt);

            return screenShot;
        }

        public static Texture2D TakeShot(List<Camera> cameras, int width, int height)
        {
            if (screenShot != null)
            {
                Object.Destroy(screenShot);
            }

            int count = cameras.Count;
            var lastActiveRt = RenderTexture.active;
            var lastTargetTextures = new RenderTexture[count];
            var rt = new RenderTexture(width, height, 24);
            RenderTexture.active = rt;
            for (int i = count - 1; i >= 0; --i)
            {
                lastTargetTextures[i] = cameras[i].targetTexture;
                cameras[i].targetTexture = rt;
                cameras[i].Render();
            }

            screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenShot.Apply();
            RenderTexture.active = lastActiveRt;
            for (int i = count - 1; i >= 0; --i)
            {
                cameras[i].targetTexture = lastTargetTextures[i];
            }
            Object.Destroy(rt);
            return screenShot;
        }
    }
}