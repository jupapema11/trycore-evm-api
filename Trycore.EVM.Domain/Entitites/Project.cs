using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Trycore.EVM.Domain.Entitites
{
    public class Project
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Activity> Activities { get; set; } = new();
    }
}
