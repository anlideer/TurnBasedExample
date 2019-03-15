using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// (战斗逻辑)参考：
/// https://blog.csdn.net/qinyuanpei/article/details/28125171
/// https://blog.csdn.net/c252270036/article/details/77126777
/// AI也可以用上面某一篇用的随机......或许比较弱智就是了
/// 
/// 
/// ************** 规则 *************
/// 决策集：{普攻(-10)，大招(蓝-40)(-30)，补血(蓝-20)(红+30)} // 瞎jb数值，可能实际上应该随机一下伤害？
/// 双方血量：100
/// 双方蓝：50 每回合回复5
/// 
/// 
/// 写代码的思路：
/// 回合控制的脚本（就是这个）控制目前是谁的回合以及回合转移所要做的事情-->>>
/// 玩家的脚本，给出可选的决策，控制属性的改变，控制给玩家显示的（本来应该单开个UI脚本？...）-->>>
/// BOSS的脚本，除了不是人为控制之外全部跟Player一样（是不是下次我该先写个类让ta俩继承？...）-->>>
/// 把玩家和BOSS攻击、受伤害的逻辑搞通-->>>
/// 写BOSS的AI
/// 
/// PS:这个规则比较简单，数值也是瞎jb设的，再加上BOSS那边我又把AI写的很猥琐...导致我好像打不过它...
/// 在此规则下要降低难度的话，我觉得可以再加上个随机的机制，让BOSS无脑普攻...
/// 瞎jb乱写一通...
/// 
/// </summary>
public class TurnController : MonoBehaviour
{

    public static bool isOver = false;
    public static bool turn = false;    // false-Player, true-Boss, 要是更多角色我寻思可以用枚举之类的东西？
    public GameObject Player;
    public GameObject Boss;

    public Text TurnText;

    bool panelShown = false;
    bool panelHidden = false;
    bool MPupedP = false;
    bool MPupedB = false;

    // Start is called before the first frame update
    void Start()
    {
        isOver = false;
        panelShown = false;
        MPupedP = false;
        MPupedB = false;
        panelHidden = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOver)
        {
            // 判断是否有一边血<=0
            // 先没搞重新开始的事情(应该不难加上...注意初始化就行了...
            if (Player.GetComponent<Player>().hp <= 0)
            {
                isOver = true;
                TurnText.text = "You Lose!";    //  暂时就用这个Text了8...
            }
            else if (Boss.GetComponent<Boss>().hp <= 0)
            {
                isOver = true;
                TurnText.text = "You Win!";
            }
        }


        if(isOver)
        {
            HidePlayerPanel();
        }
        else
        {
            // Player turn
            if (!turn)
            {
                TurnText.text = "Your Turn";
                if (!MPupedP)
                {
                    MPupedP = true;
                    MPupedB = false;
                    // 双方加MP
                    MPup();
                    
                }
                if (!panelShown)
                {
                    panelShown = true;
                    panelHidden = false;    //  顺便把这个也重置
                    StartCoroutine("ShowPlayerPanel");
                }
            }
            // Boss turn
            else
            {
                TurnText.text = "Enemy Turn";
                if (!MPupedB)
                {
                    MPupedB = true;
                    MPupedP = false;
                    // 双方加MP
                    MPup();
                    
                }
                if (!panelHidden)
                {
                    panelHidden = true;
                    panelShown = false;
                    StartCoroutine("HidePlayerPanel");
                }
            }
        }
    }

    // 使用协程延迟执行，定时器也行
    IEnumerator ShowPlayerPanel()
    {
        yield return new WaitForSeconds(1);
        Player.GetComponent<Player>().ShowPanel();
    }
    IEnumerator HidePlayerPanel()
    {
        yield return new WaitForSeconds(1);
        Player.GetComponent<Player>().HidePanel();
    }


    void MPup()
    {
        int mpP, mpB;
        mpP = Player.GetComponent<Player>().mp;
        mpB = Boss.GetComponent<Boss>().mp;
        if (mpP + 5 > 50)
            mpP = 50;
        else
            mpP += 5;
        if (mpB + 5 > 50)
            mpB = 50;
        else
            mpB += 5;
        Player.GetComponent<Player>().mp = mpP;
        Boss.GetComponent<Boss>().mp = mpB;
    }
}
