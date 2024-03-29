using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum E_SceneState_New
{
    idle,
    pass,
    fail,
    WinLose
}

public class NewGladiator : TrainingMove
{
    /* Orge.cs */
    [Header("Orge Class")]
    private Transform testTarget;
    private int Team_State;
    private float AttackWait;
    private float AttackDelaySeconds;
    private bool canOrgeAttack;
    private bool tookDamage;
    public float chaseRadius;
    public float attackRadius;
    private const float attackRangeConst = 0.15f;

    private Animator OrgeAnim;

    public Transform[] attackPoint;
    public float attackRange = attackRangeConst;
    public LayerMask enemyLayers;

    [Header("Death Effects")]
    public GameObject deathEffect;
    private float deathEffectDelay = 1f;

    [Header("Damage Popup")]
    public GameObject hudDamageText;
    public Transform hudPos;

    //Temporary
    private bool checkWinLost = false;

    /* Click Character.cs */
    [Header("Open Character Image")]
    private bool isOpend;
    public GameObject CharacterPanel;
    public GameObject GladiatorImage;
    public IntValue Check_ASite_Scene_Gladiators;
 
    [Header("Gladiator Stat TextList")]
    public Text GladiatorStat_Name;
    public Text GladiatorStat_Lv;
    public Text GladiatorStat_Hp;
    public Text GladiatorStat_Speed;
    public Text GladiatorStat_Damage;
    public Text GladiatorStat_PassChance;
    public Text GladiatorStat_WeaponSpeed;
    public Text GladiatorStat_ProjectileSpeed;
    public Transform GladiatorPos;

    private float CheckDistance_ = 1f;
    private float UpgradeMin = 0f;
    private float UpgradeMax = 100f;
    private float UpgradeResult;
    private int MaxGladiatorLevel = 9;
    //(ex) +1,+2,+3 100%, +4 51%, +5 42%, +6 30%, +7 22%...)
    private float[] UpgradeList = { 100f, 100f, 100f, 51f, 42f, 30f, 22f, 20f, 15f, 0f };
    private string[] GladiatorNameList = { "Mark", "Trers", "Obius", "Rendolf", "Duex", "Durant", "James", "Rblon", "Rbion", "Mk333" };
    private static int InitGladiatorStat_Num = 4;
    private float[] InitGladiatorStat = new float[InitGladiatorStat_Num];
 
    [Header("Scene Animation")]
    public Animator transition;
    public float transitionTime = 0.5f;
 
    public E_SceneState_New sceneState;
    public Text PassFailText;
    private bool RenewalPosition;

    [Header("Slave Active SkillList")]
    public BoolValue[] ActiveSkillList;
    private bool Skill_1_OnOff = true;

    protected bool Skill_2_OnOff = true;
    protected bool Skill_3_OnOff = true;

    private int Skill_2_Chance;

    [Header("Temp")]
    public IntValue RePosition;
    
    private void Awake()
    {
        var objs = FindObjectsOfType<ClickCharacter>();

        if (objs.Length <= 2)
        {
            if (this.Alive_BoolValue.RuntimeValue)
            {
                DontDestroyOnLoad(this.gameObject);
                if (Check_ASite_Scene_Gladiators.RuntimeValue != 0) // another Place
                {
                    // Vector3 temp;
                    // temp.x = 30;
                    // temp.y = 0;
                    // temp.z = 0;
                    // this.gameObject.transform.position = temp;
                }
                else //Training Room
                {
                    Vector3 temp;

                    switch (RePosition.RuntimeValue)
                    {
                        case 0:
                            temp.x = 10f;
                            temp.y = -4f;
                            temp.z = 0;
                            this.transform.position = temp;
                            break;
                        case 1:
                            temp.x = 8f;
                            temp.y = 0f;
                            temp.z = 0;
                            this.transform.position = temp;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);

        }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        RenewalPosition = false;
        InitailizeSetting();
        isOpend = false;
        sceneState = E_SceneState_New.idle;
        Touch_BoolValue.RuntimeValue = true;

        moveSpeed = InitmoveSpeed.RuntimeValue;
        health = maxHealth.RuntimeValue;
        baseAttack = DamageIntValue.RuntimeValue;
        Level = Level_IntValue.RuntimeValue;
        ProjectileSpeed_base = ProjectileSpeed.RuntimeValue;
        gladiatorState = GladiatorState.idle;
        AttackSpeed = WeaponSpeed.RuntimeValue;
        OrgeAnim = GetComponent<Animator>();
        Alive_BoolValue.RuntimeValue = true;
        DodgeChance = DodgeIntValue.RuntimeValue;

        OrgeAnim.SetBool("Win", false);
        OrgeAnim.SetFloat("moveX", 0);
        OrgeAnim.SetFloat("moveY", -1);
        healthBar.SetMaxHealth(health);
        A_Team_Layer = LayerMask.NameToLayer("A_TEAM_LAYER");
        B_Team_Layer = LayerMask.NameToLayer("B_TEAM_LAYER");

        if (TeamSite_IntValue.RuntimeValue == A_Team)
        {
            this.gameObject.tag = "A_Team";
            this.Team_State = A_Team;
        }
        else if (TeamSite_IntValue.RuntimeValue == B_Team)
        {
            this.gameObject.tag = "B_Team";
            this.Team_State = B_Team;
        }
        //temp As applied
        AttackWait = AttackSpeed;
        AttackDelaySeconds = AttackSpeed;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        if (Check_ASite_Scene_Gladiators.RuntimeValue != 0) // another Place
        {
            if(!RenewalPosition)
            {
                Vector3 temp;
                
                switch(RePosition.RuntimeValue)
                {
                    case 0:
                        temp.x = -15f;
                        temp.y = -7f;
                        temp.z = 0;
                        this.transform.position = temp;
                        break;
                    case 1:
                        temp.x = -15f;
                        temp.y = -4f;
                        temp.z = 0;
                        this.transform.position = temp;
                        break;
                    default:
                        break;
                }

                checkWinLost = false;
                Skill_1_OnOff = true;
                Skill_3_OnOff = true;
                canOrgeAttack = true;
            }
            //Orge_Class_Update();
            RenewalPosition = true;
        }
        else
        {
            if (RenewalPosition)
            {
                Vector3 temp;

                switch (RePosition.RuntimeValue)
                {
                    case 0:
                        temp.x = 10f;
                        temp.y = -4f;
                        temp.z = 0;
                        this.transform.position = temp;
                        break;
                    case 1:
                        temp.x = -15f;
                        temp.y = -4f;
                        temp.z = 0;
                        this.transform.position = temp;
                        break;
                    default:
                        break;
                }
                checkWinLost = false;
                OrgeAnim.SetBool("Win", false);

                Skill_1_OnOff = true;
                Skill_3_OnOff = true;
                canOrgeAttack = true;
            }
            RenewalPosition = false;
            base.Update();

            if (Input.GetMouseButtonDown(0) && Check_ASite_Scene_Gladiators.RuntimeValue == 0)
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = transform.position.z;

                var pos1 = transform.position.x - CheckDistance_;
                var pos2 = transform.position.x + CheckDistance_;
                var pos3 = transform.position.y - CheckDistance_;
                var pos4 = transform.position.y + CheckDistance_;

                if (pos.x >= pos1 && pos.x <= pos2 && pos.y <= pos4 && pos.y >= pos3)
                {
                    if (this.sceneState == E_SceneState_New.idle)
                    {
                        OpenCharacterPanel();
                    }
                }
                if (this.sceneState == E_SceneState_New.pass)
                {
                    StartCoroutine(LoadScenePass());
                }
                else if (this.sceneState == E_SceneState_New.fail)
                {
                    StartCoroutine(LoadSceneFail());
                }
            }
        }
    }

    private void Orge_Class_Update()
    {
        //AttackWait = AttackSpeed;
        TransformFunc();
        
        if (checkWinLost && testTarget == null)
        {
            OrgeAnim.SetBool("Win", true);
        }

        if (testTarget != null)
        {
            CheckDistance(testTarget);
            checkWinLost = true;
        }
        else
        {

        }

        //if (tookDamage)
        //{
        //    changeAnimAttackDirection(temp_ - transform.position);
        //    tookDamage = false;
        //}
    }

    private void Orge_Class_Start()
    {
        moveSpeed = InitmoveSpeed.RuntimeValue;
        health = maxHealth.RuntimeValue;
        baseAttack = DamageIntValue.RuntimeValue;
        Level = Level_IntValue.RuntimeValue;
        ProjectileSpeed_base = ProjectileSpeed.RuntimeValue;
        gladiatorState = GladiatorState.idle;
        AttackSpeed = WeaponSpeed.RuntimeValue;
        OrgeAnim = GetComponent<Animator>();
        Alive_BoolValue.RuntimeValue = true;

        OrgeAnim.SetBool("Win", false);
        OrgeAnim.SetFloat("moveX", 0);
        OrgeAnim.SetFloat("moveY", -1);
        healthBar.SetMaxHealth(health);
        A_Team_Layer = LayerMask.NameToLayer("A_TEAM_LAYER");
        B_Team_Layer = LayerMask.NameToLayer("B_TEAM_LAYER");

        if (TeamSite_IntValue.RuntimeValue == A_Team)
        {
            this.gameObject.tag = "A_Team";
            //this.gameObject.layer = A_Team_Layer;
            this.Team_State = A_Team;
        }
        else if (TeamSite_IntValue.RuntimeValue == B_Team)
        {
            this.gameObject.tag = "B_Team";
            //this.gameObject.layer = B_Team_Layer;
            this.Team_State = B_Team;
        }

        //temp As applied
        AttackWait = AttackSpeed;
    }
    private void TransformFunc()
    {
        if (TeamSite_IntValue.RuntimeValue == A_Team)
        {
            if (GameObject.FindGameObjectWithTag("B_Team"))
            {
                testTarget = GameObject.FindGameObjectWithTag("B_Team").GetComponent<Transform>();
            }
        }
        else if (TeamSite_IntValue.RuntimeValue == B_Team)
        {
            if (GameObject.FindGameObjectWithTag("A_Team"))
            {
                testTarget = GameObject.FindGameObjectWithTag("A_Team").GetComponent<Transform>();
            }
        }
    }

    public virtual void CheckDistance(Transform targetArray)
    {
        var tt = Vector3.Distance(targetArray.position, transform.position);
        if ( tt <= chaseRadius && tt > attackRadius)
        {
            if (gladiatorState == GladiatorState.idle || gladiatorState == GladiatorState.walk)
            {
                {
                    Vector3 temp = Vector3.MoveTowards(transform.position,
                        targetArray.position,
                        moveSpeed * Time.deltaTime);

                    changeAnim(temp - transform.position);
                    myRigidbody.MovePosition(temp);

                    ChangeState(GladiatorState.walk);
                }
                if(Vector3.Distance(targetArray.position, transform.position) <= 2.5f)
                {
                    if (Skill_3_OnOff && ActiveSkillList[2].RuntimeValue)
                    {
                        StartCoroutine(Skill_3_Swoop(targetArray));
                    }
                }
            }
        }
        else if (tt <= chaseRadius && tt <= attackRadius)
        {
            if (gladiatorState == GladiatorState.walk || gladiatorState == GladiatorState.idle)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position,
                               targetArray.position,
                               moveSpeed * Time.deltaTime);
                
                changeAnim(temp - transform.position);

                if (AttackDelaySeconds <= 0)
                {
                    Skill_2_Chance = Random.Range(0, 9);
                    canOrgeAttack = true;
                    AttackDelaySeconds = AttackWait;
                }

                if (canOrgeAttack == false)
                {
                    AttackDelaySeconds -= Time.deltaTime;
                }
                else
                {
                    if (Skill_3_OnOff && ActiveSkillList[2].RuntimeValue)
                    {
                        StartCoroutine(Skill_3_Swoop(targetArray));
                    }

                    if (Skill_1_OnOff && ActiveSkillList[0].RuntimeValue)
                    {
                        baseAttack = DamageIntValue.RuntimeValue * 2;
                        StartCoroutine(Skill_1_Strike());
                    }
                    else
                    {
                        if(DoubleChance >= 8 && ActiveSkillList[1].RuntimeValue) 
                        {
                            StartCoroutine(Skill_2_DoubleAttack());
                        }
                        else
                        {
                            StartCoroutine(AttackCo());
                        }
                    }
                    
                    if (tookDamage)
                    {
                        changeAnimAttackDirection(temp - transform.position);
                    }

                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        for (int i = 0; i < attackPoint.Length; i++)
        {
            Gizmos.DrawWireSphere(attackPoint[i].position, attackRange);
        }
    }

    private IEnumerator AttackCo()
    {
        gladiatorState = GladiatorState.attack;
        OrgeAnim.SetBool("moving", false);
        OrgeAnim.SetBool("attacking", true);
        tookDamage = true;
        canOrgeAttack = false;
        yield return new WaitForSeconds(AttackWait * 0.66f);
        //yield return null;

        gladiatorState = GladiatorState.idle;
        OrgeAnim.SetBool("attacking", false);
        OrgeAnim.SetBool("moving", true);
        yield return new WaitForSeconds(AttackWait * 0.66f);
    }

    private IEnumerator Skill_1_Strike()
    {
        gladiatorState = GladiatorState.attack;
        OrgeAnim.SetBool("Skill_1_Strike", true);
        tookDamage = true;
        canOrgeAttack = false;
        yield return new WaitForSeconds(AttackWait * 0.66f);
        //yield return null;

        Skill_1_OnOff = false;
        attackRange = attackRangeConst;
        baseAttack = DamageIntValue.RuntimeValue;

        gladiatorState = GladiatorState.idle;
        OrgeAnim.SetBool("Skill_1_Strike", false);
        yield return new WaitForSeconds(AttackWait * 0.66f);

    }

    private IEnumerator Skill_2_DoubleAttack()
    {
        gladiatorState = GladiatorState.attack;
        OrgeAnim.SetBool("Skill_2_DoubleAttack", true);
        tookDamage = true;
        yield return new WaitForSeconds(AttackWait * 0.33f);

        OrgeAnim.SetBool("Skill_2_DoubleAttack", false);
        yield return new WaitForSeconds(AttackWait * 0.33f);

        OrgeAnim.SetBool("Skill_2_DoubleAttack", true);
        yield return new WaitForSeconds(AttackWait * 0.33f);

        gladiatorState = GladiatorState.idle;
        canOrgeAttack = false;
        OrgeAnim.SetBool("Skill_2_DoubleAttack", false);
        yield return new WaitForSeconds(AttackWait * 0.33f);
    }

    private IEnumerator Skill_3_Swoop(Transform targetArray)
    {
        OrgeAnim.SetBool("Skill_3_Swoop", true);

        Vector3 temp = targetArray.position;
        temp.x += 1.3f;
        yield return new WaitForSeconds(0.1f);

        Skill_3_OnOff = false;

        gladiatorState = GladiatorState.idle;
        OrgeAnim.SetBool("Skill_3_Swoop", false);

        GladiatorPositionRenewal(temp);
        yield return new WaitForSeconds(0.1f);
    }


    public void changeAnimAttackDirection(Vector2 direction)
    {
        int tm = enemyLayers;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                OrgeDamageLayer(attackPoint[0].position);
            }
            else
            {
                OrgeDamageLayer(attackPoint[1].position);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                OrgeDamageLayer(attackPoint[2].position);
            }
            else
            {
                OrgeDamageLayer(attackPoint[3].position);
            }
        }
    }

    private void OrgeDamageLayer(Vector2 this_AttackPoint)
    {
        int inflictChance = Random.Range(0, 9);
        if (Team_State == A_Team)
        {
            Collider2D[] hitOrge = Physics2D.OverlapCircleAll(this_AttackPoint, attackRange, Orge_MASK);
            foreach (Collider2D enemy in hitOrge)
            {
                enemy.GetComponent<NewGladiator>().TakeDamage_Bteam(baseAttack, B_Team, inflictChance);
            }

            Collider2D[] hitLog = Physics2D.OverlapCircleAll(this_AttackPoint, attackRange, Log_MASK);
            foreach (Collider2D enemy in hitLog)
            {
                enemy.GetComponent<Log>().TakeDamage(baseAttack, B_Team, inflictChance);
            }
            Collider2D[] hitLog_A = Physics2D.OverlapCircleAll(this_AttackPoint, attackRange, Log_A_MASK);
            foreach (Collider2D enemy in hitLog_A)
            {
                enemy.GetComponent<EnemyAI>().TakeDamage(baseAttack, B_Team, inflictChance);
            }

            Collider2D[] hitHuman = Physics2D.OverlapCircleAll(this_AttackPoint, attackRange, Human_MASK);
            foreach(Collider2D enemy in hitHuman)
            {
                enemy.GetComponent<Human>().TakeDamage(baseAttack, B_Team, inflictChance);
            }
        }
        else if (Team_State == B_Team)
        {
            Collider2D[] hitLog = Physics2D.OverlapCircleAll(this_AttackPoint, attackRange, Log_MASK);
            foreach (Collider2D enemy in hitLog)
            {
                enemy.GetComponent<Log>().TakeDamage(baseAttack, A_Team, inflictChance);
            }
        }
    }

    public void ChangeState(GladiatorState newState)
    {
        if (gladiatorState != newState)
        {
            gladiatorState = newState;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        DamagePopupOpen(damage);
        // Play hurt animation

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void TakeDamage_Ateam(int damage, int this_team, int dodge)
    {
        if (this_team == this.Team_State)
        {
            if (DodgeChance <= dodge)
            {
                health -= damage;
                healthBar.SetHealth(health);
                DamagePopupOpen(damage);

                // Play hurt animation and KnockBack
                StartCoroutine(TakeKnock());

                if (health <= 0)
                {
                    Die();
                }
            }
            else
            {
                DodgePopupOpen();
            }
        }
    }

    public virtual void TakeDamage_Bteam(int damage, int this_team, int dodge)
    {
        if (this_team == this.Team_State)
        {
            if (DodgeChance <= dodge)
            {
                health -= damage;
                healthBar.SetHealth(health);
                DamagePopupOpen(damage);

                // Play hurt animation and KnockBack
                StartCoroutine(TakeKnock());

                if (health <= 0)
                {
                    Die();
                }
            }
            else
            {
                DodgePopupOpen();
            }
        }
    }

    private IEnumerator TakeKnock()
    {
        // Play hurt animation
        if (gladiatorState != GladiatorState.attack)
        {
            OrgeAnim.SetBool("hurting", true);
            gladiatorState = GladiatorState.stagger;
            yield return new WaitForSeconds(0.1f);

            // Play hurt animation
            OrgeAnim.SetBool("hurting", false);
            gladiatorState = GladiatorState.idle;
        }
    }

    public virtual void DamagePopupOpen(int damage)
    {
        Debug.Log("3) DamagePopupOpen Call: " + damage);
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().SelectTextType(0, damage, string.Empty);
    }

    private void DodgePopupOpen()
    {
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().SelectTextType(1, 0, "Miss");
        return;
    }

    public virtual void Die()
    {
        //Die Animation
        DeathEffect();

        Alive_BoolValue.RuntimeValue = false;
        Destroy(this.gameObject);
    }

    public virtual void DeathEffect()
    {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDelay);
        }
    }

    public void InitailizeSetting()
    {
        WriteInittime();
        moveSpeed = InitmoveSpeed.RuntimeValue;
        health = maxHealth.RuntimeValue;
        baseAttack = DamageIntValue.RuntimeValue;
        AttackSpeed = WeaponSpeed.RuntimeValue;
        ProjectileSpeed_base = ProjectileSpeed.RuntimeValue;
        Level = Level_IntValue.RuntimeValue;
        //Alive_BoolValue.RuntimeValue = true;
        InitGladiatorStat[0] = Level;
        InitGladiatorStat[1] = health;
        InitGladiatorStat[2] = moveSpeed;
        InitGladiatorStat[3] = baseAttack;

        if (GladiatorStat_Name.text == string.Empty)
        {
            int RandomList = Random.Range(0, 9);
            GladiatorStat_Name.text = gladiatorName.ToString() + GladiatorNameList[RandomList];
            gladiatorName = GladiatorStat_Name.text;
        }
    }

    public void OpenCharacterPanel()
    {
        isOpend = !isOpend;

        if (isOpend)
        {
            CharacterPanel.SetActive(true);
            GladiatorImage.SetActive(true);
            OpenTextGladiatorStat(Level);
            Time.timeScale = 0.5f;
        }
        else
        {
            CharacterPanel.SetActive(false);
            GladiatorImage.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void CloseCharacterPanel()
    {
        CharacterPanel.SetActive(false);
        GladiatorImage.SetActive(false);
        Time.timeScale = 1f;
        isOpend = false;
    }

    private void OpenTextGladiatorStat(int this_Level)
    {
        if (GladiatorStat_Name.text == string.Empty)
        {
            int RandomList = Random.Range(0, 9);
            GladiatorStat_Name.text = gladiatorName.ToString() + GladiatorNameList[RandomList];
        }
        GladiatorStat_Lv.text = Level.ToString();
        GladiatorStat_Hp.text = health.ToString();
        GladiatorStat_Speed.text = moveSpeed.ToString();
        GladiatorStat_Damage.text = baseAttack.ToString();
        if (this_Level + 1 >= MaxGladiatorLevel)
        {
            GladiatorStat_PassChance.text = "�ִ밭ȭ";
        }
        else
        {
            GladiatorStat_PassChance.text = (UpgradeList[this_Level]).ToString() + "%";
        }
        GladiatorStat_WeaponSpeed.text = AttackSpeed.ToString();
        GladiatorStat_ProjectileSpeed.text = ProjectileSpeed_base.ToString();
    }

    private void OpenTextGladiatorStatForUpgrade(int this_Level)
    {
        GladiatorStat_Lv.text = Level.ToString();
        GladiatorStat_Hp.text = health.ToString();
        GladiatorStat_Speed.text = moveSpeed.ToString();
        GladiatorStat_Damage.text = baseAttack.ToString();
        if (this_Level + 1 >= MaxGladiatorLevel)
        {
            GladiatorStat_PassChance.text = "�ִ밭ȭ";
        }
        else
        {
            GladiatorStat_PassChance.text = (UpgradeList[this_Level]).ToString() + "%";
        }
        GladiatorStat_WeaponSpeed.text = AttackSpeed.ToString();
        GladiatorStat_ProjectileSpeed.text = ProjectileSpeed_base.ToString();
    }

    public void UpgradeGladiator()
    {
        UpgradePassOrFail(Level);
    }

    protected void UpgradePassOrFail(int level)
    {
        switch (level)
        {
            case 0:
                UpgradeStat();
                OpenTextGladiatorStatForUpgrade(Level);
                break;
            case 1:
                UpgradeStat();
                OpenTextGladiatorStatForUpgrade(Level);
                break;
            case 2:
                UpgradeStat();
                OpenTextGladiatorStatForUpgrade(Level);
                break;
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
                CloseCharacterPanel();
                Debug.Log("(1) why\n");
                UpgradeResult = Random.Range(UpgradeMin, UpgradeMax);
                //2) Pass or Fail Scene Load
                ProbabilitySceneLoad(UpgradeResult, Level);
                break;
            default:
                //��ȭ ��ư �� Ȱ��ȭ
                break;
        }
    }

    private void UpgradeStat()
    {
        WriteLevelUp();
        WriteRuntime();
    }
    private void DowngradeStat()
    {
        WriteLevelDown();
        WriteRuntime();
    }

    public void UpgradeTest()
    {
        Debug.Log("��10��");
        for (int i = 0; i < 10; i++)
        {
            WriteLevelUp();
        }

        WriteRuntime();
    }

    public void UpgradeInitialize()
    {
        Debug.Log("��1");

        WriteInittime();
        WriteRuntime();
        OpenTextGladiatorStatForUpgrade(Level);
    }

    private void WriteLevelUp()
    {
        Level_IntValue.RuntimeValue += 1;
        maxHealth.RuntimeValue += 2;
        InitmoveSpeed.RuntimeValue += 0.1f;
        DamageIntValue.RuntimeValue += 1;
        WeaponSpeed.RuntimeValue += 0.1f;
        ProjectileSpeed.RuntimeValue += 0.08f;
    }

    private void WriteLevelDown()
    {
        Level_IntValue.RuntimeValue -= 1;
        maxHealth.RuntimeValue -= 2;
        InitmoveSpeed.RuntimeValue -= 0.1f;
        DamageIntValue.RuntimeValue -= 1;
        WeaponSpeed.RuntimeValue -= 0.1f;
        ProjectileSpeed.RuntimeValue -= 0.08f;
    }

    private void WriteInittime()
    {
        maxHealth.RuntimeValue = maxHealth.initialValue;
        InitmoveSpeed.RuntimeValue = InitmoveSpeed.initialValue;
        DamageIntValue.RuntimeValue = DamageIntValue.initialValue;
        ProjectileSpeed.RuntimeValue = ProjectileSpeed.initialValue;
        WeaponSpeed.RuntimeValue = WeaponSpeed.initialValue;
        Level_IntValue.RuntimeValue = Level_IntValue.initialValue;
    }
    private void WriteRuntime()
    {
        health = maxHealth.RuntimeValue;
        moveSpeed = InitmoveSpeed.RuntimeValue;
        baseAttack = DamageIntValue.RuntimeValue;
        ProjectileSpeed_base = ProjectileSpeed.RuntimeValue;
        AttackSpeed = WeaponSpeed.RuntimeValue;
        Level = Level_IntValue.RuntimeValue;
    }

    protected void ProbabilitySceneLoad(float this_Pb, int this_Level)
    {
        Debug.Log("(2) why\n");
        if (this_Pb <= UpgradeList[this_Level])
        {
            Debug.Log("(3) why\n");
            UpgradeStat();
            UpgradeLevel_IntValue.RuntimeValue = Level;
            Debug.Log("��ȭ���� :" + this_Pb);

            PassUpgradeSceneload();
            OpenTextGladiatorStatForUpgrade(Level);
        }
        else
        {
            Debug.Log("(4) why\n");
            if (this_Level > 3)
            {
                DowngradeStat();
            }
            Debug.Log("��ȭ���� :" + UpgradeResult);
            UpgradeLevel_IntValue.RuntimeValue = Level;
            FailUpgradeSceneLoad();
            OpenTextGladiatorStatForUpgrade(Level);
        }
    }
    protected void PassUpgradeSceneload()
    {
        Debug.Log("(5) why\n");
        //load Scene
        //StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 2));
        StartCoroutine(LoadScenePassFailLoop());

        //sceneState Changed
        this.sceneState = E_SceneState_New.pass;
        Touch_BoolValue.RuntimeValue = false;
    }

    protected void FailUpgradeSceneLoad()
    {
        Debug.Log("(6) why\n");
        //Fail Scene Load
        StartCoroutine(LoadScenePassFailLoop());

        //sceneState Changed
        this.sceneState = E_SceneState_New.fail;
        Touch_BoolValue.RuntimeValue = false;
    }

    IEnumerator LoadScenePassFailLoop()
    {
        //play animation
        //if transition Start_PassLoop is Loop condition
        transition.SetTrigger("Start_PassFailLoop");

        //wait
        yield return new WaitForSeconds(1f);
    }

    IEnumerator LoadScenePass()
    {
        //Scene Condtion and Text Update
        sceneState = E_SceneState_New.idle;
        Touch_BoolValue.RuntimeValue = true;

        PassFailText.text = "+" + Level.ToString() + " ��ȭ ����";

        //play animation
        transition.SetTrigger("Start_Pass");
        //wait
        yield return new WaitForSeconds(3f);

        //Reload -> Idle Set but this is so bug..
        transition.SetTrigger("Idle");
    }

    IEnumerator LoadSceneFail()
    {
        //Scene Condtion and Text Update
        sceneState = E_SceneState_New.idle;
        Touch_BoolValue.RuntimeValue = true;
        PassFailText.text = "��ȭ ����";

        //play animation
        transition.SetTrigger("Start_Fail");
        //wait
        yield return new WaitForSeconds(3f);

        //Reload -> Idle Set but this is so bug..
        transition.SetTrigger("Idle");
    }

    public void OnDisable()
    {
        //Alive_BoolValue.RuntimeValue = false;
        Check_ASite_Scene_Gladiators.RuntimeValue = 0;
        Touch_BoolValue.RuntimeValue = true;
        Debug.Log("OnDisable()\n");
    }

    public void OnMyDestroy()
    {
        {
            Debug.Log("OnMyDestroy() Call");
            Init_ScriptableOjbect();
            Destroy(this.gameObject);
        }
    }

    private void Init_ScriptableOjbect()
    {
        InitmoveSpeed.RuntimeValue = InitmoveSpeed.initialValue;
        maxHealth.RuntimeValue = maxHealth.initialValue;
        DamageIntValue.RuntimeValue = DamageIntValue.initialValue;
        WeaponSpeed.RuntimeValue = WeaponSpeed.initialValue;
        ProjectileSpeed.RuntimeValue = ProjectileSpeed.initialValue;
        Level_IntValue.RuntimeValue = Level_IntValue.initialValue;
    }

    public void GladiatorPositionRenewal(Vector3 NewPos)
    {
        Debug.Log("Call This Object?\n");
        this.transform.position = NewPos;
    }
}
