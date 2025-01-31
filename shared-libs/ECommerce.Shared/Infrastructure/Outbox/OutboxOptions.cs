using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Infrastructure.Outbox
{
    public class OutboxOptions
    {
        public const string OutboxSectionName = "Outbox";
        public int PublishIntervalInSeconds { get; set; } = 30;
    }
}
