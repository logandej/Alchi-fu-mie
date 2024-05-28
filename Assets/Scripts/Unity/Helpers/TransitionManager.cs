using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private class TransitionHelper : MonoBehaviour
    {
        private List<Transition> transitions = new List<Transition>();

        private void Update()
        {
            for (int i = transitions.Count - 1; i >= 0; i--)
            {
                if (transitions[i].UpdateTransition(Time.deltaTime))
                {
                    transitions.RemoveAt(i);
                }
            }
        }

        public void StartTransition(Transition transition)
        {
            // Remove any existing transition of the same type for the same target
            transitions.RemoveAll(t => t.Target == transition.Target && t.Type == transition.Type);
            transitions.Add(transition);
        }
    }

    private static TransitionHelper helper;

    private static TransitionHelper GetHelper()
    {
        if (helper == null)
        {
            GameObject obj = new GameObject("TransitionHelper");
            helper = obj.AddComponent<TransitionHelper>();
            Object.DontDestroyOnLoad(obj);
        }
        return helper;
    }

    public static void ChangeColor(GameObject target, Color targetColor, float duration)
    {
        GetHelper().StartTransition(new ColorTransition(target, targetColor, duration));
    }

    public static void ChangeSize(GameObject target, float targetSize, float duration)
    {
        GetHelper().StartTransition(new SizeTransition(target, targetSize, duration));
    }

    public static void ChangeLocalPosition(GameObject target, Vector3 targetPosition, float duration)
    {
        GetHelper().StartTransition(new PositionTransition(target, targetPosition, duration,true));
    }
    public static void ChangePosition(GameObject target, Vector3 targetPosition, float duration)
    {
        GetHelper().StartTransition(new PositionTransition(target, targetPosition, duration,false));
    }

    private abstract class Transition
    {
        public GameObject Target { get; private set; }
        public string Type { get; private set; }
        protected float Duration { get; private set; }
        protected float ElapsedTime { get; private set; }

        public Transition(GameObject target, string type, float duration)
        {
            Target = target;
            Type = type;
            Duration = duration;
            ElapsedTime = 0f;
        }

        public bool UpdateTransition(float deltaTime)
        {
            ElapsedTime += deltaTime;
            float t = Mathf.Clamp01(ElapsedTime / Duration);
            ApplyTransition(t);
            return ElapsedTime >= Duration;
        }

        protected abstract void ApplyTransition(float t);
    }

    private class ColorTransition : Transition
    {
        private Color startColor;
        private Color targetColor;
        private SpriteRenderer spriteRenderer;

        public ColorTransition(GameObject target, Color targetColor, float duration) : base(target, "Color", duration)
        {
            spriteRenderer = target.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                startColor = spriteRenderer.color;
                this.targetColor = targetColor;
            }
        }

        protected override void ApplyTransition(float t)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.Lerp(startColor, targetColor, t);
            }
        }
    }

    private class SizeTransition : Transition
    {
        private Vector3 startScale;
        private Vector3 targetScale;
        private Transform targetTransform;

        public SizeTransition(GameObject target, float targetSize, float duration) : base(target, "Size", duration)
        {
            targetTransform = target.transform;
            startScale = targetTransform.localScale;
            targetScale = Vector3.one * targetSize;
        }

        protected override void ApplyTransition(float t)
        {
            if (targetTransform != null)
            {
                targetTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            }
        }
    }

    private class PositionTransition : Transition
    {
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private Transform targetTransform;
        private bool local;

        public PositionTransition(GameObject target, Vector3 targetPosition, float duration, bool local) : base(target, "Position", duration)
        {
            this.local = local;
            targetTransform = target.transform;
            startPosition = local ? targetTransform.localPosition : targetTransform.position;
            this.targetPosition = targetPosition;
        }

        protected override void ApplyTransition(float t)
        {
            if (targetTransform != null)
            {
                if(local)
                    targetTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
                else
                    targetTransform.position = Vector3.Lerp(startPosition, targetPosition, t);

            }
        }
    }

}
