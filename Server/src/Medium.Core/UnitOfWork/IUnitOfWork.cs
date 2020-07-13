﻿using Medium.Core.Repositories;
using System.Threading.Tasks;

namespace Medium.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAuthorRepository Authors { get; }
        IPostRepository Posts { get; }
        Task<int> Commit();
    }
}
