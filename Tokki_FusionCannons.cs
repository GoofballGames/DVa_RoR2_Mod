using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace GoofballGames.TokkiEntityStates
{
    public class Tokki_FusionCannons : BaseSkillState
    {
        public float baseDuration = 0.15f;
        public float missleDuration = 0.139f;
        public float speedOverride = 20f;
        public float speedMod = 0.5f;
        private float duration;
        private bool isFiring = false;
        private int missleCount = 0;
        private int maxMissles = 18;
        private float lastFired;
        public GameObject effectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark");
        public GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/critspark");
        public GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/tracerbanditshotgun");
        public GameObject miniMisslePrefab = Resources.Load<GameObject>("prefabs/projectiles/MicroMissileProjectile");
        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority && !isFiring)
            {
                //Micro Missles for Critical Hits!
                if (base.RollCrit())
                {
                    isFiring = true;
                }

                //Regular Fusion Cannons for Non-Critical Hits.
                else
                {
                    this.duration = this.baseDuration / base.attackSpeedStat;
                    Ray aimRay = base.GetAimRay();
                    base.StartAimMode(aimRay, 2f, false);
                    base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration * 1.1f);
                    new BulletAttack
                    {
                        owner = base.gameObject,
                        weapon = base.gameObject,
                        origin = aimRay.origin,
                        aimVector = aimRay.direction,
                        minSpread = 0,
                        maxSpread = 4.15f,
                        bulletCount = 11U,
                        procCoefficient = 0.091f,
                        damage = base.characterBody.damage * 0.4f,
                        force = 1.5f,
                        falloffModel = BulletAttack.FalloffModel.Buckshot,
                        tracerEffectPrefab = this.tracerEffectPrefab,
                        hitEffectPrefab = this.hitEffectPrefab,
                        isCrit = false,
                        HitEffectNormal = false,
                        stopperMask = LayerIndex.world.mask,
                        smartCollision = true,
                        maxDistance = 100f
                    }.Fire();
                }
            }
        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            bool flag = base.fixedAge >= this.missleDuration && Time.time > this.lastFired;
            if (flag && isFiring && missleCount < maxMissles)
            {
                missleCount++;
                MicroMissles();
            }
            else if (missleCount >= maxMissles)
            {
                isFiring = false;
                missleCount = 0;
            }

            if (base.fixedAge >= this.duration && base.isAuthority && !isFiring)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public void MicroMissles()
        {
            this.duration = this.missleDuration / base.attackSpeedStat;
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            base.PlayAnimation("Gesture, Override", "FireEmpoweredBolt", "FireBolt.playbackRate", this.duration * 2f);
            this.lastFired = Time.time + this.missleDuration / this.attackSpeedStat;
            //Always a crit, so always divide by 2 to set the literal damage amount.
            ProjectileManager.instance.FireProjectile(miniMisslePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject,
                base.characterBody.damage * 7 / 2, 3, true, DamageColorIndex.Default, null, speedOverride + (speedMod * base.attackSpeedStat));
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}