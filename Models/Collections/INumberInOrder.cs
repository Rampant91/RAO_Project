﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Collections
{
    public interface INumberInOrder
    {
        long Order { get; }
        void SetOrder(long order);
    }
}