using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using System.Threading;

namespace Game.CoreSystem
{
    public class SpriteShaderController : CoreComponent
    {
        [Title("Initialization")]
        [SerializeField] private Transform _targetRoot;
        [Tooltip("Used to exclude SpriteRenderers with filter (Lowercases everything)")]
        [SerializeField] private string[] _forbiddenSubSpriteNames;

        [Title("Flash Settings")]
        [SerializeField] private bool _useFlashAnimationCurve;
        [SerializeField, ShowIf("_useFlashAnimationCurve")] private AnimationCurve flashCurve;

        [Title("Dissolve Settings")]
        [SerializeField] private bool _useDissolveAnimationCurve;
        [SerializeField, ShowIf("_useDissolveAnimationCurve")] private AnimationCurve dissolveCurve;

        private Material[] _materials;
        private CancellationTokenSource _flashCTS;

        private readonly int _flashAmount = Shader.PropertyToID("_FlashAmount");
        private readonly int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");

        protected override void Awake()
        {
            base.Awake();

            FindMaterials();
        }

        public void ResetEffects()
        {
            SetFloatProperty(_flashAmount, 0f);
            SetFloatProperty(_dissolveAmount, 0f);
        }

        public void PlayFlash(float duration)
        {
            _flashCTS?.Cancel();
            Flash(duration).Forget();
        }

        public void PlayDissolve(float duration)
        {
            Dissolve(duration).Forget();
        }

        private async UniTaskVoid Flash(float flashDuration)
        {
            _flashCTS = new CancellationTokenSource();

            float flashAmount;
            float elapsedTime = 0f;
            while (elapsedTime <= flashDuration)
            {
                elapsedTime += Time.deltaTime;

                if (_useFlashAnimationCurve)
                    flashAmount = Mathf.Lerp(1f, 0f, flashCurve.Evaluate(elapsedTime / flashDuration));
                else
                    flashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / flashDuration));

                SetFloatProperty(_flashAmount, flashAmount);

                await UniTask.Yield(_flashCTS.Token);
            }

            SetFloatProperty(_flashAmount, 0f);
        }

        private async UniTaskVoid Dissolve(float dissolveDuration)
        {
            float dissolveAmount;
            float elapsedTime = 0f;
            while (elapsedTime <= dissolveDuration)
            {
                elapsedTime += Time.deltaTime;

                if (_useDissolveAnimationCurve)
                    dissolveAmount = Mathf.Lerp(0f, 1.1f, dissolveCurve.Evaluate(elapsedTime / dissolveDuration));
                else
                    dissolveAmount = Mathf.Lerp(0f, 1.1f, (elapsedTime / dissolveDuration));

                SetFloatProperty(_dissolveAmount, dissolveAmount);

                await UniTask.Yield();
            }

            SetFloatProperty(_dissolveAmount, 1.1f);
        }

        private void SetFloatProperty(int propertyId, float amount)
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                if (_materials[i] != null)
                {
                    _materials[i].SetFloat(propertyId, amount);
                }
            }
        }

        private void FindMaterials()
        {
            SpriteRenderer[] spriteRenderers = _targetRoot.GetComponentsInChildren<SpriteRenderer>(true);

            _materials = spriteRenderers.Where(s => IsAllowedName(s.name)).Select(s => s.material).Where(m => m.HasFloat(_flashAmount) && m.HasFloat(_dissolveAmount)).ToArray();
        }

        private bool IsAllowedName(string name)
        {
            for (int j = 0; j < _forbiddenSubSpriteNames.Length; j++)
            {
                if (name.ToLower() == _forbiddenSubSpriteNames[j].ToLower())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
