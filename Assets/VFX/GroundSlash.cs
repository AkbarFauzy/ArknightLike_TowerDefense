using System.Collections;
using System.Collections.Generic;
using TowerDefence.Module.Characters;
using UnityEngine.VFX;
using UnityEngine;


namespace TowerDefence.Module.Ability { 
    [CreateAssetMenu]
    public class GroundSlash : Ability
    {
     /*   public override void Activate(Character character)
        {
            character.CurrentATK += (int)(character.BaseATK * Multiplier);
        }

        public override void Deactivate(Character character) {
            character.CurrentATK -= (int)(character.BaseATK * Multiplier);
            *//*vfx.Stop();*//*
        }


        public override void SpawnVFX(Character character)
        {
            GameObject Parent = character.gameObject;

            GameObject vfx_object = Instantiate(vfx_asset);
            vfx_object.transform.SetParent(Parent.transform);
            vfx_object.transform.localPosition = new Vector3(0f, 0f, 0f);
            vfx_object.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            Destroy(vfx_object, 2.0f);
        }*/

        /*    private IEnumerator SpawnSkillProjectile()
            {
               *//* current_sp = 0f;
                SPBar.SetProgressValues(current_sp);*//*
                while (true)
                {
                    GameObject _projectile = Instantiate(vfx_gameobject, new Vector3(0f,0f,0f), Quaternion.Euler(0f, 90f, 0f));
                    Vector3 start = _projectile.transform.localPosition;
                    Vector3 end = start + new Vector3(3f, 0f, 0f);

                    float counter = 0;

                    while (counter < 5f)
                    {
                        counter += Time.deltaTime;
                        Debug.LogError(counter);
                        _projectile.transform.position = Vector3.Lerp(_projectile.transform.position, end, Time.deltaTime * 10f);
                        yield return new WaitForSeconds(0.1f);
                    }

                    Destroy(_projectile);

                    yield return new WaitForSeconds(2.0f);
                }

            }*/

    }
}
