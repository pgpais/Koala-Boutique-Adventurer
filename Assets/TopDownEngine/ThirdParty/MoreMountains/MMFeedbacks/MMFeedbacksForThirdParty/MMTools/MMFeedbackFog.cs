using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.Feedbacks
{
    /// <summary>
    /// This feedback will let you animate the density, color, end and start distance of your scene's fog
    /// </summary>
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback will let you animate the density, color, end and start distance of your scene's fog")]
    [FeedbackPath("Renderer/Fog")]
    public class MMFeedbackFog : MMFeedback
    {
        /// sets the inspector color for this feedback
#if UNITY_EDITOR
        public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.RendererColor; } }
#endif

        /// the possible modes for this feedback
        public enum Modes { OverTime, Instant }

        [Header("Fog")]
        /// whether the feedback should affect the sprite renderer instantly or over a period of time
        public Modes Mode = Modes.OverTime;
        /// how long the sprite renderer should change over time
        [MMFEnumCondition("Mode", (int)Modes.OverTime)]
        public float Duration = 0.2f;

        [Header("Fog Density")] 
        /// whether or not to modify the fog's density
        public bool ModifyFogDensity = true;
        /// a curve to use to animate the fog's density over time
        public MMTweenType DensityCurve = new MMTweenType(new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.3f, 1f), new Keyframe(1, 0)));
        /// the value to remap the fog's density curve zero value to
        public float DensityRemapZero = 0.01f;
        /// the value to remap the fog's density curve one value to
        public float DensityRemapOne = 0.05f;
        /// the value to change the fog's density to when in instant mode
        public float DensityInstantChange;
        
        [Header("Fog Start Distance")] 
        /// whether or not to modify the fog's start distance
        public bool ModifyStartDistance = true;
        /// a curve to use to animate the fog's start distance over time
        public MMTweenType StartDistanceCurve = new MMTweenType(new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.3f, 1f), new Keyframe(1, 0)));
        /// the value to remap the fog's start distance curve zero value to
        public float StartDistanceRemapZero = 0f;
        /// the value to remap the fog's start distance curve one value to
        public float StartDistanceRemapOne = 0f;
        /// the value to change the fog's start distance to when in instant mode
        public float StartDistanceInstantChange;
        
        [Header("Fog End Distance")] 
        /// whether or not to modify the fog's end distance
        public bool ModifyEndDistance = true;
        /// a curve to use to animate the fog's end distance over time
        public MMTweenType EndDistanceCurve = new MMTweenType(new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.3f, 1f), new Keyframe(1, 0)));
        /// the value to remap the fog's end distance curve zero value to
        public float EndDistanceRemapZero = 0f;
        /// the value to remap the fog's end distance curve one value to
        public float EndDistanceRemapOne = 300f;
        /// the value to change the fog's end distance to when in instant mode
        public float EndDistanceInstantChange;
        
        [Header("Fog Color")]
        /// whether or not to modify the fog's color
        public bool ModifyColor = true;
        /// the colors to apply to the sprite renderer over time
        [MMFEnumCondition("Mode", (int)Modes.OverTime)]
        public Gradient ColorOverTime;
        /// the color to move to in instant mode
        [MMFEnumCondition("Mode", (int)Modes.Instant)]
        public Color InstantColor;

        /// the duration of this feedback is the duration of the sprite renderer, or 0 if instant
        public override float FeedbackDuration { get { return (Mode == Modes.Instant) ? 0f : Duration; } }

        /// <summary>
        /// On Play we change the values of our fog
        /// </summary>
        /// <param name="position"></param>
        /// <param name="attenuation"></param>
        protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1.0f)
        {
            if (Active)
            {
                switch (Mode)
                {
                    case Modes.Instant:
                        if (ModifyColor)
                        {
                            RenderSettings.fogColor = InstantColor;
                        }

                        if (ModifyStartDistance)
                        {
                            RenderSettings.fogStartDistance = StartDistanceInstantChange;
                        }

                        if (ModifyEndDistance)
                        {
                            RenderSettings.fogEndDistance = EndDistanceInstantChange;
                        }

                        if (ModifyFogDensity)
                        {
                            RenderSettings.fogDensity = DensityInstantChange;
                        }
                        break;
                    case Modes.OverTime:
                        StartCoroutine(SpriteRendererSequence());
                        break;
                }
            }
        }

        /// <summary>
        /// This coroutine will modify the values on the fog settings
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator SpriteRendererSequence()
        {
            float journey = 0f;
            while (journey < Duration)
            {
                float remappedTime = MMFeedbacksHelpers.Remap(journey, 0f, Duration, 0f, 1f);

                SetFogValues(remappedTime);

                journey += FeedbackDeltaTime;
                yield return null;
            }
            SetFogValues(1f);          
            yield return null;
        }

        /// <summary>
        /// Sets the various values on the fog on a specified time (between 0 and 1)
        /// </summary>
        /// <param name="time"></param>
        protected virtual void SetFogValues(float time)
        {
            if (ModifyColor)
            {
                RenderSettings.fogColor = ColorOverTime.Evaluate(time); 
            }

            if (ModifyFogDensity)
            {
                RenderSettings.fogDensity = MMTween.Tween(time, 0f, 1f, DensityRemapZero, DensityRemapOne, DensityCurve);
            }

            if (ModifyStartDistance)
            {
                RenderSettings.fogStartDistance = MMTween.Tween(time, 0f, 1f, StartDistanceRemapZero, StartDistanceRemapOne, StartDistanceCurve);
            }

            if (ModifyEndDistance)
            {
                RenderSettings.fogEndDistance = MMTween.Tween(time, 0f, 1f, EndDistanceRemapZero, EndDistanceRemapOne, EndDistanceCurve);
            }
        }
    }
}
