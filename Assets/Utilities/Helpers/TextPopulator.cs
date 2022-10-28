using System;
using System.Collections.Generic;
using UnityEngine;

namespace Racer.Utilities
{
    [DefaultExecutionOrder(-10)]
    public class TextPopulator : SingletonPattern.StaticInstance<TextPopulator>
    {
        private int _startIndex;

        [SerializeField] private TextAsset textFile;

        [SerializeField] private string[] allTexts;

        [field: Space(10)]

        [field: SerializeField] public List<string> CheerUpTexts { get; private set; }
        [field: SerializeField] public List<string> GuideTexts{ get; private set; }
        [field: SerializeField] public List<string> TipTexts{ get; private set; }


        [ContextMenu(nameof(Initialize))]
        private void Initialize()
        {
            allTexts = null;

            if (textFile)
                allTexts = textFile.text
                    .Split(new[] { Environment.NewLine, ":" }, StringSplitOptions.None);
            else
                allTexts = null;
        }

        [ContextMenu(nameof(PopulateCheerUpTexts))]
        private void PopulateCheerUpTexts()
        {
            CheerUpTexts.Clear();

            foreach (var t in allTexts)
            {
                if (t.StartsWith("#"))
                    continue;

                if (string.IsNullOrEmpty(t))
                    break;

                CheerUpTexts.Add(t);
            }
        }

        [ContextMenu(nameof(PopulateGuideTexts))]
        private void PopulateGuideTexts()
        {
            GuideTexts.Clear();

            _startIndex = 0;
            _startIndex = GetIndexOf("#2");

            for (var i = _startIndex; i < allTexts.Length; i++)
            {
                var t = allTexts[i];

                if (t.StartsWith("#"))
                    continue;

                if (string.IsNullOrEmpty(t))
                    break;

                GuideTexts.Add(t);
            }
        }

        
        [ContextMenu(nameof(PopulateTipTexts))]
        private void PopulateTipTexts()
        {
            TipTexts.Clear();

            _startIndex = 0;
            _startIndex = GetIndexOf("#3");

            for (var i = _startIndex; i < allTexts.Length; i++)
            {
                var t = allTexts[i];

                if (t.StartsWith("#"))
                    continue;

                if (string.IsNullOrEmpty(t))
                    break;

                TipTexts.Add(t);
            }
        }
        private int GetIndexOf(string value)
        {
            var index = 0;

            foreach (var t in allTexts)
            {
                index++;

                if (string.IsNullOrEmpty(t))
                    continue;

                if (t.Contains(value))
                    break;
            }

            return index;
        }
    }
}
