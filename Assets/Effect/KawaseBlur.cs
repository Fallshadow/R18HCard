using UnityEngine;
using UnityEngine.UI;

namespace act.effect
{
    [RequireComponent(typeof(RawImage))]
    public class KawaseBlur : MonoBehaviour
    {
        public bool IsShow { get { return rawImage.enabled; } }

        [SerializeField] private float offset = 0.5f;
        [SerializeField] private int interation = 5;

        private RenderTexture[] tempRTs = new RenderTexture[2];
        private RawImage rawImage = null;
        private int offsetId;
        private int width = 320;
        private int height = 180;

        private void Awake()
        {
            rawImage = GetComponent<RawImage>();
            offsetId = Shader.PropertyToID("_Offset");
            width = Screen.width;
            height = Screen.height;
        }

        public void Show()
        {
            rawImage.enabled = true;
        }

        public void Hide()
        {
            rawImage.enabled = false;
            for (int i = 0; i < tempRTs.Length; ++i)
            {
                if (tempRTs[i] != null)
                {
                    RenderTexture.ReleaseTemporary(tempRTs[i]);
                    tempRTs[i] = null;
                }
            }
        }

        public void Blur(Texture src)
        {
            for (int i = 0; i < tempRTs.Length; ++i)
            {
                if (tempRTs[i] == null)
                {
                    tempRTs[i] = RenderTexture.GetTemporary(width / 2, height / 2, 0, RenderTextureFormat.ARGB32);
                    tempRTs[i].name = "KawaseBlurRT";
                }
            }

            // Kawase Blur
            Graphics.Blit(src, tempRTs[0]);
            bool swich = true;
            for (int i = 0; i < interation; ++i)
            {
                clearBuffer(swich ? tempRTs[1] : tempRTs[0]);
                rawImage.material.SetFloat(offsetId, i + offset);
                Graphics.Blit(swich ? tempRTs[0] : tempRTs[1], swich ? tempRTs[1] : tempRTs[0], rawImage.material);
                swich = !swich;
            }
            rawImage.texture = swich ? tempRTs[1] : tempRTs[0];

        }

        private void clearBuffer(RenderTexture rt)
        {
            Graphics.SetRenderTarget(rt);
            GL.Clear(true, true, Color.black);
        }
    }
}