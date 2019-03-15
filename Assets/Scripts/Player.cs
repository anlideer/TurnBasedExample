using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int hp = 100;
    public int mp = 50;
    public GameObject PlayerPanel;
    public GameObject Enemy;    //  先这么将就着吧...我寻思人多了可以单独搞个脚本管理一下(

    public Text HPText;
    public Text MPText;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
        mp = 50;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowHPandMP();

    }

    public void ShowPanel()
    {
        PlayerPanel.SetActive(true);
    }

    public void HidePanel()
    {
        PlayerPanel.SetActive(false);
    }

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

    /************************** Button Functions ***************************/
    public void Attack()
    {
        if (!TurnController.isOver && !TurnController.turn)
        {
            TurnController.turn = true;
            // 攻击不需要消耗任何东西，所以不需要判断什么
            anim.Play("PlayerAttack");  // 这动画做的跟亲嘴似的...
            Debug.Log("Player: Attack");
            // 攻击一下
            Enemy.GetComponent<Boss>().DoDamage(10);
        }
    }

    public void MagicAttack()
    {
        if (!TurnController.isOver && !TurnController.turn)
        {
            // 蓝要够才行吼
            if (mp >= 40)
            {
                mp -= 40;
                TurnController.turn = true;
                anim.Play("PlayerMagic");
                Debug.Log("Player: Magic Attack");
                // 大招攻击
                Enemy.GetComponent<Boss>().DoDamage(30);
            }
            else
            {
                Debug.Log("Player: MP not enough");
            }
        }
    }

    public void Recover()
    {
        if (!TurnController.isOver && !TurnController.turn)
        {
            // 蓝要够，还有血量不能超100
            if (mp >= 20)
            {
                mp -= 20;
                TurnController.turn = true;
                anim.Play("PlayerRecover");
                Debug.Log("Player: Recover");
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
                Debug.Log("Player: MP not enough");
            }
        }
    }
}
