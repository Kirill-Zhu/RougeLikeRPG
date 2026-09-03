using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


namespace BossEntity {
    public class BattleController : MonoBehaviour {

        public SkillsStrategy[] skillStrategy;
        public Action<int, float> OnAnimation;
        HeroAudioManager audioManager;
        public float animationDuration = 0;
        const string interactionTagName = "Player";
        Transform target;
        float rotationSpeed = 1;
   

        public void Initialize(Transform target) {
            this.target = target;
           
        }
        void OnEnable() {
           
            audioManager = GetComponent<HeroAudioManager>();
            //--Remove Previous Startefies
            //if (skillStrategy != null)
            //    foreach (var skill in skillStrategy)
            //        skill.Dispose();

            //--------------Create new Sctiptable Objects

            for (int i = 0; i < skillStrategy.Length; i++) {
                this.skillStrategy[i] = Instantiate(skillStrategy[i]);
            }
            foreach (var skill in skillStrategy) {
                skill.Initialize(transform, audioManager, interactionTagName);
            }  
            //Animation
            OnAnimation += UpdateAnimationDuration;
        }
        private void OnDisable() {
            //Animation
            OnAnimation -= UpdateAnimationDuration;
        }
        public void OnUpdate() {
            //Rotate to target
            Vector3 direction = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction.WithY(0));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            
            if (animationDuration > 0) {
                animationDuration -= Time.deltaTime;
                return;
            }

            foreach (var skill in skillStrategy) {
                if (animationDuration <= 0) {
                  
                    if (skill.TryUseSkill(OnAnimation)) {
                        return;
                    }
                }
            }
        }
        public void UpdateCoolDowns() {
            foreach (var skill in skillStrategy)
                skill.OnUpdate(Time.deltaTime);

        }
        void UpdateAnimationDuration(int _, float animationDuration) {
            this.animationDuration = animationDuration;
        }
        public bool Evaluate(float distanceToHero) {
            foreach (var skill in skillStrategy) {
                if (skill.Evauate(distanceToHero)) {
                    return true;
                }
            }
            return false;
        }
    }
}