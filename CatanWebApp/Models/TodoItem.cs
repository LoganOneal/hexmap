﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatanASP.Models
{ 
    public class TodoItem
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public int Priority { get; set; }
            public bool IsDone { get; set; }
        }
}
