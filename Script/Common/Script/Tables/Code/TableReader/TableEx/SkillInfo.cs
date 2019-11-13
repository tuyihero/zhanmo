using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class SkillInfoRecord : TableRecordBase
    {

    }

    public partial class SkillInfo : TableFileBase
    {

        public static string GetSkillInputByClass(SKILL_CLASS skillClass)
        {
            switch (skillClass)
            {
                case SKILL_CLASS.NORMAL_ATTACK:
                    return "j";
                case SKILL_CLASS.SKILL1:
                    return "1";
                case SKILL_CLASS.SKILL2:
                    return "2";
                case SKILL_CLASS.SKILL3:
                    return "3";
            }
            return "";
        }
    }

}