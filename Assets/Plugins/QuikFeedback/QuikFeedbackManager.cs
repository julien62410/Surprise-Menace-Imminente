using System.Collections.Generic;
using UnityEngine;

namespace Pataya.QuikFeedback
{
    /// <summary>
    /// Manages all the feedbacks.
    /// </summary>
    public class QuikFeedbackManager : MonoBehaviour
    {
        public static QuikFeedbackManager instance;

        [Header("Shake")]
        public GameObject camShaker;
        private List<Shaker> shakers = new List<Shaker>();

        [Header("Zoom")]
        private Camera cam;
        private Zoomer zoomer;
        private FreezeFramer freezeFramer;

        [Header("Post Process")]
        public int layerPostProcess = 8;
        private List<PostProcesser> postProcessers = new List<PostProcesser>();

        private List<Bouncer> bouncers = new List<Bouncer>();

        private bool noCamShaker = true;
        private bool noCamera = true;

        public List<Shaker> Shakers { get => shakers; set => shakers = value; }
        public List<PostProcesser> PostProcessers { get => postProcessers; set => postProcessers = value; }
        public List<Bouncer> Bouncers { get => bouncers; set => bouncers = value; }

        private void Awake()
        {
            instance = this;
        }

        private void OnDrawGizmos()
        {
            if(instance == null)
            {
                instance = this;
            }

            CheckForCamShaker();
            CheckForComponents();
            CheckForCam();
        }

        private void Update()
        {
            if (instance == null)
            {
                instance = this;
            }

            CheckForCamShaker();
            CheckForComponents();
            CheckForCam();
        }

        private void CheckForCamShaker()
        {
            if(!camShaker)
            {
                camShaker = GameObject.Find("CamShaker");
                if (!camShaker)
                {
                    camShaker = GameObject.Find("CameraShaker");
                    if (!camShaker)
                    {
                        camShaker = GameObject.Find("CameraShake");
                        if (!camShaker)
                        {
                            camShaker = GameObject.Find("CamShake");
                        }
                    }
                }
            }
        }

        private void CheckForComponents()
        {
            if(zoomer == null)
            {
                Zoomer z = GetComponent<Zoomer>();
                if (z != null) zoomer = z;
                else
                {
                    Zoomer newZ = gameObject.AddComponent<Zoomer>();
                    zoomer = newZ;
                }
            }

            if(!freezeFramer)
            {
                FreezeFramer f = GetComponent<FreezeFramer>();
                if (f != null) freezeFramer = f;
                else
                {
                    FreezeFramer newF = gameObject.AddComponent<FreezeFramer>();
                    freezeFramer = newF;
                }
            }
        }

        private void CheckForCam()
        {
            if(cam == null)
            {
                Camera _cam = FindObjectOfType<Camera>();
                if (_cam != null)
                {
                    cam = _cam;
                    noCamera = false;
                }

                else
                {
                    noCamera = true;
                }
            }

            else
            {
                noCamera = false;
            }
        }

        public void Shake(ShakeFeedback shake)
        {
            for (int i = 0; i < shakers.Count; i++)
            {
                if (shakers[i].s == shake || (shakers[i].s.target != ShakeTarget.GameObjects && shake.target != ShakeTarget.GameObjects))
                {
                    shakers[i].EndShake();
                }
            }

            Shaker newShaker = gameObject.AddComponent<Shaker>();
            shakers.Add(newShaker);
            newShaker.Shake(shake, camShaker);
        }

        public void PostProcess(PostProcessFeedback _pp)
        {
            for (int i = 0; i < postProcessers.Count; i++)
            {
                if(postProcessers[i].pp == _pp)
                {
                    postProcessers[i].EndPP();
                }
            }

            PostProcesser newPP = gameObject.AddComponent<PostProcesser>();
            postProcessers.Add(newPP);
            newPP.PlayPP(_pp);
        }

        public void Zoom(ZoomFeedback zoom)
        {
            if(zoomer.IsZooming)
            {
                zoomer.EndZoom();
            }

            zoomer.Zoom(zoom, cam);
        }

        public void PlayMaterial(MaterialFeedback mat)
        {
            for (int i = 0; i < mat.materialProperties.Length; i++)
            {
                MaterialPlayer newMat = gameObject.AddComponent<MaterialPlayer>();
                newMat.PlayMaterial(mat.materialProperties[i]);
            }
        }

        public void Bounce(BounceFeedback bounce)
        {
            for (int i = 0; i < bouncers.Count; i++)
            {
                if (bouncers[i].b == bounce)
                {
                    bouncers[i].EndBounce();
                }
            }

            Bouncer newBouncer = gameObject.AddComponent<Bouncer>();
            bouncers.Add(newBouncer);
            newBouncer.Bounce(bounce);
        }

        public void FreezeFrame(FreezeFrameFeedback freeze)
        {
            freezeFramer.FreezeFrame(freeze);
        }

        [ContextMenu("Release Camera Shaker")]
        private void ReleaseCameraShaker()
        {
            if(camShaker != null)
            {
                camShaker = null;
            }
        }    
    }

    /// <summary>
    /// Manages the shake of a List of objects. Instantiated by the feedback manager to allow for multiple shakers.
    /// </summary>
    public class Shaker : MonoBehaviour
    {
        [Header("Shake")]
        public ShakeFeedback s;
        private bool isShaking = false;
        private GameObject camHolder;
        public List<GameObject> shakedObjects = new List<GameObject>();

        private List<Vector3> initialPos = new List<Vector3>();
        private List<Vector3> targetPos = new List<Vector3>();

        private List<Vector3> initialRot = new List<Vector3>();
        private List<Vector3> targetRot = new List<Vector3>();

        private Vector3 currentVelPos;
        private Vector3 currentVelRot;

        private float distanceThreshold = 0.5f;
        private float shakeProgress = 0f;

        private void Update()
        {
            ShakeUpdate();
        }

        public void Shake(ShakeFeedback shake, GameObject _camHolder)
        {
            s = shake;
            camHolder = _camHolder;
            shakedObjects.Clear();

            initialPos.Clear();
            targetPos.Clear();

            initialRot.Clear();
            targetRot.Clear();

            switch (shake.target)
            {
                case ShakeTarget.Both:
                    AddObjectsToList(shake.shakedObjects);
                    AddCameraToList();
                    break;

                case ShakeTarget.Camera:
                    AddCameraToList();
                    break;

                case ShakeTarget.GameObjects:
                    AddObjectsToList(shake.shakedObjects);
                    break;
            }

            shakeProgress = 0f;
            isShaking = true;

            if (shakedObjects.Count > 0)
            {
                for (int i = 0; i < shakedObjects.Count; i++)
                {
                    if (shakedObjects[i] != null)
                    {
                        if (s.position.enabled)
                        {
                            if (s.space == Space.Self)
                            {
                                initialPos.Add(shakedObjects[i].transform.localPosition);
                            }

                            else
                            {
                                initialPos.Add(shakedObjects[i].transform.position);
                            }

                            targetPos.Add(Vector3.zero);

                        }

                        if (s.rotation.enabled)
                        {
                            if (s.space == Space.Self)
                            {
                                initialRot.Add(shakedObjects[i].transform.localEulerAngles);
                            }

                            else
                            {
                                initialRot.Add(shakedObjects[i].transform.eulerAngles);
                            }

                            targetRot.Add(Vector3.zero);
                        }

                        CalculateNewTargets(i);

                    }
                }
            }
        }

        private void ShakeUpdate()
        {
            if (isShaking)
            {
                if (shakeProgress <= 1f)
                {
                    shakeProgress += s.speed * Time.deltaTime;

                    if (shakedObjects.Count > 0)
                    {
                        for (int i = 0; i < shakedObjects.Count; i++)
                        {
                            if (shakedObjects[i] != null)
                            {
                                if (s.position.enabled)
                                {
                                    if (s.space == Space.Self)
                                    {
                                        shakedObjects[i].transform.localPosition = Vector3.SmoothDamp(shakedObjects[i].transform.localPosition, targetPos[i], ref currentVelPos, s.interpolation);
                                    }

                                    else
                                    {
                                        shakedObjects[i].transform.position = Vector3.SmoothDamp(shakedObjects[i].transform.position, targetPos[i], ref currentVelPos, s.interpolation);
                                    }

                                    Vector3 pos;
                                    if (s.space == Space.Self) pos = shakedObjects[i].transform.localPosition;
                                    else pos = shakedObjects[i].transform.position;

                                    if (Vector3.Distance(pos, targetPos[i]) < distanceThreshold)
                                    {
                                        CalculateNewTargets(i);
                                    }

                                }

                                if (s.rotation.enabled)
                                {
                                    if (s.space == Space.Self)
                                    {
                                        shakedObjects[i].transform.localEulerAngles = Vector3.SmoothDamp(shakedObjects[i].transform.localEulerAngles, targetRot[i], ref currentVelRot, s.interpolation);
                                    }

                                    else
                                    {
                                        shakedObjects[i].transform.eulerAngles = Vector3.SmoothDamp(shakedObjects[i].transform.eulerAngles, targetRot[i], ref currentVelRot, s.interpolation);
                                    }

                                    Vector3 rot;
                                    if (s.space == Space.Self) rot = shakedObjects[i].transform.localEulerAngles;
                                    else rot = shakedObjects[i].transform.eulerAngles;

                                    if (Vector3.Distance(rot, targetRot[i]) < distanceThreshold)
                                    {
                                        CalculateNewTargets(i);
                                    }
                                }
                            }

                        }
                    }
                }

                else
                {
                    EndShake();
                }
            }
        }

        private void CalculateNewTargets(int i)
        {
            float currentIntensity = s.curve.Evaluate(shakeProgress) * s.intensity;

            //POSITION
            if (s.position.enabled)
            {
                Vector3 rand = Random.insideUnitSphere;
                Vector3 posMul = new Vector3(s.position.x * rand.x, s.position.y * rand.y, s.position.z * rand.z);
                targetPos[i] = initialPos[i] + (posMul * currentIntensity);
            }

            //ROTATION
            if (s.rotation.enabled)
            {
                float xRot = Random.Range(-1f, 1f) * s.rotation.x;
                float yRot = Random.Range(-1f, 1f) * s.rotation.y;
                float zRot = Random.Range(-1f, 1f) * s.rotation.z;

                Vector3 addedRot = new Vector3(xRot, yRot, zRot);
                targetRot[i] = initialRot[i] + (addedRot * currentIntensity);
            }
        }

        public void EndShake()
        {
            isShaking = false;

            if (shakedObjects.Count > 0)
            {
                for (int i = 0; i < shakedObjects.Count; i++)
                {
                    if (shakedObjects[i] != null)
                    {
                        if (s.position.enabled)
                        {
                            if (s.space == Space.Self)
                            {
                                shakedObjects[i].transform.localPosition = initialPos[i];
                            }

                            else
                            {
                                shakedObjects[i].transform.position = initialPos[i];
                            }
                        }

                        if (s.rotation.enabled)
                        {
                            if (s.space == Space.Self)
                            {
                                shakedObjects[i].transform.localEulerAngles = initialRot[i];
                            }

                            else
                            {
                                shakedObjects[i].transform.eulerAngles = initialRot[i];
                            }
                        }
                    }
                }
            }

            QuikFeedbackManager.instance.Shakers.Remove(this);
            //Auto Destroy
            Destroy(this);
        }

        private void AddCameraToList()
        {
            shakedObjects.Add(camHolder);
        }

        private void AddObjectsToList(GameObject[] objs)
        {
            if (objs.Length > 0)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i] != null)
                    {
                        shakedObjects.Add(objs[i]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Manages the weight of a Post Process Volume.
    /// </summary>
    public class PostProcesser : MonoBehaviour
    {
        public PostProcessFeedback pp;
        private bool isPlaying = false;
        private float progress = 0f;

        private void Update()
        {
            PPUpdate();
        }

        public void PlayPP(PostProcessFeedback _pp)
        {
            pp = _pp;
            progress = 0f;
            isPlaying = true;
        }

        private void PPUpdate()
        {
            if(isPlaying)
            {
                if(progress <= 1f)
                {
                    progress += Time.deltaTime * pp.speed;
                    pp.volume.weight = pp.curve.Evaluate(progress);
                }

                else
                {
                    EndPP();
                }
            }
        }

        public void EndPP()
        {
            isPlaying = false;
            pp.volume.weight = pp.curve.Evaluate(1f);

            QuikFeedbackManager.instance.PostProcessers.Remove(this);
            Destroy(this);
        }
    }

    /// <summary>
    /// Manages the animation of an array of material Properties.
    /// </summary>
    public class MaterialPlayer : MonoBehaviour
    {
        public MaterialPropertyFeedback m;
        private bool isPlaying = false;
        private float progress = 0f;

        private void Update()
        {
            MaterialUpdate();
        }

        public void PlayMaterial(MaterialPropertyFeedback _mat)
        {
            m = _mat;
            progress = 0f;
            isPlaying = true;
        }

        private void MaterialUpdate()
        {
            if(isPlaying)
            {
                if(progress <= 1f)
                {
                    progress += m.speed * Time.deltaTime;
                    ChangeMaterial(progress);
                }

                else
                {
                    EndMaterial();
                }
            }
        }

        public void EndMaterial()
        {
            isPlaying = false;
            ChangeMaterial(1f);

            Destroy(this);
        }

        private void ChangeMaterial(float _prog)
        {
            for (int i = 0; i < m.renderers.Length; i++)
            {
                float fValue = Mathf.Lerp(m.minValue, m.maxValue, m.curve.Evaluate(_prog));
                Color cValue = m.gradient.Evaluate(progress);

                if (m.shared)
                {
                    m.renderers[i].sharedMaterial.SetFloat(m.name, fValue);
                    m.renderers[i].sharedMaterial.SetColor(m.name, cValue);
                }

                else
                {
                    m.renderers[i].material.SetFloat(m.name, fValue);
                    m.renderers[i].material.SetColor(m.name, cValue);
                }
            }
        }
    }

    public class Bouncer : MonoBehaviour
    {
        public BounceFeedback b;
        private bool isPlaying = false;
        private float progress = 0f;
        private List<Vector3> initialScales = new List<Vector3>();
        private List<Vector3> targetScales = new List<Vector3>();
        private List<Vector3> currentVels = new List<Vector3>();
        private float currentSmooth;
        private bool hasEnded = false;

        private void Update()
        {
            BounceUpdate();   
        }

        public void Bounce(BounceFeedback _b)
        {
            b = _b;
            initialScales.Clear();
            targetScales.Clear();
            currentVels.Clear();

            for (int i = 0; i < b.bouncedObjects.Length; i++)
            {
                initialScales.Add(b.bouncedObjects[i].transform.localScale);
                targetScales.Add(Vector3.zero);
                currentVels.Add(Vector3.zero);
                CalculateNewTargets(i);
            }

            progress = 0f;
            isPlaying = true;
        }

        private void BounceUpdate()
        {
            if (isPlaying)
            {
                if (progress <= 1f)
                {
                    progress += b.speed * Time.deltaTime;
                    for (int i = 0; i < b.bouncedObjects.Length; i++)
                    {
                        Vector3 refer = currentVels[i];
                        Vector3 scale = Vector3.SmoothDamp(b.bouncedObjects[i].transform.localScale, initialScales[i] + targetScales[i], ref refer, currentSmooth);
                        b.bouncedObjects[i].transform.localScale = scale;

                        if (Vector3.Distance(scale, initialScales[i] + targetScales[i]) < 0.01f)
                        {
                            CalculateNewTargets(i);
                        }
                    }
                }

                else
                {
                    RequestEnd();
                }
            }

            if(hasEnded)
            {
                bool canEnd = true;

                for (int i = 0; i < b.bouncedObjects.Length; i++)
                {
                    Vector3 refer = currentVels[i];
                    Vector3 scale = Vector3.SmoothDamp(b.bouncedObjects[i].transform.localScale, initialScales[i], ref refer, currentSmooth);
                    b.bouncedObjects[i].transform.localScale = scale;

                    if (!(Vector3.Distance(scale, initialScales[i]) < 0.001f))
                    {
                        canEnd = false;
                    }
                }

                if(canEnd)
                {
                    EndBounce();
                }

            }
        }

        private void CalculateNewTargets(int i)
        {
            float scale = OppositeInt(b.bouncedObjects[i].transform.localScale.x, i) * b.amplitudeCurve.Evaluate(progress) * b.maxAmplitude;
            targetScales[i] = new Vector3(scale, scale, scale);
            currentSmooth = Mathf.Lerp(b.minLooseness, b.maxLooseness, b.loosenessCurve.Evaluate(progress));
        }

        private void RequestEnd()
        {
            hasEnded = true;
        }

        public void EndBounce()
        {
            isPlaying = false;
            for (int i = 0; i < b.bouncedObjects.Length; i++)
            {
                b.bouncedObjects[i].transform.localScale = initialScales[i];
            }

            QuikFeedbackManager.instance.Bouncers.Remove(this);
            Destroy(this);
        }

        private int OppositeInt(float input, int i)
        {
            if(input < initialScales[i].x)
            {
                return 1;
            }

            else
            {
                return -1;
            }
        }

    }
}

