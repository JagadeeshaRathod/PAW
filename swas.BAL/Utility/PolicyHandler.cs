﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Utility
{
    public class CustomAuthorizationRequirement : IAuthorizationRequirement
    {
        public string CustomProperty { get; }

        public CustomAuthorizationRequirement(string customProperty)
        {
            CustomProperty = customProperty;
        }
    }

}
