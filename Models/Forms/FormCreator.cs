﻿using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Models.Forms;

public static class FormCreator
{
    public static Form Create(string param)
    {
        Form tmp = param switch
        {
            "1.0" => new Form10(),
            "1.1" => new Form11(),
            "1.2" => new Form12(),
            "1.3" => new Form13(),
            "1.4" => new Form14(),
            "1.5" => new Form15(),
            "1.6" => new Form16(),
            "1.7" => new Form17(),
            "1.8" => new Form18(),
            "1.9" => new Form19(),
            "2.0" => new Form20(),
            "2.1" => new Form21(),
            "2.2" => new Form22(),
            "2.3" => new Form23(),
            "2.4" => new Form24(),
            "2.5" => new Form25(),
            "2.6" => new Form26(),
            "2.7" => new Form27(),
            "2.8" => new Form28(),
            "2.9" => new Form29(),
            "2.10" => new Form210(),
            "2.11" => new Form211(),
            "2.12" => new Form212(),
            _ => null
        };
        if (tmp != null)
        {
            tmp.FormNum.Value = param;
        }
        return tmp;
    }
}