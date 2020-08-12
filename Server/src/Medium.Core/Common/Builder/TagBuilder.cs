using System;

namespace Medium.Core.Common.Builder
{
    public class TagBuilder
    {
        private readonly Tag _tag;

        public TagBuilder()
        {
            _tag = new Tag();
        }

        public TagBuilder WithId(Guid id)
        {
            _tag.Id = id;
            return this;
        }
        public TagBuilder WithName(string name)
        {
            _tag.Name = name;
            return this;
        }
        public Tag Build()
        {
            return _tag;
        }
    }
}
