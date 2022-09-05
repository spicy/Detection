using UnityEngine;
namespace Detection
{
    public interface IShootsParticle
    {
        public void ShootAndEmitParticle(Ray ray);
    }
}