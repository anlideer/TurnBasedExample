using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    public int hp = 100;
    public int mp = 50;
    public GameObject Enemy;    //  先这么将就着吧...我寻思人多了可以单独搞个脚本管理一下(   // Enemy的Enemy就是我可还行

    public Text HPText;
    public Text MPText;

    Animator anim;

    bool thought = false;

    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
        mp = 50;
        anim = GetComponent<Animator>();
        thought = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!TurnController.isOver)
        {
            ShowHPandMP();
            // 延迟执行BOSS的决策，延迟时间跟玩家Panel消失的时间一样(1s
            // 不过话说用太多协程是不是不是很好...
            if (TurnController.turn)
            {
                if (!thought)
                {
                    thought = true;
                    StartCoroutine("Think");
                }
            }
            else
            {
                thought = false;
            }
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(1);
        TakeAction();
    }

    /**************** AI的决策 *******************/
    /**************** 人工智障 *******************/
    void TakeAction()
    {
        int hpP = Enemy.GetComponent<Player>().hp;
        // 如果可以终结对方，那么直接终结
        if (hpP <= 30 && mp >= 40)
        {
            MagicAttack();
        }
        else if (hpP <= 10)
        {
            Attack();
        }
        // 无法终结对方
        else
        {
            // 优先考虑治疗自己(好猥琐一智障
            if (hp <= 70 && mp >= 20)
            {
                Recover();
            }
            // 血也比较足，有蓝
            else if (hp > 70 && mp >= 40)
            {
                MagicAttack();
            }
            else
            {
                Attack();
            }
        }

    }

    // 下面的玩意跟Player那的基本一样
    // 要是可以不用像这样复制粘贴魔改那样复用当然更好...先这样了8，懒得搞了(
    // 我错了，下次一定写个父类，我改的要吐了

    public void DoDamage(int d)
    {
        hp -= d;
    }

    void ShowHPandMP()
    {
        int tmphp, tmpmp;
        if (hp < 0)
        {
            tmphp = 0;
        }
        else if (hp > 100)
        {
            tmphp = 100;
        }
        else
        {
            tmphp = hp;
        }

        if (mp < 0)
        {
            tmpmp = 0;
        }
        else if (mp > 50)
        {
            tmpmp = 50;
        }
        else
        {
            tmpmp = mp;
        }

        HPText.text = tmphp.ToString();
        MPText.text = tmpmp.ToString();
    }


    void Attack()
    {
        if (!TurnController.isOver && TurnController.turn)
        {
            TurnController.turn = false;
            // 攻击不需要消耗任何东西，所以不需要判断什么
            anim.Play("BossAttack");  // 这动画做的跟亲嘴似的...
            Debug.Log("Boss: Attack");
            // 攻击一下
            Enemy.GetComponent<Player>().DoDamage(10);
        }
    }

    void MagicAttack()
    {
        if (!TurnController.isOver && TurnController.turn)
        {
            // 蓝要够才行吼
            if (mp >= 40)
            {
                mp -= 40;
                TurnController.turn = false;
                anim.Play("BossMagic");
                Debug.Log("Boss: Magic Attack");
                // 大招攻击
                Enemy.GetComponent<Player>().DoDamage(30);
            }
            else
            {
                Debug.Log("Boss: MP not enough");
            }
        }
    }

    void Recover()
    {
        if (!TurnController.isOver && TurnController.turn)
        {
            // 蓝要够，还有血量不能超100
            if (mp >= 20)
            {
                mp -= 20;
                TurnController.turn = false;
                anim.Play("BossRecover");
                Debug.Log("Boss: Recover");
                if (hp + 30 > 100)
                {
                    hp = 100;
                }
                else
                {
                    hp += 30;
                }
            }
            else
            {
                Debug.Log("Boss: MP not enough");
            }
        }
    }

}
