/*
 * This file is part of alphaTab.
 * Copyright (c) 2014, Daniel Kuschny and Contributors, All rights reserved.
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or at your option any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library.
 */
using System;
using System.Runtime.CompilerServices;
using AlphaTab.Collections;

namespace AlphaTab.Model
{
    /// <summary>
    /// A note is a single played sound on a fretted instrument. 
    /// It consists of a fret offset and a string on which the note is played on.
    /// It also can be modified by a lot of different effects.  
    /// </summary>
    public class Note
    {
        [IntrinsicProperty]
        public AccentuationType Accentuated { get; set; }
        [IntrinsicProperty]
        public FastList<BendPoint> BendPoints { get; set; }
        public bool HasBend { get { return BendPoints.Count > 0; } }

        [IntrinsicProperty]
        public int Fret { get; set; }
        [IntrinsicProperty]
        public int String { get; set; }

        [IntrinsicProperty]
        public bool IsHammerPullOrigin { get; set; }
        [IntrinsicProperty]
        public Note HammerPullOrigin { get; set; }
        [IntrinsicProperty]
        public Note HammerPullDestination { get; set; }

        [IntrinsicProperty]
        public float HarmonicValue { get; set; }
        [IntrinsicProperty]
        public HarmonicType HarmonicType { get; set; }

        [IntrinsicProperty]
        public bool IsGhost { get; set; }
        [IntrinsicProperty]
        public bool IsLetRing { get; set; }
        [IntrinsicProperty]
        public bool IsPalmMute { get; set; }
        [IntrinsicProperty]
        public bool IsDead { get; set; }
        [IntrinsicProperty]
        public bool IsStaccato { get; set; }

        [IntrinsicProperty]
        public SlideType SlideType { get; set; }
        [IntrinsicProperty]
        public Note SlideTarget { get; set; }

        [IntrinsicProperty]
        public VibratoType Vibrato { get; set; }

        [IntrinsicProperty]
        public Note TieOrigin { get; set; }
        [IntrinsicProperty]
        public Note TieDestination { get; set; }
        [IntrinsicProperty]
        public bool IsTieDestination { get; set; }
        [IntrinsicProperty]
        public bool IsTieOrigin { get; set; }

        [IntrinsicProperty]
        public Fingers LeftHandFinger { get; set; }
        [IntrinsicProperty]
        public Fingers RightHandFinger { get; set; }
        [IntrinsicProperty]
        public bool IsFingering { get; set; }

        [IntrinsicProperty]
        public int TrillValue { get; set; }
        public int TrillFret
        {
            get
            {
                return TrillValue - StringTuning;
            }
        }

        public bool IsTrill
        {
            get
            {
                return TrillValue >= 0;
            }
        }
        [IntrinsicProperty]
        public Duration TrillSpeed { get; set; }

        [IntrinsicProperty]
        public double DurationPercent { get; set; }

        [IntrinsicProperty]
        public bool SwapAccidentals { get; set; }

        [IntrinsicProperty]
        public Beat Beat { get; set; }
        [IntrinsicProperty]
        public DynamicValue Dynamic { get; set; }

        [IntrinsicProperty]
        public int Octave { get; set; }
        [IntrinsicProperty]
        public int Tone { get; set; }

        public int StringTuning
        {
            get
            {
                if (Beat.Voice.Bar.Track.Tuning.Count > 0)
                    return Beat.Voice.Bar.Track.Tuning[Beat.Voice.Bar.Track.Tuning.Count - (String - 1) - 1];
                return 0;
            }
        }

        public int RealValue
        {
            get
            {
                if (Fret == -1)
                    return Octave * 12 + Tone;
                return Fret + StringTuning;
            }
        }

        public Note()
        {
            BendPoints = new FastList<BendPoint>();
            Dynamic = DynamicValue.F;

            Accentuated = AccentuationType.None;
            Fret = -1;
            HarmonicType = HarmonicType.None;
            SlideType = SlideType.None;
            Vibrato = VibratoType.None;

            LeftHandFinger = Fingers.NoOrDead;
            RightHandFinger = Fingers.NoOrDead;

            TrillValue = -1;
            TrillSpeed = Duration.ThirtySecond;
            DurationPercent = 1;
            Octave = -1;
        }

        public static void CopyTo(Note src, Note dst)
        {
            dst.Accentuated = src.Accentuated;
            dst.Fret = src.Fret;
            dst.String = src.String;
            dst.IsHammerPullOrigin = src.IsHammerPullOrigin;
            dst.HarmonicValue = src.HarmonicValue;
            dst.HarmonicType = src.HarmonicType;
            dst.IsGhost = src.IsGhost;
            dst.IsLetRing = src.IsLetRing;
            dst.IsPalmMute = src.IsPalmMute;
            dst.IsDead = src.IsDead;
            dst.IsStaccato = src.IsStaccato;
            dst.SlideType = src.SlideType;
            dst.Vibrato = src.Vibrato;
            dst.IsTieDestination = src.IsTieDestination;
            dst.LeftHandFinger = src.LeftHandFinger;
            dst.RightHandFinger = src.RightHandFinger;
            dst.IsFingering = src.IsFingering;
            dst.TrillValue = src.TrillValue;
            dst.TrillSpeed = src.TrillSpeed;
            dst.DurationPercent = src.DurationPercent;
            dst.SwapAccidentals = src.SwapAccidentals;
            dst.Dynamic = src.Dynamic;
            dst.Octave = src.Octave;
            dst.Tone = src.Tone;
        }

        public Note Clone()
        {
            var n = new Note();
            CopyTo(this, n);
            for (int i = 0, j = BendPoints.Count; i < j; i++)
            {
                n.BendPoints.Add(BendPoints[i].Clone());
            }
            return n;
        }

        public void Finish()
        {
            var nextNoteOnLine = new Lazy<Note>(() => NextNoteOnSameLine(this));
            var prevNoteOnLine = new Lazy<Note>(() => PreviousNoteOnSameLine(this));

            // connect ties
            if (IsTieDestination)
            {
                if (prevNoteOnLine.Value == null)
                {
                    IsTieDestination = false;
                }
                else
                {
                    TieOrigin = prevNoteOnLine.Value;
                    TieOrigin.IsTieOrigin = true;
                    TieOrigin.TieDestination = this;
                    Fret = TieOrigin.Fret;
                }
            }

            // set hammeron/pulloffs
            if (IsHammerPullOrigin)
            {
                if (nextNoteOnLine.Value == null)
                {
                    IsHammerPullOrigin = false;
                }
                else
                {
                    HammerPullDestination = nextNoteOnLine.Value;
                    HammerPullDestination.HammerPullOrigin = this;
                }
            }

            // set slides
            if (SlideType != SlideType.None)
            {
                SlideTarget = nextNoteOnLine.Value;
            }
        }

        private const int MaxOffsetForSameLineSearch = 3;
        private static Note NextNoteOnSameLine(Note note)
        {
            var nextBeat = note.Beat.NextBeat;
            // keep searching in same bar
            while (nextBeat != null && nextBeat.Voice.Bar.Index <= note.Beat.Voice.Bar.Index + MaxOffsetForSameLineSearch)
            {
                var noteOnString = nextBeat.GetNoteOnString(note.String);
                if (noteOnString != null)
                {
                    return noteOnString;
                }
                else
                {
                    nextBeat = nextBeat.NextBeat;
                }
            }

            return null;
        }

        private static Note PreviousNoteOnSameLine(Note note)
        {
            var previousBeat = note.Beat.PreviousBeat;

            // keep searching in same bar
            while (previousBeat != null && previousBeat.Voice.Bar.Index >= note.Beat.Voice.Bar.Index - MaxOffsetForSameLineSearch)
            {
                var noteOnString = previousBeat.GetNoteOnString(note.String);
                if (noteOnString != null)
                {
                    return noteOnString;
                }
                else
                {
                    previousBeat = previousBeat.PreviousBeat;
                }
            }

            return null;
        }

    }
}