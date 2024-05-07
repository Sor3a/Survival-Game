using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//sword scripte
public class weaponAttack : MonoBehaviour
{
    playerStats stats;
    Transform player,c;
    Animator animator;
    [SerializeField] bool isAttacking = false;
    [SerializeField] float timeToAttack,distance;
    float timeToAttackR=0;
    [SerializeField] LayerMask enemyLayer,animalLayer;
    [SerializeField] Material[] NrmlSwordMat,GoldSwordMat,DiamondSwordMat;
    MeshRenderer Mesh;
    playerMVT mvt;
    panelsControle pContole;
    [SerializeField] ParticleSystem animalParticals;
    [SerializeField] ParticleSystem[] attackingParticles;
    string attackChosed;
    soundManager manager;
    private void Awake()
    {
        manager = FindObjectOfType<soundManager>();
        c = Camera.main.transform;
        pContole = FindObjectOfType<panelsControle>();
        mvt = FindObjectOfType<playerMVT>();
        Mesh = transform.GetChild(0).GetComponent<MeshRenderer>();
        stats = FindObjectOfType<playerStats>();
        animator = GetComponent<Animator>();
        player = stats.transform;
    }
    public void setMaterial(item Item)
    {
        if (Item.itemID == 4)
            Mesh.materials = GoldSwordMat;
        else if (Item.itemID == 0)
            Mesh.materials = NrmlSwordMat;
        else if (Item.itemID == 5)
            Mesh.materials = DiamondSwordMat;
        
    }
    void setAttackingFalse()
    {
        isAttacking = false;
        
    }
    void changeAttacking()
    {
        animator.SetBool(attackChosed, false);
        if (!isAttacking) isAttacking = true;
    }
    void attack()
    {
        timeToAttackR = timeToAttack;
        attackChosed = Random.Range(0, 2) > 0 ? "attacking" : "attacking2";
        animator.SetBool(attackChosed, true);
    }
    void playSoun()
    {
        playSound.playAudoi(1, manager);
    }
    void Update()
    {
        animator.SetBool("running", mvt.ismoving);
        if (pContole.canAnimate())
        {
            if (Input.GetMouseButtonDown(0) && timeToAttackR <= 0)
                attack();
            if (isAttacking)
            {
                Ray r = new Ray(player.position, c.forward);
                RaycastHit hit;
                if (Physics.Raycast(r, out hit, distance, enemyLayer))
                {
                    playSound.playAudoi(4, manager);
                    GameObject hitEnemy = hit.collider.gameObject;

                    enemy hitedEnemy = hitEnemy.GetComponent<enemy>();
                    attackable target = hitEnemy.GetComponent<attackable>();
                    int a = Random.Range(0, attackingParticles.Length);
                    if (target!=null) target.getHealth(stats.damage);
                    if(hitedEnemy) attackingParticles[a].startColor = hitedEnemy.enemyColor;
                    

                    playParticles(attackingParticles[a], hit.point);
                    isAttacking = false;
                }
                else if (Physics.Raycast(r, out hit, distance, animalLayer))
                {
                    hit.collider.gameObject.GetComponent<Animal>().getHealth(stats.damage);
                    playParticles(animalParticals, hit.point);
                    isAttacking = false;
                    playSound.playAudoi(3, manager);
                }
            }
        }
        if (timeToAttackR > 0)
            timeToAttackR -= Time.deltaTime;
    }
    void playParticles(ParticleSystem p,Vector3 hitPos)
    {
        p.transform.position = hitPos;
        p.Play();
    }


}
public interface attackable
{
   public void getHealth(int dmg);
}
//if (hitedEnemy)
//{
//    hitedEnemy.getHealth(stats.damage);
//    attackingParticles[a].startColor = hitedEnemy.enemyColor;
//}
//else
//{
//    villager v = hitEnemy.GetComponent<villager>();
//    if(v)
//    v.getHealth(stats.damage);
//    else
//    {
//        human h = hitEnemy.GetComponent<human>();
//        if(h)
//        h.getHealth(stats.damage);
//        else
//        {
//            humanAI ha = hitEnemy.GetComponent<humanAI>();
//            if(ha)
//            ha.getHealth(10);
//            else
//            {
//                Boss b = hitEnemy.GetComponent<Boss>();
//                if (b)
//                    b.getHealth(stats.damage);
//            }

//        }
//    }
//}