﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Exceptions
{
    public  class NotNullException:Exception
    {
        public NotNullException(string property,string? message) : base(message)
        {
            Property = property;

        }

        public string Property { get; set; }
    }
}
