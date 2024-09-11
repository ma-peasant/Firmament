﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Firmament.Module
{
   public class BaseElement : Image, IBaseElement
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double XSpeed { get; set; }
        public double YSpeed { get; set; }
        public virtual bool HitState { get; set; }
        public int Flag { get; set; }
    }
}
