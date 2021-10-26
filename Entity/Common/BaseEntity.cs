using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public interface IEntity
    {
    }

    public abstract class BaseEntity<TKey> : IEntity
    {
        public TKey ID { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int>
    {

    }
}
