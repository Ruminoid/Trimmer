﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace Ruminoid.Trimmer.Shell.Models
{

    public sealed class LrcExporter
    {

        #region Const Strings

        private static string AppendString =
            @"[Script Info]
; Generated by Ruminoid Trimmer
; https://ruminoid.vbox.moe/
Title: Karaoke Subtitle File
ScriptType: v4.00+

[V4+ Styles]
Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding
Style: Default,Arial,70,&H00FFFFFF,&H000000FF,&H00000000,&H00000000,0,0,0,0,100,100,0,0,1,2,2,2,10,10,10,1

[Events]
Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
";

        #endregion

        #region Constructor

        public LrcExporter(string path, ModeType type, Encoding encoding)
        {
            _path = string.IsNullOrEmpty(path) ? "" : path;
            _type = type;
            _encoding = encoding ?? Encoding.UTF8;
        }

        #endregion

        #region Config

        private string _path;

        private ModeType _type;

        private Encoding _encoding;

        #endregion

        #region Generator

        private StringBuilder _sb = new StringBuilder(AppendString);

        #endregion

        public void Export() => File.WriteAllText(_path, Generate(LrcModel.Current), _encoding);

        private string Generate(LrcModel model)
        {
            int i;
            for (i = 0; i < model.Items.Count - 1; i++)
                GenerateLine(model.Items[i], model.Items[i + 1].Items.FirstOrDefault()?.Position);
            GenerateLine(model.Items[i], null);
            return _sb.ToString();
        }

        private void GenerateLine(LrcLine line, Position endStamp)
        {
            if (line.Items.Count <= 0) return;
            int i;
            StringBuilder sc = new StringBuilder();
            for (i = 0; i < line.Items.Count - 1; i++)
            {
                Position position = null;
                for (int delta = 1; delta < line.Items.Count - i - 1; delta++)
                {
                    LrcChar c = line.Items[i + delta];
                    if (c.Skip || c.Position.Time <= 0) continue;
                    position = c.Position;
                    break;
                }
                if (position is null) position = new Position(line.Items[i].Position.Time + 20);
                sc.Append(GenerateChar(line.Items[i], position).generated);
            }
            (Position e, string generated) = GenerateChar(line.Items[i], endStamp);
            _sb.AppendLine(
                $"Dialogue: 0,{line.Items.FirstOrDefault()?.Position.ConvertToSubtitleTimestamp()},{e.ConvertToSubtitleTimestamp()},Default,,0,0,0,,{sc}{generated}");
        }

        private (Position endStamp, string generated) GenerateChar(LrcChar chr, Position endStamp)
        {
            Position e = endStamp ?? new Position(chr.Position.Time + 20);
            if (chr.EndLine) return (chr.Position, "");
            return chr.Skip ? (e, chr.Char.ToString()) : (e, "{\\" + _type.Key + chr.Position.CalculateDelta(e) + "}" + chr.Char);
        }

    }

}
