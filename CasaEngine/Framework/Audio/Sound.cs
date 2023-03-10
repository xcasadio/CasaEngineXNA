using CasaEngine.Framework.Entities;
using Microsoft.Xna.Framework.Audio;

namespace CasaEngine.Framework.Audio
{
    public class Sound : Entity
    {
        private readonly SoundEffect _soundEffect;
        private SoundEffectInstance _soundEffectInstance;

        public SoundEffectInstance SoundEffectInstance
        {
            get => _soundEffectInstance;
            set => _soundEffectInstance = value;
        }

        public Sound(SoundEffect soundEffect)
        {
            if (soundEffect == null)
            {
                throw new ArgumentNullException("Sound() : SoundEffect is null");
            }

            _soundEffect = soundEffect;
        }

        public void Initialize()
        {
            _soundEffectInstance = _soundEffect.CreateInstance();
        }
    }
}
