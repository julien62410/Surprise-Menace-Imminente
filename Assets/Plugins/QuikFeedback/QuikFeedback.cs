using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Pataya.QuikFeedback
{
    [System.Serializable]
    public class Feedback
    {
        [Header("Feedback Settings")]
        public float delay = 0.0f;
    }

    #region Particles

    [System.Serializable]
    public class ParticleFeedback : Feedback
    {
        [Header("Particle Settings")]
        public ParticleSystem[] particleSystems;
    }

    #endregion

    #region Animation

    public enum AnimationParameterType
    {
        Trigger,
        Bool
    }

    [System.Serializable]
    public class AnimationAnimator
    {
        public string name;
        public Animator anim;
        public AnimationParameterType parameterType;
        public bool boolValue = true;
    }

    [System.Serializable]
    public class AnimationFeedback : Feedback
    {
        [Header("Animation Settings")]
        public AnimationAnimator[] animations;
    }

    #endregion

    #region Shake

    public enum ShakeTarget
    {
        Camera,
        GameObjects,
        Both
    }

    [System.Serializable]
    public class ShakeTransform
    {
        public ShakeTransform(bool enabled)
        {
            this.enabled = enabled;
        }

        [Header("Enable")]
        public bool enabled = false;

        [Header("Axes")]
        [Range(0f, 1f)] public float x = 1;
        [Range(0f, 1f)] public float y = 1;
        [Range(0f, 1f)] public float z = 1;
    }

    [System.Serializable]
    public class ShakeFeedback : Feedback
    {
        [Header("Shake")]
        public ShakeTarget target = ShakeTarget.Camera;
        public GameObject[] shakedObjects;
        public Space space = Space.Self;

        [Header("Shake Transform")]
        public ShakeTransform position = new ShakeTransform(true);
        public ShakeTransform rotation = new ShakeTransform(false);
        public ShakeTransform scale = new ShakeTransform(false);

        [Header("Settings")]
        public AnimationCurve curve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 0) });
        [Range(0f, 20f)] public float speed = 1f;
        [Range(0f, 5f)] public float intensity = 2f;
        [Range(0f, 0.05f)] public float interpolation = 0f;
    }

    #endregion

    #region Zoom

    [System.Serializable]
    public class ZoomFeedback : Feedback
    {
        [Header("Zoom Settings")]
        public AnimationCurve curve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(0.8f, -0.1f), new Keyframe(1f, 0f) });
        [Range(0f, 20f)] public float speed = 1f;
        [Range(0f, 5f)] public float multiplier = 0.5f;
    }

    #endregion

    #region Post Process

    [System.Serializable]
    public class PostProcessFeedback : Feedback
    {
        [Header("Post Process Settings")]
        public PostProcessVolume volume;
        public AnimationCurve curve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f) });
        [Range(0f, 20f)] public float speed = 10f;
    }

    #endregion

    #region Material

    [System.Serializable]
    public class MaterialPropertyFeedback
    {
        [Header("Property")]
        public string name = "_Emissive";
        public bool shared = false;
        public AnimationCurve curve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f) });
        public Gradient gradient = new Gradient();

        [Header("Renderers")]
        public Renderer[] renderers;

        [Header("Values")]
        [Range(0.001f, 20f)] public float speed = 2f;
        [Range(0f, 50f)] public float minValue = 0f;
        [Range(0f, 50f)] public float maxValue = 1f;
    }

    [System.Serializable]
    public class MaterialFeedback : Feedback
    {
        [Header("Material Settings")]
        public MaterialPropertyFeedback[] materialProperties;
    }

    #endregion

    #region Bounce

    [System.Serializable]
    public class BounceFeedback : Feedback
    {
        [Header("Bounce Settings")]
        public float speed = 2f;
        public GameObject[] bouncedObjects;

        [Header("Amplitude")]
        public AnimationCurve amplitudeCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(1f, 0f) });
        [Range(0f, 0.3f)] public float maxAmplitude = 0.15f;

        [Header("Looseness")]
        public AnimationCurve loosenessCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) });
        [Range(0.001f, 0.02f)] public float minLooseness = 0.01f;
        [Range(0.001f, 0.3f)] public float maxLooseness = 0.1f;
    }

    #endregion

    #region Freeze Frame

    [System.Serializable]
    public class FreezeFrameFeedback : Feedback
    {
        [Header("Freeze Frame Settings")]
        public AnimationCurve curve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(0.5f, 0f), new Keyframe(1f, 1f) });
        [Range(0f, 20f)] public float speed = 1f;
        [Range(0f, 5f)] public float minTimeScale = 0.5f;
        [Range(0f, 5f)] public float maxTimeScale = 1f;
    }

    #endregion

    /// <summary>
    /// Allows to setup and play Feedbacks easily.
    /// </summary>
    public class QuikFeedback : MonoBehaviour
    {
        public string feedbackName = "NewFeedback";
        public float generalDelay = 0f;        

        public bool useShake = false;
        public ShakeFeedback shakeFeedback;

        public bool useZoom = false;
        public ZoomFeedback zoomFeedback;

        public bool usePostProcess = false;
        public PostProcessFeedback postProcessFeedback;

        public bool useParticleSystem = false;
        public ParticleFeedback particleFeedback;

        public bool useAnimation = false;
        public AnimationFeedback animationFeedback;

        public bool useMaterial = false;
        public MaterialFeedback materialFeedback;

        public bool useBounce = false;
        public BounceFeedback bounceFeedback;

        public bool useFreezeFrame = false;
        public FreezeFrameFeedback freezeFrameFeedback;

        [Header("Debug")]
        public bool enableDebug = false;
        public KeyCode debugKey = KeyCode.N;

        private void Update()
        {
            if (enableDebug)
            {
                DebugUpdate();
            }
        }

        /// <summary>
        /// Play the feedback.
        /// </summary>
        public void Play()
        {
            StartCoroutine(PlayingFeedback());
        }

        private IEnumerator PlayingFeedback()
        {
            yield return new WaitForSeconds(generalDelay);

            if (useParticleSystem)
            {
                StartCoroutine(PlayingParticle());
            }

            if (useAnimation)
            {
                StartCoroutine(PlayingAnimations());
            }

            if (useShake)
            {
                StartCoroutine(PlayingShake());
            }

            if(useZoom)
            {
                StartCoroutine(PlayingZoom());
            }

            if(usePostProcess)
            {
                StartCoroutine(PlayingPostProcess());
            }

            if(useMaterial)
            {
                StartCoroutine(PlayingMaterial());
            }

            if(useBounce)
            {
                StartCoroutine(PlayingBounce());
            }

            if (useFreezeFrame)
            {
                StartCoroutine(PlayingFreezeFrame());
            }
        }

        private IEnumerator PlayingParticle()
        {
            yield return new WaitForSeconds(particleFeedback.delay);

            for (int i = 0; i < particleFeedback.particleSystems.Length; i++)
            {
                particleFeedback.particleSystems[i].Play();
            }
        }

        private IEnumerator PlayingAnimations()
        {
            yield return new WaitForSeconds(animationFeedback.delay);

            for (int i = 0; i < animationFeedback.animations.Length; i++)
            {
                AnimationAnimator anim = animationFeedback.animations[i];
                string param = anim.name;

                if (anim.parameterType == AnimationParameterType.Bool)
                {
                    animationFeedback.animations[i].anim.SetBool(param, anim.boolValue);
                }

                else
                {
                    animationFeedback.animations[i].anim.SetTrigger(param);
                }
            }
        }

        private IEnumerator PlayingShake()
        {
            yield return new WaitForSeconds(shakeFeedback.delay);
            QuikFeedbackManager.instance.Shake(shakeFeedback);
        }

        private IEnumerator PlayingZoom()
        {
            yield return new WaitForSeconds(zoomFeedback.delay);
            QuikFeedbackManager.instance.Zoom(zoomFeedback);
        }

        private IEnumerator PlayingPostProcess()
        {
            yield return new WaitForSeconds(postProcessFeedback.delay);
            QuikFeedbackManager.instance.PostProcess(postProcessFeedback);
        }

        private IEnumerator PlayingMaterial()
        {
            yield return new WaitForSeconds(materialFeedback.delay);
            QuikFeedbackManager.instance.PlayMaterial(materialFeedback);
        }

        private IEnumerator PlayingBounce()
        {
            yield return new WaitForSeconds(bounceFeedback.delay);
            QuikFeedbackManager.instance.Bounce(bounceFeedback);
        }

        private IEnumerator PlayingFreezeFrame()
        {
            yield return new WaitForSeconds(freezeFrameFeedback.delay);
            QuikFeedbackManager.instance.FreezeFrame(freezeFrameFeedback);
        }

        private void DebugUpdate()
        {
            if (Input.GetKeyDown(debugKey))
            {
                Play();
            }
        }

        private void OnDrawGizmos()
        {
            GizmosGameObject();
            CheckForPP();
            CheckForMaterial();
        }

        private void GizmosGameObject()
        {
            string name = "Feedback_" + feedbackName;
            if (gameObject.name != name) gameObject.name = name;
            transform.localPosition = Vector3.zero;
        }

        private void CheckForPP()
        {
            if(usePostProcess && GetComponent<PostProcessVolume>() == null && postProcessFeedback.volume == null)
            {
                PostProcessVolume vol = gameObject.AddComponent<PostProcessVolume>();
                postProcessFeedback.volume = vol;
                vol.isGlobal = true;
                vol.weight = 0f;
                vol.priority = 1f;
                gameObject.layer = QuikFeedbackManager.instance.layerPostProcess;
            }

            PostProcessVolume getComp = GetComponent<PostProcessVolume>();
            if(!usePostProcess && getComp != null)
            {
                DestroyImmediate(getComp);
                postProcessFeedback.volume = null;
            }
        }

        private void CheckForMaterial()
        {
            if(useMaterial && materialFeedback.materialProperties.Length == 0)
            {
                materialFeedback.materialProperties = new MaterialPropertyFeedback[1];
                MaterialPropertyFeedback m = materialFeedback.materialProperties[0];
                if(m != null)
                {
                    m.curve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f) });

                    m.gradient.SetKeys
                        (
                            new GradientColorKey[] 
                            {
                                new GradientColorKey(Color.white, 0f),
                                new GradientColorKey(Color.white, 1f)
                            }, 

                            new GradientAlphaKey[] 
                            {
                                new GradientAlphaKey(1f, 0f),
                                new GradientAlphaKey(1f, 1f)
                            }
                       );
                }
            }
        }
    }

}
