using System;
using Medium.Core.Common.Extension;
using Medium.Core.Domain;

public class Tag : BaseEntity
{
    public string Name { get; set; }
    public Tag()
    {
        CreatedAt = DateTime.Now.DefaultFormat();
        UpdatedAt = DateTime.Now.DefaultFormat();
    }
}