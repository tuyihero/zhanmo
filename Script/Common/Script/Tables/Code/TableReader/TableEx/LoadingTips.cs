using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class LoadingTipsRecord : TableRecordBase
    {

    }

    public partial class LoadingTips : TableFileBase
    {

        List<string> _ImageTipKeys;
        public List<string> ImageTipKeys
        {
            get
            {
                if (_ImageTipKeys == null)
                {
                    InitTipKeys();
                }
                return _ImageTipKeys;
            }
        }

        List<string> _TipKeys;
        public List<string> TipKeys
        {
            get
            {
                if (_TipKeys == null)
                {
                    InitTipKeys();
                }
                return _TipKeys;
            }
        }

        public void InitTipKeys()
        {
            _ImageTipKeys = new List<string>();
            _TipKeys = new List<string>();

            foreach (var tipRecord in Records)
            {
                if (!string.IsNullOrEmpty(tipRecord.Value.ImagePath))
                {
                    _ImageTipKeys.Add(tipRecord.Key);
                }
                else
                {
                    _TipKeys.Add(tipRecord.Key);
                }
            }
        }

        public LoadingTipsRecord GetRandomImageTips()
        {
            int randomIdx = UnityEngine.Random.Range(0, ImageTipKeys.Count - 1);
            return GetRecord(ImageTipKeys[randomIdx]);
        }

        public LoadingTipsRecord GetRandomTextTips(int level)
        {
            List<LoadingTipsRecord> levelTips = new List<LoadingTipsRecord>();
            foreach (var tipKey in _TipKeys)
            {
                var tipRecord = GetRecord(tipKey);
                if (level >= tipRecord.MinLevel)
                {
                    levelTips.Add(tipRecord);
                }
            }

            int randomIdx = UnityEngine.Random.Range(0, levelTips.Count - 1);
            return levelTips[randomIdx];
        }
    }

}