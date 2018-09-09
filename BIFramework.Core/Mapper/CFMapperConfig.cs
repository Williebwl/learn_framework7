using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    internal class CFMapperConfig
    {
        public CFMapperConfig(Type sourceType, Type targetType)
        {
            this.SourceType = sourceType ?? typeof(object);
            this.TargetType = targetType ?? typeof(object);
        }
        public Type SourceType { get; private set; }
        public Type TargetType { get; private set; }
        public override bool Equals(object obj)
        {
            if (obj is CFMapperConfig && obj != null && this != null)
            {
                CFMapperConfig config = obj as CFMapperConfig;
                return config.SourceType == this.SourceType && config.TargetType == this.TargetType;
            }
            else
                return base.Equals(obj);
        }
    }
    internal class MapperConfig<T, TResult> : CFMapperConfig
    {
        public MapperConfig()
            : base(typeof(T), typeof(TResult))
        {
        }
    }
}
