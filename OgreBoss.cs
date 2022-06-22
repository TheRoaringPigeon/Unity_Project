using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState{
    walking,
    staggered,
    attacking,
}
public class OgreBoss : MonoBehaviour
{
    [Header("State")]
    public BossState currentState;
    [Header("Stats")]
    public FloatValue myHealth, swingCoolDown;
    public float currentHealth;

    [Header("For Enemy Door")]
    public int myID;
    public MySignal deathSignal;

    [Header("DamageAffect")]
    public GameObject daPrefab;
    public GameObject telegraph;
    private SpriteRenderer telegraphSprite;
    private List<GameObject> daList;
    public GameObject singleAffect;
    private int ready = 0;

    [Header("IFrames")]
    public Color flashColor;
    public Color defaultColor;
    public float flashDuration;
    public Collider2D myCollider;
    private SpriteRenderer mySprite;

    private Rigidbody2D myRigidbody;
    private Animator anim;
    private Transform target;
    private float lastSwing;
    private float spawned;
    // Start is called before the first frame update
    void Start()
    {
        ChangeState("walking", "Start");
        currentHealth = myHealth.initialValue;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        spawned = Time.time;
        telegraphSprite = telegraph.GetComponent<SpriteRenderer>();
        mySprite = GetComponent<SpriteRenderer>();
        ResetTelegraph();
        daList = new List<GameObject>();
        for (int i = 0; i < 3; i++){
            GameObject obj = Instantiate(daPrefab);
            obj.SetActive(false);
            daList.Add(obj);
        }
    }

    private GameObject GetDA(){
        for (int i = 0; i < daList.Count; i++){
            if (!daList[i].activeInHierarchy){
                return daList[i];
            }
        }
        GameObject obj = Instantiate(daPrefab);
        daList.Add(obj);
        return obj;
    }
    public void Attack(Vector3 direction){
        if (daPrefab){
            GameObject prefab = GetDA();
            prefab.transform.position = transform.position;
            prefab.SetActive(true);
            prefab.GetComponent<DamageAffect>().Setup(direction - transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - spawned < 3f){
            return;
        }
        if (Time.time - lastSwing > swingCoolDown.initialValue){
            Vector3 temp = Vector3.MoveTowards(transform.position, target.position, 1 * Time.deltaTime);
            ChangeAnim(temp - transform.position);
            if (ready < 3){
                StartCoroutine(SwingCo(temp));
                ResetCD();
            }else{
                ready = 0;
                singleAffect.transform.position = target.position;
                StartCoroutine(singleAffectCo());
                ResetCD();
            }
        }
    }
    public bool GetStaggerStatus(){
        return currentState == BossState.staggered;
    }
    public void TakeDamage(float damage){
        ChangeState("staggered", "TakeDamage");
        currentHealth -= damage;
        if (currentHealth <= 0){
            gameObject.SetActive(false);
            deathSignal.Raise(myID);
        }else{
            StartCoroutine(IFramesCo());
        }
    }
    private IEnumerator IFramesCo(){
        int temp = 0;
        myCollider.enabled = false;
        while (temp < 5){
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = defaultColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        myCollider.enabled = true;
    }
    private IEnumerator singleAffectCo(){
        ChangeState("attacking", "singleAffStart");
        telegraph.transform.position = target.position;
        telegraph.transform.localScale = singleAffect.transform.localScale;
        Color tmp = telegraphSprite.color;
        tmp.a = 255f;
        telegraphSprite.color = tmp;
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("attack");
        singleAffect.SetActive(true);
        ResetTelegraph();
        ChangeState("walking", "singleAffEnd");
    }
    private void ResetCD(){
        lastSwing = Time.time;
    }
    private void ResetTelegraph(){
        Color tmp = telegraphSprite.color;
        tmp.a = 0f;
        telegraphSprite.color = tmp;
    }
    private void TelegraphSwing(bool _mirrorZ) {
        Vector3 centerPos = (transform.position + target.position) / 2f;
        telegraph.transform.position = centerPos;
        Vector3 direction = target.position - transform.position;
        direction = Vector3.Normalize(direction);
        telegraph.transform.right = direction;
        if (_mirrorZ) telegraph.transform.right *= -1f;
        Vector3 scale = new Vector3(1,1,1);
        scale.x = Vector3.Distance(transform.position, target.position);
        telegraph.transform.localScale = scale;
        Color tmp = telegraphSprite.color;
        tmp.a = 255f;
        telegraphSprite.color = tmp;
     }
    private IEnumerator SwingCo(Vector3 direction){
        TelegraphSwing(true);
        ChangeState("attacking", "SwingCo");
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("attack");
        Attack(direction);
        ready++;
        yield return new WaitForSeconds(1f);
        ResetTelegraph();
        ChangeState("walking", "SwingCo_AfterYield");
    }
    private void ChangeAnim(Vector2 direction){
       if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
           if (direction.x > 0){
               SetAnimFloat(Vector2.right);
           }else{
               SetAnimFloat(Vector2.left);
           }
       }else{
           if (direction.y > 0){
               SetAnimFloat(Vector2.up);
           }else{
               SetAnimFloat(Vector2.down);
           }
       }
    }
    private void SetAnimFloat(Vector2 vect){
        anim.SetFloat("moveX", vect.x);
        anim.SetFloat("moveY", vect.y);
    }
    private void ChangeState(string state, string caller){
        switch (state)
        {
            case "attacking":
                currentState = BossState.attacking;
                break;
            case "walking":
                currentState = BossState.walking;
                break;
            case "staggered":
                currentState = BossState.staggered;
                break;
            default:
                break;
        }
    }
}
