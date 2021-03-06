﻿/*
 * This file is part of alphaTab.
 * Copyright © 2017, Daniel Kuschny and Contributors, All rights reserved.
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
using AlphaTab.Collections;
using AlphaTab.Model;

namespace AlphaTab.Rendering.Staves
{
    /// <summary>
    /// This container represents a single column of bar renderers independent from any staves. 
    /// This container can be used to reorganize renderers into a new staves. 
    /// </summary>
    public class MasterBarsRenderers
    {
        public float Width { get; set; }
        public bool IsLinkedToPrevious { get; set; }
        public MasterBar MasterBar { get; set; }
        public FastList<BarRendererBase> Renderers { get; set; }
        public BarLayoutingInfo LayoutingInfo { get; set; }

        public MasterBarsRenderers()
        {
            Renderers = new FastList<BarRendererBase>();
        }
    }
}