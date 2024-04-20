using UnityEngine;
using Random = UnityEngine.Random;
using MoreMountains.Tools;

namespace Game
{
    [CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
    public class SoundEffectSO : ScriptableObject
    {
        private static readonly float SEMITONES_TO_PITCH_CONVERSION_UNIT = 1.05946f;

        [SerializeField] private SoundClipPlayOrder playOrder = SoundClipPlayOrder.random;

        public AudioClip[] clips;

        [SerializeField] private Vector2 volume = new(0.5f, 0.5f);

        //Pitch / Semitones
        public bool useSemitones;


        [SerializeField] private Vector2Int semitones = new(0, 0);
        [SerializeField] private Vector2 pitch = new(1, 1);

        private int playIndex;

        public void SyncPitchAndSemitones()
        {
            if (useSemitones)
            {
                pitch.x = Mathf.Pow(SEMITONES_TO_PITCH_CONVERSION_UNIT, semitones.x);
                pitch.y = Mathf.Pow(SEMITONES_TO_PITCH_CONVERSION_UNIT, semitones.y);
            }
            else
            {
                semitones.x = Mathf.RoundToInt(Mathf.Log10(pitch.x) / Mathf.Log10(SEMITONES_TO_PITCH_CONVERSION_UNIT));
                semitones.y = Mathf.RoundToInt(Mathf.Log10(pitch.y) / Mathf.Log10(SEMITONES_TO_PITCH_CONVERSION_UNIT));
            }
        }

        private AudioClip GetAudioClip()
        {
            // get current clip
            var clip = clips[playIndex >= clips.Length ? 0 : playIndex];

            // find next clip
            switch (playOrder)
            {
                case SoundClipPlayOrder.in_order:
                    playIndex = (playIndex + 1) % clips.Length;
                    break;
                case SoundClipPlayOrder.random:
                    playIndex = Random.Range(0, clips.Length);
                    break;
                case SoundClipPlayOrder.reverse:
                    playIndex = (playIndex + clips.Length - 1) % clips.Length;
                    break;
            }

            // return clip
            return clip;
        }

        public void Play(SoundClipTrack track, Transform transform)
        {
            if (clips.Length == 0)
            {
                Debug.LogWarning($"Missing sound clips for {name}");
                return;
            }

            AudioClip clip = GetAudioClip();
            MMSoundManagerPlayOptions options = MMSoundManagerPlayOptions.Default;

            options.MmSoundManagerTrack = (MMSoundManager.MMSoundManagerTracks)track;

            options.Location = transform.position;
            options.Volume = Random.Range(volume.x, volume.y);
            options.Pitch = useSemitones
                ? Mathf.Pow(SEMITONES_TO_PITCH_CONVERSION_UNIT, Random.Range(semitones.x, semitones.y))
                : Random.Range(pitch.x, pitch.y);

            MMSoundManagerSoundPlayEvent.Trigger(clip, options);
        }

        enum SoundClipPlayOrder
        {
            random,
            in_order,
            reverse
        }
        public enum SoundClipTrack
        {
            Sfx,
            Music,
            UI,
            Master,
            Other
        }

    }
}