using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Utils.Random
{
    public class RandomHelper
    {
        #region 随机昵称
        static readonly string[] names = new string[] {
                 "早茶月光",

                "、凭凑不齐",

                "nI、唯一",

                "爱与你同在",

                "傻的天真、",

                "季节温暖眼瞳。",

                "、 荼靡",

                "丶失恋的感觉",

                "゛抹去最后一丝自尊",

                "失心疯,怎么了",

                "半夏微凉。",

                "伱在我心里╭ァ",

                "潮流凯子钓裸女；",

                "偏执怪人",

                "°彼此共存°",

                "眼睛想旅行",

                "囚我心虐我身",

                "情归于尽",

                "守候的裂痕",

                "敷衍”彡",

                "爱情才是奢侈品（man）。",

                "我可以忘记你",

                "-我爱你的奋不顾身",

                "把你藏心里",

                "时光浮夸，乱了遍地流年",

                "我不愿让你一个人。",

                "随梦而飞",

                "所有的深爱都是秘密",

                "喂；丫头你是我的了",

                "人前显贵人后受罪。",

                "夏日倾情",

                "乏味的人生ソ",

                "痴人痴心终是一场梦",

                "让寂寞别走",

                "离开你并非我愿意",

                "回不去的时光",

                "情绪疯子.",

                "陪着烟消遣",

                "繁华年间〃谁许我一生|",

                "睡衣男孩",
                "你要懂得欺骗自己",

                "◆失心虐-Ⅱ/pz",

                "都扔了知道就好",

                "我给你的爱情难道不够、",

                "一闪一闪亮光头™",

                "MC’日月星辰",

                "子弟的化身丶",

                "你说过,我信过",

                "嗯，那又如何",
                "い遥远了清晰的爱",

                "リ丶丶灬尛坏坏＂",

                "恨你你兴奋了是吗",

                "唯我独尊",

                "男人酒女人泪",

                "抽烟ゞ只为麻痹我自己",

                "花花世界，何必当真",

                "中毒的爱情",

                "相濡以沫",

                "带着春心找荡漾@",

                "梦想的天空格外蓝",

                "爱原本应该能和被爱对等",

                "❀痞子时代",

                "风的季节^^",

                "这一刻，Love吧！",

                "挥别错的，才能和对的相逢",

                "胖子就是矯情"
 };
        #endregion

        #region 获取随机昵称
        public static string getRandomName()
        {
            var len = names.Length;
            System.Random random = new System.Random();
            var next = random.Next(0, len - 1);
            return names[next];

        }
        #endregion

        #region 生成用户登录token
        static readonly string[] alpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        static  System.Random r = new System.Random();
        public static string GetUserToken()
        {

            StringBuilder str = new StringBuilder("");
            var len = alpha.Length;
            for (int i = 0; i < 64; i++)
            {
                str.Append(alpha[r.Next(0, len - 1)]);
            }
            return str.ToString();
        }
        #endregion
    }
}
